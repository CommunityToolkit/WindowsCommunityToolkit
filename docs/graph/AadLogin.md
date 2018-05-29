---
title: AadLogin Control
author: OGcanviz
description: The AadLogin Control leverages existing .NET login libraries to support basic AAD sign-in processes for Microsoft Graph.
keywords: windows 10, uwp, windows community toolkit, uwp community toolkit, uwp toolkit, AadLogin Control
---

# AadLogin Control

The [AadLogin Control](https://docs.microsoft.com/dotnet/api/microsoft.toolkit.uwp.ui.controls.graph.aadlogin) leverages existing .NET login libraries to support basic AAD sign-in processes for Microsoft Graph, it relies on the [MicrosoftGraphService](../services/MicrosoftGraph.md) for authentication.

## Syntax

```xaml
<Page ...
    xmlns:controls="using:Microsoft.Toolkit.Uwp.UI.Controls.Graph"/>

<controls:AadLogin x:Name="AADLogin1"
    View="PictureOnly"
    AllowSignInAsDifferentUser="True" />
```

## Example Image

![AadLogin animation](../resources/images/Graph/AadLogin.png)

## Properties

| Property | Type | Description |
| -- | -- | -- |
| RequiredDelegatedPermissions | String[] | Gets required delegated permissions for Graph API access |
| DefaultImage | BitmapImage | The default image displayed when no user is signed in |
| View | [ViewType](https://github.com/Microsoft/WindowsCommunityToolkit/blob/master/Microsoft.Toolkit.Uwp.UI.Controls.Graph/ProfileCard/ViewType.cs) | The visual layout of the control. Default is `PictureOnly` |
| AllowSignInAsDifferentUser | Boolean | Whether or not the menu item for `Sign in as a different user` is enabled, default value is true |
| SignInDefaultText | String | Default text for sign in button |
| SignOutDefaultText | String | Default text for sign out button |
| SignInAnotherUserDefaultText | String | Default text for `Sign in with another account` button |
| CurrentUserId | String | Gets the unique identifier for current signed in user |

## Methods

| Method | Return Type | Description |
| -- | -- | -- |
| SignInAsync | bool | Method to call when to trigger the user signin.  UX of the control is updated if successful. Returns false if the user cancels sign in |
| SignOut | void | Method to call to signout the currently signed on user |

## Events

| Method | Type | Description |
| -- | -- | -- |
| SignInCompleted | EventHandler&lt;SignInEventArgs&gt; | Occurs when a user signs in |
| SignInFailed | EventHandler&lt;SignInFailedEventArgs&gt; | Occurs when sign in failed when attempting to sign in. Only fires when an exception occurs during the sign in process and not when the user cancels sign in. |
| SignOutCompleted | EventHandler | Occurs when the user clicks on SignOut, or the SignOut() method is called. Developers should clear any cached usage of GraphServiceClient objects they receive this event |

## Sample Code

First of all, initialize the [MicrosoftGraphService](../services/MicrosoftGraph.md) with your [Azure AD v2.0 app](https://docs.microsoft.com/en-us/azure/active-directory/develop/active-directory-v2-app-registration), this should be done globally with the combined and unique [delegate permissions](https://docs.microsoft.com/en-us/azure/active-directory/develop/active-directory-v2-scopes) required by all Graph controls and services used in your app.

```c#
MicrosoftGraphService.Instance.AuthenticationModel = MicrosoftGraphEnums.AuthenticationModel.V2;

MicrosoftGraphService.Instance.Initialize(
    'xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx',
    MicrosoftGraphEnums.ServicesToInitialize.UserProfile,
    AadLogin.RequiredDelegatedPermissions
);
```

[AadLogin Sample Page Source](https://github.com/Microsoft/WindowsCommunityToolkit/tree/master/Microsoft.Toolkit.Uwp.SampleApp/SamplePages/AadLogin). You can see this in action in [Windows Community Toolkit Sample App](https://www.microsoft.com/store/apps/9NBLGGH4TLCQ).

## Default Template 

[AadLogin XAML File](https://github.com/Microsoft/WindowsCommunityToolkit/blob/master/Microsoft.Toolkit.Uwp.UI.Controls.Graph/AadLogin/AadLogin.xaml) is the XAML template used in the toolkit for the default styling.

## Requirements

| Device family | Universal, 10.0.15063.0 or higher |
| -- | -- |
| Namespace | Microsoft.Toolkit.Uwp.UI.Controls.Graph |
| NuGet package | [Microsoft.Toolkit.Uwp.UI.Controls.Graph](https://www.nuget.org/packages/Microsoft.Toolkit.Uwp.UI.Controls.Graph/) |

## API

* [AadLogin source code](https://github.com/Microsoft/WindowsCommunityToolkit/tree/master/Microsoft.Toolkit.Uwp.UI.Controls.Graph/AadLogin)
