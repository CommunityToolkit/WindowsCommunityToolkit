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