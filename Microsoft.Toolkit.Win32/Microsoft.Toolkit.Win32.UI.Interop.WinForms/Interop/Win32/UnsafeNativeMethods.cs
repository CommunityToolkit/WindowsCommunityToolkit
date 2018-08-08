// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Security;
using Microsoft.Toolkit.Win32.UI.Controls.Interop.Win32;

namespace Microsoft.Toolkit.Win32.UI.Interop.WinForms.Interop.Win32
{
    internal static class UnsafeNativeMethods
    {
        [DllImport(ExternDll.User32, ExactSpelling = true, CharSet = CharSet.Auto)]
        [ResourceExposure(ResourceScope.None)]
        public static extern IntPtr GetFocus();

        /// <summary>
        /// This code returns a pointer to a native control with focus.
        /// </summary>
        /// <SecurityNote>
        ///  SecurityCritical: This code happens to return a critical resource and causes unmanaged code elevation
        /// </SecurityNote>
        /// <returns>handle</returns>
        [SecurityCritical]
        [SuppressUnmanagedCodeSecurity]
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
        [SuppressUnmanagedCodeSecurity]
        [DllImport(ExternDll.User32, EntryPoint = "EnableWindow", SetLastError = true, ExactSpelling = true, CharSet = CharSet.Auto)]
        public static extern bool IntEnableWindow(HandleRef hWnd, bool enable);

        /// <summary>
        /// Changes the size, position, and Z order of a child, pop-up, or top-level window. These windows are ordered according to their appearance on the screen. The topmost window receives the highest rank and is the first window in the Z order.
        /// </summary>
        /// <param name="hWnd">A handle to the window.</param>
        /// <param name="hWndInsertAfter">A handle to the window to precede the positioned window in the Z order.</param>
        /// <param name="x">The new position of the left side of the window, in client coordinates. </param>
        /// <param name="y">The new position of the top of the window, in client coordinates. </param>
        /// <param name="cx">The new width of the window, in pixels.</param>
        /// <param name="cy">The new height of the window, in pixels.</param>
        /// <param name="flags">The window sizing and positioning flags.</param>
        /// <returns>f the function succeeds, the return value is nonzero. If the function fails, the return value is zero.To get extended error information, call GetLastError. </returns>
        [DllImport(ExternDll.User32, ExactSpelling = true, CharSet = CharSet.Auto, EntryPoint = "SetWindowPos")]
        [ResourceExposure(ResourceScope.None)]
        [SecurityCritical]
        private static extern IntPtr _SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, uint flags);

        [SecurityCritical]
        internal static bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, uint flags)
        {
            if (_SetWindowPos(hWnd, hWndInsertAfter, x, y, cx, cy, flags) == IntPtr.Zero)
            {
                HRESULT.ThrowLastError();
            }

            return true;
        }
    }
}