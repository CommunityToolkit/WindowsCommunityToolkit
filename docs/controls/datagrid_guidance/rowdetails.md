---
title: How to - Display and Configure Row Details in the DataGrid Control
author: harinik
description: Guidance document that shows how to customize row details section in the DataGrid control
keywords: windows 10, uwp, windows community toolkit, windows toolkit, DataGrid, xaml control, xaml, RowDetails
---

# How to: Display and Configure Row Details in the DataGrid Control

Each row in the [DataGrid](../datagrid.md) control can be expanded to display a row details section. The row details section is defined by a [DataTemplate](https://docs.microsoft.com/en-us/uwp/api/windows.ui.xaml.datatemplate) that specifies the appearance of the section and the data to be displayed.

![RowDetails](../../resources/images/Controls/DataGrid/rowdetails.png)

The row details section can be displayed for selected rows, displayed for all rows, or it can be collapsed. The row details section can also be frozen so that it does not scroll horizontally when the DataGrid is scrolled.

## To display a row details section using inline XAML

1. Create a [DataTemplate](https://docs.microsoft.com/en-us/uwp/api/windows.ui.xaml.datatemplate) that defines the appearance of the row details section.
2. Place the DataTemplate inside the <DataGrid.RowDetailsTemplate> tags.

```xml
<controls:DataGrid>
   <controls:DataGrid.RowDetailsTemplate>
      <DataTemplate>
         <StackPanel Margin="20,10" Padding="5" Spacing="3">
            <TextBlock Margin="20" Text="Here are the details for the selected mountain:"/>   
            <TextBlock Text="{x:Bind Coordinates}"/>
            <TextBlock FontSize="13" Text="{x:Bind Prominence}"/>
            <TextBlock FontSize="13" Text="{x:Bind First_ascent}" />
            <TextBlock FontSize="13" Text="{x:Bind Ascents}" />
         </StackPanel>
      </DataTemplate>
   </controls:DataGrid.RowDetailsTemplate>
</controls:DataGrid>
```

## To display a row details section using a DataTemplate resource

1. Create a [DataTemplate](https://docs.microsoft.com/en-us/uwp/api/windows.ui.xaml.datatemplate) that defines the appearance of the row details section.
2. Identify the DataTemplate by assigning a value to the [x:Key Attribute](https://docs.microsoft.com/en-us/windows/uwp/xaml-platform/x-key-attribute).
3. Bind the DataTemplate to the DataGrid's **RowDetailsTemplate** property.

```xml
<Page>
   <Page.Resources>
      <DataTemplate x:Key="RowDetailsTemplate">
         <!-- Specify the template -->
      </DataTemplate>
   </Page.Resources>

   <controls:DataGrid
      RowDetailsTemplate="{StaticResource RowDetailsTemplate}" />

</Page>
```

## To change the visibility of a row details section

Set the **RowDetailsVisibilityMode** property to a value of the **DataGridRowDetailsVisibilityMode** enumeration:
   * *Collapsed* : The row details section is not displayed for any rows.
   * *Visible* : The row details section is displayed for all rows.
   * *VisibleWhenSelected* : The row details section is displayed only for selected rows.

The following example shows how to use the RowDetailsVisibilityMode property to change the row details display mode programmatically from the selection of a value in a ComboBox: 

```C#
// Set the row details visibility to the option selected in the combo box.
private void cbRowDetailsVis_SelectionChanged(object sender, RoutedEventArgs e)
{
    ComboBox cb = sender as ComboBox;
    ComboBoxItem cbi = cb.SelectedItem as ComboBoxItem;
    if (this.dataGrid1 != null)
    {
        if (cbi.Content.ToString() == "Selected Row (Default)")
            dataGrid1.RowDetailsVisibilityMode = DataGridRowDetailsVisibilityMode.VisibleWhenSelected;
        else if (cbi.Content.ToString() == "None")
            dataGrid1.RowDetailsVisibilityMode = DataGridRowDetailsVisibilityMode.Collapsed;
        else if (cbi.Content.ToString() == "All")
            dataGrid1.RowDetailsVisibilityMode = DataGridRowDetailsVisibilityMode.Visible;
    }
}
```
## To prevent a row details section from scrolling horizontally

Set the **AreRowDetailsFrozen** property to true.
```xml
   <controls:DataGrid
      AreRowDetailsFrozen="True" />
```

## See Also

* [Add a DataGrid control to a page](datagrid_basics.md)
* [Customize the DataGrid control using styling and formatting options](styling_formatting_options.md)
* [Sizing options in the DataGrid control](sizing_options.md)
* [DataGrid Sample](https://github.com/Microsoft/WindowsCommunityToolkit/tree/master/Microsoft.Toolkit.Uwp.SampleApp/SamplePages/DataGrid). 
