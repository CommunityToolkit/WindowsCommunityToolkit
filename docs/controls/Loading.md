---
title: Loading XAML Control 
author: nmetulev
description: The loading control is for showing an animation with some content when the user should wait in some tasks of the app.
keywords: windows 10, uwp, windows community toolkit, uwp community toolkit, uwp toolkit, Loading, XAML Control , xaml
---

# Loading XAML Control 

The [loading control](https://docs.microsoft.com/dotnet/api/microsoft.toolkit.uwp.ui.controls.loading) is for showing an animation with some content when the user should wait in some tasks of the app.

## Syntax

```xaml
<Page ...
     xmlns:controls="using:Microsoft.Toolkit.Uwp.UI.Controls"/>

<controls:Loading x:Name="LoadingControl" IsLoading="{Binding IsBusy}">
    <!-- Loading screen content -->
</controls:Loading>
```

## Sample Output

![Loading animation](../resources/images/Controls/LoadingXamlControl.gif)

## Properties

| Property | Type | Description |
| -- | -- | -- |
| IsLoading | bool | Gets or sets a value indicating whether the control is in the loading state |

## Examples

An example of how we can build the loading control.

- `Background` and `Opacity` are for the panel who appears and disappears behind our custom control.
- Use the `LoadingControl` to show specialized content.
- You can also use `BorderBrush` and `BorderThickness` to change the `LoadingControl`.

```xaml
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

```csharp
LoadingControl.IsLoading = true;
```
```vb
LoadingControl.IsLoading = true
```

## Sample Code

[Loading Sample Page Source](https://github.com/Microsoft/UWPCommunityToolkit/tree/master/Microsoft.Toolkit.Uwp.SampleApp/SamplePages/Loading). You can see this in action in [Windows Community Toolkit Sample App](https://www.microsoft.com/store/apps/9NBLGGH4TLCQ).

## Default Template 

[Loading XAML File](https://github.com/Microsoft/UWPCommunityToolkit/blob/master/Microsoft.Toolkit.Uwp.UI.Controls/Loading/Loading.xaml) is the XAML template used in the toolkit for the default styling.

## Requirements

| Device family | Universal, 10.0.14393.0 or higher |
| -- | -- |
| Namespace | Microsoft.Toolkit.Uwp.UI.Controls |
| NuGet package | [Microsoft.Toolkit.Uwp.UI.Controls](https://www.nuget.org/packages/Microsoft.Toolkit.Uwp.UI.Controls/) |

## API

* [Loading source code](https://github.com/Microsoft/UWPCommunityToolkit/tree/master/Microsoft.Toolkit.Uwp.UI.Controls/Loading)
