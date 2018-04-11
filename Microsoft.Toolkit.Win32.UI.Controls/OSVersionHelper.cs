// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

using Microsoft.Toolkit.Win32.UI.Controls.Interop.Win32;

using System;
using System.IO;

using Windows.Foundation.Metadata;
using Windows.Security.EnterpriseData;

namespace Microsoft.Toolkit.Win32.UI.Controls
{
    internal static class OSVersionHelper
    {
        private const string ContractName = "Windows.Foundation.UniversalApiContract";

        static OSVersionHelper()
        {
            if (IsSince(WindowsVersions.Win10))
            {
                if (IsApiContractPresent(6))
                {
                    Windows10Release = Win10Release.Redstone4;
                }
                else if (IsApiContractPresent(5))
                {
                    Windows10Release = Win10Release.FallCreators;
                }
                else if (IsApiContractPresent(4))
                {
                    Windows10Release = Win10Release.Creators;
                }
                else if (IsApiContractPresent(3))
                {
                    Windows10Release = Win10Release.Anniversary;
                }
                else if (IsApiContractPresent(2))
                {
                    Windows10Release = Win10Release.Threshold2;
                }
                else if (IsApiContractPresent(1))
                {
                    Windows10Release = Win10Release.Threshold1;
                }
                else
                {
                    Windows10Release = Win10Release.Unknown;
                }
            }
        }

        internal enum Win10Release
        {
            Unknown = 0,
            Threshold1 = 1507,   // 10240
            Threshold2 = 1511,   // 10586
            Anniversary = 1607,  // 14393 Redstone 1
            Creators = 1703,     // 15063 Redstone 2
            FallCreators = 1709, // 16299 Redstone 3
            Redstone4 = 1803     // 17133 Redstone 4
        }

        internal enum WindowsVersions
        {
            Win7,
            Server2008R2, // 6.1
            Win8,
            Server2012, // 6.2
            Win81,
            Server2012R2, // 6.3
            Win10,
            Server2016 // 10.0
        }

        public static bool EdgeExists { get; } = File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "edgehtml.dll"));

        /// <summary>
        /// Windows 10 RS4 (1803, build 17133)
        /// </summary>
        public static bool IsWindows10RS4OrGreater => Windows10Release >= Win10Release.Redstone4;

        public static bool IsWorkstation { get; } = !IsServer();

        public static bool UseWindowsInformationProtectionApi => Windows10Release >= Win10Release.Anniversary && ProtectionPolicyManager.IsProtectionEnabled;

        private static Win10Release Windows10Release { get; }

        /// <exception cref="NotSupportedException">Not running correct OS or OS Version.</exception>
        public static void ThrowIfBeforeWindows10RS4()
        {
            if (IsWindows10RS4OrGreater && IsWorkstation && EdgeExists)
            {
                return;
            }

            throw new NotSupportedException(DesignerUI.NotSup_Win10RS4);
        }

        private static bool IsApiContractPresent(ushort majorVersion) => ApiInformation.IsApiContractPresent(ContractName, majorVersion);

        private static bool IsServer()
        {
            // RtlGetVersion does not return ProductType
            var versionInfo = NativeMethods.GetVersionEx();
            return versionInfo.ProductType == 2     // VER_NT_DOMAIN_CONTROLLER
                   || versionInfo.ProductType == 3; // VER_NT_SERVER
        }

        private static bool IsSince(WindowsVersions version)
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
                    throw new ArgumentException("Unrecognized version", nameof(version));
            }

            // After 8.1 apps without manifest or are not manifested for 8.1/10 return 6.2.
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