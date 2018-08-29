// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
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
        internal static extern IntPtr IntSetFocus(IntPtr hWnd);

        /// <summary>
        /// Changes an attribute of the specified window. The function also sets the 32-bit (long) value at the specified offset into the extra window memory.
        /// </summary>
        /// <param name="hWnd">Target window</param>
        /// <param name="nIndex">Zero-based offset</param>
        /// <param name="dwNewLong">The replacement value</param>
        /// <returns>A positive integer indicates success; zero indicates failure</returns>
        [SecurityCritical]
        [DllImport(ExternDll.User32, SetLastError = true)]
        internal static extern int SetWindowLong(IntPtr hWnd, int nIndex, uint dwNewLong);

        /// <summary>
        /// Retrieves the handle to the window that has the keyboard focus, if the window is attached to the calling thread's message queue.
        /// </summary>
        /// <returns>Window handle of window that currently has Focus</returns>
        [DllImport(ExternDll.User32, ExactSpelling = true, CharSet = CharSet.Auto)]
        [SecurityCritical]
        internal static extern IntPtr GetFocus();

        /// <summary>
        /// Sends the specified message to a window or windows. The SendMessage function calls the
        /// window procedure for the specified window and does not return until the window procedure
        /// has processed the message.
        /// </summary>
        /// <param name="hWnd">Target window</param>
        /// <param name="msg">Message</param>
        /// <param name="wParam">Additional message-specific information (WPARAM).</param>
        /// <param name="lParam">Additional message-specific information (LPARAM).</param>
        /// <returns>The return value specifies the result of the message processing; it depends on the message sent.</returns>
        [DllImport(ExternDll.User32, CharSet = CharSet.Auto, SetLastError = true)]
        [SecurityCritical]
        internal static extern IntPtr SendMessage(HandleRef hWnd, int msg, IntPtr wParam, IntPtr lParam);

        /// <summary>
        /// Maps OEMASCII codes 0 through 0x0FF into the OEM scan codes and shift states. The
        /// function provides information that allows a program to send OEM text to another
        /// program by simulating keyboard input.
        /// </summary>
        /// <param name="wAsciiVal">Ascii key value</param>
        /// <returns>The low-order word of the return value contains the scan code of the
        /// OEM character, and the high-order word contains the shift state, which can be
        /// a combination of the following bits.</returns>
        [System.Runtime.InteropServices.DllImport(ExternDll.User32)]
        [SecurityCritical]
        internal static extern int OemKeyScan(short wAsciiVal);
    }
}