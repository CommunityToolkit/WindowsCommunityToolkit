using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Microsoft.Toolkit.Uwp.Services.Weibo
{
    internal class WeiboOAuthRequest
    {
        private static HttpClient client;

        public WeiboOAuthRequest()
        {
            if (client == null)
            {
                HttpClientHandler handler = new HttpClientHandler();
                handler.AutomaticDecompression = DecompressionMethods.GZip;
                client = new HttpClient(handler);
            }
        }

        public async Task<string> ExecuteGetAsync(Uri requestUri, WeiboOAuthTokens tokens)
        {
            using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, requestUri))
            {
                UriBuilder requestUriBuilder = new UriBuilder(request.RequestUri);
                if (requestUriBuilder.Query.Contains("?"))
                {
                    requestUriBuilder.Query = requestUriBuilder.Query + "&access_token=" + tokens.AccessToken;
                }
                else
                {
                    requestUriBuilder.Query = requestUriBuilder.Query + "?access_token=" + tokens.AccessToken;
                }
                request.RequestUri = requestUriBuilder.Uri;

                using (HttpResponseMessage response = await client.SendAsync(request).ConfigureAwait(false))
                {
                    return ProcessError(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
                }
            }
        }

        private string ProcessError(string content)
        {
            if (content.StartsWith("\"error\":"))
            {
                WeiboError error = JsonConvert.DeserializeObject<WeiboError>(content);

                throw new WeiboException { Error = error };
            }

            return content;
        }
    }
}