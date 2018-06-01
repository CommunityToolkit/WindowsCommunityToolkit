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
    /// Provides data for the <see cref="IWebView.NavigationStarting"/> and <see cref="IWebView.FrameNavigationStarting"/> events. This class cannot be inherited.
    /// </summary>
    /// <remarks>Copy from <see cref="Windows.Web.UI.WebViewControlNavigationStartingEventArgs"/> to avoid requirement to link Windows.winmd</remarks>
    /// <seealso cref="System.EventArgs" />
    /// <seealso cref="Windows.Web.UI.WebViewControlNavigationStartingEventArgs"/>
    public sealed class WebViewControlNavigationStartingEventArgs : EventArgs
    {
        [SecurityCritical]
        private readonly Windows.Web.UI.WebViewControlNavigationStartingEventArgs _args;

        [SecurityCritical]
        internal WebViewControlNavigationStartingEventArgs(Windows.Web.UI.WebViewControlNavigationStartingEventArgs args)
        {
            _args = args;
            Uri = args.Uri;
        }

        [SecurityCritical]
        internal WebViewControlNavigationStartingEventArgs(Windows.Web.UI.WebViewControlNavigationStartingEventArgs args, Uri uri)
            : this(args)
        {
            Uri = uri;
        }

        /// <summary>
        /// Gets or sets a value indicating whether to cancel the <see cref="IWebView"/> navigation.
        /// </summary>
        /// <value><see langword="true" /> if cancel; otherwise, <see langword="false" />.</value>
        public bool Cancel
        {
            [SecurityCritical]
            get => _args.Cancel;
            [SecurityCritical]
            set => _args.Cancel = value;
        }

        /// <summary>
        /// Gets the Uniform Resource Identifier (URI) of the content the <see cref="IWebView"/> is loading.
        /// </summary>
        /// <value>The Uniform Resource Identifier (URI) of the content the <see cref="IWebView"/> is loading.</value>
        public Uri Uri { get; }

        /// <summary>
        /// Performs an implicit conversion from <see cref="Windows.Web.UI.WebViewControlNavigationStartingEventArgs"/> to <see cref="WebViewControlNavigationStartingEventArgs"/>.
        /// </summary>
        /// <param name="args">The <see cref="Windows.Web.UI.WebViewControlNavigationStartingEventArgs"/> instance containing the event data.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator WebViewControlNavigationStartingEventArgs(Windows.Web.UI.WebViewControlNavigationStartingEventArgs args) => ToWebViewControlNavigationStartingEventArgs(args);

        /// <summary>
        /// Creates a <see cref="WebViewControlNavigationStartingEventArgs"/> from <see cref="Windows.Web.UI.WebViewControlNavigationStartingEventArgs"/>.
        /// </summary>
        /// <param name="args">The <see cref="Windows.Web.UI.WebViewControlNavigationStartingEventArgs"/> instance containing the event data.</param>
        /// <returns><see cref="WebViewControlNavigationStartingEventArgs"/>.</returns>
        public static WebViewControlNavigationStartingEventArgs ToWebViewControlNavigationStartingEventArgs(
            Windows.Web.UI.WebViewControlNavigationStartingEventArgs args) =>
            new WebViewControlNavigationStartingEventArgs(args);
    }
}