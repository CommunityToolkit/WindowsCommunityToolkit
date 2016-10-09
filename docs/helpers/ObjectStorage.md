# Object Storage

The Object Storage Helper will help you handle storage of generic objects within UWP applications, both locally and across all devices (roaming).

## Example

### Local Storage

If you need to handle local saves of any object (generic), you can use `LocalObjectStorageHelper`.

```csharp

    var localStorageService = new LocalStorageService();

    // Read simple objects
    string keySimpleObject = "simple";
    if (localStorageService.KeyExists(keySimpleObject))
    {
        string result = localStorageService.Read<string>(keySimpleObject);
    }

    // Save simple objects
    localStorageService.Save(keySimpleObject, 47);

    // Read complex/large objects 
    string keyLargeObject = "large";
    if (await localStorageService.FileExistsAsync(keyLargeObject))
    {
        var result = await localStorageService.ReadFileAsync<MyLargeObject>(keyLargeObject);
    }

    // Save complex/large objects 
    var o = new MyLargeObject
    {
        ...
    };
    await localStorageService.SaveFileAsync(keySimpleObject, o);

    // Complex object
    public class MyLargeObject
    {
        public string MyContent { get; set; }
        public List<string> MyContents { get; set; }
        public List<MyLargeObject> MyObjects { get; set; }
    }

```

### Roaming Storage

In the same way, if you need to handle roaming saves across all of user devices, you can use `RoamingObjectStorageHelper`.
The implementation of the `RoamingObjectStorageHelper` is absolutely similar to `LocalObjectStorageHelper`.

```csharp

    var roamingStorageService = new RoamingStorageService();

    // Read simple objects
    string keySimpleObject = "simple";
    if (roamingStorageService.KeyExists(keySimpleObject))
    {
        string result = roamingStorageService.Read<string>(keySimpleObject);
    }

    // Save simple objects
    roamingStorageService.Save(keySimpleObject, 47);

    // Read complex/large objects 
    string keyLargeObject = "large";
    if (await roamingStorageService.FileExistsAsync(keyLargeObject))
    {
        var result = await roamingStorageService.ReadFileAsync<MyLargeObject>(keyLargeObject);
    }

    // Save complex/large objects 
    var o = new MyLargeObject
    {
        ...
    };
    await roamingStorageService.SaveFileAsync(keySimpleObject, o);

    // Complex object
    public class MyLargeObject
    {
        public string MyContent { get; set; }
        public List<string> MyContents { get; set; }
        public List<MyLargeObject> MyObjects { get; set; }
    }

```

## Requirements (Windows 10 Device Family)

| [Device family](http://go.microsoft.com/fwlink/p/?LinkID=526370) | Universal, 10.0.10586.0 or higher |
| --- | --- |
| Namespace | Microsoft.Toolkit.Uwp |

## API
* [LocalObjectStorageHelper source code](https://github.com/Microsoft/UWPCommunityToolkit/blob/master/Microsoft.Toolkit.Uwp/Helpers/ObjectStorage/LocalObjectStorageHelper.cs)
* [RoamingObjectStorageHelper source code](https://github.com/Microsoft/UWPCommunityToolkit/blob/master/Microsoft.Toolkit.Uwp/Helpers/ObjectStorage/RoamingObjectStorageHelper.cs)

