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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates")]
        public static implicit operator WebViewControlPermissionRequestedEventArgs(Windows.Web.UI.WebViewControlPermissionRequestedEventArgs args) => new WebViewControlPermissionRequestedEventArgs(args);
    }
}