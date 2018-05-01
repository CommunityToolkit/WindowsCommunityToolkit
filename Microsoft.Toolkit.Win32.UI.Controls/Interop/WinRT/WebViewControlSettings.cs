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
    /// A proxy for <see cref="Windows.Web.UI.WebViewControlSettings"/>. This class cannot be inherited.
    /// </summary>
    /// <seealso cref="Windows.Web.UI.WebViewControlSettings"/>
    public sealed class WebViewControlSettings
    {
        [SecurityCritical]
        private readonly Windows.Web.UI.WebViewControlSettings _settings;

        internal WebViewControlSettings(Windows.Web.UI.WebViewControlSettings settings)
        {
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
        }

        /// <summary>
        /// Gets or sets a value indicating whether the use of IndexedDB is allowed.
        /// </summary>
        /// <value><c>true</c> if IndexedDB is allowed; otherwise, <c>false</c>. The default is <c>true</c>.</value>
        public bool IsIndexedDBEnabled
        {
            get => _settings.IsIndexedDBEnabled;
            set => _settings.IsIndexedDBEnabled = value;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the use of JavaScript is allowed.
        /// </summary>
        /// <value> true if JavaScript is allowed in the <see cref="IWebView"/>; otherwise, false. The default is true.</value>
        public bool IsJavaScriptEnabled
        {
            get => _settings.IsJavaScriptEnabled;
            set => _settings.IsJavaScriptEnabled = value;
        }

        /// <summary>
        /// Gets or sets a value indicating whether <see cref="IWebView.ScriptNotify" /> is allowed.
        /// </summary>
        /// <value>Whether <see cref="IWebView.ScriptNotify" /> is allowed.</value>
        public bool IsScriptNotifyAllowed
        {
            get => _settings.IsScriptNotifyAllowed;
            set => _settings.IsScriptNotifyAllowed = value;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="Windows.Web.UI.WebViewControlSettings"/> to <see cref="WebViewControlSettings"/>.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator WebViewControlSettings(Windows.Web.UI.WebViewControlSettings settings) => ToWebViewControlSettings(settings);

        /// <summary>
        /// Creates a <see cref="WebViewControlSettings" /> from <see cref="Windows.Web.UI.WebViewControlSettings"/>.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <returns><see cref="WebViewControlSettings"/></returns>
        public static WebViewControlSettings ToWebViewControlSettings(Windows.Web.UI.WebViewControlSettings settings) =>
            new WebViewControlSettings(settings);
    }
}