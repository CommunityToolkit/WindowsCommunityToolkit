---
title: Composition Animations in XAML
author: nikolame
ms.date: 11/22/2017
description: The Composition Animations can be used directly from XAML including with Implicit animations
keywords: windows 10, uwp, uwp community toolkit, uwp toolkit, composition animations, animation, implicit animations, XAML, implicit, composition
---

# Composition Animations in XAML

[Composition animations](https://docs.microsoft.com/en-us/windows/uwp/composition/composition-animation) in the universal windows platform provide a powerful and efficient way to run animations in your application UI and have been designed to ensure that your animations run at 60 FPS independent of the UI thread.

These XAML elements enable developer to specify composition animations directly in their XAML code to enable scenarios such as Implicit animations

## Syntax

**XAML**

```xml
<Page ...
     xmlns:animations="using:Microsoft.Toolkit.Uwp.UI.Animations"/>

 <animations:ScalarAnimation Target="Translation.Y" Duration="0:0:1" From="0" To="-200">
    <animations:ScalarKeyFrame Key="0.1" Value="30"></animations:ScalarKeyFrame>
    <animations:ScalarKeyFrame Key="0.5" Value="0.0"></animations:ScalarKeyFrame>
</animations:ScalarAnimation>
 ```

## Animations

### Common Properties

### ScalarAnimation

### Vector2Animation, Vector3Animation, Vector4Animation

### OpacityAnimation

### RotationAnimation

### RotationInDegreesAnimation

### ScaleAnimation

### TranslationAnimation

### OffsetAnimations


## KeyFrames

### Common Properties

### ExpressionKeyFrame

### ScalarKeyFrame

### Vector2KeyFrame, Vector3KeyFrame, Vector4KeyFrame


## AnimationCollection


## Requirements

| [Device family](http://go.microsoft.com/fwlink/p/?LinkID=526370) | Universal, 10.0.15063.0 or higher   |
| ---------------------------------------------------------------- | ----------------------------------- |
| Namespace                                                        | Microsoft.Toolkit.Uwp.UI.Animations |
| NuGet package | [Microsoft.Toolkit.Uwp.UI.Animations](https://www.nuget.org/packages/Microsoft.Toolkit.Uwp.UI.Animations/) |

## API

* [Composition animations source code](https://github.com/Microsoft/UWPCommunityToolkit/tree/master/Microsoft.Toolkit.Uwp.UI.Animations/CompositionAnimations)

