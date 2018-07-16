// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.Windows.Interop
{
    using System;
    using System.Runtime.InteropServices;

    /// <summary>
    /// IDesktopWindowXamlSourceNative interface enables access to native methods on DesktopWindowXamlSourceNative, 
    /// including the method used to set the window handle of the DesktopWindowXamlSource instance.  
    /// </summary>
    [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("3cbcf1bf-2f76-4e9c-96ab-e84b37972554")]
    partial interface IDesktopWindowXamlSourceNative
    {
        /// <summary>
        /// Attaches the DesktopWindowXamlSource to a window using a window handle. 
        /// The associated window will be used to parent UWP XAML visuals, appearing 
        /// as UWP XAML's logical render target.
        /// </summary>
        /// <param name="parentWnd"></param>
        void AttachToWindow(IntPtr parentWnd);

        /// <summary>
        /// Gets the handle associated with the DesktopWindowXamlSource instance.
        /// </summary>
        IntPtr WindowHandle { get; }
    }

    /// <summary>
    /// COM wrapper required to access native-only methods on DesktopWindowXamlSource
    /// </summary>
    static partial class DesktopWindowXamlSourceExtensions
    {
        /// <summary>
        /// Gets the IDesktopWindowXamlSourceNative interface from a DesktopWindowXamlSource instance. This
        /// interface is the only way to set DesktopWindowXamlSource's target window for rendering.
        /// </summary>
        /// <param name="this">The DesktopWindowXamlSource instance to get the interface from</param>
        /// <returns>IDesktopWindowXamlSourceNative interface pointer</returns>
        public static IDesktopWindowXamlSourceNative GetInterop(this global::Windows.UI.Xaml.Hosting.DesktopWindowXamlSource @this)
        {
            IntPtr win32XamlSourceIntPtr = Marshal.GetIUnknownForObject(@this);
            try
            {
                IDesktopWindowXamlSourceNative win32XamlSource = Marshal.GetTypedObjectForIUnknown(win32XamlSourceIntPtr, typeof(IDesktopWindowXamlSourceNative)) as IDesktopWindowXamlSourceNative;
                return win32XamlSource;
            }
            finally
            {
                 Marshal.Release(win32XamlSourceIntPtr);
                 win32XamlSourceIntPtr = IntPtr.Zero;
            }
        }
    }
}
