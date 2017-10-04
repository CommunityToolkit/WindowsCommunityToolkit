---
title: Offset animation behavior
author: nmetulev
ms.date: 08/20/2017
description: The Offset animation behavior gets the number of pixels, from the origin of the associated control, then offsets the control.
keywords: windows 10, uwp, uwp community toolkit, uwp toolkit, offset animation
---

# Offset

The **Offset animation behavior** gets the number of pixels, from the origin of the associated control, then offsets the control. 

## Syntax

```xml

<behaviors:Offset x:Name="OffsetBehavior" 
	OffsetX="25.0" 
	OffsetY="25.0"
	Duration="500" 
	Delay="250" 
	AutomaticallyStart="True"/>
</behaviors:Offset>

```

or directly from code:

```csharp

MyRectangle.Offset(
                offsetX: (float)OffsetX,
                offsetY: (float)OffsetY
                duration: Duration,
                delay: Delay);

```

## Properties

| Property Name | Type | Description |
| --- | --- | --- |
| OffsetX | float | The number of pixels to move along the x axis |
| OffsetY | float | The number of pixels to move along the y axis |
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

[Offset Behavior Sample Page Source](https://github.com/Microsoft/UWPCommunityToolkit/tree/master/Microsoft.Toolkit.Uwp.SampleApp/SamplePages/Offset)

## EasingType

You can change the way how the animation interpolates between keyframes by defining the EasingType using an optional parameter.

| EasingType | Explanation|
| --- | --- |
| Default | Creates an animation that accelerates with the default EasingType which is specified in AnimationExtensions.DefaultEasingType which is by default Cubic. |
| Linear | Creates an animation that accelerates or decelerates linear. |
| Cubic | Creates an animation that accelerates or decelerates using the formula f(t) = t3. |
| Back | Retracts the motion of an animation slightly before it begins to animate in the path indicated. |
| Bounce | Creates a bouncing effect. |
| Elastic | Creates an animation that resembles a spring oscillating back and forth until it comes to rest.|
| Circle | Creates an animation that accelerates or decelerates using a circular function. |
| Quadratic | Creates an animation that accelerates or decelerates using the formula f(t) = t2. |
| Quartic | Creates an animation that accelerates or decelerates using the formula f(t) = t4. |
| Quintic | Create an animation that accelerates or decelerates using the formula f(t) = t5. |
| Sine | Creates an animation that accelerates or decelerates using a sine formula. |

**Example Usage:**
```csharp
MyRectangle.Offset(offsetX: 10, offsetY: 10, duration: 10, delay: 0, easingType: EasingType.Bounce);       
```

*Please note that EasingType is used only when AnimationSet.UseComposition == false*

## Example Image

![Offset Behavior animation](../resources/images/Animations-Offset.gif "Offset Behavior")

## Requirements (Windows 10 Device Family)

| [Device family](http://go.microsoft.com/fwlink/p/?LinkID=526370) | Universal, 10.0.14393.0 or higher |
| --- | --- |
| Namespace | Microsoft.Toolkit.Uwp.UI.Animations |

## API

* [Offset source code](https://github.com/Microsoft/UWPCommunityToolkit/blob/master/Microsoft.Toolkit.Uwp.UI.Animations/Behaviors/Offset.cs)


