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
using Windows.UI.Xaml;

namespace Microsoft.Toolkit.Uwp.Helpers
{
    /// <summary>
    /// This class provides info about the app and the system.
    /// </summary>
    public sealed class SystemInformation
    {
        private readonly LocalObjectStorageHelper _localObjectStorageHelper = new LocalObjectStorageHelper();
        private DateTime _sessionStart;

        /// <summary>
        /// Launches the store app so the user can leave a review.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <remarks>This method needs to be called from your UI thread.</remarks>
        public static async Task LaunchStoreForReviewAsync()
        {
            await Launcher.LaunchUriAsync(new Uri(string.Format("ms-windows-store://review/?PFN={0}", Package.Current.Id.FamilyName)));
        }

        /// <summary>
        /// Gets the unique instance of <see cref="SystemInformation"/>.
        /// </summary>
        public static SystemInformation Instance { get; } = new SystemInformation();

        /// <summary>
        /// Gets the application's name.
        /// </summary>
        public string ApplicationName { get; }

        /// <summary>
        /// Gets the application's version.
        /// </summary>
        public PackageVersion ApplicationVersion { get; }

        /// <summary>
        /// Gets the user's most preferred culture.
        /// </summary>
        public CultureInfo Culture { get; }

        /// <summary>
        /// Gets the device's family.
        /// <para></para>
        /// Common values include:
        /// <list type="bullet">
        /// <item><term>"Windows.Desktop"</term></item>
        /// <item><term>"Windows.Mobile"</term></item>
        /// <item><term>"Windows.Xbox"</term></item>
        /// <item><term>"Windows.Holographic"</term></item>
        /// <item><term>"Windows.Team"</term></item>
        /// <item><term>"Windows.IoT"</term></item>
        /// </list>
        /// <para></para>
        /// Prepare your code for other values.
        /// </summary>
        public string DeviceFamily { get; }

        /// <summary>
        /// Gets the operating system's name.
        /// </summary>
        public string OperatingSystem { get; }

        /// <summary>
        /// Gets the operating system's version.
        /// </summary>
        public OSVersion OperatingSystemVersion { get; }

        /// <summary>
        /// Gets the processor architecture.
        /// </summary>
        public ProcessorArchitecture OperatingSystemArchitecture { get; }

        /// <summary>
        /// Gets the available memory.
        /// </summary>
        public float AvailableMemory => (float)MemoryManager.AppMemoryUsageLimit / 1024 / 1024;

        /// <summary>
        /// Gets the device's model.
        /// Will be empty if the model couldn't be determined (For example: when running in a virtual machine).
        /// </summary>
        public string DeviceModel { get; }

        /// <summary>
        /// Gets the device's manufacturer.
        /// Will be empty if the manufacturer couldn't be determined (For example: when running in a virtual machine).
        /// </summary>
        public string DeviceManufacturer { get; }

        /// <summary>
        /// Gets a value indicating whether the app is being used for the first time since it was installed.
        /// Use this to tell if you should do or display something different for the app's first use.
        /// </summary>
        public bool IsFirstRun { get; }

        /// <summary>
        /// Gets a value indicating whether the app is being used for the first time since being upgraded from an older version.
        /// Use this to tell if you should display details about what has changed.
        /// </summary>
        public bool IsAppUpdated { get; }

        /// <summary>
        /// Gets the first version of the app that was installed.
        /// This will be the current version if a previous verison of the app was installed before accessing this property.
        /// </summary>
        public PackageVersion FirstVersionInstalled { get; }

        /// <summary>
        /// Gets the DateTime (in UTC) when the app was launched for the first time.
        /// </summary>
        public DateTime FirstUseTime { get; }

        /// <summary>
        /// Gets the DateTime (in UTC) when the app was last launched, not including this instance.
        /// Will be <see cref="DateTime.MinValue"/> if <see cref="TrackAppUse"/> has not been called yet.
        /// </summary>
        public DateTime LastLaunchTime { get; private set; }

        /// <summary>
        /// Gets the number of times the app has been launched.
        /// Will be <c>0</c> if <see cref="TrackAppUse"/> has not been called yet.
        /// </summary>
        public long LaunchCount { get; private set; }

        /// <summary>
        /// Gets the number of times the app has been launched.
        /// Will be <c>0</c> if <see cref="TrackAppUse"/> has not been called yet.
        /// </summary>
        public long TotalLaunchCount { get; private set; }

        /// <summary>
        /// Gets the DateTime (in UTC) that this instance of the app was launched.
        /// Will be <see cref="DateTime.MinValue"/> if <see cref="TrackAppUse"/> has not been called yet.
        /// </summary>
        public DateTime LaunchTime { get; private set; }

        /// <summary>
        /// Gets the DateTime (in UTC) when the launch count was last reset.
        /// Will be <see cref="DateTime.MinValue"/> if <see cref="TrackAppUse"/> has not been called yet.
        /// </summary>
        public DateTime LastResetTime { get; private set; }

        /// <summary>
        /// Gets the length of time this instance of the app has been running.
        /// Will be <see cref="TimeSpan.MinValue"/> if <see cref="TrackAppUse"/> has not been called yet.
        /// </summary>
        public TimeSpan AppUptime
        {
            get
            {
                if (LaunchCount > 0)
                {
                    var subsessionLength = DateTime.UtcNow.Subtract(_sessionStart).Ticks;

                    var uptimeSoFar = _localObjectStorageHelper.Read<long>(nameof(AppUptime));

                    return new TimeSpan(uptimeSoFar + subsessionLength);
                }

                return TimeSpan.MinValue;
            }
        }

        /// <summary>
        /// Tracks information about the app's launch.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        /// <param name="xamlRoot">The XamlRoot object from your visual tree.</param>
        public void TrackAppUse(IActivatedEventArgs args, XamlRoot xamlRoot = null)
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

            if (xamlRoot != null)
            {
                void XamlRoot_Changed(XamlRoot sender, XamlRootChangedEventArgs e)
                {
                    UpdateVisibility(sender.IsHostVisible);
                }

                xamlRoot.Changed -= XamlRoot_Changed;
                xamlRoot.Changed += XamlRoot_Changed;
            }
            else
            {
                void App_VisibilityChanged(Windows.UI.Core.CoreWindow sender, Windows.UI.Core.VisibilityChangedEventArgs e)
                {
                    UpdateVisibility(e.Visible);
                }

                Windows.UI.Core.CoreWindow.GetForCurrentThread().VisibilityChanged -= App_VisibilityChanged;
                Windows.UI.Core.CoreWindow.GetForCurrentThread().VisibilityChanged += App_VisibilityChanged;
            }
        }

        private void UpdateVisibility(bool visible)
        {
            if (visible)
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

        /// <summary>
        /// Adds to the record of how long the app has been running.
        /// Use this to optionally include time spent in background tasks or extended execution.
        /// </summary>
        /// <param name="duration">The amount to time to add</param>
        public void AddToAppUptime(TimeSpan duration)
        {
            var uptimeSoFar = _localObjectStorageHelper.Read<long>(nameof(AppUptime));
            _localObjectStorageHelper.Save(nameof(AppUptime), uptimeSoFar + duration.Ticks);
        }

        /// <summary>
        /// Resets the launch count.
        /// </summary>
        public void ResetLaunchCount()
        {
            LastResetTime = DateTime.UtcNow;
            LaunchCount = 0;

            _localObjectStorageHelper.Save(nameof(LastResetTime), LastResetTime.ToFileTimeUtc());
            _localObjectStorageHelper.Save(nameof(LaunchCount), LaunchCount);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SystemInformation"/> class.
        /// </summary>
        private SystemInformation()
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

        private bool DetectIfFirstUse()
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

        private bool DetectIfAppUpdated()
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

        private DateTime DetectFirstUseTime()
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

        private PackageVersion DetectFirstVersionInstalled()
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

        private void InitializeValuesSetWithTrackAppUse()
        {
            LaunchTime = DateTime.MinValue;
            LaunchCount = 0;
            TotalLaunchCount = 0;
            LastLaunchTime = DateTime.MinValue;
            LastResetTime = DateTime.MinValue;
        }
    }
}
