---
title: Loading XAML Control 
author: nmetulev
ms.date: 08/20/2017
description: The loading control is for showing an animation with some content when the user should wait in some tasks of the app.
keywords: windows 10, uwp, uwp community toolkit, uwp toolkit, Loading, XAML Control , xaml
---

# Loading XAML Control 

The loading control is for showing an animation with some content when the user should wait in some tasks of the app.

## Syntax

An example of how we can build the loading control.

```xml
<controls:Loading x:Name="LoadingControl" IsLoading="{Binding IsBusy}">
    <ContentControl x:Name="LoadingContentControl"/>
</controls:Loading>
```
- **Background** and **Opacity** are for the panel who appears and disappears behind our custom control.
- Use the **LoadingControl** to show specialized content.
- You can also use **BorderBrush** and **BorderThickness** to change the **LoadingControl**.

```xml
<controls:Loading x:Name="LoadingControl" IsLoading="{Binding IsBusy}"  >
    <StackPanel Orientation="Horizontal" Padding="12">
        <Grid Margin="0,0,8,0">
            <Image Source="../../Assets/ToolkitLogo.png" Height="50" />
            <ProgressRing IsActive="True" Foreground="Blue" />
        </Grid>
        <TextBlock Text="It's ok, we are working on it :)" Foreground="Black" VerticalAlignment="Center" />
    </StackPanel>
</controls:Loading>
```

 Finally that the loading control appears, we must set the `IsLoading` property to `true`

`LoadingControl.IsLoading = true;`


## Example Image

![Loading animation](../resources/images/LoadingXamlControl.gif "Loading Xaml Control")

## Example Code

[Loading Sample Page](https://github.com/Microsoft/UWPCommunityToolkit/tree/master/Microsoft.Toolkit.Uwp.SampleApp/SamplePages/Loading)

## Default Template 

[Loading XAML File](https://github.com/Microsoft/UWPCommunityToolkit/blob/master/Microsoft.Toolkit.Uwp.UI.Controls/Loading/Loading.xaml) is the XAML template used in the toolkit for the default styling.

## Requirements (Windows 10 Device Family)

| [Device family](http://go.microsoft.com/fwlink/p/?LinkID=526370) | Universal, 10.0.14393.0 or higher |
| --- | --- |
| Namespace | Microsoft.Toolkit.Uwp.UI.Controls |

## API

* [Loading source code](https://github.com/Microsoft/UWPCommunityToolkit/tree/master/Microsoft.Toolkit.Uwp.UI.Controls/Loading)

