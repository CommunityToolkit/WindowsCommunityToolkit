---
title: Fade animation behavior
author: nmetulev
ms.date: 08/20/2017
description: The Fade animation behavior fades objects, in and out, over time and delay. It can be used along side other animations directly through XAML or code
keywords: windows 10, uwp, uwp community toolkit, uwp toolkit, fade, fade animation
---

# Fade

The **Fade animation behavior** fades objects, in and out, over time.

## Syntax

```xml

    <behaviors:Fade x:Name="FadeBehavior>" 
                Value="0.5" 
                Duration="1500" 
                Delay="500" 
                AutomaticallyStart="True">
    </behaviors:Fade>

```

or directly from code:

```csharp

    MyRectangle.Fade((float)Value, Duration, Delay);

```

## Properties

| Property Name | Type | Description |
| --- | --- | --- |
| Value | float | The amount to fade an element. The value should be between 0.0 and l.0 |
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

[Fade Behavior Sample Page Source](https://github.com/Microsoft/UWPCommunityToolkit/tree/master/Microsoft.Toolkit.Uwp.SampleApp/SamplePages/Fade)

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

*Please note that EasingType is used only when AnimationSet.UseComposition == false*

**Example Usage:**
```csharp
MyRectangle.Fade(value: 10, duration: 10, delay: 0, easingType: EasingType.Bounce);       
```

## Example Image

![Fade Behavior animation](../resources/images/Animations-Fade.gif "Fade Behavior")

## Requirements (Windows 10 Device Family)

| [Device family](http://go.microsoft.com/fwlink/p/?LinkID=526370) | Universal, 10.0.14393.0 or higher |
| --- | --- |
| Namespace | Microsoft.Toolkit.Uwp.UI.Animations |

## API

* [Fade source code](https://github.com/Microsoft/UWPCommunityToolkit/blob/master/Microsoft.Toolkit.Uwp.UI.Animations/Behaviors/Fade.cs)

