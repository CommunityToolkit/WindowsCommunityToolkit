---
title: NetworkHelper
author: nmetulev
description: he NetworkHelper class provides functionality to monitor changes in network connection and allows users to query for network information without additional lookups.
keywords: windows 10, uwp, windows community toolkit, uwp community toolkit, uwp toolkit, NetworkHelper
dev_langs:
  - csharp
  - vb
---

# NetworkHelper

The [NetworkHelper](https://docs.microsoft.com/dotnet/api/microsoft.toolkit.uwp.connectivity.networkhelper) class provides functionality to monitor changes in network connection and allows users to query for network information without additional lookups.

It exposes network information though a property called ConnectionInformation. The [ConnectionInformation](https://docs.microsoft.comdotnet/api/microsoft.toolkit.uwp.connectivity.connectioninformation) holds information about ConnectionType, ConnectivityLevel, ConnectionCost, SignalStrength, Internet Connectivity and more.

**_What is a metered connection?_**
A metered connection is an Internet connection that has a data limit or cost associated with it. Cellular data connections are set as metered by default. Wi-Fi network connections can be set to metered, but aren't by default. Application developers should take metered nature of connection into account and reduce data usage.

## NetworkHelper Properties

| Property | Type | Description |
| -- | -- | -- |
| ConnectionInformation | [ConnectionInformation](https://docs.microsoft.comdotnet/api/microsoft.toolkit.uwp.connectivity.connectioninformation) | Gets instance of ConnectionInformation |
| Instance | NetworkHelper | Gets public singleton property |

## ConnectionInformation Properties

| Property | Type | Description |
| -- | -- | -- |
| ConnectionCost | [ConnectionCost](https://docs.microsoft.com/uwp/api/Windows.Networking.Connectivity.ConnectionCost) | Gets connection cost for the current Internet Connection Profile |
| ConnectionType | [ConnectionType](https://docs.microsoft.com/dotnet/api/microsoft.toolkit.uwp.connectivity.connectiontype)] | Gets connection type for the current Internet Connection Profile |
| ConnectivityLevel | [NetworkConnectivityLevel](https://docs.microsoft.com/uwp/api/Windows.Networking.Connectivity.NetworkConnectivityLevel) | Gets connectivity level for the current Internet Connection Profile |
| IsInternetAvailable | bool | Gets a value indicating whether internet is available across all connections |
| IsInternetOnMeteredConnection | bool | Gets a value indicating whether if the current internet connection is metered |
| NetworkNames | IReadOnlyList<string> | Gets signal strength for the current Internet Connection Profile |
| SignalStrength | Nullable<Byte> | Gets signal strength for the current Internet Connection Profile |

## ConnectionInformation Methods

| Methods | Return Type | Description |
| -- | -- | -- |
| UpdateConnectionInformation(ConnectionProfile) | void | Updates the current object based on profile passed |

## NetworkHelper Events

| Events | Description |
| -- | -- |
| NetworkChanged | Event raised when the network changes |

## Example

```csharp
// Detect if Internet can be reached
if (NetworkHelper.Instance.ConnectionInformation.IsInternetAvailable)
{
}

// Detect if the connection is metered
if (NetworkHelper.Instance.ConnectionInformation.IsInternetOnMeteredConnection)
{
}

// Get precise connection type
switch(NetworkHelper.Instance.ConnectionInformation.ConnectionType)
{
    case ConnectionType.Ethernet:
        // Ethernet
        break;
    case ConnectionType.WiFi:
        // WiFi
        break;
    case ConnectionType.Data:
        // Data
        break;
    case ConnectionType.Unknown:
        // Unknown
        break;
}
```
```vb
' Detect if Internet can be reached
If NetworkHelper.Instance.ConnectionInformation.IsInternetAvailable Then
    ...
End If

' Detect if the connection is metered
If NetworkHelper.Instance.ConnectionInformation.IsInternetOnMeteredConnection Then
   ...
End If

' Get precise connection type
Select Case NetworkHelper.Instance.ConnectionInformation.ConnectionType
    Case ConnectionType.Ethernet
        ' Ethernet
    Case ConnectionType.WiFi
        ' WiFi
    Case ConnectionType.Data
        ' Data
    Case ConnectionType.Unknown
        ' Unknown
End Select
```

## Sample Code

[NetworkHelper sample page Source](https://github.com/Microsoft/WindowsCommunityToolkit//tree/master/Microsoft.Toolkit.Uwp.SampleApp/SamplePages/NetworkHelper). You can see this in action in [Windows Community Toolkit Sample App](https://www.microsoft.com/store/apps/9NBLGGH4TLCQ).

## Requirements

| Device family | Universal, 10.0.15063.0 or higher |
| --- | --- |
| Namespace | Microsoft.Toolkit.Uwp.Connectivity |
| NuGet package | [Microsoft.Toolkit.Uwp.Connectivity](https://www.nuget.org/packages/Microsoft.Toolkit.Uwp.Connectivity/) |

## API

* [NetworkHelper source code](https://github.com/Microsoft/WindowsCommunityToolkit//blob/master/Microsoft.Toolkit.Uwp.Connectivity/Network/NetworkHelper.cs)

