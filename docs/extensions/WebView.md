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

| [Device family](http://go.microsoft.com/fwlink/p/?LinkID=526370) | Universal, 10.0.10586.0 or higher |
| --- | --- |
| Namespace | Microsoft.Toolkit.Uwp.UI.Extensions |

## API

* [WebViewExtensions source code](https://github.com/Microsoft/UWPCommunityToolkit/blob/dev/Microsoft.Toolkit.Uwp.UI/Extensions/Webview)

