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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth.Advertisement;
using Windows.Devices.Enumeration;

namespace Microsoft.Toolkit.Uwp
{
    /// <summary>
    /// Context for the entire app. This is where all app wide variables are stored
    /// </summary>
    public class BluetoothLEHelper : INotifyPropertyChanged
    {
        /// <summary>
        /// AQS search string used to find bluetooth devices
        /// </summary>
        private const string BTLEDeviceWatcherAQSString =
            "(System.Devices.Aep.ProtocolId:=\"{bb7bb05e-5972-42b5-94fc-76eaa7084d49}\")";

        /// <summary>
        /// Advertisement watcher used to find bluetooth devices
        /// </summary>
        private BluetoothLEAdvertisementWatcher _advertisementWatcher;

        /// <summary>
        /// Lock around the <see cref="BluetoothLeDevices"/>. Used in the Add/Removed/Updated callbacks
        /// </summary>
        private readonly object _bluetoothLeDevicesLock = new object();

        /// <summary>
        /// Device watcher used to find bluetooth devices
        /// </summary>
        private DeviceWatcher _deviceWatcher;

        /// <summary>
        /// Source for <see cref="EnumerationFinished"/> property
        /// </summary>
        private bool _enumorationFinished;

        /// <summary>
        /// Source for <see cref="IsCentralRoleSupported"/>
        /// </summary>
        private bool _isCentralRoleSupported = true;

        /// <summary>
        /// Source for <see cref="IsEnumerating"/> property
        /// </summary>
        private bool _isEnumerating;

        /// <summary>
        /// Source for <see cref="IsPeripheralRoleSupported"/>
        /// </summary>
        private bool _isPeripheralRoleSupported = true;

        /// <summary>
        /// We need to cache all DeviceInformation objects we get as they may
        /// get updated in the future. The update may make them eligible to be put on
        /// the displayed list.
        /// </summary>
        private readonly List<DeviceInformation> _unusedDevices = new List<DeviceInformation>();

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
        public static BluetoothLEHelper Context { get; private set; } = new BluetoothLEHelper();

        /// <summary>
        /// Gets or sets the list of available bluetooth devices
        /// </summary>
        public ObservableCollection<ObservableBluetoothLEDevice> BluetoothLeDevices { get; set; } =
            new ObservableCollection<ObservableBluetoothLEDevice>();

        /// <summary>
        /// Gets or sets the selected bluetooth device
        /// </summary>
        public ObservableBluetoothLEDevice SelectedBluetoothLEDevice { get; set; } = null;

        /// <summary>
        /// Gets or sets the selected characteristic
        /// </summary>
        public ObservableGattCharacteristics SelectedCharacteristic { get; set; } = null;

        /// <summary>
        /// Gets or sets the list of created service
        /// </summary>
        // TODO: fix this
        //public ObservableCollection<GenericGattServiceViewModel> CreatedServices { get; set; } =
        //    new ObservableCollection<GenericGattServiceViewModel>();

        /// <summary>
        /// Gets or sets the currently selected gatt server service
        /// </summary>
        // TODO: fix this
        //public GenericGattServiceViewModel SelectedGattServerService { get; set; } = null;

        /// <summary>
        /// Gets a value indicating whether app is currently enumerating
        /// </summary>
        public bool IsEnumerating
        {
            get { return _isEnumerating; }

            private set
            {
                if (_isEnumerating != value)
                {
                    _isEnumerating = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("IsEnumerating"));
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether the app is finished enumerating
        /// </summary>
        public bool EnumerationFinished
        {
            get { return _enumorationFinished; }

            private set
            {
                if (_enumorationFinished != value)
                {
                    _enumorationFinished = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("EnumerationFinished"));
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether peripheral mode is supported by this device
        /// </summary>
        public bool IsPeripheralRoleSupported
        {
            get { return _isPeripheralRoleSupported; }

            private set
            {
                if (_isPeripheralRoleSupported != value)
                {
                    _isPeripheralRoleSupported = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("IsPeripheralRoleSupported"));
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether central role is supported by this device
        /// </summary>
        public bool IsCentralRoleSupported
        {
            get { return _isCentralRoleSupported; }

            private set
            {
                if (_isCentralRoleSupported != value)
                {
                    _isCentralRoleSupported = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("IsCentralRoleSupported"));
                }
            }
        }

        /// <summary>
        /// Event to notify when this object has changed
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Initializes the app context
        /// </summary>
        private async void Init()
        {
            Windows.Devices.Bluetooth.BluetoothAdapter adapter =
                await Windows.Devices.Bluetooth.BluetoothAdapter.GetDefaultAsync();

            IsPeripheralRoleSupported = adapter.IsPeripheralRoleSupported;
            IsCentralRoleSupported = adapter.IsCentralRoleSupported;
        }

        /// <summary>
        /// Starts enumeration of bluetooth device
        /// </summary>
        public void StartEnumeration()
        {
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
                ////"System.Devices.Aep.SignalStrength" //remove Sig strength for now. Might bring it back for sorting/filtering
            };

            // BT_Code: Currently Bluetooth APIs don't provide a selector to get ALL devices that are both paired and non-paired.
            _deviceWatcher =
                DeviceInformation.CreateWatcher(
                    BTLEDeviceWatcherAQSString,
                    requestedProperties,
                    DeviceInformationKind.AssociationEndpoint);

            // Register event handlers before starting the watcher.
            _deviceWatcher.Added += DeviceWatcher_Added;
            _deviceWatcher.Updated += DeviceWatcher_Updated;
            _deviceWatcher.Removed += DeviceWatcher_Removed;
            _deviceWatcher.EnumerationCompleted += DeviceWatcher_EnumerationCompleted;
            _deviceWatcher.Stopped += DeviceWatcher_Stopped;

            _advertisementWatcher = new BluetoothLEAdvertisementWatcher();
            _advertisementWatcher.Received += AdvertisementWatcher_Received;

            BluetoothLeDevices.Clear();

            _deviceWatcher.Start();
            _advertisementWatcher.Start();
            IsEnumerating = true;
            EnumerationFinished = false;
        }

        /// <summary>
        /// Stops enumeration of bluetooth device
        /// </summary>
        public void StopEnumeration()
        {
            if (_deviceWatcher != null)
            {
                // Unregister the event handlers.
                _deviceWatcher.Added -= DeviceWatcher_Added;
                _deviceWatcher.Updated -= DeviceWatcher_Updated;
                _deviceWatcher.Removed -= DeviceWatcher_Removed;
                _deviceWatcher.EnumerationCompleted -= DeviceWatcher_EnumerationCompleted;
                _deviceWatcher.Stopped -= DeviceWatcher_Stopped;

                _advertisementWatcher.Received += AdvertisementWatcher_Received;

                // Stop the watchers
                _deviceWatcher.Stop();
                _deviceWatcher = null;

                _advertisementWatcher.Stop();
                _advertisementWatcher = null;
                IsEnumerating = false;
                EnumerationFinished = false;
            }
        }

        /// <summary>
        /// Updates device metadata based on advertisement received
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private async void AdvertisementWatcher_Received(BluetoothLEAdvertisementWatcher sender,
            BluetoothLEAdvertisementReceivedEventArgs args)
        {
            try
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
            catch (Exception ex)
            {
                Debug.WriteLine("AdvertisementWatcher_Received: ", ex.Message);
            }
        }

        /// <summary>
        /// Callback when a new device is found
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="deviceInfo"></param>
        private async void DeviceWatcher_Added(DeviceWatcher sender, DeviceInformation deviceInfo)
        {
            try
            {
                // Protect against race condition if the task runs after the app stopped the deviceWatcher.
                if (sender == _deviceWatcher)
                {
                    await AddDeviceToList(deviceInfo);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("DeviceWatcher_Added: " + ex.Message);
            }
        }

        /// <summary>
        /// Executes when a device is updated
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="deviceInfoUpdate"></param>
        private async void DeviceWatcher_Updated(DeviceWatcher sender, DeviceInformationUpdate deviceInfoUpdate)
        {
            DeviceInformation di = null;
            var addNewDI = false;

            try
            {
                // Protect against race condition if the task runs after the app stopped the deviceWatcher.
                if (sender == _deviceWatcher)
                {
                    await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(
                        Windows.UI.Core.CoreDispatcherPriority.Normal,
                        async () =>
                        {
                            ObservableBluetoothLEDevice dev;

                            // Need to lock as another DeviceWatcher might be modifying BluetoothLEDevices 
                            lock (_bluetoothLeDevicesLock)
                            {
                                dev =
                                    BluetoothLeDevices.FirstOrDefault(
                                        device => device.DeviceInfo.Id == deviceInfoUpdate.Id);
                                if (dev != null)
                                {
                                    // Found a device in the list, updating it
                                    Debug.WriteLine("DeviceWatcher_Updated: Updating '{0}' - {1}", dev.Name,
                                        dev.DeviceInfo.Id);
                                    dev.Update(deviceInfoUpdate);
                                }
                                else
                                {
                                    // Need to add this device. Can't do that here as we have the lock
                                    Debug.WriteLine("DeviceWatcher_Updated: Need to add {0}", deviceInfoUpdate.Id);
                                    addNewDI = true;
                                }
                            }

                            if (addNewDI)
                            {
                                lock (_bluetoothLeDevicesLock)
                                {
                                    di = _unusedDevices.FirstOrDefault(device => device.Id == deviceInfoUpdate.Id);
                                    if (di != null)
                                    {
                                        // We found this device before.
                                        _unusedDevices.Remove(di);
                                        di.Update(deviceInfoUpdate);
                                    }
                                    else
                                    {
                                        Debug.WriteLine(
                                            "DeviceWatcher_Updated: Received DeviceInfoUpdate for a unknown device, skipping");
                                    }
                                }

                                if (di != null)
                                {
                                    await AddDeviceToList(di);
                                }
                            }
                        });
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("DeviceWatcher_Updated: " + ex.Message);
            }
        }

        /// <summary>
        /// Executes when a device is removed from enumeration
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="deviceInfoUpdate"></param>
        private async void DeviceWatcher_Removed(DeviceWatcher sender, DeviceInformationUpdate deviceInfoUpdate)
        {
            try
            {
                // Protect against race condition if the task runs after the app stopped the deviceWatcher.
                if (sender == _deviceWatcher)
                {
                    await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(
                        Windows.UI.Core.CoreDispatcherPriority.Normal,
                        () =>
                        {
                            ObservableBluetoothLEDevice dev;

                            // Need to lock as another DeviceWatcher might be modifying BluetoothLEDevices 
                            lock (_bluetoothLeDevicesLock)
                            {
                                // Find the corresponding DeviceInformation in the collection and remove it.
                                dev =
                                    BluetoothLeDevices.FirstOrDefault(
                                        device => device.DeviceInfo.Id == deviceInfoUpdate.Id);
                                if (dev != null)
                                {
                                    // Found it in our displayed devices
                                    Debug.WriteLine("DeviceWatcher_Removed: Removing '{0}' - '{1}'", dev.Name,
                                        dev.DeviceInfo.Id);
                                    Debug.Assert(BluetoothLeDevices.Remove(dev),
                                        "DeviceWatcher_Removed: Failed to remove device from list");
                                }
                                else
                                {
                                    // Did not find in diplayed list, let's check the unused list
                                    var di = _unusedDevices.FirstOrDefault(device => device.Id == deviceInfoUpdate.Id);

                                    if (di != null)
                                    {
                                        // Found in unused devices, remove it
                                        Debug.WriteLine(
                                            "DeviceWatcher_Removed: Removing from used devices '{0}' - '{1}'", di.Name,
                                            di.Id);
                                        Debug.Assert(_unusedDevices.Remove(di),
                                            "DeviceWatcher_Removed: Failed to remove device from unused");
                                    }
                                }
                            }
                        });
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("DeviceWatcher_Removed: " + ex.Message);
            }
        }

        /// <summary>
        /// Executes when Enumeration has finished
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeviceWatcher_EnumerationCompleted(DeviceWatcher sender, object e)
        {
            // Protect against race condition if the task runs after the app stopped the deviceWatcher.
            if (sender == _deviceWatcher)
            {
                Debug.WriteLine("DeviceWatcher_EnumerationCompleted: Enumoration Finished");
                StopEnumeration();
                EnumerationFinished = true;
            }
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
                if (dev.DeviceInfo.Properties.Keys.Contains("System.Devices.Aep.Bluetooth.Le.IsConnectable") == true)
                {
                    connectable = (bool) dev.DeviceInfo.Properties["System.Devices.Aep.Bluetooth.Le.IsConnectable"];
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

        /// <summary>
        /// Executes when device watcher has stopped
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeviceWatcher_Stopped(DeviceWatcher sender, object e)
        {
            // Implimented for completeness
        }

        /// <summary>
        /// Executes when a property has changed
        /// </summary>
        /// <param name="e"></param>
        private void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }
    }
}