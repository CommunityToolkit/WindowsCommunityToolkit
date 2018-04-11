using System;
using System.Security;

namespace Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT
{
    public sealed class WebViewControlNewWindowRequestedEventArgs : EventArgs
    {
        [SecurityCritical]
        private readonly Windows.Web.UI.WebViewControlNewWindowRequestedEventArgs _args;

        internal WebViewControlNewWindowRequestedEventArgs(Windows.Web.UI.WebViewControlNewWindowRequestedEventArgs args)
        {
            _args = args ?? throw new ArgumentNullException(nameof(args));
        }

        public Uri Uri => _args.Uri;
        public Uri Referrer => _args.Referrer;

        public bool Handled
        {
            get => _args.Handled;
            set => _args.Handled = value;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates")]
        public static implicit operator WebViewControlNewWindowRequestedEventArgs(Windows.Web.UI.WebViewControlNewWindowRequestedEventArgs args) => new WebViewControlNewWindowRequestedEventArgs(args);
    }
}