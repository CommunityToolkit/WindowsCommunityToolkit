---
title: UniformGrid XAML Control
author: mhawker
ms.date: 11/09/2017
description: The UniformGrid Control presents items in a evenly-spaced set of rows or columns to fill the total available display space.
keywords: windows 10, uwp, uwp community toolkit, uwp toolkit, UniformGrid, xaml control, xaml
---

# UniformGrid XAML Control 

The **UniformGrid Control** presents items in a evenly-spaced set of rows or columns to fill the total available display space. 

If no value for `Rows` or `Columns` is provided, the UniformGrid will create a square layout based on the number of visable items.
Any provided RowDefinition or ColumnDefinition are ignored.

It differs from the [AdaptiveGridView](https://docs.microsoft.com/en-us/windows/uwpcommunitytoolkit/controls/adaptivegridview) which dictates 
its layout based on item sizes to where as the UniformGrid maintains the specified number of Rows and/or Columns.
In addition, UniformGrid is a `Panel` instead of an `ItemsControl`.  As such, it could be used as a Panel in such ItemsControls.

## Syntax

```xaml

<controls:UniformGrid Margin="10" Rows="1"
        HorizontalAlignment="Right"
        VerticalAlignment="Bottom">
      <Button Grid.Column="0" Content="No" FontSize="18" Margin="5" Padding="6,3" HorizontalAlignment="Stretch" VerticalAlignment="Stretch"/>
      <Button Grid.Column="1" Content="Yes, Absolutely" Margin="5" Padding="6,3" HorizontalAlignment="Stretch" VerticalAlignment="Stretch"/>
      <Button Grid.Column="2" Content="Maybe" Margin="5" Padding="6,3" HorizontalAlignment="Stretch" VerticalAlignment="Stretch"/>
</controls:UniformGrid>

```

## Example Code

[UniformGrid Sample Page](https://github.com/Microsoft/UWPCommunityToolkit/tree/master/Microsoft.Toolkit.Uwp.SampleApp/SamplePages/UniformGrid)

## Requirements (Windows 10 Device Family)

| [Device family](http://go.microsoft.com/fwlink/p/?LinkID=526370) | Universal, 10.0.14393.0 or higher |
| --- | --- |
| Namespace | Microsoft.Toolkit.Uwp.UI.Controls |

## API

* [UniformGrid source code](https://github.com/Microsoft/UWPCommunityToolkit/tree/master/Microsoft.Toolkit.Uwp.UI.Controls/UniformGrid)
