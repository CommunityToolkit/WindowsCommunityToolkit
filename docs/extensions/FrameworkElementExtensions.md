---
title: FrameworkElement Extensions
author: ST-Apps
description: FrameworkElementExtensions provides a simple way to bind to actual size for any FrameworkElement
keywords: windows 10, uwp, windows community toolkit, uwp community toolkit, uwp toolkit, FrameworkElement, extensions
---

# FrameworkElement Extensions

FrameworkElementEx provides a simple way to bind to actual size for any FrameworkElement.

## EnableActualSizeBinding

The EnableActualSizeBinding property allows you to enable/disable the binding for the ActualHeight and ActualWidth extensions.

## ActualHeight

The ActualHeight property allows to bind to TargetObject's ActualHeight.

### Example

```xaml
<Rectangle x:Name="TargetObject"
            extensions:FrameworkElementExtensions.EnableActualSizeBinding="true"/>
...
<TextBlock Text="{Binding ElementName=TargetObject, Path=(extensions:FrameworkElementExtensions.ActualHeight)}" />
```

## ActualWidth

The ActualWidth property allows to bind to TargetObject's ActualWidth.

## Requirements (Windows 10 Device Family)

| [Device family](http://go.microsoft.com/fwlink/p/?LinkID=526370) | Universal, 10.0.14393.0 or higher |
| --- | --- |
| Namespace | Microsoft.Toolkit.Uwp.UI.Extensions |

## API

* [FrameworkElementEx source code](https://github.com/Microsoft/UWPCommunityToolkit/blob/master/Microsoft.Toolkit.Uwp.UI/Extensions/FrameworkElement)