---
title: LottieAnimationView XAML Control
author: azchohfi
description: The LottieAnimationView allows you to render an Adobe After Effects animation in your UWP App
keywords: windows 10, uwp, uwp community toolkit, uwp toolkit, lottie, lottieuwp, animations, aftereffects, XAML Control, xaml
---

# LottieAnimationView XAML Control

The **LottieAnimationView Control** allows you to render an Adobe After Effects animation in your UWP App. It lets you control the Progress, the Scale, the Frame Rate, the Speed, it's behaviour for loops and the start and ending frames.

## How it works?
Lottie loads Json files that are exported from Adobe After Effects using a plugin called [Bodymovin](http://airbnb.io/lottie/after-effects/bodymovin-installation.html).

It parses this Json, creating all the layers and animations in memory, and draws them using [Win2D](https://github.com/Microsoft/Win2D). These animations are completely unbound to the UI Thread, so they have an excelent performance. They are drawn depending on the frame rate of the animation, so if your animation runs at 30 FPS, it will only draw it 30 times per second.

## LottieAnimationView Properties

### FileName
Allows you to setup a file that will be loaded when the XAML control is loaded. If you want to reuse this LottieAnimationView to load other animations, you can use the `SetAnimationAsync` methods.

### AutoPlay
Used in conjunction with `FileName`, when set to true, will automatically play this animation when it is loaded.

### ColorFilter
Use this to filter the colors that are drawn for each of the layers of the animation. This works as a dynamic property using the key "**" (all layers) on the ColorFilter property, creating a `SimpleColorFilter` with the `ColorFilter` as the `Color` to filter to. For more informations, refer to [Dynamic Properties](#dynamic-properties).

### Progress
Gets or sets the progress of the animation. This value will be a double from 0 to 1.

### Scale
Gets or sets the scale on the current composition. The only cost of this function is re-rendering the current frame so you may call it frequent to scale something up or down. The smaller the animation is, the better the performance will be.

### FrameRate
Gets or sets the current frame rate that this animation is being executed. Right after a new animation is loaded, this value will contain the value that was exported from the Adobe After Effects file.

### Speed
Gets or sets the playback speed. If speed < 0, the animation will play backwards (or is playing backwards).

### RepeatMode
Gets or sets what this animation should do when it reaches the end. This
value is applied only when the repeat count is either greater than
0 or `LottieDrawable.Infinite` (-1). Defaults to `RepeatMode.Restart`.
Returns either `RepeatMode.Reverse` or `Lottie.RepeatMode.Restart`.

### RepeatCount
Gets or sets how many times the animation should be repeated. If the repeat count is 0, the animation is never repeated (plays only once). If the repeat count is greater than 0 or `LottieDrawable.Infinite`, the repeat mode will be taken into account. The repeat count is 0 by default.

### MinFrame
Gets or sets the minimum frame that the animation will start from when playing or looping.

### MaxFrame
Gets or sets the maximum frame that the animation will end at when playing or looping.

## Caching animations
Use `SetAnimationAsync(string, CacheStrategy)` to specify the `CacheStrategy` that should be used for this load, or set `DefaultCacheStrategy` to change the default for that instance. `CacheStrategy` can be `Strong`, `Weak`, or `None`.
Use this option if your animation will be reused frequently.

## Dynamic Properties
You can update a LottieAnimationView's properties dynamically, at runtime. This can be used for a variety of purposes such as:
* Theming (day and night or arbitrary themes).
* Responding to events such as an error or a success.
* Animating a single part of an animation in response to an event.
* Responding to view sizes or other values not known at design time.

``You can refer to the original documentation, as Lottie for UWP reimplements all the same functionality (http://airbnb.io/lottie/android/dynamic.html).``

### Usage
```cs
animationView.AddValueCallback<ColorFilter>(
    new KeyPath("Shape Layer", "Rectangle", "Fill"),
    LottieProperty.ColorFilter,
    new LottieValueCallback<ColorFilter>(new SimpleColorFilter(Colors.Red)));
```
```cs
animationView.AddValueCallback<Color?>(
    new KeyPath("Shape Layer", "Rectangle", "Fill"),,
    LottieProperty.Color,
    frameInfo =>
    {
        return frameInfo.OverallProgress < 0.5 ? Colors.Green : Colors.Red;
    });
```

## Syntax

```xaml
<lottie:LottieAnimationView
    FileName="Assets/Gears.json"
    AutoPlay="True"
    ColorFilter="Transparent"
    Progress="0.287885218858719"
    Scale="1"
    FrameRate="60"
    Speed="1"
    RepeatMode="Restart"
    RepeatCount="-1"
    MinFrame="0"
    MaxFrame="52"/>
```


## Example Image

![LottieAnimationView animation](../resources/images/Controls-LottieAnimationView.gif "LottieAnimationView")

## Example Code

[LottieAnimationView Sample Page](https://github.com/Microsoft/UWPCommunityToolkit/tree/master/Microsoft.Toolkit.Uwp.SampleApp/SamplePages/LottieAnimationView)

The following sample demonstrates how to add a LottieAnimationView Control.

```xaml
<Page x:Class="Microsoft.Toolkit.Uwp.SampleApp.SamplePages.LottieAnimationViewPage"
    xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
    xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
    xmlns:local="using:Microsoft.Toolkit.Uwp.SampleApp.SamplePages"
    xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
    xmlns:lottie="using:Microsoft.Toolkit.Uwp.UI.Controls"
    mc:Ignorable="d">

    <Grid Background="{StaticResource Brush-Grey-05}">
        <lottie:LottieAnimationView
            FileName="Assets/Gears.json"
            AutoPlay="True"
            ColorFilter="Transparent"
            Progress="0.287885218858719"
            Scale="1"
            FrameRate="60"
            Speed="1"
            RepeatMode="Restart"
            RepeatCount="-1"
            MinFrame="0"
            MaxFrame="52"/>
    </Grid>
</Page>


```

## Requirements (Windows 10 Device Family)

| [Device family](http://go.microsoft.com/fwlink/p/?LinkID=526370) | Universal, 10.0.10586.0 or higher |
| --- | --- |
| Namespace | Microsoft.Toolkit.Uwp.UI.Controls.Lottie |

## API

* [LottieAnimationView source code](https://github.com/Microsoft/UWPCommunityToolkit/tree/master/Microsoft.Toolkit.Uwp.UI.Controls.Lottie/LottieAnimationView)
