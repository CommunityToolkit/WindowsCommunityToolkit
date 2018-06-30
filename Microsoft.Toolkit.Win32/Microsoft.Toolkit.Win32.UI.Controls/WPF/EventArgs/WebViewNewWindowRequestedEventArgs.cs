using System;
using System.Security;
using Microsoft.Windows.Interop;

namespace Microsoft.Toolkit.Win32.UI.Controls.WPF.WebViewLite
{
    /// <summary>
    /// Provides data for events. This class cannot be inherited.
    /// </summary>
    /// <remarks>Copy from <see cref="global::Windows.UI.Xaml.Controls.WebViewNewWindowRequestedEventArgs"/> to avoid requirement to link Windows.winmd</remarks>
    /// <seealso cref="global::Windows.UI.Xaml.Controls.WebViewNewWindowRequestedEventArgs"/>
    public sealed class WebViewNewWindowRequestedEventArgs : EventArgs
    {
        [SecurityCritical]
        private readonly global::Windows.UI.Xaml.Controls.WebViewNewWindowRequestedEventArgs _args;

        [SecurityCritical]
        internal WebViewNewWindowRequestedEventArgs(global::Windows.UI.Xaml.Controls.WebViewNewWindowRequestedEventArgs args)
        {
            _args = args;
        }

        public bool Handled
        {
            [SecurityCritical]
            get => (bool)_args.Handled;
            [SecurityCritical]
            set => _args.Handled = value;
        }

        public System.Uri Referrer
        {
            [SecurityCritical]
            get => (System.Uri)_args.Referrer;
        }

        public System.Uri Uri
        {
            [SecurityCritical]
            get => (System.Uri)_args.Uri;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="global::Windows.UI.Xaml.Controls.WebViewNewWindowRequestedEventArgs"/> to <see cref="Microsoft.Toolkit.Win32.UI.Controls.WPF.WebViewLite.WebViewNewWindowRequestedEventArgs"/>.
        /// </summary>
        /// <param name="args">The <see cref="global::Windows.UI.Xaml.Controls.WebViewNewWindowRequestedEventArgs"/> instance containing the event data.</param>
        /// <returns>The result of the conversion.</returns>
        [SecurityCritical]
        public static implicit operator WebViewNewWindowRequestedEventArgs(
            global::Windows.UI.Xaml.Controls.WebViewNewWindowRequestedEventArgs args)
        {
            return FromWebViewNewWindowRequestedEventArgs(args);
        }

        /// <summary>
        /// Creates a <see cref="WebViewNewWindowRequestedEventArgs"/> from <see cref="global::Windows.UI.Xaml.Controls.WebViewNewWindowRequestedEventArgs"/>.
        /// </summary>
        /// <param name="args">The <see cref="global::Windows.UI.Xaml.Controls.WebViewNewWindowRequestedEventArgs"/> instance containing the event data.</param>
        /// <returns><see cref="WebViewNewWindowRequestedEventArgs"/></returns>
        public static WebViewNewWindowRequestedEventArgs FromWebViewNewWindowRequestedEventArgs(global::Windows.UI.Xaml.Controls.WebViewNewWindowRequestedEventArgs args)
        {
            return new WebViewNewWindowRequestedEventArgs(args);
        }
    }
}