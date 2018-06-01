---
title: StorageFileHelper
author: nmetulev
description: The StorageFileHelper is a static utility class that provides functions to help with reading and writing of text and bytes to the disk.  These functions are all wrapped into Async tasks.
keywords: windows 10, uwp, windows community toolkit, uwp community toolkit, uwp toolkit, StorageFileHelper
dev_langs:
  - csharp
  - vb
---

# StorageFileHelper

The [StorageFileHelper](https://docs.microsoft.com/dotnet/api/microsoft.toolkit.uwp.helpers.storagefilehelper) is a static utility class that provides functions to help with reading and writing of text and bytes to the disk.  These functions are all wrapped into Async tasks.

## Syntax

```csharp
// NOTE This must be used from an async function
string myText = "Great information that the users wants to keep";
StorageFolder localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;

// Save some text to a file named appFilename.txt (in the local cache folder)
var storageFile = await StorageFileHelper.WriteTextToLocalCacheFileAsync(myText, "appFilename.txt");

// Load some text from a file named appFilename.txt in the local cache folder	
string loadedText = await StorageFileHelper.ReadTextFromLocalCacheFileAsync("appFilename.txt");

// Save some text to a file named appFilename.txt (in the local folder)
storageFile = await StorageFileHelper.WriteTextToLocalFileAsync(myText, "appFilename.txt");

// Load some text from a file named appFilename.txt in the local folder	
loadedText = await StorageFileHelper.ReadTextFromLocalFileAsync("appFilename.txt");

// Check if a file exists in a specific folder
bool exists = await localFolder.FileExistsAsync("appFilename.txt");

// Check if a file exists in a specific folder or in one of its subfolders
bool exists = await localFolder.FileExistsAsync("appFilename.txt", true);

// Check if a file name is valid or not
bool isFileNameValid = StorageFileHelper.IsFileNameValid("appFilename.txt");

// Check if a file path is valid or not
bool isFilePathValid = StorageFileHelper.IsFilePathValid("folder/appFilename.txt");
```
```vb
' NOTE This must be used from an async function
Dim myText As String = "Great information that the users wants to keep"
Dim localFolder As StorageFolder = Windows.Storage.ApplicationData.Current.LocalFolder

' Save some text to a file named appFilename.txt (in the local cache folder)
Dim storageFile = Await StorageFileHelper.WriteTextToLocalCacheFileAsync(myText, "appFilename.txt")

' Load some text from a file named appFilename.txt in the local cache folder	
Dim loadedText As String = Await StorageFileHelper.ReadTextFromLocalCacheFileAsync("appFilename.txt")

' Save some text to a file named appFilename.txt (in the local folder)
storageFile = Await StorageFileHelper.WriteTextToLocalFileAsync(myText, "appFilename.txt")

' Load some text from a file named appFilename.txt in the local folder	
loadedText = Await StorageFileHelper.ReadTextFromLocalFileAsync("appFilename.txt")

' Check if a file exists in a specific folder
Dim exists As Boolean = Await localFolder.FileExistsAsync("appFilename.txt")

' Check if a file exists in a specific folder or in one of its subfolders
Dim exists As Boolean = Await localFolder.FileExistsAsync("appFilename.txt", True)

' Check if a file name is valid or not
Dim isFileNameValid As Boolean = StorageFileHelper.IsFileNameValid("appFilename.txt")

' Check if a file path is valid or not
Dim isFilePathValid As Boolean = StorageFileHelper.IsFilePathValid("folder/appFilename.txt")
```

## Methods

| Methods | Return Type | Description |
| -- | -- | -- |
| FileExistsAsync(StorageFolder, String, Boolean) | Task<bool> | Gets a value indicating whether a file exists in the current folder |
| IsFileNameValid(String) | bool | Gets a value indicating whether a filename is correct or not using the Storage feature |
| IsFilePathValid(String) | bool | Gets a value indicating whether a file path is correct or not using the Storage feature |
| ReadBytesAsync(StorageFile) | Task<byte[]> | Gets an array of bytes from a `StorageFile` |
| ReadBytesFromFileAsync(StorageFolder, String) | Task<byte[]> | Gets an array of bytes from a `StorageFile` located in the given `StorageFolder` |
| ReadBytesFromKnownFoldersFileAsync(KnownFolderId, String) | Task<byte[]> | Gets an array of bytes from a `StorageFile` located in a well known folder |
| ReadBytesFromLocalCacheFileAsync(String) | Task<byte[]> | Gets an array of bytes from a `StorageFile` located in the application local cache folder |
| ReadBytesFromLocalFileAsync(String) | Task<byte[]> | Gets an array of bytes from a `StorageFile` located in the application local folder |
| ReadBytesFromPackagedFileAsync(String) | Task<byte[]> | Gets an array of bytes from a `StorageFile` located in the application installation folder |
| ReadTextFromFileAsync(StorageFolder, String) | Task<string> | Gets a string value from a `StorageFile` located in the given `StorageFolder` |
| ReadTextFromKnownFoldersFileAsync(KnownFolderId, String) | Task<string> | Gets a string value from a `StorageFile` located in a well known folder |
| ReadTextFromLocalCacheFileAsync(String) | Task<string> | Gets a string value from a `StorageFile` located in the application local cache folder |
| ReadTextFromLocalFileAsync(String) | Task<string> | Gets a string value from a `StorageFile` located in the application local folder |
| ReadTextFromPackagedFileAsync(String) | Task<string> | Gets a string value from a `StorageFile` located in the application installation folder |
| WriteBytesToFileAsync(StorageFolder, Byte[], String, CreationCollisionOption) | Task<StorageFile> | Saves an array of bytes to a `StorageFile` in the given `StorageFolder` |
| WriteBytesToKnownFolderFileAsync(KnownFolderId, Byte[], String, CreationCollisionOption) | Task<StorageFile> | Saves an array of bytes to a `StorageFile` to well known folder |
| WriteBytesToLocalCacheFileAsync(Byte[], String, CreationCollisionOption) | Task<StorageFile> | Saves an array of bytes to a `StorageFile` to application local cache folder |
| WriteBytesToLocalFileAsync(Byte[], String, CreationCollisionOption) | Task<StorageFile> | Saves an array of bytes to a `StorageFile` to application local folder |
| WriteTextToFileAsync(StorageFolder, String, String, CreationCollisionOption) | Task<StorageFile> | Saves a string value to a `StorageFile` in the given `StorageFolder` |
| WriteTextToKnownFolderFileAsync(KnownFolderId, String, String, CreationCollisionOption) | Task<StorageFile> | Saves a string value to a Windows.Storage.StorageFile in well known folder |
| WriteTextToLocalCacheFileAsync(String, String, CreationCollisionOption) | Task<StorageFile> | Saves a string value to a Windows.Storage.StorageFile in application local cache folder |
| WriteTextToLocalFileAsync(String, String, CreationCollisionOption) | Task<StorageFile> | Saves a string value to a Windows.Storage.StorageFile in application local folder |

## Sample Code

You can find more examples in our [unit tests](https://github.com/Microsoft/UWPCommunityToolkit/blob/master/UnitTests/Helpers/Test_StorageFileHelper.cs)

## Requirements

| Device family | Universal, 10.0.14393.0 or higher |
| --- | --- |
| Namespace | Microsoft.Toolkit.Uwp |
| NuGet package | [Microsoft.Toolkit.Uwp](https://www.nuget.org/packages/Microsoft.Toolkit.Uwp/) |

## API

* [Storage File Helper source code](https://github.com/Microsoft/UWPCommunityToolkit/blob/master/Microsoft.Toolkit.Uwp/Helpers/StorageFileHelper.cs)
