// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExpenseGroup.cs" company="Microsoft Corporation">
//   Copyright (C) Microsoft Corporation.  All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Health.Repository.Entities
{
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("ExpenseGroup")]
    public class ExpenseGroup
    {
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string UserId { get; set; }

        [Required, StringLength(50)]
        public string Title { get; set; }

        [Required, StringLength(250)]
        public string Description { get; set; }

        public int ExpenseGroupStatusId { get; set; }

        public virtual ICollection<Expense> Expenses { get; set; }

        public virtual ExpenseGroupStatus ExpenseGroupStatus { get; set; }

        public ExpenseGroup()
        {
            Expenses = new HashSet<Expense>();
        }
        
    }
}