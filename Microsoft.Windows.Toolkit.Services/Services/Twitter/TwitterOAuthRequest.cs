// ******************************************************************
//
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
//
// ******************************************************************
using System;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Microsoft.Windows.Toolkit.Services.Twitter
{
    /// <summary>
    /// OAuth request.
    /// </summary>
    internal class TwitterOAuthRequest
    {
        /// <summary>
        /// HTTP request to specified Uri.
        /// </summary>
        /// <param name="requestUri">Uri to make OAuth request.</param>
        /// <param name="tokens">Tokens to pass in request.</param>
        /// <param name="method">Method to associate with the request</param>
        /// <returns>String result.</returns>
        public async Task<string> ExecuteAsync(Uri requestUri, TwitterOAuthTokens tokens, string method = "GET")
        {
            var requestBuilder = new TwitterOAuthRequestBuilder(requestUri, tokens, method);

            var handler = new HttpClientHandler { AutomaticDecompression = DecompressionMethods.GZip };
            var client = new HttpClient(handler);
            client.DefaultRequestHeaders.Add("Authorization", requestBuilder.AuthorizationHeader);

            HttpResponseMessage response;

            if (method == "POST")
            {
                response = await client.PostAsync(requestUri, null);
            }
            else
            {
                response = await client.GetAsync(requestUri);
            }

            return await response.Content.ReadAsStringAsync();
        }
    }
}
