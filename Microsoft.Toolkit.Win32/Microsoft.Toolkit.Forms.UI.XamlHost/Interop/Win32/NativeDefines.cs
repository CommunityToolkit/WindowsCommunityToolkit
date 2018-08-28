// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Microsoft.Toolkit.Win32.UI.Controls.Interop.Win32;

namespace Microsoft.Toolkit.Forms.UI.XamlHost.Interop.Win32
{
    /// <summary>
    /// Definitions required for native interop
    /// </summary>
    internal static partial class NativeDefines
    {
        // Window Messages
        public const int WM_MOVE = (int)WM.MOVE;
        public const int WM_SIZE = (int)WM.SIZE;
        public const int WM_WINDOWPOSCHANGING = (int)WM.WINDOWPOSCHANGING;
        public const int WM_WINDOWPOSCHANGED = (int)WM.WINDOWPOSCHANGED;
        public const int WM_SETFOCUS = (int)WM.SETFOCUS;
        public const int WM_KILLFOCUS = (int)WM.KILLFOCUS;
        public const int WM_ACTIVATE = 0x0006;
        public const int WM_PAINT = 0x000F;
        public const int WM_ERASEBKGND = 0x14; // 20
        public const int WM_PARENTNOTIFY = 0x0210;
        public const int WM_SETREDRAW = 0x000B;
        public const int WM_NCPAINT = 0x85; // 133
        public const int WM_POINTERDOWN = 0x0246;
        public const int WM_LBUTTONDOWN = 0x0201;
        public const int WM_MOUSEACTIVATE = 0x0021;
        public const int WM_USER = 0x0400;
        public const int WM_REFLECT = WM_USER + 0x1C00;
        public const int WM_KEYDOWN = 0x0100;
        public const int WM_KEYUP = 0x0101;
        public const int WM_CHAR = 0x0102;
        public const int WM_SYSKEYDOWN = 0x0104;
        public const int WM_SYSKEYUP = 0x0105;

        // Window Styles
        public const uint WS_EX_CONTROLPARENT = 0x00010000;
        public const int GWL_STYLE = -16;

        public static IntPtr HWND_TOP { get; } = IntPtr.Zero;

        public static IntPtr HWND_TOPMOST { get; } = IntPtr.Zero - 1;
    }
}
