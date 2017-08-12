# Blur

The Blur animation blurs a XAML element by increasing or decreasing pixel size. Blur animation is applied to all the XAML elements in its parent control/panel. Blur animation doesn't affect the functionality of the control.

## Syntax

**In XAML**

```xml
<Page ...
    xmlns:interactivity="using:Microsoft.Xaml.Interactivity"  
    xmlns:behaviors="using:Microsoft.Toolkit.Uwp.UI.Animations.Behaviors"/>

<interactivity:Interaction.Behaviors>
    <behaviors:Blur x:Name="BlurBehavior" 
            Value="10" 
            Duration="500" 
            Delay="250" 
            AutomaticallyStart="True"/>
</interactivity:Interaction.Behaviors>
```

**In Code Behind**

```csharp
MyUIElement.Blur(value: 5, duration: 500, delay: 250).Start();
await MyUIElement.Blur(value: 5, duration: 500, delay: 250).StartAsync();  //Blur animation can be awaited
```

## Sample Output

![Blur Behavior animation](https://github.com/Vijay-Nirmal/UWPCommunityToolkit/blob/DocImprovements/docs/resources/images/Animations/Blur/Sample-Output.gif)

## Properties



## Use Cases

- Use this to shift the focus to foreground controls.

    **Sample Code**
    ```xml
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

    ![Use Case 1 Output](https://github.com/Vijay-Nirmal/UWPCommunityToolkit/blob/DocImprovements/docs/resources/images/Animations/Blur/Use-Case-1.gif)

- Use this to create chaining animations with other animations

    **Sample Code**
    ```csharp
    var anim = MyUIElement.Blur(5).Fade(5).Rotate(30);
    anim.SetDurationForAll(2500);
    anim.SetDelay(250);
    anim.Completed += animation_completed;
    anim.Start();
    ```
    **Sample Output**

    ![Use Case 2 Output](https://github.com/Vijay-Nirmal/UWPCommunityToolkit/blob/DocImprovements/docs/resources/images/Animations/Blur/Use-Case-2.gif)

## Sample Project

[Blur Behavior Sample Page Source](https://github.com/Microsoft/UWPCommunityToolkit/tree/master/Microsoft.Toolkit.Uwp.SampleApp/SamplePages/Blur). You can see this in action in [UWP Community Toolkit Sample App](https://www.microsoft.com/en-us/store/p/uwp-community-toolkit-sample-app/9nblggh4tlcq)

## Requirements (Windows 10 Device Family)

| [Device family](http://go.microsoft.com/fwlink/p/?LinkID=526370) | Universal, [Windows 10 Anniversary SDK 14393](https://blogs.windows.com/windowsexperience/2016/07/18/build14393/) 10.0.14393.0 |
| --- | --- |
| Namespace | Microsoft.Toolkit.Uwp.UI.Animations |

## API

* [Blur source code](https://github.com/Microsoft/UWPCommunityToolkit/blob/master/Microsoft.Toolkit.Uwp.UI.Animations/Behaviors/Blur.cs)
