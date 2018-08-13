// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace Microsoft.Toolkit.Win32.UI.Interop.WinForms.Interop.Win32
{
    internal static class NativeDefines
    {
        public const int WM_MOVE = 0x0003;
        public const int WM_SIZE = 0x0005;

        public const int WM_WINDOWPOSCHANGING = 0x0046;
        public const int WM_WINDOWPOSCHANGED = 0x0047;

        public const int WM_SETFOCUS = 0x0007;
        public const int WM_KILLFOCUS = 0x0008;

        public static IntPtr HWND_TOP { get; } = IntPtr.Zero;

        public static IntPtr HWND_TOPMOST { get; } = IntPtr.Zero - 1;
    }
}
