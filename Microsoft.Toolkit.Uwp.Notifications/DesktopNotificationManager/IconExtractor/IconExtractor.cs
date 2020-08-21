// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#pragma warning disable SA1503 // Braces should not be omitted
#pragma warning disable SA1117 // Parameters should be on same line or separate lines
#pragma warning disable SA1313 // Parameter names should begin with lower-case letter
#pragma warning disable SA1407 // Arithmetic expressions should declare precedence
#pragma warning disable SA1121 // Use built-in type alias
#pragma warning disable SA1400 // Access modifier should be declared
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Toolbelt.Drawing.Win32;

namespace Toolbelt.Drawing
{
    /// <summary>
    /// Extract .ico file
    /// </summary>
    internal class IconExtractor
    {
        /// <summary>
        /// Extract first .ico file from Win32 resource of PE format file (.exe, .dll).
        /// </summary>
        /// <param name="sourceFile">path of PE format file to extract .ico file</param>
        /// <param name="stream">stream that written to .ico file which is extracted.</param>
        public static void Extract1stIconTo(string sourceFile, Stream stream)
        {
            var hModule = Kernel32.LoadLibraryEx(sourceFile, IntPtr.Zero, LOAD_LIBRARY.AS_DATAFILE);
            if (hModule == null) throw new Win32Exception();

            try
            {
                Kernel32.EnumResourceNames(hModule, RT.GROUP_ICON,
                    (IntPtr _hModule, RT type, IntPtr lpszName, IntPtr lParam) =>
                    {
                        var iconResInfos = GetIconResInfo(_hModule, lpszName);
                        WriteIconData(hModule, iconResInfos, stream);
                        return false;
                    }, IntPtr.Zero);
            }
            finally
            {
                Kernel32.FreeLibrary(hModule);
            }
        }

        private static ICONRESINF[] GetIconResInfo(IntPtr hModule, IntPtr lpszName)
        {
            var hResInf = Kernel32.FindResource(hModule, lpszName, RT.GROUP_ICON);
            if (hResInf == null) throw new Win32Exception();

            var hResource = Kernel32.LoadResource(hModule, hResInf);
            if (hResource == null) throw new Win32Exception();

            var ptrResource = Kernel32.LockResource(hResource);
            if (ptrResource == null) throw new Win32Exception();

            var iconResHead = (ICONRESHEAD)Marshal.PtrToStructure(ptrResource, typeof(ICONRESHEAD));
            var s1 = Marshal.SizeOf(typeof(ICONRESHEAD));
            var s2 = Marshal.SizeOf(typeof(ICONRESINF));

            var iconResInfos = Enumerable.Range(0, iconResHead.Count)
                .Select(i => (ICONRESINF)Marshal.PtrToStructure(ptrResource + s1 + s2 * i, typeof(ICONRESINF)))
                .ToArray();

            return iconResInfos;
        }

        private static void WriteIconData(IntPtr hModule, ICONRESINF[] iconResInfos, Stream stream)
        {
            var s1 = Marshal.SizeOf(typeof(ICONFILEHEAD));
            var s2 = Marshal.SizeOf(typeof(ICONFILEINF));
            var address = s1 + s2 * iconResInfos.Length;

            var iconFiles = iconResInfos
                .Select(iconResInf =>
                {
                    var iconBytes = GetResourceBytes(hModule, (IntPtr)iconResInf.ID, RT.ICON);
                    var iconFileInf = new ICONFILEINF
                    {
                        Cx = iconResInf.Cx,
                        Cy = iconResInf.Cy,
                        ColorCount = iconResInf.ColorCount,
                        Planes = iconResInf.Planes,
                        BitCount = iconResInf.BitCount,
                        Size = iconResInf.Size,
                        Address = (UInt32)address
                    };
                    address += iconBytes.Length;
                    return new { iconBytes, iconFileInf };
                }).ToList();

            // write headers
            var iconFileHead = new ICONFILEHEAD
            {
                Type = 1,
                Count = (UInt16)iconResInfos.Length
            };
            var iconFileHeadBytes = StructureToBytes(iconFileHead);
            stream.Write(iconFileHeadBytes, 0, iconFileHeadBytes.Length);
            iconFiles.ForEach(iconFile =>
            {
                var bytes = StructureToBytes(iconFile.iconFileInf);
                stream.Write(bytes, 0, bytes.Length);
            });

            // write images
            iconFiles.ForEach(iconFile =>
            {
                stream.Write(iconFile.iconBytes, 0, iconFile.iconBytes.Length);
            });
        }

        static byte[] GetResourceBytes(IntPtr hModule, IntPtr lpszName, RT type)
        {
            var hResInf = Kernel32.FindResource(hModule, lpszName, type);
            if (hResInf == null) throw new Win32Exception();

            var hResource = Kernel32.LoadResource(hModule, hResInf);
            if (hResource == null) throw new Win32Exception();

            var ptrResource = Kernel32.LockResource(hResource);
            if (ptrResource == null) throw new Win32Exception();

            var size = Kernel32.SizeofResource(hModule, hResInf);
            var buff = new byte[size];
            Marshal.Copy(ptrResource, buff, 0, buff.Length);

            return buff;
        }

        private static byte[] StructureToBytes(object obj)
        {
            // http://schima.hatenablog.com/entry/20090512/1242139542
            var size = Marshal.SizeOf(obj);
            var bytes = new byte[size];
            var gch = GCHandle.Alloc(bytes, GCHandleType.Pinned);
            Marshal.StructureToPtr(obj, gch.AddrOfPinnedObject(), false);
            gch.Free();

            return bytes;
        }
    }
}