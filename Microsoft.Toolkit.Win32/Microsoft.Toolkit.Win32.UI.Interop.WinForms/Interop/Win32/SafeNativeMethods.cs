// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using Microsoft.Toolkit.Win32.UI.Controls.Interop.Win32;

namespace Microsoft.Toolkit.Win32.UI.Interop.WinForms.Interop.Win32
{
    internal static class SafeNativeMethods
    {
        [DllImport(ExternDll.User32, ExactSpelling = true, CharSet = CharSet.Auto)]
        [ResourceExposure(ResourceScope.None)]
        internal static extern IntPtr SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, int flags);
    }
}
