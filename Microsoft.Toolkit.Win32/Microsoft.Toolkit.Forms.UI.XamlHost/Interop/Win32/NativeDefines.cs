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
        public const int WM_MOVE = (int)WM.MOVE;
        public const int WM_SIZE = (int)WM.SIZE;

        public const int WM_WINDOWPOSCHANGING = (int)WM.WINDOWPOSCHANGING;
        public const int WM_WINDOWPOSCHANGED = (int)WM.WINDOWPOSCHANGED;

        public const int WM_SETFOCUS = (int)WM.SETFOCUS;
        public const int WM_KILLFOCUS = (int)WM.KILLFOCUS;

        public static IntPtr HWND_TOP { get; } = IntPtr.Zero;

        public static IntPtr HWND_TOPMOST { get; } = IntPtr.Zero - 1;
    }
}
