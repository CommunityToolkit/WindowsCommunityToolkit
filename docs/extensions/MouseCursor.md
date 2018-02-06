---
title: Mouse.Cursor attached property
author: martinsuchan
ms.date: 11/05/2017
description: Mouse.Cursor attached property enables you to easily change the mouse cursor over specific Framework elements.
keywords: windows 10, uwp, uwp community toolkit, uwp toolkit, Mouse, cursor, extensions
---

# Mouse.Cursor attached property

The **Mouse.Cursor attached property** enables you to easily change the mouse cursor over specific Framework elements.

## Example

First we need to add the namespace declaration, in this case we added to the root Page element:
```xaml
xmlns:extensions="using:Microsoft.Toolkit.Uwp.UI.Extensions"
```

Then we can use the attached property on any FrameworkElement in XAML:
```xaml
extensions:Mouse.Cursor="Hand"
```

This is how it looks in full Page context:
```xaml
<Page x:Class="Microsoft.Toolkit.Uwp.SampleApp.SamplePages.MouseCursorPage"
    xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"    
    xmlns:extensions="using:Microsoft.Toolkit.Uwp.UI.Extensions">

    <Grid Background="{ThemeResource ApplicationPageBackgroundThemeBrush}">
        <Border extensions:Mouse.Cursor="Hand"
	        Width="220" Height="120" Background="DeepSkyBlue"
	        HorizontalAlignment="Center" VerticalAlignment="Center"/>
    </Grid>
</Page>
```

> [!NOTE]
Even though Microsoft recommends in [UWP Design guidelines](https://docs.microsoft.com/en-us/windows/uwp/input-and-devices/mouse-interactions#cursors) hover effects instead of custom cursors over interactive elements, custom cursors can be useful in some specific scenarios.

## Limitations
Because the UWP framework does not support metadata on Attached Properties, specifically the [FrameworkPropertyMetadata.Inherits](https://msdn.microsoft.com/en-us/library/ms557301%28v=vs.110%29.aspx) flag, the Mouse.Cursor might not work properly in some very specific XAML layout scenarios when combining nested FrameworkElements with different Mouse.Cursor values set on them.

## Requirements (Windows 10 Device Family)

| [Device family](http://go.microsoft.com/fwlink/p/?LinkID=526370) | Universal, 10.0.14393.0 or higher |
| --- | --- |
| Namespace | Microsoft.Toolkit.Uwp.Extensions |

## API

* [Mouse.Cursor source code](https://github.com/Microsoft/UWPCommunityToolkit/blob/master/Microsoft.Toolkit.Uwp.UI/Extensions/Mouse)

