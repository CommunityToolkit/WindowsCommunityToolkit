---
title: BluetoothLEHelper
author: nmetulev
ms.date: 08/20/2017
description: The BluetoothLEHelper class provides functionality to easily enumerate, connect to and interact with Bluetooth LE Peripherals. 
keywords: windows 10, uwp, uwp community toolkit, uwp toolkit, BluetoothLEHelper, bluetooth le, bluetooth
---

# BluetoothLEHelper
The BluetoothLEHelper class provides functionality to easily enumerate, connect to and interact with Bluetooth LE Peripherals. 

## BluetoothLEHelper class
### Properties
#### BluetoothLeDevices
Gets the list of available bluetooth devices

#### IsEnumerating
Gets a value indicating whether app is currently enumerating

#### IsPeripheralRoleSupported
Gets a value indicating whether peripheral mode is supported by this device

#### IsCentralRoleSupported
Gets a value indicating whether central role is supported by this device


### Events
#### EnumerationCompleted
An event for when the enumeration is complete

### Methods
#### StartEnumeration
Starts enumeration of bluetooth le devices

#### StopEnumeration
Stops enumeration of bluetooth le devices

## ObservableBluetoothLEDevice class

### Properties
#### BluetoothLEDevice
Gets the base bluetooth device this class wraps

#### Glyph
Gets or sets the glyph of this bluetooth device

#### DeviceInfo
Gets the device information for the device this class wraps

#### IsConnected
Gets a value indicating whether this device is connected

#### isPaired
Gets a value indicating whether this device is paired

#### Services
Gets the services this device supports

#### ServiceCount
Gets or sets the number of services this device has

#### Name
Gets the name of this device

#### ErrorText
Gets the error text when connecting to this device fails

#### BluetoothAddressAsString
Gets the bluetooth address of this device as a string

#### BluetoothAddressAsUlong
Gets the bluetooth address of this device

### Methods
#### ConnectAsync
Connects to this bluetooth device

#### DoInAppPairingAsync
Does the in application pairing

#### UpdateAsync
Updates this device's deviceInformation

#### ToString
Overrides the ToString function to return the name of the device

## ObservableGattDeviceService
### Properties
#### Service
Gets the service this class wraps

#### Characteristics
Gets all the characteristics of this service

#### Name
Gets the name of this service

#### UUID
Gets the UUID of this service

### ObservableGattCharacteristics
### Properties
#### Characteristic
Gets or sets the characteristic this class wraps

#### IsIndicateSet
Gets a value indicating whether indicate is set

#### IsNotifySet
Gets a value indicating whether notify is set

#### Parent
Gets or sets the parent service of this characteristic

#### Name
Gets or sets the name of this characteristic

#### UUID
Gets or sets the UUID of this characteristic

#### Value
Gets the value of this characteristic

#### DisplayType
Gets or sets how this characteristic's value should be displayed

### Methods
#### ReadValueAsync
Reads the value of the Characteristic

#### SetIndicateAsync
Set's the indicate descriptor

#### StopIndicateAsync
Unset the indicate descriptor

#### SetNotifyAsync
Sets the notify characteristic

#### StopNotifyAsync
Unsets the notify descriptor

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

## Requirements (Windows 10 Device Family)

| [Device family](http://go.microsoft.com/fwlink/p/?LinkID=526370) | Universal, 10.0.15063.0 or higher |
| --- | --- |
| Namespace | Microsoft.Toolkit.Uwp |

## API

* [BluetoothLEHelper source code](https://github.com/Microsoft/UWPCommunityToolkit/blob/master/Microsoft.Toolkit.Uwp/Helpers/BluetoothLEHelper/)


