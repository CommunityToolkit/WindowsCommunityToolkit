// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Windows.System.RemoteSystems;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    public sealed partial class RemoteDevicePickerControlPage : Page
    {
        public RemoteDevicePickerControlPage()
        {
            InitializeComponent();
        }

        private async void Button_Tapped(object sender, TappedRoutedEventArgs e)
        {
            RemoteDevicePicker remoteDevicePicker = new RemoteDevicePicker()
            {
                Title = "Pick Remote Device",
                SelectionMode = ListViewSelectionMode.Extended
            };
            var result = await remoteDevicePicker.PickDeviceAsync();
            await new MessageDialog($"You picked {result.Count().ToString()} Device(s)" + Environment.NewLine + string.Join(",", result.Select(x => x.DisplayName.ToString()).ToList())).ShowAsync();
        }
    }
}