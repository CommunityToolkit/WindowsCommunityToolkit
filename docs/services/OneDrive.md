# OneDrive Service

The **OneDrive** Service provides a simple way to access resources on either OneDrive or OneDrive for Business (Office 365).  You can:

* Authenticate using Microsoft Account, Active Directory or silently using OnlineId
* Access OneDrive folders and files in the same consistent way as local StorageFolder and StorageFile
* Perform CRUD operations on OneDrive resources

## Authentication

In order to use the OneDriveService you need to authenticate the user and get an access token

### OneDrive (Consumer)

OneDrive (Consumer) gives you two options in order to authenticate:

1) If the user is connected to a Windows session with a Microsoft Account, the service is able to silently get an access token.
    For that you need to associate your application to the Store (Project->Store->Associate App With Store...)

2) Or you have to register your app
  - go to https://dev.onedrive.com/app-registration.htm
  - When prompted, sign in with your Microsoft Account credentials.
  - Scroll to the bottom of the page (Live (SDK)), and click Add App
  - Enter your app's name and click Create application.
  - Copy the Application ID
  - Add a platform and select mobile application
  - Save

### OneDrive for Business

OneDrive for Business requires you to register your app in the Azure Management Portal:

OneDrive For Business you need to register your app from the Azure Management Portal
For more information to manualy register your app see go to the following article
https://docs.microsoft.com/fr-fr/azure/active-directory/develop/active-directory-authentication-scenarios#basics-of-registering-an-application-in-azure-ad
When registering your application don't forget to add the Office 365 Sharepoint Online application with the "Read and Write user Files" permissions

## Syntax

### Retrieve the root of your OneDrive

_// By default the service silently connects the current Windows user if Windows is associated with a Microsoft Account_

var folder = await OneDriveService.Instance.RootFolderAsync();

### Initialization

_// if Windows is not associated with a Microsoft Account, you need to initialize the service using an authentication provider AccountProviderType.Msa or AccountProviderType.Adal_

OneDriveService.Instance.Initialize(appClientId, AccountProviderType.Msa, OneDriveScopes.OfflineAccess | OneDriveScopes.ReadWrite);

### Login

if (!await OneDriveService.Instance.LoginAsync())
{
    throw new Exception("Unable to sign in");
}

### Retrieving files

_// Once you have a reference to the Root Folder you can get a list of all items_

_// List the Items from the current folder_

var OneDriveItems = await folder.GetItemsAsync();
do
{
	_//Get the next page of items_
    OneDriveItems = await folder.NextItemsAsync();   
}
while (OneDriveItems != null);

### Creating folders

_// Then from there you can play with folders and files
// Create Folder_

var level1Folder = await rootFolder.CreateFolderAsync("Level1");

var level2Folder = await level1Folder.CreateFolderAsync("Level2");

var level3Folder = await level2Folder.CreateFolderAsync("Level3");


### Retrieving subfolders

_// You can get a sub folder by path_

var level3Folder = await rootFolder.GetFolderAsync("Level1/Level2/Level3");

### Moving, copying and renaming folders

_//Move Folder_

var result=await level3Folder.MoveAsync(rootFolder);

_// Copy Folder_

Var result=level3Folder.CopyAsync(destFolder)

_// Rename Folder_

await level3Folder.RenameAsync("NewLevel3");

### Creating files

_// Create new files_

var selectedFile = await OpenLocalFileAsync(); // e.g. using file picker
if (selectedFile != null)
{
   using (var localStream = await selectedFile.OpenReadAsync())
   {
     var fileCreated = await level3Folder.CreateFileAsync(selectedFile.Name, CreationCollisionOption.GenerateUniqueName, localStream);
   }
}

### Creating files - that exceed 4MB

_// If the file exceed the Maximum size (ie 4MB) use the UploadFileAsync method instead_

var largeFileCreated = await folder.UploadFileAsync(selectedFile.Name, localStream, CreationCollisionOption.GenerateUniqueName, 320 * 1024);

### Moving, copying and renaming files

_// You can also Move, Copy or Rename a file_

await fileCreated.MoveAsync(destFolder);
await fileCreated.CopyAsync(destFolder);
await fileCreated.RenameAsync("newName");

### Downloading files

_// Download a file and save the content in a local file_

var remoteFile=await level3Folder.GetFile("NewFile.docx"); 

using (var remoteStream = await remoteFile.OpenAsync())
 {
     byte[] buffer = new byte[remoteStream.Size];
     var localBuffer = await remoteStream.ReadAsync(buffer.AsBuffer(), (uint)remoteStream.Size, InputStreamOptions.ReadAhead);
	 var localFolder = ApplicationData.Current.LocalFolder;
     var myLocalFile = await localFolder.CreateFileAsync($"{oneDriveFile.Name}", CreationCollisionOption.GenerateUniqueName);
     using (var localStream = await myLocalFile.OpenAsync(FileAccessMode.ReadWrite))
     {
         await localStream.WriteAsync(localBuffer);
         await localStream.FlushAsync();
     }
 }

 ### Retrieving file thumbnails

_// At last you can get the thumbnail of a file_

var stream = await file.GetThumbnailAsync(ThumbnailSize.Large)
Windows.UI.Xaml.Controls.Image thumbnail = new Windows.UI.Xaml.Controls.Image();
BitmapImage bmp = new BitmapImage();
await bmp.SetSourceAsync(streamTodDisplay);
thumbnail.Source = bmp;

  
## Example

[OneDrive Service Sample Page](https://github.com/Microsoft/UWPCommunityToolkit/tree/master/Microsoft.Toolkit.Uwp.SampleApp/SamplePages/OneDrive%20Service)

## Requirements (Windows 10 Device Family)

| [Device family](http://go.microsoft.com/fwlink/p/?LinkID=526370) | Universal, 10.0.10586.0 or higher |
| --- | --- |
| Namespace | Microsoft.Toolkit.Uwp.Services |

## API

* [OneDrive Service source code](https://github.com/Microsoft/UWPCommunityToolkit/tree/master/Microsoft.Toolkit.Uwp.Services/Services/OneDrive)


## NuGet Packages Required

Microsoft.Toolkit.Uwp.Services

See the [NuGet Packages page](../Nuget-Packages.md) for complete list.
