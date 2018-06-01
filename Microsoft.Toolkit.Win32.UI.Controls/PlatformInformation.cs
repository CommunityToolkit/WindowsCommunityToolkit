// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.IO;

namespace Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT
{
    // Provides simple properties for determining whether the current platform is Windows or Unix-based
    // This intentionally does not use System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform because
    // it incorrectly reports 'true' for Windows in desktop builds running on Unix-based platforms via Mono.
    internal static class PlatformInformation
    {
        public static bool IsWindows => Path.DirectorySeparatorChar == '\\';

        public static bool IsUnix => Path.DirectorySeparatorChar == '/';
    }
}