---
title: HeaderedItemsControl XAML Control
author: skendrot
ms.date: 11/09/2017
description: The HeaderedItemsControl allows items to be displayed with a specified header.
keywords: windows 10, uwp, uwp community toolkit, uwp toolkit, HeaderedItemsControl, XAML Control, xaml
---

# HeaderedItemsControl XAML Control

The **HeaderedItemsControl** is a UI control that allows content to be displayed with a specified header. The `Header` property can be any object and you can use the `HeaderTemplate` to specify a custom look to the header.

## Properties
### Header
Gets or sets the data used for the header of each control.

The `Header` property can be set to a string, or any xaml elements. If binding the `Header` to an object that is not a string, use the `HeaderTemplate` to control how the content is rendered.
```xaml
<controls:HeaderedItemsControl Header="This is the header!"/>

<controls:HeaderedItemsControl>
    <controls:HeaderedItemsControl.Header>
        <Border Background="Gray">
            <TextBlock Text="This is the header!" FontSize="16">
        </Border>
    </controls:HeaderedItemsControl.Header>
</<controls:HeaderedItemsControl>
```

### HeaderTemplate
Gets or sets the template used to display the content of the control's header.

Used to control the look of the header. The default value for the `HeaderTemplate` will display the string representation of the `Header`. Set this property if you need to bind the `Header` to an object.

```xaml
<controls:HeaderedItemsControl Header="{Binding CustomObject}">
    <controls:HeaderedItemsControl.HeaderTemplate>
        <DataTemplate>
            <TextBlock Text="{Binding Title}">
        </DataTemplate>
    </controls:HeaderedItemsControl.HeaderTemplate>
</<controls:HeaderedItemsControl>
```

> [!NOTE]
Setting the `Background`, `BorderBrush` and `BorderThickness` properties will not have any effect on the HeaderedItemsControl. This is to maintain the same functionality as the ItemsControl.

## Example Image

![HeaderedItemsControl](../resources/images/Controls-HeaderedItemsControl.jpg "HeaderedItemsControl")

## Example Code

[HeaderedItemsControl Sample Page](https://github.com/Microsoft/UWPCommunityToolkit/tree/master/Microsoft.Toolkit.Uwp.SampleApp/SamplePages/HeaderedItemsControl)

## Default Template

[HeaderedItemsControl XAML File](https://github.com/Microsoft/UWPCommunityToolkit/blob/master/Microsoft.Toolkit.Uwp.UI.Controls/HeaderedItemsControl/HeaderedItemsControl.xaml) is the XAML template used in the toolkit for the default styling.

## Requirements (Windows 10 Device Family)

| [Device family](http://go.microsoft.com/fwlink/p/?LinkID=526370) | Universal, 10.0.14393.0 or higher |
| --- | --- |
| Namespace | Microsoft.Toolkit.Uwp.UI.Controls |

## API

* [HeaderedItemsControl source code](https://github.com/Microsoft/UWPCommunityToolkit/tree/master/Microsoft.Toolkit.Uwp.UI.Controls/HeaderedItemsControl)

