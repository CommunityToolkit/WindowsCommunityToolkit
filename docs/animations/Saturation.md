---
title: Saturation animation behavior
author: nmetulev
description: The Saturation animation behavior selectively saturates a XAML element.
keywords: windows 10, uwp, windows community toolkit, uwp community toolkit, uwp toolkit, saturation animation, saturation
dev_langs:
  - csharp
  - vb
---

# Saturation

The [Saturation animation](https://docs.microsoft.com/dotnet/api/microsoft.toolkit.uwp.ui.animations.animationextensions.saturation) selectively saturates a XAML element. Saturation animation is applied to all the XAML elements in its parent control/panel. Saturation animation doesn't affect the functionality of the control.

## Syntax

```xaml
<Page ...
    xmlns:interactivity="using:Microsoft.Xaml.Interactivity"  
    xmlns:behaviors="using:Microsoft.Toolkit.Uwp.UI.Animations.Behaviors"/>

<interactivity:Interaction.Behaviors>
    <behaviors:Saturation x:Name="SaturationBehavior" 
           Value="0" 
           Duration="2500" 
           Delay="250" 
           EasingType="Linear"
           AutomaticallyStart="True"/>
</interactivity:Interaction.Behaviors>
```

```csharp
MyUIElement.Saturation(value: 0.5, duration: 500, delay: 250).Start();
await MyUIElement.Saturation(value: 0.5, duration: 500, delay: 250).StartAsync();  //Saturation animation can be awaited
```
```vb
ToolkitLogo.Saturation(value:=0, duration:=500, delay:=250)
```

## Sample Output

![Saturation Behavior animation](../resources/images/Animations/Saturation/Sample-Output.gif)

## Properties

| Property | Type | Description |
| -- | -- | -- |
| Value | float | The value of saturation. 0 is desaturated, 1 is saturated. |
| Duration | double | The duration in milliseconds |
| Delay | double | The delay for the animation to begin |

## Methods

| Methods | Return Type | Description |
| -- | -- | -- |
| Saturation(AnimationSet, Double, Double, Double) | AnimationSet | Saturates the FrameworkElement |
| Saturation(FrameworkElement, Double, Double, Double) | AnimationSet | Saturates the FrameworkElement |

## Examples

- Sometimes you want an element to desaturate, a common example of this could be when you mouse over a UI Element, now you can apply a SaturationBehavior to the original element at run time.

    **Sample Code**

    ```csharp
    private void MyUIElement_PointerEntered(object sender, PointerRoutedEventArgs e)
    {
        MyUIElement.Saturation(value: 1).Start();
    }

    private void MyUIElement_PointerExited(object sender, PointerRoutedEventArgs e)
    {
        MyUIElement.Saturation(value: 0).Start();
    }
    ```
    ```vb
    Private Sub MyUIElement_PointerEntered(sender As Object, e As PointerRoutedEventArgs)
        MyUIElement.Saturation(value:=1).Start()
    End Sub

    Private Sub MyUIElement_PointerExited(sender As Object, e As PointerRoutedEventArgs)
        MyUIElement.Saturation(value:=0).Start()
    End Sub
    ```

    **Sample Output**

    ![Use Case 1 Output](../resources/images/Animations/Saturation/Use-Case-1.gif)

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

[Saturation Behavior Sample Page Source](https://github.com/Microsoft/WindowsCommunityToolkit//tree/master/Microsoft.Toolkit.Uwp.SampleApp/SamplePages/Saturation). You can see this in action in [Windows Community Toolkit Sample App](https://www.microsoft.com/store/apps/9NBLGGH4TLCQ).

## Requirements

| Device family | Universal, 10.0.15063.0 or higher   |
| ---------------------------------------------------------------- | ----------------------------------- |
| Namespace                                                        | Microsoft.Toolkit.Uwp.UI.Animations |
| NuGet package | [Microsoft.Toolkit.Uwp.UI.Animations](https://www.nuget.org/packages/Microsoft.Toolkit.Uwp.UI.Animations/) |

## API

* [Saturation source code](https://github.com/Microsoft/WindowsCommunityToolkit//blob/master/Microsoft.Toolkit.Uwp.UI.Animations/Behaviors/Saturation.cs)

## Related Topics

- [AnimationSet Class](https://docs.microsoft.com/windows/uwpcommunitytoolkit/animations/animationset)
- [SaturationEffect Class](http://microsoft.github.io/Win2D/html/T_Microsoft_Graphics_Canvas_Effects_SaturationEffect.htm)
