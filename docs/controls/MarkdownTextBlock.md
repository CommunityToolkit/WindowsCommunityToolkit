---
title: MarkdownTextBlock XAML Control
author: nmetulev
ms.date: 08/20/2017
description: The MarkdownTextBlock control provides full markdown parsing and rendering for Universal Windows Apps.
keywords: windows 10, uwp, uwp community toolkit, uwp toolkit, MarkdownTextBlock, xaml, xaml control
---

# MarkdownTextBlock XAML Control 

The *MarkdownTextBlock control* provides full markdown parsing and rendering for Universal Windows Apps. Originally created for the open source reddit app Baconit, the control was engineered to be simple to use and very efficient. One of the main design considerations for the control was it needed to be performant enough to provide a great user experience in virtualized lists. With the custom markdown parser and efficient XAML rendering, we were able to achieve excellent performance; providing a smooth UI experience even with complex Markdown on low end hardware.

Under the hood, the control uses XAML sub elements to build the visual rendering tree for the Markdown input. We chose to use full XAML elements over using the RichEditTextBlock control because the RichEditTextBlock isn’t flexible enough to correctly render all of the standard Markdown styles.

## Syntax

```xml

 <controls:MarkdownTextBlock
    Text="**This is *Markdown*!"
    MarkdownRendered="MarkdownText_MarkdownRendered"
    LinkClicked="MarkdownText_LinkClicked"
    Margin="6">
</controls:MarkdownTextBlock>

```

## Limitations

Here are some limitations you may encounter:

- Images cannot be embedded inside a hyperlink
- All images are stretched with the same stretch value (defined by ImageStretch property)

## Example Image
Note: scrolling is smooth, the gif below is not.

![MarkdownTextBlock animation](../resources/images/Controls-MarkdownTextBlock.gif "MarkdownTextBlock")

## Properties

The MarkdownTextBlock control is highly customizable to blend with any theme. Customizable properties include:

* IsTextSelectionEnabled
* CodeBackground
* CodeBorderBrush
* CodeBorderThickness
* CodeForeground
* CodeFontFamily
* CodeMargin
* CodePadding
* Header1FontWeight
* Header1FontSize
* Header1Margin
* Header2FontWeight
* Header2FontSize
* Header2Margin
* Header3FontWeight
* Header3FontSize
* Header3Margin
* Header4FontWeight
* Header4FontSize
* Header4Margin
* Header5FontWeight
* Header5FontSize
* Header5Margin
* Header6FontWeight
* Header6FontSize
* Header6Margin
* HorizontalRuleBrush
* HorizontalRuleMargin
* HorizontalRuleThickness
* ListMargin
* ListGutterWidth
* ListBulletSpacing
* ParagraphMargin
* QuoteBackground
* QuoteBorderBrush
* QuoteBorderThickness
* QuoteForeground
* QuoteMargin
* QuotePadding
* TableBorderBrush
* TableBorderThickness
* TableCellPadding
* TableMargin
* TextWrapping

## Events

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


## Example Code

[MarkdownTextBlock Sample Page](https://github.com/Microsoft/UWPCommunityToolkit/tree/master/Microsoft.Toolkit.Uwp.SampleApp/SamplePages/MarkdownTextBlock)

## Default Template 

[MarkdownTextBlock XAML File](https://github.com/Microsoft/UWPCommunityToolkit/blob/master/Microsoft.Toolkit.Uwp.UI.Controls/MarkdownTextBlock/MarkdownTextBlock.xaml) is the XAML template used in the toolkit for the default styling.

## Requirements (Windows 10 Device Family)

| [Device family](http://go.microsoft.com/fwlink/p/?LinkID=526370) | Universal, 10.0.14393.0 or higher |
| --- | --- |
| Namespace | Microsoft.Toolkit.Uwp.UI.Controls |

## API

* [MarkdownTextBlock source code](https://github.com/Microsoft/UWPCommunityToolkit/tree/master/Microsoft.Toolkit.Uwp.UI.Controls/MarkdownTextBlock)

