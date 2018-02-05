---
title: ParallaxService
author: nmetulev
ms.date: 08/20/2017
description: The ParallaxService class allows to create a parallax effect for items contained within an element that scrolls like a ScrollViewer or ListView.
keywords: windows 10, uwp, uwp community toolkit, uwp toolkit, parallaxservice
---

# ParallaxService

The **ParallaxService** class allows to create a parallax effect for items contained within an element that scrolls like a ScrollViewer or ListView.

## Syntax

```xml

<Image Source="ms-appx:///Assets/Photos/BigFourSummerHeat.png"
       ParallaxService.VerticalMultiplier="2.5/>

```

You can define horizontal or vertical multiplier to determine the speed ratio that you want to apply to your element.

[ParallaxService Sample Page Source](https://github.com/Microsoft/UWPCommunityToolkit/tree/master/Microsoft.Toolkit.Uwp.SampleApp/SamplePages/ParallaxService)

## Example Image

![ParallaxService](../resources/images/ParallaxService.gif "ParallaxService")

## Requirements (Windows 10 Device Family)

| [Device family](http://go.microsoft.com/fwlink/p/?LinkID=526370) | Universal, 10.0.14393.0 or higher |
| --- | --- |
| Namespace | Microsoft.Toolkit.Uwp.UI.Animations |

## API

* [ParallaxService source code](https://github.com/Microsoft/UWPCommunityToolkit/blob/master/Microsoft.Toolkit.Uwp.UI.Animations/ParallaxService.cs)
