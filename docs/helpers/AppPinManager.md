---
title: AppPinManager helper
author: rvinothrajendran
ms.date: 11/08/2017
description: The AppPinManager class helps the user add the app shortcut in StartMenu or TaskBar in a easier way
keywords: windows 10, uwp, uwp community toolkit, uwp toolkit, StartMenu, TaskBar, PinManager 
---

# AppPinManager 

The **AppPinManager** is a class used to add the application shortcut in StartMenu or TaskBar.

## Syntax

```csharp
using Microsoft.Toolkit.Uwp.Helpers;
var result = await AppPinManager.PinCurrentAppToTaskBarAsync();
```

## Methods

| Methods | Description |
| -- | -- |
| PinCurrentAppToTaskBarAsync() | Pin the current app shortcut in Windows 10 TaskBar |
| PinSpecificAppToTaskBarAsync(AppListEntry) | Pin the specified app shortcut in Windows 10 TaskBar |
| PinSpecificAppToStartMenuAsync(AppListEntry) | Pin the specified app shortcut in Windows 10 StartMenu |
| PinUserSpecificAppToStartMenuAsync(User, AppListEntry) | Pin the specified app shortcut in Windows 10 StartMenu based on the user |

All the APIs return the `PinResult` enum as a status

| PinResult | Description |
| -- | -- |
| UnsupportedDevice | Unable to pin due to unsupported device |
| UnsupportedOS |  Unable to pin due to unsupported OS |
| PinNotAllowed |  Unable to pin due as pinning is not allowed |
| PinPresent | Successfully pinned |
| PinAlreadyPresent | Pin already exist |
| PinOperationFailed | Pin Operation is failed may be user has canceled|

## Examples

Pinning the specified app shortcut in Windows 10 StartMenu

```csharp
var appList = (await Package.Current.GetAppListEntriesAsync())[0];
var result = await AppPinManager.PinSpecificAppToStartMenuAsync(appList);
```

## Requirements ( StartMenu APIs)

| [Device family](http://go.microsoft.com/fwlink/p/?LinkID=526370) | Universal, 10.0.15063.0 or higher   |
| ---------------------------------------------------------------- | ----------------------------------- |
| Namespace                                                        | Microsoft.Toolkit.Uwp.Helpers |
| NuGet package | [Microsoft.Toolkit.Uwp](https://www.nuget.org/packages/Microsoft.Toolkit.Uwp/) |

## Requirements ( TaskBar APIs)

| [Device family](http://go.microsoft.com/fwlink/p/?LinkID=526370) | Universal, 10.0.16299.0 or higher   |
| ---------------------------------------------------------------- | ----------------------------------- |
| Namespace                                                        | Microsoft.Toolkit.Uwp.Helpers |
| NuGet package | [Microsoft.Toolkit.Uwp](https://www.nuget.org/packages/Microsoft.Toolkit.Uwp/) |


## API

* [AppPinManager source code](https://github.com/Microsoft/UWPCommunityToolkit/blob/master/Microsoft.Toolkit.Uwp/Helpers/AppPinManager/AppPinManager.cs)
