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

using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Security.ExchangeActiveSyncProvisioning;
using Windows.Storage;
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
        public static PackageVersion ApplicationVersion { get; }

        /// <summary>
        /// Gets the most preferred culture by the user
        /// </summary>
        public static CultureInfo Culture { get; }

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
        /// Gets used processor architecture
        /// </summary>
        public static ProcessorArchitecture OperatingSystemArchitecture { get; }

        /// <summary>
        /// Gets available memory
        /// </summary>
        public static float AvailableMemory => (float)MemoryManager.AppMemoryUsageLimit / 1024 / 1024;

        /// <summary>
        /// Gets device model
        /// </summary>
        public static string DeviceModel { get; }

        /// <summary>
        /// Gets device's manufacturer
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
        public static string FirstVersionInstalled { get; }

        /// <summary>
        /// Gets the DateTime (in UTC) that the app as first used.
        /// </summary>
        public static DateTime FirstUseTime { get; }

        /// <summary>
        /// Gets the DateTime (in UTC) that this was previously launched.
        /// Will be DateTime.MinValue if `TrackAppUse` has not been called.
        /// </summary>
        public static DateTime LastLaunchTime { get; private set; }

        /// <summary>
        /// Gets the number of times the app has been launched.
        /// Will be zero if `TrackAppUse` has not been called.
        /// </summary>
        public static long LaunchCount { get; private set; }

        /// <summary>
        /// Gets the DateTime (in UTC) that this instance of the app was launched.
        /// Will be DateTime.MinValue if `TrackAppUse` has not been called.
        /// </summary>
        public static DateTime LaunchTime { get; private set; }

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

                    ApplicationData.Current.LocalSettings.Values.TryGetValue(nameof(AppUptime), out object uptimeSoFar);

                    return new TimeSpan((long)uptimeSoFar + subsessionLength);
                }
                else
                {
                    return TimeSpan.MinValue;
                }
            }
        }

        private static DateTime _sessionStart;

        /// <summary>
        /// Track app launch information
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        public static void TrackAppUse(LaunchActivatedEventArgs args)
        {
            if (new[] { ApplicationExecutionState.ClosedByUser, ApplicationExecutionState.NotRunning }.Contains(args.PreviousExecutionState))
            {
                if (ApplicationData.Current.LocalSettings.Values.TryGetValue(nameof(LaunchCount), out object launchCount))
                {
                    LaunchCount = (long)launchCount + 1;
                }
                else
                {
                    LaunchCount = 1;
                }

                ApplicationData.Current.LocalSettings.Values[nameof(LaunchCount)] = LaunchCount;

                LaunchTime = DateTime.UtcNow;

                if (ApplicationData.Current.LocalSettings.Values.TryGetValue(nameof(LastLaunchTime), out object lastLaunch))
                {
                    LastLaunchTime = DateTime.FromFileTimeUtc((long)lastLaunch);
                }

                ApplicationData.Current.LocalSettings.Values[nameof(LastLaunchTime)] = LaunchTime.ToFileTimeUtc();

                ApplicationData.Current.LocalSettings.Values[nameof(AppUptime)] = 0L;
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

                    ApplicationData.Current.LocalSettings.Values.TryGetValue(nameof(AppUptime), out object uptimeSoFar);

                    ApplicationData.Current.LocalSettings.Values[nameof(AppUptime)] = (long)uptimeSoFar + subsessionLength;
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
            ApplicationData.Current.LocalSettings.Values.TryGetValue(nameof(AppUptime), out object uptimeSoFar);

            ApplicationData.Current.LocalSettings.Values[nameof(AppUptime)] = (long)uptimeSoFar + duration.Ticks;
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
            if (ApplicationData.Current.LocalSettings.Values.ContainsKey(nameof(IsFirstRun)))
            {
                return false;
            }
            else
            {
                ApplicationData.Current.LocalSettings.Values[nameof(IsFirstRun)] = true;
                return true;
            }
        }

        private static bool DetectIfAppUpdated()
        {
            var currentVersion = $"{ApplicationVersion.Major}.{ApplicationVersion.Minor}.{ApplicationVersion.Build}.{ApplicationVersion.Revision}";

            if (!ApplicationData.Current.LocalSettings.Values.TryGetValue(nameof(currentVersion), out object lastVersion))
            {
                ApplicationData.Current.LocalSettings.Values[nameof(currentVersion)] = currentVersion;
            }
            else
            {
                if (currentVersion != lastVersion.ToString())
                {
                    ApplicationData.Current.LocalSettings.Values[nameof(currentVersion)] = currentVersion;

                    return true;
                }
            }

            return false;
        }

        private static DateTime DetectFirstUseTime()
        {
            DateTime result;

            if (ApplicationData.Current.LocalSettings.Values.TryGetValue(nameof(FirstUseTime), out object firstUse))
            {
                result = DateTime.FromFileTimeUtc((long)firstUse);
            }
            else
            {
                result = DateTime.UtcNow;
                ApplicationData.Current.LocalSettings.Values[nameof(FirstUseTime)] = result.ToFileTimeUtc();
            }

            return result;
        }

        private static string DetectFirstVersionInstalled()
        {
            string result;

            if (ApplicationData.Current.LocalSettings.Values.TryGetValue(nameof(FirstVersionInstalled), out object firstVersion))
            {
                result = firstVersion.ToString();
            }
            else
            {
                result = $"{ApplicationVersion.Major}.{ApplicationVersion.Minor}.{ApplicationVersion.Build}.{ApplicationVersion.Revision}";
                ApplicationData.Current.LocalSettings.Values[nameof(FirstVersionInstalled)] = result;
            }

            return result;
        }

        private static void InitializeValuesSetWithTrackAppUse()
        {
            LaunchTime = DateTime.MinValue;
            LaunchCount = 0;
            LastLaunchTime = DateTime.MinValue;
        }
    }
}
