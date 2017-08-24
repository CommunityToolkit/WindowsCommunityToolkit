---
title: Blur animation behavior
author: nmetulev
ms.date: 08/20/2017
description: The UWP Community Toolkit Blur animation behavior selectively blurs a XAML element by increasing or decreasing pixel size
keywords: windows 10, uwp, uwp community toolkit, uwp toolkit, blur, blur animation
---

# Blur

The **Blur animation behavior** selectively blurs a XAML element by increasing or decreasing pixel size.
Sometimes you want an element to appear slightly out of focus, but to be familiar to the user from other locations within an app.  Rather than having to rasterize the XAML into an image and apply a blur, you can apply a BlurBehavior to the original element at run time. 

**NOTE**:  This animation REQUIRES the [Windows 10 Anniversary SDK 14393](https://blogs.windows.com/windowsexperience/2016/07/18/build14393/) 10.0.14393.0 in order for it to work.

## Syntax

You can either use the blur behavior from your XAML code:

```xml

    <interactivity:Interaction.Behaviors>
    <behaviors:Blur x:Name="BlurBehavior" 
           Value="10" 
           Duration="500" 
           Delay="250" 
           AutomaticallyStart="True"/>
    </interactivity:Interaction.Behaviors>

```

or directly from code:

```csharp

ToolkitLogo.Blur(value: 10, duration: 500, delay: 250);       

```

## Properties

| Property Name | Type | Description |
| --- | --- | --- |
| Value | double | The amount of Gaussian blur to apply to the pixel |
| Duration | double | The number of milliseconds the animation should run for |
| Delay | double | The number of milliseconds before the animation is started |

## Chaining animations

Behavior animations can also be chained and awaited.

```csharp

    Element.Rotate(value: 30f, duration: 0.3).StartAsync();

    await Element.Rotate(value: 30f, duration: 0.3).StartAsync();

    var anim = element.Rotate(30f).Fade(0.5).Blur(5);
    anim.SetDurationForAll(2);
    anim.Completed += animation_completed;
    anim.StartAsync();

    anim.Stop();

```

[Blur Behavior Sample Page Source](https://github.com/Microsoft/UWPCommunityToolkit/tree/master/Microsoft.Toolkit.Uwp.SampleApp/SamplePages/Blur)

## Example Image

![Blur Behavior animation](../resources/images/Animations-Blur.gif "Blur Behavior")

## Requirements (Windows 10 Device Family)

| [Device family](http://go.microsoft.com/fwlink/p/?LinkID=526370) | Universal, [Windows 10 Anniversary SDK 14393](https://blogs.windows.com/windowsexperience/2016/07/18/build14393/) 10.0.14393.0 |
| --- | --- |
| Namespace | Microsoft.Toolkit.Uwp.UI.Animations |

## API

* [Blur source code](https://github.com/Microsoft/UWPCommunityToolkit/blob/master/Microsoft.Toolkit.Uwp.UI.Animations/Behaviors/Blur.cs)

