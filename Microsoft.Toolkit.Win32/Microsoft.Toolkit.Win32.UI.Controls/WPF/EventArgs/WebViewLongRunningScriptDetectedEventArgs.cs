using System;
using System.Security;
using Microsoft.Windows.Interop;

namespace Microsoft.Toolkit.Win32.UI.Controls.WPF.WebViewLite
{
    /// <summary>
    /// Provides data for events. This class cannot be inherited.
    /// </summary>
    /// <remarks>Copy from <see cref="global::Windows.UI.Xaml.Controls.WebViewLongRunningScriptDetectedEventArgs"/> to avoid requirement to link Windows.winmd</remarks>
    /// <seealso cref="global::Windows.UI.Xaml.Controls.WebViewLongRunningScriptDetectedEventArgs"/>
    public sealed class WebViewLongRunningScriptDetectedEventArgs : EventArgs
    {
        [SecurityCritical]
        private readonly global::Windows.UI.Xaml.Controls.WebViewLongRunningScriptDetectedEventArgs _args;

        [SecurityCritical]
        internal WebViewLongRunningScriptDetectedEventArgs(global::Windows.UI.Xaml.Controls.WebViewLongRunningScriptDetectedEventArgs args)
        {
            _args = args;
        }

        public bool StopPageScriptExecution
        {
            [SecurityCritical]
            get => (bool)_args.StopPageScriptExecution;
            [SecurityCritical]
            set => _args.StopPageScriptExecution = value;
        }

        public System.TimeSpan ExecutionTime
        {
            [SecurityCritical]
            get => (System.TimeSpan)_args.ExecutionTime;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="global::Windows.UI.Xaml.Controls.WebViewLongRunningScriptDetectedEventArgs"/> to <see cref="Microsoft.Toolkit.Win32.UI.Controls.WPF.WebViewLite.WebViewLongRunningScriptDetectedEventArgs"/>.
        /// </summary>
        /// <param name="args">The <see cref="global::Windows.UI.Xaml.Controls.WebViewLongRunningScriptDetectedEventArgs"/> instance containing the event data.</param>
        /// <returns>The result of the conversion.</returns>
        [SecurityCritical]
        public static implicit operator WebViewLongRunningScriptDetectedEventArgs(
            global::Windows.UI.Xaml.Controls.WebViewLongRunningScriptDetectedEventArgs args)
        {
            return FromWebViewLongRunningScriptDetectedEventArgs(args);
        }

        /// <summary>
        /// Creates a <see cref="WebViewLongRunningScriptDetectedEventArgs"/> from <see cref="global::Windows.UI.Xaml.Controls.WebViewLongRunningScriptDetectedEventArgs"/>.
        /// </summary>
        /// <param name="args">The <see cref="global::Windows.UI.Xaml.Controls.WebViewLongRunningScriptDetectedEventArgs"/> instance containing the event data.</param>
        /// <returns><see cref="WebViewLongRunningScriptDetectedEventArgs"/></returns>
        public static WebViewLongRunningScriptDetectedEventArgs FromWebViewLongRunningScriptDetectedEventArgs(global::Windows.UI.Xaml.Controls.WebViewLongRunningScriptDetectedEventArgs args)
        {
            return new WebViewLongRunningScriptDetectedEventArgs(args);
        }
    }
}