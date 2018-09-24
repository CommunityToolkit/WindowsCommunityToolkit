---
title: How to - Group, Sort and Filter data in the DataGrid Control
author: harinik
description: Guidance document that shows how to group, sort and filter data in the DataGrid control
keywords: windows 10, uwp, windows community toolkit, windows toolkit, DataGrid, xaml control, xaml, group, sort, filter
---

# How to: Group, sort and filter data in the DataGrid Control

It is often useful to view data in a DataGrid in different ways by grouping, sorting, and filtering the data. To group, sort, and filter the data in a DataGrid, you bind it to a [CollectionViewSource](https://docs.microsoft.com/en-us/uwp/api/windows.ui.xaml.data.collectionviewsource). You can then manipulate the data in the backing data source using LINQ queries without affecting the underlying data. The changes in the collection view are reflected in the DataGrid user interface (UI).

The following walk-throughs demonstrate how to implement grouping, sorting and filtering for the DataGrid control through examples.

See [DataGrid Sample](https://github.com/Microsoft/WindowsCommunityToolkit/tree/master/Microsoft.Toolkit.Uwp.SampleApp/SamplePages/DataGrid) for the complete sample code and running app.

## 1. Grouping

The DataGrid control has built-in row group header visuals for one-level grouping. You can set the DataGrid.ItemsSource to a grouped collection through [CollectionViewSource](https://docs.microsoft.com/en-us/uwp/api/windows.ui.xaml.data.collectionviewsource) with **IsSourceGrouped** property set to True and the DataGrid will automatically show the contents grouped under row group headers based on the data source.

The following walk-through shows how to implement and customize grouping in the DataGrid control.

1. Add the DataGrid control to your XAML page 

```xml
<controls:DataGrid 
   x:Name="dg"
   Height="600" Margin="12"
   AutoGenerateColumns="True">

</controls:DataGrid>
```

2. Create the grouped collection using LINQ

```C#
// Create grouping for collection
ObservableCollection<GroupInfoCollection<Mountain>> mountains = new ObservableCollection<GroupInfoCollection<Mountain>>();

//Implement grouping through LINQ queries
var query = from item in _items 
            group item by item.Range into g
            select new { GroupName = g.Key, Items = g };

//Populate Mountains grouped collection with results of the query
foreach (var g in query)
{
   GroupInfoCollection<Mountain> info = new GroupInfoCollection<Mountain>();
   info.Key = g.GroupName;
   foreach (var item in g.Items)
   {
      info.Add(item);
   }
   mountains.Add(info);
}
```

3. Populate a CollectionViewSource instance with the grouped collection and set IsSourceGrouped property to True.

```C#
//Create the CollectionViewSource  and set to grouped collection
CollectionViewSource groupedItems = new CollectionViewSource();
groupedItems.IsSourceGrouped = true;
groupedItems.Source = mountains;
```

4. Set the ItemsSource of the DataGrid control

```C#
//Set the datagrid's ItemsSource to grouped collection view source
dg.ItemsSource = groupedItems.View;
```

5. Customize the Group header values through **RowGroupHeaderStyles**, **RowGroupHeaderPropertyNameAlternative** properties and by handling the **LoadingRowGroup** event to alter the auto-generated values in code.

```xml
<controls:DataGrid 
   x:Name="dg"
   Height="600" Margin="12"
   AutoGenerateColumns="True"
   LoadingRowGroup="dg_loadingRowGroup" 
   RowGroupHeaderPropertyNameAlternative="Range">
   <controls:DataGrid.RowGroupHeaderStyles>
      <!-- Override the default Style for groups headers -->
      <Style TargetType="controls:DataGridRowGroupHeader">
         <Setter Property="Background" Value="LightGray" />
      </Style>
   </controls:DataGrid.RowGroupHeaderStyles>
</controls:DataGrid>
```

```C#
//Handle the LoadingRowGroup event to alter the grouped header property value to be displayed
private void dg_loadingRowGroup(object sender, DataGridRowGroupHeaderEventArgs e)
{
   ICollectionViewGroup group = e.RowGroupHeader.CollectionViewGroup;
   Mountain item = group.GroupItems[0] as Mountain;
   e.RowGroupHeader.PropertyValue = item.Range;
}
```

![Group](../../resources/images/Controls/DataGrid/grouping.png)

## 2. Sorting

Users can sort columns in the DataGrid control by tapping on the desired column headers. To implement sorting, the DataGrid control exposes the following mechanisms:
* You can indicate columns are sortable in 2 ways. **CanUserSortColumns** property on DataGrid can be set to True to indicate all columns in the DataGrid control are sortable by the end user. Alternatively, you can also set **CanUserSort** property on individual DataGridColumns to control which columns are sortable by the end user. The default values for both properties is *True*. If both properties are set, any value of False will take precedence over a value of True.
* You can indicate the sort direction of a column by setting **DataGridColumn.SortDirection?** property. The **DataGridSortDirection** enumeration allows the values of *Ascending* and *Descending*. The default value for SortDirection property is *null* (unsorted). 
* If DataGridColumn.SortDirection property is set to *Ascending*, an ascending sort icon (upward facing arrow) will be shown to the right of the column header indicating that the specific column has been sorted in the ascending order. The reverse is true for *Descending*. When the value is *null*, no icon will be shown.
*Note: the DataGrid control does not automatically perform a sort when this property is set. The property is merely a tool for showing the correct built-in icon visual.*
* If *DataGrid.CanUserSortColumns* and *DataGridColumn.CanUserSort* properties are both true, **DataGrid.Sorting** event will be fired when the end user taps on the column header. Sorting functionality can be implemented by handling this event.
* You can use the **DataGridColumn.Tag** property to keep track of the bound column header during sort.

The following walk-through shows how to implement sorting in the DataGrid control.

1. Add the DataGrid control to your XAML page. Set the appropriate sort properties and add a handler for Sorting event.

```xml
<controls:DataGrid 
   x:Name="dg"
   Height="600" Margin="12"
   AutoGenerateColumns="False"
   CanUserSortColumns="True"
   Sorting="dg_Sorting">
       <controls:DataGrid.Columns>
           <controls:DataGridTextColumn Header="Range" Binding="{Binding Range}" Tag="Range"/>
           <!-- Add more columns -->
       </controls:DataGrid.Columns>
</controls:DataGrid>
```

2. Handle the Sorting event to implement logic for sorting 

```C#
private void dg_Sorting(object sender, DataGridColumnEventArgs e)
{
    if (e.Column.SortDirection == null || e.Column.SortDirection == DataGridSortDirection.Ascending)
    {
        //Use the Tag property to pass the bound column name for the sorting implementation 
        if (e.Column.Tag.ToString() == "Range")
        {
            //Implement ascending sort on the column "Range" using LINQ
            dg.ItemsSource = new ObservableCollection<Mountain>(from item in _items
                                                                orderby item.Range ascending
                                                                select item);
        }       
        // ... 
```

3. Set the SortDirection property to the approrpriate value for showing the built-in ascending sort icon in column header

```C#
//Show the ascending icon when acending sort is done
e.Column.SortDirection = DataGridSortDirection.Ascending;

//Show the descending icon when descending sort is done
e.Column.SortDirection = DataGridSortDirection.Ascending;

//Clear the SortDirection in a previously sorted column when a different column is sorted
previousSortedColumn.SortDirection = null;

```
![Sort](../../resources/images/Controls/DataGrid/sorting.png)

## 3. Filtering

The DataGrid control does not support any built-in filtering capabilities. The following walk-through shows how you can implement your own filtering visuals and apply it to the DataGrid's content.

1. Add the DataGrid control to your XAML page. 

```xml
<controls:DataGrid 
   x:Name="dg"
   Height="600" Margin="12"
   AutoGenerateColumns="True"/>
```

2. Add buttons for filtering the DataGrid's content. It is recommended to use the **CommandBar** control with **AppBarButtons** to add filtering visuals at the top of your page. The following example shows a CommandBar with the title for the table and one filter button.

```xml
<CommandBar Background="White" Margin="12" 
            IsOpen="True" IsSticky="True" 
            DefaultLabelPosition="Right"
            Content="Mountains table">
      <AppBarButton Icon="Filter" 
                    Label="Filter by Rank &lt; 50"
                    Click="rankLowFilter_Click">
</CommandBar>
```

3. Handle the AppBarButton's Click event to implement the filtering logic.

```C#
private void rankLowFilter_Click(object sender, RoutedEventArgs e)
{
    dg.ItemsSource = new ObservableCollection<Mountain>(from item in _items
                                                        where item.Rank < 50
                                                        select item);
}
```

## Example app

For the complete example of all the capabilities in this section in a running application, see [DataGrid Sample](https://github.com/Microsoft/WindowsCommunityToolkit/tree/master/Microsoft.Toolkit.Uwp.SampleApp/SamplePages/DataGrid).

## See Also

* [Add a DataGrid control to a page](datagrid_basics.md)
* [Configure Auto-generated columns in the DataGrid control](customize_autogenerated_columns.md)
* [Editing and input validation in the DataGrid control](editing_inputvalidation.md)
