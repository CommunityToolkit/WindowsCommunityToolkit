---
title: Saturation animation behavior
author: nmetulev
ms.date: 08/20/2017
description: The Saturation animation behavior selectively saturates a XAML element.
keywords: windows 10, uwp, uwp community toolkit, uwp toolkit, saturation animation, saturation
---

# Saturation

The **Saturation animation behavior** selectively saturates a XAML element.

Sometimes you want an element to desaturate, a common example of this could be when you mouse over a UI Element, now you can apply a SaturationBehavior to the original element at run time. 

**NOTE**:  This animation REQUIRES the [Windows 10 Anniversary SDK 14393](https://blogs.windows.com/windowsexperience/2016/07/18/build14393/) 10.0.14393.0 in order for it to work.

## Syntax

You can either use the saturation behavior from your XAML code:

```xml

    <interactivity:Interaction.Behaviors>
    <behaviors:Saturation x:Name="SaturationBehavior" 
           Value="10" 
           Duration="500" 
           Delay="250" 
           AutomaticallyStart="True"/>
    </interactivity:Interaction.Behaviors>

```

or directly from code:

```csharp

ToolkitLogo.Saturation(value: 10, duration: 500, delay: 250);       

```

## Properties

| Property Name | Type | Description |
| --- | --- | --- |
| Value | double | A range between 0 and 1 on how to saturate the UI Element. 1 is maximum saturation, 0 is desaturated.  |
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

[Saturation Behavior Sample Page Source](https://github.com/Microsoft/UWPCommunityToolkit/tree/master/Microsoft.Toolkit.Uwp.SampleApp/SamplePages/Saturation)

## Example Image

![Saturation Behavior animation](../resources/images/Animations-Saturation.gif "Saturation Behavior")

## Requirements (Windows 10 Device Family)

| [Device family](http://go.microsoft.com/fwlink/p/?LinkID=526370) | Universal, [Windows 10 Anniversary SDK 14393](https://blogs.windows.com/windowsexperience/2016/07/18/build14393/) 10.0.14393.0 |
| --- | --- |
| Namespace | Microsoft.Toolkit.Uwp.UI.Animations |

## API

* [Saturation source code](https://github.com/Microsoft/UWPCommunityToolkit/blob/master/Microsoft.Toolkit.Uwp.UI.Animations/Behaviors/Saturation.cs)

