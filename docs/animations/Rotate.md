---
title: Rotate animation behavior
author: nmetulev
description: The Rotate animation behavior allows users to modify and animate the control's rotation. 
keywords: windows 10, uwp, windows community toolkit, uwp community toolkit, uwp toolkit, rotate, rotate animation
dev_langs:
  - csharp
  - vb
---

# Rotate

The [Rotate animation](https://docs.microsoft.com/dotnet/api/microsoft.toolkit.uwp.ui.animations.animationextensions.rotate) allows users to modify and animate the control's rotation. Rotate animation is applied to all the XAML elements in its parent control/panel. Rotate animation doesn't affect the functionality of the control.

## Syntax

```xaml
<Page ...
    xmlns:interactivity="using:Microsoft.Xaml.Interactivity"  
    xmlns:behaviors="using:Microsoft.Toolkit.Uwp.UI.Animations.Behaviors"/>

<interactivity:Interaction.Behaviors>
    <behaviors:Rotate x:Name="RotateBehavior" 
				Value="180"
				CenterX="0.0" 
				CenterY="0.0" 
				Duration="500" 
				Delay="250" 
                EasingType="Linear"
				AutomaticallyStart="True"/>
  </behaviors:Rotate>
</interactivity:Interaction.Behaviors>
```

```csharp
MyUIElement.Rotate(value: 50.0f, centerX: 0.0f, centerY: 0.0f, duration: 2500, delay: 250, easingType: EasingType.Default).Start();
await MyUIElement.Rotate(value: 50.0f, centerX: 0.0f, centerY: 0.0f, duration: 2500, delay: 250, easingType: EasingType.Default).StartAsync();  //Rotate animation can be awaited
```
```vb
MyUIElement.Rotate(value:=50.0F, centerX:=0F, centerY:=0F, duration:=2500, delay:=250, easingType:=EasingType.[Default]).Start()
Await MyUIElement.Rotate(value:=50.0F, centerX:=0F, centerY:=0F, duration:=2500, delay:=250, easingType:=EasingType.[Default]).StartAsync()  ' Rotate animation can be awaited
```

## Sample Output

![Rotate Behavior animation](../resources/images/Animations/Rotate/Sample-Output.gif)

## Properties

| Property | Type | Description |
| -- | -- | -- |
| Value | float | The value in degrees to rotate |
| CenterX | float | The center x in pixels |
| CenterY | float | The center y in pixels |
| Duration | double | The duration in milliseconds |
| Delay | double | The delay for the animation to begin |
| EasingType | EasingType | Used to describe how the animation interpolates between keyframes |

### EasingType

You can change the way how the animation interpolates between keyframes by defining the EasingType using an optional parameter.

| EasingType | Explanation                                                                                                | Graphical Explanation                      |
| ---------- | ---------------------------------------------------------------------------------------------------------- | ------------------------------------------ |
| Default    | Creates an animation that accelerates with the default EasingType which is specified in AnimationExtensions.DefaultEasingType which is by default Cubic |                                                                                                                           |
| Linear     | Creates an animation that accelerates or decelerates linear                                                                                             |                                                                                                                           |
| Back       | Retracts the motion of an animation slightly before it begins to animate in the path indicated                                                          | ![BackEase](https://docs.microsoft.com/dotnet/framework/wpf/graphics-multimedia/media/backease-graph.png)           |
| Bounce     | Creates a bouncing effect                                                                                                                               | ![BounceEase](https://docs.microsoft.com/dotnet/framework/wpf/graphics-multimedia/media/bounceease-graph.png)       |
| Circle     | Creates an animation that accelerates or decelerates using a circular function                                                                          | ![CircleEase](https://docs.microsoft.com/dotnet/framework/wpf/graphics-multimedia/media/circleease-graph.png)       |
| Cubic      | Creates an animation that accelerates or decelerates using the formula f(t) = t3                                                                        | ![CubicEase](https://docs.microsoft.com/dotnet/framework/wpf/graphics-multimedia/media/cubicease-graph.png)         |
| Elastic    | Creates an animation that resembles a spring oscillating back and forth until it comes to rest                                                          | ![ElasticEase](https://docs.microsoft.com/dotnet/framework/wpf/graphics-multimedia/media/elasticease-graph.png)     |
| Quadratic  | Creates an animation that accelerates or decelerates using the formula f(t) = t2                                                                        | ![QuadraticEase](https://docs.microsoft.com/dotnet/framework/wpf/graphics-multimedia/media/quadraticease-graph.png) |
| Quartic    | Creates an animation that accelerates or decelerates using the formula f(t) = t4                                                                        | ![QuarticEase](https://docs.microsoft.com/dotnet/framework/wpf/graphics-multimedia/media/quarticease-graph.png)     |
| Quintic    | Create an animation that accelerates or decelerates using the formula f(t) = t5                                                                         | ![QuinticEase](https://docs.microsoft.com/dotnet/framework/wpf/graphics-multimedia/media/quinticease-graph.png)     |
| Sine       | Creates an animation that accelerates or decelerates using a sine formula                                                                               | ![SineEase](https://docs.microsoft.com/dotnet/framework/wpf/graphics-multimedia/media/sineease-graph.png)           |

## Methods

| Methods | Return Type | Description |
| -- | -- | -- |
| Rotate(AnimationSet, Single, Single, Single, Double, Double, EasingType)  | AnimationSet | Animates the rotation in degrees of the UIElement |
| Rotate(UIElement, Single, Single, Single, Double, Double, EasingType) | AnimationSet | Animates the rotation in degrees of the UIElement |

## Examples

- Use this to create chaining animations with other animations. Visit the [AnimationSet](AnimationSet.md) documentation for more information.

    **Sample Code**

    ```csharp
    var anim = MyUIElement.Rotate(30).Fade(0.5f).Blur(5);
    anim.SetDurationForAll(2500);
    anim.SetDelay(250);
    anim.Completed += animation_completed;
    anim.Start();
    ```
    ```vb
    Dim anim = MyUIElement.Rotate(30).Fade(0.5F).Blur(5)
    anim.SetDurationForAll(2500)
    anim.SetDelay(250)
    AddHandler anim.Completed, AddressOf animation_completed
    anim.Start()
    ```

    **Sample Output**

    ![Use Case 1 Output](../resources/images/Animations/Chaining-Animations-Blur-Fade-Rotate.gif)

## Sample Project

[Rotate Behavior Sample Page Source](https://github.com/Microsoft/WindowsCommunityToolkit//tree/master/Microsoft.Toolkit.Uwp.SampleApp/SamplePages/Rotate). You can see this in action in [Windows Community Toolkit Sample App](https://www.microsoft.com/store/apps/9NBLGGH4TLCQ).

## Requirements

| Device family | Universal, 10.0.15063.0 or higher   |
| ---------------------------------------------------------------- | ----------------------------------- |
| Namespace                                                        | Microsoft.Toolkit.Uwp.UI.Animations |
| NuGet package | [Microsoft.Toolkit.Uwp.UI.Animations](https://www.nuget.org/packages/Microsoft.Toolkit.Uwp.UI.Animations/) |

## API

* [Rotate source code](https://github.com/Microsoft/WindowsCommunityToolkit//blob/master/Microsoft.Toolkit.Uwp.UI.Animations/Behaviors/Rotate.cs)

## Related Topics

- [AnimationSet Class](https://docs.microsoft.com/windows/uwpcommunitytoolkit/animations/animationset)
