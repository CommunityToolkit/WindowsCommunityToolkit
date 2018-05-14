---
title: BindableValueHolder
author: nmetulev
description: The BindableValueHolder lets users change several objects' states at a time.
keywords: windows 10, uwp, windows community toolkit, uwp community toolkit, uwp toolkit, BindableValueHolder
---

# BindableValueHolder

The [BindableValueHolder](https://docs.microsoft.com/dotnet/api/microsoft.toolkit.uwp.ui.helpers.bindablevalueholder) lets users change several objects' states at a time.

## Syntax

```xaml
<helpers:BindableValueHolder x:Name="BindName" Value="{StaticResource BindValue}" />
```

## Properties

| Property | Type | Description |
| -- | -- | -- |
| Value | object | Gets or sets the held value |

## Example

You can use it to switch several object states by declaring it as a Resource (either in a page or control):

```xaml
<helpers:BindableValueHolder x:Name="HighlightBrushHolder" Value="{StaticResource BlackBrush}" />
```

and using it like that:

```xaml
<TextBlock x:Name="Label" Foreground="{Binding Value, ElementName=HighlightBrushHolder}" />

<TextBox x:Name="Field" BorderBrush="{Binding Value, ElementName=HighlightBrushHolder}" />
```

and switching it like that:

```xaml
<VisualStateGroup x:Name="HighlightStates">
    <VisualState x:Name="Normal" />

    <VisualState x:Name="Wrong">
        <VisualState.Setters>
            <Setter Target="HighlightBrushHolder.Value" Value="{StaticResource RedBrush}" />
        </VisualState.Setters>
    </VisualState>
</VisualStateGroup>
```

## Requirements

| Device family | Universal, 10.0.14393.0 or higher |
| --- | --- |
| Namespace | Microsoft.Toolkit.Uwp.UI |
| NuGet package | [Microsoft.Toolkit.Uwp.UI](https://www.nuget.org/packages/Microsoft.Toolkit.Uwp.UI/) |

## API

* [BindableValueHolder source code](https://github.com/Microsoft/UWPCommunityToolkit/blob/master/Microsoft.Toolkit.Uwp.UI/Helpers/BindableValueHolder.cs)