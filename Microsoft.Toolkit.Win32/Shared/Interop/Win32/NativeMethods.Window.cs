// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace Microsoft.Toolkit.Win32.UI.Controls.Interop.Win32
{
    /// <summary>
    /// Native methods for Windowing
    /// </summary>
    internal static partial class NativeMethods
    {
        // Critical : P-Invokes
        [SecurityCritical]
        [DllImport(ExternDll.User32, SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "CreateWindowExW")]
        private static extern IntPtr _CreateWindowEx(
            WS_EX dwExStyle,
            [MarshalAs(UnmanagedType.LPWStr)] string lpClassName,
            [MarshalAs(UnmanagedType.LPWStr)] string lpWindowName,
            WS dwStyle,
            int x,
            int y,
            int nWidth,
            int nHeight,
            IntPtr hWndParent,
            IntPtr hMenu,
            IntPtr hInstance,
            IntPtr lpParam);

        // Critical : Calls critical method
        [SecurityCritical]
        public static IntPtr CreateWindowEx(
            WS_EX dwExStyle,
            string lpClassName,
            string lpWindowName,
            WS dwStyle,
            int x,
            int y,
            int nWidth,
            int nHeight,
            IntPtr hWndParent,
            IntPtr hMenu,
            IntPtr hInstance,
            IntPtr lpParam)
        {
            var ret = _CreateWindowEx(dwExStyle, lpClassName, lpWindowName, dwStyle, x, y, nWidth, nHeight, hWndParent, hMenu, hInstance, lpParam);
            if (ret == IntPtr.Zero)
            {
                HRESULT.ThrowLastError();
            }

            return ret;
        }

        // Critical : Calls critical method
        [SecurityCritical]
        public static IntPtr CreateWindow(string className, WS styles, int x, int y, int width, int height, IntPtr parentWindow)
        {
            return CreateWindowEx(
                0,
                className,
                string.Empty,
                styles,
                x,
                y,
                width,
                height,
                parentWindow,
                IntPtr.Zero,
                Marshal.GetHINSTANCE(typeof(NativeMethods).Module),
                IntPtr.Zero);
        }

        // Critical : P-Invokes
        [SecurityCritical]
        [DllImport(ExternDll.User32, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DestroyWindow(IntPtr hwnd);

        // Critical : P-Invokes
        [SecurityCritical]
        [DllImport(ExternDll.User32)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool IsWindow(IntPtr hwnd);

        [SecurityCritical]
        [DllImport(ExternDll.User32, ExactSpelling = true, CharSet = CharSet.Auto)]
        public static extern bool IsChild(HandleRef parent, HandleRef child);

        // Enables the mouse to act as a pointer input device and send WM_POINTER messages.
        [SecurityCritical] // P-Invokes
        [DllImport(ExternDll.User32, SetLastError = true, EntryPoint = "EnableMouseInPointer")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool _EnableMouseInPointer(bool fEnable);

        [SecurityCritical]
        public static bool EnableMouseInPointer(bool enable)
        {
            // NOTE: This is available on Windows 8 or later only
            var ret = _EnableMouseInPointer(enable);
            if (!ret)
            {
                HRESULT.ThrowLastError();
            }

            return ret;
        }
    }
}