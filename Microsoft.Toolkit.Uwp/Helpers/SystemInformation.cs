// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
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
        private static readonly LocalObjectStorageHelper _localObjectStorageHelper = new LocalObjectStorageHelper();
        private static DateTime _sessionStart;

        /// <summary>
        /// Gets Application's name
        /// </summary>
        public static string ApplicationName { get; }

        /// <summary>
        /// Gets Application's version
        /// </summary>
        public static PackageVersion ApplicationVersion { get; }

        /// <summary>
        /// Gets the most preferred culture by the user
        /// </summary>
        public static CultureInfo Culture { get; }

        /// <summary>
        /// Gets device's family
        /// <para></para>
        /// Common values include:
        /// <list type="bullet">
        /// <item>
        /// <term>"Windows.Desktop"</term>
        /// </item>
        /// <item>
        /// <term>"Windows.Mobile"</term>
        /// </item>
        /// <item>
        /// <term>"Windows.Xbox"</term>
        /// </item>
        /// <item>
        /// <term>"Windows.Holographic"</term>
        /// </item>
        /// <item>
        /// <term>"Windows.Team"</term>
        /// </item>
        /// <item>
        /// <term>"Windows.IoT"</term>
        /// </item>
        /// </list>
        /// <para></para>
        /// Prepare your code for other values.
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
        /// Gets used processor architecture
        /// </summary>
        public static ProcessorArchitecture OperatingSystemArchitecture { get; }

        /// <summary>
        /// Gets available memory
        /// </summary>
        public static float AvailableMemory => (float)MemoryManager.AppMemoryUsageLimit / 1024 / 1024;

        /// <summary>
        /// Gets device model.
        /// Will be empty if the device model couldn't be determined (ex: when running in a virtual machine).
        /// </summary>
        public static string DeviceModel { get; }

        /// <summary>
        /// Gets device's manufacturer.
        /// Will be empty if the device manufacturer couldn't be determined (ex: when running in a virtual machine).
        /// </summary>
        public static string DeviceManufacturer { get; }

        /// <summary>
        /// Gets a value indicating whether the app is being used for the first time since it was installed.
        /// Use this to tell if you should do or display something different for the app's first use.
        /// </summary>
        public static bool IsFirstRun { get; }

        /// <summary>
        /// Gets a value indicating whether the app is being used for the first time since being upgraded from an older version.
        /// Use this to tell if you should display details about what has changed.
        /// </summary>
        public static bool IsAppUpdated { get; }

        /// <summary>
        /// Gets the first version of the app that was installed.
        /// This will be the current version if a previous verison of the app was installed before accessing this property.
        /// </summary>
        public static PackageVersion FirstVersionInstalled { get; }

        /// <summary>
        /// Gets the DateTime (in UTC) when the app was launched for the first time
        /// </summary>
        public static DateTime FirstUseTime { get; }

        /// <summary>
        /// Gets the DateTime (in UTC) when the app was previously launched, not including this instance.
        /// Will be DateTime.MinValue if `TrackAppUse` has not been called.
        /// </summary>
        public static DateTime LastLaunchTime { get; private set; }

        /// <summary>
        /// Gets the number of times the app has been launched since the last reset.
        /// Will be zero if `TrackAppUse` has not been called.
        /// </summary>
        public static long LaunchCount { get; private set; }

        /// <summary>
        /// Gets the number of times the app has been launched.
        /// Will be zero if `TrackAppUse` has not been called.
        /// </summary>
        public static long TotalLaunchCount { get; private set; }

        /// <summary>
        /// Gets the DateTime (in UTC) that this instance of the app was launched.
        /// Will be DateTime.MinValue if `TrackAppUse` has not been called.
        /// </summary>
        public static DateTime LaunchTime { get; private set; }

        /// <summary>
        /// Gets the DateTime (in UTC) when the launch count was previously reset, not including this instance.
        /// Will be DateTime.MinValue if `TrackAppUse` has not been called.
        /// </summary>
        public static DateTime LastResetTime { get; private set; }

        /// <summary>
        /// Gets the length of time this instance of the app has been running.
        /// Will be TimeSpan.MinValue if `TrackAppUse` has not been called.
        /// </summary>
        public static TimeSpan AppUptime
        {
            get
            {
                if (LaunchCount > 0)
                {
                    var subsessionLength = DateTime.UtcNow.Subtract(_sessionStart).Ticks;

                    var uptimeSoFar = _localObjectStorageHelper.Read<long>(nameof(AppUptime));

                    return new TimeSpan(uptimeSoFar + subsessionLength);
                }
                else
                {
                    return TimeSpan.MinValue;
                }
            }
        }

        /// <summary>
        /// Track app launch information
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        public static void TrackAppUse(LaunchActivatedEventArgs args)
        {
            if (args.PreviousExecutionState == ApplicationExecutionState.ClosedByUser
             || args.PreviousExecutionState == ApplicationExecutionState.NotRunning)
            {
                LaunchCount = _localObjectStorageHelper.Read<long>(nameof(LaunchCount)) + 1;
                TotalLaunchCount = _localObjectStorageHelper.Read<long>(nameof(TotalLaunchCount)) + 1;

                // In case we upgraded the properties, make TotalLaunchCount is correct
                if (TotalLaunchCount < LaunchCount)
                {
                    TotalLaunchCount = LaunchCount;
                }

                _localObjectStorageHelper.Save(nameof(LaunchCount), LaunchCount);
                _localObjectStorageHelper.Save(nameof(TotalLaunchCount), TotalLaunchCount);

                LaunchTime = DateTime.UtcNow;

                var lastLaunch = _localObjectStorageHelper.Read<long>(nameof(LastLaunchTime));
                LastLaunchTime = lastLaunch != default(long)
                    ? DateTime.FromFileTimeUtc(lastLaunch)
                    : LaunchTime;

                _localObjectStorageHelper.Save(nameof(LastLaunchTime), LaunchTime.ToFileTimeUtc());
                _localObjectStorageHelper.Save(nameof(AppUptime), 0L);

                var lastResetTime = _localObjectStorageHelper.Read<long>(nameof(LastResetTime));
                LastResetTime = lastResetTime != default(long)
                    ? DateTime.FromFileTimeUtc(lastResetTime)
                    : DateTime.MinValue;
            }

            void App_VisibilityChanged(Windows.UI.Core.CoreWindow sender, Windows.UI.Core.VisibilityChangedEventArgs e)
            {
                if (e.Visible)
                {
                    _sessionStart = DateTime.UtcNow;
                }
                else
                {
                    var subsessionLength = DateTime.UtcNow.Subtract(_sessionStart).Ticks;

                    var uptimeSoFar = _localObjectStorageHelper.Read<long>(nameof(AppUptime));

                    _localObjectStorageHelper.Save(nameof(AppUptime), uptimeSoFar + subsessionLength);
                }
            }

            Windows.UI.Core.CoreWindow.GetForCurrentThread().VisibilityChanged -= App_VisibilityChanged;
            Windows.UI.Core.CoreWindow.GetForCurrentThread().VisibilityChanged += App_VisibilityChanged;
        }

        /// <summary>
        /// Launch the store app so the user can leave a review
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation</returns>
        public static async Task LaunchStoreForReviewAsync()
        {
            await Launcher.LaunchUriAsync(new Uri(string.Format("ms-windows-store://review/?PFN={0}", Package.Current.Id.FamilyName)));
        }

        /// <summary>
        /// Add to the record of how long the app has been running.
        /// Use this to optionally include time spent in background tasks or extended execution.
        /// </summary>
        /// <param name="duration">The amount to time to add</param>
        public static void AddToAppUptime(TimeSpan duration)
        {
            var uptimeSoFar = _localObjectStorageHelper.Read<long>(nameof(AppUptime));
            _localObjectStorageHelper.Save(nameof(AppUptime), uptimeSoFar + duration.Ticks);
        }

        /// <summary>
        /// Reset launch count
        /// </summary>
        public static void ResetLaunchCount()
        {
            LastResetTime = DateTime.UtcNow;
            LaunchCount = 0;

            _localObjectStorageHelper.Save(nameof(LastResetTime), LastResetTime.ToFileTimeUtc());
            _localObjectStorageHelper.Save(nameof(LaunchCount), LaunchCount);
        }

        /// <summary>
        /// Initializes static members of the <see cref="SystemInformation"/> class.
        /// </summary>
        static SystemInformation()
        {
            ApplicationName = Package.Current.DisplayName;
            ApplicationVersion = Package.Current.Id.Version;
            try
            {
                Culture = GlobalizationPreferences.Languages.Count > 0 ? new CultureInfo(GlobalizationPreferences.Languages.First()) : null;
            }
            catch
            {
                Culture = null;
            }

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
            IsFirstRun = DetectIfFirstUse();
            IsAppUpdated = DetectIfAppUpdated();
            FirstUseTime = DetectFirstUseTime();
            FirstVersionInstalled = DetectFirstVersionInstalled();
            InitializeValuesSetWithTrackAppUse();
        }

        private static bool DetectIfFirstUse()
        {
            if (_localObjectStorageHelper.KeyExists(nameof(IsFirstRun)))
            {
                return false;
            }
            else
            {
                _localObjectStorageHelper.Save(nameof(IsFirstRun), true);
                return true;
            }
        }

        private static bool DetectIfAppUpdated()
        {
            var currentVersion = ApplicationVersion.ToFormattedString();

            if (!_localObjectStorageHelper.KeyExists(nameof(currentVersion)))
            {
                _localObjectStorageHelper.Save(nameof(currentVersion), currentVersion);
            }
            else
            {
                var lastVersion = _localObjectStorageHelper.Read<string>(nameof(currentVersion));
                if (currentVersion != lastVersion)
                {
                    _localObjectStorageHelper.Save(nameof(currentVersion), currentVersion);
                    return true;
                }
            }

            return false;
        }

        private static DateTime DetectFirstUseTime()
        {
            DateTime result;

            if (_localObjectStorageHelper.KeyExists(nameof(FirstUseTime)))
            {
                var firstUse = _localObjectStorageHelper.Read<long>(nameof(FirstUseTime));
                result = DateTime.FromFileTimeUtc(firstUse);
            }
            else
            {
                result = DateTime.UtcNow;
                _localObjectStorageHelper.Save(nameof(FirstUseTime), result.ToFileTimeUtc());
            }

            return result;
        }

        private static PackageVersion DetectFirstVersionInstalled()
        {
            PackageVersion result;

            if (_localObjectStorageHelper.KeyExists(nameof(FirstVersionInstalled)))
            {
                result = _localObjectStorageHelper.Read<string>(nameof(FirstVersionInstalled)).ToPackageVersion();
            }
            else
            {
                result = ApplicationVersion;
                _localObjectStorageHelper.Save(nameof(FirstVersionInstalled), ApplicationVersion.ToFormattedString());
            }

            return result;
        }

        private static void InitializeValuesSetWithTrackAppUse()
        {
            LaunchTime = DateTime.MinValue;
            LaunchCount = 0;
            TotalLaunchCount = 0;
            LastLaunchTime = DateTime.MinValue;
            LastResetTime = DateTime.MinValue;
        }
    }
}
