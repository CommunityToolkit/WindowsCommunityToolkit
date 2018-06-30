using System;
using System.Security;
using Microsoft.Windows.Interop;

namespace Microsoft.Toolkit.Win32.UI.Controls.WPF.WebViewLite
{
    /// <summary>
    /// Provides data for events. This class cannot be inherited.
    /// </summary>
    /// <remarks>Copy from <see cref="global::Windows.UI.Xaml.Controls.WebViewDOMContentLoadedEventArgs"/> to avoid requirement to link Windows.winmd</remarks>
    /// <seealso cref="global::Windows.UI.Xaml.Controls.WebViewDOMContentLoadedEventArgs"/>
    public sealed class WebViewDOMContentLoadedEventArgs : EventArgs
    {
        [SecurityCritical]
        private readonly global::Windows.UI.Xaml.Controls.WebViewDOMContentLoadedEventArgs _args;

        [SecurityCritical]
        internal WebViewDOMContentLoadedEventArgs(global::Windows.UI.Xaml.Controls.WebViewDOMContentLoadedEventArgs args)
        {
            _args = args;
        }

        public System.Uri Uri
        {
            [SecurityCritical]
            get => (System.Uri)_args.Uri;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="global::Windows.UI.Xaml.Controls.WebViewDOMContentLoadedEventArgs"/> to <see cref="Microsoft.Toolkit.Win32.UI.Controls.WPF.WebViewLite.WebViewDOMContentLoadedEventArgs"/>.
        /// </summary>
        /// <param name="args">The <see cref="global::Windows.UI.Xaml.Controls.WebViewDOMContentLoadedEventArgs"/> instance containing the event data.</param>
        /// <returns>The result of the conversion.</returns>
        [SecurityCritical]
        public static implicit operator WebViewDOMContentLoadedEventArgs(
            global::Windows.UI.Xaml.Controls.WebViewDOMContentLoadedEventArgs args)
        {
            return FromWebViewDOMContentLoadedEventArgs(args);
        }

        /// <summary>
        /// Creates a <see cref="WebViewDOMContentLoadedEventArgs"/> from <see cref="global::Windows.UI.Xaml.Controls.WebViewDOMContentLoadedEventArgs"/>.
        /// </summary>
        /// <param name="args">The <see cref="global::Windows.UI.Xaml.Controls.WebViewDOMContentLoadedEventArgs"/> instance containing the event data.</param>
        /// <returns><see cref="WebViewDOMContentLoadedEventArgs"/></returns>
        public static WebViewDOMContentLoadedEventArgs FromWebViewDOMContentLoadedEventArgs(global::Windows.UI.Xaml.Controls.WebViewDOMContentLoadedEventArgs args)
        {
            return new WebViewDOMContentLoadedEventArgs(args);
        }
    }
}