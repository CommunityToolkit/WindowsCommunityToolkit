// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

using System;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Windows.Web.Http;
using Windows.Web.Http.Headers;

namespace Microsoft.Toolkit.Uwp.Services.Twitter
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
            using (var request = new HttpHelperRequest(requestUri, HttpMethod.Get))
            {
                var requestBuilder = new TwitterOAuthRequestBuilder(requestUri, tokens, "GET");

                request.Headers.Authorization = HttpCredentialsHeaderValue.Parse(requestBuilder.AuthorizationHeader);

                using (var response = await HttpHelper.Instance.SendRequestAsync(request).ConfigureAwait(false))
                {
                    return ProcessErrors(await response.GetTextResultAsync().ConfigureAwait(false));
                }
            }
        }

        /// <summary>
        /// HTTP Post request to specified Uri.
        /// </summary>
        /// <param name="requestUri">Uri to make OAuth request.</param>
        /// <param name="tokens">Tokens to pass in request.</param>
        /// <returns>String result.</returns>
        public async Task<string> ExecutePostAsync(Uri requestUri, TwitterOAuthTokens tokens)
        {
            using (var request = new HttpHelperRequest(requestUri, HttpMethod.Post))
            {
                var requestBuilder = new TwitterOAuthRequestBuilder(requestUri, tokens, "POST");

                request.Headers.Authorization = HttpCredentialsHeaderValue.Parse(requestBuilder.AuthorizationHeader);

                using (var response = await HttpHelper.Instance.SendRequestAsync(request).ConfigureAwait(false))
                {
                    return ProcessErrors(await response.GetTextResultAsync().ConfigureAwait(false));
                }
            }
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
            JToken mediaId = null;

            try
            {
                using (var multipartFormDataContent = new HttpMultipartFormDataContent(boundary))
                {
                    using (var byteContent = new HttpBufferContent(content.AsBuffer()))
                    {
                        multipartFormDataContent.Add(byteContent, "media");

                        using (var request = new HttpHelperRequest(requestUri, HttpMethod.Post))
                        {
                            var requestBuilder = new TwitterOAuthRequestBuilder(requestUri, tokens, "POST");

                            request.Headers.Authorization = HttpCredentialsHeaderValue.Parse(requestBuilder.AuthorizationHeader);

                            request.Content = multipartFormDataContent;

                            using (var response = await HttpHelper.Instance.SendRequestAsync(request).ConfigureAwait(false))
                            {
                                string jsonResult = await response.GetTextResultAsync().ConfigureAwait(false);

                                JObject jObj = JObject.Parse(jsonResult);
                                mediaId = jObj["media_id_string"];
                            }
                        }
                    }
                }
            }
            catch (ObjectDisposedException)
            {
                // known issue
                // http://stackoverflow.com/questions/39109060/httpmultipartformdatacontent-dispose-throws-objectdisposedexception
            }

            return mediaId.ToString();
        }

        private string ProcessErrors(string content)
        {
            if (content.StartsWith("{\"errors\":"))
            {
                var errors = JsonConvert.DeserializeObject<TwitterErrors>(content);

                throw new TwitterException { Errors = errors };
            }

            return content;
        }
    }
}
