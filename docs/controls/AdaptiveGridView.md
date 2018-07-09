---
title: AdaptiveGridView XAML Control
author: nmetulev
description: The AdaptiveGridView Control presents items in a evenly-spaced set of columns to fill the total available display space.
keywords: windows 10, uwp, windows community toolkit, uwp community toolkit, uwp toolkit, AdaptiveGridView, xaml control, xaml
dev_langs:
  - csharp
  - vb
---

# AdaptiveGridView XAML Control 

The [AdaptiveGridView Control](https://docs.microsoft.com/dotnet/api/microsoft.toolkit.uwp.ui.controls.adaptivegridview) presents items in a evenly-spaced set of columns to fill the total available display space. It reacts to changes in the layout as well as the content so it can adapt to different form factors automatically.

There are 3 ways to use this Control:

1. You can set `DesiredWidth` and `ItemHeight`, which will scale the **width** of each item and adjust the number of columns on demand using all horizontal space.

2. You can set `DesiredWidth` only. This will mean that rows will take up as much space as required, using all horizontal space.

3. Using `OneRowModeEnabled`, you can set `DesiredWidth` and `ItemHeight` which will adjust the **width** of each item and the number of visible columns on demand, using all horizontal space.

## Syntax

```xaml
<Page ...
     xmlns:controls="using:Microsoft.Toolkit.Uwp.UI.Controls"/>

<controls:AdaptiveGridView  Name="AdaptiveGridViewControl"
    ItemHeight="200"
    DesiredWidth="300"
    ItemTemplate="{StaticResource PhotosTemplate}">
</controls:AdaptiveGridView>
```

## Sample Output

![AdaptiveGridView animation](../resources/images/Controls/AdaptiveGridView.gif)

## Properties

| Property | Type | Description |
| -- | -- | -- |
| DesiredWidth | double | Gets or sets the desired width of each item |
| ItemClickCommand | ICommand | Gets or sets the command to execute when an item is clicked and the IsItemClickEnabled property is true |
| ItemHeight | double | Gets or sets the height of each item in the grid |
| ItemsPanel | ItemsPanelTemplate | Gets the template that defines the panel that controls the layout of items |
| OneRowModeEnabled | Boolean | Gets or sets a value indicating whether only one row should be displayed |
| StretchContentForSingleRow | Boolean | Gets or sets a value indicating whether the control should stretch the content to fill at least one row |

> [!IMPORTANT]
ItemHeight property must be set when OneRowModeEnabled property set as `true`

## Examples

1. Using `DesiredWidth` in combination with `ItemHeight`:

    ![AdaptiveGridView DesiredHeight and ItemHeight](../resources/images/Controls/AdaptiveGridView/AdaptiveGridView-DesiredWidthItemHeight.gif)

2. Maintain aspect ratio by setting `DesiredWidth` with no `ItemHeight` set:

    ![AdaptiveGridView Viewbox scaled](../resources/images/Controls/AdaptiveGridView/AdaptiveGridView-ViewboxAspectRatio.gif)

    This still requires the `ItemTemplate` to contain some scaling logic, this can be done with Height and Width set on the content inside of a Viewbox, or using custom view logic.

    - Using a `Viewbox`:

        Using the `Height` and `Width` properties of content inside of a `Viewbox` means that the content inside will scale linearly when **Width** and **Height** changes, which causes a zoom-like effect. This might not be a desired effect, and it will also likely incur a slight performance penalty.

        _ItemTemplate implementation_

        ```xaml
        <DataTemplate x:Key="PhotosTemplate">
            <Viewbox>
                <Grid
                    Height="300"
                    Width="200"
                    Background="White"
                    BorderBrush="Black"
                    BorderThickness="1">
                    <Image
                        Source="{Binding Thumbnail}"
                        Stretch="UniformToFill"
                        HorizontalAlignment="Center"
                        VerticalAlignment="Center"/>
                </Grid>
            </Viewbox>
        </DataTemplate>
        ```

    - To use custom view logic:

        Using `MeasureOverride` on a ContentControl allows you to specify the **Width** and **Height** of the content, which might reap better performance compared to a `Viewbox`. The dimensions of the content space will change uniformly, but the content will not zoom.

        _Custom logic implementation_

        ```csharp
        public class AspectContentControl : ContentControl
        {
            protected override Size MeasureOverride(Size availableSize)
            {
                return new Size(availableSize.Width, availableSize.Width * 1.6);
            }
        }
        ```
        ```vb
        Public Class AspectContentControl
            Inherits ContentControl

            Protected Overrides Function MeasureOverride(ByVal availableSize As Size) As Size
                Return New Size(availableSize.Width, availableSize.Width * 1.6)
            End Function
        End Class
        ```

        _ItemTemplate implementation_

        ```xaml
        <DataTemplate x:Key="PhotosTemplate">
            <local:AspectContentControl 
                HorizontalContentAlignment="Stretch" 
                VerticalContentAlignment="Stretch">
                <Grid Background="White"
                    BorderBrush="Black"
                    BorderThickness="1">
                    <Image
                        Source="{Binding Thumbnail}"
                        Stretch="UniformToFill" />
                </Grid>
            </local:AspectContentControl>
        </DataTemplate>
        ```

3. Using `OneRowModeEnabled`:

    ![AdaptiveGridView OneRowMode](../resources/images/Controls/AdaptiveGridView/AdaptiveGridView-OneRowMode.gif)

    If there are not enough items to fill one row, the control will stretch the items until all available space is filled. This can result in much wider items than specified. If you prefer your items to always stay close to the DesiredWidth, you can set the **StretchContentForSingleRow** property to **false**, to prevent further stretching.

## Sample Code

[AdaptiveGridView Sample Page Source](https://github.com/Microsoft/WindowsCommunityToolkit//tree/master/Microsoft.Toolkit.Uwp.SampleApp/SamplePages/AdaptiveGridView). You can see this in action in [Windows Community Toolkit Sample App](https://www.microsoft.com/store/apps/9NBLGGH4TLCQ).

## Requirements

| Device family | Universal, 10.0.15063.0 or higher |
| -- | -- |
| Namespace | Microsoft.Toolkit.Uwp.UI.Controls |
| NuGet package | [Microsoft.Toolkit.Uwp.UI.Controls](https://www.nuget.org/packages/Microsoft.Toolkit.Uwp.UI.Controls/) |

## API

* [AdaptiveGridView source code](https://github.com/Microsoft/WindowsCommunityToolkit//tree/master/Microsoft.Toolkit.Uwp.UI.Controls/AdaptiveGridView)

## Related Topics

- [GridView Class](https://docs.microsoft.com/uwp/api/Windows.UI.Xaml.Controls.GridView)
