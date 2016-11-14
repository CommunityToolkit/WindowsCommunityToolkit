# Object Storage

The Object Storage Helper will help you handle storage of generic objects within UWP applications, both locally and across all devices (roaming).

## Example

### Local Storage

If you need to handle local saves of any object (generic), you can use `LocalObjectStorageHelper`.

```csharp

    var helper = new LocalObjectStorageHelper();

    // Read simple objects
    string keySimpleObject = "simple";
    if (helper.KeyExists(keySimpleObject))
    {
        string result = helper.Read<string>(keySimpleObject);
    }

    // Save simple objects
    helper.Save(keySimpleObject, 47);

    // Read complex/large objects 
    string keyLargeObject = "large";
    if (await helper.FileExistsAsync(keyLargeObject))
    {
        var result = await helper.ReadFileAsync<MyLargeObject>(keyLargeObject);
    }

    // Save complex/large objects 
    var o = new MyLargeObject
    {
        ...
    };
    await helper.SaveFileAsync(keySimpleObject, o);
```

### Roaming Storage

In the same way, if you need to handle roaming saves across all of user devices, you can use `RoamingObjectStorageHelper`.
The implementation of the `RoamingObjectStorageHelper` is absolutely similar to `LocalObjectStorageHelper`.

```csharp

    var helper = new RoamingObjectStorageHelper();

    // Read simple objects
    string keySimpleObject = "simple";
    if (helper.KeyExists(keySimpleObject))
    {
        string result = helper.Read<string>(keySimpleObject);
    }

    // Save simple objects
    helper.Save(keySimpleObject, 47);

    // Read complex/large objects 
    string keyLargeObject = "large";
    if (await helper.FileExistsAsync(keyLargeObject))
    {
        var result = await helper.ReadFileAsync<MyLargeObject>(keyLargeObject);
    }

    // Save complex/large objects 
    var o = new MyLargeObject
    {
        ...
    };
    await helper.SaveFileAsync(keySimpleObject, o);
```

## Requirements (Windows 10 Device Family)

| [Device family](http://go.microsoft.com/fwlink/p/?LinkID=526370) | Universal, 10.0.10586.0 or higher |
| --- | --- |
| Namespace | Microsoft.Toolkit.Uwp |

## API
* [LocalObjectStorageHelper source code](https://github.com/Microsoft/UWPCommunityToolkit/blob/master/Microsoft.Toolkit.Uwp/Helpers/ObjectStorage/LocalObjectStorageHelper.cs)
* [RoamingObjectStorageHelper source code](https://github.com/Microsoft/UWPCommunityToolkit/blob/master/Microsoft.Toolkit.Uwp/Helpers/ObjectStorage/RoamingObjectStorageHelper.cs)

