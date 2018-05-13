---
title: Light animation behavior
author: nmetulev
description: The Light animation behavior performs a point light in the middle of a given UIElement. 
keywords: windows 10, uwp, windows community toolkit, uwp community toolkit, uwp toolkit, light, light animation
dev_langs:
  - csharp
  - vb
---

# Light

The [Light animation](https://docs.microsoft.com/dotnet/api/microsoft.toolkit.uwp.ui.animations.animationextensions.light) behavior performs a point light (A point source of light that emits light in all directions) in the middle of a given UIElement. You set the distance property of the light to determine how bright the light will be. The closer the light source, the darker the UI element will be. 

> [!NOTE]
Heavy usage of effects may have a negative impact on the performance of your application. 

## Syntax

```xaml
<Page ...
    xmlns:interactivity="using:Microsoft.Xaml.Interactivity"  
    xmlns:behaviors="using:Microsoft.Toolkit.Uwp.UI.Animations.Behaviors"/>

<interactivity:Interaction.Behaviors>
    <behaviors:Light x:Name="LightBehavior" 
           Distance="10" 
           Duration="500" 
           Delay="0"
           EasingType="Linear"
           AutomaticallyStart="True"
           Color="Red"/>
</interactivity:Interaction.Behaviors>
```

```csharp
MyUIElement.Light(distance: 5, duration: 2500, delay: 250, color: Colors.Red).Start();
await MyUIElement.Light(distance: 5, duration: 2500, delay: 250, color: Colors.Red).StartAsync(); //Light animation can be awaited
```
```vb
MyUIElement.Light(distance:=5, duration:=2500, delay:=250, color:=Colors.Red).Start()
Await MyUIElement.Light(distance:=5, duration:=2500, delay:=250, color:=Colors.Red).StartAsync()  ' Light animation can be awaited
```

## Sample Output

![Light Behavior animation](../resources/images/Animations/Light/Sample-Output.gif)

## Properties

| Property | Type | Description |
| -- | -- | -- |
| Color | Brush | The color of the spot light |
| Delay | double | The delay for the animation to begin |
| Distance | double | The distance of the spotlight. 0 being the furthest. |
| Duration | double | The duration in milliseconds |

## Methods

| Methods | Return Type | Description |
| -- | -- | -- |
| Light(AnimationSet, Double, Double, Double) | AnimationSet | Animates a point light and it's distance |
| Light(FrameworkElement, Double, Double, Double) | AnimationSet | Animates a point light and it's distance |

## Examples

- The light behavior is great at drawing the user's eye towards a particular pieces of user interface. Closer the light source, the more focused it will be, but, will make the overall UI element darker. The further away from the light source the more the light will spread over the UIElement.
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

    ![Use Case 1 Output](../resources/images/Animations/Chaining-Animations-Light-Offset-Saturation-Scale.gif)

## Sample Project

[Light Behavior Sample Page Source](https://github.com/Microsoft/UWPCommunityToolkit/tree/master/Microsoft.Toolkit.Uwp.SampleApp/SamplePages/Light). You can see this in action in [Windows Community Toolkit Sample App](https://www.microsoft.com/store/apps/9NBLGGH4TLCQ)

## Requirements

| Device family | Universal, 10.0.14393.0 or higher   |
| ---------------------------------------------------------------- | ----------------------------------- |
| Namespace                                                        | Microsoft.Toolkit.Uwp.UI.Animations |
| NuGet package | [Microsoft.Toolkit.Uwp.UI.Animations](https://www.nuget.org/packages/Microsoft.Toolkit.Uwp.UI.Animations/) |

## API

* [Light source code](https://github.com/Microsoft/UWPCommunityToolkit/blob/master/Microsoft.Toolkit.Uwp.UI.Animations/Behaviors/Light.cs)

## Related Topics

- [SceneLightingEffect Class](https://docs.microsoft.com/uwp/api/Windows.UI.Composition.Effects.SceneLightingEffect)
