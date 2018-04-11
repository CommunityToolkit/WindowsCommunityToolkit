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
    /// A proxy for <see cref="Windows.Web.UI.WebViewControlDeferredPermissionRequest"/> class.
    /// </summary>
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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates")]
        public static implicit operator WebViewControlDeferredPermissionRequest(
      Windows.Web.UI.WebViewControlDeferredPermissionRequest webViewControlDeferredPermissionRequest)
        {
            return new WebViewControlDeferredPermissionRequest(webViewControlDeferredPermissionRequest);
        }

        /// <summary>
        /// Grants the requested permission.
        /// </summary>
        public void Allow() => _webViewControlDeferredPermissionRequest.Allow();

        /// <summary>
        /// Denies the requested permission.
        /// </summary>
        public void Deny() => _webViewControlDeferredPermissionRequest.Deny();
    }
}