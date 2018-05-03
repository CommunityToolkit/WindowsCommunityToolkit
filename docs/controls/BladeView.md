---
title: BladeView XAML Control
author: nmetulev
description: The BladeView provides a container to host blades as extra detail pages in, for example, a master-detail scenario.
keywords: windows 10, uwp, windows community toolkit, uwp community toolkit, uwp toolkit, BladeView, XAML Control, xaml
dev_langs:
  - csharp
  - vb
---

# BladeView XAML Control 

The [BladeView Control](https://docs.microsoft.com/dotnet/api/microsoft.toolkit.uwp.ui.controls.bladeview) provides a container to host [BladeItem](https://docs.microsoft.com/dotnet/api/microsoft.toolkit.uwp.ui.controls.bladeitem) as extra detail pages in, for example, a master-detail scenario. The control is based on how the Azure Portal works.

## Syntax

```xaml
<Page ...
     xmlns:controls="using:Microsoft.Toolkit.Uwp.UI.Controls"/>

<controls:BladeView>
    <controls:BladeItem IsOpen="True" TitleBarVisibility="Collapsed">
        <!-- BladeItem content -->
    </controls:BladeItem>

    <controls:BladeItem x:Name="DefaultBlade" Header="A blade" IsOpen="False">
        <!-- BladeItem content -->
    </controls:BladeItem>
</controls:BladeView>
```

## Sample Output

![BladeView animation](../resources/images/Controls/BladeView.gif)

## Properties

### BladeView Properties

| Property | Type | Description |
| -- | -- | -- |
| ActiveBlades | IList<[BladeItem](https://docs.microsoft.com/dotnet/api/microsoft.toolkit.uwp.ui.controls.bladeitem)> | Description |
| AutoCollapseCountThreshold | int | Gets or sets a value indicating what the overflow amount should be to start auto collapsing blade items |
| BladeMode | [BladeMode](https://docs.microsoft.com/dotnet/api/microsoft.toolkit.uwp.ui.controls.blademode) | Gets or sets a value indicating whether blade mode (ex: whether blades are full screen or not) |

### BladeItem Properties

| Property | Type | Description |
| -- | -- | -- |
| CloseButtonBackground | Brush | Gets or sets the background color of the default close button in the title bar |
| CloseButtonForeground | Brush | Gets or sets the foreground color of the close button |
| IsOpen | bool | Gets or sets a value indicating whether this blade is opened |
| Title | string | Gets or sets the title to appear in the title bar |
| TitleBarBackground | Brush | Gets or sets the background color of the title bar |
| TitleBarForeground | Brush | Gets or sets the titlebar foreground color |
| TitleBarVisibility | Visibility | Gets or sets the visibility of the title bar for this blade |

## Events

### BladeView Events

| Events | Description |
| -- | -- |
| BladeClosed | Fires whenever a BladeItem is closed |
| BladeOpened | Fires whenever a BladeItem is opened |

### BladeItem Events

| Events | Description |
| -- | -- |
| VisibilityChanged | Fires when the blade is opened or closed |

## Examples

- If you want to use the BladeView for handling a flow of actions, you can use the `AutoCollapseCountThreshold` property to tell it to start auto collapsing BladeItems after a certain threshold count has been reached. This will also help keep a clean, uncluttered screen real estate.

    For example: if you set `AutoCollapseCountThreshold` to 3, the BladeView will start counting all BladeItems that are open in the BladeView and have their `TitleBarVisibility` property set to Visible. When the n+1 BladeItem, in our case the 4th one, is being added, the BladeView will auto collapse all n BladeItems except for the last one. All additional BladeItems that are added afterwards will trigger the same effect; collapse all BladeItems except for the last open one.

    *Sample Code*
    
    ```xaml
    <controls:BladeView AutoCollapseCountThreshold="3">
        <controls:BladeItem>
            <!-- BladeItem content -->
        </controls:BladeItem>
        <controls:BladeItem>
            <!-- BladeItem content -->
        </controls:BladeItem>
        .....
        .....
    </controls:BladeView>
    ```

## Sample Code

[BladeView Sample Page Source](https://github.com/Microsoft/UWPCommunityToolkit/tree/master/Microsoft.Toolkit.Uwp.SampleApp/SamplePages/BladeView). You can see this in action in [Windows Community Toolkit Sample App](https://www.microsoft.com/store/apps/9NBLGGH4TLCQ).

## Default Template 

[BladeView XAML File](https://github.com/Microsoft/UWPCommunityToolkit/blob/master/Microsoft.Toolkit.Uwp.UI.Controls/BladeView/BladeView.xaml) is the XAML template used in the toolkit for the default styling.

## Requirements

| Device family | Universal, 10.0.14393.0 or higher |
| -- | -- |
| Namespace | Microsoft.Toolkit.Uwp.UI.Controls |
| NuGet package | [Microsoft.Toolkit.Uwp.UI.Controls](https://www.nuget.org/packages/Microsoft.Toolkit.Uwp.UI.Controls/) |

## API

* [BladeView source code](https://github.com/Microsoft/UWPCommunityToolkit/tree/master/Microsoft.Toolkit.Uwp.UI.Controls/BladeView)

