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
    /// Represents an HTTP request message including headers.
    /// </summary>
    [Obsolete]
    public class HttpHelperRequest : IDisposable
    {
        private HttpRequestMessage _requestMessage = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpHelperRequest"/> class with Uri and HTTP GET Method.
        /// </summary>
        /// <param name="uri">Uri for the resource</param>
        public HttpHelperRequest(Uri uri)
            : this(uri, HttpMethod.Get)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpHelperRequest"/> class with Uri and HTTP method.
        /// </summary>
        /// <param name="uri">Uri for the resource</param>
        /// <param name="method">Method to use when making the request</param>
        public HttpHelperRequest(Uri uri, HttpMethod method)
        {
            _requestMessage = new HttpRequestMessage(method, uri);
        }

        /// <summary>
        /// Gets the http reqeust method.
        /// </summary>
        public HttpMethod Method
        {
            get { return _requestMessage.Method; }
        }

        /// <summary>
        /// Gets the request Uri.
        /// </summary>
        public Uri RequestUri
        {
            get { return _requestMessage.RequestUri; }
        }

        /// <summary>
        /// Gets HTTP header collection from the underlying HttpRequestMessage.
        /// </summary>
        public HttpRequestHeaderCollection Headers
        {
            get
            {
                return _requestMessage.Headers;
            }
        }

        /// <summary>
        /// Gets or sets holds request Result.
        /// </summary>
        public IHttpContent Content
        {
            get
            {
                return _requestMessage.Content;
            }

            set
            {
                _requestMessage.Content = value;
            }
        }

        /// <summary>
        /// Creates HttpRequestMessage using the data.
        /// </summary>
        /// <returns>Instance of <see cref="HttpRequestMessage"/></returns>
        public HttpRequestMessage ToHttpRequestMessage()
        {
            return _requestMessage;
        }

        /// <summary>
        /// Dispose underlying content
        /// </summary>
        public void Dispose()
        {
            try
            {
                _requestMessage.Dispose();
            }
            catch (ObjectDisposedException)
            {
                // known issue
                // http://stackoverflow.com/questions/39109060/httpmultipartformdatacontent-dispose-throws-objectdisposedexception
            }
        }
    }
}
