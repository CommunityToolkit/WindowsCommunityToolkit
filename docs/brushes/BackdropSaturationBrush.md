---
title: BackdropSaturationBrush
author: michael-hawker
description: The BackdropSaturationBrush is a Brush that applies a Saturation effect to whatever is behind it in the application.
keywords: windows 10, uwp, windows community toolkit, uwp community toolkit, uwp toolkit, brush, backdrop, saturation
---

# BackdropSaturationBrush

The [BackdropSaturationBrush](https://docs.microsoft.com/dotnet/api/microsoft.toolkit.uwp.ui.media.backdropsaturationbrush) is a [Brush](https://docs.microsoft.com/uwp/api/windows.ui.xaml.media.brush) that blurs whatever is behind it in the application.

## Syntax

```xaml
<Border BorderBrush="Black" BorderThickness="1" VerticalAlignment="Center" HorizontalAlignment="Center" Width="400" Height="400">
  <Border.Background>
    <media:BackdropSepiaBrush Intensity="0.25" />
  </Border.Background>
</Border>
```

## Example Image

![Backdrop Saturation](../resources/images/Brushes/BackdropSaturation.jpg "Backdrop Saturation")

## Properties

| Property | Type | Description |
| -- | -- | -- |
| Saturation | double | The `Saturation` property specifies a double value for the amount of Saturation to apply from 0.0 - 1.0.  Zero being monochrome, and one being fully saturated.  The default is 0.5. |

## Sample Code

[BackdropSaturationBrush sample page Source](https://github.com/Microsoft/WindowsCommunityToolkit//tree/master/Microsoft.Toolkit.Uwp.SampleApp/SamplePages/BackdropSaturationBrush). You can see this in action in [Windows Community Toolkit Sample App](https://www.microsoft.com/store/apps/9NBLGGH4TLCQ).

## Requirements

| Device family | Universal, 10.0.15063.0 or higher |
| --- | --- |
| Namespace | Microsoft.Toolkit.Uwp.UI.Media |
| NuGet package | [Microsoft.Toolkit.Uwp.UI](https://www.nuget.org/packages/Microsoft.Toolkit.Uwp.UI/) |

## API Source Code

- [BackdropSaturationBrush source code](https://github.com/Microsoft/WindowsCommunityToolkit//blob/master/Microsoft.Toolkit.Uwp/Media/BackdropSaturationBrush.cs)

## Related Topics

- [Win2D SaturationEffect reference](http://microsoft.github.io/Win2D/html/T_Microsoft_Graphics_Canvas_Effects_SaturationEffect.htm)
