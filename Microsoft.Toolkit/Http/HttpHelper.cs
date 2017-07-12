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

using System.Collections.Concurrent;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Toolkit.Http
{
    /// <summary>
    /// This class exposes functionality of HttpClient through a singleton to take advantage of built-in connection pooling.
    /// </summary>
    public class HttpHelper
    {
        /// <summary>
        /// Maximum number of Http Clients that can be pooled.
        /// </summary>
        public const int DefaultPoolSize = 10;

        /// <summary>
        /// Private singleton field.
        /// </summary>
        private static HttpHelper _instance;

        private SemaphoreSlim _semaphore = null;

        /// <summary>
        /// Private instance field.
        /// </summary>
        private ConcurrentQueue<HttpClient> _httpClientQueue = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpHelper"/> class.
        /// </summary>
        public HttpHelper()
            : this(DefaultPoolSize)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpHelper"/> class.
        /// </summary>
        /// <param name="poolSize">number of HttpClient instances allowed</param>
        public HttpHelper(int poolSize)
        {
            _semaphore = new SemaphoreSlim(poolSize);
            _httpClientQueue = new ConcurrentQueue<HttpClient>();
        }

        /// <summary>
        /// Process Http Request using instance of HttpClient.
        /// </summary>
        /// <param name="request">instance of <see cref="HttpHelperRequest"/></param>
        /// <param name="cancellationToken">instance of <see cref="CancellationToken"/></param>
        /// <returns>Instane of <see cref="HttpHelperResponse"/></returns>
        public async Task<HttpHelperResponse> SendRequestAsync(HttpHelperRequest request, CancellationToken cancellationToken = default(CancellationToken))
        {
            await _semaphore.WaitAsync().ConfigureAwait(false);

            HttpClient client = null;

            try
            {
                var httpRequestMessage = request.ToHttpRequestMessage();

                client = GetHttpClientInstance();

                var response = await client.SendAsync(httpRequestMessage, cancellationToken).ConfigureAwait(false);

                FixInvalidCharset(response);

                return new HttpHelperResponse(response);
            }
            finally
            {
                // Add the HttpClient instance back to the queue.
                if (client != null)
                {
                    _httpClientQueue.Enqueue(client);
                }

                _semaphore.Release();
            }
        }

        /// <summary>
        /// Process Http Request using instance of HttpClient.
        /// </summary>
        /// <param name="request">instance of <see cref="HttpHelperRequest"/></param>
        /// <param name="cancellationToken">instance of <see cref="CancellationToken"/></param>
        /// <returns>Instane of <see cref="HttpHelperResponse"/></returns>
        public async Task<HttpHelperResponse> GetStreamAsync(HttpHelperRequest request)
        {
            await _semaphore.WaitAsync().ConfigureAwait(false);

            HttpClient client = null;

            try
            {
                var httpRequestMessage = request.ToHttpRequestMessage();

                client = GetHttpClientInstance();

                SetDefaultHeaders(client, httpRequestMessage.Headers);

                var response = await client.GetStreamAsync(httpRequestMessage.RequestUri).ConfigureAwait(false);

                return new HttpHelperResponse(new HttpResponseMessage { StatusCode = System.Net.HttpStatusCode.OK,  Content = new StreamContent(response) });
            }
            finally
            {
                // Add the HttpClient instance back to the queue.
                if (client != null)
                {
                    // Clean up default request headers
                    client.DefaultRequestHeaders.Clear();

                    _httpClientQueue.Enqueue(client);
                }

                _semaphore.Release();
            }
        }

        private void SetDefaultHeaders(HttpClient client, HttpRequestHeaders requestHeaders)
        {
            if (requestHeaders != null)
            {
                var enumerableHeaders = requestHeaders.ToArray();
                foreach (var header in enumerableHeaders)
                {
                    client.DefaultRequestHeaders.Add(header.Key, header.Value);
                }
            }
        }

        private HttpClient GetHttpClientInstance()
        {
            HttpClient client = null;

            // Try and get HttpClient from the queue
            if (!_httpClientQueue.TryDequeue(out client))
            {
                client = new HttpClient();
            }

            return client;
        }

        /// <summary>
        /// Fix invalid charset returned by some web sites.
        /// </summary>
        /// <param name="response">HttpResponseMessage instance.</param>
        private void FixInvalidCharset(HttpResponseMessage response)
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
