// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace Microsoft.Toolkit.Win32.UI.Controls.Interop.Win32
{
    // Some native methods are shimmed through public versions that handle converting failures into thrown exceptions.

    /// <summary>
    /// Native methods HRESULT
    /// </summary>
    internal static partial class NativeMethods
    {
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