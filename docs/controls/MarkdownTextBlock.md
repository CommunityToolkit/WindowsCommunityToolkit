# MarkdownTextBlock XAML Control 

The *MarkdownTextBlock XAML control* provides full markdown parsing and rendering for Universal Windows Apps. Originally created for the open source reddit app Baconit, the control was built to be simple to use and very efficient. One of the main design considerations for the control was it needed to be performant enough to provide a great user experience in virtualized lists. With the custom markdown parser and efficient XAML rendering, we were able to achieve excellent performance; providing a smooth UI experience even with complex Markdown on low end hardware.

Under the hood, the control uses XAML sub elements to build the visual rendering tree for the Markdown input. We chose to use full XAML elements over using the RichEditTextBlock control because the RichEditTextBlock isn’t flexible enough to correctly render all of the standard Markdown styles.

## Syntax

```xaml

 <controls:MarkdownTextBlock
    Text="**This is *Markdown*!"
    OnMarkdownRendered="MarkdownText_OnMarkdownRendered"
    OnLinkClicked="MarkdownText_OnLinkClicked"
    Margin="6">
</controls:MarkdownTextBlock>

```

## Example Image

![MarkdownTextBlock animation](../resources/images/Controls-AdaptiveGridView.gif "AdaptiveGridView")

## Example Code

[MarkdownTextBlock Sample Page](https://github.com/Microsoft/UWPCommunityToolkit/tree/master/Microsoft.Toolkit.Uwp.SampleApp/SamplePages/MarkdownTextBlock)

## Default Template 

[MarkdownTextBlock XAML File](https://github.com/Microsoft/UWPCommunityToolkit/blob/master/Microsoft.Toolkit.Uwp.UI.Controls/MarkdownTextBlock/MarkdownTextBlock.xaml) is the XAML template used in the toolkit for the default styling.

## Requirements (Windows 10 Device Family)

| [Device family](http://go.microsoft.com/fwlink/p/?LinkID=526370) | Universal, 10.0.10586.0 or higher |
| --- | --- |
| Namespace | Microsoft.Toolkit.Uwp.UI.Controls |

## API

* [MarkdownTextBlock source code](https://github.com/Microsoft/UWPCommunityToolkit/tree/master/Microsoft.Toolkit.Uwp.UI.Controls/MarkdownTextBlock)

