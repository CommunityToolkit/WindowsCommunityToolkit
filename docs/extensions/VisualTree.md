---
title: Visual Tree Extensions
author: nmetulev
description: The VisualTree extensions provide a collection of extensions methods for UI controls. It provides DependencyObject extensions to aid in using the VisualTreeHelper class.
keywords: windows 10, uwp, windows community toolkit, uwp community toolkit, uwp toolkit, Visual Tree, extentions
dev_langs:
  - csharp
  - vb
---

# Visual Tree Extensions

The [VisualTree extensions](https://docs.microsoft.com/dotnet/api/microsoft.toolkit.uwp.ui.extensions.visualtree) provide a collection of extensions methods for UI controls.

It provides [DependencyObject](https://docs.microsoft.com/uwp/api/Windows.UI.Xaml.DependencyObject) extensions to aid in using the [VisualTreeHelper](https://docs.microsoft.com/uwp/api/Windows.UI.Xaml.Media.VisualTreeHelper) class. The official [VisualTreeHelper](https://docs.microsoft.com/uwp/api/Windows.UI.Xaml.Media.VisualTreeHelper) documentation best explains reasons for walking the Visual Tree.

## Syntax

```csharp
// Include namespace to access extensions.
using Microsoft.Toolkit.Uwp.UI.Extensions;

// Find visual descendant control using its name.
var control = uiElement.FindDescendantByName("MyTextBox");

// Find first visual descendant control of a specified type.
control = uiElement.FindDescendant<ListView>();

// Find all visual descendant controls of the specified type.
foreach (var child in uiElement.FindDescendants<ListViewItem>())
{
	// ...
}

// Find first visual ascendant control using its name.
control = uiElement.FindAscendantByName("MyScrollViewer");

// Find first visual ascendant control of a specified type.
control = uiElement.FindAscendant<ScrollViewer>();
```
```vb
' Include namespace to access extensions.
Imports Microsoft.Toolkit.Uwp.UI.Extensions

' Find visual descendant control using its name.
Dim control = uiElement.FindDescendantByName("MyTextBox")

' Find first visual descendant control of a specified type.
control = uiElement.FindDescendant(Of ListView)()

' Find all visual descendant controls of the specified type.
For Each child In uiElement.FindDescendants(Of ListViewItem)()
    ' ...
Next

' Find first visual ascendant control using its name.
control = uiElement.FindAscendantByName("MyScrollViewer")
' Find first visual ascendant control of a specified type.
control = uiElement.FindAscendant(Of ScrollViewer)()
```

## Methods

| Methods | Return Type | Description |
| -- | -- | -- |
| FindAscendant<T>(this DependencyObject) | T where T : DependencyObject | Find first visual ascendant control of a specified type |
| FindAscendantByName(this DependencyObject, String) | FrameworkElement | Find visual ascendant `FrameworkElement` control using its name |
| FindDescendant<T>(this DependencyObject) | T where T : DependencyObject | Find first descendant control of a specified type |
| FindDescendantByName(this DependencyObject, String) | FrameworkElement | Find descendant `FrameworkElement` control using its name |
| FindDescendants<T>(this DependencyObject) | IEnumerable<T> where T : DependencyObject | Find all descendant controls of the specified type |

## Requirements

| Device family | Universal, 10.0.14393.0 or higher |
| --- | --- |
| Namespace | Microsoft.Toolkit.Uwp.UI.Extensions |
| NuGet package | [Microsoft.Toolkit.Uwp.UI](https://www.nuget.org/packages/Microsoft.Toolkit.Uwp.UI/) |

## API

* [VisualTree extensions source code](https://github.com/Microsoft/UWPCommunityToolkit/blob/master/Microsoft.Toolkit.Uwp.UI/Extensions/Tree/VisualTree.cs)

## Related Topics

- [LogicalTree Extensions](https://docs.microsoft.com/en-us/windows/uwpcommunitytoolkit/extensions/logicaltree)
