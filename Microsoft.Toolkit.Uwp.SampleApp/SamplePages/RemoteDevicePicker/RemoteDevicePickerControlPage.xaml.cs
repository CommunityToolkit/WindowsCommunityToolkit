// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Toolkit.Uwp.UI.Controls;
using System;
using System.Linq;
using Windows.System;
using Windows.System.RemoteSystems;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    public sealed partial class RemoteDevicePickerControlPage : Page
    {
        public RemoteDevicePickerControlPage()
        {
            this.InitializeComponent();
        }

        private async void Button_Tapped(object sender, TappedRoutedEventArgs e)
        {
            RemoteDevicePicker remoteDevicePicker = new RemoteDevicePicker()
            {
                Title = "Pick Remote Device",
                DeviceListSelectionMode = ListViewSelectionMode.Single
            };
            remoteDevicePicker.RemoteDevicePickerClosed += RemoteDevicePicker_RemoteDevicePickerClosed;
            await remoteDevicePicker.ShowAsync();
        }

        private async void RemoteDevicePicker_RemoteDevicePickerClosed(object sender, RemoteDevicePickerEventArgs e)
        {
            await new MessageDialog($"You picked {e.Devices.Count.ToString()} Device(s)" + Environment.NewLine + string.Join(",", e.Devices.Select(x => x.DisplayName.ToString()).ToList())).ShowAsync();
        }
    }
}
