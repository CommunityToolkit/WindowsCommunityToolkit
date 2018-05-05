---
title: Mouse.Cursor attached property
author: martinsuchan
description: Mouse.Cursor attached property enables you to easily change the mouse cursor over specific Framework elements.
keywords: windows 10, uwp, windows community toolkit, uwp community toolkit, uwp toolkit, Mouse, cursor, extensions
---

# Mouse.Cursor attached property

The [Mouse.Cursor attached property](https://docs.microsoft.com/dotnet/api/microsoft.toolkit.uwp.ui.extensions.mouse.cursor) enables you to easily change the mouse cursor over specific Framework elements.

## Syntax

```xaml
<Page ...
     xmlns:extensions="using:Microsoft.Toolkit.Uwp.UI.Extensions">

<UIElement extensions:Mouse.Cursor="Hand"/>
```

## Properties

| Property | Type | Description |
| -- | -- | -- |
| Mouse.Cursor | [CoreCursorType](https://docs.microsoft.com/uwp/api/Windows.UI.Core.CoreCursorType) | Set cursor type when mouse cursor over a Framework elements |

## Example

Here is a example of setting Mouse.Cursor

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
Even though Microsoft recommends in [UWP Design guidelines](https://docs.microsoft.com/windows/uwp/input-and-devices/mouse-interactions#cursors) hover effects instead of custom cursors over interactive elements, custom cursors can be useful in some specific scenarios.

## Limitations

Because the UWP framework does not support metadata on Attached Properties, specifically the [FrameworkPropertyMetadata.Inherits](https://msdn.microsoft.com/library/ms557301%28v=vs.110%29.aspx) flag, the Mouse.Cursor might not work properly in some very specific XAML layout scenarios when combining nested FrameworkElements with different Mouse.Cursor values set on them.

## Sample Code

[Mouse Sample Page](https://github.com/Microsoft/UWPCommunityToolkit/tree/master/Microsoft.Toolkit.Uwp.SampleApp/SamplePages/Mouse). You can see this in action in [Windows Community Toolkit Sample App](https://www.microsoft.com/store/apps/9NBLGGH4TLCQ).

## Requirements

| Device family | Universal, 10.0.14393.0 or higher |
| --- | --- |
| Namespace | Microsoft.Toolkit.Uwp.Extensions |
| NuGet package | [Microsoft.Toolkit.Uwp.UI](https://www.nuget.org/packages/Microsoft.Toolkit.Uwp.UI/) |

## API

* [Mouse.Cursor source code](https://github.com/Microsoft/UWPCommunityToolkit/blob/master/Microsoft.Toolkit.Uwp.UI/Extensions/Mouse)

