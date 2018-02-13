---
title: ListViewExtensions
author: nmetulev
description: ListViewExtensions extensions provide a lightweight way to extend every control that inherits the ListViewBase class with attached properties.
keywords: windows 10, uwp, uwp community toolkit, uwp toolkit, ListViewBase, extensions
---

# ListViewExtensions

ListViewExtensions extensions provide a lightweight way to extend every control that inherits the [ListViewBase](https://docs.microsoft.com/uwp/api/windows.ui.xaml.controls.listviewbase) class with attached properties.

##### AlternateColor

The AlternateColor property provides a way to assign a background color to every other item.

> [!WARNING] The [ContainerContentChanging](https://docs.microsoft.com/uwp/api/windows.ui.xaml.controls.listviewbase#Windows_UI_Xaml_Controls_ListViewBase_ContainerContentChanging) event used for this extension to work, will not be raised when the ItemsPanel is replaced with another type of panel than ItemsStackPanel or ItemsWrapGrid. 

## Example

```xaml
<ListView
    extensions:ListViewExtensions.AlternateColor="Silver"
    ItemsSource="{x:Bind MainViewModel.Items, Mode=OneWay}" />
```

##### AlternateItemTemplate

The AlternateItemTemplate property provides a way to assign an alternate [datatemplate](https://docs.microsoft.com/uwp/api/windows.ui.xaml.datatemplate) to every other item. It is also possible to combine with the AlternateColor property.

> [!WARNING] The [ContainerContentChanging](https://docs.microsoft.com/uwp/api/windows.ui.xaml.controls.listviewbase#Windows_UI_Xaml_Controls_ListViewBase_ContainerContentChanging) event used for this extension to work, will not be raised when the ItemsPanel is replaced with another type of panel than ItemsStackPanel or ItemsWrapGrid. 

## Example

```xaml
<Page.Resources>
    <DataTemplate x:Name="NormalTemplate">
        <TextBlock Text="{Binding }" Foreground="Green"></TextBlock>
    </DataTemplate>

    <DataTemplate x:Name="AlternateTemplate">
        <TextBlock Text="{Binding }" Foreground="Orange"></TextBlock>
    </DataTemplate>
</Page.Resources>

<ListView
    ItemTemplate="{StaticResource NormalTemplate}"
    extensions:ListViewExtensions.AlternateItemTemplate="{StaticResource AlternateTemplate}"
    ItemsSource="{x:Bind MainViewModel.Items, Mode=OneWay}" />
```

##### StretchItemContainerDirection

The StretchItemContainerDirection property provides a way to stretch the ItemContainer in horizontal, vertical or both ways. Possible values for this property are **Horizontal**, **Vertical** and **Both**.

> [!WARNING] The [ContainerContentChanging](https://docs.microsoft.com/uwp/api/windows.ui.xaml.controls.listviewbase#Windows_UI_Xaml_Controls_ListViewBase_ContainerContentChanging) event used for this extension to work, will not be raised when the ItemsPanel is replaced with another type of panel than ItemsStackPanel or ItemsWrapGrid. 

## Example

```xaml
<ListView
    extensions:ListViewExtensions.StretchItemContainerDirection="Horizontal"
    ItemsSource="{x:Bind MainViewModel.Items, Mode=OneWay}" />
```

## Requirements (Windows 10 Device Family)

| [Device family](http://go.microsoft.com/fwlink/p/?LinkID=526370) | Universal, 10.0.14393.0 or higher |
| --- | --- |
| Namespace | Microsoft.Toolkit.Uwp.UI.Extensions |
| NuGet package | [Microsoft.Toolkit.Uwp.UI](https://www.nuget.org/packages/Microsoft.Toolkit.Uwp.UI/) |

## API

* [ListViewExtensions source code](https://github.com/Microsoft/UWPCommunityToolkit/blob/master/Microsoft.Toolkit.Uwp.UI/Extensions/ListViewExtensions)

