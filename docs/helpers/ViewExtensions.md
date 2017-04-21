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
      viewHelper:ApplicationViewExtensions.Title="View Extensions"
      viewHelper:StatusBarExtensions.BackgroundColor="Blue"
      viewHelper:StatusBarExtensions.BackgroundOpacity="0.8"
      viewHelper:StatusBarExtensions.ForegroundColor="White"
      viewHelper:StatusBarExtensions.IsVisible="True"
      viewHelper:TitleBarExtensions.BackgroundColor="Blue"
      viewHelper:TitleBarExtensions.ForegroundColor="White"
      mc:Ignorable="d">

    <Grid Background="{StaticResource Brush-Grey-05}" />
</Page>
```

## Requirements (Windows 10 Device Family)

| [Device family](http://go.microsoft.com/fwlink/p/?LinkID=526370) | Universal, 10.0.10586.0 or higher |
| --- | --- |
| Namespace | Microsoft.Toolkit.Uwp.UI |

## API

* [ApplicationViewExtensions source code](https://github.com/Microsoft/UWPCommunityToolkit/blob/dev/Microsoft.Toolkit.Uwp.UI/Extensions/ApplicationViewExtensions.cs)
* [StatusBarExtensions source code](https://github.com/Microsoft/UWPCommunityToolkit/blob/dev/Microsoft.Toolkit.Uwp.UI/Extensions/StatusBarExtensions.cs)
* [TitleBarExtensions source code](https://github.com/Microsoft/UWPCommunityToolkit/blob/dev/Microsoft.Toolkit.Uwp.UI/Extensions/TitleBarExtensions.cs)

