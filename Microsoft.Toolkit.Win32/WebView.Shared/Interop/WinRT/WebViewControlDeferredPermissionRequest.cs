// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Security;

namespace Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT
{
    /// <summary>
    /// Represents a deferred request for permissions in an <see cref="IWebView"/>. This class cannot be inherited.
    /// </summary>
    /// <remarks>Copy from <see cref="Windows.Web.UI.WebViewControlDeferredPermissionRequest"/> to avoid requirement to link Windows.winmd</remarks>
    /// <seealso cref="Windows.Web.UI.WebViewControlDeferredPermissionRequest"/>
    public sealed class WebViewControlDeferredPermissionRequest
    {
        [SecurityCritical]
        private readonly Windows.Web.UI.WebViewControlDeferredPermissionRequest _webViewControlDeferredPermissionRequest;

        internal WebViewControlDeferredPermissionRequest(Windows.Web.UI.WebViewControlDeferredPermissionRequest webViewControlDeferredPermissionRequest)
        {
            _webViewControlDeferredPermissionRequest = webViewControlDeferredPermissionRequest;
        }

        /// <summary>
        /// Gets the identifier for the permission request.
        /// </summary>
        public uint Id => _webViewControlDeferredPermissionRequest.Id;

        /// <summary>
        /// Gets a value that indicates the type of permission that's requested.
        /// </summary>
        public WebViewControlPermissionType PermissionType => (WebViewControlPermissionType)_webViewControlDeferredPermissionRequest.PermissionType;

        /// <summary>
        /// Gets the Uniform Resource Identifier (URI) of the content where the permission request originated.
        /// </summary>
        public Uri Uri => _webViewControlDeferredPermissionRequest.Uri;

        /// <summary>
        /// Performs an implicit conversion from <see cref="Windows.Web.UI.WebViewControlDeferredPermissionRequest"/> to <see cref="WebViewControlDeferredPermissionRequest"/>.
        /// </summary>
        /// <param name="args">The <see cref="Windows.Web.UI.WebViewControlDeferredPermissionRequest"/> instance containing the event data.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator WebViewControlDeferredPermissionRequest(Windows.Web.UI.WebViewControlDeferredPermissionRequest args) => ToWebViewControlDeferredPermissionRequest(args);

        /// <summary>
        /// Creates a <see cref="WebViewControlDeferredPermissionRequest"/> from <see cref="Windows.Web.UI.WebViewControlDeferredPermissionRequest"/>.
        /// </summary>
        /// <param name="args">The <see cref="Windows.Web.UI.WebViewControlDeferredPermissionRequest"/> instance containing the deferred permission request.</param>
        /// <returns><see cref="WebViewControlDeferredPermissionRequest"/>.</returns>
        public static WebViewControlDeferredPermissionRequest ToWebViewControlDeferredPermissionRequest(Windows.Web.UI.WebViewControlDeferredPermissionRequest args) => new WebViewControlDeferredPermissionRequest(args);

        /// <summary>
        /// Grants the requested permission.
        /// </summary>
        [SecurityCritical]
        public void Allow() => _webViewControlDeferredPermissionRequest.Allow();

        /// <summary>
        /// Denies the requested permission.
        /// </summary>
        [SecurityCritical]
        public void Deny() => _webViewControlDeferredPermissionRequest.Deny();
    }
}