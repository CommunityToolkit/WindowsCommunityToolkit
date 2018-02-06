---
title: Composition Animations in XAML
ms.date: 11/22/2017
description: The Composition Animations can be used directly from XAML including with Implicit animations
keywords: windows 10, uwp, uwp community toolkit, uwp toolkit, composition animations, animation, implicit animations, XAML, implicit, composition
---

# Composition Animations in XAML

[Composition animations](https://docs.microsoft.com/en-us/windows/uwp/composition/composition-animation) in the universal windows platform provide a powerful and efficient way to run animations in your application UI and have been designed to ensure that your animations run at 60 FPS independent of the UI thread.

These XAML elements enable developer to specify composition animations directly in their XAML code to enable scenarios such as Implicit animations

## Syntax

**XAML**

```xaml
<Page ...
     xmlns:animations="using:Microsoft.Toolkit.Uwp.UI.Animations"/>

 <animations:ScalarAnimation Target="Translation.Y" Duration="0:0:1" From="0" To="-200">
    <animations:ScalarKeyFrame Key="0.1" Value="30"></animations:ScalarKeyFrame>
    <animations:ScalarKeyFrame Key="0.5" Value="0.0"></animations:ScalarKeyFrame>
</animations:ScalarAnimation>
 ```

## Animations

### Common Properties
* **Duration (TimeSpan)**: The duration of the animation. Default is 400ms
* **Delay (TimeSpan)**: The delay before starting the animation. Default is 0ms
* **KeyFrames (KeyFrameCollection)**: Key frames for the animation. Each animation will only use the appropriate type KeyFrame (see below for each animation) and ExpressionKeyFrame
* **Target (string)**: The property to animate
* **ImplicitTarget (string)**: The property that, if changed, will invoke this animation. This property is only used when specifying Implicit Animations. If this value is not set, it will default to the **Target** property
* **From (T)**: Setting this value will insert a new key frame at Key 0
* **To (T)**: Setting this value will insert a new key frame at Key 1

### ScalarAnimation

Animation that animates a scalar (double) value. 

#### Accepted KeyFrame type
ScalarKeyFrame and ExpressionKeyFrame

### Vector2Animation, Vector3Animation, Vector4Animation

Animation that animates a value of type Vector2, Vector3, or Vector4

#### Accepted KeyFrame type
Vector2(3)(4)KeyFrame and ExpressionKeyFrame

To specify the Vector2, Vector3, or Vector4 value in XAML, the From and To properties are of type string. Use the following format to specify the value:
* Vector2 - "0" or "0, 0"
* Vector3 - "0" or "0, 0, 0"
* Vector4 - "0" or "0, 0, 0, 0"

### OpacityAnimation (Scalar)

ScalarAnimation where `Target = "Opacity". Animates the Visual.Opacity property

### RotationAnimation (Scalar)

ScalarAnimation where `Target = "RotationAngle". Animates the Visual.RotationAngle property

### RotationInDegreesAnimation (Scalar)

ScalarAnimation where `Target = "RotationAngleInDegrees". Animates the Visual.RotationAngleInDegrees property

### ScaleAnimation (Vector3)

Vector3Animation where `Target = "Scale". Animates the Visual.Scale property

### TranslationAnimation (Vector3)

Vector3Animation where `Target = "Translation". Animates the Visual.Translation property

### OffsetAnimations (Vector3)

Vector3Animation where `Target = "Offset". Animates the Visual.Offset property


## KeyFrames

### Common Properties

* **Key (double)**: A value between 0.0 and 1.0
* **Value (T)**: The value that should be reached at specified Key

### ExpressionKeyFrame

A KeyFrame of type string

### ScalarKeyFrame

A KeyFrame of type double

### Vector2KeyFrame, Vector3KeyFrame, Vector4KeyFrame

A KeyFrame of type string representing a Vector2, Vector3 or Vector4

To specify the Vector2, Vector3, or Vector4 value, use the following format:
* Vector2 - "0" or "0, 0"
* Vector3 - "0" or "0, 0, 0"
* Vector4 - "0" or "0, 0, 0, 0"


## AnimationCollection

A collection of animations. 

The AnimationCollection exposes the properties:
* **ContainsTranslationAnimation**: True if any of the animations in the collection target the **Visual.Translation** property.

The AnimationCollection exposes the events:
* **AnimationCollectionChanged**: Raised when an animation has been added, removed, or a value of an animation has changed

## Requirements

| [Device family](http://go.microsoft.com/fwlink/p/?LinkID=526370) | Universal, 10.0.15063.0 or higher   |
| ---------------------------------------------------------------- | ----------------------------------- |
| Namespace                                                        | Microsoft.Toolkit.Uwp.UI.Animations |
| NuGet package | [Microsoft.Toolkit.Uwp.UI.Animations](https://www.nuget.org/packages/Microsoft.Toolkit.Uwp.UI.Animations/) |

## API

* [Composition animations source code](https://github.com/Microsoft/UWPCommunityToolkit/tree/master/Microsoft.Toolkit.Uwp.UI.Animations/CompositionAnimations)

