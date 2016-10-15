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
using System.Collections.Generic;
using Windows.Web.Http;
using Windows.Web.Http.Headers;

namespace Microsoft.Toolkit.Uwp
{
    /// <summary>
    /// HttpHelperRequest for holding request settings.
    /// </summary>
    public class HttpHelperRequest : IDisposable
    {
        private HttpRequestMessage requestMessage = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpHelperRequest"/> class.
        /// Default constructor.
        /// </summary>
        /// <param name="uri">Uri for the resource</param>
        public HttpHelperRequest(Uri uri)
            : this(uri, HttpMethod.Get)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpHelperRequest"/> class.
        /// Default constructor.
        /// </summary>
        /// <param name="uri">Uri for the resource</param>
        /// <param name="method">Method to use when making the request</param>
        public HttpHelperRequest(Uri uri, HttpMethod method)
        {
            Headers = new Dictionary<string, string>();

            requestMessage = new HttpRequestMessage(method, uri);
        }

        /// <summary>
        /// Gets the http reqeust method.
        /// </summary>
        public HttpMethod Method
        {
            get { return requestMessage.Method; }
        }

        /// <summary>
        /// Gets the request Uri.
        /// </summary>
        public Uri RequestedUri
        {
            get { return requestMessage.RequestUri; }
        }

        /// <summary>
        /// Gets the accept header collection for the request.
        /// </summary>
        public HttpMediaTypeWithQualityHeaderValueCollection Accept
        {
            get { return requestMessage.Headers.Accept; }
        }

        /// <summary>
        /// Gets the accept encoding header collection for the request.
        /// </summary>
        public HttpContentCodingWithQualityHeaderValueCollection AcceptEncoding
        {
            get { return requestMessage.Headers.AcceptEncoding; }
        }

        /// <summary>
        /// Gets the accept language header for the request.
        /// </summary>
        public HttpLanguageRangeWithQualityHeaderValueCollection AcceptLanguage
        {
            get { return requestMessage.Headers.AcceptLanguage; }
        }

        /// <summary>
        /// Gets the connection header for the request.
        /// </summary>
        public HttpConnectionOptionHeaderValueCollection Connection
        {
            get { return requestMessage.Headers.Connection; }
        }

        /// <summary>
        /// Gets or sets the If-Modified-Since header for the request.
        /// </summary>
        public DateTimeOffset? IfModifiedSince
        {
            get { return requestMessage.Headers.IfModifiedSince; }
            set { requestMessage.Headers.IfModifiedSince = value; }
        }

        /// <summary>
        /// Gets or sets the referer header for the request.
        /// </summary>
        public Uri Referer
        {
            get { return requestMessage.Headers.Referer; }
            set { requestMessage.Headers.Referer = value; }
        }

        /// <summary>
        /// Gets User Agent to pass to the request.
        /// </summary>
        public HttpProductInfoHeaderValueCollection UserAgent
        {
            get { return requestMessage.Headers.UserAgent; }
        }

        /// <summary>
        /// Gets or sets authorization related credentials to the request
        /// </summary>
        public HttpCredentialsHeaderValue Authorization
        {
            get { return requestMessage.Headers.Authorization; }
            set { requestMessage.Headers.Authorization = value; }
        }

        /// <summary>
        /// Gets collection of headers to pass with the request.
        /// </summary>
        public Dictionary<string, string> Headers { get; private set; }

        /// <summary>
        /// Gets or sets holds request Result.
        /// </summary>
        public IHttpContent Content { get; set; }

        /// <summary>
        /// Creates HttpRequestMessage using the data.
        /// </summary>
        /// <returns>Instance of <see cref="HttpRequestMessage"/></returns>
        public HttpRequestMessage ToHttpRequestMessage()
        {
            if (Headers != null && Headers.Count > 0)
            {
                foreach (var pair in Headers)
                {
                    if (pair.Key.Equals(nameof(HttpRequestHeaderCollection.Accept), StringComparison.OrdinalIgnoreCase))
                    {
                        Accept.ParseAdd(pair.Value);
                    }
                    else if (pair.Key.Equals(nameof(HttpRequestHeaderCollection.AcceptEncoding), StringComparison.OrdinalIgnoreCase))
                    {
                        AcceptEncoding.ParseAdd(pair.Value);
                    }
                    else if (pair.Key.Equals(nameof(HttpRequestHeaderCollection.AcceptLanguage), StringComparison.OrdinalIgnoreCase))
                    {
                        AcceptLanguage.ParseAdd(pair.Value);
                    }
                    else if (pair.Key.Equals(nameof(HttpRequestHeaderCollection.Authorization), StringComparison.OrdinalIgnoreCase))
                    {
                        Authorization = new HttpCredentialsHeaderValue(nameof(HttpRequestHeaderCollection.Authorization), pair.Value);
                    }
                    else if (pair.Key.Equals(nameof(HttpRequestHeaderCollection.Connection), StringComparison.OrdinalIgnoreCase))
                    {
                        Connection.ParseAdd(pair.Value);
                    }
                    else if (pair.Key.Equals(nameof(HttpRequestHeaderCollection.IfModifiedSince), StringComparison.OrdinalIgnoreCase))
                    {
                        IfModifiedSince = new DateTimeOffset(DateTime.Parse(pair.Value));
                    }
                    else if (pair.Key.Equals(nameof(HttpRequestHeaderCollection.Referer)))
                    {
                        Referer = new Uri(pair.Value);
                    }
                    else if (pair.Key.Equals(nameof(HttpRequestHeaderCollection.UserAgent), StringComparison.OrdinalIgnoreCase))
                    {
                        UserAgent.ParseAdd(pair.Value);
                    }
                    else
                    {
                        requestMessage.Headers[pair.Key] = pair.Value;
                    }
                }
            }

            if (Content != null)
            {
                requestMessage.Content = Content;
            }

            return requestMessage;
        }

        /// <summary>
        /// Dispose underlying content
        /// </summary>
        public void Dispose()
        {
            try
            {
                Content?.Dispose();
            }
            catch (ObjectDisposedException)
            {
                // known issue
                // http://stackoverflow.com/questions/39109060/httpmultipartformdatacontent-dispose-throws-objectdisposedexception
            }
        }
    }
}
