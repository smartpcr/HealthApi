// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExpenseGroupStatussesController.cs" company="Microsoft Corporation">
//   Copyright Microsoft Corporation, all rights reserved
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Api.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Http;
    using Health.Repository;
    using Health.Repository.Entities;
    using Health.Repository.Factories;

    public class ExpenseGroupStatussesController : ApiController
    {
        IExpenseTrackerRepository _repository;

        public ExpenseGroupStatussesController()
        {
            _repository = new ExpenseTrackerEFRepository(new ExpenseTrackerContext());
        }

        public ExpenseGroupStatussesController(IExpenseTrackerRepository repository)
        {
            _repository = repository;
        }


        public IHttpActionResult Get()
        {

            try
            {
                // get expensegroupstatusses & map to DTO's
                var expenseGroupStatusses = _repository.GetExpenseGroupStatusses().ToList()
                    .Select(egs=>egs.ToDto());

                return Ok(expenseGroupStatusses);

            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }
    }
}
