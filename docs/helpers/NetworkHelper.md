---
title: NetworkHelper
author: nmetulev
ms.date: 08/20/2017
description: he NetworkHelper class provides functionality to monitor changes in network connection and allows users to query for network information without additional lookups.
keywords: windows 10, uwp, uwp community toolkit, uwp toolkit, NetworkHelper
---

# NetworkHelper

The NetworkHelper class provides functionality to monitor changes in network connection and allows users to query for network information without additional lookups.

It exposes network information though a property called ConnectionInformation. The ConnectionInformation holds information about ConnectionType, ConnectivityLevel, ConnectionCost, SignalStrength, Internet Connectivity and more.

***_What is a metered connection?_***
A metered connection is an Internet connection that has a data limit or cost associated with it. Cellular data connections are set as metered by default. Wi-Fi network connections can be set to metered, but aren't by default. Application developers should take metered nature of connection into account and reduce data usage.

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

You can find more examples in our [unit tests](https://github.com/Microsoft/UWPCommunityToolkit/blob/master/UnitTests/Helpers/Test_ConnectionHelper.cs)

## Requirements (Windows 10 Device Family)

| [Device family](http://go.microsoft.com/fwlink/p/?LinkID=526370) | Universal, 10.0.14393.0 or higher |
| --- | --- |
| Namespace | Microsoft.Toolkit.Uwp |

## API

* [NetworkHelper source code](https://github.com/Microsoft/UWPCommunityToolkit/blob/master/Microsoft.Toolkit.Uwp/Helpers/Network/NetworkHelper.cs)

