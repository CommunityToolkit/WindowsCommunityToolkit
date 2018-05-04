---
title: MicrosoftGraph Service
author: nmetulev
description: The MicrosoftGraph Service aim to easily logon to Office 365 Service in order to Retrieve User Information, Retrieve and Send emails, Retrieve User events
keywords: windows 10, uwp, windows community toolkit, uwp community toolkit, uwp toolkit, MicrosoftGraph Service
dev_langs:
  - csharp
  - vb
---

# MicrosoftGraph Service

The **MicrosoftGraph** Service aim to easily logon to Office 365 Service in order to: 

* Retrieve User Information
* Retrieve and Send emails
* Retrieve User events

## Prerequisites

> [!NOTE]
This API will not work on an XBOX UWP Application

### 1. Get and Office 365 Subscription

If you don't have one, you need to create an Office 365 Developer Site. There are several ways to create one:

* [An MSDN subscription](https://msdn.microsoft.com/subscriptions/manage/default.aspx) - This is available to MSDN subscribers with Visual Studio Ultimate and Visual Studio Premium.
* [An existing Office 365 subscription](https://msdn.microsoft.com/library/2ec857d5-dc6f-4cf6-ba45-adc845ef2a25%28Office.15%29.aspx) - You can use an existing Office 365 subscription, which can be any of the following: Office 365 Midsize Business, Office 365 Enterprise, Office 365 Education, Office 365 Government.
* [Free O365 trial](https://portal.office.com/Signup?OfferId=6881A1CB-F4EB-4db3-9F18-388898DAF510&DL=DEVELOPERPACK&ali=1) - You can start with a free 30-day trial, or buy an Office 365 developer subscription.
* [Free O365 Developer](http://dev.office.com/devprogram) - Or Get a One year free Office 365 Developer account
 
### 2. Register you application in Azure Active Directory

To authenticate your app, you need to register your app with Azure AD, and provide some details about your app. You can register your app manually by using the [Azure Management Portal](http://manage.windowsazure.com), or by using Visual Studio.

To register your app manually, see [Manually register your app with Azure AD so it can access Office 365 APIs.](https://msdn.microsoft.com/en-us/office/office365/howto/add-common-consent-manually)

To register your app by using Visual Studio, see [Using Visual Studio to register your app and add Office 365 APIs.](https://msdn.microsoft.com/office/office365/HowTo/adding-service-to-your-Visual-Studio-project)

After you've registered your app, Azure AD will generate a client ID for your app. You'll need to use this client ID to get your access token.

When you register your app in the [Azure Management Portal](http://manage.windowsazure.com), you will need to configure details about your application with the following steps:

1. Specify your application as a **Web application and/or web API**
2. Specify the Redirect Uri as **http://localhost:8000**
3. Add Application: Choose **Microsoft Graph** API 
4. Specify the permission levels the MicrosoftGraph Service requires from the Office 365 API (Microsoft Graph). Choose at least:
   * **Sign in and read user profile** to access user's profile.
   * **Read user mail and Send mail as user** to retrieve/send messages.
   * **Read user calendars** to retrieve events.

**Note:** Once register copy and save the Client ID for future use.
 
|Setting|Value|
|----------|:-------------:|
|Web application and/or web API|Yes|
|Redirect Uri|http://localhost:8080|
|Resource to Add|Microsoft Graph|
|Delegate Permissions |Sign in and read user profile, Read user mail and Send mail, Read user calendars|

## Syntax

### Sign in with an Office 365 account

```csharp
// Initialize the service
if (!MicrosoftGraphService.Instance.Initialize(ClientId.Text))
{
 return;
}
// Login via Azure Active Directory 
if (!await MicrosoftGraphService.Instance.LoginAsync())
{
 return;
}
```
```vb
' Initialize the service
If Not MicrosoftGraphService.Instance.Initialize(ClientId.Text) Then
    Return
End If

' Login via Azure Active Directory
If Not Await MicrosoftGraphService.Instance.LoginAsync() Then
    Return
End If
```

### Get the connected user's info

```csharp
// Retrieve user's info from Azure Active Directory
var user = await MicrosoftGraphService.Instance.User.GetProfileAsync();
UserPanel.DataContext = user;

// You can also select any fields you want in the response
MicrosoftGraphUserFields[] selectedFields = 
{
 MicrosoftGraphUserFields.Id,
 MicrosoftGraphUserFields.DisplayName,
 MicrosoftGraphUserFields.JobTitle,
 MicrosoftGraphUserFields.Mail,
 MicrosoftGraphUserFields.Department,
 MicrosoftGraphUserFields.PreferredLanguage
};

var user =await MicrosoftGraphService.Instance.User.GetProfileAsync(selectedFields);
UserPanel.DataContext = user;

// Retrieve the user's photo 
using (IRandomAccessStream photoStream = await MicrosoftGraphService.Instance.User.GetPhotoAsync())
{
 BitmapImage photo = new BitmapImage();
 if (photoStream != null)
  {
   await photo.SetSourceAsync(photoStream);
  }
  else
  {
   photo.UriSource = new Uri("ms-appx:///SamplePages/MicrosoftGraph Service/user.png");
  }

  this.Photo.Source = photo;
}
```
```vb
' Retrieve user's info from Azure Active Directory
Dim user = Await MicrosoftGraphService.Instance.User.GetProfileAsync()
UserPanel.DataContext = user

' You can also select any fields you want in the response
Dim selectedFields As MicrosoftGraphUserFields() = {
    MicrosoftGraphUserFields.Id,
    MicrosoftGraphUserFields.DisplayName,
    MicrosoftGraphUserFields.JobTitle,
    MicrosoftGraphUserFields.Mail,
    MicrosoftGraphUserFields.Department,
    MicrosoftGraphUserFields.PreferredLanguage
}

Dim user = Await MicrosoftGraphService.Instance.User.GetProfileAsync(selectedFields)
UserPanel.DataContext = user

' Retrieve the user's photo
Using photoStream As IRandomAccessStream = Await MicrosoftGraphService.Instance.User.PhotosService.GetPhotoAsync()
    Dim photo As BitmapImage = New BitmapImage()
    If photoStream IsNot Nothing Then
        Await photo.SetSourceAsync(photoStream)
    Else
        photo.UriSource = New Uri("ms-appx:///SamplePages/MicrosoftGraph Service/user.png")
    End If

    Me.Photo.Source = photo
End Using
```

### Retrieve/Send messages

```csharp
// Get the top 10 messages
messages = await MicrosoftGraphService.Instance.User.Message.GetEmailsAsync(10);
MessagesList.ItemsSource = messages;

// You can also select any fields you want in the response
MicrosoftGraphMessageFields[] selectedFields = 
{ 
 MicrosoftGraphMessageFields.Id,
 MicrosoftGraphMessageFields.From,
 MicrosoftGraphMessageFields.Subject,
 MicrosoftGraphMessageFields.BodyPreview
};

messages = await MicrosoftGraphService.Instance.User.Message.GetEmailsAsync(10,selectedFields);
MessagesList.ItemsSource = messages;

// Request the next 10 messages 
messages = await MicrosoftGraphService.Instance.User.Message.NextPageEmailsAsync();
if (messages == null)
{
     // no more messages
}

// Send a message
string[] toRecipients = { "user1@contoso.com", "user2@contoso.com" };
string subject = "This is the subject of my message;
string content = "This is the content of my message";

await MicrosoftGraphService.Instance.User.Message.SendEmailAsync(subject, content, BodyType.Text, toRecipients);

// You can also send a message in html format
string content = GetHtmlMessage();
await MicrosoftGraphService.Instance.User.Message.SendEmailAsync(subject, content, BodyType.Html, toRecipients);
```
```vb
' Get the top 10 messages
messages = Await MicrosoftGraphService.Instance.User.Message.GetEmailsAsync(10)
MessagesList.ItemsSource = messages

' You can also select any fields you want in the response
Dim selectedFields As MicrosoftGraphMessageFields() = {
    MicrosoftGraphMessageFields.Id,
    MicrosoftGraphMessageFields.From,
    MicrosoftGraphMessageFields.Subject,
    MicrosoftGraphMessageFields.BodyPreview
}
messages = Await MicrosoftGraphService.Instance.User.Message.GetEmailsAsync(10, selectedFields)
MessagesList.ItemsSource = messages

' Request the next 10 messages
messages = Await MicrosoftGraphService.Instance.User.Message.NextPageEmailsAsync()
If messages Is Nothing Then
    ' no more messages
End If

' Send a message
Dim toRecipients As String() = {"user1@contoso.com", "user2@contoso.com"}
Dim subject As String = "This is the subject of my message;"
Dim content As String = "This is the content of my message"
Await MicrosoftGraphService.Instance.User.Message.SendEmailAsync(subject, content, BodyType.Text, toRecipients)

' You can also send a message in html format
Dim content As String = GetHtmlMessage()
Await MicrosoftGraphService.Instance.User.Message.SendEmailAsync(subject, content, BodyType.Html, toRecipients)
```

### Retrieve calendar events

```csharp
// Get the top 10 events
events = await MicrosoftGraphService.Instance.User.Event.GetEventsAsync(10);
EventsList.ItemsSource = events;

// You can also select any fields you want in the response
MicrosoftGraphEventFields[] selectedFields = 
{ 
 MicrosoftGraphEventFields.Id,
 MicrosoftGraphEventFields.Attendees,
 MicrosoftGraphEventFields.Start,
 MicrosoftGraphEventFields.HasAttachments,
 MicrosoftGraphEventFields.Subject,
 MicrosoftGraphEventFields.BodyPreview
};

events = await MicrosoftGraphService.Instance.User.Event.GetEventsAsync(10,selectedFields);
EventsList.ItemsSource = events;

// Request the next 10 events
events = await MicrosoftGraphService.Instance.User.Event.NextPageEventsAsync();
if (events == null)
{
	// no more events
}
```
```vb
' Get the top 10 events
events = Await MicrosoftGraphService.Instance.User.[Event].GetEventsAsync(10)
EventsList.ItemsSource = events

' You can also select any fields you want in the response
Dim selectedFields As MicrosoftGraphEventFields() = {
    MicrosoftGraphEventFields.Id,
    MicrosoftGraphEventFields.Attendees,
    MicrosoftGraphEventFields.Start,
    MicrosoftGraphEventFields.HasAttachments,
    MicrosoftGraphEventFields.Subject,
    MicrosoftGraphEventFields.BodyPreview
}
events = Await MicrosoftGraphService.Instance.User.[Event].GetEventsAsync(10, selectedFields)
EventsList.ItemsSource = events

' Request the next 10 events
events = Await MicrosoftGraphService.Instance.User.[Event].NextPageEventsAsync()
If events Is Nothing Then
    ' no more events
End If
```

## Sample Code

[MicrosoftGraph Service Sample Page Source](https://github.com/Microsoft/UWPCommunityToolkit/tree/master/Microsoft.Toolkit.Uwp.SampleApp/SamplePages/Microsoft%20Graph%20Service). You can see this in action in [Windows Community Toolkit Sample App](https://www.microsoft.com/store/apps/9NBLGGH4TLCQ).

### Requirements

| Device family | Universal, 10.0.14393.0 or higher |
| --- | --- |
| Namespace | Microsoft.Toolkit.Uwp.Services |
| NuGet package | [Microsoft.Toolkit.Uwp.Services](https://www.nuget.org/packages/Microsoft.Toolkit.Uwp.Services/) |

### API

* [MicrosoftGraph Service source code](https://github.com/Microsoft/UWPCommunityToolkit/tree/master/Microsoft.Toolkit.Uwp.Services/Services/MicrosoftGraph)
