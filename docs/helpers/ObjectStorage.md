---
title: Object Storage
author: nmetulev
ms.date: 08/20/2017
description: The Object Storage Helper will help you handle storage of generic objects within UWP applications, both locally and across all devices (roaming).
keywords: windows 10, uwp, uwp community toolkit, uwp toolkit, Object Storage, local storage, roaming storage
---

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
    
    // Read simple objects in a composite
    string keyCompositeObject = "composite";
    if (helper.KeyExists(keyCompositeObject, keySimpleObject))
    {
        string result = helper.Read<string>(keyCompositeObject, keySimpleObject);
    }

    // Save simple objects
    helper.Save(keySimpleObject, 47);

    // Save simple objects in a composite
    Dictionary<string, object>() simpleObjects = new Dictionary<string, object>();
    simpleObjects.add("simpleObjectValueOne", 47);
    simpleObjects.add("simpleObjectValueTwo", "hello!");
    helper.Save(keyCompositeObject, simpleObjects);

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
    
    // Read simple objects in a composite
    string keyCompositeObject = "composite";
    if (helper.KeyExists(keyCompositeObject, keySimpleObject))
    {
        string result = helper.Read<string>(keyCompositeObject, keySimpleObject);
    }

    // Save simple objects
    helper.Save(keySimpleObject, 47);
    
    // Save simple objects in a composite
    Dictionary<string, object>() simpleObjects = new Dictionary<string, object>();
    simpleObjects.add("simpleObjectValueOne", 47);
    simpleObjects.add("simpleObjectValueTwo", "hello!");
    helper.Save(keyCompositeObject, simpleObjects);

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

| [Device family](http://go.microsoft.com/fwlink/p/?LinkID=526370) | Universal, 10.0.14393.0 or higher |
| --- | --- |
| Namespace | Microsoft.Toolkit.Uwp |

## API
* [LocalObjectStorageHelper source code](https://github.com/Microsoft/UWPCommunityToolkit/blob/master/Microsoft.Toolkit.Uwp/Helpers/ObjectStorage/LocalObjectStorageHelper.cs)
* [RoamingObjectStorageHelper source code](https://github.com/Microsoft/UWPCommunityToolkit/blob/master/Microsoft.Toolkit.Uwp/Helpers/ObjectStorage/RoamingObjectStorageHelper.cs)

