---
title: BackdropGammaTransferBrush
author: mhawker
ms.date: 02/12/2018
description: The BackdropBlurBrush is a Brush that blurs whatever is behind it in the application.
keywords: windows 10, uwp, uwp community toolkit, uwp toolkit, Colors Helper
---

# BackdropGammaTransferBrush

The **BackdropGammaTransferBrush** is a [Brush](https://docs.microsoft.com/en-us/uwp/api/windows.ui.xaml.media.brush) which modifies the color values of whatever is behind it in the application.

## Example

To apply a red hue:

```xaml
    <Border BorderBrush="Black" BorderThickness="1" VerticalAlignment="Center" HorizontalAlignment="Center" Width="400" Height="400">
      <Border.Background>
        <brushes:BackdropGammaTransferBrush RedAmplitude="1.25" />
      </Border.Background>
    </Border>
```

## Properties

See the property reference for the [GammaTransferEffect](http://microsoft.github.io/Win2D/html/T_Microsoft_Graphics_Canvas_Effects_GammaTransferEffect.htm).  

All Amplitude, Disable, Exponent, and Offset properties are available for the Alpha, Red, Green, and Blue channels.

## Requirements (Windows 10 Device Family)

| [Device family](http://go.microsoft.com/fwlink/p/?LinkID=526370) | Universal, 10.0.14393.0 or higher |
| --- | --- |
| Namespace | Microsoft.Toolkit.Uwp |

## API

* [Win2D GammaTransferEffect reference](http://microsoft.github.io/Win2D/html/T_Microsoft_Graphics_Canvas_Effects_GammaTransferEffect.htm)
* [BackdropGammaTransferBrush source code](https://github.com/Microsoft/UWPCommunityToolkit/blob/master/Microsoft.Toolkit.Uwp/Brushes/BackdropGammaTransferBrush.cs)


