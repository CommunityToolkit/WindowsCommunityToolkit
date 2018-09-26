---
title: Streams Helper
author: nmetulev
description: There are several operations that apps need commonly to do against their APPX, or from the Internet that are not easy.  This helper class wraps up some of the most common operations we need in multiple apps.
keywords: windows 10, uwp, windows community toolkit, uwp community toolkit, uwp toolkit, Streams
dev_langs:
  - csharp
  - vb
---

# Streams Helper

There are several operations that apps need commonly to do against their APPX, or from the Internet that are not easy.  [Streams helper](https://docs.microsoft.com/dotnet/api/microsoft.toolkit.uwp.helpers.streamhelper) class wraps up some of the most common operations we need in multiple apps.

## Some common scenarios

* Get a stream from a URI using an in memory stream (rather than needing to download it first).
* Download a URI and write it to a local storage file.
* Get a packaged file stream (files included in the APPX as *Content - do not copy*).
* Does a file exist local folder?
* Read text from a file using ASCII or specified encoding.

## Methods

| Methods | Return Type | Description |
| -- | -- | -- |
| GetHttpStreamAsync(Uri, CancellationToken) | Task<IRandomAccessStream> | Get the response stream returned by a HTTP get request |
| GetHttpStreamToStorageFileAsync(Uri, StorageFile) | Task | Get the response stream returned by a HTTP get request and save it to a local file |
| GetKnowFoldersFileStreamAsync(KnownFolderId, String, FileAccessMode) | Task<IRandomAccessStream> | Return a stream to a specified file from the application local cache folder |
| GetLocalCacheFileStreamAsync(String, FileAccessMode) | Task<IRandomAccessStream> | Return a stream to a specified file from the application local cache folder |
| GetLocalFileStreamAsync(String, FileAccessMode) | Task<IRandomAccessStream> | Return a stream to a specified file from the application local folder |
| GetPackagedFileStreamAsync(String, FileAccessMode) | Task<IRandomAccessStream> | Return a stream to a specified file from the installation folder |
| IsFileExistsAsync(StorageFolder, String) | Task<bool> | Test if a file exists in the application local folder |
| IsKnownFolderFileExistsAsync(KnownFolderId, String) | Task<bool> | Test if a file exists in the application local cache folder |
| IsLocalCacheFileExistsAsync(String) | Task<bool> | Test if a file exists in the application local cache folder |
| IsLocalFileExistsAsync(String) | Task<bool> | Test if a file exists in the application local folder |
| IsPackagedFileExistsAsync(String) | Task<bool> | Test if a file exists in the application installation folder |
| ReadTextAsync(IRandomAccessStream, Encoding) | Task<string> | Read stream content as a string |

## Example

```csharp
// Get access to a text file that was included in solution as Content | do not copy local
using (var stream = await StreamHelper.GetPackagedFileStreamAsync("Assets/Sub/test.txt"))
{
    // Read the contents as ASCII text
    var readText = await stream.ReadTextAsync();
}

// Get access to a HTTP resource
using (var stream = await StreamHelper.GetHttpStreamAsync(new Uri("http://dev.windows.com")))
{
    ...
}
```
```vb
' Get access to a text file that was included in solution as Content | do not copy local
Using stream = Await StreamHelper.GetPackagedFileStreamAsync("Assets/Sub/test.txt")
    '  Read the contents as ASCII text
    Dim readText = Await stream.ReadTextAsync()
    ...
End Using

' // Get access to a HTTP resource
Using stream = Await StreamHelper.GetHttpStreamAsync(New Uri("http://dev.windows.com"))
    ...
End Using
```

## Sample Code

You can find more examples in our [unit tests](https://github.com/Microsoft/WindowsCommunityToolkit//blob/master/UnitTests/Helpers/Test_StreamHelper.cs)

## Requirements

| Device family | Universal, 10.0.15063.0 or higher |
| --- | --- |
| Namespace | Microsoft.Toolkit.Uwp |
| NuGet package | [Microsoft.Toolkit.Uwp](https://www.nuget.org/packages/Microsoft.Toolkit.Uwp/) |

## API Source Code

* [Stream Helper source code](https://github.com/Microsoft/WindowsCommunityToolkit//blob/master/Microsoft.Toolkit.Uwp/Helpers/StreamHelper.cs)
