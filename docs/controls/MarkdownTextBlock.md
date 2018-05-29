---
title: MarkdownTextBlock XAML Control
author: quinndamerell, deltakosh, tipa, haefele, avknaidu, nmetulev, shenchauhan, vijay-nirmal, pedrolamas, williamabradley
description: The MarkdownTextBlock control provides full markdown parsing and rendering for Universal Windows Apps.
keywords: windows 10, uwp, windows community toolkit, uwp community toolkit, uwp toolkit, MarkdownTextBlock, xaml, xaml control
---

# MarkdownTextBlock XAML Control 

The [MarkdownTextBlock control](https://docs.microsoft.com/dotnet/api/microsoft.toolkit.uwp.ui.controls.markdowntextblock) provides full markdown parsing and rendering for Universal Windows Apps. Originally created for the open source reddit app Baconit, the control was engineered to be simple to use and very efficient. One of the main design considerations for the control was it needed to be performant enough to provide a great user experience in virtualized lists. With the custom markdown parser and efficient XAML rendering, we were able to achieve excellent performance; providing a smooth UI experience even with complex Markdown on low end hardware.

Under the hood, the control uses XAML sub elements to build the visual rendering tree for the Markdown input. We chose to use full XAML elements over using the RichEditTextBlock control because the RichEditTextBlock isn't flexible enough to correctly render all of the standard Markdown styles.

## Syntax

```xaml
<controls:MarkdownTextBlock
    Text="**This is *Markdown*!**"
    MarkdownRendered="MarkdownText_MarkdownRendered"
    LinkClicked="MarkdownText_LinkClicked"
    Margin="6">
</controls:MarkdownTextBlock>
```

## Limitations

Here are some limitations you may encounter:

- All images are stretched with the same stretch value (defined by ImageStretch property)
- Relative Links & Relative Images needs to be handled manually using `LinkClicked` event.

## Sample Output

Note: scrolling is smooth, the gif below is not.

![MarkdownTextBlock animation](../resources/images/Controls-MarkdownTextBlock.gif "MarkdownTextBlock")

## Properties

The MarkdownTextBlock control is highly customizable to blend with any theme. Customizable properties include:

| Property | Type | Description |
| -- | -- | -- |
| CodeBackground | Brush | Gets or sets the brush used to fill the background of a code block |
| CodeBorderBrush | Brush | Gets or sets the brush used to render the border fill of a code block |
| CodeBorderThickness | Thickness | Gets or sets the thickness of the border around code blocks |
| CodeFontFamily | FontFamily | Gets or sets the font used to display code. If this is `null`, then Windows.UI.Xaml.Media.FontFamily is used |
| CodeForeground | Brush | Gets or sets the brush used to render the text inside a code block. If this is `null`, then Foreground is used |
| CodeMargin | Thickness | Gets or sets the space between the code border and the text |
| CodePadding | Thickness | Gets or sets space between the code border and the text |
| CodeStyling | StyleDictionary | Gets or sets the Default Code Styling for Code Blocks |
| EmojiFontFamily | FontFamily | Gets or sets the font used to display emojis. If this is `null`, then Segoe UI Emoji font is used |
| Header1FontSize | double | Gets or sets the font size for level 1 headers |
| Header1FontWeight | FontWeight | Gets or sets the font weight to use for level 1 headers |
| Header1Foreground | Brush | Gets or sets the foreground brush for level 1 headers |
| Header1Margin | Thickness | Gets or sets the margin for level 1 headers |
| Header2FontSize | double | Gets or sets the font size for level 2 headers |
| Header2FontWeight | FontWeight | Gets or sets the font weight to use for level 2 headers |
| Header2Foreground | Brush | Gets or sets the foreground brush for level 2 headers |
| Header2Margin | Thickness | Gets or sets the margin for level 2 headers |
| Header3FontSize | double | Gets or sets the font size for level 3 headers |
| Header3FontWeight | FontWeight | Gets or sets the font weight to use for level 3 headers |
| Header3Foreground | Brush | Gets or sets the foreground brush for level 3 headers |
| Header3Margin | Thickness | Gets or sets the margin for level 3 headers |
| Header4FontSize | double | Gets or sets the font size for level 4 headers |
| Header4FontWeight | FontWeight | Gets or sets the font weight to use for level 4 headers |
| Header4Foreground | Brush | Gets or sets the foreground brush for level 4 headers |
| Header4Margin | Thickness | Gets or sets the margin for level 4 headers |
| Header5FontSize | double | Gets or sets the font size for level 5 headers |
| Header5FontWeight | FontWeight | Gets or sets the font weight to use for level 5 headers |
| Header5Foreground | Brush | Gets or sets the foreground brush for level 5 headers |
| Header5Margin | Thickness | Gets or sets the margin for level 5 headers |
| Header6FontSize | double | Gets or sets the font size for level 6 headers |
| Header6FontWeight | FontWeight | Gets or sets the font weight to use for level 6 headers |
| Header6Foreground | Brush | Gets or sets the foreground brush for level 6 headers |
| Header6Margin | Thickness | Gets or sets the margin for level 6 headers |
| HorizontalRuleBrush | Brush | Gets or sets the brush used to render a horizontal rule. If this is `null`, then HorizontalRuleBrush is used |
| HorizontalRuleMargin | Thickness | Gets or sets the margin used for horizontal rules |
| HorizontalRuleThickness | double | Gets or sets the vertical thickness of the horizontal rule |
| ImageMaxHeight | double | Gets or sets the MaxHeight for images |
| ImageMaxWidth | double | Gets or sets the MaxWidth for images |
| ImageStretch | Stretch | Gets or sets the stretch used for images |
| InlineCodeBackground | Brush | Gets or sets the foreground brush for inline code. |
| InlineCodeBorderBrush | Brush | Gets or sets the border brush for inline code |
| InlineCodeBorderThickness | Thickness | Gets or sets the thickness of the border for inline code |
| InlineCodeFontFamily | FontFamily | Gets or sets the font used to display code. If this is `null`, then `Windows.UI.Xaml.Media.FontFamily` is used |
| InlineCodePadding | Thickness | Gets or sets the foreground brush for inline code |
| IsTextSelectionEnabled | bool | Gets or sets a value indicating whether text selection is enabled |
| LinkForeground | Brush | Gets or sets the brush used to render links. If this is `null`, then Foreground is used |
| ListBulletSpacing | double | Gets or sets the space between the list item bullets/numbers and the list item content |
| ListGutterWidth | double | Gets or sets the width of the space used by list item bullets/numbers |
| ListMargin | Thickness | Gets or sets the margin used by lists |
| ParagraphMargin | Thickness | Gets or sets the margin used for paragraphs |
| QuoteBackground | Brush | Gets or sets the brush used to fill the background of a quote block |
| QuoteBorderBrush | Brush | Gets or sets the brush used to render a quote border. If this is null, then [QuoteBorderBrush](https://docs.microsoft.com/dotnet/api/microsoft.toolkit.uwp.ui.controls.markdowntextblock.quoteborderbrush#Microsoft_Toolkit_Uwp_UI_Controls_MarkdownTextBlock_QuoteBorderBrush) is used |
| QuoteBorderThickness | Thickness | Gets or sets the thickness of quote borders. |
| QuoteForeground | Brush | Gets or sets the brush used to render the text inside a quote block. If this is `null`, then Foreground is used |
| QuoteMargin | Thickness | Gets or sets the space outside of quote borders |
| QuotePadding | Thickness | Gets or sets the space between the quote border and the text |
| SchemeList | string(separated by comma) | Gets or sets the custom SchemeList to render a URL. |
| TableBorderBrush | boBrushol | Gets or sets the brush used to render table borders. If this is null, then [TableBorderBrush](https://docs.microsoft.com/dotnet/api/microsoft.toolkit.uwp.ui.controls.markdowntextblock.tableborderbrush#Microsoft_Toolkit_Uwp_UI_Controls_MarkdownTextBlock_TableBorderBrush) is used |
| TableBorderThickness | double | Gets or sets the thickness of any table borders |
| TableCellPadding | Thickness | Gets or sets the padding inside each cell |
| TableMargin | Thickness | Gets or sets the margin used by tables |
| Text | string | Gets or sets the markdown text to display |
| TextWrapping | TextWrapping | Gets or sets the word wrapping behavior |
| UriPrefix | string | Gets or sets the Prefix of Uri |
| UseSyntaxHighlighting | bool | Gets or sets a value indicating whether to use Syntax Highlighting on Code |
| WrapCodeBlock | bool | Gets or sets a value indicating whether to Wrap the Code Block or use a Horizontal Scroll |

## Events

| Events | Description |
| -- | -- |
| CodeBlockResolving | Fired when a Code Block is being Rendered. The default implementation is to output the CodeBlock as Plain Text. You must set `Handled` to `true` in order to process your changes |
| ImageClicked | Fired when an image element in the markdown was tapped |
| ImageResolving | Fired when an image from the markdown document needs to be resolved. The default implementation is basically. You must set `Handled` to `true` in order to process your changes |
| LinkClicked | Fired when a link element in the markdown was tapped |
| MarkdownRendered | Fired when the text is done parsing and formatting. Fires each time the markdown is rendered |

### LinkClicked

Use this event to handle clicking on links for Markdown, by default the MarkdownTextBlock does not handle Clicking on Links.

```c#
private async void MarkdownText_LinkClicked(object sender, LinkClickedEventArgs e)
{
    if (Uri.TryCreate(e.Link, UriKind.Absolute, out Uri link))
    {
        await Launcher.LaunchUriAsync(link);
    }
}
```

### ImageClicked

Use this event to handle clicking on images for Markdown, by default the MarkdownTextBlock does not handle Clicking on Images.

```c#
private async void MarkdownText_ImageClicked(object sender, LinkClickedEventArgs e)
{
    if (Uri.TryCreate(e.Link, UriKind.Absolute, out Uri link))
    {
        await Launcher.LaunchUriAsync(link);
    }
}
```

### ImageResolving

Use this event to customize how images in the markdown document are resolved.  

Set the ImageResolvingEventArgs.Image property to the image that should be shown in the rendered markdown document.  
Also don't forget to set the ImageResolvingEventArgs.Handled flag to true, otherwise your custom image will not be used.

```c#
private void MarkdownText_OnImageResolving(object sender, ImageResolvingEventArgs e)
{
    // This is basically the default implementation
    e.Image = new BitmapImage(new Uri(e.Url));
    e.Handled = true;
}
```

This event also supports loading the image in an asynchronous way.  
Just request a Deferral which you complete when you're done.

```c#
private async void MarkdownText_OnImageResolving(object sender, ImageResolvingEventArgs e)
{
    var deferral = e.GetDeferral();

    e.Image = await GetImageFromDatabaseAsync(e.Url);
    e.Handled = true;

    deferral.Complete();
}
```

### CodeBlockResolving

Use this event to customise how Code Block text is rendered, this is useful for providing Cusom Syntax Highlighting. Built in Syntax Highlighting is already provided with `UseSyntaxHighlighting`.

Manipulate the Inline Collection, and then set e.Handled to true, otherwise the changes won't be processed.

```c#
private void MarkdownText_CodeBlockResolving(object sender, CodeBlockResolvingEventArgs e)
{
    if (e.CodeLanguage == "CUSTOM")
    {
        e.Handled = true;
        e.InlineCollection.Add(new Run { Foreground = new SolidColorBrush(Colors.Red), Text = e.Text, FontWeight = FontWeights.Bold });
    }
}
```

## Rendering

You can customise the rendering of the **MarkdownTextBlock**, by inheriting from `MarkdownRenderer` and setting it as the renderer:

```c#
var block = new MarkdownTextBlock();
block.SetRenderer<InheritedMarkdownRenderer>();
```

This will likely require intimate knowledge of the implementation of the `MarkdownRenderer`, take a look at the following:

* [MarkdownRenderer and Helpers](https://github.com/Microsoft/WindowsCommunityToolkit//blob/master/Microsoft.Toolkit.Uwp.UI.Controls/MarkdownTextBlock/Render)
* [Sample App custom markdown renderer](https://github.com/Microsoft/WindowsCommunityToolkit//blob/master/Microsoft.Toolkit.Uwp.SampleApp/Controls/SampleAppMarkdownRenderer)

## Sample Code

[MarkdownTextBlock Sample Page Source](https://github.com/Microsoft/WindowsCommunityToolkit//tree/master/Microsoft.Toolkit.Uwp.SampleApp/SamplePages/MarkdownTextBlock). You can see this in action in [Windows Community Toolkit Sample App](https://www.microsoft.com/store/apps/9NBLGGH4TLCQ).

## Default Template 

[MarkdownTextBlock XAML File](https://github.com/Microsoft/WindowsCommunityToolkit//blob/master/Microsoft.Toolkit.Uwp.UI.Controls/MarkdownTextBlock/MarkdownTextBlock.xaml) is the XAML template used in the toolkit for the default styling.

## Requirements

| Device family | Universal, 10.0.15063.0 or higher |
| --- | --- |
| Namespace | Microsoft.Toolkit.Uwp.UI.Controls |
| NuGet package | [Microsoft.Toolkit.Uwp.UI.Controls](https://www.nuget.org/packages/Microsoft.Toolkit.Uwp.UI.Controls/) |

## API

* [MarkdownTextBlock source code](https://github.com/Microsoft/WindowsCommunityToolkit//tree/master/Microsoft.Toolkit.Uwp.UI.Controls/MarkdownTextBlock)

## Related Topics

* [Markdown Parser](https://docs.microsoft.com/en-us/windows/uwpcommunitytoolkit/parsers/markdownparser)
