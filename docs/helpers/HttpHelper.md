# HttpHelper

Represents an HTTP request message including headers. 

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

## Members

The **HttpHelper** class has these type of members:

* Methods
* Properties

### Methods

The **HttpHelper** class has these methods. It also inherits from **Object** class.

| Method | Description |
| ------ | ----------- |
| SendRequestAsync() | Takes an instance of HttpHelperRequest as a parameter and passes it to server. It turns server response as HttpHelperResponse. |
| ToString() | Returns a string that represents the current HttpHelper object. |

### Properties

The **HttpHelper** class has these properties.

| Property | Access type | Description |
| -------- | ----------- | ----------- |
| Instance ![Static](https://i-msdn.sec.s-msft.com/dynimg/IC64394.jpeg) | Read-only | Gets the instance of HTTPHelper exposed as singleton. |

## Remarks

The **HttpHelper** class exposes an instance of the class as a static property.
It additionally has one method - **SendRequestAsync**. This method requires an instance of **HttpHelperRequest** as an input parameter and returns an instance of **HttpHelperResponse**

## Requirements (Windows 10 Device Family)

| [Device family](http://go.microsoft.com/fwlink/p/?LinkID=526370) | Universal, 10.0.10586.0 or higher |
| --- | --- |
| Namespace | Microsoft.Toolkit.Uwp |

## API

* [HttpHelperRequest source code](https://github.com/Microsoft/UWPCommunityToolkit/tree/dev/Microsoft.Toolkit.Uwp/Helpers/HttpHelper.cs)