﻿using Microsoft.Owin;

using Owin;

[assembly: OwinStartup(typeof(NEHO.Baseball.WebClient.Startup))]

namespace NEHO.Baseball.WebClient
{
    public partial class Startup
    
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}