// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

namespace Microsoft.Toolkit.Win32.UI.Controls.Interop.Win32
{
    // Some native methods are shimmed through public versions that handle converting failures into thrown exceptions.

    /// <summary>
    /// Native methods for Win32 controls
    /// </summary>
    internal static partial class NativeMethods
    {
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

        // Critical : P-Invokes
        [SecurityCritical]
        [DllImport(ExternDll.Kernel32, EntryPoint = "GetModuleFileName", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern int _GetModuleFileName(IntPtr hModule, StringBuilder lpFilename, int nSize);

        // Critical : P-Invokes
        [SecurityCritical]
        [DllImport(ExternDll.Kernel32, EntryPoint = "GetModuleHandleW", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern IntPtr _GetModuleHandle([MarshalAs(UnmanagedType.LPWStr)] string lpModuleName);
    }
}
