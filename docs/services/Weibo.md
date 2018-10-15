---
title: 
author: validvoid
description: The Weibo Service allows users to retrieve or publish data to Weibo.
keywords: windows 10, uwp, windows community toolkit, uwp community toolkit, uwp toolkit, Weibo
dev_langs:
  - csharp
---

# Weibo Service

The **Weibo Service** allows users to retrieve or publish data to Weibo. Visit [http://open.weibo.com](http://open.weibo.com) to create a new app or manage existing apps.

## App Setup

Go to [我的应用(my apps)](http://open.weibo.com/apps) and select your app. Then click the *应用信息*(app info) tab on the left sidebar you will be able to find the following fields:

**App Key**
Copy this from the *应用基本信息*(basic information) section on your application page.

**App Secret**
Copy this from the *应用基本信息*(basic information) section on your application page.

**Secure Domains**
Due to the restriction by Weibo API, a status you post must include a url which starts with "http"/"https". You can add a url to the list of secure domains in the *应用基本信息*(basic information) section on your application page.

**Redirect URI** Enter a unique URI for your application.  This must match the *Redirect URL* field in the *OAuth2.0 授权设置*(OAuth 2.0 Authorization Settings) section on the *高级信息*(advanced Info) page. You can visit the page by clicking on the left sidebar. 
*Example*: http://myapp.company.com - (this does not have to be a working URL)

## Syntax

```csharp
// Initialize service
WeiboService.Instance.Initialize(AppKey, AppSecret, RedirectUri);

// Login to Weibo
if (!await WeiboService.Instance.LoginAsync())
{
    return;
}

// Get current user info
var user = await WeiboService.Instance.GetUserAsync();
ProfileImage.DataContext = user;

// Get user timeline
ListView.ItemsSource = await WeiboService.Instance.GetUserTimeLineAsync(user.ScreenName, 50);

// Post a status
await WeiboService.Instance.PostStatusAsync(StatusText.Text);

// Post a status with a picture
await WeiboService.Instance.PostStatusAsync(StatusText.Text, stream);

```

## Methods

| Methods | Return Type | Description |
| -- | -- | -- |
| Initialize(string, string, string, IAuthenticationBroker, IPasswordManager, IStorageManager) | bool | Initialize underlying provider with relevant token information. |
| LoginAsync() | Task&lt;bool&gt; |  Log user in to Weibo. |
| GetUserAsync(string) | Task&lt;WeiboUser&gt; |  Retrieve user data. |
| GetUserTimeLineAsync(string, int) | Task&lt;IEnumerable&lt;WeiboStatus&gt;&gt; |  Retrieve user timeline data. |
| PostStatusAsync(string) | Task&lt;WeiboStatus&gt; |  Post a status. |
| PostStatusAsync(string, Stream) | Task&lt;WeiboStatus&gt; |  Post a status with a picture. |

## Sample Code

[Weibo Service Sample Page Source](https://github.com/Microsoft/WindowsCommunityToolkit//tree/master/Microsoft.Toolkit.Uwp.SampleApp/SamplePages/Weibo%20Service). You can see this in action in [Windows Community Toolkit Sample App](https://www.microsoft.com/store/apps/9NBLGGH4TLCQ).

## Requirements

| Device family | Universal, 10.0.15063.0 or higher |
| --- | --- |
| Namespace | Microsoft.Toolkit.Uwp.Services |
| NuGet package | [Microsoft.Toolkit.Uwp.Services](https://www.nuget.org/packages/Microsoft.Toolkit.Uwp.Services/) |

## API Source Code

- [Weibo Service source code](https://github.com/Microsoft/WindowsCommunityToolkit/tree/master/Microsoft.Toolkit.Services/Services/Weibo)