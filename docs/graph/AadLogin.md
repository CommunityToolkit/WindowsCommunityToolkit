---
title: AadLogin Control
author: OGcanviz
description: The AadLogin Control leverages existing .NET login libraries to support basic AAD sign-in processes for Microsoft Graph.
keywords: windows 10, uwp, windows community toolkit, uwp community toolkit, uwp toolkit, AadLogin Control
---

# AadLogin Control

The [AadLogin Control](https://docs.microsoft.com/dotnet/api/microsoft.toolkit.uwp.ui.controls.graph.aadlogin) leverages existing .NET login libraries to support basic AAD sign-in processes for Microsoft Graph, it relies on the [AadAuthenticationManager](../../docs/graph/AadAuthenticationManager.md) for authentication.

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
| View | [ViewType](../../Microsoft.Toolkit.Uwp.UI.Controls.Graph/ProfileCard/ViewType.cs) | The visual layout of the control. Default is `PictureOnly` |
| AllowSignInAsDifferentUser | Boolean | Whether or not the menu item for `Sign in as a different user` is enabled, default value is true |
| SignInDefaultText | String | Default text for sign in button |
| SignOutDefaultText | String | Default text for sign out button |
| SignInAnotherUserDefaultText | String | Default text for `Sign in with another account` button |
| CurrentUserId | String | Gets the unique identifier for current signed in user |

## Methods

| Method | Return Type | Description |
| -- | -- | -- |
| SignInAsync | bool | Method to call when to trigger the user signin.  UX of the control is updated if successful |
| SignOut | void | Method to call to signout the currently signed on user |

## Events

| Method | Type | Description |
| -- | -- | -- |
| SignInCompleted | EventHandler&lt;SignInEventArgs&gt; | Occurs when one of the menu items in the control is clicked. |
| SignOutCompleted | EventHandler | Occurs when the user clicks on SignOut, or the SignOut() method is called. Developers should clear any cached usage of GraphServiceClient objects they receive this event |

## Sample Code

First all all, initialize the Azure AD authentication manager, this should be done globally with all required delegate permissions if multiple Graph controls in this package are used in your app.

```c#
AadAuthenticationManager.Instance.Initialize(
    'xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx',
    new string[] { "User.Read", "User.ReadBasic.All" }
);
```

[AadLogin Sample Page Source](../../Microsoft.Toolkit.Uwp.SampleApp/SamplePages/AadLogin). You can see this in action in [Windows Community Toolkit Sample App](https://www.microsoft.com/store/apps/9NBLGGH4TLCQ).

## Default Template 

[AadLogin XAML File](../../Microsoft.Toolkit.Uwp.UI.Controls.Graph/AadLogin/AadLogin.xaml) is the XAML template used in the toolkit for the default styling.

## Requirements

| Device family | Universal, 10.0.14393.0 or higher |
| -- | -- |
| Namespace | Microsoft.Toolkit.Uwp.UI.Controls.Graph |
| NuGet package | [Microsoft.Toolkit.Uwp.UI.Controls.Graph](https://www.nuget.org/packages/Microsoft.Toolkit.Uwp.UI.Controls.Graph/) |

## API

* [AadLogin source code](../../Microsoft.Toolkit.Uwp.UI.Controls.Graph/AadLogin)
