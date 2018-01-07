---
title: FrameworkElement Extentions
author: ST-Apps
ms.date: 01/05/2018
description: FrameworkElementEx provides a simple way to bind to actual size for any FrameworkElement
keywords: windows 10, uwp, uwp community toolkit, uwp toolkit, FrameworkElement, extentions
---

# FrameworkElement extentions

FrameworkElementEx provides a simple way to bind to actual size for any FrameworkElement.

## EnableActualSizeBinding

The EnableActualSizeBinding property allows you to enable/disable the binding for the ActualHeight and ActualWidth extensions.

## ActualHeight

The ActualHeight property allows to bind to TargetObject's ActualHeight.

### Example

```xaml
    <Rectangle x:Name="TargetObject"
               extensions:FrameworkElementEx.EnableActualSizeBinding="true"/>
	...
	<TextBlock Text="{Binding ElementName=TargetObject, Path=(extensions:FrameworkElementEx.ActualHeight)}" />
```

## ActualWidth

The ActualWidth property allows to bind to TargetObject's ActualWidth.

## Requirements (Windows 10 Device Family)

| [Device family](http://go.microsoft.com/fwlink/p/?LinkID=526370) | Universal, 10.0.14393.0 or higher |
| --- | --- |
| Namespace | Microsoft.Toolkit.Uwp.UI.Extensions |

## API

* [FrameworkElementEx source code](https://github.com/Microsoft/UWPCommunityToolkit/blob/master/Microsoft.Toolkit.Uwp.UI/Extensions/FrameworkElement)