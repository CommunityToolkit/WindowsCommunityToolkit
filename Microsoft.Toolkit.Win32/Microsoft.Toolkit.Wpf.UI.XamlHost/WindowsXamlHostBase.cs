// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using Microsoft.Toolkit.Win32.UI.XamlHost;
using Windows.UI.Xaml;

namespace Microsoft.Toolkit.Wpf.UI.XamlHost
{
    /// <summary>
    /// WindowsXamlHost control hosts UWP XAML content inside the Windows Presentation Foundation
    /// </summary>
    public abstract partial class WindowsXamlHostBase : HwndHost
    {
        /// <summary>
        /// UWP XAML Application instance and root UWP XamlMetadataProvider.  Custom implementation required to
        /// probe at runtime for custom UWP XAML type information.  This must be created before
        /// creating any DesktopWindowXamlSource instances if custom UWP XAML types are required.
        /// </summary>
        private readonly Windows.UI.Xaml.Application _application;

        /// <summary>
        /// UWP XAML DesktopWindowXamlSource instance that hosts XAML content in a win32 application
        /// </summary>
        private readonly Windows.UI.Xaml.Hosting.DesktopWindowXamlSource _xamlSource;

        /// <summary>
        /// A reference count on the UWP XAML framework is tied to WindowsXamlManager's
        /// lifetime.  UWP XAML is spun up on the first WindowsXamlManager creation and
        /// deinitialized when the last instance of WindowsXamlManager is destroyed.
        /// </summary>
        private readonly Windows.UI.Xaml.Hosting.WindowsXamlManager _windowsXamlManager;

        /// <summary>
        /// Private field that backs ChildInternal property.
        /// </summary>
        private UIElement _childInternal;

        /// <summary>
        ///     Fired when WindowsXamlHost root UWP XAML content has been updated
        /// </summary>
        public event EventHandler ChildChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="WindowsXamlHostBase"/> class.
        /// </summary>
        /// <remarks>
        /// Default constructor is required for use in WPF markup. When the default constructor is called,
        /// object properties have not been set. Put WPF logic in OnInitialized.
        /// </remarks>
        public WindowsXamlHostBase()
        {
            // Windows.UI.Xaml.Application object is required for loading custom control metadata.  If a custom
            // Application object is not provided by the application, the host control will create one (XamlApplication).
            // Instantiation of the application object must occur before creating the DesktopWindowXamlSource instance.
            // If no Application object is created before DesktopWindowXamlSource is created, DestkopWindowXamlSource
            // will create a generic Application object unable to load custom UWP XAML metadata.
            Microsoft.Toolkit.Win32.UI.XamlHost.XamlApplication.GetOrCreateXamlApplicationInstance(ref _application);

            // Create an instance of the WindowsXamlManager. This initializes and holds a
            // reference on the UWP XAML DXamlCore and must be explicitly created before
            // any UWP XAML types are programmatically created.  If WindowsXamlManager has
            // not been created before creating DesktopWindowXamlSource, DesktopWindowXaml source
            // will create an instance of WindowsXamlManager internally.  (Creation is explicit
            // here to illustrate how to initialize UWP XAML before initializing the DesktopWindowXamlSource.)
            _windowsXamlManager = Windows.UI.Xaml.Hosting.WindowsXamlManager.InitializeForCurrentThread();

            // Create DesktopWindowXamlSource, host for UWP XAML content
            _xamlSource = new Windows.UI.Xaml.Hosting.DesktopWindowXamlSource();

            // Hook DesktopWindowXamlSource OnTakeFocus event for Focus processing
            _xamlSource.TakeFocusRequested += OnTakeFocusRequested;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WindowsXamlHostBase"/> class.
        /// </summary>
        /// <remarks>
        /// Constructor is required for use in WPF markup. When the default constructor is called,
        /// object properties have not been set. Put WPF logic in OnInitialized.
        /// </remarks>
        /// <param name="typeName">UWP XAML Type name</param>
        public WindowsXamlHostBase(string typeName)
            : this()
        {
            ChildInternal = UWPTypeFactory.CreateXamlContentByType(typeName);
            ChildInternal.SetWrapper(this);
        }

        /// <summary>
        /// Binds this wrapper object's exposed WPF DependencyProperty with the wrapped UWP object's DependencyProperty
        /// for what effectively works as a regular one- or two-way binding.
        /// </summary>
        /// <param name="propertyName">the registered name of the dependency property</param>
        /// <param name="wpfProperty">the DependencyProperty of the wrapper</param>
        /// <param name="uwpProperty">the related DependencyProperty of the UWP control</param>
        /// <param name="converter">a converter, if one's needed</param>
        /// <param name="direction">indicates that the binding should be one or two directional.  If one way, the Uwp control is only updated from the wrapper.</param>
        public void Bind(string propertyName, System.Windows.DependencyProperty wpfProperty, Windows.UI.Xaml.DependencyProperty uwpProperty, object converter = null, System.ComponentModel.BindingDirection direction = System.ComponentModel.BindingDirection.TwoWay)
        {
            if (direction == System.ComponentModel.BindingDirection.TwoWay)
            {
                var binder = new Windows.UI.Xaml.Data.Binding()
                {
                    Source = this,
                    Path = new Windows.UI.Xaml.PropertyPath(propertyName),
                    Converter = (Windows.UI.Xaml.Data.IValueConverter)converter
                };
                Windows.UI.Xaml.Data.BindingOperations.SetBinding(ChildInternal, uwpProperty, binder);
            }

            var rebinder = new System.Windows.Data.Binding()
            {
                Source = ChildInternal,
                Path = new System.Windows.PropertyPath(propertyName),
                Converter = (System.Windows.Data.IValueConverter)converter
            };
            System.Windows.Data.BindingOperations.SetBinding(this, wpfProperty, rebinder);
        }

        /// <inheritdoc />
        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            if (_childInternal != null)
            {
                SetContent();
            }
        }

        /// <summary>
        /// Gets or sets the root UWP XAML element displayed in the WPF control instance.
        /// </summary>
        /// <value>The <see cref="Windows.UI.Xaml.UIElement"/> child.</value>
        /// <remarks>This UWP XAML element is the root element of the wrapped <see cref="Windows.UI.Xaml.Hosting.DesktopWindowXamlSource" />.</remarks>
        protected Windows.UI.Xaml.UIElement ChildInternal
        {
            get
            {
                return _childInternal;
            }

            set
            {
                if (value == ChildInternal)
                {
                    return;
                }

                var currentRoot = (Windows.UI.Xaml.FrameworkElement)ChildInternal;
                if (currentRoot != null)
                {
                    currentRoot.SizeChanged -= XamlContentSizeChanged;
                }

                _childInternal = value;
                SetContent();

                var frameworkElement = ChildInternal as Windows.UI.Xaml.FrameworkElement;
                if (frameworkElement != null)
                {
                    // If XAML content has changed, check XAML size
                    // to determine if WindowsXamlHost needs to re-run layout.
                    frameworkElement.SizeChanged += XamlContentSizeChanged;

                    // WindowsXamlHost DataContext should flow through to UWP XAML content
                    frameworkElement.DataContext = DataContext;
                }

                // Fire updated event
                ChildChanged?.Invoke(this, new EventArgs());
            }
        }

        /// <summary>
        /// Exposes ChildInternal without exposing its actual Type.
        /// </summary>
        /// <returns>the underlying UWP child object</returns>
        public object GetUwpInternalObject()
        {
            return ChildInternal;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this wrapper control instance been disposed
        /// </summary>
        protected bool IsDisposed { get; set; }

        /// <summary>
        /// Creates <see cref="Windows.UI.Xaml.Application" /> object, wrapped <see cref="Windows.UI.Xaml.Hosting.DesktopWindowXamlSource" /> instance; creates and
        /// sets root UWP XAML element on <see cref="Windows.UI.Xaml.Hosting.DesktopWindowXamlSource" />.
        /// </summary>
        /// <param name="hwndParent">Parent window handle</param>
        /// <returns>Handle to XAML window</returns>
        protected override HandleRef BuildWindowCore(HandleRef hwndParent)
        {
            // 'EnableMouseInPointer' is called by the WindowsXamlManager during initialization. No need
            // to call it directly here.

            // Create DesktopWindowXamlSource instance
            var desktopWindowXamlSourceNative = _xamlSource.GetInterop();

            // Associate the window where UWP XAML will display content
            desktopWindowXamlSourceNative.AttachToWindow(hwndParent.Handle);

            var windowHandle = desktopWindowXamlSourceNative.WindowHandle;

            // Overridden function must return window handle of new target window (DesktopWindowXamlSource's Window)
            return new HandleRef(this, windowHandle);
        }

        /// <summary>
        /// The default implementation of SetContent applies ChildInternal to desktopWindowXamSource.Content.
        /// Override this method if that shouldn't be the case.
        /// For example, override if your control should be a child of another WindowsXamlHostBase-based control.
        /// </summary>
        protected virtual void SetContent()
        {
            if (_xamlSource != null)
            {
                _xamlSource.Content = _childInternal;
            }
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
            if (disposing)
            {
                ChildInternal = null;

                // Required by CA2213: _xamlSource?.Dispose() is insufficient.
                if (_xamlSource != null)
                {
                    _xamlSource.TakeFocusRequested -= OnTakeFocusRequested;
                    _xamlSource.Dispose();
                }
            }

            // BUGBUG: CoreInputSink cleanup is failing when explicitly disposing
            // WindowsXamlManager.  Add dispose call back when that bug is fixed in 19h1.
            base.Dispose(disposing);
        }
    }
}