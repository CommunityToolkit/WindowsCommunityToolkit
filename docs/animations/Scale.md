---
title: Scale animation behavior
author: nmetulev
description: The Scale animation behavior allows you to change a control's scale by increasing or decreasing the control through animation. 
keywords: windows 10, uwp, windows community toolkit, uwp community toolkit, uwp toolkit, scale animation, scale
dev_langs:
  - csharp
  - vb
---

# Scale

The [Scale animation](https://docs.microsoft.com/dotnet/api/microsoft.toolkit.uwp.ui.animations.animationextensions.scale) allows you to change a control's scale by increasing or decreasing the control through animation. Scale animation is applied to all the XAML elements in its parent control/panel. Scale animation doesn't affect the functionality of the control.

## Syntax

```xaml
<Page ...
    xmlns:interactivity="using:Microsoft.Xaml.Interactivity"
    xmlns:behaviors="using:Microsoft.Toolkit.Uwp.UI.Animations.Behaviors"/>

<interactivity:Interaction.Behaviors>
    <behaviors:Scale x:Name="Scale" ScaleX="2.0"
                     ScaleY="2.0" CenterX="0.0"
                     CenterY="0.0" Duration="1000" 
                     Delay="500" EasingType="Linear"
                     AutomaticallyStart="True"/>
</interactivity:Interaction.Behaviors>
```

```csharp
MyUIElement.Scale(scaleX: 2, scaleY: 2, centerX: 0, centerY: 0, duration: 2500, delay: 250, easingType: EasingType.Default).Start();
```
```vb
MyUIElement.Scale(scaleX:=2, scaleY:=2, centerX:=0, centerY:=0, duration:=2500, delay:=250, easingType:=EasingType.[Default]).Start()
```

## Sample Output

![Scale Behavior animation](../resources/images/Animations/Scale/Sample-Output.gif)

## Properties

| Property | Type | Description |
| -- | -- | -- |
| ScaleX | float | The scale on the x axis |
| ScaleY | float | The scale on the y axis |
| CenterX | float | The center x in pixels |
| CenterY | float | The center y in pixels |
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
| Scale(AnimationSet, Single, Single, Single, Single, Double, Double, EasingType) | AnimationSet | Animates the scale of the the specified UIElement |
| Scale(UIElement, Single, Single, Single, Single, Double, Double, EasingType) | AnimationSet | Animates the scale of the the specified UIElement |

## Examples

- Use this to create popup effect

    **Sample Code**

    ```csharp
    UIElement lastTapped = null;
    private void MyUIElement_Tapped(object sender, TappedRoutedEventArgs e)
    {
        if (lastTapped != null)
        {
            lastTapped.Scale(centerX: 50, centerY: 50).Start();
            Canvas.SetZIndex(lastTapped, 0);
        }
        lastTapped = sender as UIElement;
        Canvas.SetZIndex(lastTapped, 1);
        lastTapped.Scale(scaleX: 2, scaleY: 2, centerX: 50, centerY: 50).Start();
    }
    ```
    ```vb
    Private lastTapped As UIElement = Nothing

    Private Sub MyUIElement_Tapped(ByVal sender As Object, ByVal e As TappedRoutedEventArgs)
        If lastTapped IsNot Nothing Then
            lastTapped.Scale(centerX:=50, centerY:=50).Start()
            Canvas.SetZIndex(lastTapped, 0)
        End If

        lastTapped = TryCast(sender, UIElement)
        Canvas.SetZIndex(lastTapped, 1)
        lastTapped.Scale(scaleX:=2, scaleY:=2, centerX:=50, centerY:=50).Start()
    End Sub
    ```
    **Sample Output**

    ![Use Case 1 Output](../resources/images/Animations/Scale/Sample-Output.gif)

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

[Scale Behavior Sample Page Source](https://github.com/Microsoft/WindowsCommunityToolkit//tree/master/Microsoft.Toolkit.Uwp.SampleApp/SamplePages/Scale). You can see this in action in [Windows Community Toolkit Sample App](https://www.microsoft.com/store/apps/9NBLGGH4TLCQ).

## Requirements

| Device family | Universal, 10.0.15063.0 or higher   |
| ---------------------------------------------------------------- | ----------------------------------- |
| Namespace                                                        | Microsoft.Toolkit.Uwp.UI.Animations |
| NuGet package | [Microsoft.Toolkit.Uwp.UI.Animations](https://www.nuget.org/packages/Microsoft.Toolkit.Uwp.UI.Animations/) |

## API

* [Scale source code](https://github.com/Microsoft/WindowsCommunityToolkit//blob/master/Microsoft.Toolkit.Uwp.UI.Animations/Behaviors/Scale.cs)

## Related Topics

- [AnimationSet Class](https://docs.microsoft.com/windows/uwpcommunitytoolkit/animations/animationset)
- [Storyboard Class](https://docs.microsoft.com/uwp/api/Windows.UI.Xaml.Media.Animation.Storyboard)
