// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Drawing;
using System.Security.Permissions;
using System.Windows.Forms;

namespace Microsoft.Toolkit.Win32.UI.Interop.WinForms
{
    /// <summary>
    ///     A sample Windows Forms control that hosts XAML content
    /// </summary>
    [System.ComponentModel.DesignerCategory("code")]
    public partial class WindowsXamlHostBase : Control
    {
#pragma warning disable SA1401 // Fields must be private
                              /// <summary>
                              /// DesktopWindowXamlSource instance
                              /// </summary>
        protected Windows.UI.Xaml.Hosting.DesktopWindowXamlSource desktopWindowXamlSource;
#pragma warning restore SA1401 // Fields must be private

        /// <summary>
        /// UWP XAML Application instance and root UWP XamlMetadataProvider.  Custom implementation required to
        /// probe at runtime for custom UWP XAML type information.  This must be created before
        /// creating any DesktopWindowXamlSource instances if custom UWP XAML types are required.
        /// </summary>
        [ThreadStatic]
        private readonly Windows.UI.Xaml.Application _application;

        /// <summary>
        ///    Last preferredSize returned by UWP XAML during WinForms layout pass
        /// </summary>
        private Size _lastXamlContentPreferredSize;

        /// <summary>
        /// A reference count on the UWP XAML framework is tied to WindowsXamlManager's
        /// lifetime.  UWP XAML is spun up on the first WindowsXamlManager creation and
        /// deinitialized when the last instance of WindowsXamlManager is destroyed.
        /// </summary>
        private Windows.UI.Xaml.Hosting.WindowsXamlManager _windowsXamlManager;

        /// <summary>
        ///    UWP XAML island window handle associated with this Control instance
        /// </summary>
        private IntPtr _xamlIslandWindowHandle = IntPtr.Zero;

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
            SizeChanged += WindowsXamlHost_SizeChanged;

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
            desktopWindowXamlSource = new Windows.UI.Xaml.Hosting.DesktopWindowXamlSource();
        }

        /// <summary>
        /// Cleanup hosted XAML content
        /// </summary>
        /// <param name="disposing">IsDisposing?</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                SizeChanged -= WindowsXamlHost_SizeChanged;

                desktopWindowXamlSource?.Dispose();
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
                var desktopWindowXamlSourceNative = desktopWindowXamlSource.GetInterop();
                desktopWindowXamlSourceNative.AttachToWindow(Handle);
                _xamlIslandWindowHandle = desktopWindowXamlSourceNative.WindowHandle;
            }

            base.OnHandleCreated(e);
        }
    }
}
