---
title: ProfileCard Control
author: OGcanviz
description: The ProfileCard Control is a simple way to display a user in multiple different formats and mixes of name/image/e-mail.
keywords: windows 10, uwp, windows community toolkit, uwp community toolkit, uwp toolkit, ProfileCard Control
---

# ProfileCard Control

The [ProfileCard Control](https://docs.microsoft.com/dotnet/api/microsoft.toolkit.uwp.ui.controls.graph.profilecard) is a simple way to display a user in multiple different formats and mixes of name/image/e-mail, it relies on the [MicrosoftGraphService](../services/MicrosoftGraph.md) for authentication.

## Syntax

```xaml
<Page ...
    xmlns:controls="using:Microsoft.Toolkit.Uwp.UI.Controls.Graph"/>

<controls:ProfileCard x:Name="ProfileCard1"
    UserId="xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx"
    DisplayMode="PictureOnly" />
```

## Example Image

![ProfileCard animation](../resources/images/Graph/ProfileCard.png)

## Properties

| Property | Type | Description |
| -- | -- | -- |
| RequiredDelegatedPermissions | String[] | Gets required delegated permissions for Graph API access |
| UserId | String | Identifier of the user being displayed, this user id can come from the Graph APIs like `/me/people`, `/users`, etc. |
| DisplayMode | [ViewType](https://github.com/Microsoft/WindowsCommunityToolkit/blob/master/Microsoft.Toolkit.Uwp.UI.Controls.Graph/ProfileCard/ViewType.cs) | The visual layout of the control. Default is `PictureOnly` |
| DefaultImage | BitmapImage | The default image displayed when no user is signed in |
| LargeProfileTitleDefaultText | String | Default title text in LargeProfilePhotoLeft mode or LargeProfilePhotoRight mode when no user is signed in |
| LargeProfileMailDefaultText | String | Default secondary mail text in LargeProfilePhotoLeft mode or LargeProfilePhotoRight mode when no user is signed in |
| NormalMailDefaultText | String | Default mail text in EmailOnly mode when no user is signed in |

## Sample Code

First of all, initialize the [MicrosoftGraphService](../services/MicrosoftGraph.md) with your [Azure AD v2.0 app](https://docs.microsoft.com/en-us/azure/active-directory/develop/active-directory-v2-app-registration), this should be done globally with the combined and unique [delegate permissions](https://docs.microsoft.com/en-us/azure/active-directory/develop/active-directory-v2-scopes) required by all Graph controls and services used in your app.

```c#
MicrosoftGraphService.Instance.AuthenticationModel = MicrosoftGraphEnums.AuthenticationModel.V2;

MicrosoftGraphService.Instance.Initialize(
    'xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx',
    MicrosoftGraphEnums.ServicesToInitialize.UserProfile,
    ProfileCard.RequiredDelegatedPermissions
);
```

[ProfileCard Sample Page Source](https://github.com/Microsoft/WindowsCommunityToolkit/tree/master/Microsoft.Toolkit.Uwp.SampleApp/SamplePages/ProfileCard). You can see this in action in [Windows Community Toolkit Sample App](https://www.microsoft.com/store/apps/9NBLGGH4TLCQ).

## Default Template 

[ProfileCard XAML File](https://github.com/Microsoft/WindowsCommunityToolkit/blob/master/Microsoft.Toolkit.Uwp.UI.Controls.Graph/ProfileCard/ProfileCard.xaml) is the XAML template used in the toolkit for the default styling.

## Requirements

| Device family | Universal, 10.0.15063.0 or higher |
| -- | -- |
| Namespace | Microsoft.Toolkit.Uwp.UI.Controls.Graph |
| NuGet package | [Microsoft.Toolkit.Uwp.UI.Controls.Graph](https://www.nuget.org/packages/Microsoft.Toolkit.Uwp.UI.Controls.Graph/) |

## API

* [ProfileCard source code](https://github.com/Microsoft/WindowsCommunityToolkit/tree/master/Microsoft.Toolkit.Uwp.UI.Controls.Graph/ProfileCard)
