---
title: Logical Tree Extensions
author: nmetulev
description: The LogicalTree extensions provide a collection of extensions methods for UI controls. It provides FrameworkElement extensions to aid in walking the logical tree of control structures.
keywords: windows 10, uwp, uwp community toolkit, uwp toolkit, Logical Tree, extentions
dev_langs:
  - csharp
  - vb
---

# Logical Tree Extensions

The **LogicalTree** extensions provide a collection of extensions methods for UI controls.

It provides [FrameworkElement][1] extensions to aid in walking the logical tree of control structures.

This differs from the *Visual Tree* where extra containers and styles can wrap other elements.
The Logical Tree instead defines how controls are directly connected through construction.
These methods can also be used on controls that aren't yet connected or rendered in the Visual Tree.

See also [VisualTree Extensions](VisualTree.md).

## Example

```csharp
// Include namespace to access extensions.
using Microsoft.Toolkit.Uwp.UI.Extensions;

// Find logical child control using its name.
var control = uiElement.FindChildByName("MyTextBox");

// Find first logical child control of a specified type.
control = uiElement.FindChild<ListView>();

// Find all logical child controls of the specified type.
foreach (var child in uiElement.FindChildren<ListViewItem>())
{
	// ...
}

// Find first logical parent using its name.
control = uiElement.FindParentByName("MyGrid");

// Find first logical parent control of a specified type.
control = uiElement.FindParent<Grid>();

// Retrieves the Content for the specified control from whatever its 'Content' Property may be.
var content = uiElement.GetContentControl();
```
```vb
' Include namespace to access extensions.
Imports Microsoft.Toolkit.Uwp.UI.Extensions

' Find logical child control using its name.
Dim control = uiElement.FindChildByName("MyTextBox")

' Find first logical child control of a specified type.
control = uiElement.FindChild(Of ListView)()

// Retrieves the Content for the specified control from whatever its 'Content' Property may be.
For Each child In uiElement.FindChildren(Of ListViewItem)()
    ' ...
Next

' Find first logical parent using its name.
control = uiElement.FindParentByName("MyGrid")

' Find first logical parent control of a specified type.
control = uiElement.FindParent(Of Grid)()

' Retrieves the Content for the specified control from whatever its 'Content' Property may be.
Dim content = uiElement.GetContentControl()
```

## Requirements (Windows 10 Device Family)

| [Device family](http://go.microsoft.com/fwlink/p/?LinkID=526370) | Universal, 10.0.14393.0 or higher |
| --- | --- |
| Namespace | Microsoft.Toolkit.Uwp.UI.Extensions |

## API

* [LogicalTree extensions source code](https://github.com/Microsoft/UWPCommunityToolkit/blob/master/Microsoft.Toolkit.Uwp.UI/Extensions/Tree/LogicalTree.cs)

[1]:https://docs.microsoft.com/en-us/uwp/api/Windows.UI.Xaml.FrameworkElement
