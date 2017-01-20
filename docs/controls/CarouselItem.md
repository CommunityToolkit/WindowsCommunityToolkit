# Carousel Item XAML Control

The **CarouselItem** control is used by the [Carousel](http://docs.uwpcommunitytoolkit.com/en/master/controls/Carousel/) control and provides events and properties for each item that allow custom behaviours.

**CarouselItem** is automaticaly generated for each item template when not specified as the top level of the DataTemplate of the Carousel control items. When specified (as in the example below), the developer can disable default animation and provide custom animations on the *ItemGotCarouselFocus* and *ItemLostCarouselFocus* events.

## Syntax

```xaml
 <controls:Carousel ItemsSource="{x:Bind items, Mode=OneWay}" Orientation="Horizontal">
    <controls:Carousel.ItemTemplate>
        <DataTemplate>
			<controls:CarouselItem ItemGotCarouselFocus="CarouselItem_ItemGotCarouselFocus"
                                   ItemLostCarouselFocus="CarouselItem_ItemLostCarouselFocus"
                                   IsActionable="True"
                                   AnimateFocus="False">
                <Image Source="{Binding}" Width="200"></Image>
            </controls:CarouselItem>
        </DataTemplate>
    </controls:Carousel.ItemTemplate>
</controls:Carousel>
```

## Example Image

![CarouselItem animation](../resources/images/Controls-CarouselItem.gif "Carousel")

## Example Code

[CarouselItem Sample Page](https://github.com/Microsoft/UWPCommunityToolkit/tree/master/Microsoft.Toolkit.Uwp.SampleApp/SamplePages/Carousel)

## Default Template 

[CarouselItem XAML File](https://github.com/Microsoft/UWPCommunityToolkit/blob/master/Microsoft.Toolkit.Uwp.UI.Controls/Carousel/Carousel.xaml) is the XAML template used in the toolkit for the default styling.

## Requirements (Windows 10 Device Family)

| [Device family](http://go.microsoft.com/fwlink/p/?LinkID=526370) | Universal, 10.0.10586.0 or higher |
| --- | --- |
| Namespace | Microsoft.Toolkit.Uwp.UI.Controls |

## API

* [CarouselItem source code](https://github.com/Microsoft/UWPCommunityToolkit/tree/master/Microsoft.Toolkit.Uwp.UI.Controls/Carousel)