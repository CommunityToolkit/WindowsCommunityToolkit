---
title: BackdropSepiaBrush
author: michael-hawker
ms.date: 02/13/2018
description: The BackdropSepiaBrush is a Brush that applies a Sepia effect to whatever is behind it in the application.
keywords: windows 10, uwp, uwp community toolkit, uwp toolkit, Colors Helper
---

# BackdropSepiaBrush

The **BackdropSepiaBrush** is a [Brush](https://docs.microsoft.com/en-us/uwp/api/windows.ui.xaml.media.brush) that blurs whatever is behind it in the application.

## Example

```xaml
    <Border BorderBrush="Black" BorderThickness="1" VerticalAlignment="Center" HorizontalAlignment="Center" Width="400" Height="400">
      <Border.Background>
        <brushes:BackdropSepiaBrush Intensity="0.75" />
      </Border.Background>
    </Border>
```

## Properties

| Property | Type | Description |
| -- | -- | -- |
| Intensity | double | The `Intensity` property specifies a double value for the amount of Sepia to apply from 0.0 - 1.0.  Zero being none, and one being full Sepia effect.  The default is 0.5. |

## Requirements (Windows 10 Device Family)

| [Device family](http://go.microsoft.com/fwlink/p/?LinkID=526370) | Universal, 10.0.15063.0 or higher |
| --- | --- |
| Namespace | Microsoft.Toolkit.Uwp.UI.Brushes |
| NuGet package | [Microsoft.Toolkit.Uwp.UI](https://www.nuget.org/packages/Microsoft.Toolkit.Uwp.UI/) |

## API Source Code

- [BackdropSepiaBrush source code](https://github.com/Microsoft/UWPCommunityToolkit/blob/master/Microsoft.Toolkit.Uwp/Brushes/BackdropSepiaBrush.cs)

## Related Topics

- [Win2D SepiaEffect reference](http://microsoft.github.io/Win2D/html/T_Microsoft_Graphics_Canvas_Effects_SepiaEffect.htm)
