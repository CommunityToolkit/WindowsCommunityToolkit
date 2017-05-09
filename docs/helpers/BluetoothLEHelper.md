# BluetoothLEHelper
The BluetoothLEHelper class provides functionality to easily enumerate, connect to and interact with Bluetooth LE Peripherals. 

## Example

```csharp
// Get a local copy of the context for easier reading
BluetoothLEHelper bluetoothLEHelper = BluetoothLEHelper.Context;

// Subscribe to the PropertyChanged Event
bluetoothLEHelper.PropertyChanged += BluetoothLEHelper_PropertyChanged;

// Start the Enumeration
bluetoothLEHelper.StartEnumeration();

// Connect to a device if your choice
ObservableBluetoothLEDevice device = 
	bluetoothLEHelper.BluetoothLeDevices[<Device you choose>].Connect()

// See all the services
device.Services

// The PropertyChanged event so your UI can be updated with enumeration completed
private async void BluetoothLEHelper_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
{
    await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(
        Windows.UI.Core.CoreDispatcherPriority.Normal,
        () =>
        {
            if (e.PropertyName == "IsEnumerating")
            {
			// Update UI
            }
        });
}
```

## Requirements (Windows 10 Device Family)

| [Device family](http://go.microsoft.com/fwlink/p/?LinkID=526370) | Universal, 10.0.15063.0 or higher |
| --- | --- |
| Namespace | Microsoft.Toolkit.Uwp |

## API

TODO: * [BluetoothLEHelper source code]()

