---
title: Object Storage
author: nmetulev
description: The Object Storage Helper will help you handle storage of generic objects within UWP applications, both locally and across all devices (roaming).
keywords: windows 10, uwp, windows community toolkit, uwp community toolkit, uwp toolkit, Object Storage, local storage, roaming storage
dev_langs:
  - csharp
  - vb
---

# Object Storage

The Object Storage Helper will help you handle storage of generic objects within UWP applications, both locally and across all devices (roaming).

- [LocalObjectStorageHelper Class](https://docs.microsoft.com/dotnet/api/microsoft.toolkit.uwp.helpers.localobjectstoragehelper) store data in the Local environment (only on the current device)
- [RoamingObjectStorageHelper Class](https://docs.microsoft.com/dotnet/api/microsoft.toolkit.uwp.helpers.roamingobjectstoragehelper)

## Properties

| Property | Type | Description |
| -- | -- | -- |
| Folder | StorageFolder | Gets or sets storage folder |
| Settings | ApplicationDataContainer | Gets or sets settings container |

## Methods

| Methods | Return Type | Description |
| -- | -- | -- |
| FileExistsAsync(String) | Task<bool> | Detect if a file already exists |
| KeyExists(String) | bool | Detect if a setting already exists |
| KeyExists(String, String) | bool | Detect if a setting already exists in composite |
| Read(String, T) | T | Retrieve single item by its key |
| Read(String, String, T) | T | Retrieve single item by its key in composite |
| ReadFileAsync(String, T) | Task<T> | Retrieve object from file |
| Save(String, IDictionary) | void | Save a group of items by its key in a composite. This method should be considered for objects that do not exceed 8k bytes during the lifetime of the application (refers to SaveFileAsync<T>(String, T) for complex/large objects) and for groups of settings which need to be treated in an atomic way |
| Save(String, T) | void | Save single item by its key. This method should be considered for objects that do not exceed 8k bytes during the lifetime of the application (refers to SaveFileAsync<T>(String, T) for complex/large objects) |
| SaveFileAsync(String, T) | Task<StorageFile> | Save object inside file. There is no limitation to use this method (refers to Save<T>(String, T) method for simple objects) |

## Example

### Local Storage

If you need to handle local saves of any object (generic), you can use [LocalObjectStorageHelper](https://docs.microsoft.com/dotnet/api/microsoft.toolkit.uwp.helpers.localobjectstoragehelper).

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
```vb
Dim helper = New LocalObjectStorageHelper()

' Read simple objects
Dim keySimpleObject As String = "simple"
If helper.KeyExists(keySimpleObject) Then
    Dim result As String = helper.Read(Of String)(keySimpleObject)
End If

' Read simple objects in a composite
Dim keyCompositeObject As String = "composite"
If helper.KeyExists(keyCompositeObject, keySimpleObject) Then
    Dim result As String = helper.Read(Of String)(keyCompositeObject, keySimpleObject)
End If

' Save simple objects
helper.Save(keySimpleObject, 47)

' Save simple objects in a composite
Dictionary(Of String, Object)() simpleObjects = New Dictionary(Of String, Object)()
simpleObjects.add("simpleObjectValueOne", 47)
simpleObjects.add("simpleObjectValueTwo", "hello!")
helper.Save(keyCompositeObject, simpleObjects)

' Read complex/large objects 
Dim keyLargeObject As String = "large"
If Await helper.FileExistsAsync(keyLargeObject) Then
    Dim result = Await helper.ReadFileAsync(Of MyLargeObject)(keyLargeObject)
End If

' Save complex/large objects
Dim o = New MyLargeObject With {
    ...
}
Await helper.SaveFileAsync(keySimpleObject, o)
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
```vb
Dim helper = New RoamingObjectStorageHelper()

' Read simple objects
Dim keySimpleObject As String = "simple"
If helper.KeyExists(keySimpleObject) Then
    Dim result As String = helper.Read(Of String)(keySimpleObject)
End If

' Read simple objects in a composite
Dim keyCompositeObject As String = "composite"
If helper.KeyExists(keyCompositeObject, keySimpleObject) Then
    Dim result As String = helper.Read(Of String)(keyCompositeObject, keySimpleObject)
End If

' Save simple objects
helper.Save(keySimpleObject, 47)

' Save simple objects in a composite
Dictionary(Of String, Object)() simpleObjects = New Dictionary(Of String, Object)()
simpleObjects.add("simpleObjectValueOne", 47)
simpleObjects.add("simpleObjectValueTwo", "hello!")
helper.Save(keyCompositeObject, simpleObjects)

' Read complex/large objects 
Dim keyLargeObject As String = "large"
If Await helper.FileExistsAsync(keyLargeObject) Then
    Dim result = Await helper.ReadFileAsync(Of MyLargeObject)(keyLargeObject)
End If

' Save complex/large objects 
Dim o = New MyLargeObject With {
    ...
}
Await helper.SaveFileAsync(keySimpleObject, o)
```

## Sample Code

[Object Storage sample page Source](https://github.com/Microsoft/WindowsCommunityToolkit//tree/master/Microsoft.Toolkit.Uwp.SampleApp/SamplePages/Object%20Storage). You can see this in action in [Windows Community Toolkit Sample App](https://www.microsoft.com/store/apps/9NBLGGH4TLCQ).

## Requirements

| Device family | Universal, 10.0.15063.0 or higher |
| --- | --- |
| Namespace | Microsoft.Toolkit.Uwp |
| NuGet package | [Microsoft.Toolkit.Uwp](https://www.nuget.org/packages/Microsoft.Toolkit.Uwp/) |

## API

* [LocalObjectStorageHelper source code](https://github.com/Microsoft/WindowsCommunityToolkit//blob/master/Microsoft.Toolkit.Uwp/Helpers/ObjectStorage/LocalObjectStorageHelper.cs)
* [RoamingObjectStorageHelper source code](https://github.com/Microsoft/WindowsCommunityToolkit//blob/master/Microsoft.Toolkit.Uwp/Helpers/ObjectStorage/RoamingObjectStorageHelper.cs)
