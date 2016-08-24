using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web.Http.Routing;

namespace NEHO.Baseball.API.Helpers
{
    public class VersionConstraint : IHttpRouteConstraint
    {
        public const string VersionHeaderName = "api-version";
        private const int DefaultVersion = 1;

        public int AllowedVersion { get; }

        public VersionConstraint(int allowedVersion)
        {
            AllowedVersion = allowedVersion;
        }

        public bool Match(HttpRequestMessage httpMessageRequest, IHttpRoute route, string parameterName, IDictionary<string, object> values,
            HttpRouteDirection routeDirection)
        {
            if (routeDirection != HttpRouteDirection.UriResolution) return true;
            var version = GetVersionFromCustomRequestHeader(httpMessageRequest) ??
                          GetVersionFromCustomContentType(httpMessageRequest);

            return ((version ?? DefaultVersion) == AllowedVersion);
        }

        private int? GetVersionFromCustomContentType(HttpRequestMessage httpRequestMessage)
        {
            var mediaTypes = httpRequestMessage.Headers.Accept.Select(h => h.MediaType);

            var regEx = new Regex(@"application\/vnd\.playerapi\.v([\d]+)\+json");

            var matchingMediaType = mediaTypes.FirstOrDefault(mediaType => regEx.IsMatch(mediaType));

            if (matchingMediaType == null)
            {
                return null;
            }

            var match = regEx.Match(matchingMediaType);
            var versionAsString = match.Groups[1].Value;

            int version;

            if (int.TryParse(versionAsString, out version))
            {
                return version;
            }

            return null;
        }

        private int? GetVersionFromCustomRequestHeader(HttpRequestMessage httpRequestMessage)
        {
            string versionAsString;
            IEnumerable<string> headerValues;

            if (httpRequestMessage.Headers.TryGetValues(VersionHeaderName, out headerValues) &&
                headerValues.Count() == 1)
            {
                versionAsString = headerValues.First();
            }
            else
            {
                return null;
            }

            int version;

            if(versionAsString != null && int.TryParse(versionAsString, out version))
            {
                return version;
            }

            return null;
        }
    }
}