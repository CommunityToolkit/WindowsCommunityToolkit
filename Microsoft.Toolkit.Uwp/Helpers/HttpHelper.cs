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
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Windows.Web.Http;
using Windows.Web.Http.Filters;

namespace Microsoft.Toolkit.Uwp
{
    /// <summary>
    /// This class exposes functionality of HttpClient through a singleton to take advantage of built-in connection pooling.
    /// </summary>
    public class HttpHelper
    {
        /// <summary>
        /// Maximum number of Http Clients that can be pooled.
        /// </summary>
        private const int MaxPoolSize = 10;

        /// <summary>
        /// Private singleton field.
        /// </summary>
        private static HttpHelper _instance;

        private ManualResetEventSlim resetEvent = new ManualResetEventSlim();

        /// <summary>
        /// Private instance field.
        /// </summary>
        private ConcurrentQueue<HttpClient> _httpClientQueue = null;

        /// <summary>
        /// count of how many HttpClient instances have been created.
        /// </summary>
        private int _queueCount = 0;

        /// <summary>
        /// Gets public singleton property.
        /// </summary>
        public static HttpHelper Instance => _instance ?? (_instance = new HttpHelper());

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpHelper"/> class.
        /// </summary>
        protected HttpHelper()
        {
            _httpClientQueue = new ConcurrentQueue<HttpClient>();
        }

        /// <summary>
        /// Process Http Request using instance of HttpClient.
        /// </summary>
        /// <param name="request">instance of <see cref="HttpHelperRequest"/></param>
        /// <returns>Instane of <see cref="HttpHelperResponse"/></returns>
        public async Task<HttpHelperResponse> SendRequestAsync(HttpHelperRequest request)
        {
            var httpRequestMessage = request.ToHttpRequestMessage();

            var client = await GetHttpClientInstance();

            var response = await client.SendRequestAsync(httpRequestMessage).AsTask().ConfigureAwait(false);

            // Add the HttpClient instance back to the queue.
            _httpClientQueue.Enqueue(client);

            FixInvalidCharset(response);

            return new HttpHelperResponse(response);
        }

        private async Task<HttpClient> GetHttpClientInstance()
        {
            HttpClient client;

            // Try and get HttpClient from the queue
            if (!_httpClientQueue.TryDequeue(out client))
            {
                if (_queueCount == MaxPoolSize)
                {
                    do
                    {
                        // HttpClient connection queue all instances in use.. Wait
                        resetEvent.Wait(50);
                    }
                    while (!_httpClientQueue.TryDequeue(out client));  // HttpClient connection queue free instance found
                }
                else
                {
                    var filter = new HttpBaseProtocolFilter();
                    filter.CacheControl.ReadBehavior = HttpCacheReadBehavior.MostRecent;

                    client = new HttpClient(filter);

                    // Increment HttpClient connection queue count.
                    Interlocked.Increment(ref _queueCount);
                }
            }

            return client;
        }

        /// <summary>
        /// Fix invalid charset returned by some web sites.
        /// </summary>
        /// <param name="response">HttpResponseMessage instance.</param>
        private static void FixInvalidCharset(HttpResponseMessage response)
        {
            if (response != null && response.Content != null && response.Content.Headers != null
                && response.Content.Headers.ContentType != null && response.Content.Headers.ContentType.CharSet != null)
            {
                // Fix invalid charset returned by some web sites.
                string charset = response.Content.Headers.ContentType.CharSet;
                if (charset.Contains("\""))
                {
                    response.Content.Headers.ContentType.CharSet = charset.Replace("\"", string.Empty);
                }
            }
        }
    }
}
