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
    /// Provides data for the <see cref="IWebView.MoveFocusRequested"/> event. This class cannot be inherited.
    /// </summary>
    /// <remarks>Copy from <see cref="Windows.Web.UI.Interop.WebViewControlMoveFocusRequestedEventArgs"/> to avoid requirement to link Windows.winmd</remarks>
    /// <seealso cref="EventArgs" />
    /// <seealso cref="Windows.Web.UI.Interop.WebViewControlMoveFocusRequestedEventArgs"/>
    public sealed class WebViewControlMoveFocusRequestedEventArgs : EventArgs
    {
        [SecurityCritical]
        private readonly Windows.Web.UI.Interop.WebViewControlMoveFocusRequestedEventArgs _args;

        internal WebViewControlMoveFocusRequestedEventArgs(Windows.Web.UI.Interop.WebViewControlMoveFocusRequestedEventArgs args)
        {
            _args = args ?? throw new ArgumentNullException(nameof(args));
        }

        /// <summary>
        /// Gets the move focus reason.
        /// </summary>
        /// <value><see cref="WebViewControlMoveFocusReason" /> The move focus reason</value>
        public WebViewControlMoveFocusReason Reason => (WebViewControlMoveFocusReason)_args.Reason;

        /// <summary>
        /// Performs an implicit conversion from <see cref="Windows.Web.UI.Interop.WebViewControlMoveFocusRequestedEventArgs"/> to <see cref="WebViewControlMoveFocusRequestedEventArgs"/>.
        /// </summary>
        /// <param name="args">The <see cref="Windows.Web.UI.Interop.WebViewControlMoveFocusRequestedEventArgs"/> instance containing the event data.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator WebViewControlMoveFocusRequestedEventArgs(Windows.Web.UI.Interop.WebViewControlMoveFocusRequestedEventArgs args) => ToWebViewControlMoveFocusRequestedEventArgs(args);

        /// <summary>
        /// Creates a <see cref="WebViewControlMoveFocusRequestedEventArgs"/> from <see cref="Windows.Web.UI.Interop.WebViewControlMoveFocusRequestedEventArgs"/>.
        /// </summary>
        /// <param name="args">The <see cref="Windows.Web.UI.Interop.WebViewControlMoveFocusRequestedEventArgs"/> instance containing the event data.</param>
        /// <returns><see cref="WebViewControlMoveFocusRequestedEventArgs"/></returns>
        public static WebViewControlMoveFocusRequestedEventArgs ToWebViewControlMoveFocusRequestedEventArgs(
            Windows.Web.UI.Interop.WebViewControlMoveFocusRequestedEventArgs args) =>
            new WebViewControlMoveFocusRequestedEventArgs(args);
    }
}