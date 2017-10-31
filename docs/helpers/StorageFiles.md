---
title: StorageFileHelper
author: nmetulev
ms.date: 08/20/2017
description: The StorageFileHelper is a static utility class that provides functions to help with reading and writing of text and bytes to the disk.  These functions are all wrapped into Async tasks.
keywords: windows 10, uwp, uwp community toolkit, uwp toolkit, StorageFileHelper
---

# StorageFileHelper

The StorageFileHelper is a static utility class that provides functions to help with reading and writing of text and bytes to the disk.  These functions are all wrapped into Async tasks.


## Example

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

You can find more examples in our [unit tests](https://github.com/Microsoft/UWPCommunityToolkit/blob/master/UnitTests/Helpers/Test_StorageFileHelper.cs)

## Requirements (Windows 10 Device Family)

| [Device family](http://go.microsoft.com/fwlink/p/?LinkID=526370) | Universal, 10.0.14393.0 or higher |
| --- | --- |
| Namespace | Microsoft.Toolkit.Uwp |

## API
* [Storage File Helper source code](https://github.com/Microsoft/UWPCommunityToolkit/blob/master/Microsoft.Toolkit.Uwp/Helpers/StorageFileHelper.cs)

