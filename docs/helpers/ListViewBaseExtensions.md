# ListViewBaseExtensions class

ListViewBaseExtensions provides extension method that allow attaching ICommand to handle ListViewBase Item interaction by means of [ItemClick](https://msdn.microsoft.com/en-us/library/windows/apps/windows.ui.xaml.controls.listviewbase.itemclick.aspx) event. 
ListViewBase [IsItemClickEnabled](https://msdn.microsoft.com/en-us/library/windows/apps/windows.ui.xaml.controls.listviewbase.isitemclickenabled.aspx) must be set to **true**



## Example

```xml
    // Attach the command declared in MainViewModel to ListView declared in XAML
    // IsItemClickEnabled is set to true as shown below
    <ListView
        ui:ListViewBaseExtensions.Command="{x:Bind MainViewModel.ItemSelectedCommand, Mode=OneWay}"
        IsItemClickEnabled="True"
        ItemsSource="{x:Bind MainViewModel.Items, Mode=OneWay}"
        SelectionMode="None" />
```

<br/>
The AlternateRowColor property provides a way to assign a background color to every other row.

## Example

```xml
    <ListView
        ui:ListViewBaseExtensions.AlternateRowColor="Silver"
        ItemsSource="{x:Bind MainViewModel.Items, Mode=OneWay}" />
```

## Requirements (Windows 10 Device Family)

| [Device family](http://go.microsoft.com/fwlink/p/?LinkID=526370) | Universal, 10.0.10586.0 or higher |
| --- | --- |
| Namespace | Microsoft.Toolkit.Uwp.UI |

## API

* [ListViewBaseExtensions source code](https://github.com/Microsoft/UWPCommunityToolkit/blob/dev/Microsoft.Toolkit.Uwp.UI/Extensions/ListViewBaseExtensions.cs)

