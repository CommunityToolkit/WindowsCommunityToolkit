// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.Toolkit.Win32.UI.Controls.Interop.Win32
{
    internal enum Facility
    {
        /// <summary>FACILITY_NULL</summary>
        Null = 0,

        /// <summary>FACILITY_RPC</summary>
        Rpc = 1,

        /// <summary>FACILITY_DISPATCH</summary>
        Dispatch = 2,

        /// <summary>FACILITY_STORAGE</summary>
        Storage = 3,

        /// <summary>FACILITY_ITF</summary>
        Itf = 4,

        /// <summary>FACILITY_WIN32</summary>
        Win32 = 7,

        /// <summary>FACILITY_WINDOWS</summary>
        Windows = 8,

        /// <summary>FACILITY_CONTROL</summary>
        Control = 10,

        /// <summary>MSDN doced facility code for ESE errors.</summary>
        Ese = 0xE5E,

        /// <summary>FACILITY_WINCODEC (WIC)</summary>
        WinCodec = 0x898,
    }
}