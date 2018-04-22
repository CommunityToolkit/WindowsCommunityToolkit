---
title: Expander Control
author: nmetulev
description: The Expander Control provides an expandable container to host any content.
keywords: windows 10, uwp, uwp community toolkit, uwp toolkit, Expander, xaml Control, xaml
---

# Expander Control

The **Expander Control** provides an expandable container to host any content.  It is a specialized form of a [HeaderedContentControl](HeaderedContentControl.md)
You can show or hide this content by interacting with the Header.

## Syntax

```xaml

<controls:Expander Header="Header of the expander"
  Foreground="White"
  Background="Gray"
  IsExpanded="True">

	<Grid Height="250">
    <TextBlock HorizontalAlignment="Center"
      TextWrapping="Wrap"
      Text="This is the content"
      VerticalAlignment="Center" />
  </Grid>
</controls:Expander>       

```

## Properties

| Property | Type | Description |
| -- | -- | -- |
| ContentOverlay | UIElement | Specifies alternate content to show when the Expander is collapsed. |
| ExpandDirection | ExpandDirection enum | Specifies the direction of where expanded content should be displayed in relation to the header. |
| HeaderStyle | Style | Specifies an alternate style template for the `ToggleButton` header control. |
| IsExpanded | bool | Indicates if the Expander is currently open or closed.  The default is `False`. |

### ContentOverlay

The `ContentOverlay` property can be used to define the content to be shown when the Expander is collapsed

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

### ExpandDirection

The `ExpandDirection` property can take 4 values that will expand the content based on the selected direction:

* `Down` - from top to bottom (default)
* `Up` - from bottom to top
* `Right` - from left to right
* `Left` - from right to left

### HeaderStyle

Allows creating an alternate style for the entire Expander header including the arrow symbol, in contrast to the `HeaderTemplate` which can control the content next to the arrow.

For instance to remove the header entirely from the Expander:

```xaml
  <Page.Resources>
    <Style x:Key="NoExpanderHeaderStyle" TargetType="ToggleButton">
      <Setter Property="Height" Value="0"/>
      <Setter Property="Template">
        <Setter.Value>
          <ControlTemplate TargetType="ToggleButton">
            <Grid/>
          </ControlTemplate>
        </Setter.Value>
      </Setter>
    </Style>
  </Page.Resources>

  <controls:Expander HeaderStyle="{StaticResource NoExpanderHeaderStyle}" IsExpanded="True">
    <TextBlock Text="My Content"/>
  </controls:Expander>
```

## Events

<!-- Explain all events in a table format -->

| Events | Description |
| -- | -- |
| Collapsed | Fires when the expander is closed. |
| Expanded | Fires when the expander is opened. |

## Example Image

![Expander animation](../resources/images/Controls-Expander.gif "Expander")

## Sample Code

[Expander Sample Page](https://github.com/Microsoft/UWPCommunityToolkit/tree/master/Microsoft.Toolkit.Uwp.SampleApp/SamplePages/Expander)

## Default Template 

[Expander XAML File](https://github.com/Microsoft/UWPCommunityToolkit/blob/master/Microsoft.Toolkit.Uwp.UI.Controls/Expander/Expander.xaml) is the XAML template used in the toolkit for the default styling.

## Requirements (Windows 10 Device Family)

| [Device family](http://go.microsoft.com/fwlink/p/?LinkID=526370) | Universal, 10.0.14393.0 or higher |
| --- | --- |
| Namespace | Microsoft.Toolkit.Uwp.UI.Controls |

## API

- [Expander source code](https://github.com/Microsoft/UWPCommunityToolkit/tree/master/Microsoft.Toolkit.Uwp.UI.Controls/Expander)

## Related Topics

- [HeaderedControlControl](HeaderedContentControl.md)
- [ToggleButton](https://docs.microsoft.com/en-us/uwp/api/Windows.UI.Xaml.Controls.Primitives.ToggleButton)
