---
title: Colors Helper
author: nmetulev
ms.date: 08/20/2017
description: The Colors Helper lets users convert colors from text names, HTML hex, HSV, or HSL to Windows UI Colors
keywords: windows 10, uwp, uwp community toolkit, uwp toolkit, Colors Helper
---

# Colors Helper

The **Colors Helper** lets users convert colors from text names, HTML hex, HSV, or HSL to Windows UI Colors (and back again of course).

## Example

```csharp
	// Be sure to include the using at the top of the file:
	//using Microsoft.Toolkit.Uwp;

	// Given an HTML color, lets convert it to a Windows Color
	Windows.UI.Color color = ColorHelper.ToColor("#3a4ab0");

	// Also works with an Alpha code
	Windows.UI.Color myColor = ColorHelper.ToColor("#ff3a4ab0");

	// Given a color name, lets convert it to a Windows Color
	Windows.UI.Color redColor = "Red".ToColor();
```

You can find more examples in our [unit tests](https://github.com/Microsoft/UWPCommunityToolkit/blob/master/UnitTests/Helpers/Test_ColorHelper.cs)

## Requirements (Windows 10 Device Family)

| [Device family](http://go.microsoft.com/fwlink/p/?LinkID=526370) | Universal, 10.0.14393.0 or higher |
| --- | --- |
| Namespace | Microsoft.Toolkit.Uwp |

## API

* [Color Helper source code](https://github.com/Microsoft/UWPCommunityToolkit/blob/master/Microsoft.Toolkit.Uwp/Helpers/ColorHelper.cs)


