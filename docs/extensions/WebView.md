---
title: WebViewExtensions
author: nmetulev
ms.date: 08/20/2017
description: The UWP Community Toolkit WebView extensions allow attaching HTML content to WebView through XAML directly or through Binding
keywords: windows 10, uwp, uwp community toolkit, uwp toolkit, WebViewExtensions, webview, extensions
---

# WebViewExtensions

The **WebView** allows attaching HTML content to WebView.

## Example

```xml
	// Attach HTML content directly to WebView.
	<WebView extensions:WebView.Content="{Binding HtmlContent}" />

    // Attach Uri directly to WebView
    <WebView extensions:WebView.ContentUri="{Binding ContentUri}" />
```

## Requirements (Windows 10 Device Family)

| [Device family](http://go.microsoft.com/fwlink/p/?LinkID=526370) | Universal, 10.0.14393.0 or higher |
| --- | --- |
| Namespace | Microsoft.Toolkit.Uwp.UI.Extensions |

## API

* [WebViewExtensions source code](https://github.com/Microsoft/UWPCommunityToolkit/tree/master/Microsoft.Toolkit.Uwp.UI/Extensions/WebView)

