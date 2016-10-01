# ConnectionHelper

The ConnectionHelper class is used to determine whether the app has Internet, and if it is on a metered Internet connection.

_What is a metered connection?_
A metered connection is an Internet connection that has a data limit or cost associated with it. Cellular data connections are set as metered by default. Wi-Fi network connections can be set to metered, but aren't by default. Some apps and features in Windows will behave differently on a metered connection to help reduce your data usage.

## Example

```csharp

	// Metered connections are determined by the OS
    if( ConnectionHelper.IsInternetOnMeteredConnection )
    {
        // Attempt to only use local resources
    }
    else
    {
        // Attempt to refresh from internet
    }

	// Test if there is any network available
    if (ConnectionHelper.IsInternetAvailable == false)
        return;

```

You can find more examples in our [unit tests](https://github.com/Microsoft/UWPCommunityToolkit/blob/master/UnitTests/Helpers/Test_ConnectionHelper.cs)

## Requirements (Windows 10 Device Family)

| [Device family](http://go.microsoft.com/fwlink/p/?LinkID=526370) | Universal, 10.0.10586.0 or higher |
| --- | --- |
| Namespace | Microsoft.Toolkit.Uwp |

## API

* [Connection Helper source code](https://github.com/Microsoft/UWPCommunityToolkit/blob/master/Microsoft.Toolkit.Uwp/Helpers/ConnectionHelper.cs)

