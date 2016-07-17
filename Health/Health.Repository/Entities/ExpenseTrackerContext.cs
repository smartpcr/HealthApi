// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExpenseTrackerContext.cs" company="Microsoft Corporation">
//   Copyright (C) Microsoft Corporation.  All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Health.Repository.Entities
{
    using System.Data.Entity;

    public class ExpenseTrackerContext : DbContext
    {
        public virtual DbSet<Expense> Expenses { get; set; }
        public virtual DbSet<ExpenseGroup> ExpenseGroups { get; set; }
        public virtual DbSet<ExpenseGroupStatus> ExpenseGroupStatusses { get; set; }

        public ExpenseTrackerContext() : base("name=ExpenseTrackerContext")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Expense>()
                .Property(e => e.Amount)
                .HasPrecision(18, 0);
            modelBuilder.Entity<ExpenseGroup>()
                .HasMany(e => e.Expenses)
                .WithRequired(e => e.ExpenseGroup).WillCascadeOnDelete();
            modelBuilder.Entity<ExpenseGroupStatus>()
                .HasMany(e=>e.ExpenseGroups)
                .WithRequired(e=>e.ExpenseGroupStatus)
                .HasForeignKey(e=>e.ExpenseGroupStatusId)
                .WillCascadeOnDelete(false);
        }
    }
}