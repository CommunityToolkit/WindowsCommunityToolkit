// <copyright file="WindowsXamlHostBase.Window.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation
// </copyright>
// <author>Microsoft</author>

using System;
using MS.Win32;

namespace Microsoft.Toolkit.Win32.UI.Interop.WinForms
{
    /// <summary>
    ///     A sample Windows Forms control that can be used to host XAML content
    /// </summary>
    partial class WindowsXamlHostBase : System.Windows.Forms.Control
    {
        /// <summary>
        ///    Sets XAML window size using dimensions of the host control
        /// </summary>
        private void SetDesktopWindowXamlSourceWindowPos()
        {
            if (xamlIslandWindowHandle != IntPtr.Zero && this.Width != 0 && this.Height != 0)
            {  
                if (SafeNativeMethods.SetWindowPos(xamlIslandWindowHandle, NativeDefines.HWND_TOP, 0, 0, this.Width, this.Height, NativeDefines.SetWindowPosFlags.SHOWWINDOW) == IntPtr.Zero)
                {
                    throw new InvalidOperationException("WindowXamlHostBase::SetDesktopWindowXamlSourceWindowPos failed to set UWP XAML window position.");
                }
            }
        }
    }
}
