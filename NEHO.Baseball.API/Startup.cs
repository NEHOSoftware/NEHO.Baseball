using Microsoft.Owin;

using Owin;

[assembly: OwinStartup("Baseball.API", typeof(NEHO.Baseball.API.Startup))]

namespace NEHO.Baseball.API
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}