using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Spoticom.Middleware
{
    public static class AppBuilderExtensions
    {
        public static void UseSpotifyAuthentication(this IAppBuilder app)
        {
            app.Use<SpotifyAuthenticationComponent>();
        }
    }
}