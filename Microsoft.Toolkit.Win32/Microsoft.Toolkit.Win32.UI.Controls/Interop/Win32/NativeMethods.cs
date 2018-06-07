// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Security;
using System.Text;

namespace Microsoft.Toolkit.Win32.UI.Controls.Interop.Win32
{
    // Some native methods are shimmed through public versions that handle converting failures into thrown exceptions.
    internal static class NativeMethods
    {
        public const int LOGPIXELSX = 88;
        public const int LOGPIXELSY = 90;

        public const int DPI_AWARENESS_CONTEXT_UNAWARE = -1;
        public const int DPI_AWARENESS_CONTEXT_SYSTEM_AWARE = -2;
        public const int DPI_AWARENESS_CONTEXT_PER_MONITOR_AWARE = -3;
        public const int DPI_AWARENESS_CONTEXT_PER_MONITOR_AWARE_V2 = -4;

#pragma warning disable SA1401 // Fields must be private
        public static HandleRef NullHandleRef = new HandleRef(null, IntPtr.Zero);
#pragma warning restore SA1401 // Fields must be private

        // Critical: P-Invoke
        // Available in Windows 10 version RS1 and above.
        [SecurityCritical]
        [DllImport(ExternDll.User32)]
        [ResourceExposure(ResourceScope.None)]
        public static extern bool AreDpiAwarenessContextsEqual(int dpiContextA, int dpiContextB);

        // Critical: P-Invoke
        // Available in Windows 10 version RS1 and above.
        [SecurityCritical]
        [DllImport(ExternDll.User32)]
        [ResourceExposure(ResourceScope.None)]
        public static extern int GetThreadDpiAwarenessContext();

        // Critical: P-Invoke
        // for Windows 10 version RS2 and above
        [SecurityCritical]
        [DllImport(ExternDll.User32, SetLastError = true)]
        [ResourceExposure(ResourceScope.None)]
        public static extern bool SetProcessDpiAwarenessContext(int dpiFlag);

        [SecurityCritical]
        [DllImport(ExternDll.User32, ExactSpelling = true, CharSet = CharSet.Auto)]
        public static extern bool IsChild(HandleRef parent, HandleRef child);

        [SecurityCritical]
        [DllImport(ExternDll.Ntdll, SetLastError = true, EntryPoint = "RtlGetVersion")]
        private static extern bool _RtlGetVersion(ref OSVERSIONINFOEX versionInfo);

        [SecurityCritical]
        public static OSVERSIONINFOEX RtlGetVersion()
        {
            var osVersionInfo = new OSVERSIONINFOEX { OSVersionInfoSize = Marshal.SizeOf(typeof(OSVERSIONINFOEX)) };
            _RtlGetVersion(ref osVersionInfo);
            var err = Win32Error.GetLastError();
            if (!err.Equals(Win32Error.ERROR_SUCCESS))
            {
                if (osVersionInfo.MajorVersion == 0)
                {
                    err.ToHRESULT().ThrowIfFailed();
                }
            }

            return osVersionInfo;
        }

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

        // Critical: This calls into Marshal.GetExceptionForHR which is critical
        //           it populates the exception object from data stored in a per thread IErrorInfo
        //           the IErrorInfo may have security sensitive information like file paths stored in it
        // TreatAsSafe: Uses overload of GetExceptionForHR that omits IErrorInfo information from exception message
        [SecuritySafeCritical]
        internal static Exception GetExceptionForHR(int hr)
        {
            return Marshal.GetExceptionForHR(hr, new IntPtr(-1));
        }

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

        // Critical : P-Invokes
        [SecurityCritical]
        [DllImport(ExternDll.Kernel32, EntryPoint = "GetModuleFileName", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern int _GetModuleFileName(IntPtr hModule, StringBuilder lpFilename, int nSize);

        // Critical : Calls critical method
        [SecurityCritical]
        public static string GetModuleFileName(IntPtr hModule)
        {
            var buffer = new StringBuilder((int)Win32Value.MAX_PATH);
            while (true)
            {
                var size = _GetModuleFileName(hModule, buffer, buffer.Capacity);
                if (size == 0)
                {
                    HRESULT.ThrowLastError();
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

        // Critical : P-Invokes
        [SecurityCritical]
        [DllImport(ExternDll.Kernel32, EntryPoint = "GetModuleHandleW", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern IntPtr _GetModuleHandle([MarshalAs(UnmanagedType.LPWStr)] string lpModuleName);

        // Critical : Calls critical method
        [SecurityCritical]
        public static IntPtr GetModuleHandle(string lpModuleName)
        {
            var retPtr = _GetModuleHandle(lpModuleName);
            if (retPtr == IntPtr.Zero)
            {
                HRESULT.ThrowLastError();
            }

            return retPtr;
        }
    }
}
