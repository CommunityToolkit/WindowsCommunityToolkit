# MosaicControl XAML Control

The **Mosaic Control** is a control that repeat an image many times. It enables you to use animation and synchronization with a ScrollViewer to create parallax effect. XAML or Microsoft Composition are automatically used to render the control.

## Syntax

```xaml

<controls:MosaicControl x:Name="Mosaic1"
	OffsetX="-10" 
	OffsetY="10"
	IsAnimated="True"
	ScrollViewerContainer="{x:Bind FlipView}"
	ParallaxSpeedRatio="1.2"
	/>

```

## Example Image

![MosaicControl animation](../resources/images/MosaicControl.gif "MosaicControl")

## Example Code

[MosaicControl Sample Page](https://github.com/Microsoft/UWPCommunityToolkit/tree/master/Microsoft.Toolkit.Uwp.SampleApp/SamplePages/MosaicControl)

## Default Template 

[MosaicControl XAML File](https://github.com/Microsoft/UWPCommunityToolkit/blob/master/Microsoft.Toolkit.Uwp.UI.Controls/MosaicControl/MosaicControl.xaml) is the XAML template used in the toolkit for the default styling.

## Requirements (Windows 10 Device Family)

| [Device family](http://go.microsoft.com/fwlink/p/?LinkID=526370) | Universal, 10.0.10586.0 or higher |
| --- | --- |
| Namespace | Microsoft.Toolkit.Uwp.UI.Controls |

## API

* [MosaicControl source code](https://github.com/Microsoft/UWPCommunityToolkit/tree/master/Microsoft.Toolkit.Uwp.UI.Controls/MosaicControl)