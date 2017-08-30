---
title: BindableValueHolder
author: nmetulev
ms.date: 08/20/2017
description: The BindableValueHolder lets users change several objects' states at a time.
keywords: windows 10, uwp, uwp community toolkit, uwp toolkit, BindableValueHolder
---

# BindableValueHolder

The **BindableValueHolder** lets users change several objects' states at a time.

## Example

You can use it to switch several object states by declaring it as a Resource (either in a page or control):

```xml

<helpers:BindableValueHolder x:Name="HighlightBrushHolder" Value="{StaticResource BlackBrush}" />

```

and using it like that:

```xml

<TextBlock x:Name="Label" Foreground="{Binding Value, ElementName=HighlightBrushHolder}" />

<TextBox x:Name="Field" BorderBrush="{Binding Value, ElementName=HighlightBrushHolder}" />

```

and switching it like that:

```xml

<VisualStateGroup x:Name="HighlightStates">
    <VisualState x:Name="Normal" />

    <VisualState x:Name="Wrong">
        <VisualState.Setters>
            <Setter Target="HighlightBrushHolder.Value" Value="{StaticResource RedBrush}" />
        </VisualState.Setters>
    </VisualState>
</VisualStateGroup>

```

## Requirements (Windows 10 Device Family)

| [Device family](http://go.microsoft.com/fwlink/p/?LinkID=526370) | Universal, 10.0.14393.0 or higher |
| --- | --- |
| Namespace | Microsoft.Toolkit.Uwp.UI |

## API

* [BindableValueHolder source code](https://github.com/Microsoft/UWPCommunityToolkit/blob/master/Microsoft.Toolkit.Uwp.UI/Helpers/BindableValueHolder.cs)