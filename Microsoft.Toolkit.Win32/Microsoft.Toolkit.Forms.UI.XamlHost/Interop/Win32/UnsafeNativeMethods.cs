// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Runtime.InteropServices;
using System.Security;
using Microsoft.Toolkit.Win32.UI.Controls.Interop.Win32;

namespace Microsoft.Toolkit.Forms.UI.XamlHost.Interop.Win32
{
    /// <summary>
    /// This class is for methods that are potentially dangerous. Any caller of these methods must perform a full security review to make sure that the usage is secure because no stack walk will be performed.
    /// </summary>
    /// <remarks>
    /// <see cref="SuppressUnmanagedCodeSecurityAttribute"/> is applied to this class.
    /// </remarks>
    [SuppressUnmanagedCodeSecurity]
    internal static partial class UnsafeNativeMethods
    {
        /// <summary>
        /// This code returns a pointer to a native control with focus.
        /// </summary>
        /// <SecurityNote>
        ///  SecurityCritical: This code happens to return a critical resource and causes unmanaged code elevation
        /// </SecurityNote>
        /// <returns>handle</returns>
        [SecurityCritical]
        [DllImport(ExternDll.User32, EntryPoint = "SetFocus", ExactSpelling = true, CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr IntSetFocus(IntPtr hWnd);

        /// <summary>
        /// Enables a window and returns an unmanaged handle to it.
        /// </summary>
        /// <SecurityNote>
        ///    Critical: This code calls into unmanaged code which elevates
        /// </SecurityNote>
        /// <returns>handle</returns>
        [SecurityCritical]
        [DllImport(ExternDll.User32, EntryPoint = "EnableWindow", SetLastError = true, ExactSpelling = true, CharSet = CharSet.Auto)]
        public static extern bool IntEnableWindow(HandleRef hWnd, bool enable);

        /// <summary>
        /// Changes an attribute of the specified window. The function also sets the 32-bit (long) value at the specified offset into the extra window memory.
        /// </summary>
        /// <param name="hWnd">Target window</param>
        /// <param name="nIndex">Zero-based offset</param>
        /// <param name="dwNewLong">The replacement value</param>
        /// <returns>A positive integer indicates success; zero indicates failure</returns>
        [SecurityCritical]
        [DllImport("user32.dll", SetLastError = true)]
        internal static extern int SetWindowLong(IntPtr hWnd, int nIndex, uint dwNewLong);
    }
}