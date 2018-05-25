// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using System.Security;
using Microsoft.Toolkit.Win32.UI.Controls.Interop.Win32;
using Windows.Foundation.Metadata;
using Windows.Security.EnterpriseData;

namespace Microsoft.Toolkit.Win32.UI.Controls.Interop
{
    internal static class OSVersionHelper
    {
        private const string ContractName = "Windows.Foundation.UniversalApiContract";

        [SecurityCritical]
        static OSVersionHelper()
        {
            if (IsSince(WindowsVersions.Win10))
            {
                if (IsApiContractPresent(6))
                {
                    Windows10Release = Windows10Release.April2018;
                }
                else if (IsApiContractPresent(5))
                {
                    Windows10Release = Windows10Release.FallCreators;
                }
                else if (IsApiContractPresent(4))
                {
                    Windows10Release = Windows10Release.Creators;
                }
                else if (IsApiContractPresent(3))
                {
                    Windows10Release = Windows10Release.Anniversary;
                }
                else if (IsApiContractPresent(2))
                {
                    Windows10Release = Windows10Release.Threshold2;
                }
                else if (IsApiContractPresent(1))
                {
                    Windows10Release = Windows10Release.Threshold1;
                }
                else
                {
                    Windows10Release = Windows10Release.Unknown;
                }
            }
        }

        internal static bool IsWindowsNt { get; } = Environment.OSVersion.Platform == PlatformID.Win32NT;

        internal static bool EdgeExists { get; } = File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), ExternDll.EdgeHtml));

        internal static bool IsWindows10 { get; } = IsWindowsNt && IsSince(WindowsVersions.Win10);

        /// <summary>
        /// Gets a value indicating whether the current OS is Windows 10 April 2018 Update (Redstone 4) or greater
        /// </summary>
        internal static bool IsWindows10April2018OrGreater => IsWindows10 && Windows10Release >= Windows10Release.April2018;

        /// <summary>
        /// Gets a value indicating whether the current OS is Windows 10 Fall Creators Update (Redstone 3) or greater
        /// </summary>
        internal static bool IsWindows10FallCreatorsOrGreater => IsWindows10 && Windows10Release >= Windows10Release.FallCreators;

        /// <summary>
        /// Gets a value indicating whether the current OS is Windows 10 Creators Update (Redstone 2) or greater
        /// </summary>
        internal static bool IsWindows10CreatorsOrGreater => IsWindows10 && Windows10Release >= Windows10Release.Creators;

        /// <summary>
        /// Gets a value indicating whether the current OS is Windows 10 Anniversary Update (Redstone 1) or greater
        /// </summary>
        internal static bool IsWindows10AnniversaryOrGreater => IsWindows10 && Windows10Release >= Windows10Release.Anniversary;

        /// <summary>
        /// Gets a value indicating whether the current OS is Windows 10 Threshold 2 or greater
        /// </summary>
        internal static bool IsWindows10Threshold2OrGreater => IsWindows10 && Windows10Release >= Windows10Release.Threshold2;

        /// <summary>
        /// Gets a value indicating whether the current OS is Windows 10 Threshold 1 or greater
        /// </summary>
        internal static bool IsWindows10Threshold1OrGreater => IsWindows10 && Windows10Release >= Windows10Release.Threshold1;

        internal static bool IsWorkstation { get; } = !IsServer();

        internal static bool UseWindowsInformationProtectionApi
        {
            [SecurityCritical]
            get => Windows10Release >= Windows10Release.Anniversary && ProtectionPolicyManager.IsProtectionEnabled;
        }

        internal static Windows10Release Windows10Release { get; }

        /// <summary>
        /// Checks if OS is Windows 10 April 2018 or later, is a workstation, and Microsoft Edge exists.
        /// </summary>
        /// <exception cref="NotSupportedException">Not running correct OS or OS Version, or Microsoft Edge does not exist.</exception>
        internal static void ThrowIfBeforeWindows10April2018()
        {
            if (IsWindows10April2018OrGreater && IsWorkstation && EdgeExists)
            {
                return;
            }

            throw new NotSupportedException(DesignerUI.E_NOTSUPPORTED_OS_RS4);
        }

        [SecurityCritical]
        private static bool IsApiContractPresent(ushort majorVersion) => ApiInformation.IsApiContractPresent(ContractName, majorVersion);

        [SecurityCritical]
        private static bool IsServer()
        {
            var versionInfo = NativeMethods.RtlGetVersion();
            return versionInfo.ProductType == ProductType.VER_NT_DOMAIN_CONTROLLER
                   || versionInfo.ProductType == ProductType.VER_NT_SERVER;
        }

        [SecurityCritical]
        internal static bool IsSince(WindowsVersions version)
        {
            int major;
            int minor;

            switch (version)
            {
                case WindowsVersions.Win7:
                case WindowsVersions.Server2008R2:
                    major = 6;
                    minor = 1;
                    break;

                case WindowsVersions.Win8:
                case WindowsVersions.Server2012:
                    major = 6;
                    minor = 2;
                    break;

                case WindowsVersions.Win81:
                case WindowsVersions.Server2012R2:
                    major = 6;
                    minor = 3;
                    break;

                case WindowsVersions.Win10:
                case WindowsVersions.Server2016:
                    major = 10;
                    minor = 0;
                    break;

                default:
                    throw new ArgumentException(DesignerUI.E_UNRECOGNIZED_OS, nameof(version));
            }

            // After 8.1 apps without manifest or are not manifested for 8.1/10 return 6.2 when using GetVersionEx.
            // Need to use RtlGetVersion to get correct major/minor/build
            var os = NativeMethods.RtlGetVersion();

            if (os.MajorVersion > major)
            {
                return true;
            }

            if (os.MajorVersion == major)
            {
                return os.MinorVersion >= minor;
            }

            return false;
        }
    }
}