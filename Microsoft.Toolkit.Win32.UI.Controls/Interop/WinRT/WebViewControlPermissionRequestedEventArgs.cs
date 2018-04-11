using System;
using System.Security;

namespace Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT
{
  public sealed class WebViewControlPermissionRequestedEventArgs : EventArgs
  {
    [SecurityCritical]
    private readonly Windows.Web.UI.WebViewControlPermissionRequestedEventArgs _args;

    internal WebViewControlPermissionRequestedEventArgs(Windows.Web.UI.WebViewControlPermissionRequestedEventArgs args)
    {
      _args = args;
    }

    public WebViewControlPermissionRequest PermissionRequest => _args.PermissionRequest;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates")]
        public static implicit operator WebViewControlPermissionRequestedEventArgs(
      Windows.Web.UI.WebViewControlPermissionRequestedEventArgs args)
    {
      return new WebViewControlPermissionRequestedEventArgs(args);
    }
  }
}