---
title: Bing Service
author: nmetulev
ms.date: 08/20/2017
description: The Bing Service allows you to retrieve Bing results. Bing can return web and news results in your language, images, and videos for many countries around the world.
keywords: windows 10, uwp, uwp community toolkit, uwp toolkit, bing
---

# Bing Service

The **Bing Service** allows you to retrieve Bing results. Bing can return web and news results in your language, images, and videos for many countries around the world.

## Set up Bing API

**Note:**  The current version does not require the API key and is using the rate limited public access point.  The ability to specify your own key to remove the rate limits is on our backlog for a future release.

[Signup for API Access](https://www.microsoft.com/cognitive-services/en-us/sign-up) using your Microsoft account.  There is a free trial option for all of the Bing services (fully functional, just with API rate limits or capacity limits).

Choose the *Bing Search - Free* option.  After selecting this and agreeing to the terms of service you will be issued two keys that are limited to 5,000 queries per month.

## Example Syntax

```csharp

// using Microsoft.Toolkit.Uwp.Services.Bing;

var searchConfig = new BingSearchConfig
{
    Country = BingCountry.UnitedStates,
    Language = BingLanguage.English,
    Query = SearchText.Text,
    QueryType = BingQueryType.Search
};

ListView.ItemsSource = await BingService.Instance.RequestAsync(searchConfig, 50);

```

## Example

[Bing Service Sample Page](https://github.com/Microsoft/UWPCommunityToolkit/tree/master/Microsoft.Toolkit.Uwp.SampleApp/SamplePages/Bing%20Service)

## Requirements (Windows 10 Device Family)

| [Device family](http://go.microsoft.com/fwlink/p/?LinkID=526370) | Universal, 10.0.14393.0 or higher |
| --- | --- |
| Namespace | Microsoft.Toolkit.Uwp.Services |

## API

* [Bing Service source code](https://github.com/Microsoft/UWPCommunityToolkit/tree/master/Microsoft.Toolkit.Uwp.Services/Services/Bing)


## NuGet Packages Required

**Microsoft.Toolkit.Uwp.Services**

See the [NuGet Packages page](../Nuget-Packages.md) for complete list.
