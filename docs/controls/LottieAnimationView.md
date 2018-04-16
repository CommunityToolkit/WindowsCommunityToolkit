---
title: LottieAnimationView XAML Control
author: azchohfi
description: The LottieAnimationView allows you to render an Adobe After Effects animation in your UWP App
keywords: windows 10, uwp, uwp community toolkit, uwp toolkit, lottie, lottieuwp, animations, aftereffects, XAML Control, xaml
---

# LottieAnimationView XAML Control

The **LottieAnimationView Control** allows you to render an Adobe After Effects animation in your UWP App. It lets you control the Progress, the Scale, the Frame Rate, the Speed, it's behavior for loops and the start and ending frames.

## How it works?
Lottie loads Json files that are exported from Adobe After Effects using a plugin called [Bodymovin](http://airbnb.io/lottie/after-effects/bodymovin-installation.html).

It parses this Json, creating all the layers and animations in memory, and draws them using [Win2D](https://github.com/Microsoft/Win2D). These animations are completely unbound to the UI Thread, so they have an excelent performance. They are drawn depending on the frame rate of the animation, so if your animation runs at 30 FPS, it will only draw it 30 times per second.

## LottieAnimationView Properties

| Property | Type | Description |
| -- | -- | -- |
| Source | string | Allows you to setup a file that will be loaded when the XAML control is loaded. If you want to reuse this LottieAnimationView to load other animations, you can use the `SetAnimationAsync` methods. |
| AutoPlay | bool | Used in conjunction with `Source`, when set to true, will automatically play this animation when it is loaded. |
| ColorFilter | ColorFilter | Use this to filter the colors that are drawn for each of the layers of the animation. This works as a dynamic property using the key "**" (all layers) on the ColorFilter property, creating a `SimpleColorFilter` with the `ColorFilter` as the `Color` to filter to. For more informations, refer to [Dynamic Properties](#dynamic-properties). |
| Progress | double | Gets or sets the progress of the animation. This value will be a double from 0 to 1. |
| Scale | double | Gets or sets the scale on the current composition. The only cost of this function is re-rendering the current frame so you may call it frequent to scale something up or down. The smaller the animation is, the better the performance will be. |
| FrameRate | double | Gets or sets the current frame rate that this animation is being executed. Right after a new animation is loaded, this value will contain the value that was exported from the Adobe After Effects file. |
| Speed | double | Gets or sets the playback speed. If speed < 0, the animation will play backwards (or is playing backwards). |
| RepeatMode | RepeatMode | Gets or sets what this animation should do when it reaches the end. This value is applied only when the repeat count is either greater than 0 or `LottieDrawable.Infinite` (-1). Defaults to `RepeatMode.Restart`. Returns either `RepeatMode.Reverse` or `Lottie.RepeatMode.Restart`. |
| RepeatCount | int |Gets or sets how many times the animation should be repeated. If the repeat count is 0, the animation is never repeated (plays only once). If the repeat count is greater than 0 or `LottieDrawable.Infinite`, the repeat mode will be taken into account. The repeat count is 0 by default. |
| MinFrame | double | Gets or sets the minimum frame that the animation will start from when playing or looping. |
| MaxFrame | double | Gets or sets the maximum frame that the animation will end at when playing or looping. |
| DefaultCacheStrategy | CacheStrategy | Gets or sets the default cache strategy for this `LottieAnimationView` |
| ImageAssetsFolder | string | Gets or sets the folder to look for the image assets. |
| TimesRepeated | int | Gets the number of times that this animation finished since the last call to `PlayAnimation`. |
| HardwareAcceleration | bool | Gets or sets a value indicating whether hardware acceleration is enabled for this view. |
| Composition | LottieComposition | Gets or sets a composition. You can set a default cache strategy if this view was inflated with xml by using `CacheStrategy`. |
| StartFrame | double | Gets the starting frame of the `LottieComposition` associated with this `LottieAnimationView` |
| EndFrame | double | Gets the ending frame of the `LottieComposition` associated with this `LottieAnimationView` |
| MinProgress | float | Sets the minimum progress that the animation will start from when playing or looping. |
| MaxProgress | float | Sets the maximum progress that the animation will end at when playing or looping. |
| IsAnimating | bool | Gets a value indicating whether the internal `ValueAnimator` is running or not. |
| Frame | float | Gets or sets the currently rendered frame. Sets the progress to the specified frame. If the composition isn't set yet, the progress will be set to the frame when it is. |
| Duration | long | Gets the animation duration, in milliseconds. |
| PerformanceTracker | PerformanceTracker | Gets the `PerformanceTracker` object associated with this `LottieAnimationView`. |
| FontAssetDelegate | FontAssetDelegate | Sets the `FontAssetDelegate` to be used on this `LottieAnimationView`. Use this to manually set fonts. |
| TextDelegate | TextDelegate | Sets the `TextDelegate` to be used on this `LottieAnimationView`. Set this to replace animation text with custom text at runtime |
| ImageDrawable | LottieDrawable | Sets the current `LottieDrawable` associated with this `LottieAnimationView` |
| ImageAssetDelegate | IImageAssetDelegate | Sets the image asset delegate. Use this if you can't bundle images with your app. This may be useful if you download the animations from the network or have the images saved to an SD Card. In that case, Lottie will defer the loading of the bitmap to this delegate. |
| PerformanceTrackingEnabled | bool | Sets a value indicating whether the performance tracking is enabled or not. |

## LottieAnimationView Methods

| Methods | Return Type | Description |
| -- | -- | -- |
| ResolveKeyPath(KeyPath) | List<KeyPath> | Takes a `KeyPath` potentially with wildcards or globstars and resolve it to a list of zero or more actual `KeyPath`s that exist in the current animation. |
| AddValueCallback<T>(KeyPath, LottieProperty, ILottieValueCallback<T>) | void | Add an property callback for the specified `KeyPath`. This KeyPath can resolve to multiple contents. In that case, the callbacks's value will apply to all of them. |
| AddValueCallback<T>(KeyPath, LottieProperty, SimpleLottieValueCallback<T>) | void | Overload of `AddValueCallback<T>(KeyPath, LottieProperty, ILottieValueCallback<T>)` that takes a delegate. |
| EnableMergePaths(bool) | void | Enable this to get merge path support. |
| IsMergePathsEnabled() | bool | Returns whether merge paths are enabled. |
| UseExperimentalHardwareAcceleration(bool) | void | This method enables or disables hardware acceletation. |
| SetAnimationAsync(string) | Task | Sets the animation from a file in the assets directory. This will load and deserialize the file asynchronously. |
| SetAnimationAsync(string, CacheStrategy) | Task | Sets the animation from a file in the assets directory. This will load and deserialize the file asynchronously. You may also specify a cache strategy. Specifying `CacheStrategy.Strong` will hold a strong reference to the composition once it is loaded and deserialized. `CacheStrategy.Weak` will hold a weak reference to said composition. |
| SetAnimationFromJsonAsync(string) | Task | Sets the animation from json string. This is the ideal API to use when loading an animation over the network because you can use the raw response body here and a converstion to a JsonObject never has to be done. |
| SetAnimationAsync(TextReader) | Task | Sets the animation from a TextReader. This will load and deserialize the file asynchronously. |
| HasMasks() | bool | Returns whether or not any layers in this composition has masks. |
| HasMatte() | bool | Returns whether or not any layers in this composition has a matte layer. |
| PlayAnimation() | void | Plays the animation from the beginning.If speed is < 0, it will start at the end and play towards the beginning. |
| ResumeAnimation() | void | Continues playing the animation from its current position. If speed <> 0, it will play backwards from the current position. |
| SetMinAndMaxFrame(float, float) | void | Sets the minimum and the maximum frame that the animation will start and end when playing or looping. |
| SetMinAndMaxProgress(float, float) | void | Sets the minimum and the maximum progress that the animation will start and end from when playing or looping. |
| ReverseAnimationSpeed() | void | Reverses the current animation speed. This does NOT play the animation. |
| UpdateBitmap(string, CanvasBitmap) | CanvasBitmap | Allows you to modify or clear a bitmap that was loaded for an image either automatically through ImageAssetsFolder or with an ImageAssetDelegate. Return the previous Bitmap or null. |
| CancelAnimation() | void | Cancels the animations completely. |
| PauseAnimation() | void | Pauses the animation at the current `Frame` |
| RemoveAllUpdateListeners() | void | Clears the `AnimatorUpdate` event handler. |
| RemoveAllAnimatorListeners() | void | Clears the `ValueChanged` event handler. |

## Events

| Events | Description |
| -- | -- |
| AnimatorUpdate | This event will be invoked whenever the frame of the animation changes. |
| ValueChanged | This event will be invoked whenever the internal animator is executed. |

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
    Source="Assets/Gears.json"
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
            Source="Assets/Gears.json"
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

## Related Topics
* [Adobe After Effects](http://www.adobe.com/products/aftereffects.html)
* [Lottie Web](https://github.com/airbnb/lottie-web)
* [Body Movin](http://airbnb.io/lottie/after-effects/bodymovin-installation.html)

## Requirements (Windows 10 Device Family)

| [Device family](http://go.microsoft.com/fwlink/p/?LinkID=526370) | Universal, 10.0.10586.0 or higher |
| --- | --- |
| Namespace | Microsoft.Toolkit.Uwp.UI.Controls.Lottie |

## API

* [LottieAnimationView source code](https://github.com/Microsoft/UWPCommunityToolkit/tree/master/Microsoft.Toolkit.Uwp.UI.Controls.Lottie/LottieAnimationView)
