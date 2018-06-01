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
using System.Security;

namespace Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT
{
    /// <summary>
    /// Provides data for the <see cref="IWebView.ContentLoading"/> and <see cref="IWebView.FrameContentLoading"/> events. This class cannot be inherited.
    /// </summary>
    /// <remarks>Copy from <see cref="Windows.Web.UI.WebViewControlContentLoadingEventArgs"/> to avoid requirement to link Windows.winmd</remarks>
    /// <seealso cref="Windows.Web.UI.WebViewControlContentLoadingEventArgs"/>
    public sealed class WebViewControlContentLoadingEventArgs : EventArgs
    {
        [SecurityCritical]
        private readonly Windows.Web.UI.WebViewControlContentLoadingEventArgs _args;

        [SecurityCritical]
        internal WebViewControlContentLoadingEventArgs(Windows.Web.UI.WebViewControlContentLoadingEventArgs args)
        {
            _args = args;
        }

        /// <summary>
        /// Gets the Uniform Resource Identifier (URI) of the content the <see cref="IWebView"/> is loading.
        /// </summary>
        public Uri Uri
        {
            [SecurityCritical]
            get { return _args.Uri; }
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="Windows.Web.UI.WebViewControlContentLoadingEventArgs"/> to <see cref="WebViewControlContentLoadingEventArgs"/>.
        /// </summary>
        /// <param name="args">The <see cref="Windows.Web.UI.WebViewControlContentLoadingEventArgs"/> instance containing the event data.</param>
        /// <returns>The result of the conversion.</returns>
        [SecurityCritical]
        public static implicit operator WebViewControlContentLoadingEventArgs(
            Windows.Web.UI.WebViewControlContentLoadingEventArgs args)
        {
            return FromWebViewControlContentLoadingEventArgs(args);
        }

        /// <summary>
        /// Creates a <see cref="WebViewControlContentLoadingEventArgs"/> from <see cref="Windows.Web.UI.WebViewControlContentLoadingEventArgs"/>.
        /// </summary>
        /// <param name="args">The <see cref="Windows.Web.UI.WebViewControlContentLoadingEventArgs"/> instance containing the event data.</param>
        /// <returns><see cref="WebViewControlContentLoadingEventArgs"/></returns>
        public static WebViewControlContentLoadingEventArgs FromWebViewControlContentLoadingEventArgs(Windows.Web.UI.WebViewControlContentLoadingEventArgs args)
        {
            return new WebViewControlContentLoadingEventArgs(args);
        }
    }
}