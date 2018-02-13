---
title: BackdropBlurBrush
author: michael-hawker
ms.date: 02/08/2018
description: The BackdropBlurBrush is a Brush that blurs whatever is behind it in the application.
keywords: windows 10, uwp, uwp community toolkit, uwp toolkit, Colors Helper
---

# BackdropBlurBrush

The **BackdropBlurBrush** is a [Brush](https://docs.microsoft.com/en-us/uwp/api/windows.ui.xaml.media.brush) that blurs whatever is behind it in the application.

It is based on the example provided on [MSDN](https://docs.microsoft.com/en-us/uwp/api/windows.ui.xaml.media.xamlcompositionbrushbase) for the XamlCompositionBrushBase.

## Example

```xaml
    <Border BorderBrush="Black" BorderThickness="1" VerticalAlignment="Center" HorizontalAlignment="Center" Width="400" Height="400">
      <Border.Background>
        <brushes:BackdropBlurBrush Amount="3.0" />
      </Border.Background>
    </Border>
```

## Properties

| Property | Type | Description |
| -- | -- | -- |
| Amount | double | The `Amount` property specifies a double value for the amount of Gaussian blur to apply. |

## Requirements (Windows 10 Device Family)

| [Device family](http://go.microsoft.com/fwlink/p/?LinkID=526370) | Universal, 10.0.15063.0 or higher |
| --- | --- |
| Namespace | Microsoft.Toolkit.Uwp.UI.Brushes |
| NuGet package | [Microsoft.Toolkit.Uwp.UI](https://www.nuget.org/packages/Microsoft.Toolkit.Uwp.UI/) |

## API Source Code

- [BackdropBlurBrush source code](https://github.com/Microsoft/UWPCommunityToolkit/blob/master/Microsoft.Toolkit.Uwp/Brushes/BackdropBlurBrush.cs)

## Related Topics

- [Win2D GaussianBlurEffect reference](http://microsoft.github.io/Win2D/html/T_Microsoft_Graphics_Canvas_Effects_GaussianBlurEffect.htm)
