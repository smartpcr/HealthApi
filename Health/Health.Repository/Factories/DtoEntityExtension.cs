// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DtoEntityExtension.cs" company="Microsoft Corporation">
//   Copyright (C) Microsoft Corporation.  All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Health.Repository.Factories
{
    using System.Collections.Generic;
    using System.Linq;
    using Health.Repository.Entities;

    public static class DtoEntityExtension
    {
        public static DTO.Expense ToDto(this Expense expense)
        {
            return new DTO.Expense()
            {
                Amount = expense.Amount,
                Date = expense.Date,
                Description=expense.Description,
                ExpenseGroupId = expense.ExpenseGroupId,
                Id = expense.Id
            };
        }

        public static Expense ToEntity(this DTO.Expense expense)
        {
            return new Expense()
            {
                Amount = expense.Amount,
                Date = expense.Date,
                Description = expense.Description,
                ExpenseGroupId = expense.ExpenseGroupId,
                Id = expense.Id
            };
        }

        public static ExpenseGroup ToEntity(this DTO.ExpenseGroup expenseGroup)
        {
            return new ExpenseGroup()
            {
                Description = expenseGroup.Description,
                ExpenseGroupStatusId = expenseGroup.ExpenseGroupStatusId,
                Id = expenseGroup.Id,
                Title = expenseGroup.Title,
                UserId = expenseGroup.UserId,
                Expenses = expenseGroup.Expenses == null ? new List<Expense>() : expenseGroup.Expenses.Select(e => e.ToEntity()).ToList()
            };
        }


        public static DTO.ExpenseGroup ToDto(this ExpenseGroup expenseGroup)
        {
            return new DTO.ExpenseGroup()
            {
                Description = expenseGroup.Description,
                ExpenseGroupStatusId = expenseGroup.ExpenseGroupStatusId,
                Id = expenseGroup.Id,
                Title = expenseGroup.Title,
                UserId = expenseGroup.UserId,
                Expenses = expenseGroup.Expenses.Select(e => e.ToDto()).ToList()
            };
        }

        public static ExpenseGroupStatus ToEntity(this DTO.ExpenseGroupStatus expenseGroupStatus)
        {
            return new ExpenseGroupStatus()
            {
                Description = expenseGroupStatus.Description,
                Id = expenseGroupStatus.Id
            };
        }


        public static DTO.ExpenseGroupStatus ToDto(this ExpenseGroupStatus expenseGroupStatus)
        {
            return new DTO.ExpenseGroupStatus()
            {
                Description = expenseGroupStatus.Description,
                Id = expenseGroupStatus.Id
            };
        }
    }
}