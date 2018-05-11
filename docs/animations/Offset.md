---
title: Offset animation behavior
author: nmetulev
description: The Offset animation behavior gets the number of pixels, from the origin of the associated control, then offsets the control.
keywords: windows 10, uwp, windows community toolkit, uwp community toolkit, uwp toolkit, offset animation
dev_langs:
  - csharp
  - vb
---

# Offset

The [Offset animation](https://docs.microsoft.com/dotnet/api/microsoft.toolkit.uwp.ui.animations.animationextensions.offset) is used to move the control from one place to another. Offset animation is applied to all the XAML elements in its parent control/panel. Offset animation doesn't affect the functionality of the control.

## Syntax

```xaml
<Page ...
    xmlns:interactivity="using:Microsoft.Xaml.Interactivity"  
    xmlns:behaviors="using:Microsoft.Toolkit.Uwp.UI.Animations.Behaviors"/>

<interactivity:Interaction.Behaviors>
    <behaviors:Offset x:Name="OffsetBehavior" 
            OffsetX="25.0" 
            OffsetY="25.0"
            Duration="2500" 
            Delay="250" 
            EasingType="Linear"
            AutomaticallyStart="True"/>
</interactivity:Interaction.Behaviors>
```

```csharp
MyUIElement.Offset(offsetX: 25, offsetY: 25, duration: 2500, delay: 250, easingType: EasingType.Default).Start();
await MyUIElement.Offset(offsetX: 25, offsetY: 25, duration: 2500, delay: 250, easingType: EasingType.Default).StartAsync();  //Offset animation can be awaited
```
```vb
MyUIElement.Offset(offsetX:=25, offsetY:=25, duration:=2500, delay:=250, easingType:=EasingType.[Default]).Start()
Await MyUIElement.Offset(offsetX:=25, offsetY:=25, duration:=2500, delay:=250, easingType:=EasingType.[Default]).StartAsync()  ' Offset animation can be awaited
```

## Sample Output

![Offset Behavior animation](../resources/images/Animations/Offset/Sample-Output.gif)

## Properties

| Property | Type | Description |
| -- | -- | -- |
| OffsetX | float | The offset on the x axis |
| OffsetY | float | The offset on the y axis |
| Duration | double | The duration in milliseconds |
| Delay | double | The delay for the animation to begin |
| EasingType | EasingType | Used to describe how the animation interpolates between keyframes |

### EasingType

You can change the way how the animation interpolates between keyframes by defining the EasingType.

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
| Offset(AnimationSet, Single, Single, Double, Double, EasingType) | AnimationSet | Animates the offset of the the UIElement |
| Offset(UIElement, Single, Single, Double, Double, EasingType) | AnimationSet | Animates the offset of the the UIElement |

## Examples

- You can just call `Offset()` set the control in the original position

    **Sample Code**

    ```csharp
    await MyUIElement.Offset().Start();
    ```
    ```vb
    Await MyUIElement.Offset().Start()
    ```
- Use await to create a continuous movement

    **Sample Code**

    ```csharp
    public async void OffsetAsync()
    {
        await MyUIElement.Offset(offsetX: 100, duration:1000).StartAsync();
        await MyUIElement.Offset(offsetX: 100, offsetY: 100, duration: 1000).StartAsync();
        await MyUIElement.Offset(offsetX: 0, offsetY:100, duration: 1000).StartAsync();
        await MyUIElement.Offset(duration: 1000).StartAsync();
    }
    ```
    ```vb
    Public Async Function OffsetAsync() As Task
        Await MyUIElement.Offset(offsetX:=100, duration:=1000).StartAsync()
        Await MyUIElement.Offset(offsetX:=100, offsetY:=100, duration:=1000).StartAsync()
        Await MyUIElement.Offset(offsetX:=0, offsetY:=100, duration:=1000).StartAsync()
        Await MyUIElement.Offset(duration:=1000).StartAsync()
    End Function
    ```

    **Sample Output**

    ![Use Case 2 Output](../resources/images/Animations/Offset/Use-Case-1.gif)

- Use this to create chaining animations with other animations. Visit the [AnimationSet](AnimationSet.md) documentation for more information.

    **Sample Code**

    ```csharp
    var anim = MyUIElement.Light(5).Offset(offsetX: 100, offsetY: 100).Saturation(0.5).Scale(scaleX: 2, scaleY: 2);
    anim.SetDurationForAll(2500);
    anim.SetDelay(250);
    anim.Completed += animation_completed;
    anim.Start();
    ```
    ```vb
    Dim anim = MyUIElement.Light(5).Offset(offsetX:=100, offsetY:=100).Saturation(0.5).Scale(scaleX:=2, scaleY:=2)
    anim.SetDurationForAll(2500)
    anim.SetDelay(250)
    AddHandler anim.Completed, AddressOf animation_completed
    anim.Start()
    ```

    **Sample Output**

    ![Use Case 2 Output](../resources/images/Animations/Chaining-Animations-Light-Offset-Saturation-Scale.gif)

## Sample Project

[Offset Behavior Sample Page Source](https://github.com/Microsoft/UWPCommunityToolkit/tree/master/Microsoft.Toolkit.Uwp.SampleApp/SamplePages/Offset). You can see this in action in [Windows Community Toolkit Sample App](https://www.microsoft.com/store/apps/9NBLGGH4TLCQ).

## Requirements

| Device family | Universal, 10.0.14393.0 or higher   |
| ---------------------------------------------------------------- | ----------------------------------- |
| Namespace                                                        | Microsoft.Toolkit.Uwp.UI.Animations |
| NuGet package | [Microsoft.Toolkit.Uwp.UI.Animations](https://www.nuget.org/packages/Microsoft.Toolkit.Uwp.UI.Animations/) |

## API

* [Offset source code](https://github.com/Microsoft/UWPCommunityToolkit/blob/master/Microsoft.Toolkit.Uwp.UI.Animations/Behaviors/Offset.cs)

## Related Topics

- [AnimationSet Class](https://docs.microsoft.com/windows/uwpcommunitytoolkit/animations/animationset)
- [Storyboard Class](https://docs.microsoft.com/uwp/api/Windows.UI.Xaml.Media.Animation.Storyboard)
