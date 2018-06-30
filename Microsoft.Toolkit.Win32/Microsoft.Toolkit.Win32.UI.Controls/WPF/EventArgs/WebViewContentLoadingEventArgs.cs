using System;
using System.Security;
using Microsoft.Windows.Interop;

namespace Microsoft.Toolkit.Win32.UI.Controls.WPF.WebViewLite
{
    /// <summary>
    /// Provides data for events. This class cannot be inherited.
    /// </summary>
    /// <remarks>Copy from <see cref="global::Windows.UI.Xaml.Controls.WebViewContentLoadingEventArgs"/> to avoid requirement to link Windows.winmd</remarks>
    /// <seealso cref="global::Windows.UI.Xaml.Controls.WebViewContentLoadingEventArgs"/>
    public sealed class WebViewContentLoadingEventArgs : EventArgs
    {
        [SecurityCritical]
        private readonly global::Windows.UI.Xaml.Controls.WebViewContentLoadingEventArgs _args;

        [SecurityCritical]
        internal WebViewContentLoadingEventArgs(global::Windows.UI.Xaml.Controls.WebViewContentLoadingEventArgs args)
        {
            _args = args;
        }

        public System.Uri Uri
        {
            [SecurityCritical]
            get => (System.Uri)_args.Uri;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="global::Windows.UI.Xaml.Controls.WebViewContentLoadingEventArgs"/> to <see cref="Microsoft.Toolkit.Win32.UI.Controls.WPF.WebViewLite.WebViewContentLoadingEventArgs"/>.
        /// </summary>
        /// <param name="args">The <see cref="global::Windows.UI.Xaml.Controls.WebViewContentLoadingEventArgs"/> instance containing the event data.</param>
        /// <returns>The result of the conversion.</returns>
        [SecurityCritical]
        public static implicit operator WebViewContentLoadingEventArgs(
            global::Windows.UI.Xaml.Controls.WebViewContentLoadingEventArgs args)
        {
            return FromWebViewContentLoadingEventArgs(args);
        }

        /// <summary>
        /// Creates a <see cref="WebViewContentLoadingEventArgs"/> from <see cref="global::Windows.UI.Xaml.Controls.WebViewContentLoadingEventArgs"/>.
        /// </summary>
        /// <param name="args">The <see cref="global::Windows.UI.Xaml.Controls.WebViewContentLoadingEventArgs"/> instance containing the event data.</param>
        /// <returns><see cref="WebViewContentLoadingEventArgs"/></returns>
        public static WebViewContentLoadingEventArgs FromWebViewContentLoadingEventArgs(global::Windows.UI.Xaml.Controls.WebViewContentLoadingEventArgs args)
        {
            return new WebViewContentLoadingEventArgs(args);
        }
    }
}