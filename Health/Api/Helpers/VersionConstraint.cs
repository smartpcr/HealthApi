// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VersionConstraint.cs" company="Microsoft Corporation">
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
    using System.Linq;
    using System.Linq.Dynamic;
    using System.Net.Http;
    using System.Text.RegularExpressions;
    using System.Web.Http.Routing;

    public class VersionConstraint : IHttpRouteConstraint
    {

        public const string VersionHeaderName = "api-version";
        private const int DefaultVersion = 1;
        public int AllowedVersion { get; private set; }

        public VersionConstraint(int allowedVersion)
        {
            AllowedVersion = allowedVersion;
        }

        #region Implementation of IHttpRouteConstraint

        /// <summary>Determines whether this instance equals a specified route.</summary>
        /// <returns>True if this instance equals a specified route; otherwise, false.</returns>
        /// <param name="request">The request.</param>
        /// <param name="route">The route to compare.</param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <param name="values">A list of parameter values.</param>
        /// <param name="routeDirection">The route direction.</param>
        public bool Match(HttpRequestMessage request, IHttpRoute route, string parameterName, IDictionary<string, object> values, HttpRouteDirection routeDirection)
        {
            if (routeDirection == HttpRouteDirection.UriResolution)
            {
                // try custom request header "api-version" before custom content type in accept header
                int? version = GetVersionFromCustomRequestHeader(request);
                if (version == null)
                {
                    version = GetVersionFromCustomContentType(request);
                }

                return (version ?? DefaultVersion) == AllowedVersion;
            }
            return false;
        }

        #endregion

        #region private

        private int? GetVersionFromCustomContentType(HttpRequestMessage request)
        {
            var mediaTypes = request.Headers.Accept.Select(h => h.MediaType);
            string matchingMediaType = null;
            Regex regex = new Regex(@"application\/vnd\.expensetrackerapi\.v([\d]+)+json");
            foreach (var mediaType in mediaTypes)
            {
                if (regex.IsMatch(mediaType))
                {
                    matchingMediaType = mediaType;
                    break;
                }
            }

            if (matchingMediaType == null)
                return null;

            Match m = regex.Match(matchingMediaType);
            if (m.Success)
            {
                var versionAsString = m.Groups[1].Value;
                int version;
                if (int.TryParse(versionAsString, out version))
                {
                    return version;
                }
            }
            
            return null;
        }

        private int? GetVersionFromCustomRequestHeader(HttpRequestMessage request)
        {
            string versionAsString = null;
            IEnumerable<string> headerValues;
            if (request.Headers.TryGetValues(VersionHeaderName, out headerValues) && headerValues.Count() == 1)
            {
                versionAsString = headerValues.First();
            }
            else
            {
                return null;
            }

            int version;
            if (versionAsString != null && int.TryParse(versionAsString, out version))
            {
                return version;
            }

            return null;
        }
        #endregion
    }
}
