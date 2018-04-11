using System;
using System.Security;

namespace Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT
{
    public sealed class WebViewNavigationCompletedEventArgs : EventArgs
    {
        internal WebViewNavigationCompletedEventArgs(Windows.Web.UI.WebViewControlNavigationCompletedEventArgs args)
        {
            IsSuccess = args.IsSuccess;
            Uri = args.Uri;
            WebErrorStatus = (WebErrorStatus)args.WebErrorStatus;
        }

        internal WebViewNavigationCompletedEventArgs(Windows.Web.UI.WebViewControlNavigationCompletedEventArgs args, Uri uri)
        : this(args)
        {
            Uri = uri;
        }

        public bool IsSuccess { get; }

        public Uri Uri { get; }

        public WebErrorStatus WebErrorStatus { get; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates")]
        public static implicit operator WebViewNavigationCompletedEventArgs(
            Windows.Web.UI.WebViewControlNavigationCompletedEventArgs args)
        {
            return new WebViewNavigationCompletedEventArgs(args);
        }
    }
}