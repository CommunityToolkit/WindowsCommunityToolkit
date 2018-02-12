---
title: BackdropInvertBrush
author: mhawker
ms.date: 02/12/2018
description: The BackdropInvertBrush is a Brush that inverts whatever is behind it in the application.
keywords: windows 10, uwp, uwp community toolkit, uwp toolkit, Colors Helper
---

# BackdropInvertBrush

The **BackdropInvertBrush** is a [Brush](https://docs.microsoft.com/en-us/uwp/api/windows.ui.xaml.media.brush) that inverts whatever is behind it in the application.

It is based on the example provided on [Windows Blogs](https://blogs.windows.com/buildingapps/2017/07/18/working-brushes-content-xaml-visual-layer-interop-part-one/#c57zf3bW4ylLlSvJ.97).

## Example

```xaml
    <Border BorderBrush="Black" BorderThickness="1" VerticalAlignment="Center" HorizontalAlignment="Center" Width="400" Height="400">
      <Border.Background>
        <brushes:BackdropInvertBrush />
      </Border.Background>
    </Border>
```

## Requirements (Windows 10 Device Family)

| [Device family](http://go.microsoft.com/fwlink/p/?LinkID=526370) | Universal, 10.0.14393.0 or higher |
| --- | --- |
| Namespace | Microsoft.Toolkit.Uwp |

## API

* [Win2D InvertEffect reference](http://microsoft.github.io/Win2D/html/T_Microsoft_Graphics_Canvas_Effects_InvertEffect.htm)
* [BackdropInvertBrush source code](https://github.com/Microsoft/UWPCommunityToolkit/blob/master/Microsoft.Toolkit.Uwp/Brushes/BackdropInvertBrush.cs)


