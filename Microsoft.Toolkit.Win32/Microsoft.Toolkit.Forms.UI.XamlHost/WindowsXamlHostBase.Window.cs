// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Microsoft.Toolkit.Forms.UI.XamlHost.Interop.Win32;

namespace Microsoft.Toolkit.Forms.UI.XamlHost
{
    /// <summary>
    ///     WindowsXamlHostBase hosts UWP XAML content inside Windows Forms
    /// </summary>
    public partial class WindowsXamlHostBase
    {
        /// <summary>
        ///    Sets XAML window size using dimensions of the host control
        /// </summary>
        private void SetDesktopWindowXamlSourceWindowPos()
        {
            if (_xamlIslandWindowHandle != IntPtr.Zero && Width != 0 && Height != 0)
            {
                if (SafeNativeMethods.SetWindowPos(_xamlIslandWindowHandle, NativeDefines.HWND_TOP, 0, 0, Width, Height, SetWindowPosFlags.SHOWWINDOW) == IntPtr.Zero)
                {
                    throw new InvalidOperationException($"{nameof(WindowsXamlHostBase)}::{nameof(SetDesktopWindowXamlSourceWindowPos)} failed to set UWP XAML window position.");
                }
            }
        }
    }
}
