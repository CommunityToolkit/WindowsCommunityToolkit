---
title: DropShadowPanel XAML Control
author: nmetulev
description: The DropShadowPanel Control allows the creation of a drop shadow effect for any Xaml FrameworkElement in the markup.
keywords: windows 10, uwp, windows community toolkit, uwp community toolkit, uwp toolkit, DropShadowPanel, DropShadow, xaml Control, xaml
---

# DropShadowPanel XAML Control

The [DropShadowPanel Control](https://docs.microsoft.com/dotnet/api/microsoft.toolkit.uwp.ui.controls.dropshadowpanel) allows the creation of a drop shadow effect for any Xaml FrameworkElement in the markup.

## Syntax

```xaml
<Page ...
     xmlns:controls="using:Microsoft.Toolkit.Uwp.UI.Controls"/>

<controls:DropShadowPanel BlurRadius="4.0" ShadowOpacity="0.70"
                          OffsetX="5.0" OffsetY="5.0" Color="Black">
    <Image Width="200" Source="Unicorn.png" Stretch="Uniform"/>
</controls:DropShadowPanel>
```

## Sample Output

![DropShadowPanel animation](../resources/images/Controls/DropShadowPanel.png)

## Properties

| Property | Type | Description |
| -- | -- | -- |
| BlurRadius | double | Gets or sets the blur radius of the drop shadow |
| Color | Color | Gets or sets the color of the drop shadow |
| IsSupported | bool | Gets a value indicating whether the platform supports drop shadows |
| OffsetX | double | Gets or sets the x offset of the drop shadow |
| OffsetY | double | Gets or sets the y offset of the drop shadow |
| OffsetZ | double | Gets or sets the z offset of the drop shadow |
| ShadowOpacity | double | Gets or sets the opacity of the drop shadow |

## Sample Code

[DropShadowPanel Sample Page Source](https://github.com/Microsoft/UWPCommunityToolkit/tree/master/Microsoft.Toolkit.Uwp.SampleApp/SamplePages/DropShadowPanel). You can see this in action in [Windows Community Toolkit Sample App](https://www.microsoft.com/store/apps/9NBLGGH4TLCQ).

## Examples

- Use IsSupported property to verify the availability of drop shadow and take necessary action if not available

    ```csharp
    if(!DropShadowPanel.IsSupported)
    {
        // Change something to counter the lack of drop shadow
    }
    ```
    ```vb
    If Not DropShadowPanel.IsSupported Then
        ' Change something to counter the lack of drop shadow
    End If
    ```

## Default Template 

[DropShadowPanel XAML File](https://github.com/Microsoft/UWPCommunityToolkit/blob/master/Microsoft.Toolkit.Uwp.UI.Controls/DropShadowPanel/DropShadowPanel.xaml) is the XAML template used in the toolkit for the default styling.

## Requirements

| Device family | Universal, 10.0.14393.0 or higher |
| -- | -- |
| Namespace | Microsoft.Toolkit.Uwp.UI.Controls |
| NuGet package | [Microsoft.Toolkit.Uwp.UI.Controls](https://www.nuget.org/packages/Microsoft.Toolkit.Uwp.UI.Controls/) |

## API

* [DropShadowPanel source code](https://github.com/Microsoft/UWPCommunityToolkit/tree/master/Microsoft.Toolkit.Uwp.UI.Controls/DropShadowPanel)
