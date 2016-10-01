# Offset

The **Offset animation behavior** gets the number of pixels, from the origin of the associated control, then offsets the control. 

## Syntax

```xaml

<behaviors:Offset x:Name="OffsetBehavior" 
	OffsetX="25.0" 
	OffsetY="25.0"
	Duration="2.0" 
	Delay="0.5" 
	AutomaticallyStart="True"/>
</behaviors:Offset>

```

or directly from code:

```csharp

MyRectangle.Offset(
                offsetX: (float)OffsetX,
                offsetY: (float)OffsetY
                duration: Duration,
                delay: Delay);

```

Behavior animations can also be chained and awaited.

```csharp

    Element.Rotate(value: 30f, duration: 0.3).StartAsync();

    await Element.Rotate(value: 30f, duration: 0.3).StartAsync();

    var anim = element.Rotate(30f).Fade(0.5).Blur(5);
    anim.SetDurationForAll(2);
    anim.Completed += animation_completed;
    anim.StartAsync();

    anim.Stop();

```

[Offset Behavior Sample Page Source](https://github.com/Microsoft/UWPCommunityToolkit/tree/master/Microsoft.Toolkit.Uwp.SampleApp/SamplePages/Offset)

## Example Image

![Offset Behavior animation](../resources/images/Animations-Offset.gif "Offset Behavior")

## Requirements (Windows 10 Device Family)

| [Device family](http://go.microsoft.com/fwlink/p/?LinkID=526370) | Universal, 10.0.10586.0 or higher |
| --- | --- |
| Namespace | Microsoft.Toolkit.Uwp.UI.Animations |

## API

* [Offset source code](https://github.com/Microsoft/UWPCommunityToolkit/blob/master/Microsoft.Toolkit.Uwp.UI.Animations/Behaviors/Offset.cs)


