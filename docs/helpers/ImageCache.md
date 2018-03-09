---
title: ImageCache
author: nmetulev
description: The ImageCache provides methods and tools to cache images in a temporary local folder.
keywords: windows 10, uwp, uwp community toolkit, uwp toolkit, ImageCache
---

# ImageCache

The **ImageCache** provides methods and tools to cache images in a temporary local folder. ImageCache also supports optional in-memory layer of caching, that provides better performance when same images are requested multiple times (like in long virtualized lists of images). This type of caching is disabled by default, but can be enabled by setting MaxMemoryCacheSize to desired size. For example: setting MaxMemoryCacheSize to 100 means that 100 last requested images will be held in memory to be instantly available, without disk reads.

## Example

[!div class="tabbedCodeSnippets" data-resources="OutlookServices.Calendar"]
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

## Requirements (Windows 10 Device Family)

| [Device family](http://go.microsoft.com/fwlink/p/?LinkID=526370) | Universal, 10.0.14393.0 or higher |
| --- | --- |
| Namespace | Microsoft.Toolkit.Uwp |


## API

* [ImageCache source code](https://github.com/Microsoft/UWPCommunityToolkit/blob/master/Microsoft.Toolkit.Uwp.UI/Cache/ImageCache.cs)


