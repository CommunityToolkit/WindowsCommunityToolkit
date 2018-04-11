using System;
using System.Security;

namespace Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT
{
    // ReSharper disable InconsistentNaming
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "DOM")]
    public sealed class WebViewControlDOMContentLoadedEventArgs : EventArgs
  // ReSharper restore InconsistentNaming
  {
    [SecurityCritical]
    internal WebViewControlDOMContentLoadedEventArgs(Windows.Web.UI.WebViewControlDOMContentLoadedEventArgs args)
    {
      Uri = args.Uri;
    }

    internal WebViewControlDOMContentLoadedEventArgs(Uri uri)
    {
      Uri = uri;
    }

    public Uri Uri { get; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates")]
        public static implicit operator WebViewControlDOMContentLoadedEventArgs(
      Windows.Web.UI.WebViewControlDOMContentLoadedEventArgs args)
    {
      return new WebViewControlDOMContentLoadedEventArgs(args);
    }
  }
}