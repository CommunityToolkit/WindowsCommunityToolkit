// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.ComponentModel;
using System.Drawing;
using System.Security.Permissions;
using System.Windows.Forms;
using Microsoft.Toolkit.Win32.UI.XamlHost;

namespace Microsoft.Toolkit.Forms.UI.XamlHost
{
    /// <summary>
    ///     WindowsXamlHostBase hosts UWP XAML content inside Windows Forms
    /// </summary>
    [System.ComponentModel.DesignerCategory("code")]
    public abstract partial class WindowsXamlHostBase : ContainerControl
    {
#pragma warning disable SA1401 // Fields must be private
        /// <summary>
        /// DesktopWindowXamlSource instance
        /// </summary>
        protected internal readonly Windows.UI.Xaml.Hosting.DesktopWindowXamlSource _xamlSource;

        /// <summary>
        ///    A render transform to scale the UWP XAML content should be applied
        /// </summary>
        protected internal bool _dpiScalingRenderTransformEnabled = false;
#pragma warning restore SA1401 // Fields must be private

        /// <summary>
        /// A reference count on the UWP XAML framework is tied to WindowsXamlManager's
        /// lifetime.  UWP XAML is spun up on the first WindowsXamlManager creation and
        /// deinitialized when the last instance of WindowsXamlManager is destroyed.
        /// </summary>
        private readonly Windows.UI.Xaml.Hosting.WindowsXamlManager _windowsXamlManager;

        /// <summary>
        /// UWP XAML Application instance and root UWP XamlMetadataProvider.  Custom implementation required to
        /// probe at runtime for custom UWP XAML type information.  This must be created before
        /// creating any DesktopWindowXamlSource instances if custom UWP XAML types are required.
        /// </summary>
        private readonly Windows.UI.Xaml.Application _application;

        /// <summary>
        ///    Last preferredSize returned by UWP XAML during WinForms layout pass
        /// </summary>
        private Size _lastXamlContentPreferredSize;

        /// <summary>
        ///    UWP XAML island window handle associated with this Control instance
        /// </summary>
        private IntPtr _xamlIslandWindowHandle = IntPtr.Zero;

        /// <summary>
        ///     Fired when XAML content has been updated
        /// </summary>
        [Browsable(true)]
        [Category("UWP XAML")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Description("Fired when UWP XAML content has been updated")]
        public event EventHandler ChildChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="WindowsXamlHostBase"/> class.
        /// </summary>
        public WindowsXamlHostBase()
        {
            SetStyle(ControlStyles.ContainerControl, true);
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);

            // Must be a container control with TabStop == false to allow nested UWP XAML Focus
            // BUGBUG: Uncomment when nested Focus is available
            // TabStop = false;

            // Respond to size changes on this Control
            SizeChanged += OnWindowXamlHostSizeChanged;

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

            // Hook up method for DesktopWindowXamlSource Focus handling
            _xamlSource.TakeFocusRequested += this.OnTakeFocusRequested;

            // Add scaling panel as the root XAML element
            _xamlSource.Content = new DpiScalingPanel();
        }

        /// <summary>
        ///    Gets or sets XAML content for XamlContentHost
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        protected Windows.UI.Xaml.UIElement ChildInternal
        {
            get => (_xamlSource.Content as DpiScalingPanel).Child;

            set
            {
                if (!DesignMode)
                {
                    var newFrameworkElement = value as Windows.UI.Xaml.FrameworkElement;
                    var oldFrameworkElement = (_xamlSource.Content as DpiScalingPanel).Child as Windows.UI.Xaml.FrameworkElement;

                    if (oldFrameworkElement != null)
                    {
                        oldFrameworkElement.SizeChanged -= OnChildSizeChanged;
                    }

                    if (newFrameworkElement != null)
                    {
                        // If XAML content has changed, check XAML size and WindowsXamlHost.AutoSize
                        // setting to determine if WindowsXamlHost needs to re-run layout.
                        newFrameworkElement.SizeChanged += OnChildSizeChanged;
                    }

                    (_xamlSource.Content as DpiScalingPanel).Child = value;

                    PerformLayout();

                    ChildChanged?.Invoke(this, new EventArgs());
                }
            }
        }

        /// <summary>
        /// Sets the root UWP XAML element on DesktopWindowXamlSource
        /// </summary>
        /// <param name="newValue">A UWP XAML Framework element</param>
        protected virtual void SetContent(Windows.UI.Xaml.FrameworkElement newValue)
        {
            if (_xamlSource != null)
            {
                (_xamlSource.Content as DpiScalingPanel).Child = newValue;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether a render transform is added to the UWP control corresponding to the current dpi scaling factor
        /// </summary>
        /// <value>The dpi scaling mode.</value>
        /// <remarks>A custom render transform added to the root UWP control will be overwritten.</remarks>
        [ReadOnly(false)]
        [Browsable(true)]
        [Category("Layout")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public bool DpiScalingRenderTransform
        {
            get => _dpiScalingRenderTransformEnabled;

            set
            {
                _dpiScalingRenderTransformEnabled = value;
                UpdateDpiScalingFactor();
                PerformLayout();
            }
        }

        /// <summary>
        /// Clean up hosted UWP XAML content
        /// </summary>
        /// <param name="disposing">IsDisposing?</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                SizeChanged -= OnWindowXamlHostSizeChanged;

                // Required by CA2213: _xamlSource?.Dispose() is insufficient.
                if (_xamlSource != null)
                {
                    _xamlSource.TakeFocusRequested -= OnTakeFocusRequested;
                    _xamlSource.Dispose();
                }

                _windowsXamlManager?.Dispose();
            }

            base.Dispose(disposing);
        }

        /// <summary>
        /// Raises the HandleCreated event.  Assign window render target to UWP XAML content.
        /// </summary>
        /// <param name="e">EventArgs</param>
        protected override void OnHandleCreated(EventArgs e)
        {
            if (!DesignMode)
            {
                // Attach window to DesktopWindowXamSource as a render target
                var desktopWindowXamlSourceNative = _xamlSource.GetInterop();
                desktopWindowXamlSourceNative.AttachToWindow(Handle);
                _xamlIslandWindowHandle = desktopWindowXamlSourceNative.WindowHandle;

                // Set window style required by container control to support Focus
                if (Interop.Win32.UnsafeNativeMethods.SetWindowLong(Handle, Interop.Win32.NativeDefines.GWL_EXSTYLE, Interop.Win32.NativeDefines.WS_EX_CONTROLPARENT) == 0)
                {
                    throw new InvalidOperationException("WindowsXamlHostBase::OnHandleCreated: Failed to set WS_EX_CONTROLPARENT on control window.");
                }

                UpdateDpiScalingFactor();
            }

            base.OnHandleCreated(e);
        }
    }
}
