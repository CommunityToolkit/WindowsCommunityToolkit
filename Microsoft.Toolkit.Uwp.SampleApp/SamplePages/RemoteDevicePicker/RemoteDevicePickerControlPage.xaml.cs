// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using Microsoft.Toolkit.Uwp.UI.Controls;
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
                SecondaryButtonText = "Select",
                SelectionMode = bool.Parse(SelectionCheckBox.IsChecked.ToString()) ? ListViewSelectionMode.Extended : ListViewSelectionMode.Single
            };
            var remoteSystems = await remoteDevicePicker.PickDeviceAsync();
            MyInAppNotification.Show($"You picked {remoteSystems.Count().ToString()} Device(s)" + Environment.NewLine + string.Join(",", remoteSystems.Select(x => x.DisplayName.ToString()).ToList()), 2000);
        }
    }
}