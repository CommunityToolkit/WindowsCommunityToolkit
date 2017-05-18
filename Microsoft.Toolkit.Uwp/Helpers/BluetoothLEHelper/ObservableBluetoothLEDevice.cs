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
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using Windows.Devices.Enumeration;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml.Media.Imaging;

namespace Microsoft.Toolkit.Uwp
{
    /// <summary>
    /// Wrapper around <see cref="BluetoothLEDevice"/> to make it easier to use
    /// </summary>
    public class ObservableBluetoothLEDevice : INotifyPropertyChanged, IEquatable<ObservableBluetoothLEDevice>
    {
        /// <summary>
        /// Source for <see cref="BluetoothLEDevice"/>
        /// </summary>
        private BluetoothLEDevice _bluetoothLeDevice;

        /// <summary>
        /// Source for <see cref="DeviceInfo"/>
        /// </summary>
        private DeviceInformation _deviceInfo;

        /// <summary>
        /// Source for <see cref="ErrorText"/>
        /// </summary>
        private string _errorText;

        /// <summary>
        /// Source for <see cref="Glyph"/>
        /// </summary>
        private BitmapImage _glyph;

        /// <summary>
        /// Source for <see cref="IsConnected"/>
        /// </summary>
        private bool _isConnected;

        /// <summary>
        /// Source for <see cref="IsPaired"/>
        /// </summary>
        private bool _isPaired;

        /// <summary>
        /// Source for <see cref="Name"/>
        /// </summary>
        private string _name;

        /// <summary>
        /// result of finding all the services
        /// </summary>
        private GattDeviceServicesResult _result;

        /// <summary>
        /// Source for <see cref="ServiceCount"/>
        /// </summary>
        private int _serviceCount;

        /// <summary>
        /// Source for <see cref="Services"/>
        /// </summary>
        private ObservableCollection<ObservableGattDeviceService> _services =
            new ObservableCollection<ObservableGattDeviceService>();

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
        /// Gets the bluetooth device this class wraps
        /// </summary>
        public BluetoothLEDevice BluetoothLEDevice
        {
            get => _bluetoothLeDevice;
            private set
            {
                _bluetoothLeDevice = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the glyph of this bluetooth device
        /// </summary>
        public BitmapImage Glyph
        {
            get => _glyph;
            set
            {
                _glyph = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets the device information for the device this class wraps
        /// </summary>
        public DeviceInformation DeviceInfo
        {
            get => _deviceInfo;
            private set
            {
                _deviceInfo = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this device is connected
        /// </summary>
        public bool IsConnected
        {
            get => _isConnected;
            set
            {
                if (_isConnected != value)
                {
                    _isConnected = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this device is paired
        /// </summary>
        public bool IsPaired
        {
            get { return _isPaired; }

            set
            {
                if (_isPaired != value)
                {
                    _isPaired = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets the services this device supports
        /// </summary>
        public ObservableCollection<ObservableGattDeviceService> Services
        {
            get { return _services; }

            private set
            {
                _services = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the number of services this device has
        /// </summary>
        public int ServiceCount
        {
            get { return _serviceCount; }

            set
            {
                if (_serviceCount != value)
                {
                    _serviceCount = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets the name of this device
        /// </summary>
        public string Name
        {
            get { return _name; }

            private set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets the error text when connecting to this device fails
        /// </summary>
        public string ErrorText
        {
            get { return _errorText; }

            private set
            {
                _errorText = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets the bluetooth address of this device as a string
        /// </summary>
        public string BluetoothAddressAsString => DeviceInfo.Properties["System.Devices.Aep.DeviceAddress"].ToString();

        /// <summary>
        /// Gets the bluetooth address of this device
        /// </summary>
        public ulong BluetoothAddressAsUlong => Convert.ToUInt64(BluetoothAddressAsString.Replace(":", string.Empty), 16);

        /// <summary>
        /// Compares this device to other bluetooth devices by checking the id
        /// </summary>
        /// <param name="other"></param>
        /// <returns>true for equal</returns>
        bool IEquatable<ObservableBluetoothLEDevice>.Equals(ObservableBluetoothLEDevice other)
        {
            return other?.DeviceInfo.Id != null && (DeviceInfo.Id == other.DeviceInfo.Id);
        }

        /// <summary>
        /// Event to notify when this object has changed
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Connect to this bluetooth device
        /// </summary>
        /// <exception cref="Exception"></exception>
        /// <returns>Connection task</returns>
        public async Task Connect()
        {
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
            {
                if (BluetoothLEDevice == null)
                {
                    BluetoothLEDevice = await BluetoothLEDevice.FromIdAsync(DeviceInfo.Id);
                }

                if (BluetoothLEDevice == null)
                {
                    throw new Exception("Connection error, no permission to access device");
                }

                BluetoothLEDevice.ConnectionStatusChanged += BluetoothLEDevice_ConnectionStatusChanged;
                BluetoothLEDevice.NameChanged += BluetoothLEDevice_NameChanged;

                IsPaired = DeviceInfo.Pairing.IsPaired;
                IsConnected = BluetoothLEDevice.ConnectionStatus == BluetoothConnectionStatus.Connected;
                Name = BluetoothLEDevice.Name;

                // Get all the services for this device
                var getGattServicesAsyncTokenSource = new CancellationTokenSource(5000);
                var getGattServicesAsyncTask = await
                    Task.Run(
                        () => BluetoothLEDevice.GetGattServicesAsync(BluetoothCacheMode.Uncached),
                        getGattServicesAsyncTokenSource.Token);

                _result = await getGattServicesAsyncTask;

                if (_result.Status == GattCommunicationStatus.Success)
                {
                    foreach (var serv in _result.Services)
                    {
                        Services.Add(new ObservableGattDeviceService(serv));
                    }

                    ServiceCount = Services.Count;
                }
                else
                {
                    if (_result.ProtocolError != null)
                    {
                        throw new Exception(_result.ProtocolError.GetErrorString());
                    }
                }
            });
        }

        public async Task DoInAppPairing()
        {
            var result = await DeviceInfo.Pairing.PairAsync();

            if (result.Status != DevicePairingResultStatus.Paired ||
                result.Status != DevicePairingResultStatus.AlreadyPaired)
            {
                throw new Exception(result.Status.ToString());
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
                CoreDispatcherPriority.Normal,
                () => { Name = BluetoothLEDevice.Name; });
        }

        /// <summary>
        /// Executes when the connection state changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private async void BluetoothLEDevice_ConnectionStatusChanged(BluetoothLEDevice sender, object args)
        {
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(
                CoreDispatcherPriority.Normal,
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
                CoreDispatcherPriority.Normal,
                async () =>
                {
                    var deviceThumbnail = await DeviceInfo.GetGlyphThumbnailAsync();
                    var glyphBitmapImage = new BitmapImage();
                    await glyphBitmapImage.SetSourceAsync(deviceThumbnail);
                    Glyph = glyphBitmapImage;
                });
        }

        /// <summary>
        /// Property changed event invoker
        /// </summary>
        /// <param name="propertyName">name of the property that changed</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Overrides the ToString function to return the name of the device
        /// </summary>
        /// <returns>Name of this characteristic</returns>
        public override string ToString()
        {
            return Name;
        }

        /// <summary>
        /// Updates this device's deviceInformation
        /// </summary>
        /// <param name="deviceUpdate"></param>
        public async Task Update(DeviceInformationUpdate deviceUpdate)
        {
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(
                CoreDispatcherPriority.Normal,
                () =>
                {
                    DeviceInfo.Update(deviceUpdate);
                    Name = DeviceInfo.Name;

                    IsPaired = DeviceInfo.Pairing.IsPaired;

                    LoadGlyph();
                    OnPropertyChanged(new PropertyChangedEventArgs("DeviceInfo"));
                });
        }
    }
}