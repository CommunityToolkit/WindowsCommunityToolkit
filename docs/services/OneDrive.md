---
title: OneDrive Service
author: tgoodhew
description: The OneDrive Service provides a simple way to access resources on either OneDrive or OneDrive for Business (Office 365).
keywords: windows 10, uwp, windows community toolkit, uwp community toolkit, uwp toolkit, OneDrive
dev_langs:
  - csharp
  - vb
---

# OneDrive Service

The **OneDrive** Service provides an easy to use service helper for the [OneDrive Developer Platform](https://docs.microsoft.com/en-us/onedrive/developer/) that uses the [Microsoft Graph](https://developer.microsoft.com/en-us/graph/docs/concepts/overview). The new OneDrive API is REST API that brings together both personal and work accounts in a single authentication model. The OneDrive Service helps you:

* Initialize and authenticate with a common set of objects
* Access OneDrive, OneDrive for Business, SharePoint document libraries, and Office Groups, to allow your app the flexibility to read and store content in any of these locations with the same code
* Perform CRUD operations on OneDrive resources

## Getting Started

To use the OneDrive API, you need to have an access token that authenticates your app to a particular set of permissions for a user. In this section, you'll learn how to:

1. Register your application to get a client ID and a client secret.
2. Sign your user in to OneDrive with the specified scopes using the token flow or code flow.
3. Sign the user out (optional).

### Register Your App

To connect with Microsoft Graph, you'll need a work/school account or a Microsoft account.

1. Go to the [Microsoft Application Registration Portal.](https://apps.dev.microsoft.com/)
2. When prompted, sign in with your account credentials.
3. Find My applications and click Add an app.
4. Enter your app's name.
5. Check "Let us help you get started" and click "Create".
6. Select "Mobile and Desktop App" as the kind of app.
7. Click "Windows Desktop (.NET) Start Guided Setup".
8. Complete the guided setup flow.

After you've completed these steps, an application ID is created for your app and displayed on your new app's properties page.

### Authentication

Before your app can make requests to OneDrive, it needs a user to authenticate and authorize the application to have access to their data. Microsoft Graph, OneDrive, and SharePoint support using a standard OAuth2 or Open ID Connect authorization flow. Requests to Microsoft Graph are authenticated using bearer tokens obtained from one of these flows.

#### Manifest capabilities for authentication

You will need to add:

< Capability Name="privateNetworkClientServer" />

to your application manifest to enable AAD authentication. Capabilities in the manifest are described in more detail in this document [Capability](https://docs.microsoft.com/en-us/uwp/schemas/appxpackage/appxmanifestschema/element-capability)

Authentication, sign-in and permission scopes are discussed in more detail in this document, [Authorization and sign-in for OneDrive in Microsoft Graph](https://docs.microsoft.com/en-us/onedrive/developer/rest-api/getting-started/graph-oauth)

### Testing access to the OneDrive API
Registering your applicatioin creates an App ID/Client and you can simply paste that into the Client Id field inside of the OneDrive services page.  

## Syntax

### Initialization

```csharp
// Using the new converged authentication of the Microsoft Graph we can simply
// call the Initialize method on the OneDriveService singleton when initializing
// in UWP applications
Microsoft.Toolkit.Services.OneDrive.OneDriveService.Instance.Initialize
    (appClientId, 
     scopes, 
     null, 
     null);
```
```vb
' Using the new converged authentication of the Microsoft Graph we can simply
' call the Initialize method on the OneDriveService singleton when initializing
' in UWP applications
Microsoft.Toolkit.Services.OneDrive.OneDriveService.Instance.Initialize(appClientId, scopes, Nothing, Nothing)
```

### Defining scopes
More information on scopes can be found in this document [Authentication scopes](https://docs.microsoft.com/en-us/onedrive/developer/rest-api/getting-started/msa-oauth#authentication-scopes)

```csharp
// If the user hasn't selected a scope then set it to FilesReadAll
if (scopes == null)
{
    scopes = new string[] { MicrosoftGraphScope.FilesReadAll };
}
```
```vb
' If the user hasn't selected a scope then set it to FilesReadAll
If scopes Is Nothing Then
    scopes = New String() {MicrosoftGraphScope.FilesReadAll}
End If
```

### Login
```csharp
// Login
if (!await OneDriveService.Instance.LoginAsync())
{
    throw new Exception("Unable to sign in");
}
```
```vb
' Login
If Not Await OneDriveService.Instance.LoginAsync() Then
    Throw New Exception("Unable to sign in")
End If
```

### Retrieve the root of your OneDrive

```csharp
var folder = await OneDriveService.Instance.RootFolderForMeAsync();
```
```vb
Dim folder = Await OneDriveService.Instance.RootFolderForMeAsync()
```

### Retrieving files

```csharp
// Once you have a reference to the Root Folder you can get a list of all items
// List the Items from the current folder
var OneDriveItems = await folder.GetItemsAsync();
do
{
    // Get the next page of items
    OneDriveItems = await folder.NextItemsAsync();
}
while (OneDriveItems != null);
```
```vb
' Once you have a reference to the Root Folder you can get a list of all items
' List the Items from the current folder
Dim OneDriveItems = Await folder.GetItemsAsync()
Do
    ' Get the next page of items
    OneDriveItems = Await folder.NextItemsAsync()
Loop While OneDriveItems IsNot Nothing
```

### Creating folders

```csharp
// Then from there you can play with folders and files
// Create Folder
string newFolderName = await OneDriveSampleHelpers.InputTextDialogAsync("New Folder Name");
if (!string.IsNullOrEmpty(newFolderName))
{
    await folder.StorageFolderPlatformService.CreateFolderAsync(newFolderName, CreationCollisionOption.GenerateUniqueName);
}
```
```vb
' Then from there you can play with folders and files
' Create Folder
Dim newFolderName As String = Await OneDriveSampleHelpers.InputTextDialogAsync("New Folder Name")
If Not String.IsNullOrEmpty(newFolderName) Then
    Await folder.StorageFolderPlatformService.CreateFolderAsync(newFolderName, CreationCollisionOption.GenerateUniqueName)
End If
```

### Navigating subfolders

```csharp
var currentFolder = await _graphCurrentFolder.GetFolderAsync(item.Name);
OneDriveItemsList.ItemsSource = await currentFolder.GetItemsAsync(20);
_graphCurrentFolder = currentFolder;
```
```vb
Dim currentFolder = Await _graphCurrentFolder.GetFolderAsync(item.Name)
OneDriveItemsList.ItemsSource = Await currentFolder.GetItemsAsync(20)
_graphCurrentFolder = currentFolder
```

### Moving, copying and renaming items

```csharp
// OneDrive API treats all items the same whether file, folder, etc.
// Move item
await _onedriveStorageItem.MoveAsync(targetonedriveStorageFolder);

// Copy Folder
await _onedriveStorageItem.CopyAsync(targetonedriveStorageFolder);

// Rename Folder
await _onedriveStorageItem.RenameAsync("NewLevel3");
```
```vb
' OneDrive API treats all items the same whether file, folder, etc.
' Move Folder
Await _onedriveStorageItem.MoveAsync(targetonedriveStorageFolder)

' Copy Folder
Await _onedriveStorageItem.CopyAsync(targetonedriveStorageFolder)

' Rename Folder
Await _onedriveStorageItem.RenameAsync("NewLevel3")
```

### Creating or uploading files less than 4MB

```csharp
// Open the local file or create a local file if brand new
var selectedFile = await OpenLocalFileAsync();
if (selectedFile != null)
{
    using (var localStream = await selectedFile.OpenReadAsync())
    {
        var fileCreated = await folder.StorageFolderPlatformService.CreateFileAsync(selectedFile.Name, CreationCollisionOption.GenerateUniqueName, localStream);
    }
}
```
```vb
' Open the local file or create a local file if brand new
Dim selectedFile = Await OpenLocalFileAsync()
If selectedFile IsNot Nothing Then
    Using localStream = Await selectedFile.OpenReadAsync()
        Dim fileCreated = Await level3Folder.CreateFileAsync(selectedFile.Name, CreationCollisionOption.GenerateUniqueName, localStream)
    End Using
End If
```

### Creating or uploading files - that exceed 4MB

```csharp
var selectedFile = await OpenLocalFileAsync();
if (selectedFile != null)
    {
        using (var localStream = await selectedFile.OpenReadAsync())
        {
            Shell.Current.DisplayWaitRing = true;

            // If the file exceed the Maximum size (ie 4MB)
            var largeFileCreated = await folder.StorageFolderPlatformService.UploadFileAsync(selectedFile.Name, localStream, CreationCollisionOption.GenerateUniqueName, 320 * 1024);
        }
    }
}
```
```vb
Dim selectedFile = Await OpenLocalFileAsync()
If selectedFile IsNot Nothing Then
    Using localStream = Await selectedFile.OpenReadAsync()
        Shell.Current.DisplayWaitRing = True

        ' If the file exceed the Maximum size (ie 4MB)
        Dim largeFileCreated = Await folder.StorageFolderPlatformService.UploadFileAsync(selectedFile.Name, localStream, CreationCollisionOption.GenerateUniqueName, 320 * 1024)
    End Using
End If
```

### Downloading files

```csharp
// Download a file and save the content in a local file
// Convert the storage item to a storage file
var oneDriveFile = (Toolkit.Services.OneDrive.OneDriveStorageFile)item;
using (var remoteStream = (await oneDriveFile.StorageFilePlatformService.OpenAsync()) as IRandomAccessStream)
{
    // Use a helper method to open local filestream and write to it 
    await SaveToLocalFolder(remoteStream, oneDriveFile.Name);
}
```
```vb
' Download a file and save the content in a local file
' Convert the storage item to a storage file
Dim oneDriveFile = CType(item, Toolkit.Services.OneDrive.OneDriveStorageFile)
Using remoteStream = TryCast((Await oneDriveFile.StorageFilePlatformService.OpenAsync()), IRandomAccessStream)
    ' Use a helper method to open local filestream and write to it
    Await SaveToLocalFolder(remoteStream, oneDriveFile.Name)
End Using
```

### Retrieving file thumbnails

```csharp
var file = (Toolkit.Services.OneDrive.OneDriveStorageItem)((AppBarButton)e.OriginalSource).DataContext;
using (var stream = (await file.StorageItemPlatformService.GetThumbnailAsync(Toolkit.Services.MicrosoftGraph.MicrosoftGraphEnums.ThumbnailSize.Large)) as IRandomAccessStream)
{
    // Use a helper method to display the images on the xaml view
    await OneDriveSampleHelpers.DisplayThumbnail(stream, "thumbnail");
}
```
```vb
Dim file = CType((CType(e.OriginalSource, AppBarButton)).DataContext, Toolkit.Services.OneDrive.OneDriveStorageItem)
Using stream = TryCast((Await file.StorageItemPlatformService.GetThumbnailAsync(Toolkit.Services.MicrosoftGraph.MicrosoftGraphEnums.ThumbnailSize.Large)), IRandomAccessStream)
    ' Use a helper method to display the images on the xaml view
    Await OneDriveSampleHelpers.DisplayThumbnail(stream, "thumbnail")
End Using
```
  
## Sample Code

[OneDrive Service Sample Page Source](https://github.com/Microsoft/WindowsCommunityToolkit//tree/master/Microsoft.Toolkit.Uwp.SampleApp/SamplePages/OneDrive%20Service). You can see this in action in [Windows Community Toolkit Sample App](https://www.microsoft.com/store/apps/9NBLGGH4TLCQ).

## Requirements

| Device family | Universal, 10.0.15063.0 or higher |
| --- | --- |
| Namespace | Microsoft.Toolkit.Services |
| NuGet package | [Microsoft.Toolkit.Services](https://www.nuget.org/packages/Microsoft.Toolkit.Services/) |

## API

* [OneDrive Service source code](https://github.com/Microsoft/WindowsCommunityToolkit//tree/master/Microsoft.Toolkit.Services/Services/OneDrive)
