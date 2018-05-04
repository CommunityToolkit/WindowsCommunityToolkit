---
title: Bing Service
author: nmetulev
description: The Bing Service allows you to retrieve Bing results. Bing can return web and news results in your language, images, and videos for many countries around the world.
keywords: windows 10, uwp, windows community toolkit, uwp community toolkit, uwp toolkit, bing
dev_langs:
  - csharp
  - vb
---

# Bing Service

The **Bing Service** allows you to retrieve Bing results. Bing can return web and news results in your language, images, and videos for many countries around the world.

## Set up Bing API

> [!NOTE]
The current version does not require the API key and is using the rate limited public access point.  The ability to specify your own key to remove the rate limits is on our backlog for a future release.

[Signup for API Access](https://www.microsoft.com/cognitive-services/sign-up) using your Microsoft account.  There is a free trial option for all of the Bing services (fully functional, just with API rate limits or capacity limits).

Choose the *Bing Search - Free* option.  After selecting this and agreeing to the terms of service you will be issued two keys that are limited to 5,000 queries per month.

## Syntax

```csharp
using Microsoft.Toolkit.Uwp.Services.Bing;

var searchConfig = new BingSearchConfig
{
    Country = BingCountry.UnitedStates,
    Language = BingLanguage.English,
    Query = SearchText.Text,
    QueryType = BingQueryType.Search
};

ListView.ItemsSource = await BingService.Instance.RequestAsync(searchConfig, 50);
```
```vb
Imports using Microsoft.Toolkit.Uwp.Services.Bing

Dim searchConfig = New BingSearchConfig With {
    .Country = BingCountry.UnitedStates,
    .Language = BingLanguage.English,
    .Query = SearchText.Text,
    .QueryType = BingQueryType.Search
}
ListView.ItemsSource = Await BingService.Instance.RequestAsync(searchConfig, 50)
```

## BingDataProvider Class

**BingDataProvider** is a Data Provider for connecting to Bing service

### Methods

| Methods | Return Type | Description |
| -- | -- | -- |
| GetDataAsync(BingSearchConfig, Int32, Int32, IParser) | Task<IEnumerable<TSchema>> | Wrapper around REST API for making data request |
| GetDefaultParser(BingSearchConfig) | IParser<[BingResult](https://docs.microsoft.com/dotnet/api/microsoft.toolkit.services.bing.bingresult)> | Returns parser implementation for specified configuration |

## BingParser Class

**BingParser** parse Bing results into strong type

### Methods

| Methods | Return Type | Description |
| -- | -- | -- |
| Parse(String) | IEnumerable<[BingResult](https://docs.microsoft.com/dotnet/api/microsoft.toolkit.services.bing.bingresult)> | Take string data and parse into strong data type |

## BingResult Class

**BingResult** is a implementation of the Bing result class.

### Properties

| Property | Type | Description |
| -- | -- | -- |
| InternalID | string | Gets or sets identifier for strong typed record |
| Link | string | Description |
| Published | DateTime | Description |
| Summary | string | Description |
| Title | string | Gets or sets title of the search result |

## BingSearchConfig Class

**BingSearchConfig** configures the search query

### Properties

| Property | Type | Description |
| -- | -- | -- |
| Country | [BingCountry](https://docs.microsoft.com/dotnet/api/microsoft.toolkit.services.bing.bingcountry) | Description |
| Language | [BingLanguage](https://docs.microsoft.com/dotnet/api/microsoft.toolkit.services.bing.binglanguage) | Gets or sets search query language |
| Query | string | Gets or sets search query |
| QueryType | [BingQueryType](https://docs.microsoft.com/dotnet/api/microsoft.toolkit.services.bing.bingquerytype) | Gets or sets search query type |

## BingService Class

**BingService** Class for connecting to Bing

### Properties

| Property | Type | Description |
| -- | -- | -- |
| Instance | [BingService](https://docs.microsoft.com/dotnet/api/microsoft.toolkit.services.bing.bingservice) | Gets public singleton property |
| Provider | [BingDataProvider](https://docs.microsoft.com/dotnet/api/microsoft.toolkit.services.bing.bingdataprovider) | Gets a reference to an instance of the underlying data provider |

### Methods

| Methods | Return Type | Description |
| -- | -- | -- |
| GetPagedItemsAsync(Int32, Int32, CancellationToken) | Task<IEnumerable<BingResult>> | Retrieves items based on `pageIndex` and `pageSize` arguments |
| RequestAsync(BingSearchConfig, Int32, Int32) | Task<List<BingResult>> | Request list data from service provider based upon a given config / query |

## Sample Code

[Bing Service Sample Page Source](https://github.com/Microsoft/UWPCommunityToolkit/tree/master/Microsoft.Toolkit.Uwp.SampleApp/SamplePages/Bing%20Service). You can see this in action in [Windows Community Toolkit Sample App](https://www.microsoft.com/store/apps/9NBLGGH4TLCQ).

## Requirements

| Device family | Universal, 10.0.14393.0 or higher |
| --- | --- |
| Namespace | Microsoft.Toolkit.Uwp.Services |
| NuGet package | [Microsoft.Toolkit.Uwp.Services](https://www.nuget.org/packages/Microsoft.Toolkit.Uwp.Services/) |

## API

* [Bing Service source code](https://github.com/Microsoft/UWPCommunityToolkit/tree/master/Microsoft.Toolkit.Uwp.Services/Services/Bing)
