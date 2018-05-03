---
title: Colors Helper
author: nmetulev
description: The Colors Helper lets users convert colors from text names, HTML hex, HSV, or HSL to Windows UI Colors
keywords: windows 10, uwp, windows community toolkit, uwp community toolkit, uwp toolkit, Colors Helper
dev_langs:
  - csharp
  - vb
---

# Colors Helper

The [Colors Helper](https://docs.microsoft.com/dotnet/api/microsoft.toolkit.uwp.helpers.colorhelper) lets users convert colors from text names, HTML hex, HSV, or HSL to Windows UI Colors (and back again of course).

## Syntax

```csharp
// Be sure to include the using at the top of the file:
using Microsoft.Toolkit.Uwp;

// Given an HTML color, lets convert it to a Windows Color
Windows.UI.Color color = ColorHelper.ToColor("#3a4ab0");

// Also works with an Alpha code
Windows.UI.Color myColor = ColorHelper.ToColor("#ff3a4ab0");

// Given a color name, lets convert it to a Windows Color
Windows.UI.Color redColor = "Red".ToColor();
```
```vb
' Be sure to include the imports at the top of the file:
Imports Microsoft.Toolkit.Uwp

' Given an HTML color, lets convert it to a Windows Color
Dim color As Windows.UI.Color = ColorHelper.ToColor("#3a4ab0")

' Also works with an Alpha code
Dim myColor As Windows.UI.Color = ColorHelper.ToColor("#ff3a4ab0")

' Given a color name, lets convert it to a Windows Color
Dim redColor As Windows.UI.Color = "Red".ToColor()
```

## Methods

| Methods | Return Type | Description |
| -- | -- | -- |
| FromHsl(Double, Double, Double, Double) | Color | Returns a Color struct based on HSL model. Hue: 0-360, Saturation:  0-1, Lightness:  0-1, Alpha:  0-1 |
| FromHsv(Double, Double, Double, Double) | int | Returns a Color struct based on HSV model. Hue: 0-360, Saturation:  0-1, Lightness:  0-1, Alpha:  0-1 |
| ToColor(String) | Color | Returns a color based on XAML color string |
| ToHex(Color) | string | Converts a Color value to a string representation of the value in hexadecimal |
| ToHsl(Color) | [HslColor](https://docs.microsoft.com/dotnet/api/microsoft.toolkit.uwp.hslcolor) | Converts an RGBA Color the HSL representation |
| ToHsv(Color) | [HsvColor](https://docs.microsoft.com/dotnet/api/microsoft.toolkit.uwp.hsvcolor) | Converts an RGBA Color the HSV representation |
| ToInt(Color) | int | Returns the color value as a premultiplied Int32 - 4 byte ARGB structure |

## Sample Code

You can find more examples in our [unit tests](https://github.com/Microsoft/UWPCommunityToolkit/blob/master/UnitTests/Helpers/Test_ColorHelper.cs)

## Requirements

| Device family | Universal, 10.0.14393.0 or higher |
| --- | --- |
| Namespace | Microsoft.Toolkit.Uwp |
| NuGet package | [Microsoft.Toolkit.Uwp](https://www.nuget.org/packages/Microsoft.Toolkit.Uwp/) |

## API

* [Color Helper source code](https://github.com/Microsoft/UWPCommunityToolkit/blob/master/Microsoft.Toolkit.Uwp/Helpers/ColorHelper.cs)
