// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Runtime.InteropServices;

namespace Microsoft.Toolkit.Win32.UI.XamlHost
{
    /// <summary>
    /// COM wrapper required to access native-only methods on <see cref="Windows.UI.Xaml.Hosting.DesktopWindowXamlSource" />
    /// </summary>
    public static class DesktopWindowXamlSourceExtensions
    {
        /// <summary>
        /// Gets the <see cref="IDesktopWindowXamlSourceNative" /> interface from a <see cref="Windows.UI.Xaml.Hosting.DesktopWindowXamlSource" /> instance.
        /// </summary>
        /// <param name="desktopWindowXamlSource">The DesktopWindowXamlSource instance to get the interface from</param>
        /// <returns><see cref="IDesktopWindowXamlSourceNative" /> interface pointer</returns>
        /// <remarks>
        /// This interface is the only way to set DesktopWindowXamlSource's target window for rendering.
        /// </remarks>
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