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
    SelectionMode = RemoteDevicesSelectionMode.Multiple
};
var result = await remoteDevicePicker.PickDeviceAsync();
await new MessageDialog($"You picked {result.Count.ToString()} Device(s)" + Environment.NewLine + string.Join(",", result.Select(x => x.DisplayName.ToString()).ToList())).ShowAsync();
```

## Properties

| Property | Type | Description |
| -- | -- | -- |
| SelectionMode | RemoteDevicesSelectionMode | Gets or sets the DeviceList Selection Mode. Defaults to RemoteDevicesSelectionMode.Single |
| ShowAdvancedFilters | Boolean | Gets or sets a value indicating whether Advanced Filters visible or not |

## Sample Code

[RemoteDevicePicker Sample Page Source](https://github.com/Microsoft/UWPCommunityToolkit/tree/master/Microsoft.Toolkit.Uwp.SampleApp/SamplePages/RemoteDevicePicker). You can see this in action in [Windows Community Toolkit Sample App](https://www.microsoft.com/store/apps/9NBLGGH4TLCQ).

## Default Template 

[RemoteDevicePicker XAML File](https://github.com/Microsoft/UWPCommunityToolkit/blob/master/Microsoft.Toolkit.Uwp.UI.Controls/RemoteDevicePicker/RemoteDevicePicker.xaml) is the XAML template used in the toolkit for the default styling.

## Requirements

| Device family | Universal, 10.0.15063.0 or higher |
| --- | --- |
| Namespace | Microsoft.Toolkit.Uwp.UI.Controls |
| NuGet package | [Microsoft.Toolkit.Uwp.UI.Controls](https://www.nuget.org/packages/Microsoft.Toolkit.Uwp.UI.Controls/) |

## API

* [RemoteDevicePicker source code](https://github.com/Microsoft/UWPCommunityToolkit/tree/master/Microsoft.Toolkit.Uwp.UI.Controls/RemoteDevicePicker)

## Related Topics

* [Project Rome](https://developer.microsoft.com/en-us/windows/project-rome)
* [Remote Systems Sample](https://github.com/Microsoft/Windows-universal-samples/tree/master/Samples/RemoteSystems)