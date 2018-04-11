using System;
using System.Security;

namespace Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT
{
    public sealed class WebViewControlScriptNotifyEventArgs : EventArgs
    {
        [SecurityCritical]
        private readonly Windows.Web.UI.WebViewControlScriptNotifyEventArgs _args;

        internal WebViewControlScriptNotifyEventArgs(Windows.Web.UI.WebViewControlScriptNotifyEventArgs args)
        {
            _args = args;
        }

        public Uri Uri => _args.Uri;
        public string Value => _args.Value;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates")]
        public static implicit operator WebViewControlScriptNotifyEventArgs(
            Windows.Web.UI.WebViewControlScriptNotifyEventArgs args)
        {
            return new WebViewControlScriptNotifyEventArgs(args);
        }
    }
}