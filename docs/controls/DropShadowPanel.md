---
title: DropShadowPanel XAML Control
author: nmetulev
ms.date: 08/20/2017
description: The DropShadowPanel Control allows the creation of a drop shadow effect for any Xaml FrameworkElement in the markup.
keywords: windows 10, uwp, uwp community toolkit, uwp toolkit, DropShadowPanel, DropShadow, xaml Control, xaml
---

# DropShadowPanel XAML Control

The **DropShadowPanel Control** allows the creation of a drop shadow effect for any Xaml FrameworkElement in the markup.
You can control the following property of the drop shadow effect : Offset, Color, Opactity and Blur Radius.

**NOTE:** Windows Anniversary Update (10.0.14393.0) is needed to support correctly this effect.

## Syntax

```xml

<controls:DropShadowPanel BlurRadius="4.0"
                          ShadowOpacity="0.70"
                          OffsetX="5.0"
                          OffsetY="5.0"
                          Color="Black">
	<Image Width="200" Source="Unicorn.png" Stretch="Uniform"/>
</controls:DropShadowPanel>       

```

## Example Image

![DropShadowPanel animation](../resources/images/Controls-DropShadowPanel.png "DropShadowPanel")

## Example Code

[DropShadowPanel Sample Page](https://github.com/Microsoft/UWPCommunityToolkit/tree/master/Microsoft.Toolkit.Uwp.SampleApp/SamplePages/DropShadowPanel)

## Default Template 

[DropShadowPanel XAML File](https://github.com/Microsoft/UWPCommunityToolkit/blob/master/Microsoft.Toolkit.Uwp.UI.Controls/DropShadowPanel/DropShadowPanel.xaml) is the XAML template used in the toolkit for the default styling.

## Requirements (Windows 10 Device Family)

| [Device family](http://go.microsoft.com/fwlink/p/?LinkID=526370) | Universal, 10.0.14393.0 or higher |
| --- | --- |
| Namespace | Microsoft.Toolkit.Uwp.UI.Controls |

## API

* [DropShadowPanel source code](https://github.com/Microsoft/UWPCommunityToolkit/tree/master/Microsoft.Toolkit.Uwp.UI.Controls/DropShadowPanel)
