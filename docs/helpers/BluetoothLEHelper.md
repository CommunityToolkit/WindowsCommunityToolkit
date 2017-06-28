# BluetoothLEHelper
The BluetoothLEHelper class provides functionality to easily enumerate, connect to and interact with Bluetooth LE Peripherals. 

## Example

```csharp
// Get a local copy of the context for easier reading
BluetoothLEHelper bluetoothLEHelper = BluetoothLEHelper.Context;

// Start the Enumeration
bluetoothLEHelper.StartEnumeration();

// Connect to a device if your choice
ObservableBluetoothLEDevice device = 
	bluetoothLEHelper.BluetoothLeDevices[<Device you choose>].Connect()

// See all the services
var services = device.Services

```

## Requirements (Windows 10 Device Family)

| [Device family](http://go.microsoft.com/fwlink/p/?LinkID=526370) | Universal, 10.0.15063.0 or higher |
| --- | --- |
| Namespace | Microsoft.Toolkit.Uwp |

## API

* [BluetoothLEHelper source code](https://github.com/Microsoft/UWPCommunityToolkit/blob/master/Microsoft.Toolkit.Uwp/BluetoothLEHelper/)


