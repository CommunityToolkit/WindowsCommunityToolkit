---
title: HeaderedContentControl XAML Control
author: skendrot
description: The HeaderedContentControl allows content to be displayed with a specified header.
keywords: windows 10, uwp, windows community toolkit, uwp community toolkit, uwp toolkit, HeaderedContentControl, XAML Control, xaml
---

# HeaderedContentControl XAML Control

The [HeaderedContentControl](https://docs.microsoft.com/dotnet/api/microsoft.toolkit.uwp.ui.controls.headeredcontentcontrol) is a UI control that allows content to be displayed with a specified header. The `Header` property can be any object and you can use the `HeaderTemplate` to specify a custom look to the header. Content for the HeaderedContentControl will align to the top left. This is to maintain the same functionality as the ContentControl.

> [!NOTE]
Setting the `Background`, `BorderBrush` and `BorderThickness` properties will not have any effect on the HeaderedContentControl. This is to maintain the same functionality as the ContentControl.

## Syntax

```xaml
<Page ...
     xmlns:controls="using:Microsoft.Toolkit.Uwp.UI.Controls"/>

<controls:HeaderedContentControl>
    <!-- Header content or HeaderTemplate content -->
</<controls:HeaderedContentControl>
```

## Sample Output

![HeaderedContentControl](../resources/images/Controls/HeaderedContentControl.jpg)

## Properties

| Property | Type | Gets or sets the data used for the header of each control |
| -- | -- | -- |
| Header | object | Gets or sets the data used for the header of each control |
| HeaderTemplate | DataTemplate | Gets or sets the template used to display the content of the control's header |

### Examples

- The `Header` property can be set to a string, or any xaml elements. If binding the `Header` to an object that is not a string, use the `HeaderTemplate` to control how the content is rendered.

    *Sample Code*
    
    ```xaml
    <controls:HeaderedContentControl Header="This is the header!"/>
    
    <controls:HeaderedContentControl>
        <controls:HeaderedContentControl.Header>
            <Border Background="Gray">
                <TextBlock Text="This is the header!" FontSize="16">
            </Border>
        </controls:HeaderedContentControl.Header>
    </<controls:HeaderedContentControl>
    ```

- Used to control the look of the header. The default value for the `HeaderTemplate` will display the string representation of the `Header`. Set this property if you need to bind the `Header` to an object.

    ```xaml
    <controls:HeaderedContentControl Header="{Binding CustomObject}">
        <controls:HeaderedContentControl.HeaderTemplate>
            <DataTemplate>
                <TextBlock Text="{Binding Title}">
            </DataTemplate>
        </controls:HeaderedContentControl.HeaderTemplate>
    </<controls:HeaderedContentControl>
    ```

## Sample Code
[HeaderedContentControl Sample Page Source](https://github.com/Microsoft/UWPCommunityToolkit/tree/master/Microsoft.Toolkit.Uwp.SampleApp/SamplePages/HeaderedContentControl). You can see this in action in [Windows Community Toolkit Sample App](https://www.microsoft.com/store/apps/9NBLGGH4TLCQ).

## Default Template

[HeaderedContentControl XAML File](https://github.com/Microsoft/UWPCommunityToolkit/blob/master/Microsoft.Toolkit.Uwp.UI.Controls/HeaderedContentControl/HeaderedContentControl.xaml) is the XAML template used in the toolkit for the default styling.

## Requirements

| Device family | Universal, 10.0.14393.0 or higher |
| -- | -- |
| Namespace | Microsoft.Toolkit.Uwp.UI.Controls |
| NuGet package | [Microsoft.Toolkit.Uwp.UI.Controls](https://www.nuget.org/packages/Microsoft.Toolkit.Uwp.UI.Controls/) |

## API

* [HeaderedContentControl source code](https://github.com/Microsoft/UWPCommunityToolkit/tree/master/Microsoft.Toolkit.Uwp.UI.Controls/HeaderedContentControl)
