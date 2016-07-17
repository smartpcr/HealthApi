// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DtoEntityExtension.cs" company="Microsoft Corporation">
//   Copyright (C) Microsoft Corporation.  All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Health.Repository.Factories
{
    using System;
    using System.Collections.Generic;
    using System.Dynamic;
    using System.Linq;
    using System.Reflection;
    using Health.Repository.Entities;
    using Health.Repository.Helpers;

    public static class DtoEntityExtension
    {
        #region expense 
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

        public static object Expand(this DTO.Expense expense, List<string> listOfFields)
        {
            return ExpandObject(expense, listOfFields);
        }
        #endregion

        #region expenseGroup 
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

        public static object Expand(this DTO.ExpenseGroup expenseGroup, List<string> lstOfFields)
        {
            List<string> lstOfFieldsToWorkWith = new List<string>(lstOfFields);
            // does it include any expense-related field?
            var lstOfExpenseFields = lstOfFieldsToWorkWith.Where(f => f.Contains("expenses")).ToList();
            if (!lstOfExpenseFields.Any())
            {
                return ExpandObject(expenseGroup, lstOfFields);
            }

            // if one of those fields is "expenses", we need to ensure the FULL expense is returned.  If
            // it's only subfields, only those subfields have to be returned.
            bool returnPartialExpense = lstOfExpenseFields.Any() && !lstOfExpenseFields.Contains("expenses");

            if (returnPartialExpense)
            {
                // remove all expense-related fields from the list of fields,
                // as we will use the CreateDateShapedObject function in ExpenseFactory
                // for that.
                lstOfFieldsToWorkWith.RemoveRange(lstOfExpenseFields);
                lstOfExpenseFields = lstOfExpenseFields.Select(f =>
                    f.Substring(f.IndexOf(".", StringComparison.Ordinal) + 1)).ToList();
            }
            else
            {
                // we shouldn't return a partial expense, but the consumer might still have
                // asked for a subfield together with the main field, ie: expense,expense.id.  We 
                // need to remove those subfields in that case.
                lstOfExpenseFields.Remove("expenses");
                lstOfFieldsToWorkWith.RemoveRange(lstOfExpenseFields);
            }

            // create a new ExpandoObject & dynamically create the properties for this object
            // if we have an expense
            ExpandoObject objectToReturn = new ExpandoObject();
            foreach (var field in lstOfFieldsToWorkWith)
            {
                // need to include public and instance, b/c specifying a binding flag overwrites the
                // already-existing binding flags.

                var fieldValue = expenseGroup.GetType()
                    .GetProperty(field, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance)
                    .GetValue(expenseGroup, null);

                // add the field to the ExpandoObject
                ((IDictionary<string, object>)objectToReturn).Add(field, fieldValue);
            }

            if (returnPartialExpense)
            {
                // add a list of expenses, and in that, add all those expenses
                List<object> expenses = new List<object>();
                foreach (var expense in expenseGroup.Expenses)
                {
                    expenses.Add(ExpandObject(expense, lstOfExpenseFields));
                }
                ((IDictionary<string, object>)objectToReturn).Add("expenses", expenses);
            }

            return objectToReturn;
        }
        #endregion

        #region expenseGroupStatus 
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

        public static object Expand(this DTO.ExpenseGroupStatus expenseGroupStatus, List<string> listOfFields)
        {
            return ExpandObject(expenseGroupStatus, listOfFields);
        }
        #endregion

        #region private

        private static object ExpandObject(object obj, List<string> lstFields)
        {
            if (!lstFields.Any())
            {
                return obj;
            }

            ExpandoObject objectToReturn = new ExpandoObject();
            foreach (var field in lstFields)
            {
                var fieldValue = obj.GetType().GetProperty(field, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance)
                    .GetValue(obj, null);

                ((IDictionary<string, object>)objectToReturn).Add(field, fieldValue);
            }

            return objectToReturn;
        }
        #endregion
    }
}