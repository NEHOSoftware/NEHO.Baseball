using System.Collections.Generic;
using System.Web.Http.Routing;

namespace NEHO.Baseball.API.Helpers
{
    internal class VersionedRoute : RouteFactoryAttribute
    {
        public int AllowedVersion { get; }

        public VersionedRoute(string template, int allowedVersion) : base(template)
        {
            AllowedVersion = allowedVersion;
        }

        public override IDictionary<string, object> Constraints
        {
            get
            {
                var constraints = new HttpRouteValueDictionary();
                constraints.Add("version", new VersionConstraint(AllowedVersion));

                return constraints;
            }
        }
    }
}