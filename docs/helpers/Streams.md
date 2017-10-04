---
title: Streams Helper
author: nmetulev
ms.date: 08/20/2017
description: There are several operations that apps need commonly to do against their APPX, or from the Internet that are not easy.  This helper class wraps up some of the most common operations we need in multiple apps.
keywords: windows 10, uwp, uwp community toolkit, uwp toolkit, Streams
---

# Streams Helper

There are several operations that apps need commonly to do against their APPX, or from the Internet that are not easy.  This helper class wraps up some of the most common operations we need in multiple apps.

## Some common scenarios

* Get a stream from a URI using an in memory stream (rather than needing to download it first).
* Download a URI and write it to a local storage file.
* Get a packaged file stream (files included in the APPX as *Content - do not copy*).
* Does a file exist local folder?
* Read text from a file using ASCII or specified encoding.

## Example

```csharp

    // Get access to a text file that was included in solution as Content | do not copy local
    using (var stream = await StreamHelper.GetPackagedFileStreamAsync("Assets/Sub/test.txt"))
    {
	// Read the contents as ASCII text
        var readText = await stream.ReadTextAsync();
    }
    
    // Get access to a HTTP ressource
    using (var stream = await StreamHelper.GetHttpStreamAsync(new Uri("http://dev.windows.com")))
    {
        ...
    }

```

You can find more examples in our [unit tests](https://github.com/Microsoft/UWPCommunityToolkit/blob/master/UnitTests/Helpers/Test_StreamHelper.cs)

## Requirements (Windows 10 Device Family)

| [Device family](http://go.microsoft.com/fwlink/p/?LinkID=526370) | Universal, 10.0.14393.0 or higher |
| --- | --- |
| Namespace | Microsoft.Toolkit.Uwp |

## API

* [Stream Helper source code](https://github.com/Microsoft/UWPCommunityToolkit/blob/master/Microsoft.Toolkit.Uwp/Helpers/StreamHelper.cs)

