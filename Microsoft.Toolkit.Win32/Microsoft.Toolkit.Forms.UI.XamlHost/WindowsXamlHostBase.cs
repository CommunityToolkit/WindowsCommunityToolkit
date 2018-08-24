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
    ///     A sample Windows Forms control that hosts XAML content
    /// </summary>
    [System.ComponentModel.DesignerCategory("code")]
    public abstract partial class WindowsXamlHostBase : ContainerControl
    {
#pragma warning disable SA1401 // Fields must be private
                              /// <summary>
                              /// DesktopWindowXamlSource instance
                              /// </summary>
        protected readonly Windows.UI.Xaml.Hosting.DesktopWindowXamlSource xamlSource;
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
        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        public WindowsXamlHostBase()
        {
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);

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
            xamlSource = new Windows.UI.Xaml.Hosting.DesktopWindowXamlSource();
        }

        /// <summary>
        ///    Gets or sets XAML content for XamlContentHost
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Windows.UI.Xaml.UIElement ChildInternal
        {
            get => xamlSource.Content;

            set
            {
                if (!DesignMode)
                {
                    var newFrameworkElement = value as Windows.UI.Xaml.FrameworkElement;
                    var oldFrameworkElement = xamlSource.Content as Windows.UI.Xaml.FrameworkElement;

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

                    xamlSource.Content = value;

                    PerformLayout();

                    ChildChanged?.Invoke(this, new EventArgs());
                }
            }
        }

        /// <summary>
        /// Cleanup hosted XAML content
        /// </summary>
        /// <param name="disposing">IsDisposing?</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                SizeChanged -= OnWindowXamlHostSizeChanged;

                // Required by CA2213: xamlSource?.Dispose() is insufficient.
                if (xamlSource != null)
                {
                    xamlSource.Dispose();
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
                var desktopWindowXamlSourceNative = xamlSource.GetInterop();
                desktopWindowXamlSourceNative.AttachToWindow(Handle);
                _xamlIslandWindowHandle = desktopWindowXamlSourceNative.WindowHandle;
            }

            base.OnHandleCreated(e);
        }
    }
}
