// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Security;

namespace Microsoft.Toolkit.Win32.UI.Controls.Interop.Win32
{
    /// <summary>
    /// Wrapper for common Win32 status codes.
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    internal struct Win32Error
    {
        [FieldOffset(0)]
        private readonly int _value;

        // NOTE: These public static field declarations are automatically
        // picked up by (HRESULT's) ToString through reflection.

        /// <summary>The operation completed successfully.</summary>
        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields", Justification = "May be used in DEBUG")]
        public static readonly Win32Error ERROR_SUCCESS = new Win32Error(0);

        /// <summary>Incorrect function.</summary>
        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields", Justification = "May be used in DEBUG")]
        public static readonly Win32Error ERROR_INVALID_FUNCTION = new Win32Error(1);

        /// <summary>The system cannot find the file specified.</summary>
        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields", Justification = "May be used in DEBUG")]
        public static readonly Win32Error ERROR_FILE_NOT_FOUND = new Win32Error(2);

        /// <summary>The system cannot find the path specified.</summary>
        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields", Justification = "May be used in DEBUG")]
        public static readonly Win32Error ERROR_PATH_NOT_FOUND = new Win32Error(3);

        /// <summary>The system cannot open the file.</summary>
        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields", Justification = "May be used in DEBUG")]
        public static readonly Win32Error ERROR_TOO_MANY_OPEN_FILES = new Win32Error(4);

        /// <summary>Access is denied.</summary>
        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields", Justification = "May be used in DEBUG")]
        public static readonly Win32Error ERROR_ACCESS_DENIED = new Win32Error(5);

        /// <summary>The handle is invalid.</summary>
        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields", Justification = "May be used in DEBUG")]
        public static readonly Win32Error ERROR_INVALID_HANDLE = new Win32Error(6);

        /// <summary>Not enough storage is available to complete this operation.</summary>
        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields", Justification = "May be used in DEBUG")]
        public static readonly Win32Error ERROR_OUTOFMEMORY = new Win32Error(14);

        /// <summary>There are no more files.</summary>
        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields", Justification = "May be used in DEBUG")]
        public static readonly Win32Error ERROR_NO_MORE_FILES = new Win32Error(18);

        /// <summary>The process cannot access the file because it is being used by another process.</summary>
        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields", Justification = "May be used in DEBUG")]
        public static readonly Win32Error ERROR_SHARING_VIOLATION = new Win32Error(32);

        /// <summary>The parameter is incorrect.</summary>
        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields", Justification = "May be used in DEBUG")]
        public static readonly Win32Error ERROR_INVALID_PARAMETER = new Win32Error(87);

        /// <summary>The data area passed to a system call is too small.</summary>
        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields", Justification = "May be used in DEBUG")]
        public static readonly Win32Error ERROR_INSUFFICIENT_BUFFER = new Win32Error(122);

        /// <summary>Cannot nest calls to LoadModule.</summary>
        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields", Justification = "May be used in DEBUG")]
        public static readonly Win32Error ERROR_NESTING_NOT_ALLOWED = new Win32Error(215);

        /// <summary>Illegal operation attempted on a registry key that has been marked for deletion.</summary>
        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields", Justification = "May be used in DEBUG")]
        public static readonly Win32Error ERROR_KEY_DELETED = new Win32Error(1018);

        /// <summary>Element not found.</summary>
        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields", Justification = "May be used in DEBUG")]
        public static readonly Win32Error ERROR_NOT_FOUND = new Win32Error(1168);

        /// <summary>There was no match for the specified key in the index.</summary>
        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields", Justification = "May be used in DEBUG")]
        public static readonly Win32Error ERROR_NO_MATCH = new Win32Error(1169);

        /// <summary>An invalid device was specified.</summary>
        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields", Justification = "May be used in DEBUG")]
        public static readonly Win32Error ERROR_BAD_DEVICE = new Win32Error(1200);

        /// <summary>The operation was canceled by the user.</summary>
        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields", Justification = "May be used in DEBUG")]
        public static readonly Win32Error ERROR_CANCELLED = new Win32Error(1223);

        /// <summary>Not all privileges or groups references are assigned to the caller </summary>
        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields", Justification = "May be used in DEBUG")]
        public static readonly Win32Error ERROR_NOT_ALL_ASSIGNED = new Win32Error(1300);

        /// <summary>The window class was already registered.</summary>
        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields", Justification = "May be used in DEBUG")]
        public static readonly Win32Error ERROR_CLASS_ALREADY_EXISTS = new Win32Error(1410);

        /// <summary>The specified datatype is invalid.</summary>
        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields", Justification = "May be used in DEBUG")]
        public static readonly Win32Error ERROR_INVALID_DATATYPE = new Win32Error(1804);

        /// <summary>
        /// Initializes a new instance of the <see cref="Win32Error"/> struct.
        /// </summary>
        /// <param name="i">The integer value of the error.</param>
        public Win32Error(int i)
        {
            _value = i;
        }

        /// <summary>Performs HRESULT_FROM_WIN32 conversion.</summary>
        /// <param name="error">The Win32 error being converted to an HRESULT.</param>
        /// <returns>The equivalent HRESULT value.</returns>
        public static explicit operator HRESULT(Win32Error error)
        {
            // #define __HRESULT_FROM_WIN32(x)
            //     ((HRESULT)(x) <= 0 ? ((HRESULT)(x)) : ((HRESULT) (((x) & 0x0000FFFF) | (FACILITY_WIN32 << 16) | 0x80000000)))
            if (error._value <= 0)
            {
                return new HRESULT((uint)error._value);
            }

            return HRESULT.Make(true, Facility.Win32, error._value & 0x0000FFFF);
        }

        /// <summary>Performs the equivalent of Win32's GetLastError()</summary>
        /// <returns>A Win32Error instance with the result of the native GetLastError</returns>
        [SecurityCritical]
        public static Win32Error GetLastError()
        {
            return new Win32Error(Marshal.GetLastWin32Error());
        }

        // Method version of the cast operation

        /// <summary>Performs HRESULT_FROM_WIN32 conversion.</summary>
        /// <returns>The equivalent HRESULT value.</returns>
        public HRESULT ToHRESULT()
        {
            return (HRESULT)this;
        }

        public override bool Equals(object obj)
        {
            try
            {
                return ((Win32Error)obj)._value == _value;
            }
            catch (InvalidCastException)
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }

        /// <summary>
        /// Compare two Win32 error codes for equality.
        /// </summary>
        /// <param name="errLeft">The first error code to compare.</param>
        /// <param name="errRight">The second error code to compare.</param>
        /// <returns>Whether the two error codes are the same.</returns>
        public static bool operator ==(Win32Error errLeft, Win32Error errRight)
        {
            return errLeft._value == errRight._value;
        }

        /// <summary>
        /// Compare two Win32 error codes for inequality.
        /// </summary>
        /// <param name="errLeft">The first error code to compare.</param>
        /// <param name="errRight">The second error code to compare.</param>
        /// <returns>Whether the two error codes are not the same.</returns>
        public static bool operator !=(Win32Error errLeft, Win32Error errRight)
        {
            return !(errLeft == errRight);
        }
    }
}