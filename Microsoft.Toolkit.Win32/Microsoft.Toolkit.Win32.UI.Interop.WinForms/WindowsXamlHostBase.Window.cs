// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Microsoft.Toolkit.Win32.UI.Interop.WinForms.Interop.Win32;
using MS.Win32;

namespace Microsoft.Toolkit.Win32.UI.Interop.WinForms
{
    /// <summary>
    ///     A sample Windows Forms control that can be used to host XAML content
    /// </summary>
    public partial class WindowsXamlHostBase
    {
        private static readonly IntPtr HWND_TOP = IntPtr.Zero;

        /// <summary>
        ///    Sets XAML window size using dimensions of the host control
        /// </summary>
        private void SetDesktopWindowXamlSourceWindowPos()
        {
            if (_xamlIslandWindowHandle != IntPtr.Zero && Width != 0 && Height != 0)
            {
                const int SWP_SHOWWINDOW = 0x0040;

                try
                {
                    UnsafeNativeMethods.SetWindowPos(
                        _xamlIslandWindowHandle,
                        HWND_TOP,
                        0,
                        0,
                        Width,
                        Height,
                        SWP_SHOWWINDOW);
                }
                catch (Exception e)
                {
                    throw new InvalidOperationException(
                        "WindowXamlHostBase::SetDesktopWindowXamlSourceWindowPos failed to set UWP XAML window position.",
                        e);
                }
            }
        }
    }
}
