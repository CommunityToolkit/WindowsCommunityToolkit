---
title: ParallaxService
author: nmetulev
ms.date: 08/20/2017
description: The ParallaxService class allows to create a parallax effect for items contained within an element that scrolls like a ScrollViewer or ListView.
keywords: windows 10, uwp, uwp community toolkit, uwp toolkit, parallaxservice
---

# ParallaxService

The ParallaxService class allows to create a parallax effect for items contained within an element that scrolls like a ScrollViewer or ListView.

## Syntax

**XAML**

```xml
<Page ...
    xmlns:animations="using:Microsoft.Toolkit.Uwp.UI.Animations"/>
<ScrollViewer>
    <Image Source="ms-appx:///Assets/Image.png"
            animations:ParallaxService.VerticalMultiplier="0.5" 
            animations:ParallaxService.HorizontalMultiplier="0.5"/>
    <!-- Other Controls -->
</ScrollViewer>
```

**C#**

```csharp
MyUIElement.SetValue(ParallaxService.VerticalMultiplierProperty, 0.5);
MyUIElement.SetValue(ParallaxService.HorizontalMultiplierProperty, 0.5);
```

## Sample Output

![ParallaxService](../resources/images/Animations/ParallaxService/Sample-Output.gif)

## Sample Project

[ParallaxService Sample Page Source](https://github.com/Microsoft/UWPCommunityToolkit/tree/master/Microsoft.Toolkit.Uwp.SampleApp/SamplePages/ParallaxService). You can see this in action in [UWP Community Toolkit Sample App](https://www.microsoft.com/store/apps/9NBLGGH4TLCQ)

## Requirements

| [Device family](http://go.microsoft.com/fwlink/p/?LinkID=526370) | Universal, 10.0.14393.0 or higher   |
| ---------------------------------------------------------------- | ----------------------------------- |
| Namespace                                                        | Microsoft.Toolkit.Uwp.UI.Animations |
| NuGet package | [Microsoft.Toolkit.Uwp.UI.Animations](https://www.nuget.org/packages/Microsoft.Toolkit.Uwp.UI.Animations/) |

## API

* [ParallaxService source code](https://github.com/Microsoft/UWPCommunityToolkit/blob/master/Microsoft.Toolkit.Uwp.UI.Animations/ParallaxService.cs)
