// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Runtime.InteropServices;

namespace Microsoft.Toolkit.Win32.UI.Controls.Interop.Win32
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    internal struct OSVERSIONINFOEX
    {
        // The OSVersionInfoSize field must be set to Marshal.SizeOf(typeof(OSVERSIONINFOEX))
        internal int OSVersionInfoSize;
        internal int MajorVersion;
        internal int MinorVersion;
        internal int BuildNumber;
        internal int PlatformId;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        internal string CSDVersion;
        internal ushort ServicePackMajor;
        internal ushort ServicePackMinor;
        internal short SuiteMask;
        internal ProductType ProductType;
        internal byte Reserved;
    }
}