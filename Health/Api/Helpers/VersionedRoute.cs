// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VersionedRoute.cs" company="Microsoft Corporation">
//   Copyright Microsoft Corporation, all rights reserved
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Api.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Web.Http.Routing;

    public class VersionedRoute : RouteFactoryAttribute
    {
        public int AllowedVersion { get; private set; }

        public VersionedRoute(string template, int allowedVersion)
            : base(template)
        {
            AllowedVersion = allowedVersion;
        }

        #region Overrides of RouteFactoryAttribute

        /// <summary>Gets the route constraints, if any; otherwise null.</summary>
        /// <returns>The route constraints, if any; otherwise null.</returns>
        public override IDictionary<string, object> Constraints
        {
            get
            {
                var constraints = new HttpRouteValueDictionary();
                constraints.Add("version", new VersionConstraint(AllowedVersion));
                return constraints;
            }
        }

        #endregion
    }
}
