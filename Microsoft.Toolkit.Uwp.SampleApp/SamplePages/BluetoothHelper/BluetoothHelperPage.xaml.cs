using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BluetoothHelperPage : Page
    {
        private BluetoothLEHelper bluetoothLEHelper = BluetoothLEHelper.Context;

        public BluetoothHelperPage()
        {
            this.InitializeComponent();

            bluetoothLEHelper.PropertyChanged += BluetoothLEHelper_PropertyChanged;
        }

        private async void BluetoothLEHelper_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(
                Windows.UI.Core.CoreDispatcherPriority.Normal,
                () =>
                {
                    if (e.PropertyName == "IsEnumerating")
                    {
                        if (bluetoothLEHelper.IsEnumerating)
                        {
                            BtEnumeration.Content = "Stop Enumerating";
                        }
                        else
                        {
                            BtEnumeration.Content = "Start Enumerating";
                        }
                    }
                });
        }

        private void Enumeration_Click(object sender, RoutedEventArgs e)
        {
            if (bluetoothLEHelper.IsEnumerating == false)
            {
                bluetoothLEHelper.StartEnumeration();
            }
            else
            {
                bluetoothLEHelper.StopEnumeration();
            }
        }

        private async void LV_Devices_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TbDeviceName.Text = "No device selected";
            TbDeviceBtAddr.Text = "No device selected";

            LVServices.Visibility = Visibility.Collapsed;
            LVServices.ItemsSource = null;

            if (e.AddedItems.Count > 0)
            {
                ObservableBluetoothLEDevice device = e.AddedItems[0] as ObservableBluetoothLEDevice;

                if (device != null)
                {
                    TbDeviceName.Text = "Device Name: " + device.Name;
                    TbDeviceBtAddr.Text = "Device Address: " + device.BluetoothAddressAsString;

                    // Make sure the Bluetooth capability is set else this will fail
                    bluetoothLEHelper.StopEnumeration();
                    bool connected = await device.Connect();
                    if (connected)
                    {
                        LVServices.Visibility = Visibility.Visible;
                        LVServices.ItemsSource = device.Services;
                    }
                }
            }
        }
    }
}
