# Expander Control

The **Expander Control** provides an expandable container to host any content.
You can show or hide this content by toggling a Header.

You can use these properties :

* Header
* HeaderTemplate
* IsExpanded (define if the content is visible or not)

You can also use these events :

* Expanded (fires when the expander is opened)
* Collapsed (fires when the expander is closed)

## Syntax

```xml

<controls:Expander Header="Header of the expander"
                   Foreground="White"
                   Background="Gray"
                   IsExpanded="True">
	<Grid Height="250">
        <TextBlock HorizontalAlignment="Center"
                   TextWrapping="Wrap"
                   Text="This is the content"
                   VerticalAlignment="Center" />
    </Grid>
</controls:Expander>       

```

## Example Image

![Expander animation](../resources/images/Controls-Expander.gif "Expander")

## Example Code

[Expander Sample Page](https://github.com/Microsoft/UWPCommunityToolkit/tree/master/Microsoft.Toolkit.Uwp.SampleApp/SamplePages/Expander)

## Default Template 

[Expander XAML File](https://github.com/Microsoft/UWPCommunityToolkit/blob/master/Microsoft.Toolkit.Uwp.UI.Controls/Expander/Expander.xaml) is the XAML template used in the toolkit for the default styling.

## Requirements (Windows 10 Device Family)

| [Device family]("http://go.microsoft.com/fwlink/p/?LinkID=526370) | Universal, 10.0.10586.0 or higher |
| --- | --- |
| Namespace | Microsoft.Toolkit.Uwp.UI.Controls |

## API

* [Expander source code](https://github.com/Microsoft/UWPCommunityToolkit/tree/master/Microsoft.Toolkit.Uwp.UI.Controls/Expander)