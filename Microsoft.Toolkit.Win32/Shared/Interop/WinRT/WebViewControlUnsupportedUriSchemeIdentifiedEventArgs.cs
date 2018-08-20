// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Security;

namespace Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT
{
    /// <summary>
    /// Provides data for the <see cref="IWebView.UnsupportedUriSchemeIdentified" /> event. This class cannot be inherited.
    /// </summary>
    /// <remarks>Copy from <see cref="Windows.Web.UI.WebViewControlUnsupportedUriSchemeIdentifiedEventArgs"/> to avoid requirement to link Windows.winmd</remarks>
    /// <seealso cref="System.EventArgs" />
    /// <seealso cref="Windows.Web.UI.WebViewControlUnsupportedUriSchemeIdentifiedEventArgs"/>
    public sealed class WebViewControlUnsupportedUriSchemeIdentifiedEventArgs : EventArgs
    {
        [SecurityCritical]
        private readonly Windows.Web.UI.WebViewControlUnsupportedUriSchemeIdentifiedEventArgs _args;

        internal WebViewControlUnsupportedUriSchemeIdentifiedEventArgs(Windows.Web.UI.WebViewControlUnsupportedUriSchemeIdentifiedEventArgs args)
        {
            _args = args ?? throw new ArgumentNullException(nameof(args));
        }

        /// <summary>
        /// Gets or sets a value indicating whether the routed event is handled. A <see langword="true" /> value for Handled prevents other handlers along the event route from handling the same event again.
        /// </summary>
        /// <value><see langword="true" /> to mark the routed event handled. <see langword="false" /> to leave the routed event unhandled, which permits the event to potentially route further and be acted on by other handlers. The default is <see langword="true" />.</value>
        public bool Handled
        {
            get => _args.Handled;
            set => _args.Handled = value;
        }

        /// <summary>
        /// Gets the Uniform Resource Identifier (URI) of the content the <see cref="IWebView" /> attempted to navigate to.
        /// </summary>
        /// <value>The Uniform Resource Identifier (URI) of the content.</value>
        public Uri Uri => _args.Uri;

        /// <summary>
        /// Performs an implicit conversion from <see cref="Windows.Web.UI.WebViewControlUnsupportedUriSchemeIdentifiedEventArgs"/> to <see cref="WebViewControlUnsupportedUriSchemeIdentifiedEventArgs"/>.
        /// </summary>
        /// <param name="args">The <see cref="Windows.Web.UI.WebViewControlUnsupportedUriSchemeIdentifiedEventArgs"/> instance containing the event data.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator WebViewControlUnsupportedUriSchemeIdentifiedEventArgs(Windows.Web.UI.WebViewControlUnsupportedUriSchemeIdentifiedEventArgs args) => ToWebViewControlUnsupportedUriSchemeIdentifiedEventArgs(args);

        /// <summary>
        /// Creates a <see cref="WebViewControlUnsupportedUriSchemeIdentifiedEventArgs"/> from <see cref="Windows.Web.UI.WebViewControlUnsupportedUriSchemeIdentifiedEventArgs"/>.
        /// </summary>
        /// <param name="args">The <see cref="Windows.Web.UI.WebViewControlUnsupportedUriSchemeIdentifiedEventArgs"/> instance containing the event data.</param>
        /// <returns><see cref="WebViewControlUnsupportedUriSchemeIdentifiedEventArgs"/></returns>
        public static WebViewControlUnsupportedUriSchemeIdentifiedEventArgs ToWebViewControlUnsupportedUriSchemeIdentifiedEventArgs(
            Windows.Web.UI.WebViewControlUnsupportedUriSchemeIdentifiedEventArgs args) =>
            new WebViewControlUnsupportedUriSchemeIdentifiedEventArgs(args);
    }
}