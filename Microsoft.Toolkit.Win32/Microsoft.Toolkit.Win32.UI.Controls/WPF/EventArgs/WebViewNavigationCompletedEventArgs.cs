using System;
using System.Security;
using Microsoft.Windows.Interop;

namespace Microsoft.Toolkit.Win32.UI.Controls.WPF.WebViewLite
{
    /// <summary>
    /// Provides data for events. This class cannot be inherited.
    /// </summary>
    /// <remarks>Copy from <see cref="global::Windows.UI.Xaml.Controls.WebViewNavigationCompletedEventArgs"/> to avoid requirement to link Windows.winmd</remarks>
    /// <seealso cref="global::Windows.UI.Xaml.Controls.WebViewNavigationCompletedEventArgs"/>
    public sealed class WebViewNavigationCompletedEventArgs : EventArgs
    {
        [SecurityCritical]
        private readonly global::Windows.UI.Xaml.Controls.WebViewNavigationCompletedEventArgs _args;

        [SecurityCritical]
        internal WebViewNavigationCompletedEventArgs(global::Windows.UI.Xaml.Controls.WebViewNavigationCompletedEventArgs args)
        {
            _args = args;
        }

        public bool IsSuccess
        {
            [SecurityCritical]
            get => (bool)_args.IsSuccess;
        }

        public System.Uri Uri
        {
            [SecurityCritical]
            get => (System.Uri)_args.Uri;
        }

        public global::Windows.Web.WebErrorStatus WebErrorStatus
        {
            [SecurityCritical]
            get => (global::Windows.Web.WebErrorStatus)_args.WebErrorStatus;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="global::Windows.UI.Xaml.Controls.WebViewNavigationCompletedEventArgs"/> to <see cref="Microsoft.Toolkit.Win32.UI.Controls.WPF.WebViewLite.WebViewNavigationCompletedEventArgs"/>.
        /// </summary>
        /// <param name="args">The <see cref="global::Windows.UI.Xaml.Controls.WebViewNavigationCompletedEventArgs"/> instance containing the event data.</param>
        /// <returns>The result of the conversion.</returns>
        [SecurityCritical]
        public static implicit operator WebViewNavigationCompletedEventArgs(
            global::Windows.UI.Xaml.Controls.WebViewNavigationCompletedEventArgs args)
        {
            return FromWebViewNavigationCompletedEventArgs(args);
        }

        /// <summary>
        /// Creates a <see cref="WebViewNavigationCompletedEventArgs"/> from <see cref="global::Windows.UI.Xaml.Controls.WebViewNavigationCompletedEventArgs"/>.
        /// </summary>
        /// <param name="args">The <see cref="global::Windows.UI.Xaml.Controls.WebViewNavigationCompletedEventArgs"/> instance containing the event data.</param>
        /// <returns><see cref="WebViewNavigationCompletedEventArgs"/></returns>
        public static WebViewNavigationCompletedEventArgs FromWebViewNavigationCompletedEventArgs(global::Windows.UI.Xaml.Controls.WebViewNavigationCompletedEventArgs args)
        {
            return new WebViewNavigationCompletedEventArgs(args);
        }
    }
}