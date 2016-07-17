// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RepositoryActionResult.cs" company="Microsoft Corporation">
//   Copyright Microsoft Corporation, all rights reserved
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Health.Repository
{
    using System;

    public class RepositoryActionResult<T> where T: class 
    {
        public T Entity { get; private set; }
        public RepositoryActionStatus Status { get; private set; }
        public Exception Exception { get; private set; }

        public RepositoryActionResult(T entity, RepositoryActionStatus status)
        {
            this.Entity = entity;
            this.Status = status;
        }

        public RepositoryActionResult(T entity, RepositoryActionStatus status, Exception exception)
            :this(entity, status)
        {
            this.Exception = exception;
        }
    }
}
