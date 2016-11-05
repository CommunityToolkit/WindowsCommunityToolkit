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
        public string ApplicationVersion => $"{SystemInformation.ApplicationVersion.Major}.{SystemInformation.ApplicationVersion.Minor}.{SystemInformation.ApplicationVersion.Build}.{SystemInformation.ApplicationVersion.Revision}";

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

        public SystemInformationPage()
        {
            InitializeComponent();
        }
    }
}
