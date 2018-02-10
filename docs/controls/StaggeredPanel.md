---
title: StaggeredPanel
author: skendrot
description: Arranges child elements into a staggered grid pattern.
keywords: windows 10, uwp, uwp community toolkit, uwp toolkit, StaggeredPanel 
---

# StaggeredPanel
The [StaggeredPanel](https://docs.microsoft.com/en-us/dotnet/api/microsoft.toolkit.uwp.ui.controls.staggeredpanel) allows for layout of items in a column approach where an item will be added to whichever column has used the least amount of space.

<!-- Use below format to display note
> [!NOTE]
Some note
-->

## Syntax

**XAML**

```xaml
<ItemsControl>
    <ItemsControl.ItemTemplate>
        <DataTemplate>
            <Image Source="{Binding Thumbnail}"/>
        </DataTemplate>
    </ItemsControl.ItemTemplate>
    <ItemsControl.ItemsPanel>
        <ItemsPanelTemplate>
            <controls:StaggeredPanel/>
        </ItemsPanelTemplate>
    </ItemsControl.ItemsPanel>
</ItemsControl>
```

## Sample Output

![StaggeredPanel](../resources/images/Controls-StaggeredPanel.jpg "StaggeredPanel")

## Properties

<!-- Explain all properties in a table format -->

| Property | Type | Description |
| -- | -- | -- |
| DesiredColumnWidth | double | The desired width of each column. The width of columns can exceed the DesiredColumnWidth if the HorizontalAlignment is set to Stretch. |
| Padding | Thickness | The dimensions of the space between the edge and its child as a Thickness value. Thickness is a structure that stores dimension values using pixel measures. |

## Sample Code

<!-- Link to the sample page in the UWP Community Toolkit Sample App -->
[StaggeredPanel Sample Page](https://github.com/Microsoft/UWPCommunityToolkit/tree/master/Microsoft.Toolkit.Uwp.SampleApp/SamplePages/StaggeredPanel). You can see this in action in UWP Community Toolkit Sample App.

## Default Template

[StaggeredPanel XAML File](https://github.com/Microsoft/UWPCommunityToolkit/blob/master/Microsoft.Toolkit.Uwp.UI.Controls/StaggeredPanel/StaggeredPanel.xaml) is the XAML template used in the toolkit for the default styling.

## Requirements

| [Device family](http://go.microsoft.com/fwlink/p/?LinkID=526370#device-families) | Universal, 10.0.14393.0 or higher   |
| -- | -- |
| Namespace | Microsoft.Toolkit.Uwp.UI.Controls |
| NuGet package | [Microsoft.Toolkit.Uwp.UI.Controls](https://www.nuget.org/packages/Microsoft.Toolkit.Uwp.UI.Controls/) |

## API Source Code

- [StaggeredPanel](https://github.com/Microsoft/UWPCommunityToolkit/tree/master/Microsoft.Toolkit.Uwp.UI.Controls/StaggeredPanel)