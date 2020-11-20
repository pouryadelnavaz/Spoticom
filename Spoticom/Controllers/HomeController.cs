using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Spoticom.Classes;


namespace Spoticom.Controllers
{
    public class HomeController : Controller
    {
        private string GetRedirectURI()
        {
            string HostName = Request.Url.GetLeftPart(UriPartial.Authority);
            string RedirectURI = $"{HostName}{Url.Action("GetRecommendation", "Home")}";
            return RedirectURI;
        }
        private void SetCookie(string Name, string Value, DateTime Expires)
        {
            if (Request.Cookies[Name] != null)
            {
                Response.Cookies[Name].Expires = DateTime.Now.AddDays(-1);
            }
            HttpCookie MyCookie  = new HttpCookie(Name);
            MyCookie.Value = Value;
            MyCookie.Expires = Expires;
            Response.Cookies.Add(MyCookie);
        }
        public ActionResult Index()
        {
            Utilities.GetMarkets();
            return View();
            
        }
        public ActionResult StartAuthorization()
        {
            var AuthorizationInfo = Utilities.GetAuthorizationInfo();
            var OAuthClient = new SpotifyCommunication.Authorization(AuthorizationInfo.ClientID, AuthorizationInfo.ClientSecret);

            var RedirectURI = GetRedirectURI();
            // You can read about the scopes in the API Documentation https://developer.spotify.com/documentation/general/guides/scopes/
            string[] Scopes = new string[] { "user-library-modify", "user-library-read", "user-read-currently-playing" };
            string AuthorizationURL = OAuthClient.GetAuthorizationURL(RedirectURI, Guid.NewGuid().ToString().Split('-')[0], Scopes, true);
            return Redirect(AuthorizationURL);
        }
        public async Task<ActionResult> GetRecommendation(string code, string state)
        {
            if (string.IsNullOrEmpty(code))
            {
                return RedirectToAction("Index");
            }
            var AuthorizationInfo = Utilities.GetAuthorizationInfo();
            var OAuthClient = new SpotifyCommunication.Authorization(AuthorizationInfo.ClientID, AuthorizationInfo.ClientSecret);
            var RedirectURI = GetRedirectURI();
            var AuthorizationResult = await OAuthClient.GetAccessTokenAsync(code, RedirectURI);
            if (string.IsNullOrEmpty(AuthorizationResult.access_token))
            {
                return RedirectToAction("Index");
            }
            ViewBag.AccessToken = AuthorizationResult.access_token;
            SpotifyCommunication.SpotifyApiLibrary client = new SpotifyCommunication.SpotifyApiLibrary(AuthorizationResult.access_token);
            var CurrentUser = await client.GetCurrentUserInfoAsync();
            ViewBag.Title = $"Welcome {CurrentUser.display_name}";
            ViewBag.User = CurrentUser;
            SetCookie("SpotifyAccessToken", AuthorizationResult.access_token, AuthorizationResult.ActiveUntil);
            return View();
        }
    }
}