// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ListExtensions.cs" company="Microsoft Corporation">
//   Copyright (C) Microsoft Corporation.  All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Health.Repository.Helpers
{
    using System.Collections.Generic;
    using System.Linq;

    public static class ListExtensions
    {
        public static void ReoveRange<T>(this List<T> source, IEnumerable<T> rangeToRemove)
        {
            if (rangeToRemove == null || !rangeToRemove.Any())
                return;

            foreach (T item in rangeToRemove)
            {
                source.Remove(item);
            }
        }
    }
}