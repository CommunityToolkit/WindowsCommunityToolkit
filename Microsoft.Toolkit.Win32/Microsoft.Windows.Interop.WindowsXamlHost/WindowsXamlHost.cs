// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Data;
using System.Windows.Interop;

namespace Microsoft.Toolkit.Win32.UI.Interop
{
    /// <summary>
    /// WindowsXamlHost control hosts UWP XAML content inside the Windows Presentation Foundation
    /// </summary>
    public partial class WindowsXamlHost : HwndHost
    {
        /// <summary>
        /// UWP XAML Application instance and root UWP XamlMetadataProvider.  Custom implementation required to
        /// probe at runtime for custom UWP XAML type information.  This must be created before
        /// creating any DesktopWindowXamlSource instances if custom UWP XAML types are required.
        /// </summary>
        [ThreadStatic]
        private readonly Windows.UI.Xaml.Application _application;

        /// <summary>
        /// XAML Content by type name : MyNamespace.MyClass.MyType
        /// ex: XamlClassLibrary.MyUserControl
        /// </summary>
        public static DependencyProperty TypeNameProperty = DependencyProperty.Register(nameof(TypeName), typeof(string), typeof(WindowsXamlHost));

        /// <summary>
        /// Root UWP XAML element displayed in the WindowsXamlHost control.  This UWP XAML element is
        /// the root element of the wrapped DesktopWindowXamlSource instance.
        /// </summary>
        public static DependencyProperty XamlRootProperty = DependencyProperty.Register(nameof(XamlRoot), typeof(Windows.UI.Xaml.UIElement), typeof(WindowsXamlHost));


        /// <summary>
        /// UWP XAML DesktopWindowXamlSource instance that hosts XAML content in a win32 application
        /// </summary>
        public Windows.UI.Xaml.Hosting.DesktopWindowXamlSource DesktopWindowXamlSource;

        /// <summary>
        /// Has this wrapper control instance been disposed?
        /// </summary>
        private bool IsDisposed { get; set; }

        /// <summary>
        /// A reference count on the UWP XAML framework is tied to WindowsXamlManager's
        /// lifetime.  UWP XAML is spun up on the first WindowsXamlManager creation and
        /// deinitialized when the last instance of WindowsXamlManager is destroyed.
        /// </summary>
        private Windows.UI.Xaml.Hosting.WindowsXamlManager _windowsXamlManager;

        public WindowsXamlHost(string typeName)
            : this()
        {
            TypeName = typeName;

            // Create and set initial root UWP XAML content
            if (TypeName != null)
            {
                XamlRoot = CreateXamlContentByType(TypeName);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WindowsXamlHost"/> class.
        /// Initializes a new instance of the WindowsXamlHost class: default constructor is required for use in WPF markup.
        /// (When the default constructor is called, object properties have not been set. Put WPF logic in OnInitialized.)
        /// </summary>
        public WindowsXamlHost()
        {
            // Create a custom UWP XAML Application object that implements reflection-based XAML metdata probing.
            // Instantiation of the application object must occur before creating the DesktopWindowXamlSource instance.
            // DesktopWindowXamlSource will create a generic Application object unable to load custom UWP XAML metadata.
            if (_application == null)
            {
                try
                {
                    // global::Windows.UI.Xaml.Application.Current may throw if DXamlCore has not been initialized.
                    // Treat the exception as an uninitialized global::Windows.UI.Xaml.Application condition.
                    _application = Windows.UI.Xaml.Application.Current as XamlApplication;
                }
                catch
                {
                    _application = new XamlApplication();
                }
            }

            // Create an instance of the WindowsXamlManager. This initializes and holds a
            // reference on the UWP XAML DXamlCore and must be explicitly created before
            // any UWP XAML types are programmatically created.  If WindowsXamlManager has
            // not been created before creating DesktopWindowXamlSource, DesktopWindowXaml source
            // will create an instance of WindowsXamlManager internally.  (Creation is explicit
            // here to illustrate how to initialize UWP XAML before initializing the DesktopWindowXamlSource.)
            _windowsXamlManager = Windows.UI.Xaml.Hosting.WindowsXamlManager.InitializeForCurrentThread();

            // Create DesktopWindowXamlSource, host for UWP XAML content
            DesktopWindowXamlSource = new Windows.UI.Xaml.Hosting.DesktopWindowXamlSource();

            // Hook OnTakeFocus event for Focus processing
            DesktopWindowXamlSource.TakeFocusRequested += OnTakeFocusRequested;
        }

        /// <summary>
        /// Binds this wrapper object's exposed WPF DependencyProperty with the wrapped UWP object's DependencyProperty
        /// for what becomes effectively a two-way binding.
        /// </summary>
        /// <param name="propertyName">the registered name of the dependency property</param>
        /// <param name="wpfProperty">the DependencyProperty of the wrapper</param>
        /// <param name="uwpProperty">the related DependencyProperty of the UWP control</param>
        /// <param name="converter">a converter, if one's needed</param>
        public void Bind(string propertyName, DependencyProperty wpfProperty, Windows.UI.Xaml.DependencyProperty uwpProperty, object converter = null, BindingDirection direction = BindingDirection.TwoWay)
        {
            if (direction == BindingDirection.TwoWay)
            {
                var binder = new Windows.UI.Xaml.Data.Binding()
                {
                    Source = this,
                    Path = new Windows.UI.Xaml.PropertyPath(propertyName),
                    Converter = (Windows.UI.Xaml.Data.IValueConverter)converter
                };
                Windows.UI.Xaml.Data.BindingOperations.SetBinding(XamlRoot, uwpProperty, binder);
            }

            var rebinder = new Binding()
            {
                Source = XamlRoot,
                Path = new PropertyPath(propertyName),
                Converter = (IValueConverter)converter
            };
            BindingOperations.SetBinding(this, wpfProperty, rebinder);
        }

        /// <summary>
        /// Creates initial UWP XAML content if TypeName has been set
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            // Create and set initial root UWP XAML content
            if (TypeName != null && XamlRoot == null)
            {
                XamlRoot = CreateXamlContentByType(TypeName);
            }
        }

        /// <summary>
        ///     Fired when WindowsXamlHost root UWP XAML content has been updated
        /// </summary>
        public event EventHandler XamlContentUpdated;

        /// <summary>
        /// Gets or sets XAML Content by type name : MyNamespace.MyClass.MyType
        /// ex: XamlClassLibrary.MyUserControl
        /// (Content creation is deferred until after the parent hwnd has been created.)
        /// </summary>
        [Browsable(true)]
        [Category("XAML")]
        public virtual string TypeName
        {
            get
            {
                return (string)GetValue(TypeNameProperty);
            }

            set
            {
                SetValue(TypeNameProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the root UWP XAML element displayed in the WPF control instance.  This UWP XAML element is
        /// the root element of the wrapped DesktopWindowXamlSource.
        /// </summary>
        [Browsable(true)]
        public virtual Windows.UI.Xaml.UIElement XamlRoot
        {
            get
            {
                return (Windows.UI.Xaml.UIElement)GetValue(XamlRootProperty);
            }

            set
            {
                // TODO: Fix and cleanup this entire method. Remove unnecessary layout events.
                TraceSource.TraceEvent(TraceEventType.Verbose, 0, "Setting Content...");

                if (value == (Windows.UI.Xaml.UIElement)GetValue(XamlRootProperty))
                {
                    TraceSource.TraceEvent(TraceEventType.Verbose, 0, "Content: Content unchanged.");

                    return;
                }

                var currentRoot = (Windows.UI.Xaml.FrameworkElement)GetValue(XamlRootProperty);
                if (currentRoot != null)
                {
                    // COM object separated from its current RCW cannot be used. Don't try to set
                    // Content to NULL after DesktopWindowXamlSource has been destroyed.
                    currentRoot.LayoutUpdated -= XamlContentLayoutUpdated;

                    currentRoot.SizeChanged -= XamlContentSizeChanged;
                }

                // TODO: Add special case for NULL Content.  This should resize the HwndIslandSite to 0, 0.
                SetValue(XamlRootProperty, value);
                value?.SetWrapper(this);

                if (DesktopWindowXamlSource != null)
                {
                    DesktopWindowXamlSource.Content = value;
                }

                var frameworkElement = value as Windows.UI.Xaml.FrameworkElement;
                if (frameworkElement != null)
                {
                    // If XAML content has changed, check XAML size
                    // to determine if WindowsXamlHost needs to re-run layout.
                    frameworkElement.LayoutUpdated += XamlContentLayoutUpdated;
                    frameworkElement.SizeChanged += XamlContentSizeChanged;

                    // WindowsXamlHost DataContext should flow through to UWP XAML content
                    frameworkElement.DataContext = DataContext;
                }

                // Fire updated event
                if (XamlContentUpdated != null)
                {
                    XamlContentUpdated(this, new EventArgs());
                }
            }
        }

        public static Windows.UI.Xaml.DependencyProperty WrapperProperty { get; } =
            Windows.UI.Xaml.DependencyProperty.RegisterAttached("Wrapper", typeof(UIElement), typeof(WindowsXamlHost), new Windows.UI.Xaml.PropertyMetadata(null));


        /// <summary>
        /// Creates global::Windows.UI.Xaml.Application object, wrapped DesktopWindowXamlSource instance; creates and
        /// sets root UWP XAML element on DesktopWindowXamlSource.
        /// </summary>
        /// <param name="hwndParent">Parent window handle</param>
        /// <returns>Handle to XAML window</returns>
        protected override HandleRef BuildWindowCore(HandleRef hwndParent)
        {
            // 'EnableMouseInPointer' is called by the WindowsXamlManager during initialization. No need
            // to call it directly here.

            // Create DesktopWindowXamlSource instance
            var desktopWindowXamlSourceNative = DesktopWindowXamlSource.GetInterop();

            // Associate the window where UWP XAML will display content
            desktopWindowXamlSourceNative.AttachToWindow(hwndParent.Handle);

            var windowHandle = desktopWindowXamlSourceNative.WindowHandle;

            // Overridden function must return window handle of new target window (DesktopWindowXamlSource's Window)
            return new HandleRef(this, windowHandle);
        }

        /// <summary>
        /// WPF framework request to destroy control window.  Cleans up the HwndIslandSite created by DesktopWindowXamlSource
        /// </summary>
        /// <param name="hwnd">Handle of window to be destroyed</param>
        protected override void DestroyWindowCore(HandleRef hwnd)
        {
            Dispose(true);
        }

        /// <summary>
        /// WindowsXamlHost Dispose
        /// </summary>
        /// <param name="disposing">Is disposing?</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && !IsDisposed)
            {
                IsDisposed = true;
                DesktopWindowXamlSource.TakeFocusRequested -= OnTakeFocusRequested;
                XamlRoot = null;
                DesktopWindowXamlSource.Dispose();
                DesktopWindowXamlSource = null;
            }
        }
    }
}
