---
title: WebViewExtensions
author: nmetulev
ms.date: 08/20/2017
<<<<<<< HEAD
description: The UWP Community Toolkit WebView extensions allow attaching HTML content to WebView through XAML directly or through Binding
=======
description: The WebView extensions allow attaching HTML content to WebView.
>>>>>>> fb2912293936b8803e6224af5086e6d0c8780bcd
keywords: windows 10, uwp, uwp community toolkit, uwp toolkit, WebViewExtensions, webview, extensions
---

# WebViewExtensions

The **WebView** allows attaching HTML content to WebView.

## Example

<<<<<<< HEAD
```xaml
=======
```xml
>>>>>>> fb2912293936b8803e6224af5086e6d0c8780bcd
	// Attach HTML content directly to WebView.
	<WebView extensions:WebView.Content="{Binding HtmlContent}" />

    // Attach Uri directly to WebView
    <WebView extensions:WebView.ContentUri="{Binding ContentUri}" />
```

## Requirements (Windows 10 Device Family)

<<<<<<< HEAD
| [Device family](http://go.microsoft.com/fwlink/p/?LinkID=526370) | Universal, 10.0.14393.0 or higher |
=======
| [Device family](http://go.microsoft.com/fwlink/p/?LinkID=526370) | Universal, 10.0.10586.0 or higher |
>>>>>>> fb2912293936b8803e6224af5086e6d0c8780bcd
| --- | --- |
| Namespace | Microsoft.Toolkit.Uwp.UI.Extensions |

## API

* [WebViewExtensions source code](https://github.com/Microsoft/UWPCommunityToolkit/tree/master/Microsoft.Toolkit.Uwp.UI/Extensions/WebView)

