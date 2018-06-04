// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Security;

namespace Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT
{
    /// <summary>
    /// Provides data for the <see cref="IWebView.PermissionRequested"/> event. This class cannot be inherited.
    /// </summary>
    /// <remarks>Copy from <see cref="Windows.Web.UI.WebViewControlPermissionRequestedEventArgs"/> to avoid requirement to link Windows.winmd</remarks>
    /// <seealso cref="System.EventArgs" />
    /// <seealso cref="Windows.Web.UI.WebViewControlPermissionRequestedEventArgs"/>
    public sealed class WebViewControlPermissionRequestedEventArgs : EventArgs
    {
        [SecurityCritical]
        private readonly Windows.Web.UI.WebViewControlPermissionRequestedEventArgs _args;

        internal WebViewControlPermissionRequestedEventArgs(Windows.Web.UI.WebViewControlPermissionRequestedEventArgs args)
        {
            _args = args;
        }

        /// <summary>
        /// Gets the <see cref="WebViewControlPermissionRequest"/> object that contains information about the request.
        /// </summary>
        /// <value>The permission request.</value>
        public WebViewControlPermissionRequest PermissionRequest => _args.PermissionRequest;

        /// <summary>
        /// Performs an implicit conversion from <see cref="Windows.Web.UI.WebViewControlPermissionRequestedEventArgs"/> to <see cref="WebViewControlPermissionRequestedEventArgs"/>.
        /// </summary>
        /// <param name="args">The <see cref="Windows.Web.UI.WebViewControlPermissionRequestedEventArgs"/> instance containing the event data.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator WebViewControlPermissionRequestedEventArgs(Windows.Web.UI.WebViewControlPermissionRequestedEventArgs args) => ToWebViewControlPermissionRequestedEventArgs(args);

        /// <summary>
        /// Creates a <see cref="WebViewControlPermissionRequestedEventArgs"/> from <see cref="Windows.Web.UI.WebViewControlPermissionRequestedEventArgs"/>.
        /// </summary>
        /// <param name="args">The <see cref="Windows.Web.UI.WebViewControlPermissionRequestedEventArgs"/> instance containing the event data.</param>
        /// <returns><see cref="WebViewControlPermissionRequestedEventArgs"/></returns>
        public static WebViewControlPermissionRequestedEventArgs ToWebViewControlPermissionRequestedEventArgs(
            Windows.Web.UI.WebViewControlPermissionRequestedEventArgs args) =>
            new WebViewControlPermissionRequestedEventArgs(args);
    }
}