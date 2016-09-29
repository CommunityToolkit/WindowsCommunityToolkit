# Blade XAML Control 

The BladeControl provides a container to host blades as extra detail pages in, for example, a master-detail scenario. The control is based on how the Azure Portal works. 

## Syntax

```xaml

<controls:BladeControl>
    <controls:BladeControl.Blades>
        <controls:Blade IsOpen="True"
                        TitleBarVisibility="Collapsed">
            <controls:Blade.Element>
                <StackPanel Margin="8">
                        <Button Width="180"
                            Height="100"
                            Margin="0, 20, 0, 0"
                            controls:BladeControl.ToggleBlade="Blade1"
                            Content="Default blade" />
                </StackPanel>
            </controls:Blade.Element>
        </controls:Blade>

        <controls:Blade Title="A blade"
                        BladeID="Blade1"
                        IsOpen="False">
            <controls:Blade.Element>
                <TextBlock HorizontalAlignment="Center"
                           VerticalAlignment="Center"
                           Style="{StaticResource SubtitleTextBlockStyle}"
                           Text="This is a blade with all settings set to default." />
            </controls:Blade.Element>
        </controls:Blade>
    </controls:BladeControl.Blades>
</controls:BladeControl>

```

## Example Image

![BladeControl animation](../resources/images/Controls-BladeControl.gif "BladeControl")

## Example Code

[BladeControl Sample Page](https://github.com/Microsoft/UWPCommunityToolkit/tree/master/Microsoft.Toolkit.Uwp.SampleApp/SamplePages/BladeControl)

## Default Template 

[BladeControl XAML File](https://github.com/Microsoft/UWPCommunityToolkit/blob/master/Microsoft.Toolkit.Uwp.UI.Controls/BladeControl/BladeControl.xaml) is the XAML template used in the toolkit for the default styling.

## Requirements (Windows 10 Device Family)

| [Device family]("http://go.microsoft.com/fwlink/p/?LinkID=526370) | Universal, 10.0.10586.0 or higher |
| --- | --- |
| Namespace | Microsoft.Toolkit.Uwp.UI.Controls |

## API

* [BladeControl source code](https://github.com/Microsoft/UWPCommunityToolkit/tree/master/Microsoft.Toolkit.Uwp.UI.Controls/BladeControl)

