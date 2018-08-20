// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace Microsoft.Toolkit.Win32.UI.Interop.WPF
{
    /// <summary>
    /// WindowsXamlHost control hosts UWP XAML content inside the Windows Presentation Foundation
    /// </summary>
    public class WindowsXamlHost : WindowsXamlHostBase
    {
        /// <summary>
        /// Gets xAML Content by type name : MyNamespace.MyClass.MyType
        /// ex: XamlClassLibrary.MyUserControl
        /// </summary>
        public static DependencyProperty InitialTypeNameProperty { get; } = DependencyProperty.Register("InitialTypeName", typeof(string), typeof(WindowsXamlHost));

        /// <summary>
        ///     Fired when WindowsXamlHost root UWP XAML content has been updated
        /// </summary>
        public event EventHandler XamlRootChanged;

        /// <summary>
        /// Gets or sets XAML Content by type name : MyNamespace.MyClass.MyType
        /// ex: XamlClassLibrary.MyUserControl
        /// (Content creation is deferred until after the parent hwnd has been created.)
        /// </summary>
        [Browsable(true)]
        [Category("XAML")]
        public string InitialTypeName
        {
            get => (string)GetValue(InitialTypeNameProperty);

            set => SetValue(InitialTypeNameProperty, value);
        }

        /// <summary>
        /// Gets or sets the root UWP XAML element displayed in the WPF control instance.  This UWP XAML element is
        /// the root element of the wrapped DesktopWindowXamlSource.
        /// </summary>
        [Browsable(true)]
        public Windows.UI.Xaml.UIElement XamlRoot
        {
            get => XamlRootInternal;

            set
            {
                if (value == XamlRootInternal)
                {
                    return;
                }

                var currentRoot = (Windows.UI.Xaml.FrameworkElement)XamlRootInternal;
                if (currentRoot != null)
                {
                    currentRoot.SizeChanged -= XamlContentSizeChanged;
                }

                XamlRootInternal = value;

                SetContent();

                var frameworkElement = XamlRootInternal as Windows.UI.Xaml.FrameworkElement;
                if (frameworkElement != null)
                {
                    // If XAML content has changed, check XAML size
                    // to determine if WindowsXamlHost needs to re-run layout.
                    frameworkElement.SizeChanged += XamlContentSizeChanged;

                    // WindowsXamlHost DataContext should flow through to UWP XAML content
                    frameworkElement.DataContext = DataContext;
                }

                // Fire updated event
                XamlRootChanged?.Invoke(this, new EventArgs());
            }
        }

        /// <summary>
        /// Creates global::Windows.UI.Xaml.Application object, wrapped DesktopWindowXamlSource instance; creates and
        /// sets root UWP XAML element on DesktopWindowXamlSource.
        /// </summary>
        /// <param name="hwndParent">Parent window handle</param>
        /// <returns>Handle to XAML window</returns>
        protected override HandleRef BuildWindowCore(HandleRef hwndParent)
        {
            // Create and set initial root UWP XAML content
            if (!string.IsNullOrEmpty(InitialTypeName) && XamlRoot == null)
            {
                XamlRoot = UWPTypeFactory.CreateXamlContentByType(InitialTypeName);

                var frameworkElement = XamlRoot as Windows.UI.Xaml.FrameworkElement;

                // TODO: Check frameworkElement is not NULL

                // Default to stretch : UWP XAML content will conform to the size of WindowsXamlHost
                frameworkElement.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Stretch;
                frameworkElement.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Stretch;
            }

            return base.BuildWindowCore(hwndParent);
        }
    }
}
