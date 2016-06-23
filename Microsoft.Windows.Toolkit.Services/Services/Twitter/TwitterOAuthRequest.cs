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
            string result;
            var request = CreateRequest(requestUri, tokens, method);
            var response = await request.GetResponseAsync();
            var responseStream = GetResponseStream(response);

            using (StreamReader sr = new StreamReader(responseStream))
            {
                result = sr.ReadToEnd();
            }

            return result;
        }

        /// <summary>
        /// Returns stream from web response.
        /// </summary>
        /// <param name="response">WebResponse response.</param>
        /// <returns>Stream of response.</returns>
        private static Stream GetResponseStream(WebResponse response)
        {
            var encoding = response.Headers["content-encoding"];

            if (encoding != null && encoding == "gzip")
            {
                return new GZipStream(response.GetResponseStream(), CompressionMode.Decompress);
            }

            return response.GetResponseStream();
        }

        /// <summary>
        /// Builds HttpWebRequest.
        /// </summary>
        /// <param name="requestUri">Uri to request.</param>
        /// <param name="tokens">OAuth tokens to pass in request.</param>
        /// <param name="method">Method to use with the request.</param>
        /// <returns>Built up WebRequest.</returns>
        private static WebRequest CreateRequest(Uri requestUri, TwitterOAuthTokens tokens, string method = "GET")
        {
            var requestBuilder = new TwitterOAuthRequestBuilder(requestUri, tokens, method);

            var request = (HttpWebRequest)WebRequest.Create(requestBuilder.EncodedRequestUri);

            request.UseDefaultCredentials = true;
            request.Method = method;
            request.Headers["Authorization"] = requestBuilder.AuthorizationHeader;

            return request;
        }
    }
}
