// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Microsoft.Toolkit.Services.Weibo
{
    /// <summary>
    /// OAuth request.
    /// </summary>
    internal class WeiboOAuthRequest
    {
        private static HttpClient client;

        /// <summary>
        /// Initializes a new instance of the <see cref="WeiboOAuthRequest"/> class.
        /// </summary>
        public WeiboOAuthRequest()
        {
            if (client == null)
            {
                HttpClientHandler handler = new HttpClientHandler();
                handler.AutomaticDecompression = DecompressionMethods.GZip;
                client = new HttpClient(handler);
            }
        }

        /// <summary>
        /// HTTP Get request to specified Uri.
        /// </summary>
        /// <param name="requestUri">Uri to make OAuth request.</param>
        /// <param name="tokens">Tokens to pass in request.</param>
        /// <returns>String result.</returns>
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