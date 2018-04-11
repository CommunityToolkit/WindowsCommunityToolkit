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

#define DEBUG_FOCUS
using System;
using System.Collections;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using Microsoft.Toolkit.Win32.UI.Controls.Interop.Win32;

namespace Microsoft.Toolkit.Win32.UI.Controls.WPF
{
    /// <summary>
    /// The <see cref="WebViewHost"/> class hosts a WebView inside of a WPF tree
    /// </summary>
    /// <remarks>Requires unmanaged code permissions</remarks>
    [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
    public abstract class WebViewHost : HwndHost
    {
        static WebViewHost()
        {


            // register for access keys
            EventManager.RegisterClassHandler(typeof(WebViewHost), AccessKeyManager.AccessKeyPressedEvent, new AccessKeyPressedEventHandler(OnAccessKeyPressed));

            Control.IsTabStopProperty.OverrideMetadata(typeof(WebViewHost), new FrameworkPropertyMetadata(true));

            FocusableProperty.OverrideMetadata(typeof(WebViewHost), new FrameworkPropertyMetadata(true));

            KeyboardNavigation.TabNavigationProperty.OverrideMetadata(typeof(WebViewHost), new FrameworkPropertyMetadata(KeyboardNavigationMode.Once));
        }

        public WebViewHost()
        {
#if DEBUG_FOCUS
            GotFocus += (o, e) => { Debug.WriteLine($"GotFocus"); };
            GotKeyboardFocus += (o, e) => { Debug.WriteLine("GotKeyboardFocus"); };
            LostFocus += (o, e) => { Debug.WriteLine($"LostFocus"); };
            LostKeyboardFocus += (o, e) => { Debug.WriteLine("LostKeyboardFocus"); };
            KeyUp += (o, e) =>
            {
                Debug.WriteLine($"KeyUp: Key: {e.Key}, SystemKey: {e.SystemKey}");
            };
#endif
        }

        private delegate void PropertyInvalidator(WebViewHost webViewHost);

        protected HandleRef ChildWindow { get; private set; }

        protected HandleRef ParentHandle { get; private set; }

        public abstract void Close();

        protected override HandleRef BuildWindowCore(HandleRef hwndParent)
        {
            ParentHandle = hwndParent;

            NativeMethods.EnableMouseInPointer(true);

            // TODO: There is a case where a process could have already been created
            // TODO: Multiple WebViewControl instances
            if (ChildWindow.Handle == IntPtr.Zero)
            {
                // Create a simple STATIC HWND
                var windowHandle = NativeMethods.CreateWindow(
                    "Static",
                    WS.CHILD | WS.VISIBLE | WS.CLIPCHILDREN,
                    0,
                    0,
                    0,
                    0,
                    hwndParent.Handle);
                Verify.IsTrue(windowHandle != IntPtr.Zero, "Could not create child window");

                ChildWindow = new HandleRef(null, windowHandle);
            }

            Initialize();

            return ChildWindow;
        }

        protected abstract void Initialize();

        protected override void DestroyWindowCore(HandleRef hwnd)
        {
            NativeMethods.DestroyWindow(hwnd.Handle);
        }

        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    Close();
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        protected override Size MeasureOverride(Size constraint)
        {
            base.MeasureOverride(constraint);

            return new Size(
                !double.IsPositiveInfinity(constraint.Width) ? constraint.Width : 250.0,
                !double.IsPositiveInfinity(constraint.Height) ? constraint.Height : 250.0);
        }



        protected override bool TabIntoCore(TraversalRequest request)
        {
            // TODO: Assert should be at least InPlaceActive
            return base.TabIntoCore(request);
        }

        [SecurityCritical]
        [UIPermission(SecurityAction.LinkDemand, Unrestricted = true)]
        protected override bool TranslateAcceleratorCore(ref MSG msg, ModifierKeys modifiers)
        {
            Debug.WriteLine($"HWND: {msg.hwnd}, msg: {msg.message}, wParam: {msg.wParam}, lParam: {msg.lParam}");

            // TODO: Convert to Win32.WM
            // If TAB key was pressed
            if (msg.message == 0x0100 && msg.wParam.ToInt32() == 0x09)
            {
                // Handle case for SHIFT+TAB
                // Otherwise, TAB
            }

            return base.TranslateAcceleratorCore(ref msg, modifiers);
        }

        protected abstract void UpdateBounds(Rect bounds);

        protected override IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            Debug.WriteLine($"HWND: {hwnd}, msg: {msg} ({msg:x4}), wParam: {wParam}, lParam: {lParam}");

            return base.WndProc(hwnd, msg, wParam, lParam, ref handled);
        }

        private static void OnAccessKeyPressed(object sender, AccessKeyPressedEventArgs args)
        {
            if (args.Handled || args.Scope != null || args.Target != null)
            {
                return;
            }

            args.Target = (UIElement)sender;
        }
    }
}