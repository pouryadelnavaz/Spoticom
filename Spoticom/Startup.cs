using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using Spoticom.Middleware;

[assembly: OwinStartup(typeof(Spoticom.Startup))]

namespace Spoticom
{

    using AppFunc = Func<IDictionary<string, object>, Task>;
    public class Startup
    {

        public void Configuration(IAppBuilder app)
        {
            // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=316888
            app.Map("/WebMethods", MyApp =>
            {
                MyApp.UseSpotifyAuthentication();
            });
        }

    }
}
