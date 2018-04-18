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
    /// Provides data for the <see cref="IWebView.NavigationCompleted"/> and <see cref="IWebView.FrameNavigationCompleted"/> events. This class cannot be inherited.
    /// </summary>
    /// <remarks>Copy from <see cref="Windows.Web.UI.WebViewControlNavigationCompletedEventArgs"/> to avoid requirement to link Windows.winmd</remarks>
    /// <seealso cref="System.EventArgs" />
    /// <seealso cref="Windows.Web.UI.WebViewControlNavigationCompletedEventArgs"/>
    public sealed class WebViewControlNavigationCompletedEventArgs : EventArgs
    {
        internal WebViewControlNavigationCompletedEventArgs(Windows.Web.UI.WebViewControlNavigationCompletedEventArgs args)
        {
            IsSuccess = args.IsSuccess;
            Uri = args.Uri;
            WebErrorStatus = (WebErrorStatus)args.WebErrorStatus;
        }

        internal WebViewControlNavigationCompletedEventArgs(Windows.Web.UI.WebViewControlNavigationCompletedEventArgs args, Uri uri)
        : this(args)
        {
            Uri = uri;
        }

        /// <summary>
        /// Gets a value indicating whether the navigation completed successfully.
        /// </summary>
        /// <value><see langword="true" /> if the navigation completed successfully; otherwise, <see langword="false" />.</value>
        public bool IsSuccess { get; }

        /// <summary>
        /// Gets the Uniform Resource Identifier (URI) of the navigated <see cref="IWebView"/> content.
        /// </summary>
        /// <value>The URI.</value>
        public Uri Uri { get; }

        /// <summary>
        /// Gets the reason the navigation was successful.
        /// </summary>
        /// <value>The web error status.</value>
        /// <seealso cref="IsSuccess"/>
        public WebErrorStatus WebErrorStatus { get; }

        /// <summary>
        /// Performs an implicit conversion from <see cref="Windows.Web.UI.WebViewControlNavigationCompletedEventArgs"/> to <see cref="WebViewControlNavigationCompletedEventArgs"/>.
        /// </summary>
        /// <param name="args">The <see cref="Windows.Web.UI.WebViewControlNavigationCompletedEventArgs"/> instance containing the event data.</param>
        /// <returns>The result of the conversion.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates")]
        public static implicit operator WebViewControlNavigationCompletedEventArgs(Windows.Web.UI.WebViewControlNavigationCompletedEventArgs args) => new WebViewControlNavigationCompletedEventArgs(args);
    }
}