// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Security;

namespace Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT
{
    /// <summary>
    /// Represents a request for permissions in an <see cref="IWebView"/>. This class cannot be inherited.
    /// </summary>
    /// <remarks>
    /// <p>Copy from <see cref="Windows.Web.UI.WebViewControlPermissionRequest"/> to avoid requirement to link Windows.winmd.</p>
    /// <p>For more info, see the <seealso cref="IWebView.PermissionRequested"/> event.</p>
    /// </remarks>
    /// <seealso cref="IWebView.PermissionRequested"/>
    public sealed class WebViewControlPermissionRequest
    {
        [SecurityCritical]
        private readonly Windows.Web.UI.WebViewControlPermissionRequest _permissionRequest;

        internal WebViewControlPermissionRequest(Windows.Web.UI.WebViewControlPermissionRequest permissionRequest)
        {
            _permissionRequest = permissionRequest ?? throw new ArgumentNullException(nameof(permissionRequest));
        }

        /// <summary>
        /// Gets the identifier for the permission request.
        /// </summary>
        /// <value>The permission request identifier.</value>
        public uint Id => _permissionRequest?.Id ?? 0U;

        /// <summary>
        /// Gets the type of the permission that's requested.
        /// </summary>
        /// <value>The type of the permission that's requested.</value>
        public WebViewControlPermissionType PermissionType => (WebViewControlPermissionType)_permissionRequest.PermissionType;

        /// <summary>
        /// Gets the current state of the permission request.
        /// </summary>
        /// <value>The current state of the permission request.</value>
        public WebViewControlPermissionState State => (WebViewControlPermissionState)_permissionRequest.State;

        /// <summary>
        /// Gets the Uniform Resource Identifier (URI) of the content where the permission request originated.
        /// </summary>
        /// <value>The Uniform Resource Identifier (URI) of the content where the permission request originated.</value>
        public Uri Uri => _permissionRequest.Uri;

        /// <summary>
        /// Performs an implicit conversion from <see cref="Windows.Web.UI.WebViewControlPermissionRequest"/> to <see cref="WebViewControlPermissionRequest"/>.
        /// </summary>
        /// <param name="args">The permission request.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator WebViewControlPermissionRequest(Windows.Web.UI.WebViewControlPermissionRequest args) => ToWebViewControlPermissionRequest(args);

        /// <summary>
        /// Creates a <see cref="WebViewControlPermissionRequest"/> from <see cref="Windows.Web.UI.WebViewControlPermissionRequest"/>.
        /// </summary>
        /// <param name="args">The <see cref="Windows.Web.UI.WebViewControlPermissionRequest"/>.</param>
        /// <returns><see cref="WebViewControlPermissionRequest"/></returns>
        public static WebViewControlPermissionRequest ToWebViewControlPermissionRequest(
            Windows.Web.UI.WebViewControlPermissionRequest args) => new WebViewControlPermissionRequest(args);

        /// <summary>
        /// Grants the requested permission.
        /// </summary>
        public void Allow()
        {
            _permissionRequest?.Allow();
        }

        /// <summary>
        /// Defers the permission request to be allowed or denied at a later time.
        /// </summary>
        /// <seealso cref="IWebView.GetDeferredPermissionRequestById"/>
        public void Defer()
        {
            _permissionRequest?.Defer();
        }

        /// <summary>
        /// Denies the requested permission.
        /// </summary>
        public void Deny()
        {
            _permissionRequest?.Deny();
        }
    }
}