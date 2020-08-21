// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#pragma warning disable SA1503 // Braces should not be omitted
#pragma warning disable SA1117 // Parameters should be on same line or separate lines
#pragma warning disable SA1313 // Parameter names should begin with lower-case letter
#pragma warning disable SA1407 // Arithmetic expressions should declare precedence
#pragma warning disable SA1121 // Use built-in type alias
#pragma warning disable SA1400 // Access modifier should be declared
#pragma warning disable SA1512 // Single-line comments should not be followed by blank line
#pragma warning disable SA1649 // File name should match first type name
#pragma warning disable SA1515 // Single-line comment should be preceded by blank line
#pragma warning disable SA1513 // Closing brace should be followed by blank line
#pragma warning disable SA1005 // Single line comments should begin with single space
#pragma warning disable SA1106 // Code should not contain empty statements
#pragma warning disable SA1027 // Use tabs correctly
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

// http://www.pinvoke.net/

namespace Toolbelt.Drawing.Win32
{
    [Flags]
    internal enum LOAD_LIBRARY : uint
    {
        DONT_RESOLVE_DLL_REFERENCES = 0x00000001,
        IGNORE_CODE_AUTHZ_LEVEL = 0x00000010,
        AS_DATAFILE = 0x00000002,
        AS_DATAFILE_EXCLUSIVE = 0x00000040,
        AS_IMAGE_RESOURCE = 0x00000020,
        WITH_ALTERED_SEARCH_PATH = 0x00000008
    }

    internal enum RT
    {
        CURSOR = 1,
        BITMAP = 2,
        ICON = 3,
        MENU = 4,
        DIALOG = 5,
        STRING = 6,
        FONTDIR = 7,
        FONT = 8,
        ACCELERATOR = 9,
        RCDATA = 10,
        MESSAGETABLE = 11,
        GROUP_CURSOR = 12,
        GROUP_ICON = 14,
        VERSION = 16,
        DLGINCLUDE = 17,
        PLUGPLAY = 19,
        VXD = 20,
        ANICURSOR = 21,
        ANIICON = 22,
        HTML = 23,
        MANIFEST = 24
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    [DebuggerDisplay("{Cx} x {Cy}, {BitCount}bit, {Size}bytes")]
    internal struct ICONRESINF
    {
        public byte Cx;
        public byte Cy;
        public byte ColorCount;
        public byte Reserved;
        public UInt16 Planes;
        public UInt16 BitCount;
        public UInt32 Size;
        public UInt16 ID;
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct ICONRESHEAD
    {
        public UInt16 Reserved;
        public UInt16 Type;
        public UInt16 Count;
        // public ICONRESINF[] IconInf;
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    [DebuggerDisplay("{Cx} x {Cy}, {BitCount}bit, {Size}bytes")]
    internal struct ICONFILEINF
    {
        public byte Cx;
        public byte Cy;
        public byte ColorCount;
        public byte Reserved;
        public UInt16 Planes;
        public UInt16 BitCount;
        public UInt32 Size;
        public UInt32 Address;
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct ICONFILEHEAD
    {
        public UInt16 Reserved;
        public UInt16 Type;
        public UInt16 Count;
        //ICONFILEINF	IconInf[];.
    };

    internal delegate bool EnumResNameProcDelegate(IntPtr hModule, RT lpszType, IntPtr lpszName, IntPtr lParam);

    internal class Kernel32
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr LoadLibraryEx(string lpFileName, IntPtr hReservedNull, LOAD_LIBRARY dwFlags);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool FreeLibrary(IntPtr hModule);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool EnumResourceNames(IntPtr hModule, RT type, EnumResNameProcDelegate lpEnumFunc, IntPtr lParam);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr FindResource(IntPtr hModule, IntPtr lpszName, RT type);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr LoadResource(IntPtr hModule, IntPtr hResInfo);

        [DllImport("Kernel32.dll", SetLastError = true)]
        public static extern IntPtr LockResource(IntPtr hResource);

        [DllImport("Kernel32.dll", SetLastError = true)]
        public static extern uint SizeofResource(IntPtr hModule, IntPtr hResInfo);
    }
}