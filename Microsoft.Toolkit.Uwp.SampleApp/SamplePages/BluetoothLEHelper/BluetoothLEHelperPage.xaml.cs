using System;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BluetoothLEHelperPage : Page
    {
        private BluetoothLEHelper bluetoothLEHelper = BluetoothLEHelper.Context;

        public BluetoothLEHelperPage()
        {
            this.InitializeComponent();
            bluetoothLEHelper.EnumerationCompleted += BluetoothLEHelper_EnumerationCompleted;
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
                    await device.Connect();
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
                    TBCharValue.Text = String.Empty;
                }
                else
                {
                    TBCharValue.Text = "This characteristic can not be read because the read property is not set";
                }
            }
            else
            {
                BtReadCharValue.Visibility = Visibility.Collapsed;
                TBCharValue.Text = String.Empty;
            }
        }

        private void ReadCharValue_Click(object sender, RoutedEventArgs e)
        {
            ObservableGattCharacteristics characteristic = CBCharacteristic.SelectedItem as ObservableGattCharacteristics;

            if (characteristic != null)
            {
                characteristic.ReadValueAsync();
                TBCharValue.Text = characteristic.Value;
            }
        }
    }
}
