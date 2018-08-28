// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Runtime.InteropServices;

namespace Microsoft.Toolkit.Win32.UI.XamlHost
{
    /// <summary>
    /// IDesktopWindowXamlSourceNative interface enables access to native methods on DesktopWindowXamlSourceNative,
    /// including the method used to set the window handle of the DesktopWindowXamlSource instance.
    /// </summary>
    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("3cbcf1bf-2f76-4e9c-96ab-e84b37972554")]
    public interface IDesktopWindowXamlSourceNative
    {
        /// <summary>
        /// Attaches the DesktopWindowXamlSource to a window using a window handle.
        /// The associated window will be used to parent UWP XAML visuals, appearing
        /// as UWP XAML's logical render target.
        /// </summary>
        /// <param name="parentWnd">pointer to parent Wnd</param>
        void AttachToWindow(IntPtr parentWnd);

        /// <summary>
        /// Gets the handle associated with the DesktopWindowXamlSource instance.
        /// </summary>
        IntPtr WindowHandle { get; }
    }
}
