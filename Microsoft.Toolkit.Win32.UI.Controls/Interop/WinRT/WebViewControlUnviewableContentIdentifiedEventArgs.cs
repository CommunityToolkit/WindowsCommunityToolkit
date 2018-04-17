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
    /// CProvides data for the <see cref="IWebView.UnviewableContentIdentified" /> event. This class cannot be inherited.
    /// </summary>
    /// <seealso cref="System.EventArgs" />
    /// <seealso cref="Windows.Web.UI.WebViewControlUnviewableContentIdentifiedEventArgs"/>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Unviewable")]
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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates")]
        public static implicit operator WebViewControlUnviewableContentIdentifiedEventArgs(Windows.Web.UI.WebViewControlUnviewableContentIdentifiedEventArgs args) => new WebViewControlUnviewableContentIdentifiedEventArgs(args);
    }
}
