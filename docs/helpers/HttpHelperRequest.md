# HttpHelperRequest

Represents an HTTP request message including headers. 

## Example

```csharp

var request = new HttpHelperRequest(uri, HttpMethod.Get);
request.Headers.Authorization = new Windows.Web.Http.Headers.HttpCredentialsHeaderValue("OAuth", authorizationHeaderParams);

```

## Members

The **HttpHelperRequest** class has these type of members:

* Constructors
* Methods
* Properties

### Constructors

The **HttpHelperRequest** class has these constructors.

| Constructor | Description |
| ----------  | ----------- |
| HttpHelperRequest([Uri](https://msdn.microsoft.com/library/system.uri.aspx))  | Initializes a new instance of the HttpHelperRequest class with an HTTP Get method and a request Uri.|
| HttpHelperRequest([HttpMethod](https://msdn.microsoft.com/en-us/library/windows/apps/windows.web.http.httpmethod.aspx), [Uri](https://msdn.microsoft.com/library/system.uri.aspx))  | Initializes a new instance of the HttpRequestMessage class with an HTTP method and a request Uri.|

### Methods

The **HttpHelperRequest** class has these methods. It also inherits from **Object** class.

| Method | Description |
| ------ | ----------- |
| ToHttpRequestMessage() | Returns an instance of [**HttpRequestMessage**](https://msdn.microsoft.com/en-us/library/windows/apps/windows.web.http.httprequestmessage.aspx) representing current HttpHelperRequest object. |
| Dispose() | Performs tasks associated with freeing, releasing, or resetting unmanaged resources. |
| ToString() | Returns a string that represents the current HttpHelperRequest object. |

### Properties

The **HttpHelperRequest** class has these properties.

| Property | Access type | Description |
| -------- | ----------- | ----------- |
| Content | Read-write | Gets or sets the HTTP content to send to the server on the HTTP request. |
| Headers | Read-only | Gets the collection of the HTTP request headers associated with the request. |
| Method | Read-only | Gets the HTTP method to be performed on the request URI. |
| RequestUri | Read-only | Gets the Uri used for the HTTP request. |

## Remarks

The **HttpHelperRequest** class contains headers, the HTTP verb, and potentially data. 
An app starts by using one of the **HttpHelperRequest** constructors to create an **HttpRequestHelper** instance. The app then sets various properties on the **HttpRequestHelper** as needed. Then the **HttpRequestHelper** is passed as a parameter to the HttpHelper.SendRequestAsync method.

## Requirements (Windows 10 Device Family)

| [Device family](http://go.microsoft.com/fwlink/p/?LinkID=526370) | Universal, 10.0.10586.0 or higher |
| --- | --- |
| Namespace | Microsoft.Toolkit.Uwp |

## API

* [HttpHelperRequest source code](https://github.com/Microsoft/UWPCommunityToolkit/tree/dev/Microsoft.Toolkit.Uwp/Helpers/HttpHelperRequest.cs)