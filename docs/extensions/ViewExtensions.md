---
title: ViewExtensions
author: nmetulev
ms.date: 08/20/2017
description: The ApplicationViewExtensions, StatusBarExtensions & TitleBarExtensions provide a declarative way of setting AppView, StatusBar & TitleBar properties from XAML.
keywords: windows 10, uwp, uwp community toolkit, uwp toolkit, ViewExtensions, ApplicationViewExtensions, StatusBarExtensions, TitleBarExtensions, statusbar, titlebar, xaml
---

# ViewExtensions

The **ApplicationViewExtensions, StatusBarExtensions & TitleBarExtensions** provide a declarative way of setting AppView, StatusBar & TitleBar properties from XAML.

## Example

```xml
<Page x:Class="Microsoft.Toolkit.Uwp.SampleApp.SamplePages.ViewExtensionsPage"
      xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
      xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
      xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
      xmlns:local="using:Microsoft.Toolkit.Uwp.SampleApp.SamplePages"
      xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
      xmlns:viewHelper="using:Microsoft.Toolkit.Uwp.UI.Extensions"
      extensions:ApplicationViewExtensions.Title="View Extensions"
      extensions:StatusBarExtensions.BackgroundColor="Blue"
      extensions:StatusBarExtensions.BackgroundOpacity="0.8"
      extensions:StatusBarExtensions.ForegroundColor="White"
      extensions:StatusBarExtensions.IsVisible="True"
      extensions:TitleBarExtensions.BackgroundColor="Blue"
      extensions:TitleBarExtensions.ForegroundColor="White"
      mc:Ignorable="d">

    <Grid Background="{StaticResource Brush-Grey-05}" />
</Page>
```

## Requirements (Windows 10 Device Family)

| [Device family](http://go.microsoft.com/fwlink/p/?LinkID=526370) | Universal, 10.0.14393.0 or higher |
| --- | --- |
| Namespace | Microsoft.Toolkit.Uwp.UI.Extensions |

## API

* [ApplicationViewExtensions source code](https://github.com/Microsoft/UWPCommunityToolkit/blob/master/Microsoft.Toolkit.Uwp.UI/Extensions/ApplicationView.cs)
* [StatusBarExtensions source code](https://github.com/Microsoft/UWPCommunityToolkit/blob/master/Microsoft.Toolkit.Uwp.UI/Extensions/StatusBar.cs)
* [TitleBarExtensions source code](https://github.com/Microsoft/UWPCommunityToolkit/blob/master/Microsoft.Toolkit.Uwp.UI/Extensions/TitleBar.cs)

