// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Security;
using System.Text;

namespace Microsoft.Toolkit.Win32.UI.Controls.Interop.Win32
{
    internal static class UnsafeNativeMethods
    {
        // Critical: P-Invokes
        [SecurityCritical]
        [DllImport(ExternDll.ShCore, SetLastError = true)]
        public static extern int GetProcessDpiAwareness(
            IntPtr hprocess,
            out PROCESS_DPI_AWARENESS value);

        [DllImport(ExternDll.User32, ExactSpelling = true, EntryPoint = "GetDC", CharSet = CharSet.Auto)]
        [ResourceExposure(ResourceScope.Process)]
        private static extern IntPtr IntGetDC(HandleRef hWnd);

        [ResourceExposure(ResourceScope.Process)]
        [ResourceConsumption(ResourceScope.Process)]
        public static IntPtr GetDC(HandleRef hWnd)
        {
            // REVIEW: We can leak this handle unless ReleaseDC is called
            return IntGetDC(hWnd);
        }

        [DllImport(ExternDll.User32, ExactSpelling = true, EntryPoint = "ReleaseDC", CharSet = CharSet.Auto)]
        [ResourceExposure(ResourceScope.None)]
        private static extern int IntReleaseDC(HandleRef hWnd, HandleRef hDC);

        public static int ReleaseDC(HandleRef hWnd, HandleRef hDC)
        {
            return IntReleaseDC(hWnd, hDC);
        }

        [DllImport(ExternDll.Gdi32, SetLastError = true, ExactSpelling = true, CharSet = CharSet.Auto)]
        [ResourceExposure(ResourceScope.None)]
        public static extern int GetDeviceCaps(HandleRef hDC, int nIndex);

        // Critical: P-Invokes
        [SecurityCritical]
        [DllImport(ExternDll.User32, CharSet = CharSet.Auto)]
        internal static extern bool GetClientRect(HandleRef hWnd, [In, Out] ref RECT rect);

        // Critical: P-Invokes
        [SecurityCritical]
        [DllImport(ExternDll.User32, CharSet = CharSet.Auto)]
        internal static extern IntPtr GetParent(HandleRef hWnd);

        // Critical: This code elevates to unmanaged code permission
        [SecurityCritical]
        [SuppressUnmanagedCodeSecurity]
        [DllImport(ExternDll.Kernel32, EntryPoint = "GetModuleFileName", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern int IntGetModuleFileName(HandleRef hModule, StringBuilder buffer, int length);

        // Critical: This code elevates to unmanaged code permission by calling into IntGetModuleFileName
        [SecurityCritical]
        internal static string GetModuleFileName(HandleRef hModule)
        {
            // .Net is currently far behind Windows with regard to supporting paths longer than MAX_PATH.
            // At one point it was tested trying to load UNC paths longer than MAX_PATH and mscorlib threw
            // FileIOExceptions before WPF was even on the stack.
            // All the same, we still want to have this grow-and-retry logic because the CLR can be hosted
            // in a native application.  Callers bothering to use this rather than Assembly based reflection
            // are likely doing so because of (at least the potential for) the returned name referring to a
            // native module.
            var buffer = new StringBuilder((int)Win32Value.MAX_PATH);
            while (true)
            {
                var size = IntGetModuleFileName(hModule, buffer, buffer.Capacity);
                if (size == 0)
                {
                    throw new Win32Exception();
                }

                // GetModuleFileName returns nSize when it's truncated but does NOT set the last error.
                // MSDN documentation says this has changed in Windows 2000+.
                if (size == buffer.Capacity)
                {
                    // Enlarge the buffer and try again.
                    buffer.EnsureCapacity(buffer.Capacity * 2);
                    continue;
                }

                return buffer.ToString();
            }
        }

        // Critical: P-Invokes
        [SecurityCritical]
        [DllImport(ExternDll.User32, ExactSpelling = true, CharSet = CharSet.Auto)]
        public static extern IntPtr GetFocus();

        // Critical: P-Invokes
        [SecurityCritical]
        [DllImport(ExternDll.User32, CharSet = CharSet.Auto)]
        public static extern int MapWindowPoints(HandleRef hWndFrom, HandleRef hWndTo, [In, Out] ref RECT rect, int cPoints);

        // Critical: P-Invokes
        [SecurityCritical]
        [DllImport(ExternDll.User32, CharSet = CharSet.Auto)]
        public static extern bool GetWindowRect(HandleRef hWnd, [In, Out] ref RECT rect);
    }
}