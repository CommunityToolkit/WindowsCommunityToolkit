using System;
using System.Security;

namespace Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT
{
    public sealed class WebViewControlContentLoadingEventArgs : EventArgs
    {
        [SecurityCritical]
        private readonly Windows.Web.UI.WebViewControlContentLoadingEventArgs _args;

        [SecurityCritical]
        internal WebViewControlContentLoadingEventArgs(Windows.Web.UI.WebViewControlContentLoadingEventArgs args)
        {
            _args = args;
        }

        public Uri Uri
        {
            [SecurityCritical]
            get { return _args.Uri; }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates")]
        [SecurityCritical]
        public static implicit operator WebViewControlContentLoadingEventArgs(
            Windows.Web.UI.WebViewControlContentLoadingEventArgs args)
        {
            return new WebViewControlContentLoadingEventArgs(args);
        }
    }
}