---
title: BackdropGammaTransferBrush
author: michael-hawker
description: The BackdropBlurBrush is a Brush that blurs whatever is behind it in the application.
keywords: windows 10, uwp, uwp community toolkit, uwp toolkit, brush, backdrop, gamma, color
---

# BackdropGammaTransferBrush

The **BackdropGammaTransferBrush** is a [Brush](https://docs.microsoft.com/en-us/uwp/api/windows.ui.xaml.media.brush) which modifies the color values of whatever is behind it in the application.

## Example

To apply a red hue:

```xaml
    <Border BorderBrush="Black" BorderThickness="1" VerticalAlignment="Center" HorizontalAlignment="Center" Width="400" Height="400">
      <Border.Background>
        <media:BackdropGammaTransferBrush RedAmplitude="1.25" />
      </Border.Background>
    </Border>
```

## Example Image

![Backdrop Gamma](../resources/images/Brushes-BackdropGamma.jpg "Backdrop Gamma")

## Properties

See the property reference for the [GammaTransferEffect](http://microsoft.github.io/Win2D/html/T_Microsoft_Graphics_Canvas_Effects_GammaTransferEffect.htm).  

All Amplitude, Disable, Exponent, and Offset properties are available for the Alpha, Red, Green, and Blue channels.

## Requirements (Windows 10 Device Family)

| [Device family](http://go.microsoft.com/fwlink/p/?LinkID=526370) | Universal, 10.0.15063.0 or higher |
| --- | --- |
| Namespace | Microsoft.Toolkit.Uwp.UI.Media |
| NuGet package | [Microsoft.Toolkit.Uwp.UI](https://www.nuget.org/packages/Microsoft.Toolkit.Uwp.UI/) |

## API Source Code

- [BackdropGammaTransferBrush source code](https://github.com/Microsoft/UWPCommunityToolkit/blob/master/Microsoft.Toolkit.Uwp/Media/BackdropGammaTransferBrush.cs)

## Related Topics

- [Win2D GammaTransferEffect reference](http://microsoft.github.io/Win2D/html/T_Microsoft_Graphics_Canvas_Effects_GammaTransferEffect.htm)
