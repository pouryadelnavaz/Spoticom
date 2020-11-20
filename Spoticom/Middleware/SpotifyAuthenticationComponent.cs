using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Spoticom.Middleware
{
    // use an alias for the OWIN AppFunc:
    using AppFunc = Func<IDictionary<string, object>, Task>;

    public class SpotifyAuthenticationComponent
    {
        AppFunc _next;
        public SpotifyAuthenticationComponent(AppFunc next)
        {
            _next = next;
        }

        public async Task Invoke(IDictionary<string, object> environment)
        {
            IOwinContext context = new OwinContext(environment);
            var AccessTokenCookie = context.Request.Cookies["SpotifyAccessToken"];
            if (AccessTokenCookie != null)
            {
                context.Response.StatusCode = 200;
                context.Response.ReasonPhrase = "OK";
                await _next.Invoke(environment);
            }
            else
            {
                context.Response.StatusCode = 401;
                context.Response.ReasonPhrase = "Not Authorized";

                // Send back a really silly error page:
                await context.Response.WriteAsync(string.Format("<h1>Error {0}-{1}",
                    context.Response.StatusCode,
                    context.Response.ReasonPhrase));
            }
        }
    }
}