---
title: Rotate animation behavior
author: nmetulev
ms.date: 08/20/2017
description: The Rotate animation behavior allows users to modify and animate the control's rotation. 
keywords: windows 10, uwp, uwp community toolkit, uwp toolkit, rotate, rotate animation
---

# Rotate

The **Rotate animation behavior** allows users to modify and animate the control's rotation. Parameters include: angle values, time, pause delay, duration, and diameter.

## Syntax

```xml

   <behaviors:Rotate x:Name="RotateBehavior" 
				Value="180"
				CenterX="0.0" 
				CenterY="0.0" 
				Duration="500" 
				Delay="250" 
				AutomaticallyStart="True"/>
  </behaviors:Rotate>

```

or directly from code:

```csharp

    MyRectangle.Rotate(
                value: (float)Value,
                centerX: (float)CenterX,
                centerY: (float)CenterY,
                duration: Duration,
                delay: Delay);

```

## Properties

| Property Name | Type | Description |
| --- | --- | --- |
| Value | float | The rotation of the element in degrees |
| CenterX | float | The pivot point on the x axis |
| CenterY | float | The pivot point on the y axis |
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

[Rotate Behavior Sample Page Source](https://github.com/Microsoft/UWPCommunityToolkit/tree/master/Microsoft.Toolkit.Uwp.SampleApp/SamplePages/Rotate)

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
MyRectangle.Offset(value: 10, duration: 10, delay: 0, easingType: EasingType.Bounce);       
```

*Please note that EasingType is used only when AnimationSet.UseComposition == false*

## Example Image

![Rotate Behavior animation](../resources/images/Animations-Rotate.gif "Rotate Behavior")

## Requirements (Windows 10 Device Family)

| [Device family](http://go.microsoft.com/fwlink/p/?LinkID=526370) | Universal, 10.0.14393.0 or higher |
| --- | --- |
| Namespace | Microsoft.Toolkit.Uwp.UI.Animations |

## API

* [Rotate source code](https://github.com/Microsoft/UWPCommunityToolkit/blob/master/Microsoft.Toolkit.Uwp.UI.Animations/Behaviors/Rotate.cs)

