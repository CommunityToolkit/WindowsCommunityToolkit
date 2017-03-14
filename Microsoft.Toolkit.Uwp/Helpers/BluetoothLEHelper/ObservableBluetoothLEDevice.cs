// <copyright file="ObservableBluetoothLEDevice.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//----------------------------------------------------------------------------------------------
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BluetoothExplorer.Services.DispatcherService;
using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using Windows.Devices.Enumeration;
using Windows.UI.Popups;
using Windows.UI.Xaml.Media.Imaging;

namespace BluetoothExplorer.Models
{
    /// <summary>
    /// Wrapper around <see cref="BluetoothLEDevice"/> to make it easier to use
    /// </summary>
    public class ObservableBluetoothLEDevice : INotifyPropertyChanged, IEquatable<ObservableBluetoothLEDevice>
    {
        /// <summary>
        /// Source for <see cref="BluetoothLEDevice"/>
        /// </summary>
        private BluetoothLEDevice bluetoothLEDevice;
        
        /// <summary>
        /// Gets the bluetooth device this class wraps
        /// </summary>
        public BluetoothLEDevice BluetoothLEDevice 
        {
            get
            {
                return bluetoothLEDevice;
            }

            private set
            {
                bluetoothLEDevice = value;
                OnPropertyChanged(new PropertyChangedEventArgs("BluetoothLEDevice"));
            }
        }
        
        /// <summary>
        /// Source for <see cref="Glyph"/>
        /// </summary>
        private BitmapImage glyph;

        /// <summary>
        /// Gets or sets the glyph of this bluetooth device
        /// </summary>
        public BitmapImage Glyph
        {
            get
            {
                return glyph;
            }

            set
            {
                glyph = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Glyph"));
            }
        }

        /// <summary>
        /// Source for <see cref="DeviceInfo"/>
        /// </summary>
        private DeviceInformation deviceInfo;

        /// <summary>
        /// Gets the device information for the device this class wraps
        /// </summary>
        public DeviceInformation DeviceInfo
        {
            get
            {
                return deviceInfo;
            }

            private set
            {
                deviceInfo = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DeviceInfo"));
            }
        }

        /// <summary>
        /// Source for <see cref="IsConnected"/>
        /// </summary>
        private bool isConnected;

        /// <summary>
        /// Gets or sets a value indicating whether this device is connected
        /// </summary>
        public bool IsConnected
        {
            get
            {
                return isConnected;
            }

            set
            {
                if (isConnected != value)
                {
                    isConnected = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("IsConnected"));
                }
            }
        }

        /// <summary>
        /// Source for <see cref="IsPaired"/>
        /// </summary>
        private bool isPaired;

        /// <summary>
        /// Gets or sets a value indicating whether this device is paired
        /// </summary>
        public bool IsPaired
        {
            get
            {
                return isPaired;
            }

            set
            {
                if (isPaired != value)
                {
                    isPaired = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("IsPaired"));
                }
            }
        }

        /// <summary>
        /// Source for <see cref="Services"/>
        /// </summary>
        private ObservableCollection<ObservableGattDeviceService> services = new ObservableCollection<ObservableGattDeviceService>();

        /// <summary>
        /// Gets the services this device supports
        /// </summary>
        public ObservableCollection<ObservableGattDeviceService> Services
        {
            get
            {
                return services;
            }

            private set
            {
                services = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Services"));
            }
        }

        /// <summary>
        /// Source for <see cref="ServiceCount"/>
        /// </summary>
        private int serviceCount;

        /// <summary>
        /// Gets or sets the number of services this device has
        /// </summary>
        public int ServiceCount
        {
            get
            {
                return serviceCount;
            }

            set
            {
                if (serviceCount != value)
                {
                    serviceCount = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("ServiceCount"));
                }
            }
        }

        /// <summary>
        /// Source for <see cref="Name"/>
        /// </summary>
        private string name;

        /// <summary>
        /// Gets the name of this device
        /// </summary>
        public string Name
        {
            get
            {
                return name;
            }

            private set
            {
                if (name != value)
                {
                    name = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("Name"));
                }
            }
        }

        /// <summary>
        /// Source for <see cref="ErrorText"/>
        /// </summary>
        private string errorText;

        /// <summary>
        /// Gets the error text when connecting to this device fails
        /// </summary>
        public string ErrorText
        {
            get
            {
                return errorText;
            }

            private set
            {
                errorText = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ErrorText"));
            }
        }

        /// <summary>
        /// Gets the bluetooth address of this device as a string
        /// </summary>
        public string BluetoothAddressAsString
        {
            get
            {
                string ret = String.Empty;

                try
                {
                    ret = DeviceInfo.Properties["System.Devices.Aep.DeviceAddress"].ToString();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }

                return ret;
            }
        }

        /// <summary>
        /// Gets the bluetooth address of this device
        /// </summary>
        public ulong BluetoothAddressAsUlong
        {
            get
            {
                try
                { 
                    ulong ret = Convert.ToUInt64(BluetoothAddressAsString.Replace(":", String.Empty), 16);
                    return ret;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("BluetoothAddressAsUlong Getter: " + ex.Message);
                }

                return 0;
            }
        }

        /// <summary>
        /// Initializes a new instance of the<see cref="ObservableBluetoothLEDevice" /> class.
        /// </summary>
        /// <param name="deviceInfo">The device info that describes this bluetooth device"/></param>
        public ObservableBluetoothLEDevice(DeviceInformation deviceInfo)
        {
            DeviceInfo = deviceInfo;
            Name = DeviceInfo.Name;

            IsPaired = DeviceInfo.Pairing.IsPaired;

            LoadGlyph();
        }

        /// <summary>
        /// result of finding all the services
        /// </summary>
        private GattDeviceServicesResult result;

        /// <summary>
        /// Event to notify when this object has changed
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Connect to this bluetooth device
        /// </summary>
        /// <returns>Connection task</returns>
        public async Task<bool> Connect()
        {
            bool ret = false;
            string debugMsg = String.Format("Connect: ");

            Debug.WriteLine(debugMsg + "Entering");

            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunTaskAsync(async () =>
            {
                Debug.WriteLine(debugMsg + "In UI thread");
                try
                {
                    
                    if (BluetoothLEDevice == null)
                    {
                        Debug.WriteLine(debugMsg + "Calling BluetoothLEDevice.FromIdAsync");
                        BluetoothLEDevice = await BluetoothLEDevice.FromIdAsync(DeviceInfo.Id);
                    }
                    else
                    {
                        Debug.WriteLine(debugMsg + "Previously connected, not calling BluetoothLEDevice.FromIdAsync");
                    }

                    if (BluetoothLEDevice == null)
                    {
                        ret = false;
                        Debug.WriteLine(debugMsg + "BluetoothLEDevice is null");

                        MessageDialog dialog = new MessageDialog("No permission to access device", "Connection error");
                        await dialog.ShowAsync();
                    }
                    else
                    {
                        Debug.WriteLine(debugMsg + "BluetoothLEDevice is " + BluetoothLEDevice.Name);

                        // Setup our event handlers and view model properties
                        BluetoothLEDevice.ConnectionStatusChanged += BluetoothLEDevice_ConnectionStatusChanged;
                        BluetoothLEDevice.NameChanged += BluetoothLEDevice_NameChanged;

                        IsPaired = DeviceInfo.Pairing.IsPaired;
                        IsConnected = BluetoothLEDevice.ConnectionStatus == BluetoothConnectionStatus.Connected;
                        Name = BluetoothLEDevice.Name;

                        // Get all the services for this device
                        CancellationTokenSource GetGattServicesAsyncTokenSource = new CancellationTokenSource(5000);
                        var GetGattServicesAsyncTask = Task.Run(() => BluetoothLEDevice.GetGattServicesAsync(BluetoothCacheMode.Uncached), GetGattServicesAsyncTokenSource.Token);

                        result = await GetGattServicesAsyncTask.Result;

                        if (result.Status == GattCommunicationStatus.Success)
                        {
                            System.Diagnostics.Debug.WriteLine(debugMsg + "GetGattServiceAsync SUCCESS");
                            foreach (var serv in result.Services)
                            {
                                Services.Add(new ObservableGattDeviceService(serv));
                            }

                            ServiceCount = Services.Count();
                            ret = true;
                        }
                        else if (result.Status == GattCommunicationStatus.ProtocolError)
                        {
                            ErrorText = debugMsg + "GetGattServiceAsync Error: Protocol Error - " + result.ProtocolError.Value;
                            System.Diagnostics.Debug.WriteLine(ErrorText);
                            string msg = "Connection protocol error: " + result.ProtocolError.Value.ToString();
                            var messageDialog = new MessageDialog(msg, "Connection failures");
                            await messageDialog.ShowAsync();

                        }
                        else if (result.Status == GattCommunicationStatus.Unreachable)
                        {
                            ErrorText = debugMsg + "GetGattServiceAsync Error: Unreachable";
                            System.Diagnostics.Debug.WriteLine(ErrorText);
                            string msg = "Device unreachable";
                            var messageDialog = new MessageDialog(msg, "Connection failures");
                            await messageDialog.ShowAsync();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(debugMsg + "Exception - " + ex.Message);
                    string msg = String.Format("Message:\n{0}\n\nInnerException:\n{1}\n\nStack:\n{2}", ex.Message, ex.InnerException, ex.StackTrace);

                    var messageDialog = new MessageDialog(msg, "Exception");
                    await messageDialog.ShowAsync();

                    // Debugger break here so we can catch unknown exceptions
                    Debugger.Break();
                }
            });

            if (ret)
            {
                Debug.WriteLine(debugMsg + "Exiting (0)");
            }
            else
            {
                Debug.WriteLine(debugMsg + "Exiting (-1)");
            }

            return ret;
        }

        public async Task<bool> DoInAppPairing()
        {
            Debug.WriteLine("Trying in app pairing");

            // BT_Code: Pair the currently selected device.
            DevicePairingResult result = await DeviceInfo.Pairing.PairAsync();

            Debug.WriteLine($"Pairing result: {result.Status.ToString()}");

            if (result.Status != DevicePairingResultStatus.Paired || result.Status != DevicePairingResultStatus.AlreadyPaired)
            {
                MessageDialog d = new MessageDialog("Pairing error", result.Status.ToString());
                await d.ShowAsync();
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Executes when the name of this devices changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private async void BluetoothLEDevice_NameChanged(BluetoothLEDevice sender, object args)
        {
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(
                Windows.UI.Core.CoreDispatcherPriority.Normal, 
                () =>
            {
                Name = BluetoothLEDevice.Name;
            });
        }

        /// <summary>
        /// Executes when the connection state changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private async void BluetoothLEDevice_ConnectionStatusChanged(BluetoothLEDevice sender, object args)
        {
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(
                Windows.UI.Core.CoreDispatcherPriority.Normal, 
                () =>
            {
                IsPaired = DeviceInfo.Pairing.IsPaired;
                IsConnected = BluetoothLEDevice.ConnectionStatus == BluetoothConnectionStatus.Connected;
            });
        }

        /// <summary>
        /// Load the glyph for this device
        /// </summary>
        private async void LoadGlyph()
        {
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(
                Windows.UI.Core.CoreDispatcherPriority.Normal, 
                async () =>
            {
                DeviceThumbnail deviceThumbnail = await DeviceInfo.GetGlyphThumbnailAsync();
                BitmapImage glyphBitmapImage = new BitmapImage();
                await glyphBitmapImage.SetSourceAsync(deviceThumbnail);
                Glyph = glyphBitmapImage;
            });
        }

        /// <summary>
        /// Executes when a property is changed
        /// </summary>
        /// <param name="e"></param>
        private void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }

        /// <summary>
        /// Overrides the ToString function to return the name of the device
        /// </summary>
        /// <returns>Name of this characteristic</returns>
        public override string ToString()
        {
            return this.Name;
        }

        /// <summary>
        /// Compares this device to other bluetooth devices by checking the id
        /// </summary>
        /// <param name="other"></param>
        /// <returns>true for equal</returns>
        bool IEquatable<ObservableBluetoothLEDevice>.Equals(ObservableBluetoothLEDevice other)
        {
            if (other == null)
            {
                return false;
            }

            if (this.DeviceInfo.Id == other.DeviceInfo.Id)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Updates this device's deviceInformation
        /// </summary>
        /// <param name="deviceUpdate"></param>
        public void Update(DeviceInformationUpdate deviceUpdate)
        {
            DeviceInfo.Update(deviceUpdate);

            OnPropertyChanged(new PropertyChangedEventArgs("DeviceInfo"));
        }
    }
}
