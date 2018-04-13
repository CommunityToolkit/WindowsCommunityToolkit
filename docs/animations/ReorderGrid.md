---
title: ReorderGridAnimation
author: nmetulev
description: The ReorderGridAnimation class allows your GridView controls to animate items into position when the size of the GridView changes.
keywords: windows 10, uwp, uwp community toolkit, uwp toolkit, ReorderGridAnimation
---

# ReorderGridAnimation

The ReorderGridAnimation class allows your GridView controls to animate items into position when the size of the GridView changes.

## Syntax

**XAML**

```xaml
<Page ...
    xmlns:animations="using:Microsoft.Toolkit.Uwp.UI.Animations"/>
<GridView x:Name="MyGridView"
          animations:ReorderGridAnimation.Duration="250"/>
```

**C#**

```csharp
MyGridView.SetValue(ReorderGridAnimation.DurationProperty, 250);
```
## Sample Output

![ReorderGridAnimation](../resources/images/Animations/ReorderGridAnimation/Sample-Output.gif)

## Sample Project

[ReorderGridAnimation Sample Page Source](https://github.com/Microsoft/UWPCommunityToolkit/tree/master/Microsoft.Toolkit.Uwp.SampleApp/SamplePages/ReorderGridAnimation). You can see this in action in [UWP Community Toolkit Sample App](https://www.microsoft.com/store/apps/9NBLGGH4TLCQ)

## Requirements

| [Device family](http://go.microsoft.com/fwlink/p/?LinkID=526370) | Universal, 10.0.14393.0 or higher   |
| ---------------------------------------------------------------- | ----------------------------------- |
| Namespace                                                        | Microsoft.Toolkit.Uwp.UI.Animations |
| NuGet package | [Microsoft.Toolkit.Uwp.UI.Animations](https://www.nuget.org/packages/Microsoft.Toolkit.Uwp.UI.Animations/) |
## API

* [ReorderGridAnimation source code](https://github.com/Microsoft/UWPCommunityToolkit/blob/master/Microsoft.Toolkit.Uwp.UI.Animations/ReorderGridAnimation.cs)
