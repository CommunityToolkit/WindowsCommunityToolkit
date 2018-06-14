---
title: Facebook Service 
author: nmetulev
description: The Facebook Service allows you to retrieve or publish data to the Facebook graph. Examples of the types of objects you can work with are Posts, Tagged Objects, and the primary user feed.
keywords: windows 10, uwp, windows community toolkit, uwp community toolkit, uwp toolkit, Facebook Service 
dev_langs:
  - csharp
  - vb
---

# Facebook Service 

The Facebook Service allows you to retrieve or publish data to the Facebook graph. Examples of the types of objects you can work with are Posts, Tagged Objects, and the primary user feed.

## Getting Windows Store SID

The Windows Store SID is a unique value per application generated, and it not tied to the actual store publication.  Creating a local application will give you a valid SID that you can use for debugging against Facebook.  

```csharp
// Put the following code in your mainform loaded event
// Note that this will not work in the App.xaml.cs Loaded
#if DEBUG
	System.Diagnostics.Debug.WriteLine("Windows Store SID = " + Microsoft.Toolkit.Uwp.Services.Facebook.FacebookService.Instance.WindowsStoreId);
#endif
```
```vb
' Put the following code in your mainform loaded event
' Note that this will not work in the App.xaml.cs Loaded
#If DEBUG Then
    System.Diagnostics.Debug.WriteLine("Windows Store SID = " & Microsoft.Toolkit.Uwp.Services.Facebook.FacebookService.Instance.WindowsStoreId)
#End If
```

> [!NOTE]
You may have to turn on the Output window in Visual Studio to see this debug writeline.

The above code will output something like this:

```
Windows Store SID = ms-app://s-1-15-2-12341451-1486691014-2395677208-123421631-1234998043-1234490472-123452499/
```

When entering the value into the Facebook Developer site you must strip the ms-app:// and the trailing / off the string.

## Creating a new Application on Facebook Developer Site

1. To get a **Facebook.WindowsStoreID**, go to: https://developers.facebook.com/apps. 
2. Select **Create a New App ID**, to start integration Facebook into your app or website. 
3. Click, **Create a New App**
4. From the app Dashboard choose the **Settings** item on the left.  It should select the *Basic* item under it by default.
5. **+Add Platform** choose Windows App.  Leave the *Namespace* and *App Domains* entries blank.
6. Enter the **Windows Store SID** from within your app (see *Getting Windows Store SID* section)
7. From left side menu choose **+Add Product** Click to add *Facebook Login*.  Ensure you set the following options in the UI: 

| Setting | Value |
|----------|------:|
| Client OAuth Login | Yes |
| Web OAuth Login | No |
| Embedded Browser OAuth Login | Yes |
| Force Web OAuth Redirection | No |
| Login from Devices | No |
| Valid OAuth redirect URIs | Blank |

## Syntax

```csharp
// Initialize service
FacebookService.Instance.Initialize(AppIDText.Text);

// Login to Facebook
if (!await FacebookService.Instance.LoginAsync())
{
    return;
}

// Get user's feed
ListView.ItemsSource = await FacebookService.Instance.RequestAsync(FacebookDataConfig.MyFeed, 50);

// Get current user profile picture
ProfileImage.DataContext = await FacebookService.Instance.GetUserPictureInfoAsync();

// Post a message on your wall
await FacebookService.Instance.PostToFeedAsync(TitleText.Text, MessageText.Text, DescriptionText.Text, UrlText.Text);

// Post a message on your wall using Facebook Dialog
await FacebookService.Instance.PostToFeedWithDialogAsync(TitleText.Text, DescriptionText.Text, UrlText.Text);

// Post a message with a picture on your wall
await FacebookService.Instance.PostToFeedAsync(TitleText.Text, MessageText.Text, DescriptionText.Text, picture.Name, stream);

// Get current user's photo albums
await FacebookService.Instance.GetUserAlbumsAsync();

// Get current user's photos by album Id
await FacebookService.Instance.GetUserPhotosByAlbumIdAsync(addedItem.Id);
```
```vb
' Initialize service
FacebookService.Instance.Initialize(AppIDText.Text)

' Login to Facebook
If Not Await FacebookService.Instance.LoginAsync() Then
    Return
End If

' Get user's feed
ListView.ItemsSource = Await FacebookService.Instance.RequestAsync(FacebookDataConfig.MyFeed, 50)

' Get current user profile picture
ProfileImage.DataContext = Await FacebookService.Instance.GetUserPictureInfoAsync()

' Post a message on your wall
Await FacebookService.Instance.PostToFeedAsync(TitleText.Text, MessageText.Text, DescriptionText.Text, UrlText.Text)

' Post a message on your wall using Facebook Dialog
Await FacebookService.Instance.PostToFeedWithDialogAsync(TitleText.Text, DescriptionText.Text, UrlText.Text)

' Post a message with a picture on your wall
Await FacebookService.Instance.PostToFeedAsync(TitleText.Text, MessageText.Text, DescriptionText.Text, picture.Name, Stream)

' Get current user's photo albums
Await FacebookService.Instance.GetUserAlbumsAsync()

' Get current user's photos by album Id
Await FacebookService.Instance.GetUserPhotosByAlbumIdAsync(addedItem.Id)
```

## FacebookAlbum Class

FacebookAlbum has properties to hold album details

### Properties

| Property | Type | Description |
| -- | -- | -- |
| Cover_Photo | FacebookPhoto | Gets or sets cover_photo property |
| Description | string | Gets or sets description property |
| Id | string | Gets or sets id property |
| Name | string | Gets or sets name property |
| Picture | FacebookPictureData | Gets or sets picture property |

## FacebookDataConfig Class

Configuration object for specifying richer query information

### Fields

| Field | Type | Description |
| -- | -- | -- |
| MyFeed | FacebookDataConfig | Gets a predefined config to get user feed. The feed of posts (including status updates) and links published by this person, or by others on this person's profile |
| MyPosts | FacebookDataConfig | Gets a predefined config to show only the posts that were published by this person |
| MyTagged | FacebookDataConfig | Gets a predefined config to show only the posts that this person was tagged in |

### Properties

| Property | Type | Description |
| -- | -- | -- |
| Query | string |  Gets or sets the query string for filtering service results |

## FacebookOAuthTokens Class

Facebook OAuth tokens

### Properties

| Property | Type | Description |
| -- | -- | -- |
| AppId | string | Gets or sets facebook AppId |
| WindowsStoreId | string | Gets or sets Windows Store ID |

## FacebookPhoto Class

FacebookAlbum has properties to hold photo details

### Properties

| Property | Type | Description |
| -- | -- | -- |
| Album | int | Gets or sets album property |
| Created_Time | int | Gets or sets time the entity instance was created |
| Id | string | Gets or sets id property |
| Images | int | Gets or sets images property |
| Link | string | Gets or sets a link to the entity instance |
| Name | string |  Gets or sets name property |
| Picture | string | Gets or sets picture property |

## FacebookPicture Class

Class for presenting picture data returned from service provider

### Properties

| Property | Type | Description |
| -- | -- | -- |
| Id | string | Gets or sets the ID of the picture |
| Is_Silhouette | bool | Gets or sets a value indicating whether the picture is a silhouette or not |
| Link | string | Gets or sets the url of the page with the picture |
| Url | string | Gets or sets an url to the picture |

## FacebookPictureData Class

Holds picture data

### Properties

| Property | Type | Description |
| -- | -- | -- |
| Data | FacebookPicture | Gets or sets data property |

## FacebookPlatformImageSource Class

Holds image details

### Properties

| Property | Type | Description |
| -- | -- | -- |
| Height | string | Gets or sets height property |
| Source | string | Gets or sets source property |
| Width | string | Gets or sets width property |

## FacebookPost Class

Holds facebook post data

### Properties

| Property | Type | Description |
| -- | -- | -- |
| Id | string | Gets or sets id property |
| Message | string | Gets or sets message or post text |
| Created_Time | DateTime | Gets or sets time the entity instance was created |
| Link | string | Gets or sets a link to the entity instance |
| Full_Picture | string | Gets or sets a link to the accompanying image |

## FacebookRequestSource<T> Class

Type to handle paged requests to Facebook Graph

### Constructor

| Constructor | Description |
| -- | -- |
| FacebookRequestSource(FacebookDataConfig, string, string, int) | Initializes a new instance of the `FacebookRequestSource<T>` class |

### Methods

| Methods | Return Type | Description |
| -- | -- | -- |
| GetPagedItemsAsync(int, int, CancellationToken) | Task<IEnumerable<T>> | Returns strong typed page of data |

## FacebookService Class

Class for connecting to Facebook

### Properties

| Property | Type | Description |
| -- | -- | -- |
| WindowsStoreId | string | Gets a Windows Store ID associated with the current app |
| Instance | FacebookService | Gets public singleton property |
| LoggedUser | string | Gets the current logged user name |
| Provider | FBSession | Gets a reference to an instance of the underlying data provider |
| B | int | Description |
| B | int | Description |

### Methods

| Methods | Return Type | Description |
| -- | -- | -- |
| Initialize(FacebookOAuthTokens, FacebookPermissions) | bool | Initialize underlying provider with relevant token information |
| LoginAsync() | Task<bool> | Login with set of required requiredPermissions |
| LogoutAsync() | Task | Log out of the underlying service instance |
| RequestAsync(FacebookDataConfig, int) | Task<List<FacebookPost>> | Request list data from service provider based upon a given config / query |
| RequestAsync<T>(FacebookDataConfig, int, string) | Task<List<T>> | Request list data from service provider based upon a given config / query |
| RequestAsync(FacebookDataConfig, int, int) | Task<IncrementalLoadingCollection<FacebookRequestSource<FacebookPost>, FacebookPost>> | Request list data from service provider based upon a given config / query |
| RequestAsync<T>(FacebookDataConfig, int, int, string) | Task<IncrementalLoadingCollection<FacebookRequestSource<T>, T>> | Request generic list data from service provider based upon a given config / query |
| GetUserPictureInfoAsync() | Task<FacebookPicture> | Returns the `FacebookPicture` object associated with the logged user |
| GetUserAlbumsAsync(int, string) | Task<List<FacebookAlbum>> | Retrieves list of user photo albums |
| GetUserAlbumsAsync(int, int, string) | Task<IncrementalLoadingCollection<FacebookRequestSource<FacebookAlbum>, FacebookAlbum>> | Retrieves list of user photo albums |
| GetUserPhotosByAlbumIdAsync(string, int, string) | Task<List<FacebookPhoto>> | Retrieves list of user photos by album id |
| GetUserPhotosByAlbumIdAsync(string, int, int, string) | Task<IncrementalLoadingCollection<FacebookRequestSource<FacebookPhoto>, FacebookPhoto>> | Retrieves list of user photos by album id |
| GetPhotoByPhotoIdAsync(string) | Task<FacebookPhoto> | Retrieves a photo by id |
| PostToFeedAsync(string) | Task<bool> | Enables direct posting data to the timeline |
| PostToFeedWithDialogAsync(string) | Task<bool> | Enables posting data to the timeline using Facebook dialog |
| PostPictureToFeedAsync(string, string, IRandomAccessStreamWithContentType) | Task<string> | Enables posting a picture to the timeline |

## Sample Code

[Facebook Service Sample Page Source](https://github.com/Microsoft/WindowsCommunityToolkit//tree/master/Microsoft.Toolkit.Uwp.SampleApp/SamplePages/Facebook%20Service). You can see this in action in [Windows Community Toolkit Sample App](https://www.microsoft.com/store/apps/9NBLGGH4TLCQ).

## Requirements

| Device family | Universal, 10.0.15063.0 or higher |
| --- | --- |
| Namespace | Microsoft.Toolkit.Uwp.Services |
| NuGet package | [Microsoft.Toolkit.Uwp.Services](https://www.nuget.org/packages/Microsoft.Toolkit.Uwp.Services/) |

## API

* [Facebook Service source code](https://github.com/Microsoft/WindowsCommunityToolkit//tree/master/Microsoft.Toolkit.Uwp.Services/Services/Facebook)
