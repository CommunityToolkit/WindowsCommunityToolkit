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

        public uint Id => _permissionRequest?.Id ?? 0;

        public WebViewControlPermissionType PermissionType => (WebViewControlPermissionType)_permissionRequest.PermissionType;

        public WebViewControlPermissionState State => (WebViewControlPermissionState)_permissionRequest.State;

        public Uri Uri => _permissionRequest.Uri;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates")]
        public static implicit operator WebViewControlPermissionRequest(
            Windows.Web.UI.WebViewControlPermissionRequest permissionRequest)
        {
            return new WebViewControlPermissionRequest(permissionRequest);
        }

        public void Allow()
        {
            _permissionRequest?.Allow();
        }

        public void Defer()
        {
            _permissionRequest?.Defer();
        }

        public void Deny()
        {
            _permissionRequest?.Deny();
        }
    }
}