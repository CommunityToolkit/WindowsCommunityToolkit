using System;
using System.Runtime.InteropServices;

namespace Microsoft.Toolkit.Win32.UI.Interop
{
    /// <summary>
    /// COM wrapper required to access native-only methods on DesktopWindowXamlSource
    /// </summary>
    public static class DesktopWindowXamlSourceExtensions
    {
        /// <summary>
        /// Gets the IDesktopWindowXamlSourceNative interface from a DesktopWindowXamlSource instance. This
        /// interface is the only way to set DesktopWindowXamlSource's target window for rendering.
        /// </summary>
        /// <param name="desktopWindowXamlSource">The DesktopWindowXamlSource instance to get the interface from</param>
        /// <returns>IDesktopWindowXamlSourceNative interface pointer</returns>
        public static IDesktopWindowXamlSourceNative GetInterop(this Windows.UI.Xaml.Hosting.DesktopWindowXamlSource desktopWindowXamlSource)
        {
            var win32XamlSourceIntPtr = Marshal.GetIUnknownForObject(desktopWindowXamlSource);
            try
            {
                var win32XamlSource = Marshal.GetTypedObjectForIUnknown(win32XamlSourceIntPtr, typeof(IDesktopWindowXamlSourceNative)) as IDesktopWindowXamlSourceNative;
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