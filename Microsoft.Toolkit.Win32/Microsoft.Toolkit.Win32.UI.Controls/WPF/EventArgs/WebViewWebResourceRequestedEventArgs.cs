using System;
using System.Security;
using Microsoft.Windows.Interop;

namespace Microsoft.Toolkit.Win32.UI.Controls.WPF.WebViewLite
{
    /// <summary>
    /// Provides data for events. This class cannot be inherited.
    /// </summary>
    /// <remarks>Copy from <see cref="global::Windows.UI.Xaml.Controls.WebViewWebResourceRequestedEventArgs"/> to avoid requirement to link Windows.winmd</remarks>
    /// <seealso cref="global::Windows.UI.Xaml.Controls.WebViewWebResourceRequestedEventArgs"/>
    public sealed class WebViewWebResourceRequestedEventArgs : EventArgs
    {
        [SecurityCritical]
        private readonly global::Windows.UI.Xaml.Controls.WebViewWebResourceRequestedEventArgs _args;

        [SecurityCritical]
        internal WebViewWebResourceRequestedEventArgs(global::Windows.UI.Xaml.Controls.WebViewWebResourceRequestedEventArgs args)
        {
            _args = args;
        }

        public global::Windows.Web.Http.HttpResponseMessage Response
        {
            [SecurityCritical]
            get => (global::Windows.Web.Http.HttpResponseMessage)_args.Response;
            [SecurityCritical]
            set => _args.Response = value;
        }

        public global::Windows.Web.Http.HttpRequestMessage Request
        {
            [SecurityCritical]
            get => (global::Windows.Web.Http.HttpRequestMessage)_args.Request;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="global::Windows.UI.Xaml.Controls.WebViewWebResourceRequestedEventArgs"/> to <see cref="Microsoft.Toolkit.Win32.UI.Controls.WPF.WebViewLite.WebViewWebResourceRequestedEventArgs"/>.
        /// </summary>
        /// <param name="args">The <see cref="global::Windows.UI.Xaml.Controls.WebViewWebResourceRequestedEventArgs"/> instance containing the event data.</param>
        /// <returns>The result of the conversion.</returns>
        [SecurityCritical]
        public static implicit operator WebViewWebResourceRequestedEventArgs(
            global::Windows.UI.Xaml.Controls.WebViewWebResourceRequestedEventArgs args)
        {
            return FromWebViewWebResourceRequestedEventArgs(args);
        }

        /// <summary>
        /// Creates a <see cref="WebViewWebResourceRequestedEventArgs"/> from <see cref="global::Windows.UI.Xaml.Controls.WebViewWebResourceRequestedEventArgs"/>.
        /// </summary>
        /// <param name="args">The <see cref="global::Windows.UI.Xaml.Controls.WebViewWebResourceRequestedEventArgs"/> instance containing the event data.</param>
        /// <returns><see cref="WebViewWebResourceRequestedEventArgs"/></returns>
        public static WebViewWebResourceRequestedEventArgs FromWebViewWebResourceRequestedEventArgs(global::Windows.UI.Xaml.Controls.WebViewWebResourceRequestedEventArgs args)
        {
            return new WebViewWebResourceRequestedEventArgs(args);
        }
    }
}