---
title: HttpHelper
author: nmetulev
description: HttpHelper is a Windows Community Toolkit helper class used to assist in common http and networking scenarios.
keywords: windows 10, uwp, windows community toolkit, uwp community toolkit, uwp toolkit, HttpHelper
dev_langs:
  - csharp
  - vb
---

# HttpHelper

> [!WARNING]
(This API is obsolete and will be removed in the future. Please use [System.Net.Http.HttpClient](https://msdn.microsoft.com/library/system.net.http.httpclient(v=vs.110).aspx) or [Windows.Web.Http.HttpClient](https://docs.microsoft.com/uwp/api/Windows.Web.Http.HttpClient) directly)

The [HttpHelper](https://docs.microsoft.com/dotnet/api/microsoft.toolkit.uwp.httphelper) represents an HTTP request message including headers.

## Syntax

```csharp
var request = new HttpHelperRequest(new Uri("URI"), HttpMethod.Post);

var response = await HttpHelper.Instance.SendRequestAsync(request);
```
```vb
Private request = New HttpHelperRequest(New Uri("URI"), HttpMethod.Post)

Private response = Await HttpHelper.Instance.SendRequestAsync(request)
```

## Properties

The **HttpHelper** class has these properties.

| Property | Type | Description |
| -------- | ----------- | ----------- |
| Instance ![Static](https://i-msdn.sec.s-msft.com/dynimg/IC64394.jpeg) | HttpHelper | Gets the instance of HTTPHelper exposed as singleton. |

## Methods

The **HttpHelper** class has these methods. It also inherits from **Object** class.

| Methods | Return Type | Description |
| -- | -- | -- |
| GetInputStreamAsync(HttpHelperRequest, CancellationToken) | Task<[HttpHelperResponse](HttpHelperResponse.md)> | Process Http Request using instance of HttpClient |
| SendRequestAsync(HttpHelperRequest, CancellationToken) | Task<[HttpHelperResponse](HttpHelperResponse.md)> | Takes an instance of HttpHelperRequest as a parameter and passes it to server. It turns server response as HttpHelperResponse |
| ToString() | string | Returns a string that represents the current HttpHelper object |

## Remarks

The **HttpHelper** class exposes an instance of the class as a static property.
It additionally has one method - **SendRequestAsync**. This method requires an instance of **HttpHelperRequest** as an input parameter and returns an instance of **HttpHelperResponse**

## Example

```csharp
using (var request = new HttpHelperRequest(new Uri(twitterUrl), HttpMethod.Post))
{
    using (var response = await HttpHelper.Instance.SendRequestAsync(request))
    {
        return await response.GetTextResultAsync();
    }
}
```
```vb
Using request = New HttpHelperRequest(New Uri(twitterUrl), HttpMethod.Post)
    Using response = Await HttpHelper.Instance.SendRequestAsync(request)
        Return Await response.GetTextResultAsync()
    End Using
End Using
```

## Requirements

| Device family | Universal, 10.0.15063.0 or higher |
| --- | --- |
| Namespace | Microsoft.Toolkit.Uwp |
| NuGet package | [Microsoft.Toolkit.Uwp](https://www.nuget.org/packages/Microsoft.Toolkit.Uwp/) |

## API

* [HttpHelper source code](https://github.com/Microsoft/WindowsCommunityToolkit//blob/master/Microsoft.Toolkit.Uwp/Helpers/HttpHelper/HttpHelper.cs)
