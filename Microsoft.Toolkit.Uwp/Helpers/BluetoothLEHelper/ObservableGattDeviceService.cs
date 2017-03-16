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
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth.GenericAttributeProfile;

namespace Microsoft.Toolkit.Uwp
{
    /// <summary>
    /// Wrapper around <see cref="GattDeviceService"/> to make it easier to use
    /// </summary>
    public class ObservableGattDeviceService : INotifyPropertyChanged
    {
        /// <summary>
        /// Source for <see cref="Characteristics"/>
        /// </summary>
        private ObservableCollection<ObservableGattCharacteristics> _characteristics =
            new ObservableCollection<ObservableGattCharacteristics>();

        /// <summary>
        /// Determines if the SelectedCharacteristic_PropertyChanged has been added
        /// </summary>
        private bool _hasSelectedCharacteristicPropertyChangedHandler;

        /// <summary>
        /// Source for <see cref="Name"/>
        /// </summary>
        private string _name;

        /// <summary>
        /// Source for <see cref="SelectedCharacteristic"/>
        /// </summary>
        private ObservableGattCharacteristics _selectedCharacteristic;

        /// <summary>
        /// Source for <see cref="Service"/>
        /// </summary>
        private GattDeviceService _service;

        /// <summary>
        /// Source for <see cref="UUID"/>
        /// </summary>
        private string _uuid;

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableGattDeviceService" /> class.
        /// </summary>
        /// <param name="service">The service this class wraps</param>
        public ObservableGattDeviceService(GattDeviceService service)
        {
            Service = service;
            Name = GattUuidsService.ConvertUuidToName(service.Uuid);
            UUID = Service.Uuid.ToString();
            GetAllCharacteristics();
        }

        /// <summary>
        /// Gets or sets the service this class wraps
        /// </summary>
        public GattDeviceService Service
        {
            get { return _service; }

            set
            {
                if (_service != value)
                {
                    _service = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("Service"));
                }
            }
        }

        /// <summary>
        /// Gets or sets all the characteristics of this service
        /// </summary>
        public ObservableCollection<ObservableGattCharacteristics> Characteristics
        {
            get { return _characteristics; }

            set
            {
                if (_characteristics != value)
                {
                    _characteristics = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("Characteristics"));
                }
            }
        }

        /// <summary>
        /// Gets or sets the currently selected characteristic
        /// </summary>
        public ObservableGattCharacteristics SelectedCharacteristic
        {
            get { return _selectedCharacteristic; }

            set
            {
                if (_selectedCharacteristic != value)
                {
                    _selectedCharacteristic = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("SelectedCharacteristic"));

                    // The SelectedProperty doesn't exist when this object is first created. This takes
                    // care of adding the correct event handler after the first time it's changed. 
                    SelectedCharacteristic_PropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the name of this service
        /// </summary>
        public string Name
        {
            get { return _name; }

            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("Name"));
                }
            }
        }

        /// <summary>
        /// Gets or sets the UUID of this service
        /// </summary>
        public string UUID
        {
            get { return _uuid; }

            set
            {
                if (_uuid != value)
                {
                    _uuid = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("UUID"));
                }
            }
        }

        /// <summary>
        /// Event to notify when this object has changed
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Adds the SelectedCharacteristic_PropertyChanged event handler
        /// </summary>
        private void SelectedCharacteristic_PropertyChanged()
        {
            if (_hasSelectedCharacteristicPropertyChangedHandler == false)
            {
                SelectedCharacteristic.PropertyChanged += SelectedCharacteristic_PropertyChanged;
                _hasSelectedCharacteristicPropertyChangedHandler = true;
            }
        }

        /// <summary>
        /// Updates the selected characteristic in the app context
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectedCharacteristic_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            BluetoothLEHelper.Context.SelectedCharacteristic = SelectedCharacteristic;
        }

        /// <summary>
        /// Gets all the characteristics of this service
        /// </summary>
        private async void GetAllCharacteristics()
        {
            var sb = new StringBuilder();
            sb.Append("ObservableGattDeviceService::getAllCharacteristics: ");
            sb.Append(Name);

            try
            {
                var tokenSource = new CancellationTokenSource(5000);
                var t =
                    Task.Run(
                        () => Service.GetCharacteristicsAsync(Windows.Devices.Bluetooth.BluetoothCacheMode.Uncached),
                        tokenSource.Token);

                GattCharacteristicsResult result = null;
                result = await t.Result;

                if (result.Status == GattCommunicationStatus.Success)
                {
                    sb.Append(" - getAllCharacteristics found ");
                    sb.Append(result.Characteristics.Count());
                    sb.Append(" characteristics");
                    Debug.WriteLine(sb);
                    foreach (GattCharacteristic gattchar in result.Characteristics)
                    {
                        Characteristics.Add(new ObservableGattCharacteristics(gattchar, this));
                    }
                }
                else if (result.Status == GattCommunicationStatus.Unreachable)
                {
                    sb.Append(" - getAllCharacteristics failed with Unreachable");
                    Debug.WriteLine(sb.ToString());
                }
                else if (result.Status == GattCommunicationStatus.ProtocolError)
                {
                    sb.Append(" - getAllCharacteristics failed with Unreachable");
                    Debug.WriteLine(sb.ToString());
                }
            }
            catch (AggregateException ae)
            {
                foreach (var ex in ae.InnerExceptions)
                {
                    if (ex is TaskCanceledException)
                    {
                        Debug.WriteLine("Getting characteristics took too long.");
                        Name += " - Timed out getting some characteristics";
                        return;
                    }
                }
            }
            catch (UnauthorizedAccessException)
            {
                // Bug 9145823:GetCharacteristicsAsync throw System.UnauthorizedAccessException when querying GenericAccess Service Characteristics
                Name += " - Unauthorized Access";
            }
            catch (Exception ex)
            {
                Debug.WriteLine("getAllCharacteristics: Exception - {0}" + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Property changed
        /// </summary>
        /// <param name="e"></param>
        private void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, e);
        }
    }
}