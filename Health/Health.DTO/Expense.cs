// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Expense.cs" company="Microsoft Corporation">
//   Copyright (C) Microsoft Corporation.  All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Health.DTO
{
    using System;

    public class Expense
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public int ExpenseGroupId { get; set; }
    }
}