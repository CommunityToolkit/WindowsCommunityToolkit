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

using System.Linq;
using Windows.ApplicationModel;
using Windows.Security.ExchangeActiveSyncProvisioning;
using Windows.System;
using Windows.System.Profile;
using Windows.System.UserProfile;

namespace Microsoft.Toolkit.Uwp.Helpers
{
    /// <summary>
    /// Defines class providing information of OS and application
    /// </summary>
    public static class SystemInformation
    {
        /// <summary>
        /// Gets Application's name
        /// </summary>
        public static string ApplicationName { get; }

        /// <summary>
        /// Gets Application's version
        /// </summary>
        public static PackageVersion ApplicationVersion { get;  }

        /// <summary>
        /// Gets current culture
        /// </summary>
        public static string Culture { get; }

        /// <summary>
        /// Gets device's family
        /// </summary>
        public static string DeviceFamily { get; }

        /// <summary>
        /// Gets operating system
        /// </summary>
        public static string OperatingSystem { get; }

        /// <summary>
        /// Gets operating system version
        /// </summary>
        public static OSVersion OperatingSystemVersion { get; }

        /// <summary>
        /// Gets architecture of the processor
        /// </summary>
        public static ProcessorArchitecture OperatingSystemArchitecture { get; }

        /// <summary>
        /// Gets available memory
        /// </summary>
        public static float AvailableMemory => (float)MemoryManager.AppMemoryUsageLimit / 1024 / 1024;

        /// <summary>
        /// Gets device model
        /// </summary>
        public static string DeviceModel { get;  }

        /// <summary>
        /// Gets device's manufacturer
        /// </summary>
        public static string DeviceManufacturer { get; }

        /// <summary>
        /// Initializes static members of the <see cref="SystemInformation"/> class.
        /// </summary>
        static SystemInformation()
        {
            ApplicationName = Package.Current.DisplayName;
            ApplicationVersion = Package.Current.Id.Version;
            Culture = GlobalizationPreferences.Languages.FirstOrDefault() ?? string.Empty;
            DeviceFamily = AnalyticsInfo.VersionInfo.DeviceFamily;
            ulong version = ulong.Parse(AnalyticsInfo.VersionInfo.DeviceFamilyVersion);
            OperatingSystemVersion = new OSVersion
            {
                Major = (ushort)((version & 0xFFFF000000000000L) >> 48),
                Minor = (ushort)((version & 0x0000FFFF00000000L) >> 32),
                Build = (ushort)((version & 0x00000000FFFF0000L) >> 16),
                Revision = (ushort)(version & 0x000000000000FFFFL)
            };
            OperatingSystemArchitecture = Package.Current.Id.Architecture;
            EasClientDeviceInformation deviceInfo = new EasClientDeviceInformation();
            OperatingSystem = deviceInfo.OperatingSystem;
            DeviceManufacturer = deviceInfo.SystemManufacturer;
            DeviceModel = deviceInfo.SystemProductName;
        }
    }
}
