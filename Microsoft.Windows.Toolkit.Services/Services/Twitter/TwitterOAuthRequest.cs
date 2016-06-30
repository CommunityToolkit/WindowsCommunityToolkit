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
        /// HTTP Get request to specified Uri.
        /// </summary>
        /// <param name="requestUri">Uri to make OAuth request.</param>
        /// <param name="tokens">Tokens to pass in request.</param>
        /// <returns>String result.</returns>
        public async Task<string> ExecuteGetAsync(Uri requestUri, TwitterOAuthTokens tokens)
        {
            HttpClient client = GetHttpClient(requestUri, tokens);

            HttpResponseMessage response = await client.GetAsync(requestUri);

            return await response.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// HTTP Post request to specified Uri.
        /// </summary>
        /// <param name="requestUri">Uri to make OAuth request.</param>
        /// <param name="tokens">Tokens to pass in request.</param>
        /// <returns>String result.</returns>
        public async Task<string> ExecutePostAsync(Uri requestUri, TwitterOAuthTokens tokens)
        {
            HttpClient client = GetHttpClient(requestUri, tokens, "POST");

            HttpResponseMessage response = await client.PostAsync(requestUri, null);

            return await response.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// HTTP Post request to specified Uri.
        /// </summary>
        /// <param name="requestUri">Uri to make OAuth request.</param>
        /// <param name="tokens">Tokens to pass in request.</param>
        /// <param name="boundary">Boundary used to separate data.</param>
        /// <param name="content">Data to post to server.</param>
        /// <returns>String result.</returns>
        public async Task<string> ExecutePostMultipartAsync(Uri requestUri, TwitterOAuthTokens tokens, string boundary, byte[] content)
        {
            HttpClient client = GetHttpClient(requestUri, tokens, "POST");
            MultipartFormDataContent multipartFormDataContent = new MultipartFormDataContent(boundary);
            HttpContent byteContent = new ByteArrayContent(content);

            multipartFormDataContent.Add(byteContent);

            HttpResponseMessage response = await client.PostAsync(requestUri, multipartFormDataContent);

            var respe = await response.Content.ReadAsStringAsync();

            return respe;
        }

        private static HttpClient GetHttpClient(Uri requestUri, TwitterOAuthTokens tokens, string method = "GET")
        {
            var requestBuilder = new TwitterOAuthRequestBuilder(requestUri, tokens, method);

            var handler = new HttpClientHandler { AutomaticDecompression = DecompressionMethods.GZip };
            var client = new HttpClient(handler);
            client.DefaultRequestHeaders.Add("Authorization", requestBuilder.AuthorizationHeader);
            return client;
        }
    }
}
