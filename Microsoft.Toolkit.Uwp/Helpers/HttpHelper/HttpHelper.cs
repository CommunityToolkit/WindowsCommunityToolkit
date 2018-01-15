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
using System.Diagnostics;
using System.IO;
using System.Net.Http.Headers;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Windows.Web.Http;
using Windows.Web.Http.Filters;

namespace Microsoft.Toolkit.Uwp
{
    /// <summary>
    /// This class exposes functionality of HttpClient through a singleton to take advantage of built-in connection pooling.
    /// </summary>
    [Obsolete]
    public class HttpHelper
    {
        /// <summary>
        /// Maximum number of Http Clients that can be pooled.
        /// </summary>
        private const int DefaultPoolSize = 10;

        /// <summary>
        /// Private singleton field.
        /// </summary>
        private static HttpHelper _instance;

        private SemaphoreSlim _semaphore = null;

        /// <summary>
        /// Private instance field.
        /// </summary>
        private ConcurrentQueue<HttpClient> _httpClientQueue = null;

        private IHttpFilter _httpFilter = null;

        /// <summary>
        /// Gets public singleton property.
        /// </summary>
        public static HttpHelper Instance => _instance ?? (_instance = new HttpHelper());

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpHelper"/> class.
        /// </summary>
        public HttpHelper()
            : this(DefaultPoolSize, GetDefaultFilter())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpHelper"/> class.
        /// </summary>
        /// <param name="poolSize">number of HttpClient instances allowed</param>
        /// <param name="httpFilter">HttpFilter to use when instances of HttpClient are created</param>
        public HttpHelper(int poolSize, IHttpFilter httpFilter)
        {
            _httpFilter = httpFilter;
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

                var response = await client.SendRequestAsync(httpRequestMessage).AsTask(cancellationToken).ConfigureAwait(false);

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
        public async Task<HttpHelperResponse> GetInputStreamAsync(HttpHelperRequest request, CancellationToken cancellationToken = default(CancellationToken))
        {
            await _semaphore.WaitAsync().ConfigureAwait(false);

            HttpClient client = null;

            try
            {
                var httpRequestMessage = request.ToHttpRequestMessage();

                client = GetHttpClientInstance();
                foreach (var header in request.Headers)
                {
                    client.DefaultRequestHeaders[header.Key] = header.Value;
                }

                var response = await client.GetInputStreamAsync(httpRequestMessage.RequestUri).AsTask(cancellationToken).ConfigureAwait(false);

                return new HttpHelperResponse(new HttpResponseMessage { Content = new HttpStreamContent(response) });
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

        private static IHttpFilter GetDefaultFilter()
        {
            var filter = new HttpBaseProtocolFilter();
            filter.CacheControl.ReadBehavior = HttpCacheReadBehavior.MostRecent;

            return filter;
        }

        private HttpClient GetHttpClientInstance()
        {
            HttpClient client = null;

            // Try and get HttpClient from the queue
            if (!_httpClientQueue.TryDequeue(out client))
            {
                client = new HttpClient(_httpFilter);
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
