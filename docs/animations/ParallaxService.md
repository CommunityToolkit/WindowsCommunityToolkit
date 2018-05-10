---
title: ParallaxService
author: nmetulev
description: The ParallaxService class allows to create a parallax effect for items contained within an element that scrolls like a ScrollViewer or ListView.
keywords: windows 10, uwp, windows community toolkit, uwp community toolkit, uwp toolkit, parallaxservice
dev_langs:
  - csharp
  - vb
---

# ParallaxService

> [!WARNING]
The ParallaxService is deprecated and will be removed in a future major release. Please use the [ParallaxView](https://docs.microsoft.com/windows/uwp/style/parallax) available in the Fall Creators Update. Read the [Moving to ParallaxView](#parallaxview) section for more info.

The [ParallaxService class](https://docs.microsoft.com/dotnet/api/microsoft.toolkit.uwp.ui.animations.parallaxservice) allows to create a parallax effect for items contained within an element that scrolls like a ScrollViewer or ListView.

## Syntax

```xaml
<Page ...
    xmlns:animations="using:Microsoft.Toolkit.Uwp.UI.Animations"/>
<ScrollViewer>
    <Image Source="ms-appx:///Assets/Image.png"
            animations:ParallaxService.VerticalMultiplier="0.5" 
            animations:ParallaxService.HorizontalMultiplier="0.5"/>
    <!-- Other Controls -->
</ScrollViewer>
```

```csharp
ParallaxService.SetHorizontalMultiplier(MyUIElement, 0.5)
ParallaxService.SetVerticalMultiplier(MyUIElement, 0.5)
```
```vb
MyUIElement.SetValue(ParallaxService.VerticalMultiplierProperty, 0.5)
MyUIElement.SetValue(ParallaxService.HorizontalMultiplierProperty, 0.5)
```

## Sample Output

![ParallaxService](../resources/images/Animations/ParallaxService/Sample-Output.gif)

## Properties

| Property | Type | Description |
| -- | -- | -- |
| HorizontalMultiplier | double | Set HorizontalMultiplier |
| VerticalMultiplier | double | Get VerticalMultiplier |

## Methods

| Methods | Return Type | Description |
| -- | -- | -- |
| GetHorizontalMultiplier(UIElement) | double | Gets how fast the parallax effect should scroll horizontally |
| GetVerticalMultiplier(UIElement) | double | Gets how fast the parallax effect should scroll vertically |
| SetHorizontalMultiplier(UIElement, Double) | void  | Sets how fast the parallax effect should scroll horizontally |
| SetVerticalMultiplier(UIElement, Double) | void  | Sets how fast the parallax effect should scroll vertically |

## Sample Project

[ParallaxService Sample Page Source](https://github.com/Microsoft/UWPCommunityToolkit/tree/master/Microsoft.Toolkit.Uwp.SampleApp/SamplePages/ParallaxService). You can see this in action in [Windows Community Toolkit Sample App](https://www.microsoft.com/store/apps/9NBLGGH4TLCQ)

## <a name="parallaxview"></a> Moving to ParallaxView
The Windows 10 Fall Creators Update SDK now includes the [ParallaxView](https://docs.microsoft.com/windows/uwp/style/parallax) control among other new controls and APIs. The ParallaxService and ParallaxView share the same concepts and provide the same functionality. In fact, the ParallaxView adds even more functionality and can be used in even more scenarios.

However, the way the two are used is different. Unlike the ParallaxService, the ParallaxView is a control hosting the background element. This control ties the scroll position of a foreground element, such as a list, to a background element, such as an image. As you scroll through the foreground element, it animates the background element to create a parallax effect. To use the ParallaxView control, you provide a Source element, a background element, and set the VerticalShift (for vertical scrolling) and/or HorizontalShift (for horizontal scrolling) properties to a value greater than zero. To create a parallax effect, the ParallaxView must be behind the foreground element.

Here is an example of using the ParallaxView

```xaml
<Grid>
    <ParallaxView Source="{x:Bind ForegroundElement}" VerticalShift="50"> 

        <!-- Background element --> 
        <Image x:Name="BackgroundImage" Source="mybackgroundimage.png"
               Stretch="UniformToFill"/>
    </ParallaxView>

    <!-- Foreground element -->
    <ListView x:Name="ForegroundElement" ItemsSource="{x:Bind Items}">       
    </ListView>
</Grid>
```

## Requirements

| Device family | Universal, 10.0.14393.0 or higher   |
| ---------------------------------------------------------------- | ----------------------------------- |
| Namespace                                                        | Microsoft.Toolkit.Uwp.UI.Animations |
| NuGet package | [Microsoft.Toolkit.Uwp.UI.Animations](https://www.nuget.org/packages/Microsoft.Toolkit.Uwp.UI.Animations/) |

## API

* [ParallaxService source code](https://github.com/Microsoft/UWPCommunityToolkit/blob/master/Microsoft.Toolkit.Uwp.UI.Animations/ParallaxService.cs)
