// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;

namespace Microsoft.Toolkit.Win32.UI.Controls.Interop.Win32
{
    /// <summary>Wrapper for HRESULT status codes.</summary>
    [StructLayout(LayoutKind.Explicit)]
    internal struct HRESULT
    {
        [FieldOffset(0)]
        private readonly uint _value;

        // NOTE: These public static field declarations are automatically
        // picked up by ToString through reflection.

        /// <summary>S_OK</summary>
        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields", Justification = "May only be used in DEBUG builds")]
        public static readonly HRESULT S_OK = new HRESULT(0x00000000);

        /// <summary>S_FALSE</summary>
        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields", Justification = "May only be used in DEBUG builds")]
        public static readonly HRESULT S_FALSE = new HRESULT(0x00000001);

        /// <summary>E_PENDING</summary>
        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields", Justification = "May only be used in DEBUG builds")]
        public static readonly HRESULT E_PENDING = new HRESULT(0x8000000A);

        /// <summary>E_NOTIMPL</summary>
        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields", Justification = "May only be used in DEBUG builds")]
        public static readonly HRESULT E_NOTIMPL = new HRESULT(0x80004001);

        /// <summary>E_NOINTERFACE</summary>
        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields", Justification = "May only be used in DEBUG builds")]
        public static readonly HRESULT E_NOINTERFACE = new HRESULT(0x80004002);

        /// <summary>E_POINTER</summary>
        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields", Justification = "May only be used in DEBUG builds")]
        public static readonly HRESULT E_POINTER = new HRESULT(0x80004003);

        /// <summary>E_ABORT</summary>
        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields", Justification = "May only be used in DEBUG builds")]
        public static readonly HRESULT E_ABORT = new HRESULT(0x80004004);

        /// <summary>E_FAIL</summary>
        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields", Justification = "May only be used in DEBUG builds")]
        public static readonly HRESULT E_FAIL = new HRESULT(0x80004005);

        /// <summary>E_UNEXPECTED</summary>
        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields", Justification = "May only be used in DEBUG builds")]
        public static readonly HRESULT E_UNEXPECTED = new HRESULT(0x8000FFFF);

        /// <summary>STG_E_INVALIDFUNCTION</summary>
        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields", Justification = "May only be used in DEBUG builds")]
        public static readonly HRESULT STG_E_INVALIDFUNCTION = new HRESULT(0x80030001);

        /// <summary>REGDB_E_CLASSNOTREG</summary>
        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields", Justification = "May only be used in DEBUG builds")]
        public static readonly HRESULT REGDB_E_CLASSNOTREG = new HRESULT(0x80040154);

        /// <summary>DESTS_E_NO_MATCHING_ASSOC_HANDLER.  Win7 internal error code for Jump Lists.</summary>
        /// <remarks>There is no Assoc Handler for the given item registered by the specified application.</remarks>
        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields", Justification = "May only be used in DEBUG builds")]
        public static readonly HRESULT DESTS_E_NO_MATCHING_ASSOC_HANDLER = new HRESULT(0x80040F03);

        /// <summary>DESTS_E_NORECDOCS.  Win7 internal error code for Jump Lists.</summary>
        /// <remarks>The given item is excluded from the recent docs folder by the NoRecDocs bit on its registration.</remarks>
        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields", Justification = "May only be used in DEBUG builds")]
        public static readonly HRESULT DESTS_E_NORECDOCS = new HRESULT(0x80040F04);

        /// <summary>DESTS_E_NOTALLCLEARED.  Win7 internal error code for Jump Lists.</summary>
        /// <remarks>Not all of the items were successfully cleared</remarks>
        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields", Justification = "May only be used in DEBUG builds")]
        public static readonly HRESULT DESTS_E_NOTALLCLEARED = new HRESULT(0x80040F05);

        /// <summary>E_ACCESSDENIED</summary>
        /// <remarks>Win32Error ERROR_ACCESS_DENIED.</remarks>
        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields", Justification = "May only be used in DEBUG builds")]
        public static readonly HRESULT E_ACCESSDENIED = new HRESULT(0x80070005);

        /// <summary>E_OUTOFMEMORY</summary>
        /// <remarks>Win32Error ERROR_OUTOFMEMORY.</remarks>
        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields", Justification = "May only be used in DEBUG builds")]
        public static readonly HRESULT E_OUTOFMEMORY = new HRESULT(0x8007000E);

        /// <summary>E_INVALIDARG</summary>
        /// <remarks>Win32Error ERROR_INVALID_PARAMETER.</remarks>
        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields", Justification = "May only be used in DEBUG builds")]
        public static readonly HRESULT E_INVALIDARG = new HRESULT(0x80070057);

        /// <summary>INTSAFE_E_ARITHMETIC_OVERFLOW</summary>
        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields", Justification = "May only be used in DEBUG builds")]
        public static readonly HRESULT INTSAFE_E_ARITHMETIC_OVERFLOW = new HRESULT(0x80070216);

        /// <summary>COR_E_OBJECTDISPOSED</summary>
        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields", Justification = "May only be used in DEBUG builds")]
        public static readonly HRESULT COR_E_OBJECTDISPOSED = new HRESULT(0x80131622);

        /// <summary>WC_E_GREATERTHAN</summary>
        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields", Justification = "May only be used in DEBUG builds")]
        public static readonly HRESULT WC_E_GREATERTHAN = new HRESULT(0xC00CEE23);

        /// <summary>WC_E_SYNTAX</summary>
        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields", Justification = "May only be used in DEBUG builds")]
        public static readonly HRESULT WC_E_SYNTAX = new HRESULT(0xC00CEE2D);

        /// <summary>
        /// Initializes a new instance of the <see cref="HRESULT"/> struct.
        /// </summary>
        /// <param name="i"><see cref="uint" /> containing HRESULT.</param>
        public HRESULT(uint i)
        {
            _value = i;
        }

        public static HRESULT Make(bool severe, Facility facility, int code)
        {
            // #define MAKE_HRESULT(sev,fac,code) \
            //    ((HRESULT) (((unsigned long)(sev)<<31) | ((unsigned long)(fac)<<16) | ((unsigned long)(code))) )

            // Severity has 1 bit reserved.
            // bitness is enforced by the boolean parameter.

            // Facility has 11 bits reserved (different than SCODES, which have 4 bits reserved)
            // MSDN documentation incorrectly uses 12 bits for the ESE facility (e5e), so go ahead and let that one slide.
            // And WIC also ignores it the documented size...
            Verify.Implies((int)facility != (int)((int)facility & 0x1FF), facility == Facility.Ese || facility == Facility.WinCodec);

            // Code has 4 bits reserved.
            Verify.AreEqual(code, code & 0xFFFF);

            return new HRESULT((uint)((severe ? 1 << 31 : 0) | ((int)facility << 16) | code));
        }

        /// <summary>
        /// Gets HRESULT_FACILITY
        /// </summary>
        public Facility Facility
        {
            get
            {
                return GetFacility((int)_value);
            }
        }

        public static Facility GetFacility(int errorCode)
        {
            // #define HRESULT_FACILITY(hr)  (((hr) >> 16) & 0x1fff)
            return (Facility)((errorCode >> 16) & 0x1fff);
        }

        /// <summary>
        /// Gets HRESULT_CODE
        /// </summary>
        public int Code
        {
            get
            {
                return GetCode((int)_value);
            }
        }

        public static int GetCode(int error)
        {
            // #define HRESULT_CODE(hr)    ((hr) & 0xFFFF)
            return (int)(error & 0xFFFF);
        }

        /// <summary>
        /// Get a string representation of this HRESULT.
        /// </summary>
        /// <returns>Name of HRESULT</returns>
        public override string ToString()
        {
            // Use reflection to try to name this HRESULT.
            // This is expensive, but if someone's ever printing HRESULT strings then
            // I think it's a fair guess that they're not in a performance critical area
            // (e.g. printing exception strings).
            // This is less error prone than trying to keep the list in the function.
            // To properly add an HRESULT's name to the ToString table, just add the HRESULT
            // like all the others above.
            //
            // CONSIDER: This data is static.  It could be cached
            // after first usage for fast lookup since the keys are unique.
            foreach (var publicStaticField in typeof(HRESULT).GetFields(BindingFlags.Static | BindingFlags.Public))
            {
                if (publicStaticField.FieldType == typeof(HRESULT))
                {
                    var hr = (HRESULT)publicStaticField.GetValue(null);
                    if (hr == this)
                    {
                        return publicStaticField.Name;
                    }
                }
            }

            // Try Win32 error codes also
            if (Facility == Facility.Win32)
            {
                foreach (var publicStaticField in typeof(Win32Error).GetFields(BindingFlags.Static | BindingFlags.Public))
                {
                    if (publicStaticField.FieldType == typeof(Win32Error))
                    {
                        var error = (Win32Error)publicStaticField.GetValue(null);
                        if ((HRESULT)error == this)
                        {
                            return "HRESULT_FROM_WIN32(" + publicStaticField.Name + ")";
                        }
                    }
                }
            }

            // If there's no good name for this HRESULT,
            // return the string as readable hex (0x########) format.
            return string.Format(CultureInfo.InvariantCulture, "0x{0:X8}", _value);
        }

        public override bool Equals(object obj)
        {
            try
            {
                return ((HRESULT)obj)._value == _value;
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

        public static bool operator ==(HRESULT hrLeft, HRESULT hrRight)
        {
            return hrLeft._value == hrRight._value;
        }

        public static bool operator !=(HRESULT hrLeft, HRESULT hrRight)
        {
            return !(hrLeft == hrRight);
        }

        public bool Succeeded
        {
            get { return (int)_value >= 0; }
        }

        public bool Failed
        {
            get { return (int)_value < 0; }
        }

        /// <summary>
        /// Convert the result of Win32 GetLastError() into a raised exception.
        /// </summary>
        [SecurityCritical]
        public static void ThrowLastError()
        {
            ((HRESULT)Win32Error.GetLastError()).ThrowIfFailed();

            // Only expecting to call this when we're expecting a failed GetLastError()
            Verify.Fail();
        }

        // Critical: P-Invoke
        [SecurityCritical]
        [SuppressMessage(
                "Microsoft.Usage",
                "CA2201:DoNotRaiseReservedExceptionTypes",
                Justification = "Only recreating Exceptions that were already raised.")]
        [SuppressMessage(
                "Microsoft.Globalization",
                "CA1303:DoNotPassLiteralsAsLocalizedParameters",
                Justification ="For DEBUG only")]
        public void ThrowIfFailed(string message = null)
        {
            if (Failed)
            {
                if (string.IsNullOrEmpty(message))
                {
                    message = ToString();
                }
#if DEBUG
                else
                {
                    message += " (" + ToString() + ")";
                }
#endif

                // Wow.  Reflection in a throw call.  Later on this may turn out to have been a bad idea.
                // If you're throwing an exception I assume it's OK for me to take some time to give it back.
                // I want to convert the HRESULT to a more appropriate exception type than COMException.
                // Marshal.ThrowExceptionForHR does this for me, but the general call uses GetErrorInfo
                // if it's set, and then ignores the HRESULT that I've provided.  This makes it so this
                // call works the first time but you get burned on the second.  To avoid this, I use
                // the overload that explicitly ignores the IErrorInfo.
                // In addition, the function doesn't allow me to set the Message unless I go through
                // the process of implementing an IErrorInfo and then use that.  There's no stock
                // implementations of IErrorInfo available and I don't think it's worth the maintenance
                // overhead of doing it, nor would it have significant value over this approach.
                var e = NativeMethods.GetExceptionForHR((int)_value);
                Verify.IsNotNull(e);

                // ArgumentNullException doesn't have the right constructor parameters,
                // (nor does Win32Exception...)
                // but E_POINTER gets mapped to NullReferenceException,
                // so I don't think it will ever matter.
                Verify.IsFalse(e is ArgumentNullException);

                // If we're not getting anything better than a COMException from Marshal,
                // then at least check the facility and attempt to do better ourselves.
                if (e.GetType() == typeof(COMException))
                {
                    switch (Facility)
                    {
                        case Facility.Win32:
                            e = CreateWin32Exception(Code, message);
                            break;

                        default:
                            e = new COMException(message, (int)_value);
                            break;
                    }
                }
                else
                {
                    var cons = e.GetType().GetConstructor(new[] { typeof(string) });
                    if (cons != null)
                    {
                        e = cons.Invoke(new object[] { message }) as Exception;
                        Verify.IsNotNull(e);
                    }
                }

                throw e;
            }
        }

        [SecuritySafeCritical]
        [SuppressMessage(
            "Microsoft.Security",
            "CA2136:TransparencyAnnotationsShouldNotConflict",
            Justification = "Calls safe overload of Win32Exception ctor that explicitly sets the error code and message.")]
        private static Exception CreateWin32Exception(int code, string message)
        {
            return new Win32Exception(code, message);
        }
    }
}