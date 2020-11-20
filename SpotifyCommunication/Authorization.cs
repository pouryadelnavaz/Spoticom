using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using SpotifyCommunication.Models;

namespace SpotifyCommunication
{
    public class Authorization : HTTPClient
    {
        private string _ClientID, _ClientSecret;
        private const string _AuthorizeEndpoint = "https://accounts.spotify.com/authorize";
        private const string _TokenEndpoint = "https://accounts.spotify.com/api/token";


        public Authorization(string ClientID, string ClientSecret)
        {
            _ClientID = ClientID;
            _ClientSecret = ClientSecret;
        }
        public string GetAuthorizationURL(string RedirectURI, string State, string[] Scope, bool ShowDialog)
        {
            string _Scope = string.Join(" ", Scope);
            return $"{_AuthorizeEndpoint}?client_id={_ClientID}&response_type=code&redirect_uri={RedirectURI}&scope={_Scope}&show_dialog={ShowDialog}&state={State}";
        }
        
        public async Task<SpotifyAccessToken> GetAccessTokenAsync(string Code, string RedirectURI)
        {
            var RequestBody = new Dictionary<string, string>
            {
                 {"grant_type", "authorization_code" },
                 {"code", Code },
                 {"redirect_uri",RedirectURI },
                 {"client_id",_ClientID},
                 { "client_secret",_ClientSecret}
            };
            var AccessTokenJson = await SendPostRequestAsync(_TokenEndpoint, RequestBody);
            var AccessTokenResponse = JsonConvert.DeserializeObject<SpotifyAccessToken>(AccessTokenJson);
            AccessTokenResponse.ActiveUntil = DateTime.Now.AddSeconds(AccessTokenResponse.expires_in);
            return AccessTokenResponse;
        }
    }
}
