# Blade XAML Control 

The BladeView provides a container to host blades as extra detail pages in, for example, a master-detail scenario. The control is based on how the Azure Portal works. 

## Syntax

```xml

<controls:BladeView>
    <controls:BladeItem IsOpen="True"
                        TitleBarVisibility="Collapsed">
        <StackPanel Margin="8">
            <ToggleButton Width="180"
                          Height="100"
                          Margin="0, 20, 0, 0"
                          IsChecked="{Binding IsOpen, Mode=TwoWay, ElementName=DefaultBlade}"
                          Content="Default blade" />
        </StackPanel>
    </controls:BladeItem>

    <controls:BladeItem x:Name="DefaultBlade" 
	                    Title="A blade"
                        IsOpen="False">
        <TextBlock HorizontalAlignment="Center"
                   VerticalAlignment="Center"
                   Style="{StaticResource SubtitleTextBlockStyle}"
                   Text="This is a blade with all settings set to default." />
    </controls:BladeItem>
</controls:BladeView>

```

## Blade modes

You can customize your BladeView control by setting the `BladeMode` property.
If you want blade items to stay unchanged (based on their respective width and height), you will choose the default mode (BladeMode.Normal).
Otherwise, you can extend each blade items to fill the entire container (example: Grid, StackPanel, etc..). To do that, you'll have to choose the Fullscreen mode (BladeMode.Fullscreen).

```csharp

public enum BladeMode
{
    /// <summary>
    /// Default mode : each blade will take the specified Width and Height
    /// </summary>
    Normal,

    /// <summary>
    /// Fullscreen mode : each blade will take the entire Width and Height of the UI control container (cf <see cref="BladeView"/>)
    /// </summary>
    Fullscreen
}

```

Here is an example of a BladeView where the `BladeMode` property is binded to a value in the code-behind.

```xml

<controls:BladeView x:Name="BladeView"
                    Padding="0"
                    HorizontalAlignment="Stretch"
                    VerticalAlignment="Stretch"
                    BladeMode="{Binding BladeMode}">
</controls:BladeView>

```

## Example Image

![BladeView animation](../resources/images/Controls-BladeView.gif "BladeView")

## Example Code

[BladeView Sample Page](https://github.com/Microsoft/UWPCommunityToolkit/tree/master/Microsoft.Toolkit.Uwp.SampleApp/SamplePages/BladeView)

## Default Template 

[BladeView XAML File](https://github.com/Microsoft/UWPCommunityToolkit/blob/master/Microsoft.Toolkit.Uwp.UI.Controls/BladeView/BladeView.xaml) is the XAML template used in the toolkit for the default styling.

## Requirements (Windows 10 Device Family)

| [Device family]("http://go.microsoft.com/fwlink/p/?LinkID=526370) | Universal, 10.0.10586.0 or higher |
| --- | --- |
| Namespace | Microsoft.Toolkit.Uwp.UI.Controls |

## API

* [BladeView source code](https://github.com/Microsoft/UWPCommunityToolkit/tree/master/Microsoft.Toolkit.Uwp.UI.Controls/BladeView)

