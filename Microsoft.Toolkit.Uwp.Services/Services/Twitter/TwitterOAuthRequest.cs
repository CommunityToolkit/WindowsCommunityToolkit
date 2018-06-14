// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Microsoft.Toolkit.Uwp.Services.Twitter
{
    /// <summary>
    /// OAuth request.
    /// </summary>
    internal class TwitterOAuthRequest
    {
        private static HttpClient client;

        private bool _abort;

        /// <summary>
        /// Initializes a new instance of the <see cref="TwitterOAuthRequest"/> class.
        /// </summary>
        public TwitterOAuthRequest()
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
        public async Task<string> ExecuteGetAsync(Uri requestUri, TwitterOAuthTokens tokens)
        {
            using (var request = new HttpRequestMessage(HttpMethod.Get, requestUri))
            {
                var requestBuilder = new TwitterOAuthRequestBuilder(requestUri, tokens, "GET");

                request.Headers.Authorization = AuthenticationHeaderValue.Parse(requestBuilder.AuthorizationHeader);

                using (var response = await client.SendAsync(request).ConfigureAwait(false))
                {
                    return ProcessErrors(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
                }
            }
        }

        /// <summary>
        /// HTTP Get request for stream service.
        /// </summary>
        /// <param name="requestUri">Uri to make OAuth request.</param>
        /// <param name="tokens">Tokens to pass in request.</param>
        /// <param name="callback">Function invoked when stream available.</param>
        /// <returns>awaitable task</returns>
        public async Task ExecuteGetStreamAsync(Uri requestUri, TwitterOAuthTokens tokens, TwitterStreamCallbacks.RawJsonCallback callback)
        {
            using (var request = new HttpRequestMessage(HttpMethod.Get, requestUri))
            {
                var requestBuilder = new TwitterOAuthRequestBuilder(requestUri, tokens);

                request.Headers.Authorization = AuthenticationHeaderValue.Parse(requestBuilder.AuthorizationHeader);

                using (var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false))
                {
                    var responseStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);

                    using (var reader = new StreamReader(responseStream))
                    {
                        while (!_abort && !reader.EndOfStream)
                        {
                            var result = reader.ReadLine();

                            if (!string.IsNullOrEmpty(result))
                            {
                                callback?.Invoke(result);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Stop reading stream
        /// </summary>
        public void Abort()
        {
            _abort = true;
        }

        /// <summary>
        /// HTTP Post request to specified Uri.
        /// </summary>
        /// <param name="requestUri">Uri to make OAuth request.</param>
        /// <param name="tokens">Tokens to pass in request.</param>
        /// <returns>String result.</returns>
        public async Task<string> ExecutePostAsync(Uri requestUri, TwitterOAuthTokens tokens)
        {
            using (var request = new HttpRequestMessage(HttpMethod.Post, requestUri))
            {
                var requestBuilder = new TwitterOAuthRequestBuilder(requestUri, tokens, "POST");

                request.Headers.Authorization = AuthenticationHeaderValue.Parse(requestBuilder.AuthorizationHeader);

                using (var response = await client.SendAsync(request).ConfigureAwait(false))
                {
                    return ProcessErrors(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
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
                using (var multipartFormDataContent = new MultipartFormDataContent(boundary))
                {
                    using (var byteContent = new ByteArrayContent(content))
                    {
                        multipartFormDataContent.Add(byteContent, "media");

                        using (var request = new HttpRequestMessage(HttpMethod.Post, requestUri))
                        {
                            var requestBuilder = new TwitterOAuthRequestBuilder(requestUri, tokens, "POST");

                            request.Headers.Authorization = AuthenticationHeaderValue.Parse(requestBuilder.AuthorizationHeader);

                            request.Content = multipartFormDataContent;

                            using (var response = await client.SendAsync(request).ConfigureAwait(false))
                            {
                                string jsonResult = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

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
