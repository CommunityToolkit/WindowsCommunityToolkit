---
title: Scale animation behavior
author: nmetulev
ms.date: 08/20/2017
description: The Scale animation behavior allows you to change a control's scale by increasing or decreasing the control through animation. 
keywords: windows 10, uwp, uwp community toolkit, uwp toolkit, scale animation, scale
---

# Scale

The **Scale animation behavior** allows you to change a control's scale by increasing or decreasing the control through animation. For example, perhaps you want an entry field to change size when the user taps it.

## Syntax

```xml

<interactivity:Interaction.Behaviors>
    <behaviors:Scale x:Name="Scale" 
                     ScaleX="2.0"
                     ScaleY="2.0"
                     CenterX="0.0"
                     CenterY="0.0" 
                     Duration="1000" 
                     Delay="500" 
                     AutomaticallyStart="True"/>
</interactivity:Interaction.Behaviors>

```

or directly from code:

```csharp

MyRectangle.Scale(
                scaleX: (float)ScaleX,
                scaleY: (float)ScaleY,
                centerX: (float)CenterX,
                centerY: (float)CenterY,
                duration: Duration,
                delay: Delay);                

```

## Properties

| Property Name | Type | Description |
| --- | --- | --- |
| ScaleX | float | The scale of the element along the x axis |
| ScaleY | float | The scale of the element along the y axis |
| CenterX | float | The pivot point on the x axis |
| CenterY | float | The pivot point on the y axis |
| Duration | double | The number of milliseconds the animation should run for |
| Delay | double | The number of milliseconds before the animation is started |

## Chaining animations

Behaviors can also be chained and awaited.

```csharp

    Element.Rotate(value: 30f, duration: 0.3).StartAsync();

    await Element.Rotate(value: 30f, duration: 0.3).StartAsync();

    var anim = element.Rotate(30f).Fade(0.5).Blur(5);
    anim.SetDurationForAll(2);
    anim.Completed += animation_completed;
    anim.StartAsync();

    anim.Stop();

```

[Scale Behavior Sample Page Source](https://github.com/Microsoft/UWPCommunityToolkit/tree/master/Microsoft.Toolkit.Uwp.SampleApp/SamplePages/Scale)

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

![Scale Behavior animation](../resources/images/Animations-Scale.gif "Scale Behavior")

## Requirements (Windows 10 Device Family)

| [Device family](http://go.microsoft.com/fwlink/p/?LinkID=526370) | Universal, 10.0.14393.0 or higher |
| --- | --- |
| Namespace | Microsoft.Toolkit.Uwp.UI.Animations |

## API

* [Scale source code](https://github.com/Microsoft/UWPCommunityToolkit/blob/master/Microsoft.Toolkit.Uwp.UI.Animations/Behaviors/Scale.cs)
