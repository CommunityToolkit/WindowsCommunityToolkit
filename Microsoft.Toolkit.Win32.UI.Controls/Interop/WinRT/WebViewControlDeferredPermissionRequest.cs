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
  public sealed class WebViewControlDeferredPermissionRequest
  {
    [SecurityCritical]
    private readonly Windows.Web.UI.WebViewControlDeferredPermissionRequest _webViewControlDeferredPermissionRequest;

    internal WebViewControlDeferredPermissionRequest(Windows.Web.UI.WebViewControlDeferredPermissionRequest webViewControlDeferredPermissionRequest)
    {
      _webViewControlDeferredPermissionRequest = webViewControlDeferredPermissionRequest;
    }

    public uint Id => _webViewControlDeferredPermissionRequest.Id;
    public WebViewControlPermissionType PermissionType => (WebViewControlPermissionType)_webViewControlDeferredPermissionRequest.PermissionType;
    public Uri Uri => _webViewControlDeferredPermissionRequest.Uri;

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates")]
    public static implicit operator WebViewControlDeferredPermissionRequest(
  Windows.Web.UI.WebViewControlDeferredPermissionRequest webViewControlDeferredPermissionRequest)
    {
      return new WebViewControlDeferredPermissionRequest(webViewControlDeferredPermissionRequest);
    }

    public void Allow() => _webViewControlDeferredPermissionRequest.Allow();

    public void Deny() => _webViewControlDeferredPermissionRequest.Deny();
  }
}