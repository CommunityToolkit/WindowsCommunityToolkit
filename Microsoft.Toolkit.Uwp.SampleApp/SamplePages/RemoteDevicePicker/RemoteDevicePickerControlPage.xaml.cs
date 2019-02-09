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
            var selectionEnum = Enum.GetNames(typeof(RemoteDeviceSelectionMode)).Cast<string>().ToList();
            MyComboBox.ItemsSource = selectionEnum;
            MyComboBox.SelectedIndex = 1;
        }

        private async void Button_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var remoteDevicePicker = new RemoteDevicePicker()
            {
                Title = "Pick Remote Device",
                SelectionMode = (RemoteDeviceSelectionMode)Enum.Parse(typeof(RemoteDeviceSelectionMode), MyComboBox.SelectedValue.ToString()),
                ShowAdvancedFilters = ShowAdvancedFilters.IsChecked.Value
            };
            var remoteSystems = await remoteDevicePicker.PickDeviceAsync();
            MyInAppNotification.Show($"You picked {remoteSystems.Count().ToString()} Device(s)" + Environment.NewLine + string.Join(",", remoteSystems.Select(x => x.DisplayName.ToString()).ToList()), 2000);
        }
    }
}