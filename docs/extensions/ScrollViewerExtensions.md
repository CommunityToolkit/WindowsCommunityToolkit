---
title: ScrollViewer extentions
author: ST-Apps
description: ScrollViewerEx provides a simple way to manage Margin for any ScrollBar inside any container.
keywords: windows 10, uwp, uwp community toolkit, uwp toolkit, ScrollViewer, extentions
---

# ScrollViewer extentions

The [ScrollViewerExtensions](https://docs.microsoft.com/en-us/dotnet/api/microsoft.toolkit.uwp.ui.extensions.scrollviewerextensions) provide extension methods to improve your ScrollViewer implementation.

## ScrollBarMargin

The ScrollBarMargin property provides a way to assign a Thickness to the vertical/horizontal ScrollBar of your container.

### Syntax

```xaml
<ListView extensions:ScrollViewerEx.HorizontalScrollBarMargin="2, 2, 2, 2">
    <!-- ListView Item -->
</ListView>

<ListView extensions:ScrollViewerEx.VerticalScrollBarMargin="2, 2, 2, 2">
    <!-- ListView Item -->
</ListView>
```

### Attached Properties

| Property | Type | Description |
| -- | -- | -- |
| HorizontalScrollBarMargin | Thickness | Set `Thickness` of the horizontal ScrollBar of your container |
| VerticalScrollBarMargin | Thickness | Set `Thickness` of the vertical ScrollBar of your container |

### Example

```xaml
<ListView Name="listView"
            extensions:ScrollViewerEx.VerticalScrollBarMargin="{Binding MinHeight, ElementName=MyHeaderGrid, Converter={StaticResource DoubleTopThicknessConverter}}">
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

The converter is used to just bind to top margin, moving only the ScrollBar's top end.

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

## MiddleClickScrolling

MiddleClickScrolling allows you to scroll by click middle mouse button (scroll wheel button) and move the pointer of the direction to be scrolled. This extension method can be used directly in `ScrollViewer` or ancestor of `ScrollViewer`.

### Syntax

```xaml
<!-- Setting MiddleClickScrolling directely for ScrollViewer -->
<ScrollViewer extensions:ScrollViewerExtensions.EnableMiddleClickScrolling="True">
    <!-- ScrollViewer Content -->
</ScrollViewer>

<!-- Setting MiddleClickScrolling fot the ancestor of ScrollViewer -->
<ListView extensions:ScrollViewerExtensions.EnableMiddleClickScrolling="True">
    <!-- ListView Item -->
</ListView>
```

### Sample Output

![MiddleClickScrolling](../resources/images/Extensions/MiddleClickScrolling.gif)

### Changing Cursor Type

> [!IMPORTANT]
Resource file must be manually added to change the cursor type when middle click scrolling.

#### Using Existing Resource File

1. Download [MiddleClickScrolling-CursorType.res](https://github.com/Microsoft/UWPCommunityToolkit/tree/master/Microsoft.Toolkit.Uwp.UI/Extensions/ScrollViewer/MiddleClickScrolling-CursorType.res) file
2. Move this file into your project's folder
2. Open .csproj file of your project in [Visual Studio Code](https://code.visualstudio.com/) or in any other code editor
3. Added `<Win32Resource>MiddleClickScrolling-CursorType.res</Win32Resource>` in the first `<PropertyGroup>`

### Using Your Own Resource File

- You need 9 cursor resource in your resource file
- Your cursor number should be 101 to 109
- Cursor number 101 must be the centre cursor
- Cursor number 102, 103, 104, 105, 106, 107, 108, 109 must be the NorthArror, NorthEastArror, EastArror, SouthEastArror, SouthArror, SouthWestArror, WestArror, NorthWestArror respectively
- Every cursor will be automatically attached to the corresponding direction of scrolling

### Attached Properties

| Property | Type | Description |
| -- | -- | -- |
| EnableMiddleClickScrolling | bool | Set `true` to enable middle click scrolling |

## Sample Code

[ScrollViewerExtensions sample page Source](https://github.com/Microsoft/UWPCommunityToolkit/tree/master/Microsoft.Toolkit.Uwp.SampleApp/SamplePages/ScrollViewerExtensions). You can see this in action in [UWP Community Toolkit Sample App](https://www.microsoft.com/store/apps/9NBLGGH4TLCQ).

## Requirements

| Device family | Universal, 10.0.14393.0 or higher |
| -- | -- |
| Namespace | Microsoft.Toolkit.Uwp.UI.Extensions |
| NuGet package | [Microsoft.Toolkit.Uwp.UI](https://www.nuget.org/packages/Microsoft.Toolkit.Uwp.UI/) |

## API Source Code

- [ScrollViewerExtensions source code](https://github.com/Microsoft/UWPCommunityToolkit/tree/master/Microsoft.Toolkit.Uwp.UI/Extensions/ScrollViewer)
