---
title: ImageCache
author: nmetulev
description: The ImageCache provides methods and tools to cache images in a temporary local folder.
keywords: windows 10, uwp, windows community toolkit, uwp community toolkit, uwp toolkit, ImageCache
dev_langs:
  - csharp
  - vb
---

# ImageCache

The [ImageCache](https://docs.microsoft.com/dotnet/api/microsoft.toolkit.uwp.ui.imagecache) provides methods and tools to cache images in a temporary local folder. ImageCache also supports optional in-memory layer of caching, that provides better performance when same images are requested multiple times (like in long virtualized lists of images). This type of caching is disabled by default, but can be enabled by setting MaxMemoryCacheSize to desired size. For example: setting MaxMemoryCacheSize to 100 means that 100 last requested images will be held in memory to be instantly available, without disk reads.

## Syntax

```csharp
// Set cache duration
ImageCache.Instance.CacheDuration = TimeSpan.FromHours(24);

// Enable in-memory caching
ImageCache.Instance.MaxMemoryCacheCount = 100;

var distantUri = new Uri("http://www.myserver.com/image.jpg");

// Load a specific image from the cache. If the image is not in the cache, ImageCache will try to download and store it
var bitmapImage = await ImageCache.Instance.GetFromCacheAsync(distantUri);

// Clear the cache. Please note that you can provide a parameter to define a timespan from now to select cache entries to delete.
await ImageCache.Instance.ClearAsync();	
```
```vb
' Set cache duration
ImageCache.Instance.CacheDuration = TimeSpan.FromHours(24)

' Enable in-memory caching
ImageCache.Instance.MaxMemoryCacheCount = 100

Dim distantUri = New Uri("http://www.myserver.com/image.jpg")

' Load a specific image from the cache. If the image is not in the cache, ImageCache will try to download and store it
Dim bitmapImage = Await ImageCache.Instance.GetFromCacheAsync(distantUri)

' Clear the cache. Please note that you can provide a parameter to define a timespan from now to select cache entries to delete.
Await ImageCache.Instance.ClearAsync()
```

## Properties

| Property | Type | Description |
| -- | -- | -- |
| CacheDuration | TimeSpan | Gets or sets the life duration of every cache entry |
| HttpClient | HttpClient | Gets instance of HttpClient |
| Instance | ImageCache | Gets public singleton property |
| MaintainContext | bool | Gets or sets a value indicating whether context should be maintained until type has been instantiated or not |
| MaxMemoryCacheCount | int | Gets or sets max in-memory item storage count |
| RetryCount | uint | Gets or sets the number of retries trying to ensure the file is cached |

## Methods

| Methods | Return Type | Description |
| -- | -- | -- |
| ClearAsync() | Task | Clears all files in the cache |
| ClearAsync(TimeSpan) | Task | Clears file if it has expired |
| GetFileFromCacheAsync(Uri) | Task<StorageFile> | Gets the StorageFile containing cached item for given Uri |
| GetFromCacheAsync(Uri, Boolean, CancellationToken, List<KeyValuePair<string, object>>) | Task<T> | Retrieves item represented by Uri from the cache. If the item is not found in the cache, it will try to downloaded and saved before returning it to the caller |
| GetFromMemoryCache(Uri) | T | Retrieves item represented by Uri from the in-memory cache if it exists and is not out of date. If item is not found or is out of date, default instance of the generic type is returned |
| PreCacheAsync(Uri, Boolean, Boolean, CancellationToken) | Task | Assures that item represented by Uri is cached |
| RemoveAsync(IEnumerable) | Task | Removed items based on uri list passed |
| RemoveExpiredAsync(Nullable) | Task | Removes cached files that have expired |

## Requirements

| Device family | Universal, 10.0.14393.0 or higher |
| --- | --- |
| Namespace | Microsoft.Toolkit.Uwp |
| NuGet package | [Microsoft.Toolkit.Uwp](https://www.nuget.org/packages/Microsoft.Toolkit.Uwp/) |

## API

* [ImageCache source code](https://github.com/Microsoft/UWPCommunityToolkit/blob/master/Microsoft.Toolkit.Uwp.UI/Cache/ImageCache.cs)
