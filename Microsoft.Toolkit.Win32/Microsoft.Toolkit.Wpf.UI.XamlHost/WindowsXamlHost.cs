// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows;
using Microsoft.Toolkit.Win32.UI.XamlHost;

namespace Microsoft.Toolkit.Wpf.UI.XamlHost
{
    /// <summary>
    /// WindowsXamlHost control hosts UWP XAML content inside the Windows Presentation Foundation
    /// </summary>
    public class WindowsXamlHost : WindowsXamlHostBase
    {
        /// <summary>
        /// Gets XAML Content by type name
        /// </summary>
        public static DependencyProperty InitialTypeNameProperty { get; } = DependencyProperty.Register("InitialTypeName", typeof(string), typeof(WindowsXamlHost));

        /// <summary>
        /// Gets or sets XAML Content by type name
        /// </summary>
        /// <example><code>XamlClassLibrary.MyUserControl</code></example>
        /// <remarks>
        /// Content creation is deferred until after the parent hwnd has been created.
        /// </remarks>
        [Browsable(true)]
        [Category("XAML")]
        public string InitialTypeName
        {
            get => (string)GetValue(InitialTypeNameProperty);

            set => SetValue(InitialTypeNameProperty, value);
        }

        /// <summary>
        /// Gets or sets the root UWP XAML element displayed in the WPF control instance.
        /// </summary>
        /// <remarks>This UWP XAML element is the root element of the wrapped DesktopWindowXamlSource.</remarks>
        [Browsable(true)]
        public Windows.UI.Xaml.UIElement Child
        {
            get => ChildInternal;

            set => ChildInternal = value;
        }

        /// <summary>
        /// Creates <see cref="Windows.UI.Xaml.Application" /> object, wrapped <see cref="Windows.UI.Xaml.Hosting.DesktopWindowXamlSource" /> instance; creates and
        /// sets root UWP XAML element on DesktopWindowXamlSource.
        /// </summary>
        /// <param name="hwndParent">Parent window handle</param>
        /// <returns>Handle to XAML window</returns>
        protected override HandleRef BuildWindowCore(HandleRef hwndParent)
        {
            // Create and set initial root UWP XAML content
            if (!string.IsNullOrEmpty(InitialTypeName) && Child == null)
            {
                Child = UWPTypeFactory.CreateXamlContentByType(InitialTypeName);

                var frameworkElement = Child as Windows.UI.Xaml.FrameworkElement;

                // Default to stretch : UWP XAML content will conform to the size of WindowsXamlHost
                if (frameworkElement != null)
                {
                    frameworkElement.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Stretch;
                    frameworkElement.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Stretch;
                }
            }

            return base.BuildWindowCore(hwndParent);
        }

        /// <inheritdoc />
        protected override void Dispose(bool disposing)
        {
            if (disposing && !IsDisposed)
            {
                if (Child is Windows.UI.Xaml.FrameworkElement frameworkElement)
                {
                    frameworkElement.SizeChanged -= XamlContentSizeChanged;
                }

                base.Dispose(disposing);
            }
        }

        protected override System.IntPtr WndProc(System.IntPtr hwnd, int msg, System.IntPtr wParam, System.IntPtr lParam, ref bool handled)
        {
            const int WM_GETOBJECT = 0x003D;
            switch (msg)
            {
                // We don't want HwndHost to handle the WM_GETOBJECT.
                // Instead we want to let the HwndIslandSite's WndProc get it
                // So return handled = false and don't let the base class do
                // anything on that message.
                case WM_GETOBJECT:
                    handled = false;
                    return System.IntPtr.Zero;
            }

            return base.WndProc(hwnd, msg, wParam, lParam, ref handled);
        }
    }
}
