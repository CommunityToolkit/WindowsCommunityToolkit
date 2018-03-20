---
title: AdaptiveGridView XAML Control
author: nmetulev, lukasf, vijay-nirmal, pedrolamas, skendrot, deltakosh, scottisafool, hwaitebt, hermitdave, bkaankose, dotmorten, williamabradley
description: The AdaptiveGridView Control presents items in a evenly-spaced set of columns to fill the total available display space.
keywords: windows 10, uwp, uwp community toolkit, uwp toolkit, AdaptiveGridView, xaml control, xaml
---

# AdaptiveGridView XAML Control 

The **AdaptiveGridView Control** presents items in a evenly-spaced set of columns to fill the total available display space. It reacts to changes in the layout as well as the content so it can adapt to different form factors automatically.

There are 3 ways to use this Control:

1. You can set `DesiredWidth` and `ItemHeight`, which will scale the **width** of each item and adjust the number of columns on demand using all horizontal space.

2. You can set `DesiredWidth` only. This will mean that rows will take up as much space as required, using all horizontal space.

3. Using `OneRowModeEnabled`, you can set `DesiredWidth` and `ItemHeight` which will adjust the **width** of each item and the number of visible columns on demand, using all horizontal space.

## Syntax

```xaml
<controls:AdaptiveGridView  Name="AdaptiveGridViewControl"
    ItemHeight="200"
    DesiredWidth="300"
    ItemTemplate="{StaticResource PhotosTemplate}">
</controls:AdaptiveGridView>
```

## Examples

1. Using `DesiredWidth` in combination with `ItemHeight`:

    ![AdaptiveGridView DesiredHeight and ItemHeight](../resources/images/Controls/AdaptiveGridView/AdaptiveGridView-DesiredWidthItemHeight.gif)

2. Maintain aspect ratio by setting `DesiredWidth` with no `ItemHeight` set:

    ![AdaptiveGridView Viewbox scaled](../resources/images/Controls/AdaptiveGridView/AdaptiveGridView-ViewboxAspectRatio.gif)

    This still requires the `ItemTemplate` to contain some scaling logic, this can be done with Height and Width set on the content inside of a Viewbox, or using custom view logic.

    - Using a `Viewbox`:

        Using the `Height` and `Width` properties of content inside of a `Viewbox` means that the content inside will scale linearly when **Width** and **Height** changes, which causes a zoom-like effect. This might not be a desired effect, and it will also likely incur a slight performance penalty.

        _ItemTemplate implementation_

        ```xaml
        <DataTemplate x:Key="PhotosTemplate">
            <Viewbox>
                <Grid
                    Height="300"
                    Width="200"
                    Background="White"
                    BorderBrush="Black"
                    BorderThickness="1">
                    <Image
                        Source="{Binding Thumbnail}"
                        Stretch="UniformToFill"
                        HorizontalAlignment="Center"
                        VerticalAlignment="Center"/>
                </Grid>
            </Viewbox>
        </DataTemplate>
        ```

    - To use custom view logic:

        Using `MeasureOverride` on a ContentControl allows you to specify the **Width** and **Height** of the content, which might reap better performance compared to a `Viewbox`. The dimensions of the content space will change uniformly, but the content will not zoom.

        _Custom logic implementation_

        ```csharp
        public class AspectContentControl : ContentControl
        {
            protected override Size MeasureOverride(Size availableSize)
            {
                return new Size(availableSize.Width, availableSize.Width * 1.6);
            }
        }
        ```

        _ItemTemplate implementation_

        ```xaml
        <DataTemplate x:Key="PhotosTemplate">
            <local:AspectContentControl 
                HorizontalContentAlignment="Stretch" 
                VerticalContentAlignment="Stretch">
                <Grid Background="White"
                    BorderBrush="Black"
                    BorderThickness="1">
                    <Image
                        Source="{Binding Thumbnail}"
                        Stretch="UniformToFill" />
                </Grid>
            </local:AspectContentControl>
        </DataTemplate>
        ```

3. Using `OneRowModeEnabled`:

    ![AdaptiveGridView OneRowMode](../resources/images/Controls/AdaptiveGridView/AdaptiveGridView-OneRowMode.gif)

    If there are not enough items to fill one row, the control will stretch the items until all available space is filled. This can result in much wider items than specified. If you prefer your items to always stay close to the DesiredWidth, you can set the **StretchContentForSingleRow** property to **false**, to prevent further stretching.

## Example Code

[AdaptiveGridView Sample Page](https://github.com/Microsoft/UWPCommunityToolkit/tree/master/Microsoft.Toolkit.Uwp.SampleApp/SamplePages/AdaptiveGridView)

## Requirements (Windows 10 Device Family)

| [Device family](http://go.microsoft.com/fwlink/p/?LinkID=526370) | Universal, 10.0.14393.0 or higher |
| --- | --- |
| Namespace | Microsoft.Toolkit.Uwp.UI.Controls |

## API

* [AdaptiveGridView source code](https://github.com/Microsoft/UWPCommunityToolkit/tree/master/Microsoft.Toolkit.Uwp.UI.Controls/AdaptiveGridView)
