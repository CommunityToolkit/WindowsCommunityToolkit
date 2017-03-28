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
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using Windows.Security.Cryptography;
using Windows.Storage.Streams;

namespace Microsoft.Toolkit.Uwp
{
    /// <summary>
    /// Wrapper around <see cref="GattCharacteristic"/>  to make it easier to use
    /// </summary>
    public class ObservableGattCharacteristics : INotifyPropertyChanged
    {
        /// <summary>
        /// Enum used to determine how the <see cref="Value"/> should be displayed
        /// </summary>
        public enum DisplayTypes
        {
            NotSet,
            Bool,
            Decimal,
            Hex,
            UTF8,
            UTF16,
            Unsupported
        }

        /// <summary>
        /// Source for <see cref="Characteristic"/>
        /// </summary>
        private GattCharacteristic _characteristic;

        /// <summary>
        /// byte array representation of the characteristic value
        /// </summary>
        private byte[] _data;

        /// <summary>
        /// Source for <see cref="IsIndicateSet"/>
        /// </summary>
        private bool _isIndicateSet;

        /// <summary>
        /// Source for <see cref="IsNotifySet"/>
        /// </summary>
        private bool _isNotifySet;

        /// <summary>
        /// Raw buffer of this value of this characteristic
        /// </summary>
        private IBuffer _rawData;

        /// <summary>
        /// Source for <see cref="DisplayType"/>
        /// </summary>
        private DisplayTypes _displayType = DisplayTypes.NotSet;

        /// <summary>
        /// Source for <see cref="Name"/>
        /// </summary>
        private string _name;

        /// <summary>
        /// Source for <see cref="Parent"/>
        /// </summary>
        private ObservableGattDeviceService _parent;

        /// <summary>
        /// Source for <see cref="UUID"/>
        /// </summary>
        private string _uuid;

        /// <summary>
        /// Source for <see cref="Value"/>
        /// </summary>
        private string _value;

        /// <summary>
        /// Initializes a new instance of the<see cref="ObservableGattCharacteristics" /> class.
        /// </summary>
        /// <param name="characteristic">Characteristic this class wraps</param>
        /// <param name="parent">The parent service that wraps this characteristic</param>
        public ObservableGattCharacteristics(GattCharacteristic characteristic, ObservableGattDeviceService parent)
        {
            Characteristic = characteristic;
            Parent = parent;
            Name = GattUuidsService.ConvertUuidToName(Characteristic.Uuid);
            UUID = Characteristic.Uuid.ToString();

            ReadValueAsync();

            characteristic.ValueChanged += Characteristic_ValueChanged;

            PropertyChanged += ObservableGattCharacteristics_PropertyChanged;
        }

        /// <summary>
        /// Gets or sets the characteristic this class wraps
        /// </summary>
        public GattCharacteristic Characteristic
        {
            get { return _characteristic; }

            set
            {
                if (_characteristic != value)
                {
                    _characteristic = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("Characteristic"));
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether indicate is set
        /// </summary>
        public bool IsIndicateSet
        {
            get { return _isIndicateSet; }

            set
            {
                if (_isIndicateSet != value)
                {
                    _isIndicateSet = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("IsIndicateSet"));
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether notify is set
        /// </summary>
        public bool IsNotifySet
        {
            get { return _isNotifySet; }

            set
            {
                if (_isNotifySet != value)
                {
                    _isNotifySet = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("IsNotifySet"));
                }
            }
        }

        /// <summary>
        /// Gets or sets the parent service of this characteristic
        /// </summary>
        public ObservableGattDeviceService Parent
        {
            get { return _parent; }

            set
            {
                if (_parent != value)
                {
                    _parent = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("Parent"));
                }
            }
        }

        /// <summary>
        /// Gets or sets the name of this characteristic
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
        /// Gets or sets the UUID of this characteristic
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
        /// Gets the value of this characteristic
        /// </summary>
        public string Value
        {
            get { return _value; }

            private set
            {
                if (_value != value)
                {
                    _value = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("Value"));
                }
            }
        }

        /// <summary>
        /// Gets or sets how this characteristic's value should be displayed
        /// </summary>
        public DisplayTypes DisplayType
        {
            get { return _displayType; }

            set
            {
                if (value == DisplayTypes.NotSet)
                {
                    return;
                }

                if (_displayType != value)
                {
                    _displayType = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("DisplayType"));
                }
            }
        }

        /// <summary>
        /// Event to notify when this object has changed
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Executes when this characteristic changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ObservableGattCharacteristics_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "DisplayType")
            {
                SetValue();
            }
        }

        /// <summary>
        /// Reads the value of the Characteristic
        /// </summary>
        public async void ReadValueAsync()
        {
            try
            {
                var result = await Characteristic.ReadValueAsync(BluetoothCacheMode.Uncached);

                if (result.Status == GattCommunicationStatus.Success)
                {
                    SetValue(result.Value);
                }
                else if (result.Status == GattCommunicationStatus.ProtocolError)
                {
                    Value = GattProtocolErrorParser.GetErrorString(result.ProtocolError);
                }
                else
                {
                    Value = "Unreachable";
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception: " + ex.Message);
                Value = "Exception!";
            }
        }

        /// <summary>
        /// Set's the indicate descriptor
        /// </summary>
        /// <returns>Set indicate task</returns>
        public async Task<bool> SetIndicate()
        {
            if (IsIndicateSet)
            {
                // already set
                return true;
            }

            try
            {
                // BT_Code: Must write the CCCD in order for server to send indications.
                // We receive them in the ValueChanged event handler.
                // Note that this sample configures either Indicate or Notify, but not both.
                var result = await
                    Characteristic.WriteClientCharacteristicConfigurationDescriptorAsync(
                        GattClientCharacteristicConfigurationDescriptorValue.Indicate);
                if (result == GattCommunicationStatus.Success)
                {
                    Debug.WriteLine("Successfully registered for indications");
                    IsIndicateSet = true;
                    return true;
                }
                if (result == GattCommunicationStatus.ProtocolError)
                {
                    Debug.WriteLine("Error registering for indications: Protocol Error");
                    IsIndicateSet = false;
                    return false;
                }
                if (result == GattCommunicationStatus.Unreachable)
                {
                    Debug.WriteLine("Error registering for indications: Unreachable");
                    IsIndicateSet = false;
                    return false;
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                // This usually happens when a device reports that it support indicate, but it actually doesn't.
                Debug.WriteLine("Unauthorized Exception: " + ex.Message);
                IsIndicateSet = false;
                return false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Generic Exception: " + ex.Message);
                IsIndicateSet = false;
                return false;
            }

            IsIndicateSet = false;
            return false;
        }

        /// <summary>
        /// Unsets the indicate descriptor
        /// </summary>
        /// <returns>Unset indicate task</returns>
        public async Task<bool> StopIndicate()
        {
            if (IsIndicateSet == false)
            {
                // indicate is not set, can skip this
                return true;
            }

            try
            {
                // BT_Code: Must write the CCCD in order for server to send indications.
                // We receive them in the ValueChanged event handler.
                // Note that this sample configures either Indicate or Notify, but not both.
                var result = await
                    Characteristic.WriteClientCharacteristicConfigurationDescriptorAsync(
                        GattClientCharacteristicConfigurationDescriptorValue.None);
                if (result == GattCommunicationStatus.Success)
                {
                    Debug.WriteLine("Successfully un-registered for indications");
                    IsIndicateSet = false;
                    return true;
                }
                if (result == GattCommunicationStatus.ProtocolError)
                {
                    Debug.WriteLine("Error un-registering for indications: Protocol Error");
                    IsIndicateSet = true;
                    return false;
                }
                if (result == GattCommunicationStatus.Unreachable)
                {
                    Debug.WriteLine("Error un-registering for indications: Unreachable");
                    IsIndicateSet = true;
                    return false;
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                // This usually happens when a device reports that it support indicate, but it actually doesn't.
                Debug.WriteLine("Exception: " + ex.Message);
                IsIndicateSet = true;
                return false;
            }

            return false;
        }

        /// <summary>
        /// Sets the notify characteristic
        /// </summary>
        /// <returns>Set notify task</returns>
        public async Task<bool> SetNotify()
        {
            if (IsNotifySet)
            {
                // already set
                return true;
            }

            try
            {
                // BT_Code: Must write the CCCD in order for server to send indications.
                // We receive them in the ValueChanged event handler.
                // Note that this sample configures either Indicate or Notify, but not both.
                var result = await
                    Characteristic.WriteClientCharacteristicConfigurationDescriptorAsync(
                        GattClientCharacteristicConfigurationDescriptorValue.Notify);
                if (result == GattCommunicationStatus.Success)
                {
                    Debug.WriteLine("Successfully registered for notifications");
                    IsNotifySet = true;
                    return true;
                }
                if (result == GattCommunicationStatus.ProtocolError)
                {
                    Debug.WriteLine("Error registering for notifications: Protocol Error");
                    IsNotifySet = false;
                    return false;
                }
                if (result == GattCommunicationStatus.Unreachable)
                {
                    Debug.WriteLine("Error registering for notifications: Unreachable");
                    IsNotifySet = false;
                    return false;
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                // This usually happens when a device reports that it support indicate, but it actually doesn't.
                Debug.WriteLine("Unauthorized Exception: " + ex.Message);
                IsNotifySet = false;
                return false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Generic Exception: " + ex.Message);
                IsNotifySet = false;
                return false;
            }

            IsNotifySet = false;
            return false;
        }

        /// <summary>
        /// Unsets the notify descriptor
        /// </summary>
        /// <returns>Unset notify task</returns>
        public async Task<bool> StopNotify()
        {
            if (IsNotifySet == false)
            {
                // indicate is not set, can skip this
                return true;
            }

            try
            {
                // BT_Code: Must write the CCCD in order for server to send indications.
                // We receive them in the ValueChanged event handler.
                // Note that this sample configures either Indicate or Notify, but not both.
                var result = await
                    Characteristic.WriteClientCharacteristicConfigurationDescriptorAsync(
                        GattClientCharacteristicConfigurationDescriptorValue.None);
                if (result == GattCommunicationStatus.Success)
                {
                    Debug.WriteLine("Successfully un-registered for notifications");
                    IsNotifySet = false;
                    return true;
                }
                if (result == GattCommunicationStatus.ProtocolError)
                {
                    Debug.WriteLine("Error un-registering for notifications: Protocol Error");
                    IsNotifySet = true;
                    return false;
                }
                if (result == GattCommunicationStatus.Unreachable)
                {
                    Debug.WriteLine("Error un-registering for notifications: Unreachable");
                    IsNotifySet = true;
                    return false;
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                // This usually happens when a device reports that it support indicate, but it actually doesn't.
                Debug.WriteLine("Exception: " + ex.Message);
                IsNotifySet = true;
                return false;
            }

            return false;
        }

        /// <summary>
        /// Executes when value changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private async void Characteristic_ValueChanged(GattCharacteristic sender, GattValueChangedEventArgs args)
        {
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(
                Windows.UI.Core.CoreDispatcherPriority.Normal,
                () => { SetValue(args.CharacteristicValue); });
        }

        /// <summary>
        /// helper function that copies the raw data into byte array
        /// </summary>
        /// <param name="buffer">The raw input buffer</param>
        private void SetValue(IBuffer buffer)
        {
            _rawData = buffer;
            CryptographicBuffer.CopyToByteArray(_rawData, out _data);

            SetValue();
        }

        /// <summary>
        /// Sets the value of this characteristic based on the display type
        /// </summary>
        private void SetValue()
        {
            if (_data == null)
            {
                Value = "NULL";
                return;
            }

            GattPresentationFormat format = null;

            if (Characteristic.PresentationFormats.Count > 0)
            {
                format = Characteristic.PresentationFormats[0];
            }

            // Determine what to set our DisplayType to
            if (format == null && DisplayType == DisplayTypes.NotSet)
            {
                if (Name == "DeviceName")
                {
                    // All devices have DeviceName so this is a special case. 
                    DisplayType = DisplayTypes.UTF8;
                }
                else
                {
                    var isString = true;
                    var buffer = Encoding.UTF8.GetString(_data);

                    // if buffer is only 1 char or 2 char with 0 at end then let's assume it's hex
                    if (buffer.Length == 1)
                    {
                        isString = false;
                    }
                    else if (buffer.Length == 2 && buffer[1] == 0)
                    {
                        isString = false;
                    }
                    else
                    {
                        foreach (var b in buffer)
                        {
                            // if within the reasonable range of used characters and not null, let's assume it's a UTF8 string by default, else hex
                            if ((b < ' ' || b > '~') && b != 0)
                            {
                                isString = false;
                                break;
                            }
                        }
                    }

                    if (isString)
                    {
                        DisplayType = DisplayTypes.UTF8;
                    }
                    else
                    {
                        // By default, display as Hex
                        DisplayType = DisplayTypes.Hex;
                    }
                }
            }
            else if (format != null && DisplayType == DisplayTypes.NotSet)
            {
                if (format.FormatType == GattPresentationFormatTypes.Boolean ||
                    format.FormatType == GattPresentationFormatTypes.Bit2 ||
                    format.FormatType == GattPresentationFormatTypes.Nibble ||
                    format.FormatType == GattPresentationFormatTypes.UInt8 ||
                    format.FormatType == GattPresentationFormatTypes.UInt12 ||
                    format.FormatType == GattPresentationFormatTypes.UInt16 ||
                    format.FormatType == GattPresentationFormatTypes.UInt24 ||
                    format.FormatType == GattPresentationFormatTypes.UInt32 ||
                    format.FormatType == GattPresentationFormatTypes.UInt48 ||
                    format.FormatType == GattPresentationFormatTypes.UInt64 ||
                    format.FormatType == GattPresentationFormatTypes.SInt8 ||
                    format.FormatType == GattPresentationFormatTypes.SInt12 ||
                    format.FormatType == GattPresentationFormatTypes.SInt16 ||
                    format.FormatType == GattPresentationFormatTypes.SInt24 ||
                    format.FormatType == GattPresentationFormatTypes.SInt32)
                {
                    DisplayType = DisplayTypes.Decimal;
                }
                else if (format.FormatType == GattPresentationFormatTypes.Utf8)
                {
                    DisplayType = DisplayTypes.UTF8;
                }
                else if (format.FormatType == GattPresentationFormatTypes.Utf16)
                {
                    DisplayType = DisplayTypes.UTF16;
                }
                else if (format.FormatType == GattPresentationFormatTypes.UInt128 ||
                         format.FormatType == GattPresentationFormatTypes.SInt128 ||
                         format.FormatType == GattPresentationFormatTypes.DUInt16 ||
                         format.FormatType == GattPresentationFormatTypes.SInt64 ||
                         format.FormatType == GattPresentationFormatTypes.Struct ||
                         format.FormatType == GattPresentationFormatTypes.Float ||
                         format.FormatType == GattPresentationFormatTypes.Float32 ||
                         format.FormatType == GattPresentationFormatTypes.Float64)
                {
                    DisplayType = DisplayTypes.Unsupported;
                }
                else
                {
                    DisplayType = DisplayTypes.Unsupported;
                }
            }

            // Decode the value into the right display type
            if (DisplayType == DisplayTypes.Hex || DisplayType == DisplayTypes.Unsupported)
            {
                Value = CryptographicBuffer.EncodeToHexString(_rawData);
            }
            else if (DisplayType == DisplayTypes.Decimal)
            {
                byte[] buf = BytePadder.GetBytes(_data, 8);
                Value = BitConverter.ToUInt64(buf, 0).ToString();
            }
            else if (DisplayType == DisplayTypes.UTF8)
            {
                Value = Encoding.UTF8.GetString(_data);
            }
            else if (DisplayType == DisplayTypes.UTF16)
            {
                Value = Encoding.Unicode.GetString(_data);
            }
        }

        /// <summary>
        /// Executes when this class changes
        /// </summary>
        /// <param name="e"></param>
        private void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "DisplayType")
            {
                Debug.WriteLine($"{Name} - DisplayType set: {DisplayType}");
            }

            PropertyChanged?.Invoke(this, e);
        }
    }
}