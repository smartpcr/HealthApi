// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExpensesController.cs" company="Microsoft Corporation">
//   Copyright (C) Microsoft Corporation.  All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Api.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Linq.Dynamic;
    using System.Net;
    using System.Web;
    using System.Web.Http;
    using System.Web.Http.Routing;
    using Api.Helpers;
    using Health.Repository;
    using Health.Repository.Entities;
    using Health.Repository.Factories;
    using Marvin.JsonPatch;
    using Newtonsoft.Json;

    [RoutePrefix("api")]
    public class ExpensesController : ApiController
    {
        IExpenseTrackerRepository _repository;
        const int maxPageSize = 10;

        #region ctor

        public ExpensesController()
        {
            _repository = new ExpenseTrackerEFRepository(new ExpenseTrackerContext());
        }

        public ExpensesController(IExpenseTrackerRepository repository)
        {
            _repository = repository;
        }

        #endregion

        [Route("expensegroups/{expenseGroupId}/expenses", Name = "ExpensesForGroup")]
        public IHttpActionResult Get(int expenseGroupId, string fields = null, string sort = "date", int page = 1, int pageSize = maxPageSize)
        {
            try
            {
                List<string> lstOfFields = new List<string>();
                if (fields != null)
                {
                    lstOfFields = fields.ToLower().Split(',').ToList();
                }

                var expenses = _repository.GetExpenses(expenseGroupId);
                if (expenses == null)
                {
                    return NotFound();
                }

                if (pageSize > maxPageSize)
                {
                    pageSize = maxPageSize;
                }

                var totalCount = expenses.Count();
                var totalPages = (int) Math.Ceiling((double) totalCount/pageSize);
                var urlHelper = new UrlHelper(Request);
                var prevLink = page > 1
                    ? urlHelper.Link("ExpensesForGroup", new
                    {
                        page = page - 1,
                        pageSize = pageSize,
                        expenseGroupId = expenseGroupId,
                        sort = sort
                    })
                    : "";
                var nextLink = page < totalPages
                    ? urlHelper.Link("ExpensesForGroup", new
                    {
                        page = page + 1,
                        pageSize = pageSize,
                        expenseGroupId = expenseGroupId,
                        sort = sort
                    })
                    : "";
                var paginationHeader = new
                {
                    currentPage = page,
                    pageSize = pageSize,
                    totalCount = totalCount,
                    totalPages = totalPages,
                    previousPageLink = prevLink,
                    nextPageLink = nextLink
                };

                HttpContext.Current.Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(paginationHeader));

                var expenseResult = expenses.ApplySort(sort).Skip(pageSize*(page - 1)).Take(pageSize).ToList()
                    .Select(exp => exp.ToDto().Expand(lstOfFields));
                return Ok(expenseResult);
            }
            catch (Exception)
            {
                // TODO: log error 
                return InternalServerError();
            }
        }

        [Route("expensegroups/{expenseGroupId}/expenses/{id}")]
        [Route("expenses/{id}")]
        public IHttpActionResult Get(int id, int? expenseGroupId = null, string fields = null)
        {
            try
            {
                List<string> lstOfFields = new List<string>();
                if (fields != null)
                {
                    lstOfFields = fields.ToLower().Split(',').ToList();
                }

                Expense expense = null;
                if (expenseGroupId == null)
                {
                    expense = _repository.GetExpense(id);
                }
                else
                {
                    var expensesForGroup = _repository.GetExpenses((int) expenseGroupId);
                    if (expensesForGroup != null)
                    {
                        expense = expensesForGroup.FirstOrDefault(e => e.Id == id);
                    }
                }

                if (expense != null)
                {
                    var returnValue = expense.ToDto().Expand(lstOfFields);
                    return Ok(returnValue);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }

        [Route("expenses/{id}")]
        public IHttpActionResult Delete(int id)
        {
            try
            {
                var result = _repository.DeleteExpense(id);
                if (result.Status == RepositoryActionStatus.Deleted)
                {
                    return StatusCode(HttpStatusCode.NoContent);
                }
                else if (result.Status == RepositoryActionStatus.NotFound)
                {
                    return NotFound();
                }

                return BadRequest();
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }

        [Route("expenses")]
        public IHttpActionResult Post([FromBody] Health.DTO.Expense expense)
        {
            try
            {
                if (expense == null)
                {
                    return BadRequest();
                }

                var exp = expense.ToEntity();
                var result = _repository.InsertExpense(exp);
                if (result.Status == RepositoryActionStatus.Created)
                {
                    var newExp = result.Entity.ToDto();
                    return Created(Request.RequestUri + "/" + newExp.Id.ToString(), newExp);
                }

                return BadRequest();
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }

        [Route("expenses/{id}")]
        public IHttpActionResult Put(int id, [FromBody] Health.DTO.Expense expense)
        {
            try
            {
                if (expense == null)
                {
                    return BadRequest();
                }

                var exp = expense.ToEntity();
                var result = _repository.UpdateExpense(exp);

                if (result.Status == RepositoryActionStatus.Updated)
                {
                    var updatedExpense = result.Entity.ToDto();
                    return Ok(updatedExpense);
                }
                else if (result.Status == RepositoryActionStatus.NotFound)
                {
                    return NotFound();
                }

                return BadRequest();
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }

        [Route("expenses/{id}"), HttpPatch]
        public IHttpActionResult Patch(int id, [FromBody] JsonPatchDocument<Health.DTO.Expense> expensePatchDocument)
        {
            try
            {
                if (expensePatchDocument == null)
                {
                    return BadRequest();
                }

                var expense = _repository.GetExpense(id);
                if (expense == null)
                {
                    return NotFound();
                }

                var exp = expense.ToDto();
                expensePatchDocument.ApplyTo(exp);
                var result = _repository.UpdateExpense(exp.ToEntity());
                if (result.Status == RepositoryActionStatus.Updated)
                {
                    return Ok(result.Entity.ToDto());
                }

                return BadRequest();
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }
    }
}