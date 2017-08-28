---
title: AdaptiveGridView XAML Control
author: nmetulev
ms.date: 08/20/2017
description: The AdaptiveGridView Control presents items in a evenly-spaced set of columns to fill the total available display space.
keywords: windows 10, uwp, uwp community toolkit, uwp toolkit, AdaptiveGridView, xaml control, xaml
---

# AdaptiveGridView XAML Control 

The **AdaptiveGridView Control** presents items in a evenly-spaced set of columns to fill the total available display space. It reacts to changes in the layout as well as the content so it can adapt to different form factors automatically.

If there are not enough items to fill one row, the control will stretch the items until all available space is filled. This can result in much wider items than specified. If you prefer your items to always stay close to the DesiredWidth, you can set the **StretchContentForSingleRow** property to **false**, to prevent further stretching.

## Syntax

```xml

<controls:AdaptiveGridView  Name="AdaptiveGridViewControl"
          ItemHeight="200"
          DesiredWidth="300"
          ItemTemplate="{StaticResource PhotosTemplate}">
</controls:AdaptiveGridView>

```

## Example Image

![AdaptiveGridView animation](../resources/images/Controls-AdaptiveGridView.gif "AdaptiveGridView")

## Example Code

[AdaptiveGridView Sample Page](https://github.com/Microsoft/UWPCommunityToolkit/tree/master/Microsoft.Toolkit.Uwp.SampleApp/SamplePages/AdaptiveGridView)

## Requirements (Windows 10 Device Family)

| [Device family](http://go.microsoft.com/fwlink/p/?LinkID=526370) | Universal, 10.0.14393.0 or higher |
| --- | --- |
| Namespace | Microsoft.Toolkit.Uwp.UI.Controls |

## API

* [AdaptiveGridView source code](https://github.com/Microsoft/UWPCommunityToolkit/tree/master/Microsoft.Toolkit.Uwp.UI.Controls/AdaptiveGridView)

