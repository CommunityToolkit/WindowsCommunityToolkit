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

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.Advertisement;
using Windows.Devices.Enumeration;

namespace Microsoft.Toolkit.Uwp
{
    /// <summary>
    /// Context for the entire app. This is where all app wide variables are stored
    /// </summary>
    public class BluetoothLEHelper
    {
        /// <summary>
        /// AQS search string used to find bluetooth devices.
        /// </summary>
        private const string BluetoothLeDeviceWatcherAqs = "(System.Devices.Aep.ProtocolId:=\"{bb7bb05e-5972-42b5-94fc-76eaa7084d49}\")";

        /// <summary>
        /// Lock around the <see cref="BluetoothLeDevices"/>. Used in the Add/Removed/Updated callbacks
        /// </summary>
        private readonly object _bluetoothLeDevicesLock = new object();

        /// <summary>
        /// We need to cache all DeviceInformation objects we get as they may
        /// get updated in the future. The update may make them eligible to be put on
        /// the displayed list.
        /// </summary>
        private readonly List<DeviceInformation> _unusedDevices = new List<DeviceInformation>();

        /// <summary>
        /// Advertisement watcher used to find bluetooth devices.
        /// </summary>
        private BluetoothLEAdvertisementWatcher _advertisementWatcher;

        /// <summary>
        /// Device watcher used to find bluetooth devices
        /// </summary>
        private DeviceWatcher _deviceWatcher;

        /// <summary>
        /// The Bluetooth adapter.
        /// </summary>
        private BluetoothAdapter _adapter;

        /// <summary>
        /// Prevents a default instance of the <see cref="BluetoothLEHelper" /> class from being created.
        /// </summary>
        private BluetoothLEHelper()
        {
            Init();
        }

        /// <summary>
        /// Gets the app context
        /// </summary>
        public static BluetoothLEHelper Context { get; } = new BluetoothLEHelper();

        /// <summary>
        /// Gets the list of available bluetooth devices
        /// </summary>
        public ObservableCollection<ObservableBluetoothLEDevice> BluetoothLeDevices { get; } = new ObservableCollection<ObservableBluetoothLEDevice>();

        /// <summary>
        /// Gets or sets the selected characteristic
        /// </summary>
        public ObservableGattCharacteristics SelectedCharacteristic { get; set; } = null;

        /// <summary>
        /// Gets a value indicating whether app is currently enumerating
        /// </summary>
        public bool IsEnumerating
        {
            get
            {
                if (_advertisementWatcher == null)
                {
                    _advertisementWatcher = new BluetoothLEAdvertisementWatcher();
                }

                return _advertisementWatcher.Status == BluetoothLEAdvertisementWatcherStatus.Started;
            }
        }

        /// <summary>
        /// Gets a value indicating whether peripheral mode is supported by this device
        /// </summary>
        public bool IsPeripheralRoleSupported => _adapter.IsPeripheralRoleSupported;

        /// <summary>
        /// Gets a value indicating whether central role is supported by this device
        /// </summary>
        public bool IsCentralRoleSupported => _adapter.IsCentralRoleSupported;

        /// <summary>
        /// Starts enumeration of bluetooth device
        /// </summary>
        public void StartEnumeration()
        {
            if (_advertisementWatcher?.Status == BluetoothLEAdvertisementWatcherStatus.Started)
            {
                return;
            }

            // Additional properties we would like about the device.
            string[] requestedProperties =
            {
                "System.Devices.Aep.Category",
                "System.Devices.Aep.ContainerId",
                "System.Devices.Aep.DeviceAddress",
                "System.Devices.Aep.IsConnected",
                "System.Devices.Aep.IsPaired",
                "System.Devices.Aep.IsPresent",
                "System.Devices.Aep.ProtocolId",
                "System.Devices.Aep.Bluetooth.Le.IsConnectable"
            };

            // BT_Code: Currently Bluetooth APIs don't provide a selector to get ALL devices that are both paired and non-paired.
            _deviceWatcher = DeviceInformation.CreateWatcher(
                BluetoothLeDeviceWatcherAqs,
                requestedProperties,
                DeviceInformationKind.AssociationEndpoint);

            // Register event handlers before starting the watcher.
            _deviceWatcher.Added += DeviceWatcher_Added;
            _deviceWatcher.Updated += DeviceWatcher_Updated;
            _deviceWatcher.Removed += DeviceWatcher_Removed;
            _deviceWatcher.EnumerationCompleted += DeviceWatcher_EnumerationCompleted;

            _advertisementWatcher = new BluetoothLEAdvertisementWatcher();
            _advertisementWatcher.Received += AdvertisementWatcher_Received;

            BluetoothLeDevices.Clear();

            _deviceWatcher.Start();
            _advertisementWatcher.Start();
        }

        /// <summary>
        /// Stops enumeration of bluetooth device
        /// </summary>
        public void StopEnumeration()
        {
            if (_deviceWatcher != null)
            {
                _deviceWatcher.Added -= DeviceWatcher_Added;
                _deviceWatcher.Updated -= DeviceWatcher_Updated;
                _deviceWatcher.Removed -= DeviceWatcher_Removed;
                _deviceWatcher.EnumerationCompleted -= DeviceWatcher_EnumerationCompleted;

                _deviceWatcher.Stop();
                _deviceWatcher = null;
            }

            if (_advertisementWatcher != null)
            {
                _advertisementWatcher.Received -= AdvertisementWatcher_Received;
                _advertisementWatcher.Stop();
                _advertisementWatcher = null;
            }
        }

        /// <summary>
        /// Initializes the app context
        /// </summary>
        private async void Init()
        {
            _adapter = await BluetoothAdapter.GetDefaultAsync();
        }

        /// <summary>
        /// Updates device metadata based on advertisement received
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private async void AdvertisementWatcher_Received(BluetoothLEAdvertisementWatcher sender, BluetoothLEAdvertisementReceivedEventArgs args)
        {
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(
                Windows.UI.Core.CoreDispatcherPriority.Normal,
                () =>
                {
                    lock (_bluetoothLeDevicesLock)
                    {
                        foreach (ObservableBluetoothLEDevice d in BluetoothLeDevices)
                        {
                            if (d.BluetoothAddressAsUlong == args.BluetoothAddress)
                            {
                                d.ServiceCount = args.Advertisement.ServiceUuids.Count();
                            }
                        }
                    }
                });
        }

        /// <summary>
        /// Callback when a new device is found
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="deviceInfo"></param>
        private async void DeviceWatcher_Added(DeviceWatcher sender, DeviceInformation deviceInfo)
        {
            Debug.WriteLine(deviceInfo.Name);
            await AddDeviceToList(deviceInfo);
        }

        /// <summary>
        /// Executes when a device is updated
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="deviceInfoUpdate"></param>
        private async void DeviceWatcher_Updated(DeviceWatcher sender, DeviceInformationUpdate deviceInfoUpdate)
        {
            var device = BluetoothLeDevices.FirstOrDefault(i => i.DeviceInfo.Id == deviceInfoUpdate.Id);
            device?.Update(deviceInfoUpdate);

            if (device != null)
            {
                Debug.WriteLine("Updated: " + device.Name);
            }

            //DeviceInformation di = null;
            //var addNewDI = false;

            //try
            //{
            //    // Protect against race condition if the task runs after the app stopped the deviceWatcher.
            //    if (sender == _deviceWatcher)
            //    {
            //        await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(
            //            Windows.UI.Core.CoreDispatcherPriority.Normal,
            //            async () =>
            //            {
            //                ObservableBluetoothLEDevice dev;

            //                // Need to lock as another DeviceWatcher might be modifying BluetoothLEDevices 
            //                lock (_bluetoothLeDevicesLock)
            //                {
            //                    dev =
            //                        BluetoothLeDevices.FirstOrDefault(
            //                            device => device.DeviceInfo.Id == deviceInfoUpdate.Id);
            //                    if (dev != null)
            //                    {
            //                        // Found a device in the list, updating it
            //                        Debug.WriteLine("DeviceWatcher_Updated: Updating '{0}' - {1}", dev.Name,
            //                            dev.DeviceInfo.Id);
            //                        dev.Update(deviceInfoUpdate);
            //                    }
            //                    else
            //                    {
            //                        // Need to add this device. Can't do that here as we have the lock
            //                        Debug.WriteLine("DeviceWatcher_Updated: Need to add {0}", deviceInfoUpdate.Id);
            //                        addNewDI = true;
            //                    }
            //                }

            //                if (addNewDI)
            //                {
            //                    lock (_bluetoothLeDevicesLock)
            //                    {
            //                        di = _unusedDevices.FirstOrDefault(device => device.Id == deviceInfoUpdate.Id);
            //                        if (di != null)
            //                        {
            //                            // We found this device before.
            //                            _unusedDevices.Remove(di);
            //                            di.Update(deviceInfoUpdate);
            //                        }
            //                        else
            //                        {
            //                            Debug.WriteLine(
            //                                "DeviceWatcher_Updated: Received DeviceInfoUpdate for a unknown device, skipping");
            //                        }
            //                    }

            //                    if (di != null)
            //                    {
            //                        await AddDeviceToList(di);
            //                    }
            //                }
            //            });
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Debug.WriteLine("DeviceWatcher_Updated: " + ex.Message);
            //}
        }

        /// <summary>
        /// Executes when a device is removed from enumeration
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="deviceInfoUpdate"></param>
        private async void DeviceWatcher_Removed(DeviceWatcher sender, DeviceInformationUpdate deviceInfoUpdate)
        {
            var device = BluetoothLeDevices.FirstOrDefault(i => i.DeviceInfo.Id == deviceInfoUpdate.Id);
            BluetoothLeDevices?.Remove(device);

            if (device != null)
            {
                Debug.WriteLine("Removed: " + device.Name);
            }

            //await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(
            //    Windows.UI.Core.CoreDispatcherPriority.Normal,
            //    () =>
            //    {
            //        ObservableBluetoothLEDevice dev;

            //            // Need to lock as another DeviceWatcher might be modifying BluetoothLEDevices 
            //            lock (_bluetoothLeDevicesLock)
            //        {
            //                // Find the corresponding DeviceInformation in the collection and remove it.
            //                dev =
            //                BluetoothLeDevices.FirstOrDefault(
            //                    device => device.DeviceInfo.Id == deviceInfoUpdate.Id);
            //            if (dev != null)
            //            {
            //                    // Found it in our displayed devices
            //                    Debug.WriteLine("DeviceWatcher_Removed: Removing '{0}' - '{1}'", dev.Name,
            //                    dev.DeviceInfo.Id);
            //                Debug.Assert(BluetoothLeDevices.Remove(dev),
            //                    "DeviceWatcher_Removed: Failed to remove device from list");
            //            }
            //            else
            //            {
            //                    // Did not find in diplayed list, let's check the unused list
            //                    var di = _unusedDevices.FirstOrDefault(device => device.Id == deviceInfoUpdate.Id);

            //                if (di != null)
            //                {
            //                        // Found in unused devices, remove it
            //                        Debug.WriteLine(
            //                        "DeviceWatcher_Removed: Removing from used devices '{0}' - '{1}'", di.Name,
            //                        di.Id);
            //                    Debug.Assert(_unusedDevices.Remove(di),
            //                        "DeviceWatcher_Removed: Failed to remove device from unused");
            //                }
            //            }
            //        }
            //    });
        }

        /// <summary>
        /// Executes when Enumeration has finished
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeviceWatcher_EnumerationCompleted(DeviceWatcher sender, object e)
        {
            //StopEnumeration();
        }

        /// <summary>
        /// Adds the new or updated device to the displayed or unused list
        /// </summary>
        /// <param name="deviceInfo"></param>
        /// <returns></returns>
        private async Task AddDeviceToList(DeviceInformation deviceInfo)
        {
            // Make sure device name isn't blank or already present in the list.
            if (deviceInfo.Name != string.Empty)
            {
                ObservableBluetoothLEDevice dev = new ObservableBluetoothLEDevice(deviceInfo);

                // Let's make it connectable by default, we have error handles in case it doesn't work
                var connectable = true;

                // If the connectable key exists then let's read it
                if (dev.DeviceInfo.Properties.Keys.Contains("System.Devices.Aep.Bluetooth.Le.IsConnectable"))
                {
                    connectable = (bool)dev.DeviceInfo.Properties["System.Devices.Aep.Bluetooth.Le.IsConnectable"];
                }

                if (connectable)
                {
                    await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(
                        Windows.UI.Core.CoreDispatcherPriority.Normal,
                        () =>
                        {
                            // Need to lock as another DeviceWatcher might be modifying BluetoothLEDevices 
                            lock (_bluetoothLeDevicesLock)
                            {
                                if (BluetoothLeDevices.Contains(dev) == false)
                                {
                                    Debug.WriteLine("AddDeviceToList: Adding '{0}' - connectable: {1}", dev.Name,
                                        connectable);
                                    BluetoothLeDevices.Add(dev);
                                }
                            }
                        });
                }
                else
                {
                    lock (_bluetoothLeDevicesLock)
                    {
                        _unusedDevices.Add(deviceInfo);
                    }
                    Debug.WriteLine(
                        "AddDeviceToList: Found but not adding because it's not connectable '{0}' - connectable: {1}, deviceID: {2}",
                        dev.Name, dev.DeviceInfo.Properties["System.Devices.Aep.Bluetooth.Le.IsConnectable"],
                        dev.DeviceInfo.Id);
                }
            }
            else
            {
                lock (_bluetoothLeDevicesLock)
                {
                    _unusedDevices.Add(deviceInfo);
                }

                Debug.WriteLine($"AddDeviceToList: Found device {deviceInfo.Id} without a name. Not displaying.");
            }
        }
    }
}