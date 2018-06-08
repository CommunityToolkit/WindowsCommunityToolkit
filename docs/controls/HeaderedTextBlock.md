---
title: HeaderedTextBlock XAML Control
author: nmetulev
description: The HeaderedTextBlock Control provides a header for read-only text. This control is useful for displaying read-only forms, content, or a collection of items depending on the type. 
keywords: windows 10, uwp, windows community toolkit, uwp community toolkit, uwp toolkit, HeaderedTextBlock, XAML Control, xaml
---

# HeaderedTextBlock XAML Control

The [HeaderedTextBlock Control](https://docs.microsoft.com/dotnet/api/microsoft.toolkit.uwp.ui.controls.headeredtextblock) provides a header for read-only text. This control is useful for displaying read-only forms, content, or a collection of items depending on the type. 

## Syntax

```xaml
<controls:HeaderedTextBlock Header="HeaderedTextBlockControl" 
           Text="Windows Community Toolkit"  Orientation="Vertical"/>  
```

## Sample Output

![HeaderedTextBlock animation](../resources/images/Controls/HeaderedTextBlock.png)

## Properties

| Property | Type | Description |
| -- | -- | -- |
| Header | object | Gets or sets the header |
| HeaderTemplate | DataTemplate | Gets or sets the header style |
| HideTextIfEmpty | bool | Gets or sets a value indicating whether the Text TextBlock is hidden if its value is empty |
| Orientation | Orientation | Gets or sets the orientation |
| Text | string | Gets or sets the text |
| TextStyle | Style | Gets or sets the text style |

## Sample Code

[HeaderedTextBlock Sample Page Source](https://github.com/Microsoft/WindowsCommunityToolkit//tree/master/Microsoft.Toolkit.Uwp.SampleApp/SamplePages/HeaderedTextBlock). You can see this in action in [Windows Community Toolkit Sample App](https://www.microsoft.com/store/apps/9NBLGGH4TLCQ).

## Default Template

[HeaderedTextBlock XAML File](https://github.com/Microsoft/WindowsCommunityToolkit//blob/master/Microsoft.Toolkit.Uwp.UI.Controls/HeaderedTextBlock/HeaderedTextBlock.xaml) is the XAML template used in the toolkit for the default styling.

## Requirements

| Device family | Universal, 10.0.15063.0 or higher |
| -- | -- |
| Namespace | Microsoft.Toolkit.Uwp.UI.Controls |
| NuGet package | [Microsoft.Toolkit.Uwp.UI.Controls](https://www.nuget.org/packages/Microsoft.Toolkit.Uwp.UI.Controls/) |

## API

* [HeaderedTextBlock source code](https://github.com/Microsoft/WindowsCommunityToolkit//tree/master/Microsoft.Toolkit.Uwp.UI.Controls/HeaderedTextBlock)

