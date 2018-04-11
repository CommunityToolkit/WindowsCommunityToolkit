using System;
using System.Security;

namespace Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT
{
    public sealed class WebViewControlUnsupportedUriSchemeIdentifiedEventArgs : EventArgs
    {
        [SecurityCritical]
        private readonly Windows.Web.UI.WebViewControlUnsupportedUriSchemeIdentifiedEventArgs _args;

        internal WebViewControlUnsupportedUriSchemeIdentifiedEventArgs(Windows.Web.UI.WebViewControlUnsupportedUriSchemeIdentifiedEventArgs args)
        {
            _args = args ?? throw new ArgumentNullException(nameof(args));
        }

        public Uri Uri => _args.Uri;

        public bool Handled
        {
            get => _args.Handled;
            set => _args.Handled = value;
        }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates")]
        public static implicit operator WebViewControlUnsupportedUriSchemeIdentifiedEventArgs(Windows.Web.UI.WebViewControlUnsupportedUriSchemeIdentifiedEventArgs args) =>
            new WebViewControlUnsupportedUriSchemeIdentifiedEventArgs(args);
    }
}