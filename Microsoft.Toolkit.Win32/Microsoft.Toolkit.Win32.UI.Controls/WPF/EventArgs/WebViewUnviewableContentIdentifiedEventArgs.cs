using System;
using System.Security;
using Microsoft.Windows.Interop;

namespace Microsoft.Toolkit.Win32.UI.Controls.WPF.WebViewLite
{
    /// <summary>
    /// Provides data for events. This class cannot be inherited.
    /// </summary>
    /// <remarks>Copy from <see cref="global::Windows.UI.Xaml.Controls.WebViewUnviewableContentIdentifiedEventArgs"/> to avoid requirement to link Windows.winmd</remarks>
    /// <seealso cref="global::Windows.UI.Xaml.Controls.WebViewUnviewableContentIdentifiedEventArgs"/>
    public sealed class WebViewUnviewableContentIdentifiedEventArgs : EventArgs
    {
        [SecurityCritical]
        private readonly global::Windows.UI.Xaml.Controls.WebViewUnviewableContentIdentifiedEventArgs _args;

        [SecurityCritical]
        internal WebViewUnviewableContentIdentifiedEventArgs(global::Windows.UI.Xaml.Controls.WebViewUnviewableContentIdentifiedEventArgs args)
        {
            _args = args;
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

        public string MediaType
        {
            [SecurityCritical]
            get => (string)_args.MediaType;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="global::Windows.UI.Xaml.Controls.WebViewUnviewableContentIdentifiedEventArgs"/> to <see cref="Microsoft.Toolkit.Win32.UI.Controls.WPF.WebViewLite.WebViewUnviewableContentIdentifiedEventArgs"/>.
        /// </summary>
        /// <param name="args">The <see cref="global::Windows.UI.Xaml.Controls.WebViewUnviewableContentIdentifiedEventArgs"/> instance containing the event data.</param>
        /// <returns>The result of the conversion.</returns>
        [SecurityCritical]
        public static implicit operator WebViewUnviewableContentIdentifiedEventArgs(
            global::Windows.UI.Xaml.Controls.WebViewUnviewableContentIdentifiedEventArgs args)
        {
            return FromWebViewUnviewableContentIdentifiedEventArgs(args);
        }

        /// <summary>
        /// Creates a <see cref="WebViewUnviewableContentIdentifiedEventArgs"/> from <see cref="global::Windows.UI.Xaml.Controls.WebViewUnviewableContentIdentifiedEventArgs"/>.
        /// </summary>
        /// <param name="args">The <see cref="global::Windows.UI.Xaml.Controls.WebViewUnviewableContentIdentifiedEventArgs"/> instance containing the event data.</param>
        /// <returns><see cref="WebViewUnviewableContentIdentifiedEventArgs"/></returns>
        public static WebViewUnviewableContentIdentifiedEventArgs FromWebViewUnviewableContentIdentifiedEventArgs(global::Windows.UI.Xaml.Controls.WebViewUnviewableContentIdentifiedEventArgs args)
        {
            return new WebViewUnviewableContentIdentifiedEventArgs(args);
        }
    }
}