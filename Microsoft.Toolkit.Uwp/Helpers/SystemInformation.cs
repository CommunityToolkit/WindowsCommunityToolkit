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

namespace Microsoft.Toolkit.Uwp.Helpers
{
    /// <summary>
    /// Defines class providing information of OS and application
    /// </summary>
    public class SystemInformation
    {
        /// <summary>
        /// Gets Application's name
        /// </summary>
        public string ApplicationName { get; private set; }

        /// <summary>
        /// Gets Application's version
        /// </summary>
        public PackageVersion ApplicationVersion { get; private set; }

        /// <summary>
        /// Gets current culture
        /// </summary>
        public string Culture { get; private set; }

        /// <summary>
        /// Gets device's family
        /// </summary>
        public string DeviceFamily { get; private set; }

        /// <summary>
        /// Gets operating system
        /// </summary>
        public string OperatingSystem { get; private set; }

        /// <summary>
        /// Gets operating system version
        /// </summary>
        public OSVersion OperatingSystemVersion { get; private set; }

        /// <summary>
        /// Gets architecture of the processor
        /// </summary>
        public ProcessorArchitecture OperatingSystemArchitecture { get; private set; }

        /// <summary>
        /// Gets available memory
        /// </summary>
        public float AvailableMemory { get; private set; }

        /// <summary>
        /// Gets device model
        /// </summary>
        public string DeviceModel { get; private set; }

        /// <summary>
        /// Gets device's manufacturer
        /// </summary>
        public string DeviceManufacturer { get; private set; }

        private SystemInformation()
        {
            ApplicationName = Package.Current.DisplayName;
            ApplicationVersion = Package.Current.Id.Version;
            Culture = Windows.System.UserProfile.GlobalizationPreferences.Languages.FirstOrDefault() ?? string.Empty;
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
            AvailableMemory = (float)MemoryManager.AppMemoryUsageLimit / 1024 / 1024;
        }

        /// <summary>
        /// Method responsible for getting current system information
        /// </summary>
        /// <returns>current system information</returns>
        public static SystemInformation GetCurrent() => new SystemInformation();
    }
}
