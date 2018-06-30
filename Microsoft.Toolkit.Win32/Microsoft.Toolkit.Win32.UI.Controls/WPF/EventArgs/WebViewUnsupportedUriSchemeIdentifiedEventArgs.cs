using System;
using System.Security;
using Microsoft.Windows.Interop;

namespace Microsoft.Toolkit.Win32.UI.Controls.WPF.WebViewLite
{
    /// <summary>
    /// Provides data for events. This class cannot be inherited.
    /// </summary>
    /// <remarks>Copy from <see cref="global::Windows.UI.Xaml.Controls.WebViewUnsupportedUriSchemeIdentifiedEventArgs"/> to avoid requirement to link Windows.winmd</remarks>
    /// <seealso cref="global::Windows.UI.Xaml.Controls.WebViewUnsupportedUriSchemeIdentifiedEventArgs"/>
    public sealed class WebViewUnsupportedUriSchemeIdentifiedEventArgs : EventArgs
    {
        [SecurityCritical]
        private readonly global::Windows.UI.Xaml.Controls.WebViewUnsupportedUriSchemeIdentifiedEventArgs _args;

        [SecurityCritical]
        internal WebViewUnsupportedUriSchemeIdentifiedEventArgs(global::Windows.UI.Xaml.Controls.WebViewUnsupportedUriSchemeIdentifiedEventArgs args)
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

        public System.Uri Uri
        {
            [SecurityCritical]
            get => (System.Uri)_args.Uri;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="global::Windows.UI.Xaml.Controls.WebViewUnsupportedUriSchemeIdentifiedEventArgs"/> to <see cref="Microsoft.Toolkit.Win32.UI.Controls.WPF.WebViewLite.WebViewUnsupportedUriSchemeIdentifiedEventArgs"/>.
        /// </summary>
        /// <param name="args">The <see cref="global::Windows.UI.Xaml.Controls.WebViewUnsupportedUriSchemeIdentifiedEventArgs"/> instance containing the event data.</param>
        /// <returns>The result of the conversion.</returns>
        [SecurityCritical]
        public static implicit operator WebViewUnsupportedUriSchemeIdentifiedEventArgs(
            global::Windows.UI.Xaml.Controls.WebViewUnsupportedUriSchemeIdentifiedEventArgs args)
        {
            return FromWebViewUnsupportedUriSchemeIdentifiedEventArgs(args);
        }

        /// <summary>
        /// Creates a <see cref="WebViewUnsupportedUriSchemeIdentifiedEventArgs"/> from <see cref="global::Windows.UI.Xaml.Controls.WebViewUnsupportedUriSchemeIdentifiedEventArgs"/>.
        /// </summary>
        /// <param name="args">The <see cref="global::Windows.UI.Xaml.Controls.WebViewUnsupportedUriSchemeIdentifiedEventArgs"/> instance containing the event data.</param>
        /// <returns><see cref="WebViewUnsupportedUriSchemeIdentifiedEventArgs"/></returns>
        public static WebViewUnsupportedUriSchemeIdentifiedEventArgs FromWebViewUnsupportedUriSchemeIdentifiedEventArgs(global::Windows.UI.Xaml.Controls.WebViewUnsupportedUriSchemeIdentifiedEventArgs args)
        {
            return new WebViewUnsupportedUriSchemeIdentifiedEventArgs(args);
        }
    }
}