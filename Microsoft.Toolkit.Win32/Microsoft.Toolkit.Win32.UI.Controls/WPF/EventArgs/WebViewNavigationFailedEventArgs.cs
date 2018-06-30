using System;
using System.Security;
using Microsoft.Windows.Interop;

namespace Microsoft.Toolkit.Win32.UI.Controls.WPF.WebViewLite
{
    /// <summary>
    /// Provides data for events. This class cannot be inherited.
    /// </summary>
    /// <remarks>Copy from <see cref="global::Windows.UI.Xaml.Controls.WebViewNavigationFailedEventArgs"/> to avoid requirement to link Windows.winmd</remarks>
    /// <seealso cref="global::Windows.UI.Xaml.Controls.WebViewNavigationFailedEventArgs"/>
    public sealed class WebViewNavigationFailedEventArgs : EventArgs
    {
        [SecurityCritical]
        private readonly global::Windows.UI.Xaml.Controls.WebViewNavigationFailedEventArgs _args;

        [SecurityCritical]
        internal WebViewNavigationFailedEventArgs(global::Windows.UI.Xaml.Controls.WebViewNavigationFailedEventArgs args)
        {
            _args = args;
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
        /// Performs an implicit conversion from <see cref="global::Windows.UI.Xaml.Controls.WebViewNavigationFailedEventArgs"/> to <see cref="Microsoft.Toolkit.Win32.UI.Controls.WPF.WebViewLite.WebViewNavigationFailedEventArgs"/>.
        /// </summary>
        /// <param name="args">The <see cref="global::Windows.UI.Xaml.Controls.WebViewNavigationFailedEventArgs"/> instance containing the event data.</param>
        /// <returns>The result of the conversion.</returns>
        [SecurityCritical]
        public static implicit operator WebViewNavigationFailedEventArgs(
            global::Windows.UI.Xaml.Controls.WebViewNavigationFailedEventArgs args)
        {
            return FromWebViewNavigationFailedEventArgs(args);
        }

        /// <summary>
        /// Creates a <see cref="WebViewNavigationFailedEventArgs"/> from <see cref="global::Windows.UI.Xaml.Controls.WebViewNavigationFailedEventArgs"/>.
        /// </summary>
        /// <param name="args">The <see cref="global::Windows.UI.Xaml.Controls.WebViewNavigationFailedEventArgs"/> instance containing the event data.</param>
        /// <returns><see cref="WebViewNavigationFailedEventArgs"/></returns>
        public static WebViewNavigationFailedEventArgs FromWebViewNavigationFailedEventArgs(global::Windows.UI.Xaml.Controls.WebViewNavigationFailedEventArgs args)
        {
            return new WebViewNavigationFailedEventArgs(args);
        }
    }
}