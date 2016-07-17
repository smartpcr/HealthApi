// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Expense.cs" company="Microsoft Corporation">
//   Copyright (C) Microsoft Corporation.  All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Health.Repository.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Expense")]
    public class Expense
    {
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Description { get; set; }

        [Column(TypeName = "date")]
        public DateTime Date { get; set; }

        public decimal Amount { get; set; }

        public int ExpenseGroupId { get; set; }

        public virtual ExpenseGroup ExpenseGroup { get; set; }
    }
}