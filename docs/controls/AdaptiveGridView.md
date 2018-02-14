---
title: AdaptiveGridView XAML Control
author: nmetulev
description: The AdaptiveGridView Control presents items in a evenly-spaced set of columns to fill the total available display space.
keywords: windows 10, uwp, uwp community toolkit, uwp toolkit, AdaptiveGridView, xaml control, xaml
---

# AdaptiveGridView XAML Control 

The [AdaptiveGridView Control](https://docs.microsoft.com/dotnet/api/microsoft.toolkit.uwp.ui.controls.adaptivegridview) presents items in a evenly-spaced set of columns to fill the total available display space. It reacts to changes in the layout as well as the content so it can adapt to different form factors automatically.

## Syntax

```xaml
<Page ...
     xmlns:controls="using:Microsoft.Toolkit.Uwp.UI.Controls"/>

<controls:AdaptiveGridView  Name="AdaptiveGridViewControl"
    ItemHeight="200"
    DesiredWidth="300"
    ItemTemplate="{StaticResource PhotosTemplate}">
</controls:AdaptiveGridView>
```

## Sample Output

![AdaptiveGridView animation](../resources/images/Controls/AdaptiveGridView.gif)

## Properties

| Property | Type | Description |
| -- | -- | -- |
| DesiredWidth | double | Gets or sets the desired width of each item |
| ItemClickCommand | ICommand | Gets or sets the command to execute when an item is clicked and the IsItemClickEnabled property is true |
| ItemHeight | double | Gets or sets the height of each item in the grid |
| ItemsPanel | ItemsPanelTemplate | Gets the template that defines the panel that controls the layout of items |
| OneRowModeEnabled | Boolean | Gets or sets a value indicating whether only one row should be displayed |
| StretchContentForSingleRow | Boolean | Gets or sets a value indicating whether the control should stretch the content to fill at least one row |

> [!IMPORTANT]
ItemHeight property must be set when OneRowModeEnabled property set as `true`

## Sample Code

[AdaptiveGridView Sample Page Source](https://github.com/Microsoft/UWPCommunityToolkit/tree/master/Microsoft.Toolkit.Uwp.SampleApp/SamplePages/AdaptiveGridView). You can see this in action in [UWP Community Toolkit Sample App](https://www.microsoft.com/store/apps/9NBLGGH4TLCQ).

## Requirements

| Device family | Universal, 10.0.14393.0 or higher |
| -- | -- |
| Namespace | Microsoft.Toolkit.Uwp.UI.Controls |
| NuGet package | [Microsoft.Toolkit.Uwp.UI.Controls](https://www.nuget.org/packages/Microsoft.Toolkit.Uwp.UI.Controls/) |

## API

* [AdaptiveGridView source code](https://github.com/Microsoft/UWPCommunityToolkit/tree/master/Microsoft.Toolkit.Uwp.UI.Controls/AdaptiveGridView)

## Related Topics

- [GridView Class](https://docs.microsoft.com/uwp/api/Windows.UI.Xaml.Controls.GridView)
