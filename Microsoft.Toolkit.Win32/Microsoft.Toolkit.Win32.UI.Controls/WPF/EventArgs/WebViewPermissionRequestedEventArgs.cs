using System;
using System.Security;
using Microsoft.Windows.Interop;

namespace Microsoft.Toolkit.Win32.UI.Controls.WPF.WebViewLite
{
    /// <summary>
    /// Provides data for events. This class cannot be inherited.
    /// </summary>
    /// <remarks>Copy from <see cref="global::Windows.UI.Xaml.Controls.WebViewPermissionRequestedEventArgs"/> to avoid requirement to link Windows.winmd</remarks>
    /// <seealso cref="global::Windows.UI.Xaml.Controls.WebViewPermissionRequestedEventArgs"/>
    public sealed class WebViewPermissionRequestedEventArgs : EventArgs
    {
        [SecurityCritical]
        private readonly global::Windows.UI.Xaml.Controls.WebViewPermissionRequestedEventArgs _args;

        [SecurityCritical]
        internal WebViewPermissionRequestedEventArgs(global::Windows.UI.Xaml.Controls.WebViewPermissionRequestedEventArgs args)
        {
            _args = args;
        }

        public global::Windows.UI.Xaml.Controls.WebViewPermissionRequest PermissionRequest
        {
            [SecurityCritical]
            get => (global::Windows.UI.Xaml.Controls.WebViewPermissionRequest)_args.PermissionRequest;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="global::Windows.UI.Xaml.Controls.WebViewPermissionRequestedEventArgs"/> to <see cref="Microsoft.Toolkit.Win32.UI.Controls.WPF.WebViewLite.WebViewPermissionRequestedEventArgs"/>.
        /// </summary>
        /// <param name="args">The <see cref="global::Windows.UI.Xaml.Controls.WebViewPermissionRequestedEventArgs"/> instance containing the event data.</param>
        /// <returns>The result of the conversion.</returns>
        [SecurityCritical]
        public static implicit operator WebViewPermissionRequestedEventArgs(
            global::Windows.UI.Xaml.Controls.WebViewPermissionRequestedEventArgs args)
        {
            return FromWebViewPermissionRequestedEventArgs(args);
        }

        /// <summary>
        /// Creates a <see cref="WebViewPermissionRequestedEventArgs"/> from <see cref="global::Windows.UI.Xaml.Controls.WebViewPermissionRequestedEventArgs"/>.
        /// </summary>
        /// <param name="args">The <see cref="global::Windows.UI.Xaml.Controls.WebViewPermissionRequestedEventArgs"/> instance containing the event data.</param>
        /// <returns><see cref="WebViewPermissionRequestedEventArgs"/></returns>
        public static WebViewPermissionRequestedEventArgs FromWebViewPermissionRequestedEventArgs(global::Windows.UI.Xaml.Controls.WebViewPermissionRequestedEventArgs args)
        {
            return new WebViewPermissionRequestedEventArgs(args);
        }
    }
}