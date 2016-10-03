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
            RequestedUri = uri;
            Headers = new Dictionary<string, string>();
            Method = method;
        }

        /// <summary>
        /// Gets or sets reqeust method.
        /// </summary>
        public HttpMethod Method { get; set; }

        /// <summary>
        /// Gets or sets request Uri.
        /// </summary>
        public Uri RequestedUri { get; set; }

        /// <summary>
        /// Gets or sets User Agent to pass to the request.
        /// </summary>
        public string UserAgent { get; set; }

        /// <summary>
        /// Gets or sets authorization related credentials to the request
        /// </summary>
        public HttpCredentialsHeaderValue Authorization { get; set; }

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
            HttpRequestMessage request = new HttpRequestMessage(Method, RequestedUri);

            if (!string.IsNullOrWhiteSpace(UserAgent))
            {
                request.Headers.UserAgent.ParseAdd(UserAgent);
            }

            if (Authorization != null)
            {
                request.Headers.Authorization = Authorization;
            }

            if (Headers != null && Headers.Count > 0)
            {
                foreach (var key in Headers.Keys)
                {
                    if (!string.IsNullOrEmpty(Headers[key]))
                    {
                        request.Headers[key] = Headers[key];
                    }
                }
            }

            if (Content != null)
            {
                request.Content = Content;
            }

            return request;
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
