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
    /// Native methods for DPI
    /// </summary>
    internal static partial class NativeMethods
    {
        public const int LOGPIXELSX = 88;
        public const int LOGPIXELSY = 90;

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
    }
}