// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Security;
using System.Windows.Forms;

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
#if WPF
        [SecurityCritical]
        private readonly System.Windows.Navigation.NavigatingCancelEventArgs _wpfArgs;
#endif
#if WINFORMS
        [SecurityCritical]
        private readonly WebBrowserNavigatingEventArgs _formsArgs;
#endif

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

#if WPF
        [SecurityCritical]
        internal WebViewControlNavigationStartingEventArgs(System.Windows.Navigation.NavigatingCancelEventArgs args)
        {
            _wpfArgs = args;
            Uri = args.Uri;
        }
#endif
#if WINFORMS
        [SecurityCritical]
        internal WebViewControlNavigationStartingEventArgs(WebBrowserNavigatingEventArgs e)
        {
            _formsArgs = e;
            Uri = e.Url;
        }
#endif

        /// <summary>
        /// Gets or sets a value indicating whether to cancel the <see cref="IWebView"/> navigation.
        /// </summary>
        /// <value><see langword="true" /> if cancel; otherwise, <see langword="false" />.</value>
        public bool Cancel
        {
            [SecurityCritical]
            get
            {
#if WINFORMS
                return _args?.Cancel ?? _formsArgs?.Cancel ?? false;
#elif WPF

                return _args?.Cancel ?? _wpfArgs?.Cancel ?? false;
#else
                return _args?.Cancel ?? false;
#endif
            }

            [SecurityCritical]
            set
            {
                if (_args != null)
                {
                    _args.Cancel = value;
                }
#if WINFORMS
                else if (_formsArgs != null)
                {
                    _formsArgs.Cancel = value;
                }
#endif
#if WPF
                else if (_wpfArgs != null)
                {
                    _wpfArgs.Cancel = value;
                }
#endif
            }
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
#if WPF
        /// <summary>
        /// Performs an implicit conversion from <see cref="System.Windows.Navigation.NavigatingCancelEventArgs"/> to <see cref="WebViewControlNavigationStartingEventArgs"/>.
        /// </summary>
        /// <param name="args">The <see cref="System.Windows.Navigation.NavigatingCancelEventArgs"/> instance containing the event data.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator WebViewControlNavigationStartingEventArgs(System.Windows.Navigation.NavigatingCancelEventArgs args) => ToWebViewControlNavigationStartingEventArgs(args);

        /// <summary>
        /// Creates a <see cref="WebViewControlNavigationStartingEventArgs"/> from <see cref="System.Windows.Navigation.NavigatingCancelEventArgs"/>.
        /// </summary>
        /// <param name="args">The <see cref="System.Windows.Navigation.NavigatingCancelEventArgs"/> instance containing the event data.</param>
        /// <returns><see cref="WebViewControlNavigationStartingEventArgs"/>.</returns>
        public static WebViewControlNavigationStartingEventArgs ToWebViewControlNavigationStartingEventArgs(
            System.Windows.Navigation.NavigatingCancelEventArgs args) =>
            new WebViewControlNavigationStartingEventArgs(args);
#endif
#if WINFORMS
        /// <summary>
        /// Performs an implicit conversion from <see cref="WebBrowserNavigatingEventArgs"/> to <see cref="WebViewControlNavigationStartingEventArgs"/>.
        /// </summary>
        /// <param name="args">The <see cref="WebBrowserNavigatingEventArgs"/> instance containing the event data.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator WebViewControlNavigationStartingEventArgs(WebBrowserNavigatingEventArgs args) => ToWebViewControlNavigationStartingEventArgs(args);

        /// <summary>
        /// Creates a <see cref="WebViewControlNavigationStartingEventArgs"/> from <see cref="WebBrowserNavigatingEventArgs"/>.
        /// </summary>
        /// <param name="args">The <see cref="WebBrowserNavigatingEventArgs"/> instance containing the event data.</param>
        /// <returns><see cref="WebViewControlNavigationStartingEventArgs"/>.</returns>
        public static WebViewControlNavigationStartingEventArgs ToWebViewControlNavigationStartingEventArgs(
            WebBrowserNavigatingEventArgs args) =>
            new WebViewControlNavigationStartingEventArgs(args);
#endif
    }
}