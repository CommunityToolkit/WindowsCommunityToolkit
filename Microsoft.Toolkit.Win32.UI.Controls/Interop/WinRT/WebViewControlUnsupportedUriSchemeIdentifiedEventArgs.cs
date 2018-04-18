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
        /// Gets or sets a value that marks the routed event as handled. A <see langword="true" /> value for Handled prevents other handlers along the event route from handling the same event again.
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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates")]
        public static implicit operator WebViewControlUnsupportedUriSchemeIdentifiedEventArgs(Windows.Web.UI.WebViewControlUnsupportedUriSchemeIdentifiedEventArgs args) => new WebViewControlUnsupportedUriSchemeIdentifiedEventArgs(args);
    }
}