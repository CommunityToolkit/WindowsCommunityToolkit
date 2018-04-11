using System;
using System.Security;

namespace Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT
{
    public class WebViewControlAcceleratorKeyPressedEventArgs : EventArgs
    {
        [SecurityCritical]
        private readonly Windows.Web.UI.Interop.WebViewControlAcceleratorKeyPressedEventArgs _args;

        [SecurityCritical]
        internal WebViewControlAcceleratorKeyPressedEventArgs(Windows.Web.UI.Interop.WebViewControlAcceleratorKeyPressedEventArgs args)
        {
            _args = args;
        }

        public CoreAcceleratorKeyEventType EventType
        {
            [SecurityCritical]
            get { return (CoreAcceleratorKeyEventType) _args.EventType; }
        }

        public bool Handled
        {
            [SecurityCritical]
            get => _args.Handled;
            [SecurityCritical]
            set => _args.Handled = value;
        }

        public WebViewControlAcceleratorKeyRoutingStage RoutingStage
        {
            [SecurityCritical]
            get { return (WebViewControlAcceleratorKeyRoutingStage) _args.RoutingStage; }
        }

        public VirtualKey VirtualKey
        {
            [SecurityCritical]
            get { return (VirtualKey) _args.VirtualKey; }
        }

        [SecurityCritical]
        public static implicit operator WebViewControlAcceleratorKeyPressedEventArgs(
            Windows.Web.UI.Interop.WebViewControlAcceleratorKeyPressedEventArgs args)
        {
            return new WebViewControlAcceleratorKeyPressedEventArgs(args);
        }
    }
}