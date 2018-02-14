---
title: ImageBlendBrush
author: michael-hawker
ms.date: 02/13/2018
description: The ImageBlendBrush is a Brush that inverts whatever is behind it in the application.
keywords: windows 10, uwp, uwp community toolkit, uwp toolkit, Colors Helper
---

# ImageBlendBrush

The **ImageBlendBrush** is a [Brush](https://docs.microsoft.com/en-us/uwp/api/windows.ui.xaml.media.brush) that blends the provided image with whatever is behind it in the application with the provided blend mode.

The image loading code is based on an example provided on [Windows Blogs](https://blogs.windows.com/buildingapps/2017/07/18/working-brushes-content-xaml-visual-layer-interop-part-one/#c57zf3bW4ylLlSvJ.97).

## Example

```xaml
  <Border Grid.Column="0">
    <Border.Background>
      <brushes:ImageBlendBrush 
        Source="/SamplePages/DropShadowPanel/Unicorn.png"
        Stretch="None"
        Mode="ColorBurn"
      />
    </Border.Background>
  </Border>
```

## Properties

| Property | Type | Description |
| -- | -- | -- |
| Source | Windows.UI.Xaml.Media.ImageSource | The `ImageSource` property specifies which image to use for the effect.  It is assumed it will resolve to a [BitmapImage](https://docs.microsoft.com/en-us/uwp/api/windows.ui.xaml.media.imaging.bitmapimage). |
| Stretch | Windows.UI.Xaml.Media.Stretch | The `Stretch` property specifies how the image should stretch to its container. |
| Mode | ImageBlendMode | The `ImageBlendMode` property specifies how the image should be blended with the backdrop.  See the [BlendEffectMode](http://microsoft.github.io/Win2D/html/T_Microsoft_Graphics_Canvas_Effects_BlendEffectMode.htm) reference. |


http://microsoft.github.io/Win2D/html/T_Microsoft_Graphics_Canvas_Effects_BlendEffectMode.htm

## Requirements (Windows 10 Device Family)

| [Device family](http://go.microsoft.com/fwlink/p/?LinkID=526370) | Universal, 10.0.15063.0 or higher |
| --- | --- |
| Namespace | Microsoft.Toolkit.Uwp.UI.Brushes |
| NuGet package | [Microsoft.Toolkit.Uwp.UI](https://www.nuget.org/packages/Microsoft.Toolkit.Uwp.UI/) |

## API Source Code

- [BackdropInvertBrush source code](https://github.com/Microsoft/UWPCommunityToolkit/blob/master/Microsoft.Toolkit.Uwp/Brushes/ImageBlendBrush.cs)

## Related Topics

- [BitmapImage](https://docs.microsoft.com/en-us/uwp/api/windows.ui.xaml.media.imaging.bitmapimage)
- [Win2D BlendEffect reference](http://microsoft.github.io/Win2D/html/T_Microsoft_Graphics_Canvas_Effects_BlendEffect.htm)
- [BlendEffectMode reference](http://microsoft.github.io/Win2D/html/T_Microsoft_Graphics_Canvas_Effects_BlendEffectMode.htm)
