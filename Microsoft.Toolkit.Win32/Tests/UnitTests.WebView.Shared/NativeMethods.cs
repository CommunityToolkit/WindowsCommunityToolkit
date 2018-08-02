// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace Microsoft.Toolkit.Win32.UI.Controls.Interop.Win32
{
    internal static class NativeMethods
    {
        [SecurityCritical]
        [DllImport(ExternDll.Ntdll, SetLastError = true, EntryPoint = "RtlGetVersion")]
        private static extern bool _RtlGetVersion(ref OSVERSIONINFOEX versionInfo);

        [SecurityCritical]
        internal static OSVERSIONINFOEX RtlGetVersion()
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

        // Critical: This calls into Marshal.GetExceptionForHR which is critical
        //           it populates the exception object from data stored in a per thread IErrorInfo
        //           the IErrorInfo may have security sensitive information like file paths stored in it
        // TreatAsSafe: Uses overload of GetExceptionForHR that omits IErrorInfo information from exception message
        [SecuritySafeCritical]
        internal static Exception GetExceptionForHR(int hr)
        {
            return Marshal.GetExceptionForHR(hr, new IntPtr(-1));
        }
    }
}