---
title: Visual Tree Extensions
author: nmetulev
ms.date: 08/20/2017
description: The VisualTree extensions provide a collection of extensions methods for UI controls. It provides DependencyObject extensions to aid in using the VisualTreeHelper class.
keywords: windows 10, uwp, uwp community toolkit, uwp toolkit, Visual Tree, extentions
---

# Visual Tree Extensions

The **VisualTree** extensions provide a collection of extensions methods for UI controls.

It provides [DependencyObject][1] extensions to aid in using the [VisualTreeHelper][2] class.
The official [VisualTreeHelper][2] documentation best explains reasons for walking the Visual Tree.

See also [LogicalTree Extensions](LogicalTree.md).

## Example

```csharp

	// Include namespace to access extensions.
	using Microsoft.Toolkit.Uwp.UI;

	// Find visual descendant control using its name.
	var control = uiElement.FindDescendantByName("MyTextBox");

	// Find first visual descendant control of a specified type.
	control = uiElement.FindDescendant<ListView>();

	// Find all visual descendant controls of the specified type.
	foreach (var child in uiElement.FindDescendant<ListViewItem>())
	{
		// ...
	}

	// Find first visual ascendant control using its name.
	control = uiElement.FindAscendantByName("MyScrollViewer");

	// Find first visual ascendant control of a specified type.
	control = uiElement.FindAscendant<ScrollViewer>();
```

## Requirements (Windows 10 Device Family)

| [Device family](http://go.microsoft.com/fwlink/p/?LinkID=526370) | Universal, 10.0.14393.0 or higher |
| --- | --- |
| Namespace | Microsoft.Toolkit.Uwp.UI.Extensions |

## API

* [VisualTree extensions source code](https://github.com/Microsoft/UWPCommunityToolkit/blob/master/Microsoft.Toolkit.Uwp.UI/Extensions/Tree/VisualTree.cs)

[1]:https://docs.microsoft.com/en-us/uwp/api/Windows.UI.Xaml.DependencyObject
[2]:https://docs.microsoft.com/en-us/uwp/api/Windows.UI.Xaml.Media.VisualTreeHelper
