# Scale

The **Scale animation behavior** allows you to change a control's scale by increasing or decreasing the control through animation. For example, perhaps you want an entry field to change size when the user taps it.

## Syntax

```xaml

<interactivity:Interaction.Behaviors>
    <behaviors:Scale x:Name="Scale" 
                     ScaleX="2.0"
                     ScaleY="2.0"
                     CenterX="0.0"
                     CenterY="0.0" 
                     Duration="1.0" 
                     Delay="0.5" 
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

## Example Image

![Scale Behavior animation](../resources/images/Animations-Scale.gif "Scale Behavior")

## Requirements (Windows 10 Device Family)

| [Device family](http://go.microsoft.com/fwlink/p/?LinkID=526370) | Universal, 10.0.10586.0 or higher |
| --- | --- |
| Namespace | Microsoft.Toolkit.Uwp.UI.Animations |

## API

* [Scale source code](https://github.com/Microsoft/UWPCommunityToolkit/blob/master/Microsoft.Toolkit.Uwp.UI.Animations/Behaviors/Scale.cs)
