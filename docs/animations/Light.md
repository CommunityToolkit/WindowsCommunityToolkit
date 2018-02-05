---
title: Light animation behavior
author: nmetulev
ms.date: 08/20/2017
description: The Light animation behavior performs a point light in the middle of a given UIElement. 
keywords: windows 10, uwp, uwp community toolkit, uwp toolkit, light, light animation
---

# Light

The **Light animation behavior** performs a point light (A point source of light that emits light in all directions) in the middle of a given UIElement. 
The light behavior is great drawing the user's eye towards a particular pieces of user interface. You set the distance property of the 
light to determine how bright the light will be. The closer the light source, the more focused it will be, but, will make the overall UI element darker.
The further away from the light source the more the light will spread over the UIElement.

**NOTE**:  Heavy usage of effects may have a negative impact on the performance of your application. 

## Syntax

You can either use the light behavior from your XAML code:

```xml

    <interactivity:Interaction.Behaviors>
    <behaviors:Light x:Name="LightBehavior" 
           Distance="10" 
           Duration="500" 
           Delay="0" 
           AutomaticallyStart="True"/>
    </interactivity:Interaction.Behaviors>

```

or directly from code:

```csharp
var animation = ToolkitLogo.Light(value: 10, duration: 500, delay: 0); 
await animation.StartAsync();

```

Behavior animations can also be chained and awaited.

```csharp

    Element.Rotate(value: 30f, duration: 0.3).StartAsync();

    await Element.Rotate(value: 30f, duration: 0.3).StartAsync();

    var anim = element.Rotate(30f).Fade(0.5).Light(10);
    anim.SetDurationForAll(2);
    anim.Completed += animation_completed;
    anim.StartAsync();

    anim.Stop();

```

[Light Behavior Sample Page Source](https://github.com/Microsoft/UWPCommunityToolkit/tree/master/Microsoft.Toolkit.Uwp.SampleApp/SamplePages/Light)

## Example Image

![Light Behavior animation](../resources/images/Animations-Light.gif "Light Behavior")

## Requirements (Windows 10 Device Family)

| [Device family](http://go.microsoft.com/fwlink/p/?LinkID=526370) | Universal, [Windows 10 Anniversary SDK 14393](https://blogs.windows.com/windowsexperience/2016/07/18/build14393/) 10.0.14393.0 |
| --- | --- |
| Namespace | Microsoft.Toolkit.Uwp.UI.Animations |

## API

* [Light source code](https://github.com/Microsoft/UWPCommunityToolkit/blob/master/Microsoft.Toolkit.Uwp.UI.Animations/Behaviors/Light.cs)

