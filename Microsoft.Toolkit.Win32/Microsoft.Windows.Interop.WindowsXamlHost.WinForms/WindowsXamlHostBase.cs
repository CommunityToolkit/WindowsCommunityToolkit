// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.Toolkit.Win32.UI.Interop.WinForms
{
    using System;
    using System.Drawing;
    using System.Security;
    using System.Security.Permissions;
    using System.Windows.Forms;

    /// <summary>
    ///     A sample Windows Forms control that hosts XAML content
    /// </summary>
    [System.ComponentModel.DesignerCategory("code")]
    partial class WindowsXamlHostBase : Control
    {
        /// <summary>
        ///    UWP XAML island window handle associated with this Control instance
        /// </summary>
        private IntPtr xamlIslandWindowHandle = IntPtr.Zero;

        /// <summary>
        ///    Last preferredSize returned by UWP XAML during WinForms layout pass
        /// </summary>
        private Size lastXamlContentPreferredSize = new Size();

        /// <summary>
        /// DesktopWindowXamlSource instance
        /// </summary>
        protected global::Windows.UI.Xaml.Hosting.DesktopWindowXamlSource desktopWindowXamlSource;

        /// <summary>
        /// UWP XAML Application instance and root UWP XamlMetadataProvider.  Custom implementation required to 
        /// probe at runtime for custom UWP XAML type information.  This must be created before 
        /// creating any DesktopWindowXamlSource instances if custom UWP XAML types are required.
        /// </summary>
        [ThreadStatic]
        private global::Windows.UI.Xaml.Application application;

        /// <summary>
        /// A reference count on the UWP XAML framework is tied to WindowsXamlManager's 
        /// lifetime.  UWP XAML is spun up on the first WindowsXamlManager creation and 
        /// deinitialized when the last instance of WindowsXamlManager is destroyed.
        /// </summary>
        private global::Windows.UI.Xaml.Hosting.WindowsXamlManager windowsXamlManager;

        /// <summary>
        ///     Initializes a new instance of the XamlContentHost class.
        /// </summary>
        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        public WindowsXamlHostBase()
            : base()
        {
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);

            // Respond to size changes on this Control
            this.SizeChanged += this.WindowsXamlHost_SizeChanged;

            // Create a custom UWP XAML Application object that implements reflection-based XAML metdata probing.
            // Instantiation of the application object must occur before creating the DesktopWindowXamlSource instance. 
            // DesktopWindowXamlSource will create a generic Application object unable to load custom UWP XAML metadata.
            if (this.application == null)
            {
                try
                {
                    // global::Windows.UI.Xaml.Application.Current may throw if DXamlCore has not been initialized.
                    // Treat the exception as an uninitialized global::Windows.UI.Xaml.Application condition.
                    this.application = global::Windows.UI.Xaml.Application.Current as XamlApplication;
                }
                catch
                {
                    this.application = new XamlApplication();
                }
            }

            // Create an instance of the WindowsXamlManager. This initializes and holds a 
            // reference on the UWP XAML DXamlCore and must be explicitly created before 
            // any UWP XAML types are programmatically created.  If WindowsXamlManager has 
            // not been created before creating DesktopWindowXamlSource, DesktopWindowXaml source
            // will create an instance of WindowsXamlManager internally.  (Creation is explicit
            // here to illustrate how to initialize UWP XAML before initializing the DesktopWindowXamlSource.) 
            windowsXamlManager = global::Windows.UI.Xaml.Hosting.WindowsXamlManager.InitializeForCurrentThread();

            // Create DesktopWindowXamlSource, host for UWP XAML content
            this.desktopWindowXamlSource = new global::Windows.UI.Xaml.Hosting.DesktopWindowXamlSource();
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
                IDesktopWindowXamlSourceNative desktopWindowXamlSourceNative = this.desktopWindowXamlSource.GetInterop();
                desktopWindowXamlSourceNative.AttachToWindow(Handle);
                this.xamlIslandWindowHandle = desktopWindowXamlSourceNative.WindowHandle;
            }

            base.OnHandleCreated(e);
        }

        /// <summary>
        /// Cleanup hosted XAML content
        /// </summary>
        /// <param name="disposing">IsDisposing?</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing == true)
            {
                this.SizeChanged -= this.WindowsXamlHost_SizeChanged;

                desktopWindowXamlSource?.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
