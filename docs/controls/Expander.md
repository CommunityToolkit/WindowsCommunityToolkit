---
title: Expander Control
author: nmetulev
description: The Expander Control provides an expandable container to host any content.
keywords: windows 10, uwp, uwp community toolkit, uwp toolkit, Expander, xaml Control, xaml
---

# Expander Control

The [Expander Control](https://docs.microsoft.com/dotnet/api/microsoft.toolkit.uwp.ui.controls.expander) provides an expandable container to host any content. You can show or hide this content by toggling a Header.

## Syntax

```xaml
<Page ...
     xmlns:controls="using:Microsoft.Toolkit.Uwp.UI.Controls"/>

<controls:Expander Header="Header of the expander" Foreground="White"
                   Background="Gray" IsExpanded="True">
	<!-- Expander content -->
</controls:Expander>       
```

## Sample Output

![Expander animation](../resources/images/Controls/Expander.gif)

## Properties

| Property | Type | Description |
| -- | -- | -- |
| ContentOverlay | ContentPresenter | Gets or sets the content to be overlay |
| ExpandDirection | [ExpandDirection](https://docs.microsoft.com/dotnet/api/microsoft.toolkit.uwp.ui.controls.expanddirection) | Gets or sets a value indicating whether the Expand Direction of the control like Down, Up, Right, Left |
| Header | string | Gets or sets a value indicating whether the Header of the control |
| HeaderTemplate | DataTemplate | Gets or sets a value indicating whether the HeaderTemplate of the control |
| IsExpanded | bool | Gets or sets a value indicating whether the content of the control is opened/visible or closed/hidden |

## Events

| Events | Description |
| -- | -- |
| Collapsed | Fires when the expander is closed |
| Expanded | Fires when the expander is opened |

## Examples

- The ContentOverlay property can be used to define the content to be shown when the Expander is collapsed

    *Sample Code*
    ```xaml
    <controls:Expander Header="Header">
      <Grid>
        <TextBlock Text="Expanded content" />
      </Grid>

      <controls:Expander.ContentOverlay>
        <Grid MinHeight="250">
          <TextBlock Text="Collapsed content" />
        </Grid>
      </controls:Expander.ContentOverlay>
    </controls:Expander>
    ```

## Sample Code

[Expander Sample Page Source](https://github.com/Microsoft/UWPCommunityToolkit/tree/master/Microsoft.Toolkit.Uwp.SampleApp/SamplePages/Expander). You can see this in action in [UWP Community Toolkit Sample App](https://www.microsoft.com/store/apps/9NBLGGH4TLCQ).

## Default Template 

[Expander XAML File](https://github.com/Microsoft/UWPCommunityToolkit/blob/master/Microsoft.Toolkit.Uwp.UI.Controls/Expander/Expander.xaml) is the XAML template used in the toolkit for the default styling.

## Requirements

| Device family | Universal, 10.0.14393.0 or higher |
| -- | -- |
| Namespace | Microsoft.Toolkit.Uwp.UI.Controls |
| NuGet package | [Microsoft.Toolkit.Uwp.UI.Controls](https://www.nuget.org/packages/Microsoft.Toolkit.Uwp.UI.Controls/) |

## API

* [Expander source code](https://github.com/Microsoft/UWPCommunityToolkit/tree/master/Microsoft.Toolkit.Uwp.UI.Controls/Expander)