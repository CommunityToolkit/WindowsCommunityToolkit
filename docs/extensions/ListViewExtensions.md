---
title: ListViewExtensions
author: nmetulev
description: ListViewExtensions extensions provide a lightweight way to extend every control that inherits the ListViewBase class with attached properties.
keywords: windows 10, uwp, windows community toolkit, uwp community toolkit, uwp toolkit, ListViewBase, extensions
---

# ListViewExtensions

[ListViewExtensions](https://docs.microsoft.com/dotnet/api/microsoft.toolkit.uwp.ui.extensions.listviewextensions) provide a lightweight way to extend every control that inherits the [ListViewBase](https://docs.microsoft.com/uwp/api/Windows.UI.Xaml.Controls.ListViewBase) class with attached properties.

## AlternateColor extentions

The AlternateColor property provides a way to assign a background color to every other item.

> [!WARNING]
The [ContainerContentChanging](https://docs.microsoft.com/uwp/api/windows.ui.xaml.controls.listviewbase#Windows_UI_Xaml_Controls_ListViewBase_ContainerContentChanging) event used for this extension to work, will not be raised when the ItemsPanel is replaced with another type of panel than ItemsStackPanel or ItemsWrapGrid. 

### Syntax

```xaml
<Page ...
     xmlns:extensions="using:Microsoft.Toolkit.Uwp.UI.Extensions">

<ListView
    extensions:ListViewExtensions.AlternateColor="Silver"
    ItemsSource="{x:Bind MainViewModel.Items, Mode=OneWay}" />
```

### Properties

| Property | Description |
| --| -- |
| AlternateColor | Attached `DependencyProperty` for binding a `Brush` as an alternate background color to a `ListViewBase` |

## AlternateItemTemplate extentions

The AlternateItemTemplate property provides a way to assign an alternate [datatemplate](https://docs.microsoft.com/uwp/api/windows.ui.xaml.datatemplate) to every other item. It is also possible to combine with the AlternateColor property.

> [!WARNING]
The [ContainerContentChanging](https://docs.microsoft.com/uwp/api/windows.ui.xaml.controls.listviewbase#Windows_UI_Xaml_Controls_ListViewBase_ContainerContentChanging) event used for this extension to work, will not be raised when the ItemsPanel is replaced with another type of panel than ItemsStackPanel or ItemsWrapGrid. 

### Syntax

```xaml
<Page ...
     xmlns:extensions="using:Microsoft.Toolkit.Uwp.UI.Extensions">

<Page.Resources>
    <DataTemplate x:Name="NormalTemplate">
        <TextBlock Text="{Binding " Foreground="Green"></TextBlock>
    </DataTemplate>
    
    <DataTemplate x:Name="AlternateTemplate">
        <TextBlock Text="{Binding}" Foreground="Orange"></TextBlock>
    </DataTemplate>
</Page.Resources>

<ListView
    ItemTemplate="{StaticResource NormalTemplate}"
    extensions:ListViewExtensions.AlternateItemTemplate="{StaticResource AlternateTemplate}"
    ItemsSource="{x:Bind MainViewModel.Items, Mode=OneWay}" />
```

### Properties

| Property | Description |
| --| -- |
| AlternateItemTemplate | Attached `DependencyProperty` for binding a `DataTemplate` as an alternate template to a `ListViewBase` |

## Command extentions

ListViewExtensions provides extension method that allow attaching [ICommand](https://docs.microsoft.com/uwp/api/Windows.UI.Xaml.Input.ICommand) to handle ListViewBase Item interaction by means of [ItemClick](https://docs.microsoft.com/uwp/api/windows.ui.xaml.controls.listviewbase#Windows_UI_Xaml_Controls_ListViewBase_ItemClick) event.

> [!IMPORTANT]
ListViewBase [IsItemClickEnabled](https://docs.microsoft.com/uwp/api/windows.ui.xaml.controls.listviewbase#Windows_UI_Xaml_Controls_ListViewBase_IsItemClickEnabled) must be set to `true`

### Syntax

```xaml
<Page ...
     xmlns:extensions="using:Microsoft.Toolkit.Uwp.UI.Extensions">
     
<ListView
    extensions:ListViewExtensions.Command="{x:Bind MainViewModel.ItemSelectedCommand, Mode=OneWay}"
    IsItemClickEnabled="True"
    ItemsSource="{x:Bind MainViewModel.Items, Mode=OneWay}"
    SelectionMode="None" />
```

### Properties

| Property | Description |
| --| -- |
| Command | Attached `DependencyProperty` for binding an `ICommand` instance to a `ListViewBase` |

## StretchItemContainerDirection extentions

The StretchItemContainerDirection property provides a way to stretch the ItemContainer in horizontal, vertical or both ways. Possible values for this property are *Horizontal*, *Vertical* and *Both*.

> [!WARNING]
The [ContainerContentChanging](https://docs.microsoft.com/uwp/api/windows.ui.xaml.controls.listviewbase#Windows_UI_Xaml_Controls_ListViewBase_ContainerContentChanging) event used for this extension to work, will not be raised when the ItemsPanel is replaced with another type of panel than `ItemsStackPanel` or `ItemsWrapGrid`.

### Syntax

```xaml
<Page ...
     xmlns:extensions="using:Microsoft.Toolkit.Uwp.UI.Extensions">

<ListView
    extensions:ListViewExtensions.StretchItemContainerDirection="Horizontal"
    ItemsSource="{x:Bind MainViewModel.Items, Mode=OneWay}" />
```

### Properties

| Property | Type | Description |
| --| -- | -- |
| StretchItemContainerDirection | [ListViewBase.StretchDirection](https://docs.microsoft.com/dotnet/api/microsoft.toolkit.uwp.ui.extensions.listviewbase.stretchdirection) | Attached `DependencyProperty` for setting the container content stretch direction on the `ListViewBase` |

## Sample Code

[ListViewExtensions](https://github.com/Microsoft/UWPCommunityToolkit/tree/master/Microsoft.Toolkit.Uwp.SampleApp/SamplePages/ListViewExtensions). You can see this in action in [Windows Community Toolkit Sample App](https://www.microsoft.com/store/apps/9NBLGGH4TLCQ).

## Requirements

| Device family | Universal, 10.0.14393.0 or higher |
| --- | --- |
| Namespace | Microsoft.Toolkit.Uwp.UI.Extensions |
| NuGet package | [Microsoft.Toolkit.Uwp.UI](https://www.nuget.org/packages/Microsoft.Toolkit.Uwp.UI/) |

## API

* [ListViewExtensions source code](https://github.com/Microsoft/UWPCommunityToolkit/blob/master/Microsoft.Toolkit.Uwp.UI/Extensions/ListViewBase)
