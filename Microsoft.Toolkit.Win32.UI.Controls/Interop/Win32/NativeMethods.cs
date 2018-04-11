// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

namespace Microsoft.Toolkit.Win32.UI.Controls.Interop.Win32
{
    // Some native methods are shimmed through public versions that handle converting failures into thrown exceptions.
    internal static class NativeMethods
    {
        public static HandleRef NullHandleRef = new HandleRef(null, IntPtr.Zero);

        [SecurityCritical]
        [DllImport(ExternDll.User32, ExactSpelling = true, CharSet = CharSet.Auto)]
        public static extern bool IsChild(HandleRef parent, HandleRef child);

        [SecurityCritical]
        [DllImport(ExternDll.Kernel32, SetLastError = true, EntryPoint = "GetVersionEx")]
        private static extern bool _GetVersionEx(ref OSVERSIONINFOEX osVersionInfo);

        // With the release of Windows 8.1, the behavior of the GetVersionEx API has changed the
        // value it will return for the OS version. The value returned now depends on how the application
        // is manifested.
        //
        // Applications not manifested for for Windows 8.1 or Windows 10 will return the Windows 8 OS
        // version value of 6.2. Once an application is manifested for a given OS version, GetVersionEx
        // will always return the version that the application is manifested for in future releases.
        [SecurityCritical]
        public static OSVERSIONINFOEX GetVersionEx()
        {
            var osVersionInfo = new OSVERSIONINFOEX { OSVersionInfoSize = Marshal.SizeOf(typeof(OSVERSIONINFOEX)) };
            if (!_GetVersionEx(ref osVersionInfo))
            {
                HRESULT.ThrowLastError();
            }

            return osVersionInfo;
        }

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

        /// <summary>
        /// Enables the mouse to act as a pointer input device and send WM_POINTER messages.
        /// </summary>
        /// <param name="fEnable"><see langword="true"/> to turn on mouse input support</param>
        /// <returns></returns>
        [SecurityCritical]  // P-Invokes
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

        //   Critical : P-Invokes
        [SecurityCritical]
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "CreateWindowExW")]
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
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
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
            if (IntPtr.Zero == ret)
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


        //   Critical : P-Invokes
        [SecurityCritical]
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DestroyWindow(IntPtr hwnd);

        //   Critical : P-Invokes
        [SecurityCritical]
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool IsWindow(IntPtr hwnd);


        //   Critical : P-Invokes
        [SecurityCritical]
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        [DllImport("kernel32.dll", EntryPoint = "GetModuleFileName", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern int _GetModuleFileName(IntPtr hModule, StringBuilder lpFilename, int nSize);



        //   Critical : Calls critical method
        [SecurityCritical]
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
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


        //   Critical : P-Invokes
        [SecurityCritical]
        [DllImport("kernel32.dll", EntryPoint = "GetModuleHandleW", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern IntPtr _GetModuleHandle([MarshalAs(UnmanagedType.LPWStr)] string lpModuleName);


        //   Critical : Calls critical method
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


    internal static class ExternDll
    {
        public const string Kernel32 = "kernel32.dll";
        public const string User32 = "user32.dll";
        public const string Ntdll = "ntdll.dll";
    }
}
