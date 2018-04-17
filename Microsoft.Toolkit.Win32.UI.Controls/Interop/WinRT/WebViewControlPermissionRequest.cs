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
    /// A proxy for <seealso cref="Windows.Web.UI.WebViewControlPermissionRequest"/>. This class cannot be inherited.
    /// </summary>
    /// <remarks>For more info, see the <seealso cref="IWebView.PermissionRequested"/> event.</remarks>
    public sealed class WebViewControlPermissionRequest
    {
        [SecurityCritical]
        private readonly Windows.Web.UI.WebViewControlPermissionRequest _permissionRequest;

        internal WebViewControlPermissionRequest(Windows.Web.UI.WebViewControlPermissionRequest permissionRequest)
        {
            _permissionRequest = permissionRequest ?? throw new ArgumentNullException(nameof(permissionRequest));
        }

        private WebViewControlPermissionRequest()
        {
        }

        /// <summary>
        /// Gets the identifier for the permission request.
        /// </summary>
        /// <value>The permission request identifier.</value>
        public uint Id => _permissionRequest?.Id ?? 0;

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
        /// <param name="permissionRequest">The permission request.</param>
        /// <returns>The result of the conversion.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates")]
        public static implicit operator WebViewControlPermissionRequest(
            Windows.Web.UI.WebViewControlPermissionRequest permissionRequest)
        {
            return new WebViewControlPermissionRequest(permissionRequest);
        }

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