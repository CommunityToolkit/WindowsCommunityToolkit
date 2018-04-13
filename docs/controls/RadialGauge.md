---
title: RadialGauge XAML Control
author: nmetulev
description: The Radial Gauge Control displays a value in a certain range using a needle on a circular face.
keywords: windows 10, uwp, uwp community toolkit, uwp toolkit, Radial Gauge, RadialGauge, xaml control, xaml
---

# RadialGauge XAML Control

The **Radial Gauge Control** displays a value in a certain range using a needle on a circular face. This control will make data visualizations and dashboards more engaging with rich style and interactivity. 
The round gauges are powerful, easy to use, and highly configurable to present dashboards capable of displaying clocks, industrial panels, automotive dashboards, and even aircraft cockpits.

## How it works

The Radial Gauge supports animated transitions between configuration states. The control gradually animates as it redraws changes to the needle, needle position, scale range, color range, and more. 

## Syntax

```xaml
<controls:RadialGauge x:Name="RadialGaugeControl"
	Column="1"
	Value="70"
	Minimum="0"
	Maximum="180"
	TickSpacing="20"
	ScaleWidth="26"
	Unit="Units"
	TickBrush="Gainsboro"
	ScaleTickBrush="{ThemeResource ApplicationPageBackgroundThemeBrush}"
	NeedleWidth="5" 
	TickLength="18">
</controls:RadialGauge>
```

## Example Image

![RadialGauge animation](../resources/images/Controls-RadialGauge.gif "RadialGauge")


## Control style and template
You can modify the default [Style](https://docs.microsoft.com/en-us/uwp/api/windows.ui.xaml.style) and [ControlTemplate](https://docs.microsoft.com/en-us/uwp/api/windows.ui.xaml.controls.controltemplate) to give the control a unique appearance. For information about modifying a control's style and template, see [Styling controls](https://msdn.microsoft.com/windows/uwp/controls-and-patterns/styling-controls). The default style, template, and resources that define the look of the control are included in the RadialGauge.xaml file. For design purposes, RadialGauge.xaml is available on [GitHub](https://github.com/Microsoft/UWPCommunityToolkit/blob/master/Microsoft.Toolkit.Uwp.UI.Controls/RadialGauge/RadialGauge.xaml). Styles and resources from different versions of the SDK might have different values.

Starting in Toolkit version 2.2, RadialGauge.xaml includes resources that you can use to modify the colors of a control in different visual states without modifying the control template. In apps that target this software development kit (SDK) or later, modifying these resources is preferred to setting properties such as Background and Foreground. For more info, see the Light-weight styling section of the Styling controls article.

This table shows the resources used by the [RadialGauge](https://docs.microsoft.com/dotnet/api/microsoft.toolkit.uwp.ui.controls.radialgauge) control.

| Resource key | Description |
| -- | -- | -- |
| RadialGaugeForegroundBrush | Label text color for the value of the gauge |
| RadialGaugeAccentBrush | Label text color for the units of the gauge |

## Example Code

[RadialGauge Sample Page](https://github.com/Microsoft/UWPCommunityToolkit/tree/master/Microsoft.Toolkit.Uwp.SampleApp/SamplePages/RadialGauge)

## Default Template 

[RadialGauge XAML File](https://github.com/Microsoft/UWPCommunityToolkit/blob/master/Microsoft.Toolkit.Uwp.UI.Controls/RadialGauge/RadialGauge.xaml) is the XAML template used in the toolkit for the default styling.

## Requirements (Windows 10 Device Family)

| [Device family](http://go.microsoft.com/fwlink/p/?LinkID=526370) | Universal, 10.0.14393.0 or higher |
| --- | --- |
| Namespace | Microsoft.Toolkit.Uwp.UI.Controls |

## API

* [RadialGauge source code](https://github.com/Microsoft/UWPCommunityToolkit/tree/master/Microsoft.Toolkit.Uwp.UI.Controls/RadialGauge)

