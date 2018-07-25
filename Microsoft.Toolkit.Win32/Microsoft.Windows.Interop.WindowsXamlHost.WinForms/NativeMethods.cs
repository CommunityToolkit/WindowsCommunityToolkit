// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace MS.Win32
{
    using System;
    using System.Runtime.InteropServices;
    using System.Security;
    using System.Runtime.Versioning;

    internal static class NativeDefines
    {
        public static IntPtr HWND_TOP = IntPtr.Zero;
        public static IntPtr HWND_TOPMOST = IntPtr.Zero-1;

        public const int WM_MOVE = 0x0003;
        public const int WM_SIZE = 0x0005;

        public const int WM_WINDOWPOSCHANGING = 0x0046;
        public const int WM_WINDOWPOSCHANGED = 0x0047;

        public const int WM_SETFOCUS   = 0x0007;
        public const int WM_KILLFOCUS = 0x0008;

        /// <summary>
        /// SetWindowPos Flags
        /// </summary>
        internal static class SetWindowPosFlags
        {
            public static readonly int
            SHOWWINDOW = 0x0040;
        }
    }

    internal static class UnsafeNativeMethods
    {
        [DllImport("User32", ExactSpelling = true, CharSet = CharSet.Auto)]
        [ResourceExposure(ResourceScope.None)]
        public static extern IntPtr GetFocus();

        /// <SecurityNote>
        ///  SecurityCritical: This code happens to return a critical resource and causes unmanaged code elevation
        /// </SecurityNote>
        [SecurityCritical]
        [SuppressUnmanagedCodeSecurity]
        [DllImport("user32.dll", EntryPoint = "SetFocus", ExactSpelling = true, CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr IntSetFocus(IntPtr hWnd);

        /// <SecurityNote>
        ///    Critical: This code calls into unmanaged code which elevates
        /// </SecurityNote>
        [SecurityCritical, SuppressUnmanagedCodeSecurity]
        [DllImport("user32.dll", EntryPoint = "EnableWindow", SetLastError = true, ExactSpelling = true, CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        public static extern bool IntEnableWindow(HandleRef hWnd, bool enable);

    } // End Unsafe methods

    internal static class SafeNativeMethods
    {
        [DllImport("User32", ExactSpelling = true, CharSet = CharSet.Auto)]
        [ResourceExposure(ResourceScope.None)]
        internal static extern IntPtr SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter,
            int x, int y, int cx, int cy, int flags);
    }
}
