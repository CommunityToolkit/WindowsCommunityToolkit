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

using System;
using Microsoft.Toolkit.Uwp.Connectivity;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    public sealed partial class BluetoothLEHelperPage : Page
    {
        private BluetoothLEHelper bluetoothLEHelper = BluetoothLEHelper.Context;

        public BluetoothLEHelperPage()
        {
            this.InitializeComponent();
            bluetoothLEHelper.EnumerationCompleted += BluetoothLEHelper_EnumerationCompleted;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (BluetoothLEHelper.IsBluetoothLESupported)
            {
                MainContent.Visibility = Visibility.Visible;
            }
            else
            {
                NotAvailableMessage.Visibility = Visibility.Visible;
            }
        }

        private async void BluetoothLEHelper_EnumerationCompleted(object sender, EventArgs e)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                bluetoothLEHelper.StopEnumeration();
                BtEnumeration.Content = "Start Enumerating";
            });
        }

        private void Enumeration_Click(object sender, RoutedEventArgs e)
        {
            if (!BluetoothLEHelper.IsBluetoothLESupported)
            {
                return;
            }

            if (!bluetoothLEHelper.IsEnumerating)
            {
                bluetoothLEHelper.StartEnumeration();
                BtEnumeration.Content = "Stop Enumerating";
            }
            else
            {
                bluetoothLEHelper.StopEnumeration();
                BtEnumeration.Content = "Start Enumerating";
            }
        }

        private async void LVDevices_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TbDeviceName.Text = "No device selected";
            TbDeviceBtAddr.Text = "No device selected";

            CBServices.Visibility = Visibility.Collapsed;
            CBServices.ItemsSource = null;

            if (e.AddedItems.Count > 0)
            {
                ObservableBluetoothLEDevice device = e.AddedItems[0] as ObservableBluetoothLEDevice;

                if (device != null)
                {
                    TbDeviceName.Text = "Device Name: " + device.Name;
                    TbDeviceBtAddr.Text = "Device Address: " + device.BluetoothAddressAsString;

                    // Make sure the Bluetooth capability is set else this will fail
                    bluetoothLEHelper.StopEnumeration();
                    await device.ConnectAsync();
                    CBServices.ItemsSource = device.Services;
                    CBServices.Visibility = Visibility.Visible;
                }
            }
        }

        private void CBServices_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CBCharacteristic.ItemsSource = null;
            CBCharacteristic.Visibility = Visibility.Collapsed;

            if (e.AddedItems.Count > 0)
            {
                ObservableGattDeviceService service = e.AddedItems[0] as ObservableGattDeviceService;

                if (service != null)
                {
                    CBCharacteristic.ItemsSource = service.Characteristics;
                    CBCharacteristic.Visibility = Visibility.Visible;
                }
            }
        }

        private void CBCharacteristic_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                ObservableGattCharacteristics characteristic = e.AddedItems[0] as ObservableGattCharacteristics;

                if (characteristic.Characteristic.CharacteristicProperties.HasFlag(GattCharacteristicProperties.Read))
                {
                    BtReadCharValue.Visibility = Visibility.Visible;
                    TBCharValue.Text = string.Empty;
                }
                else
                {
                    TBCharValue.Text = "This characteristic can not be read because the read property is not set";
                }
            }
            else
            {
                BtReadCharValue.Visibility = Visibility.Collapsed;
                TBCharValue.Text = string.Empty;
            }
        }

        private async void ReadCharValue_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            button.IsEnabled = false;

            ObservableGattCharacteristics characteristic = CBCharacteristic.SelectedItem as ObservableGattCharacteristics;

            if (characteristic != null)
            {
                TBCharValue.Text = await characteristic.ReadValueAsync();
            }

            button.IsEnabled = true;
        }
    }
}
