// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExpenseGroupStatus.cs" company="Microsoft Corporation">
//   Copyright Microsoft Corporation, all rights reserved
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Health.Repository.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class ExpenseGroupStatus
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [Required, StringLength(50)]
        public string Description { get; set; }

        public virtual ICollection<ExpenseGroup> ExpenseGroups { get; set; }

        public ExpenseGroupStatus()
        {
            ExpenseGroups = new HashSet<ExpenseGroup>();
        }
    }
}
