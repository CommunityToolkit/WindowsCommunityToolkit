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
    /// This class is for methods that are safe for anyone to call. Callers of these methods are not required to perform a full security review to make sure that the usage is secure because the methods are harmless for any caller.
    /// </summary>
    /// <remarks>
    /// <see cref="SuppressUnmanagedCodeSecurityAttribute"/> is applied to this class.
    /// </remarks>
    [SuppressUnmanagedCodeSecurity]
    internal static partial class SafeNativeMethods
    {
        /// <summary>
        /// Retrieves the handle to the window that has the keyboard focus, if the window is attached
        /// to the calling thread's message queue.
        /// </summary>
        /// <returns>Window handle of window that currently has focus</returns>
        [DllImport(ExternDll.User32, ExactSpelling = true, CharSet = CharSet.Auto)]
        [ResourceExposure(ResourceScope.None)]
        public static extern IntPtr GetFocus();

        /// <summary>
        /// Changes the size, position, and Z order of a child, pop-up, or top-level window.These windows are ordered according to their appearance on the screen. The topmost window receives the highest rank and is the first window in the Z order.
        /// </summary>
        /// <param name="hWnd">Handle to target window</param>
        /// <param name="hWndInsertAfter">A handle to the window to precede the positioned window in the Z order.</param>
        /// <param name="x">The new position of the left side of the window, in client coordinates.</param>
        /// <param name="y">The new position of the top of the window, in client coordinates. </param>
        /// <param name="cx">The new width of the window, in pixels. </param>
        /// <param name="cy">The new height of the window, in pixels. </param>
        /// <param name="flags">The window sizing and positioning flags. This parameter can be a combination of the following values. </param>
        /// <returns>If the function succeeds, the return value is nonzero. Otherwise, call GLE.</returns>
        [DllImport(ExternDll.User32, ExactSpelling = true, CharSet = CharSet.Auto)]
        [ResourceExposure(ResourceScope.None)]
        internal static extern IntPtr SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, int flags);
    }
}
