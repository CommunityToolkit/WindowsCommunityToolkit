---
title: ScrollViewer extentions
author: ST-Apps
description: ScrollViewerEx provides a simple way to manage Margin for any ScrollBar inside any container.
keywords: windows 10, uwp, windows community toolkit, uwp community toolkit, uwp toolkit, ScrollViewer, extentions
---

# ScrollViewer extentions

ScrollViewer extensions provide a simple way to manage Margin for any ScrollBar inside any container.

## VerticalScrollBarMargin

The VerticalScrollBarMargin property provides a way to assign a Thickness to the vertical ScrollBar of your container.

### Example

```xaml
    <ListView Name="listView"
              extensions:ScrollViewerExtensions.VerticalScrollBarMargin="{Binding MinHeight, ElementName=MyHeaderGrid, Converter={StaticResource DoubleTopThicknessConverter}}">
            <ListView.Header>
                <controls:ScrollHeader Mode="Sticky">
                    <Grid x:Name="MyHeaderGrid"
                          MinHeight="100"
                          Background="{ThemeResource SystemControlAccentAcrylicElementAccentMediumHighBrush}">
                        <StackPanel HorizontalAlignment="Center"
                                    VerticalAlignment="Center">
                            <TextBlock Margin="12"
                                       FontSize="48"
                                       FontWeight="Bold"
                                       Foreground="{StaticResource Brush-White}"
                                       Text="Scroll Header"
                                       TextAlignment="Center"
                                       TextWrapping="WrapWholeWords" />
                        </StackPanel>
                    </Grid>
                </controls:ScrollHeader>
            </ListView.Header>
			...
	</ListView>
```

```c#
class DoubleTopThicknessConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        return new Thickness(0, (double)value, 0, 0);
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        return ((Thickness)value).Top;
    }
}
```

The converter is used to just bind to top margin, moving only the ScrollBar's top end.

## HorizontalScrollBarMargin

The HorizontalScrollBarMargin works exactly as VerticalScrollBarMargin.

## Requirements (Windows 10 Device Family)

| [Device family](http://go.microsoft.com/fwlink/p/?LinkID=526370) | Universal, 10.0.14393.0 or higher |
| --- | --- |
| Namespace | Microsoft.Toolkit.Uwp.UI.Extensions |

## API

* [ScrollViewerEx source code](https://github.com/Microsoft/UWPCommunityToolkit/blob/master/Microsoft.Toolkit.Uwp.UI/Extensions/ScrollViewer)