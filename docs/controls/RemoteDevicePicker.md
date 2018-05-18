---
title: RemoteDevicePicker Control
author: avknaidu
description: The RemoteDevicePicker control helps you to pick a device that you can use to Remote Launch Apps, Services.
keywords: windows 10, uwp, windows community toolkit, uwp community toolkit, uwp toolkit, RemoteDevicePicker, picker
---

# RemoteDevicePicker Control 

The [RemoteDevicePicker](https://docs.microsoft.com/dotnet/api/microsoft.toolkit.uwp.ui.controls.remotedevicepicker) gives you a list of Remote Systems. All the systems must be signed in with the same Microsoft Account (MSA)

**Make sure you enable "RemoteSystem" Capability in `package.appxmanifest`**

## Syntax

```c#
RemoteDevicePicker remoteDevicePicker = new RemoteDevicePicker()
{
    Title = "Pick Remote Device",
    DeviceListSelectionMode = ListViewSelectionMode.Single
};
remoteDevicePicker.RemoteDevicePickerClosed += RemoteDevicePicker_RemoteDevicePickerClosed;
await remoteDevicePicker.ShowAsync();
```

## Properties

| Property | Type | Description |
| -- | -- | -- |
| DeviceListSelectionMode | ListViewSelectionMode | Gets or sets the DeviceList Selection Mode. Defaults to ListViewSelectionMode.Single |
| HeaderLineColor | Brush | Gets or sets the Line Color on control Header. takes **SystemControlBackgroundAccentBrush** by default |

## Events

| Events | Description |
| -- | -- |
| RemoteDevicePickerClosed | Fired when the Remote Device Picker is Closed. |

### RemoteDevicePickerClosed

Use this event to get the list of devices selected from RemoteDevicePicker

```c#
private async void RemoteDevicePicker_RemoteDevicePickerClosed(object sender, RemoteDevicePickerEventArgs e)
{
    await new MessageDialog($"You picked {e.Devices.Count.ToString()} Device(s)" + Environment.NewLine + string.Join(",", e.Devices.Select(x => x.DisplayName.ToString()).ToList())).ShowAsync();
}
```

## Sample Code

[RemoteDevicePicker Sample Page Source](https://github.com/Microsoft/UWPCommunityToolkit/tree/master/Microsoft.Toolkit.Uwp.SampleApp/SamplePages/RemoteDevicePicker). You can see this in action in [Windows Community Toolkit Sample App](https://www.microsoft.com/store/apps/9NBLGGH4TLCQ).

## Default Template 

[RemoteDevicePicker XAML File](https://github.com/Microsoft/UWPCommunityToolkit/blob/master/Microsoft.Toolkit.Uwp.UI.Controls/RemoteDevicePicker/RemoteDevicePicker.xaml) is the XAML template used in the toolkit for the default styling.

## Requirements

| Device family | Universal, 10.0.14393.0 or higher |
| --- | --- |
| Namespace | Microsoft.Toolkit.Uwp.UI.Controls |
| NuGet package | [Microsoft.Toolkit.Uwp.UI.Controls](https://www.nuget.org/packages/Microsoft.Toolkit.Uwp.UI.Controls/) |

## API

* [RemoteDevicePicker source code](https://github.com/Microsoft/UWPCommunityToolkit/tree/master/Microsoft.Toolkit.Uwp.UI.Controls/RemoteDevicePicker)

## Related Topics

* [Project Rome](https://developer.microsoft.com/en-us/windows/project-rome)
* [Remote Systems Sample](https://github.com/Microsoft/Windows-universal-samples/tree/master/Samples/RemoteSystems)