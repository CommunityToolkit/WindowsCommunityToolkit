// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

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
            get { return (CoreAcceleratorKeyEventType)_args.EventType; }
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
            get { return (WebViewControlAcceleratorKeyRoutingStage)_args.RoutingStage; }
        }

        public VirtualKey VirtualKey
        {
            [SecurityCritical]
            get { return (VirtualKey)_args.VirtualKey; }
        }

        [SecurityCritical]
        public static implicit operator WebViewControlAcceleratorKeyPressedEventArgs(
            Windows.Web.UI.Interop.WebViewControlAcceleratorKeyPressedEventArgs args)
        {
            return new WebViewControlAcceleratorKeyPressedEventArgs(args);
        }
    }
}