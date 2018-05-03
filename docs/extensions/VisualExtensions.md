---
title: Composition Visual Attached Properties
author: nmetulev
description: The Composition Visual Attached Properties allow Composition Visual Properties to be modified directly in XAML
keywords: windows 10, uwp, windows community toolkit, uwp community toolkit, uwp toolkit, Visual, composition, xaml, attached property
---

# Composition Visual Attached Properties

The [Composition Visual](https://docs.microsoft.com/dotnet/api/microsoft.toolkit.uwp.ui.extensions.visualextensions) Attached Properties allow developers to modify common properties of the [object visual](https://docs.microsoft.com/uwp/api/Windows.UI.Composition.Visual) of an element directly in XAML. 

## Syntax

```xaml
<Page ...
    xmlns:extensions="using:Microsoft.Toolkit.Uwp.UI.Extensions">

<Border Height="100"
	Width="100"
	Background="Purple"
	extensions:VisualExtensions.CenterPoint="50,50,0"
	extensions:VisualExtensions.Opacity="0.5"
	extensions:VisualExtensions.RotationAngleInDegrees="80"
	extensions:VisualExtensions.Scale="2, 0.5, 1"
	extensions:VisualExtensions.NormalizedCenterPoint="0.5, 0.5, 0" />
```

## Properties

| Property | Type | Description |
| -- | -- | -- |
| AnchorPoint | [Vector2](https://docs.microsoft.com/uwp/api/Windows.Foundation.Numerics.Vector2) ("0" or "0, 0") | The point on the visual to be positioned at the visual's offset. Value is normalized with respect to the size of the visual |
| CenterPoint | [Vector3](https://docs.microsoft.com/uwp/api/Windows.Foundation.Numerics.Vector3) ("0" or "0, 0, 0") | The point about which rotation or scaling occurs |
| Offset | [Vector3](https://docs.microsoft.com/uwp/api/Windows.Foundation.Numerics.Vector3) ("0" or "0, 0, 0") | The offset of the visual relative to its parent or for a root visual the offset relative to the upper-left corner of the windows that hosts the visual |
| Opacity | double | The opacity of the visual |
| RotationAngle | double | The rotation angle in radians of the visual |
| RotationAngleInDegrees | double | The rotation angle of the visual in degrees |
| RotationAxis | [Vector3](https://docs.microsoft.com/uwp/api/Windows.Foundation.Numerics.Vector3) ("0" or "0, 0, 0") | The axis to rotate the visual around |
| Scale | [Vector3](https://docs.microsoft.com/uwp/api/Windows.Foundation.Numerics.Vector3) ("0" or "0, 0, 0") | The scale to apply to the visual |
| Size | [Vector2](https://docs.microsoft.com/uwp/api/Windows.Foundation.Numerics.Vector2) ("0" or "0, 0") | The width and height of the visual |
| NormalizedCenterPoint | [Vector3](https://docs.microsoft.com/uwp/api/Windows.Foundation.Numerics.Vector3) ("0" or "0, 0, 0") | The point about which rotation or scaling occurs, normalized between the values 0.0 and 1.0 |

## Requirements

| Device family | Universal, 10.0.15063.0 or higher |
| --- | --- |
| Namespace | Microsoft.Toolkit.Uwp.UI.Extensions |
| NuGet package | [Microsoft.Toolkit.Uwp.UI](https://www.nuget.org/packages/Microsoft.Toolkit.Uwp.UI/) |

## API

* [Visual extensions source code](https://github.com/Microsoft/UWPCommunityToolkit/blob/master/Microsoft.Toolkit.Uwp.UI/Extensions/Visual/VisualExtensions.cs)

