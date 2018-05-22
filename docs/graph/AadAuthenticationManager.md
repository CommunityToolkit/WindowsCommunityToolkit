---
title: AadAuthenticationManager class
author: OGcanviz
description: The AadAuthenticationManager provide the ability to manage the Graph authentication.
keywords: windows 10, uwp, windows community toolkit, uwp community toolkit, uwp toolkit, AadAuthenticationManager class
---

# AadAuthenticationManager Class

The AadAuthenticationManager provide the ability to manage the Graph authentication.

## Properties

| Property | Type | Description |
| -- | -- | -- |
| Instance | [AadAuthenticationManager](../../Microsoft.Toolkit.Uwp.UI.Controls.Graph/Core/AadAuthenticationManager.cs) | Public singleton instance |
| ClientId | String | Id of the Azure AD v2.0 app, see this [article](https://docs.microsoft.com/en-us/azure/active-directory/develop/active-directory-v2-app-registration) for how to register a new app with the platform `Native Application` |
| Scopes | string[] | Required permission scopes for the authentication, see this [article](https://docs.microsoft.com/en-us/azure/active-directory/develop/active-directory-v2-scopes) for more details |
| IsAuthenticated | Boolean | Indicates whether the user is signed in |
| CurrentUserId | String | Id of the current user |

## Methods

| Method | Return Type | Description |
| -- | -- | -- |
| Initialize | Void | Initialize the AadAuthenticationManager |

## Sample Code

```c#
AadAuthenticationManager.Instance.Initialize(
    'xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx',
    new string[] { "User.Read", "User.ReadBasic.All" }
);
```

[AadAuthenticationManager Sample Page Source](../../Microsoft.Toolkit.Uwp.SampleApp/Controls/AadAuthControl.xaml.cs). You can see this in action in [Windows Community Toolkit Sample App](https://www.microsoft.com/store/apps/9NBLGGH4TLCQ).

## Requirements

| Device family | Universal, 10.0.14393.0 or higher |
| -- | -- |
| Namespace | Microsoft.Toolkit.Uwp.UI.Controls.Graph |
| NuGet package | [Microsoft.Toolkit.Uwp.UI.Controls.Graph](https://www.nuget.org/packages/Microsoft.Toolkit.Uwp.UI.Controls.Graph/) |

## API

* [AadAuthenticationManager source code](../../Microsoft.Toolkit.Uwp.UI.Controls.Graph/Core/AadAuthenticationManager.cs)
