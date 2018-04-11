using System;
using System.Security;

namespace Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT
{
    public sealed class WebViewControlNavigationStartingEventArgs : EventArgs
    {
        [SecurityCritical]
        private readonly Windows.Web.UI.WebViewControlNavigationStartingEventArgs _args;

        [SecurityCritical]
        internal WebViewControlNavigationStartingEventArgs(Windows.Web.UI.WebViewControlNavigationStartingEventArgs args)
        {
            _args = args;
            Uri = args.Uri;
        }

        [SecurityCritical]
        internal WebViewControlNavigationStartingEventArgs(Windows.Web.UI.WebViewControlNavigationStartingEventArgs args, Uri uri)
            :this(args)
        {
            Uri = uri;
        }

        public bool Cancel
        {
            [SecurityCritical]
            get => _args.Cancel;
            [SecurityCritical]
            set => _args.Cancel = value;
        }

        public Uri Uri { get; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates")]
        public static implicit operator WebViewControlNavigationStartingEventArgs(
            Windows.Web.UI.WebViewControlNavigationStartingEventArgs args)
        {
            return new WebViewControlNavigationStartingEventArgs(args);
        }
    }
}