---
title: HttpHelperRequest
author: nmetulev
description: HttpHelperRequest is a Windows Community Toolkit helper class used with the HttpHelper class to create http requests.
keywords: windows 10, uwp, windows community toolkit, uwp community toolkit, uwp toolkit, HttpHelperRequest
dev_langs:
  - csharp
  - vb
---

# HttpHelperRequest

> [!WARNING]
(This API is obsolete and will be removed in the future. Please use [System.Net.Http.HttpRequestMessage](https://msdn.microsoft.com/library/system.net.http.httprequestmessage(v=vs.110).aspx) 
or [Windows.Web.Http.HttpRequestMessage](https://docs.microsoft.com/uwp/api/windows.web.http.httprequestmessage) directly)

The [HttpHelperRequest](https://docs.microsoft.com/dotnet/api/microsoft.toolkit.uwp.httphelperrequest) represents an HTTP request message including headers. 

```csharp
var request = new HttpHelperRequest(uri, HttpMethod.Get);
```
```vb
Dim request = New HttpHelperRequest(uri, HttpMethod.[Get])
```

## Constructors

The **HttpHelperRequest** class has these constructors.

| Constructor | Description |
| ----------  | ----------- |
| HttpHelperRequest([Uri](https://msdn.microsoft.com/library/system.uri.aspx))  | Initializes a new instance of the HttpHelperRequest class with an HTTP Get method and a request Uri.|
| HttpHelperRequest([HttpMethod](https://msdn.microsoft.com/en-us/library/windows/apps/windows.web.http.httpmethod.aspx), [Uri](https://msdn.microsoft.com/library/system.uri.aspx))  | Initializes a new instance of the HttpRequestMessage class with an HTTP method and a request Uri.|

## Methods

The **HttpHelperRequest** class has these methods. It also inherits from **Object** class.

| Method | Description |
| ------ | ----------- |
| ToHttpRequestMessage() | Returns an instance of [**HttpRequestMessage**](https://msdn.microsoft.com/en-us/library/windows/apps/windows.web.http.httprequestmessage.aspx) representing current HttpHelperRequest object. |
| Dispose() | Performs tasks associated with freeing, releasing, or resetting unmanaged resources. |
| ToString() | Returns a string that represents the current HttpHelperRequest object. |

## Properties

The **HttpHelperRequest** class has these properties.

| Property | Type | Description |
| -------- | ----------- | ----------- |
| Content | IHttpContent | Gets or sets the HTTP content to send to the server on the HTTP request |
| Headers | HttpRequestHeaderCollection | Gets the collection of the HTTP request headers associated with the request |
| Method | HttpMethod | Gets the HTTP method to be performed on the request URI |
| RequestUri | Uri | Gets the Uri used for the HTTP request |

## Remarks

The **HttpHelperRequest** class contains headers, the HTTP verb, and potentially data. 
An app starts by using one of the **HttpHelperRequest** constructors to create an **HttpRequestHelper** instance. The app then sets various properties on the **HttpRequestHelper** as needed. Then the **HttpRequestHelper** is passed as a parameter to the HttpHelper.SendRequestAsync method.

## Example

```csharp
var request = new HttpHelperRequest(uri, HttpMethod.Get);
request.Headers.Authorization = new Windows.Web.Http.Headers.HttpCredentialsHeaderValue("OAuth", authorizationHeaderParams);
```
```vb
Dim request = New HttpHelperRequest(uri, HttpMethod.[Get])
request.Headers.Authorization = New Windows.Web.Http.Headers.HttpCredentialsHeaderValue("OAuth", authorizationHeaderParams)
```

## Requirements

| Device family | Universal, 10.0.15063.0 or higher |
| --- | --- |
| Namespace | Microsoft.Toolkit.Uwp |
| NuGet package | [Microsoft.Toolkit.Uwp](https://www.nuget.org/packages/Microsoft.Toolkit.Uwp/) |

## API

* [HttpHelperRequest source code](https://github.com/Microsoft/WindowsCommunityToolkit//blob/master/Microsoft.Toolkit.Uwp/Helpers/HttpHelper/HttpHelperRequest.cs)
