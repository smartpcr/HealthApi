﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExpenseGroupsController.cs" company="Microsoft Corporation">
//   Copyright Microsoft Corporation, all rights reserved
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Api.Controllers
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Web;
    using System.Web.Http;
    using System.Web.Http.Routing;
    using Api.Helpers;
    using Health.Repository;
    using Health.Repository.Entities;
    using Health.Repository.Factories;
    using Marvin.JsonPatch;

    public class ExpenseGroupsController : ApiController
    {
        IExpenseTrackerRepository _repository;
        const int maxPageSize = 10;

        #region ctor

        public ExpenseGroupsController()
        {
            _repository = new ExpenseTrackerEFRepository(new ExpenseTrackerContext());
        }

        public ExpenseGroupsController(IExpenseTrackerRepository repository)
        {
            _repository = repository;
        }
        #endregion

        [Route("api/expensegroups", Name = "ExpenseGroupsList")]
        public IHttpActionResult Get(string sort = "id", string status = null, string userId = null,
             int page = 1, int pageSize = maxPageSize)
        {
            try
            {
                int statusId = -1;
                if (status != null)
                {
                    switch (status.ToLower())
                    {
                        case "open":
                            statusId = 1;
                            break;
                        case "confirmed":
                            statusId = 2;
                            break;
                        case "processed":
                            statusId = 3;
                            break;
                        default:
                            break;
                    }
                }


                // get expensegroups from repository
                var expenseGroups = _repository.GetExpenseGroups()
                    .ApplySort(sort)
                    .Where(eg => (statusId == -1 || eg.ExpenseGroupStatusId == statusId))
                    .Where(eg => (userId == null || eg.UserId == userId));


                // ensure the page size isn't larger than the maximum.
                if (pageSize > maxPageSize)
                {
                    pageSize = maxPageSize;
                }

                // calculate data for metadata
                var totalCount = expenseGroups.Count();
                var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

                var urlHelper = new UrlHelper(Request);
                var prevLink = page > 1 ? urlHelper.Link("ExpenseGroupsList",
                    new
                    {
                        page = page - 1,
                        pageSize = pageSize,
                        sort = sort
                        ,
                        status = status,
                        userId = userId
                    }) : "";
                var nextLink = page < totalPages ? urlHelper.Link("ExpenseGroupsList",
                    new
                    {
                        page = page + 1,
                        pageSize = pageSize,
                        sort = sort
                        ,
                        status = status,
                        userId = userId
                    }) : "";


                var paginationHeader = new
                {
                    currentPage = page,
                    pageSize = pageSize,
                    totalCount = totalCount,
                    totalPages = totalPages,
                    previousPageLink = prevLink,
                    nextPageLink = nextLink
                };

                HttpContext.Current.Response.Headers.Add("X-Pagination",
                   Newtonsoft.Json.JsonConvert.SerializeObject(paginationHeader));


                // return result
                return Ok(expenseGroups
                    .Skip(pageSize * (page - 1))
                    .Take(pageSize)
                    .ToList()
                    .Select(eg => eg.ToDto()));

            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }


        public IHttpActionResult Get(int id)
        {
            try
            {
                var expenseGroup = _repository.GetExpenseGroup(id);

                if (expenseGroup != null)
                {
                    return Ok(expenseGroup.ToDto());
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



        [HttpPost]
        [Route("api/expensegroups")]
        public IHttpActionResult Post([FromBody]Health.DTO.ExpenseGroup expenseGroup)
        {
            try
            {
                if (expenseGroup == null)
                {
                    return BadRequest();
                }

                // try mapping & saving
                var eg = expenseGroup.ToEntity();

                var result = _repository.InsertExpenseGroup(eg);
                if (result.Status == RepositoryActionStatus.Created)
                {
                    // map to dto
                    var newExpenseGroup = result.Entity.ToDto();
                    return Created<Health.DTO.ExpenseGroup>(Request.RequestUri
                        + "/" + newExpenseGroup.Id.ToString(), newExpenseGroup);
                }

                return BadRequest();

            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }



        public IHttpActionResult Put(int id, [FromBody]Health.DTO.ExpenseGroup expenseGroup)
        {
            try
            {
                if (expenseGroup == null)
                    return BadRequest();

                // map
                var eg = expenseGroup.ToEntity();

                var result = _repository.UpdateExpenseGroup(eg);
                if (result.Status == RepositoryActionStatus.Updated)
                {
                    // map to dto
                    var updatedExpenseGroup = result.Entity.ToDto();
                    return Ok(updatedExpenseGroup);
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



        [HttpPatch]
        public IHttpActionResult Patch(int id, [FromBody]JsonPatchDocument<Health.DTO.ExpenseGroup> expenseGroupPatchDocument)
        {
            try
            {
                if (expenseGroupPatchDocument == null)
                {
                    return BadRequest();
                }

                var expenseGroup = _repository.GetExpenseGroup(id);
                if (expenseGroup == null)
                {
                    return NotFound();
                }

                // map
                var eg = expenseGroup.ToDto();

                // apply changes to the DTO
                expenseGroupPatchDocument.ApplyTo(eg);

                // map the DTO with applied changes to the entity, & update
                var result = _repository.UpdateExpenseGroup(eg.ToEntity());

                if (result.Status == RepositoryActionStatus.Updated)
                {
                    // map to dto
                    var patchedExpenseGroup = result.Entity.ToDto();
                    return Ok(patchedExpenseGroup);
                }

                return BadRequest();
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }



        public IHttpActionResult Delete(int id)
        {
            try
            {

                var result = _repository.DeleteExpenseGroup(id);

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
    }
}
