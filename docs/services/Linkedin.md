---
title: LinkedIn Service
author: nmetulev
description: The LinkedIn Service allows you to retrieve or publish data to the LinkedIn graph. Examples of the types of objects you can work with are User profile data and sharing Activity.
keywords: windows 10, uwp, windows community toolkit, uwp community toolkit, uwp toolkit, LinkedIn Service
dev_langs:
  - csharp
  - vb
---

# LinkedIn Service

The **LinkedIn Service** allows you to retrieve or publish data to the LinkedIn graph. Examples of the types of objects you can work with are User profile data and sharing Activity.

## Creating a new Application on LinkedIn Developer Site

1. Go to: https://www.linkedin.com/developer/apps.
2. Select **Create Application**, to start integrating LinkedIn into your app.
3. Complete all mandatory fields signified by the red star.  If you agree to the terms and conditions, hit **Submit**.
4. Make a note of the **Client Id** and **Client Secret** for your app - you will need to supply these in your code.
5. Take note of the **Default Application Permissions**.  You can either set these in this portal or via code.  These are the permissions your user will need to agree to for you to make calls on their behalf.
6. Under **OAuth 2.0** you will need to enter a **Authorized Redirect URLs**.  For UWP app development purposes this is arbitrary, but it will need to match what you have in your code (e.g. https://github.com/Microsoft/WindowsCommunityToolkit/).
7. Once you have done, hit **Update**.

## Syntax

```csharp
// Initialize service - use overload if you need to supply additional permissions
LinkedInService.Instance.Initialize(ClientId.Text, ClientSecret.Text, CallbackUri.Text);

// Login to LinkedIn
if (!await LinkedInService.Instance.LoginAsync())
{
    return;
}

// Get current user's profile details
await LinkedInService.Instance.GetUserProfileAsync();

// Share message to LinkedIn (text should include a Url so LinkedIn can scrape preview information)
await LinkedInService.Instance.ShareActivityAsync(ShareText.Text);
```
```vb
' Initialize service - use overload if you need to supply additional permissions
LinkedInService.Instance.Initialize(ClientId.Text, ClientSecret.Text, CallbackUri.Text)

' Login to LinkedIn
If Not Await LinkedInService.Instance.LoginAsync() Then
    Return
End If

' Get current user's profile details
Await LinkedInService.Instance.GetUserProfileAsync()

' Share message to LinkedIn (text should include a Url so LinkedIn can scrape preview information)
Await LinkedInService.Instance.ShareActivityAsync(ShareText.Text)
```

[LinkedIn Service Sample Page Source](https://github.com/Microsoft/WindowsCommunityToolkit//tree/master/Microsoft.Toolkit.Uwp.SampleApp/SamplePages/LinkedIn%20Service). You can see this in action in [Windows Community Toolkit Sample App](https://www.microsoft.com/store/apps/9NBLGGH4TLCQ).

## Using the service on non-UWP platforms

**Note**: The LinkedIn Service also has built-in .NET Framework support. You can skip this section if you'll be using it on that platform.

To use the service outside the UWP platform, you'll need to implement some interfaces. These interfaces are the IAuthenticationBroker, IPasswordManager and IStorageManager.

**IAuthenticationBroker**

The IAuthenticationBroker only has the Authenticate method. This method receives a request uri and a callback uri, which you'll use to authenticate with the API. The method returns an AuthenticationResult that tells the service the authentication result.

**IPasswordManager**

The IPasswordManager will allow the service to manage passwords. The methods you'll have to implement are Get, Store and Remove.

The Get method receives a string key and returns a PasswordCredential.

The Store method receives a string resource and a PasswordCredential.

The Remove method receives a string key.

**IStorageManager**

Finally, the IStorageManager will allow the service to store application data. The methods you'll have to implement are Get and Set.

The Get method receives a string key and returns the saved string.

The Set method receives a string key and a string value.

The toolkit has implementations of each of them for UWP. You can find them as UwpAuthenticationBroker, UwpPasswordManager and UwpStorageManager.

## Requirements

| Device family | Universal, 10.0.15063.0 or higher |
| --- | --- |
| Namespace | Microsoft.Toolkit.Services |
| NuGet package | [Microsoft.Toolkit.Services](https://www.nuget.org/packages/Microsoft.Toolkit.Services/) |

## API

* [LinkedIn Service source code](https://github.com/Microsoft/WindowsCommunityToolkit//tree/master/Microsoft.Toolkit.Uwp.Services/Services/LinkedIn)
