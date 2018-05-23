// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using Microsoft.Toolkit.Win32.UI.Controls.Interop;
using Microsoft.Toolkit.Win32.UI.Controls.Interop.Win32;

namespace Microsoft.Toolkit.Win32.UI.Controls.WPF
{
    /// <summary>
    /// The <see cref="WebViewHost"/> class hosts a <see cref="WebView"/> inside of a WPF tree
    /// </summary>
    /// <remarks>Requires unmanaged code permissions</remarks>
    [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
    [ToolboxItem(false)]
    [DesignTimeVisible(false)]
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

        /// <summary>
        /// Initializes a new instance of the <see cref="WebViewHost"/> class.
        /// </summary>
        protected WebViewHost()
        {
            DpiHelper.Initialize();
            DpiHelper.SetPerMonitorDpiAwareness();

            // Get system DPI
            DeviceDpi = DpiHelper.DeviceDpi;

            DpiChanged += OnDpiChanged;
            SizeChanged += OnSizeChanged;

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

        /// <summary>
        /// Gets the child window.
        /// </summary>
        /// <value>The child window created during <see cref="Initialize"/>.</value>
        protected HandleRef ChildWindow { get; private set; }

        /// <summary>
        /// Gets the current DPI for this control
        /// </summary>
        /// <seealso cref="IsScalingRequired"/>
        protected int DeviceDpi { get; private set; }

        /// <summary>
        /// Gets a value indicating whether scaling is required for the current DPI.
        /// </summary>
        /// <value><see langword="true"/> if scaling is required; otherwise, <see langword="false"/>.</value>
        protected bool IsScalingRequired => DeviceDpi != DpiHelper.LogicalDpi;

        /// <summary>
        /// Gets the parent handle.
        /// </summary>
        /// <value>The <see cref="HandleRef"/> passed in to <see cref="BuildWindowCore"/>.</value>
        protected HandleRef ParentHandle { get; private set; }

        /// <summary>
        /// Closes the <see cref="WebViewHost"/>.
        /// </summary>
        /// <seealso cref="IDisposable.Dispose"/>
        public abstract void Close();

        /// <summary>
        /// When overridden in a derived class, creates the window to be hosted.
        /// </summary>
        /// <param name="hwndParent">The window handle of the parent window.</param>
        /// <returns>The handle to the child Win32 window to create.</returns>
        /// <seealso cref="Initialize"/>
        protected override HandleRef BuildWindowCore(HandleRef hwndParent)
        {
            ParentHandle = hwndParent;
            NativeMethods.EnableMouseInPointer(true);

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

        /// <summary>
        /// Destroys the hosted window.
        /// </summary>
        /// <param name="hwnd">A structure that contains the window handle.</param>
        protected override void DestroyWindowCore(HandleRef hwnd)
        {
            NativeMethods.DestroyWindow(hwnd.Handle);
        }

        /// <summary>
        /// Immediately frees any system resources that the <see cref="WebViewHost"/> might hold.
        /// </summary>
        /// <param name="disposing">Set to <see langword="true" /> if called from an explicit disposer and <see langword="false" /> otherwise.</param>
        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    Close();
                    DpiChanged -= OnDpiChanged;
                    SizeChanged -= OnSizeChanged;
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        protected abstract void Initialize();

        /// <summary>
        /// Processes keyboard input at the keydown message level.
        /// </summary>
        /// <param name="msg">The message and associated data. Do not modify this structure. It is passed by reference for performance reasons only.</param>
        /// <param name="modifiers">Modifier keys.</param>
        /// <returns>Always returns <see langword="false" />.</returns>
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

        /// <summary>
        /// Updates the location and size of <see cref="WebViewHost"/>.
        /// </summary>
        /// <param name="bounds">A <see cref="Rect"/> containing numerical values that represent the location and size of the control.</param>
        protected abstract void UpdateBounds(Rect bounds);

        /// <summary>
        /// Updates the size of the <see cref="WebViewHost"/> using the current visual offset for location.
        /// </summary>
        /// <param name="size">A <see cref="Size" /> containing numerical values that represent the size of the control.</param>
        protected virtual void UpdateSize(Size size)
        {
            var rect = new Rect(
                VisualOffset.X,
                VisualOffset.Y,
                size.Width,
                size.Height);
            UpdateBounds(rect);
        }

        /// <summary>
        /// When overridden in a derived class, accesses the window process (handle) of the hosted child window.
        /// </summary>
        /// <param name="hwnd">The window handle of the hosted window.</param>
        /// <param name="msg">The message to act upon.</param>
        /// <param name="wParam">Information that may be relevant to handling the message. This is typically used to store small pieces of information, such as flags.</param>
        /// <param name="lParam">Information that may be relevant to handling the message. This is typically used to reference an object.</param>
        /// <param name="handled">Whether events resulting should be marked handled.</param>
        /// <returns>The window handle of the child window.</returns>
        protected override IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            Debug.WriteLine($"HWND: {hwnd}, msg: {msg} (0x{msg:x4}), wParam: {wParam}, lParam: {lParam}");

            switch (msg)
            {
                // WM_DPICHANGED
                // WM_DPICHANGED_BEFOREPARENT
                // WM_DPICHANGED_AFTERPARENT
                default:
                    return base.WndProc(hwnd, msg, wParam, lParam, ref handled);
            }
        }

        private static void OnAccessKeyPressed(object sender, AccessKeyPressedEventArgs args)
        {
            if (args.Handled || args.Scope != null || args.Target != null)
            {
                return;
            }

            args.Target = (UIElement)sender;
        }

        private void OnDpiChanged(object o, DpiChangedEventArgs e)
        {
#if DEBUG_LAYOUT
            Debug.WriteLine("Old DPI: ({0}, {1}), New DPI: ({2}, {3})", e.OldDpi.DpiScaleX, e.OldDpi.DpiScaleY, e.NewDpi.DpiScaleX, e.NewDpi.DpiScaleY);
#endif
            Verify.AreEqual(DeviceDpi, e.OldDpi.PixelsPerInchX);
            DeviceDpi = (int)e.NewDpi.PixelsPerInchX;
        }

        private void OnSizeChanged(object o, SizeChangedEventArgs e)
        {
            UpdateSize(e.NewSize);
        }
    }
}