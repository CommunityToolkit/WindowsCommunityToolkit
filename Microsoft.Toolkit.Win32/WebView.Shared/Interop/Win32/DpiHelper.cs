// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Toolkit.Win32.UI.Controls.Interop.Win32;

namespace Microsoft.Toolkit.Win32.UI.Controls.Interop
{
    /// <summary>
    /// Class for scaling coordinates according to current DPI scaling set in Windows
    /// </summary>
    internal static class DpiHelper
    {
        // Sets DPI awareness for the process. Returns true if DPI awareness is successfully set; otherwise, false.
        public static bool SetPerMonitorDpiAwareness()
        {
            // Only works if we're on RS2 or later and have ComCtl v6
            if (OSVersionHelper.IsWindows10CreatorsOrGreater)
            {
                const int rs2AndAboveDpiFlag = NativeMethods.DPI_AWARENESS_CONTEXT_PER_MONITOR_AWARE_V2;
                return NativeMethods.SetProcessDpiAwarenessContext(rs2AndAboveDpiFlag);
            }

            return false;
        }
    }
}