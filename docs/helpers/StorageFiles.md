# StorageFileHelper

The StorageFileHelper is a static utility class that provides functions to help with reading and writing of text and bytes to the disk.  These functions are all wrapped into Async tasks.


## Example

```csharp

	// NOTE This must be used from an async function
	string myText = "Great information that the users wants to keep";
	
	// Save some text to a file named appFilename.txt (in the local cache folder)
	var storageFile = await StorageFileHelper.WriteTextToLocalCacheFileAsync(myText, "appFilename.txt");
	
	// Load some text from a file named appFilename.txt in the local cache folder	
	string loadedText = await StorageFileHelper.ReadTextFromLocalCacheFileAsync("appFilename.txt");
	
	// Save some text to a file named appFilename.txt (in the local folder)
	storageFile = await StorageFileHelper.WriteTextToLocalFileAsync(myText, "appFilename.txt");
	
	// Load some text from a file named appFilename.txt in the local folder	
	loadedText = await StorageFileHelper.ReadTextFromLocalFileAsync("appFilename.txt");

```

You can find more examples in our [unit tests](https://github.com/Microsoft/UWPCommunityToolkit/blob/master/UnitTests/Helpers/Test_StorageFileHelper.cs)

## Requirements (Windows 10 Device Family)

| [Device family](http://go.microsoft.com/fwlink/p/?LinkID=526370) | Universal, 10.0.10586.0 or higher |
| --- | --- |
| Namespace | Microsoft.Toolkit.Uwp |

## API
* [Storage File Helper source code](https://github.com/Microsoft/UWPCommunityToolkit/blob/master/Microsoft.Toolkit.Uwp/Helpers/StorageFileHelper.cs)

