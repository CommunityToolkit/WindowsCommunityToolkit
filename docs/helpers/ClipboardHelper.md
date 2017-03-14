# ClipboardHelper

The ClipboardHelper can set/get clipboard easier.

## Example

```csharp

    // Set html into clipboard.
    var html = "<div style=\"color:red;\">hello world</div>";
    ClipboardHelper.SetRawHtml(html);

    // Set normal text into clipboard.
    var text = "hello world";
    ClipboardHelper.SetText(text);

    // Get text from clipboard.
    var text = await ClipboardHelper.GetTextAsync();

```

## Requirements (Windows 10 Device Family)

| [Device family](http://go.microsoft.com/fwlink/p/?LinkID=526370) | Universal, 10.0.10586.0 or higher |
| --- | --- |
| Namespace | Microsoft.Toolkit.Uwp |

## API

* [Connection Helper source code](https://github.com/Microsoft/UWPCommunityToolkit/blob/master/Microsoft.Toolkit.Uwp/Helpers/ClipboardHelper.cs)
