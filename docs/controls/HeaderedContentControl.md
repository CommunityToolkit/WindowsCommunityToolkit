---
title: HeaderedControlControl XAML Control
author: skendrot
ms.date: 11/09/2017
description: The HeaderedControlControl allows content to be displayed with a specified header.
keywords: windows 10, uwp, uwp community toolkit, uwp toolkit, HeaderedControlControl, XAML Control, xaml
---

# HeaderedControlControl XAML Control

The **HeaderedControlControl** is a UI control that allows content to be displayed with a specified header. The `Header` property can be any object and you can use the `HeaderTemplate` to specify a custom look to the header. Content for the HeaderedContentControl will align to the top left. This is to maintain the same functionality as the ContentControl.


## Properties
### Header
Gets or sets the data used for the header of each control.

The `Header` property can be set to a string, or any xaml elements. If binding the `Header` to an object that is not a string, use the `HeaderTemplate` to control how the content is rendered.
```xaml
<controls:HeaderedControlControl Header="This is the header!"/>

<controls:HeaderedControlControl>
    <controls:HeaderedControlControl.Header>
        <Border Background="Gray">
            <TextBlock Text="This is the header!" FontSize="16">
        </Border>
    </controls:HeaderedControlControl.Header>
</<controls:HeaderedControlControl>
```

### HeaderTemplate
Gets or sets the template used to display the content of the control's header.

Used to control the look of the header. The default value for the `HeaderTemplate` will display the string representation of the `Header`. Set this property if you need to bind the `Header` to an object.

```xaml
<controls:HeaderedControlControl Header="{Binding CustomObject}">
    <controls:HeaderedControlControl.HeaderTemplate>
        <DataTemplate>
            <TextBlock Text="{Binding Title}">
        </DataTemplate>
    </controls:HeaderedControlControl.HeaderTemplate>
</<controls:HeaderedControlControl>
```

> [!NOTE] Setting the `Background`, `BorderBrush` and `BorderThickness` properties will not have any effect on the HeaderedControlControl. This is to maintain the same functionality as the ContentControl.

## Example Image

![HeaderedControlControl](../resources/images/Controls-HeaderedContentControl.jpg "HeaderedControlControl")

## Example Code

[HeaderedControlControl Sample Page](https://github.com/Microsoft/UWPCommunityToolkit/tree/master/Microsoft.Toolkit.Uwp.SampleApp/SamplePages/HeaderedControlControl)

## Default Template

[HeaderedControlControl XAML File](https://github.com/Microsoft/UWPCommunityToolkit/blob/master/Microsoft.Toolkit.Uwp.UI.Controls/HeaderedControlControl/HeaderedControlControl.xaml) is the XAML template used in the toolkit for the default styling.

## Requirements (Windows 10 Device Family)

| [Device family](http://go.microsoft.com/fwlink/p/?LinkID=526370) | Universal, 10.0.14393.0 or higher |
| --- | --- |
| Namespace | Microsoft.Toolkit.Uwp.UI.Controls |

## API

* [HeaderedControlControl source code](https://github.com/Microsoft/UWPCommunityToolkit/tree/master/Microsoft.Toolkit.Uwp.UI.Controls/HeaderedControlControl)

