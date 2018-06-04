// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Security;

namespace Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT
{
    /// <summary>
    /// CProvides data for the <see cref="IWebView.UnviewableContentIdentified" /> event. This class cannot be inherited.
    /// </summary>
    /// <remarks>Copy from <see cref="Windows.Web.UI.WebViewControlUnviewableContentIdentifiedEventArgs"/> to avoid requirement to link Windows.winmd</remarks>
    /// <seealso cref="System.EventArgs" />
    /// <seealso cref="Windows.Web.UI.WebViewControlUnviewableContentIdentifiedEventArgs"/>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Unviewable", Justification = "Same as WinRT type")]
    public sealed class WebViewControlUnviewableContentIdentifiedEventArgs : EventArgs
    {
        [SecurityCritical]
        private readonly Windows.Web.UI.WebViewControlUnviewableContentIdentifiedEventArgs _args;

        internal WebViewControlUnviewableContentIdentifiedEventArgs(Windows.Web.UI.WebViewControlUnviewableContentIdentifiedEventArgs args)
        {
            _args = args ?? throw new ArgumentNullException(nameof(args));
        }

        /// <summary>
        /// Gets the Uniform Resource Identifier (URI) of the content the <see cref="IWebView"/> attempted to load.
        /// </summary>
        /// <value>The URI.</value>
        public Uri Uri => _args.Uri;

        /// <summary>
        /// Gets the Uniform Resource Identifier (URI) of the page that contains the link to the unviewable content.
        /// </summary>
        /// <value>The referrer.</value>
        public Uri Referrer => _args.Referrer;

        /// <summary>
        /// Gets the media type of the content that cannot be viewed.
        /// </summary>
        /// <value>The type of the media.</value>
        public string MediaType => _args.MediaType;

        /// <summary>
        /// Performs an implicit conversion from <see cref="Windows.Web.UI.WebViewControlUnviewableContentIdentifiedEventArgs"/> to <see cref="WebViewControlUnviewableContentIdentifiedEventArgs"/>.
        /// </summary>
        /// <param name="args">The <see cref="Windows.Web.UI.WebViewControlUnviewableContentIdentifiedEventArgs"/> instance containing the event data.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator WebViewControlUnviewableContentIdentifiedEventArgs(Windows.Web.UI.WebViewControlUnviewableContentIdentifiedEventArgs args) => ToWebViewControlUnviewableContentIdentifiedEventArgs(args);

        /// <summary>
        /// Creates a <see cref="WebViewControlUnviewableContentIdentifiedEventArgs"/> from <see cref="Windows.Web.UI.WebViewControlUnviewableContentIdentifiedEventArgs"/>.
        /// </summary>
        /// <param name="args">The <see cref="Windows.Web.UI.WebViewControlUnviewableContentIdentifiedEventArgs"/> instance containing the event data.</param>
        /// <returns><see cref="WebViewControlUnviewableContentIdentifiedEventArgs"/></returns>
        public static WebViewControlUnviewableContentIdentifiedEventArgs
            ToWebViewControlUnviewableContentIdentifiedEventArgs(
                Windows.Web.UI.WebViewControlUnviewableContentIdentifiedEventArgs args) =>
            new WebViewControlUnviewableContentIdentifiedEventArgs(args);
    }
}
