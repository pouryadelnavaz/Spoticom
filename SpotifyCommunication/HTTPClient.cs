using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;


namespace SpotifyCommunication
{
    public class HTTPClient
    {
        private const string _ApiEndPoint = "https://api.spotify.com/v1/";
        protected async Task<string> SendPostRequestAsync(string RequestURI, Dictionary<string, string> RequestBody)
        {
            HttpClient client = new HttpClient();
            var content = new FormUrlEncodedContent(RequestBody);
            var response = await client.PostAsync(RequestURI, content);
            var responseString = await response.Content.ReadAsStringAsync();
            return responseString;
        }
    
        protected async Task<T> SendGetRequestAsync<T>(bool AuthorizationReqierd, string ApiFunction, string AccessToken = null, string Content = null)
        {
            
            string Url = $"{_ApiEndPoint}{ApiFunction}";
            if (!string.IsNullOrEmpty(Content))
                Url = $"{Url}{Content}";
            HttpClient client = new HttpClient();
            if (AuthorizationReqierd)
            {
                client.DefaultRequestHeaders.Authorization
                                         = new AuthenticationHeaderValue("Bearer", AccessToken);
            }

            var Response = await client.GetAsync(Url);
            var ResponseString = await Response.Content.ReadAsStringAsync();
            var ObjectResult = JsonConvert.DeserializeObject<T>(ResponseString);
            return (T)Convert.ChangeType(ObjectResult, typeof(T));
        }
    }
}
