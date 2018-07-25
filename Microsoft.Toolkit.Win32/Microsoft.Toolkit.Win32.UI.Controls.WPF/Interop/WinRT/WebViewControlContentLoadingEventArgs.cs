using System.Security;
using Microsoft.Toolkit.Win32.UI.Controls.WPF;

namespace Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT
{
    /// <summary>
    /// Contains WPF specific implementation for use with <see cref="WebViewCompatible"/>
    /// </summary>
    public sealed partial class WebViewControlContentLoadingEventArgs
    {
        private readonly System.Windows.Navigation.NavigatingCancelEventArgs _compatibleArgs;

        [SecurityCritical]
        internal WebViewControlContentLoadingEventArgs(System.Windows.Navigation.NavigatingCancelEventArgs args)
            : this((global::Windows.Web.UI.WebViewControlContentLoadingEventArgs)null)
        {
            _compatibleArgs = args;
            Uri = _compatibleArgs?.Uri;
        }

        /// <summary>
        /// Creates a <see cref="WebViewControlNavigationStartingEventArgs"/> from <see cref="System.Windows.Navigation.NavigatingCancelEventArgs"/>.
        /// </summary>
        /// <param name="args">The <see cref="System.Windows.Navigation.NavigatingCancelEventArgs"/> instance containing the event data.</param>
        /// <returns><see cref="WebViewControlContentLoadingEventArgs"/>.</returns>
        public static WebViewControlContentLoadingEventArgs ToWebViewControlContentLoadingEventArgs(
            System.Windows.Navigation.NavigatingCancelEventArgs args) =>
            new WebViewControlContentLoadingEventArgs(args);

        /// <summary>
        /// Performs an implicit conversion from <see cref="System.Windows.Navigation.NavigatingCancelEventArgs"/> to <see cref="WebViewControlContentLoadingEventArgs"/>.
        /// </summary>
        /// <param name="args">The <see cref="System.Windows.Navigation.NavigatingCancelEventArgs"/> instance containing the event data.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator WebViewControlContentLoadingEventArgs(System.Windows.Navigation.NavigatingCancelEventArgs args) => ToWebViewControlContentLoadingEventArgs(args);
    }
}
