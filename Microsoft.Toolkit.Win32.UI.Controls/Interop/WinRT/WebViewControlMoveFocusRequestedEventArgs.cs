using System;
using System.Security;

namespace Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT
{
  public sealed class WebViewControlMoveFocusRequestedEventArgs : EventArgs
  {
    [SecurityCritical]
    private readonly Windows.Web.UI.Interop.WebViewControlMoveFocusRequestedEventArgs _args;

    internal WebViewControlMoveFocusRequestedEventArgs(Windows.Web.UI.Interop.WebViewControlMoveFocusRequestedEventArgs args)
    {
      _args = args ?? throw new ArgumentNullException(nameof(args));
    }

    public WebViewControlMoveFocusReason Reason => (WebViewControlMoveFocusReason)_args.Reason;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates")]
        public static implicit operator WebViewControlMoveFocusRequestedEventArgs(Windows.Web.UI.Interop.WebViewControlMoveFocusRequestedEventArgs args) => new WebViewControlMoveFocusRequestedEventArgs(args);
  }
}