// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IExpenseTrackerRepository.cs" company="Microsoft Corporation">
//   Copyright (C) Microsoft Corporation.  All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Health.Repository
{
    using System.Linq;
    using Health.Repository.Entities;

    public interface IExpenseTrackerRepository
    {
        RepositoryActionResult<Expense> DeleteExpense(int id);
        RepositoryActionResult<ExpenseGroup> DeleteExpenseGroup(int id);
        Expense GetExpense(int id, int? expenseGroupId = null);
        ExpenseGroup GetExpenseGroup(int id);
        ExpenseGroup GetExpenseGroup(int id, string userId);
        IQueryable<ExpenseGroup> GetExpenseGroups();
        IQueryable<ExpenseGroup> GetExpenseGroups(string userId);
        ExpenseGroupStatus GetExpenseGroupStatus(int id);
        IQueryable<ExpenseGroupStatus> GetExpenseGroupStatusses();
        IQueryable<ExpenseGroup> GetExpenseGroupsWithExpenses();
        ExpenseGroup GetExpenseGroupWithExpenses(int id);
        ExpenseGroup GetExpenseGroupWithExpenses(int id, string userId);
        IQueryable<Expense> GetExpenses();
        IQueryable<Expense> GetExpenses(int expenseGroupId);

        RepositoryActionResult<Expense> InsertExpense(Expense e);
        RepositoryActionResult<ExpenseGroup> InsertExpenseGroup(ExpenseGroup eg);
        RepositoryActionResult<Expense> UpdateExpense(Expense e);
        RepositoryActionResult<ExpenseGroup> UpdateExpenseGroup(ExpenseGroup eg);
    }
}