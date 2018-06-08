---
title: BluetoothLEHelper
author: nmetulev
description: The BluetoothLEHelper class provides functionality to easily enumerate, connect to and interact with Bluetooth LE Peripherals. 
keywords: windows 10, uwp, windows community toolkit, uwp community toolkit, uwp toolkit, BluetoothLEHelper, bluetooth le, bluetooth
dev_langs:
  - csharp
  - vb
---

# BluetoothLEHelper

The BluetoothLEHelper class provides functionality to easily enumerate, connect to and interact with Bluetooth LE Peripherals. 

## BluetoothLEHelper class

### Properties

| Property | Type | Description |
| -- | -- | -- |
| BluetoothLeDevices | ObservableCollection<ObservableBluetoothLEDevice> | Gets the list of available bluetooth devices |
| IsEnumerating | bool | Gets a value indicating whether app is currently enumerating |
| IsPeripheralRoleSupported | bool | Gets a value indicating whether peripheral mode is supported by this device |
| IsCentralRoleSupported | bool | Gets a value indicating whether central role is supported by this device |

### Methods

| Methods | Return Type | Description |
| -- | -- | -- |
| StartEnumeration() | void | Starts enumeration of bluetooth le devices |
| StopEnumeration() | void | Stops enumeration of bluetooth device |

### Events

| Events | Description |
| -- | -- |
| EnumerationCompleted | An event for when the enumeration is complete |

## ObservableBluetoothLEDevice class

### Properties

| Property | Type | Description |
| -- | -- | -- |
| BluetoothAddressAsString | string | Gets the bluetooth address of this device as a string |
| BluetoothAddressAsUlong | ulong | Gets the bluetooth address of this device |
| BluetoothLEDevice | BluetoothLEDevice | Gets the base bluetooth device this class wraps |
| DeviceInfo | [DeviceInformation](https://docs.microsoft.com/uwp/api/Windows.Devices.Enumeration.DeviceInformation) | Gets the device information for the device this class wraps |
| ErrorText | string | Gets the error text when connecting to this device fails |
| Glyph | BitmapImage | Gets or sets the glyph of this bluetooth device |
| IsConnected | bool | Gets a value indicating whether this device is connected |
| IsPaired | bool | Gets a value indicating whether this device is paired |
| Name | string | Gets the name of this device |
| RSSI | int | Gets the RSSI value of this device |
| Services | ObservableCollection<ObservableGattDeviceService> | Gets the services this device supports |
| ServiceCount | int | Gets or sets the number of services this device has |

### Methods

| Methods | Return Type | Description |
| -- | -- | -- |
| ConnectAsync() | Task | ConnectAsync to this bluetooth device |
| DoInAppPairingAsync() | Task | Does the in application pairing |
| UpdateAsync(DeviceInformationUpdate) | Task | Updates this device's deviceInformation |
| ToString() | string | Overrides the ToString function to return the name of the device |

## ObservableGattDeviceService

### Properties

| Property | Type | Description |
| -- | -- | -- |
| Characteristics | ObservableCollection<ObservableGattCharacteristics> | Gets all the characteristics of this service |
| Name | string | Gets the name of this service |
| UUID | string | Gets the UUID of this service |
| Service | GattDeviceService | Gets the service this class wraps |

## ObservableGattCharacteristics

### Properties

| Property | Type | Description |
| -- | -- | -- |
| Characteristic | GattCharacteristic | Gets or sets the characteristic this class wraps |
| IsIndicateSet | bool | Gets a value indicating whether indicate is set |
| IsNotifySet | bool | Gets a value indicating whether notify is set |
| Parent | ObservableGattDeviceService | Gets or sets the parent service of this characteristic |
| Name | string | Gets or sets the name of this characteristic |
| UUID | string | Gets or sets the UUID of this characteristic |
| Value | string | Gets the value of this characteristic |
| DisplayType | DisplayTypes | Gets or sets how this characteristic's value should be displayed |

### Methods

| Methods | Return Type | Description |
| -- | -- | -- |
| ReadValueAsync() | Task<string> | Reads the value of the Characteristic |
| SetIndicateAsync() | Task<bool> | Set's the indicate descriptor |
| StopIndicateAsync() | Task<bool> | Unset the indicate descriptor |
| SetNotifyAsync() | Task<bool> | Sets the notify characteristic |
| StopNotifyAsync() | Task<bool> | Unsets the notify descriptor |

## Example

```csharp
// Get a local copy of the context for easier reading
BluetoothLEHelper bluetoothLEHelper = BluetoothLEHelper.Context;

// check if BluetoothLE APIs are available
if (BluetoothLEHelper.IsBluetoothLESupported)
{
    // Start the Enumeration
	bluetoothLEHelper.StartEnumeration();

	// At this point the user needs to select a device they want to connect to. This can be done by
	// creating a ListView and binding the bluetoothLEHelper collection to it. Once a device is found, 
	// the Connect() method can be called to connect to the device and start interacting with its services

	// Connect to a device if your choice
	ObservableBluetoothLEDevice device = bluetoothLEHelper.BluetoothLeDevices[<Device you choose>];
	await device.ConnectAsync();

	// At this point the device is connected and the Services property is populated.

	// See all the services
	var services = device.Services;
}
```
```vb
' Get a local copy of the context for easier reading
Dim bluetoothLEHelper As BluetoothLEHelper = BluetoothLEHelper.Context

' check if BluetoothLE APIs are available
If BluetoothLEHelper.IsBluetoothLESupported Then
    ' Start the Enumeration
	bluetoothLEHelper.StartEnumeration()

	' At this point the user needs to select a device they want to connect to. This can be done by
	' creating a ListView and binding the bluetoothLEHelper collection to it. Once a device is found, 
	' the Connect() method can be called to connect to the device and start interacting with its services

	' Connect to a device if your choice
	Dim device As ObservableBluetoothLEDevice = bluetoothLEHelper.BluetoothLeDevices(<Device you choose>)
	Await device.ConnectAsync()

	' At this point the device is connected and the Services property is populated.

	' See all the services
	Dim services = device.Services
End If
```

## Requirements

| Device family | Universal, 10.0.15063.0 or higher |
| --- | --- |
| Namespace | Microsoft.Toolkit.Uwp.Connectivity |
| NuGet package | [Microsoft.Toolkit.Uwp](https://www.nuget.org/packages/Microsoft.Toolkit.Uwp/) |

## API

* [BluetoothLEHelper source code](https://github.com/Microsoft/WindowsCommunityToolkit//tree/master/Microsoft.Toolkit.Uwp.Connectivity/BluetoothLEHelper)
