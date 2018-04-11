using System;
using System.Security;

namespace Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Unviewable")]
    public sealed class WebViewControlUnviewableContentIdentifiedEventArgs : EventArgs
  {
    [SecurityCritical]
    private readonly Windows.Web.UI.WebViewControlUnviewableContentIdentifiedEventArgs _args;

    internal WebViewControlUnviewableContentIdentifiedEventArgs(Windows.Web.UI.WebViewControlUnviewableContentIdentifiedEventArgs args)
    {
      _args = args ?? throw new ArgumentNullException(nameof(args));
    }

    public Uri Uri => _args.Uri;
    public Uri Referrer => _args.Referrer;
    public string MediaType => _args.MediaType;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates")]
        public static implicit operator WebViewControlUnviewableContentIdentifiedEventArgs(Windows.Web.UI.WebViewControlUnviewableContentIdentifiedEventArgs args) => new WebViewControlUnviewableContentIdentifiedEventArgs(args);
  }
}
