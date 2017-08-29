---
title: SystemInformation
author: nmetulev
ms.date: 08/20/2017
description: The SystemInformation is a static utility class that provides properties with some system, application and device information.
keywords: windows 10, uwp, uwp community toolkit, uwp toolkit, SystemInformation
---

# SystemInformation

The SystemInformation is a static utility class that provides properties with some system, application and device information.


## Properties

| Property | Purpose |
| --- | --- |
|ApplicationName | Gets the application's name as a _string_ |
|ApplicationVersion | Gets the application's version as a [PackageVersion](https://msdn.microsoft.com/en-us/library/windows/apps/xaml/windows.applicationmodel.packageversion.aspx) |
|Culture | Gets the most preferred language by the user as a [CultureInfo](https://msdn.microsoft.com/en-us/library/windows/apps/xaml/system.globalization.cultureinfo(v=vs.105).aspx) |
|DeviceFamily | Gets the family of used device as a _string_ |
|OperatingSystem | Gets the operating system as a _string_ |
|OperatingSystemVersion | Gets the operating system version (for example 10.0.10586.0) as _OSVersion_ structure |
|OperatingSystemArchitecture | Gets used processor architecture as [ProcessorArchitecture](https://msdn.microsoft.com/en-us/library/windows/apps/windows.system.processorarchitecture) |
|AvailableMemory | Gets the available memory in _MB_ as a _float_ |
|DeviceModel | Gets the model of the device as a _string_ |
|DeviceManufacturer | Gets the name of device manufacturer as a _string_ |

## Requirements (Windows 10 Device Family)

| [Device family](http://go.microsoft.com/fwlink/p/?LinkID=526370) | Universal, 10.0.14393.0 or higher |
| --- | --- |
| Namespace | Microsoft.Toolkit.Uwp |

## API
* [SystemInformation source code](https://github.com/Microsoft/UWPCommunityToolkit/blob/master/Microsoft.Toolkit.Uwp/Helpers/SystemInformation.cs)
