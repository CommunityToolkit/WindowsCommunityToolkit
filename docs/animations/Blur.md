---
title: Blur animation behavior
author: nmetulev
description: The UWP Community Toolkit Blur animation behavior selectively blurs a XAML element by increasing or decreasing pixel size
keywords: windows 10, uwp, uwp community toolkit, uwp toolkit, blur, blur animation
---

# Blur

The Blur animation blurs a XAML element by increasing or decreasing pixel size. Blur animation is applied to all the XAML elements in its parent control/panel. Blur animation doesn't affect the functionality of the control.

## Syntax

**XAML**

```xaml
<Page ...
    xmlns:interactivity="using:Microsoft.Xaml.Interactivity"  
    xmlns:behaviors="using:Microsoft.Toolkit.Uwp.UI.Animations.Behaviors"/>

<interactivity:Interaction.Behaviors>
    <behaviors:Blur x:Name="BlurBehavior" 
            Value="10" 
            Duration="2500" 
            Delay="250"
            EasingType="Linear"
            AutomaticallyStart="True"/>
</interactivity:Interaction.Behaviors>
```

**C#**

```csharp
MyUIElement.Blur(value: 5, duration: 2500, delay: 250).Start();
await MyUIElement.Blur(value: 5, duration: 2500, delay: 250).StartAsync();  //Blur animation can be awaited
```

## Sample Output

![Blur Behavior animation](../resources/images/Animations/Blur/Sample-Output.gif)

## Examples

- Use this to shift the focus to foreground controls.

    **Sample Code**

    ```xaml
    <Grid>
        <Grid>
            <interactivity:Interaction.Behaviors>
                <behaviors:Blur x:Name="BlurBehavior" Value="5" Duration="2500" Delay="0" AutomaticallyStart="True"/>
            </interactivity:Interaction.Behaviors>
            <!-- XAML Element to be Blurred -->
            <!-- Background(even for Transparent background) of this Grid will also be Blurred -->
        </Grid>
        <!-- Foreground XAML Element -->
    </Grid>
    ```

    **Sample Output**

    ![Use Case 1 Output](../resources/images/Animations/Blur/Use-Case-1.gif)

- Use this to create chaining animations with other animations. Visit the [AnimationSet](AnimationSet.md) documentation for more information.

    **Sample Code**

    ```csharp
    var anim = MyUIElement.Blur(5).Fade(0.5f).Rotate(30);
    anim.SetDurationForAll(2500);
    anim.SetDelay(250);
    anim.Completed += animation_completed;
    anim.Start();
    ```
    
    **Sample Output**

    ![Use Case 2 Output](../resources/images/Animations/Chaining-Animations-Blur-Fade-Rotate.gif)

## Sample Project

[Blur Behavior Sample Page Source](https://github.com/Microsoft/UWPCommunityToolkit/tree/master/Microsoft.Toolkit.Uwp.SampleApp/SamplePages/Blur). You can see this in action in [UWP Community Toolkit Sample App](https://www.microsoft.com/store/apps/9NBLGGH4TLCQ)

## Requirements

| [Device family](http://go.microsoft.com/fwlink/p/?LinkID=526370) | Universal, 10.0.14393.0 or higher   |
| ---------------------------------------------------------------- | ----------------------------------- |
| Namespace                                                        | Microsoft.Toolkit.Uwp.UI.Animations |
| NuGet package | [Microsoft.Toolkit.Uwp.UI.Animations](https://www.nuget.org/packages/Microsoft.Toolkit.Uwp.UI.Animations/) |

## API

* [Blur source code](https://github.com/Microsoft/UWPCommunityToolkit/blob/master/Microsoft.Toolkit.Uwp.UI.Animations/Behaviors/Blur.cs)
