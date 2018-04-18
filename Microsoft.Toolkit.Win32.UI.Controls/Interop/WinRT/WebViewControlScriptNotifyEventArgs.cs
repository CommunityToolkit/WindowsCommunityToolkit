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
    /// Provides data for the <see cref="IWebView.ScriptNotify"/> event. This class cannot be inherited.
    /// </summary>
    /// <remarks>Copy from <see cref="Windows.Web.UI.WebViewControlScriptNotifyEventArgs"/> to avoid requirement to link Windows.winmd</remarks>
    /// <seealso cref="System.EventArgs" />
    /// <seealso cref="Windows.Web.UI.WebViewControlScriptNotifyEventArgs"/>
    public sealed class WebViewControlScriptNotifyEventArgs : EventArgs
    {
        [SecurityCritical]
        private readonly Windows.Web.UI.WebViewControlScriptNotifyEventArgs _args;

        internal WebViewControlScriptNotifyEventArgs(Windows.Web.UI.WebViewControlScriptNotifyEventArgs args)
        {
            _args = args;
        }

        /// <summary>
        /// Gets the Uniform Resource Identifier (URI) that originated the <see cref="IWebView.ScriptNotify"/> event.
        /// </summary>
        /// <value>The Uniform Resource Identifier (URI).</value>
        public Uri Uri => _args.Uri;

        public string Value => _args.Value;

        /// <summary>
        /// Performs an implicit conversion from <see cref="Windows.Web.UI.WebViewControlScriptNotifyEventArgs"/> to <see cref="WebViewControlScriptNotifyEventArgs"/>.
        /// </summary>
        /// <param name="args">The <see cref="Windows.Web.UI.WebViewControlScriptNotifyEventArgs"/> instance containing the event data.</param>
        /// <returns>The result of the conversion.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates")]
        public static implicit operator WebViewControlScriptNotifyEventArgs(Windows.Web.UI.WebViewControlScriptNotifyEventArgs args) => new WebViewControlScriptNotifyEventArgs(args);
    }
}