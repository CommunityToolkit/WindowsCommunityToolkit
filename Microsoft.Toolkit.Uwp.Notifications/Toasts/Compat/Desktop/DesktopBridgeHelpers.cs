// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#if WIN32

using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Microsoft.Toolkit.Uwp.Notifications
{
    /// <summary>
    /// Code from https://github.com/qmatteoq/DesktopBridgeHelpers/edit/master/DesktopBridge.Helpers/Helpers.cs
    /// </summary>
    internal class DesktopBridgeHelpers
    {
        private const long APPMODEL_ERROR_NO_PACKAGE = 15700L;

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern int GetCurrentPackageFullName(ref int packageFullNameLength, StringBuilder packageFullName);

        private static bool? _isRunningAsUwp;

        public static bool IsRunningAsUwp()
        {
            if (_isRunningAsUwp == null)
            {
                if (IsWindows7OrLower)
                {
                    _isRunningAsUwp = false;
                }
                else
                {
                    int length = 0;
                    var sb = new StringBuilder(0);
                    GetCurrentPackageFullName(ref length, sb);

                    sb = new StringBuilder(length);
                    int error = GetCurrentPackageFullName(ref length, sb);

                    _isRunningAsUwp = error != APPMODEL_ERROR_NO_PACKAGE;
                }
            }

            return _isRunningAsUwp.Value;
        }

        private static bool IsWindows7OrLower
        {
            get
            {
                int versionMajor = Environment.OSVersion.Version.Major;
                int versionMinor = Environment.OSVersion.Version.Minor;
                double version = versionMajor + ((double)versionMinor / 10);
                return version <= 6.1;
            }
        }
    }
}
#endif