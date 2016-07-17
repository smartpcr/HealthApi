// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RepositoryActionStatus.cs" company="Microsoft Corporation">
//   Copyright Microsoft Corporation, all rights reserved
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Health.Repository
{
    public enum RepositoryActionStatus
    {
        Ok,
        Created,
        Updated,
        NotFound,
        Deleted,
        NothingModified,
        Error
    }
}
