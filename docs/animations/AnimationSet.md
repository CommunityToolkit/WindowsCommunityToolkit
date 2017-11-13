---
title: AnimationSet class
author: Vijay-Nirmal
ms.date: 09/01/2017
description: The AnimationSet class defines an object for storing and managing Storyboard and CompositionAnimations for an element
keywords: windows 10, uwp, uwp community toolkit, uwp toolkit, animationset, animationset class
---

# AnimationSet

The AnimationSet class defines an object for storing and managing Storyboard and CompositionAnimations for an element. AnimationSet includes [Blur](\Blur.md), [Fade](\Fade.md), [Light](\Light.md), [Offset](\Offset.md), [Rotate](\Rotate.md), [Saturation](\Saturation.md) and [Scale](\Scale.md) animations. AnimationSet animations is applied to all the XAML elements in its parent control/panel. AnimationSet animations doesn't affect the functionality of the control.

## Syntax

**XAML**

```xaml
<Page ...
     xmlns:interactivity="using:Microsoft.Xaml.Interactivity"  
     xmlns:behaviors="using:Microsoft.Toolkit.Uwp.UI.Animations.Behaviors"/>
 
<interactivity:Interaction.Behaviors>
    <interactivity:BehaviorCollection>
        <behaviors:Blur Value="10" Duration="2500" AutomaticallyStart="True"/>
        <behaviors:Scale ScaleX="2" ScaleY="2" Duration="2500" AutomaticallyStart="True"/>
        <!-- Others -->
    </interactivity:BehaviorCollection>
</interactivity:Interaction.Behaviors> 
 ```

**C#**

```csharp
var anim = MyUIElement.Light(5).Offset(offsetX: 100, offsetY: 100).Saturation(0.5).Scale(scaleX: 2, scaleY: 2);
anim.SetDurationForAll(2500);
anim.SetDelay(250);
anim.Start();
```

## Sample Output

![AnimationSet animations](../resources/images/Animations/Chaining-Animations-Light-Offset-Saturation-Scale.gif)

## Properties



### EasingType

You can change the way how the animation interpolates between keyframes by defining the EasingType.

| EasingType | Explanation                                                                                                | Graphical Explanation                      |
| ---------- | ---------------------------------------------------------------------------------------------------------- | ------------------------------------------ |
| Default    | Creates an animation that accelerates with the default EasingType which is specified in AnimationExtensions.DefaultEasingType which is by default Cubic |                                                                                                                           |
| Linear     | Creates an animation that accelerates or decelerates linear                                                                                             |                                                                                                                           |
| Back       | Retracts the motion of an animation slightly before it begins to animate in the path indicated                                                          | ![BackEase](https://docs.microsoft.com/en-us/dotnet/framework/wpf/graphics-multimedia/media/backease-graph.png)           |
| Bounce     | Creates a bouncing effect                                                                                                                               | ![BounceEase](https://docs.microsoft.com/en-us/dotnet/framework/wpf/graphics-multimedia/media/bounceease-graph.png)       |
| Circle     | Creates an animation that accelerates or decelerates using a circular function                                                                          | ![CircleEase](https://docs.microsoft.com/en-us/dotnet/framework/wpf/graphics-multimedia/media/circleease-graph.png)       |
| Cubic      | Creates an animation that accelerates or decelerates using the formula f(t) = t3                                                                        | ![CubicEase](https://docs.microsoft.com/en-us/dotnet/framework/wpf/graphics-multimedia/media/cubicease-graph.png)         |
| Elastic    | Creates an animation that resembles a spring oscillating back and forth until it comes to rest                                                          | ![ElasticEase](https://docs.microsoft.com/en-us/dotnet/framework/wpf/graphics-multimedia/media/elasticease-graph.png)     |
| Quadratic  | Creates an animation that accelerates or decelerates using the formula f(t) = t2                                                                        | ![QuadraticEase](https://docs.microsoft.com/en-us/dotnet/framework/wpf/graphics-multimedia/media/quadraticease-graph.png) |
| Quartic    | Creates an animation that accelerates or decelerates using the formula f(t) = t4                                                                        | ![QuarticEase](https://docs.microsoft.com/en-us/dotnet/framework/wpf/graphics-multimedia/media/quarticease-graph.png)     |
| Quintic    | Create an animation that accelerates or decelerates using the formula f(t) = t5                                                                         | ![QuinticEase](https://docs.microsoft.com/en-us/dotnet/framework/wpf/graphics-multimedia/media/quinticease-graph.png)     |
| Sine       | Creates an animation that accelerates or decelerates using a sine formula                                                                               | ![SineEase](https://docs.microsoft.com/en-us/dotnet/framework/wpf/graphics-multimedia/media/sineease-graph.png)           |

***Note:** EasingType is used only when AnimationSet.UseComposition == false*  
***Note:** Blur, Light and Saturation animation don't support easing*

## Examples

- AnimationSet has endless possibility. Here is an example of creating popup effect

    **Sample Code**
    ```csharp
    FrameworkElement preElement = null;
    private void MyUIElement_PointerEntered(object sender, PointerRoutedEventArgs e)
    {
        preElement = sender as FrameworkElement;
        preElement.Blur(value: 0).Fade(value: 1).Scale(centerX: 100, centerY: 100, easingType: EasingType.Sine);
                .SetDurationForAll(500);
                .Start();
    }

    private void MyUIElement_PointerExited(object sender, PointerRoutedEventArgs e)
    {
        if (preElement != null)
        {
            preElement.Blur(value: 0).Fade(value: 0.1f).Scale(scaleX: 0.5f, scaleY: 0.5f, centerX: 100, centerY: 100, easingType: EasingType.Sine)
                    .SetDurationForAll(500);
                    .Start();
        }
    }
    ```
    **Sample Output**

    ![Use Case 1 Output](../resources/images/Animations/AnimationSet/Use-Case-1.gif)
- Use `Then()` to create a successive animation

    **Sample Code**
    ```csharp
    MyUIElement.Blur(value: 10).Fade(value: 0.5f)
            .Then()
            .Fade(value: 1).Scale(scaleX: 2, scaleY: 2, centerX: 100, centerY: 100, easingType: EasingType.Sine)
            .SetDurationForAll(2500)
            .Start();
    ```
    **Sample Output**

    ![Use Case 2 Output](../resources/images/Animations/AnimationSet/Use-Case-2.gif)

## Requirements

| [Device family](http://go.microsoft.com/fwlink/p/?LinkID=526370) | Universal, 10.0.14393.0 or higher   |
| ---------------------------------------------------------------- | ----------------------------------- |
| Namespace                                                        | Microsoft.Toolkit.Uwp.UI.Animations |
| NuGet package | [Microsoft.Toolkit.Uwp.UI.Animations](https://www.nuget.org/packages/Microsoft.Toolkit.Uwp.UI.Animations/) |

## API

* [AnimationSet source code](https://github.com/Microsoft/UWPCommunityToolkit/tree/master/Microsoft.Toolkit.Uwp.UI.Animations)

