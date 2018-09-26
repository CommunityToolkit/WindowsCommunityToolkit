---
title: WebViewExtensions
author: nmetulev
description: The Windows Community Toolkit WebView extensions allow attaching HTML content to WebView through XAML directly or through Binding
keywords: windows 10, uwp, windows community toolkit, uwp community toolkit, uwp toolkit, WebViewExtensions, webview, extensions
---

# WebViewExtensions

The [WebViewExtensions](https://docs.microsoft.com/dotnet/api/microsoft.toolkit.uwp.ui.extensions.webview) allows attaching HTML content to WebView.

## Syntax

```xaml
// Attach HTML content directly to WebView
<WebView extensions:WebViewExtensions.Content="{Binding HtmlContent}" />

// Attach Uri directly to WebView
<WebView extensions:WebViewExtensions.ContentUri="{Binding ContentUri}" />
```

## Attached Properties

| Property | Type | Description |
| -- | -- | -- |
| Content | string | Get or set HTML from the WebView through string |
| ContentUri | Uri | Get or set HTML from the WebView through Uri |

## Requirements

| Device family | Universal, 10.0.15063.0 or higher |
| --- | --- |
| Namespace | Microsoft.Toolkit.Uwp.UI.Extensions |
| NuGet package | [Microsoft.Toolkit.Uwp.UI](https://www.nuget.org/packages/Microsoft.Toolkit.Uwp.UI/) |

## API

* [WebViewExtensions source code](https://github.com/Microsoft/WindowsCommunityToolkit//tree/master/Microsoft.Toolkit.Uwp.UI/Extensions/WebView)

