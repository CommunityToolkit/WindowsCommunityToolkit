using System;
using System.Security;
using Microsoft.Windows.Interop;

namespace Microsoft.Toolkit.Win32.UI.Controls.WPF.WebViewLite
{
    /// <summary>
    /// Provides data for events. This class cannot be inherited.
    /// </summary>
    /// <remarks>Copy from <see cref="global::Windows.UI.Xaml.Controls.WebViewSeparateProcessLostEventArgs"/> to avoid requirement to link Windows.winmd</remarks>
    /// <seealso cref="global::Windows.UI.Xaml.Controls.WebViewSeparateProcessLostEventArgs"/>
    public sealed class WebViewSeparateProcessLostEventArgs : EventArgs
    {
        [SecurityCritical]
        private readonly global::Windows.UI.Xaml.Controls.WebViewSeparateProcessLostEventArgs _args;

        [SecurityCritical]
        internal WebViewSeparateProcessLostEventArgs(global::Windows.UI.Xaml.Controls.WebViewSeparateProcessLostEventArgs args)
        {
            _args = args;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="global::Windows.UI.Xaml.Controls.WebViewSeparateProcessLostEventArgs"/> to <see cref="Microsoft.Toolkit.Win32.UI.Controls.WPF.WebViewLite.WebViewSeparateProcessLostEventArgs"/>.
        /// </summary>
        /// <param name="args">The <see cref="global::Windows.UI.Xaml.Controls.WebViewSeparateProcessLostEventArgs"/> instance containing the event data.</param>
        /// <returns>The result of the conversion.</returns>
        [SecurityCritical]
        public static implicit operator WebViewSeparateProcessLostEventArgs(
            global::Windows.UI.Xaml.Controls.WebViewSeparateProcessLostEventArgs args)
        {
            return FromWebViewSeparateProcessLostEventArgs(args);
        }

        /// <summary>
        /// Creates a <see cref="WebViewSeparateProcessLostEventArgs"/> from <see cref="global::Windows.UI.Xaml.Controls.WebViewSeparateProcessLostEventArgs"/>.
        /// </summary>
        /// <param name="args">The <see cref="global::Windows.UI.Xaml.Controls.WebViewSeparateProcessLostEventArgs"/> instance containing the event data.</param>
        /// <returns><see cref="WebViewSeparateProcessLostEventArgs"/></returns>
        public static WebViewSeparateProcessLostEventArgs FromWebViewSeparateProcessLostEventArgs(global::Windows.UI.Xaml.Controls.WebViewSeparateProcessLostEventArgs args)
        {
            return new WebViewSeparateProcessLostEventArgs(args);
        }
    }
}