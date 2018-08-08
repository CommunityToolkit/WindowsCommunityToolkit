// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Security;

namespace Microsoft.Toolkit.Win32.UI.Controls.Interop.Win32
{
    /// <summary>
    /// Native methods for OS version checks
    /// </summary>
    internal static partial class NativeMethods
    {
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
    }
}