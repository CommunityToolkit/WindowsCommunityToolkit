---
title: ViewExtensions
author: nmetulev
description: The ApplicationViewExtensions, StatusBarExtensions & TitleBarExtensions provide a declarative way of setting AppView, StatusBar & TitleBar properties from XAML.
keywords: windows 10, uwp, windows community toolkit, uwp community toolkit, uwp toolkit, ViewExtensions, ApplicationViewExtensions, StatusBarExtensions, TitleBarExtensions, statusbar, titlebar, xaml
---

# ViewExtensions

The [ApplicationViewExtensions](https://docs.microsoft.com/dotnet/api/microsoft.toolkit.uwp.ui.extensions.applicationview), [StatusBarExtensions](https://docs.microsoft.com/dotnet/api/microsoft.toolkit.uwp.ui.extensions.statusbar) & [TitleBarExtensions](https://docs.microsoft.com/dotnet/api/microsoft.toolkit.uwp.ui.extensions.titlebar) provide a declarative way of setting AppView, StatusBar & TitleBar properties from XAML.

## Example

```xaml
<Page x:Class="Microsoft.Toolkit.Uwp.SampleApp.SamplePages.ViewExtensionsPage"
      xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
      xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
      xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
      xmlns:local="using:Microsoft.Toolkit.Uwp.SampleApp.SamplePages"
      xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
      xmlns:extensions="using:Microsoft.Toolkit.Uwp.UI.Extensions"
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

## Properties

### ApplicationViewExtensions Properties

| Property | Type | Description |
| -- | -- | -- |
| Title | string | Set application title |

### StatusBarExtensions Properties

| Property | Type | Description |
| -- | -- | -- |
| BackgroundColor | Color | Set background color of status bar |
| BackgroundOpacity | double | Set background color opacity of status bar |
| ForegroundColor | Color | Set foreground color of status bar |
| IsVisible | bool | Set visibility of status bar |

### TitleBarExtensions Properties

| Property | Type | Description |
| -- | -- | -- |
| BackgroundColor | Color | Set background color of title bar |
| ForegroundColor | Color | Set foreground color of title bar |

## Requirements

| Device family | Universal, 10.0.15063.0 or higher |
| --- | --- |
| Namespace | Microsoft.Toolkit.Uwp.UI.Extensions |
| NuGet package | [Microsoft.Toolkit.Uwp.UI](https://www.nuget.org/packages/Microsoft.Toolkit.Uwp.UI/) |

## API

* [ApplicationViewExtensions source code](https://github.com/Microsoft/WindowsCommunityToolkit//blob/master/Microsoft.Toolkit.Uwp.UI/Extensions/ApplicationView/ApplicationViewExtensions.cs)
* [StatusBarExtensions source code](https://github.com/Microsoft/WindowsCommunityToolkit//blob/master/Microsoft.Toolkit.Uwp.UI/Extensions/StatusBar/StatusBarExtensions.cs)
* [TitleBarExtensions source code](https://github.com/Microsoft/WindowsCommunityToolkit//blob/master/Microsoft.Toolkit.Uwp.UI/Extensions/TitleBar/TitleBarExtensions.cs)

