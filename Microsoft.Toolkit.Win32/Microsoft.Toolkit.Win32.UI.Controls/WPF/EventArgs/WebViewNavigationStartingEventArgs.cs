using System;
using System.Security;
using Microsoft.Windows.Interop;

namespace Microsoft.Toolkit.Win32.UI.Controls.WPF.WebViewLite
{
    /// <summary>
    /// Provides data for events. This class cannot be inherited.
    /// </summary>
    /// <remarks>Copy from <see cref="global::Windows.UI.Xaml.Controls.WebViewNavigationStartingEventArgs"/> to avoid requirement to link Windows.winmd</remarks>
    /// <seealso cref="global::Windows.UI.Xaml.Controls.WebViewNavigationStartingEventArgs"/>
    public sealed class WebViewNavigationStartingEventArgs : EventArgs
    {
        [SecurityCritical]
        private readonly global::Windows.UI.Xaml.Controls.WebViewNavigationStartingEventArgs _args;

        [SecurityCritical]
        internal WebViewNavigationStartingEventArgs(global::Windows.UI.Xaml.Controls.WebViewNavigationStartingEventArgs args)
        {
            _args = args;
        }

        public bool Cancel
        {
            [SecurityCritical]
            get => (bool)_args.Cancel;
            [SecurityCritical]
            set => _args.Cancel = value;
        }

        public System.Uri Uri
        {
            [SecurityCritical]
            get => (System.Uri)_args.Uri;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="global::Windows.UI.Xaml.Controls.WebViewNavigationStartingEventArgs"/> to <see cref="Microsoft.Toolkit.Win32.UI.Controls.WPF.WebViewLite.WebViewNavigationStartingEventArgs"/>.
        /// </summary>
        /// <param name="args">The <see cref="global::Windows.UI.Xaml.Controls.WebViewNavigationStartingEventArgs"/> instance containing the event data.</param>
        /// <returns>The result of the conversion.</returns>
        [SecurityCritical]
        public static implicit operator WebViewNavigationStartingEventArgs(
            global::Windows.UI.Xaml.Controls.WebViewNavigationStartingEventArgs args)
        {
            return FromWebViewNavigationStartingEventArgs(args);
        }

        /// <summary>
        /// Creates a <see cref="WebViewNavigationStartingEventArgs"/> from <see cref="global::Windows.UI.Xaml.Controls.WebViewNavigationStartingEventArgs"/>.
        /// </summary>
        /// <param name="args">The <see cref="global::Windows.UI.Xaml.Controls.WebViewNavigationStartingEventArgs"/> instance containing the event data.</param>
        /// <returns><see cref="WebViewNavigationStartingEventArgs"/></returns>
        public static WebViewNavigationStartingEventArgs FromWebViewNavigationStartingEventArgs(global::Windows.UI.Xaml.Controls.WebViewNavigationStartingEventArgs args)
        {
            return new WebViewNavigationStartingEventArgs(args);
        }
    }
}