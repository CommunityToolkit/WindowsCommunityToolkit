# Carousel XAML Control

The **Carousel Control** is a great way to cycle through a collection of items. It is supported on all UWP form factors and works with all input modalities.

## Syntax

```xaml
 <controls:Carousel ItemsSource="{x:Bind items, Mode=OneWay}" Orientation="Horizontal">
    <controls:Carousel.ItemTemplate>
        <DataTemplate>
                <Image Source="{Binding}" Width="400"></Image>
        </DataTemplate>
    </controls:Carousel.ItemTemplate>
</controls:Carousel>
```

## Example Image

![Carousel animation](../resources/images/Controls-Carousel.gif "Carousel")

## Example Code

[Carousel Sample Page](https://github.com/Microsoft/UWPCommunityToolkit/tree/master/Microsoft.Toolkit.Uwp.SampleApp/SamplePages/Carousel)

## Default Template 

[Carousel XAML File](https://github.com/Microsoft/UWPCommunityToolkit/blob/master/Microsoft.Toolkit.Uwp.UI.Controls/Carousel/Carousel.xaml) is the XAML template used in the toolkit for the default styling.

## Requirements (Windows 10 Device Family)

| [Device family](http://go.microsoft.com/fwlink/p/?LinkID=526370) | Universal, 10.0.10586.0 or higher |
| --- | --- |
| Namespace | Microsoft.Toolkit.Uwp.UI.Controls |

## API

* [Carousel source code](https://github.com/Microsoft/UWPCommunityToolkit/tree/master/Microsoft.Toolkit.Uwp.UI.Controls/Carousel)