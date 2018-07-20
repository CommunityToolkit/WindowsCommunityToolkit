// <copyright file="WindowsXamlHost.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// <author>Microsoft</author>

namespace Microsoft.Windows.Interop
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Runtime.InteropServices;
    using System.Windows;
    using System.Windows.Interop;

    /// <summary>
    /// WindowsXamlHost control hosts UWP XAML content inside the Windows Presentation Foundation
    /// </summary>
    public partial class WindowsXamlHost : WindowsXamlHostBase
    {
        #region DependencyProperties

        /// <summary>
        /// XAML Content by type name : MyNamespace.MyClass.MyType
        /// ex: XamlClassLibrary.MyUserControl
        /// </summary>
        public static DependencyProperty InitialTypeNameProperty = DependencyProperty.Register("InitialTypeName", typeof(string), typeof(WindowsXamlHost));

        #endregion

        #region Constructors and Initialization
        /// <summary>
        /// Initializes a new instance of the WindowsXamlHost class: default constructor is required for use in WPF markup.
        /// (When the default constructor is called, object properties have not been set. Put WPF logic in OnInitialized.)
        /// </summary>
        public WindowsXamlHost() : base()
        {
        }

        #endregion

        #region Events
        /// <summary>
        ///     Fired when WindowsXamlHost root UWP XAML content has been updated
        /// </summary>
        public event EventHandler XamlRootChanged;
        #endregion

        #region Properties
 
        /// <summary>
        /// Gets or sets XAML Content by type name : MyNamespace.MyClass.MyType
        /// ex: XamlClassLibrary.MyUserControl
        /// (Content creation is deferred until after the parent hwnd has been created.)
        /// </summary>
        [Browsable(true)]
        [Category("XAML")]
        public string InitialTypeName
        {
            get
            {
                return (string)GetValue(InitialTypeNameProperty);
            }

            set
            {
                SetValue(InitialTypeNameProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the root UWP XAML element displayed in the WPF control instance.  This UWP XAML element is 
        /// the root element of the wrapped DesktopWindowXamlSource.
        /// </summary>
        [Browsable(true)]
        public global::Windows.UI.Xaml.UIElement XamlRoot
        {
            get
            {
                return this.xamlRoot;
            }

            set
            {
                if (value == this.xamlRoot)
                {
                    return;
                }

                global::Windows.UI.Xaml.FrameworkElement currentRoot = (global::Windows.UI.Xaml.FrameworkElement)this.xamlRoot;
                if (currentRoot != null)
                { 
                    currentRoot.SizeChanged -= this.XamlContentSizeChanged;
                }

                this.xamlRoot = value;

                if (this.desktopWindowXamlSource != null)
                {
                    this.desktopWindowXamlSource.Content = this.xamlRoot;
                }

                global::Windows.UI.Xaml.FrameworkElement frameworkElement = this.xamlRoot as global::Windows.UI.Xaml.FrameworkElement;
                if (frameworkElement != null)
                {
                    // If XAML content has changed, check XAML size 
                    // to determine if WindowsXamlHost needs to re-run layout.
                    frameworkElement.SizeChanged += this.XamlContentSizeChanged;

                    // WindowsXamlHost DataContext should flow through to UWP XAML content
                    frameworkElement.DataContext = this.DataContext;
                }

                // Fire updated event
                this.XamlRootChanged?.Invoke(this, new System.EventArgs());
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Creates global::Windows.UI.Xaml.Application object, wrapped DesktopWindowXamlSource instance; creates and
        /// sets root UWP XAML element on DesktopWindowXamlSource.
        /// </summary>
        /// <param name="hwndParent">Parent window handle</param>
        /// <returns>Handle to XAML window</returns>
        protected override HandleRef BuildWindowCore(HandleRef hwndParent)
        {
            // Create and set initial root UWP XAML content
            if (!string.IsNullOrEmpty(this.InitialTypeName) && this.XamlRoot == null)
            {
                this.XamlRoot = UWPTypeFactory.CreateXamlContentByType(this.InitialTypeName);

                global::Windows.UI.Xaml.FrameworkElement frameworkElement = this.XamlRoot as global::Windows.UI.Xaml.FrameworkElement;

                // Default to stretch : UWP XAML content will conform to the size of WindowsXamlHost
                frameworkElement.HorizontalAlignment = global::Windows.UI.Xaml.HorizontalAlignment.Stretch;
                frameworkElement.VerticalAlignment = global::Windows.UI.Xaml.VerticalAlignment.Stretch;
            }

            return base.BuildWindowCore(hwndParent);
        }

        #endregion
    }
}
