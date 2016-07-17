// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExpenseGroup.cs" company="Microsoft Corporation">
//   Copyright (C) Microsoft Corporation.  All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Health.DTO
{
    using System.Collections;
    using System.Collections.Generic;

    public class ExpenseGroup
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int ExpenseGroupStatusId { get; set; }
        public ICollection<Expense> Expenses { get; set; }
    }
}