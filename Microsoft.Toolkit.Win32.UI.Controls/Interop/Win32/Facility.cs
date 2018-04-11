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