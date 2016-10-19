# HttpHelperResponse

Represents an HTTP response message including headers. 

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

## Member
The **HttpHelperResponse** class has these type of members:

* Constructors
* Methods
* Properties

### Constructors

The **HttpHelperResponse** class has these constructors.

| Constructor | Description |
| ----------  | ----------- |
| HttpHelperResponse([HttpResponseMessage](https://msdn.microsoft.com/en-us/library/windows/apps/windows.web.http.httpresponsemessage.aspx))  | Initializes an instance of the HttpHelperRequest class with supplied HttpHelperResponse. |

### Methods

The **HttpHelperResponse** class has these methods. It also inherits from **Object** class.

| Method | Description |
| ------ | ----------- |
| GetTextResultAsync() | Serialize the HTTP content to a String as an asynchronous operation. |
| Dispose() | Performs tasks associated with freeing, releasing, or resetting unmanaged resources. |
| ToString() | Returns a string that represents the current HttpHelperResponse object. |

### Properties

The **HttpHelperResponse** class has these properties.

| Property | Access type | Description |
| -------- | ----------- | ----------- |
| Content | Read-only | Gets the HTTP content return from the server. |
| Headers | Read-only | Gets the collection of HTTP response headers associated with the response that were sent by the server. |
| StatusCode | Read-only | Gets the status code of the response. |
| Success | Read-only | Gets a value that indicates whether the response was successful. |

## Remarks

The **HttpHelperResponse** class contains headers and potentially data. 
An app receives an instance of **HttpHelperResponse** when it creates an instance of **HttpHelperRequest** and passes it to the **HttpHelper.SendRequestAsync** method.

## Requirements (Windows 10 Device Family)

| [Device family](http://go.microsoft.com/fwlink/p/?LinkID=526370) | Universal, 10.0.10586.0 or higher |
| --- | --- |
| Namespace | Microsoft.Toolkit.Uwp |

## API

* [HttpHelperRequest source code](https://github.com/Microsoft/UWPCommunityToolkit/tree/dev/Microsoft.Toolkit.Uwp/Helpers/HttpHelperResponse.cs)