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
using System.Net;

namespace Microsoft.Toolkit.Uwp.Services.Core
{
    /// <summary>
    /// HttpRequestSettings for holding request settings.
    /// </summary>
    internal class HttpRequestSettings
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HttpRequestSettings"/> class.
        /// Default constructor.
        /// </summary>
        public HttpRequestSettings()
        {
            Headers = new WebHeaderCollection();
        }

        /// <summary>
        /// Gets or sets request Uri.
        /// </summary>
        public Uri RequestedUri { get; set; }

        /// <summary>
        /// Gets or sets user Agent to pass to request.
        /// </summary>
        public string UserAgent { get; set; }

        /// <summary>
        /// Gets collection of headers to pass with request.
        /// </summary>
        public WebHeaderCollection Headers { get; private set; }
    }
}
