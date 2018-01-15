﻿// ******************************************************************
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

using System.Globalization;
using Microsoft.Toolkit.Uwp.Helpers;
using Windows.System;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SystemInformationPage : Page
    {
        // To get application's name:
        public string ApplicationName => SystemInformation.ApplicationName;

        // To get application's version:
        public string ApplicationVersion => SystemInformation.ApplicationVersion.ToFormattedString();

        // To get the most preferred language by the user:
        public CultureInfo Culture => SystemInformation.Culture;

        // To get operating syste,
        public string OperatingSystem => SystemInformation.OperatingSystem;

        // To get used processor architecture
        public ProcessorArchitecture OperatingSystemArchitecture => SystemInformation.OperatingSystemArchitecture;

        // To get operating system version
        public OSVersion OperatingSystemVersion => SystemInformation.OperatingSystemVersion;

        // To get device family
        public string DeviceFamily => SystemInformation.DeviceFamily;

        // To get device model
        public string DeviceModel => SystemInformation.DeviceModel;

        // To get device manufacturer
        public string DeviceManufacturer => SystemInformation.DeviceManufacturer;

        // To get available memory in MB
        public float AvailableMemory => SystemInformation.AvailableMemory;

        // To get if the app is being used for the first time since it was installed.
        public string IsFirstUse => SystemInformation.IsFirstRun.ToString();

        // To get if the app is being used for the first time since being upgraded from an older version.
        public string IsAppUpdated => SystemInformation.IsAppUpdated.ToString();

        // To get the first version installed
        public string FirstVersionInstalled => SystemInformation.FirstVersionInstalled.ToFormattedString();

        // To get the first time the app was launched
        public string FirstUseTime => SystemInformation.FirstUseTime.ToString(Culture.DateTimeFormat);

        // To get the time the app was launched
        public string LaunchTime => SystemInformation.LaunchTime.ToString(Culture.DateTimeFormat);

        // To get the time the app was previously launched, not including this instance
        public string LastLaunchTime => SystemInformation.LastLaunchTime.ToString(Culture.DateTimeFormat);

        // To get the number of times the app has been launched.
        public long LaunchCount => SystemInformation.LaunchCount;

        // To get how long the app has been running
        public string AppUptime => SystemInformation.AppUptime.ToString();

        public SystemInformationPage()
        {
            InitializeComponent();
        }
    }
}
