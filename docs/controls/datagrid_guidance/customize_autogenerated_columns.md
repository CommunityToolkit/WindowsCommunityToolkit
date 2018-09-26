---
title: How to - Customize Auto-Generated Columns in the DataGrid Control
author: harinik
description: Guidance document that shows how to customize auto generated columns in the DataGrid control
keywords: windows 10, uwp, windows community toolkit, windows toolkit, DataGrid, xaml control, xaml, AutoGenerateColumns
---

# How to: Customize Auto-Generated Columns in the DataGrid Control

The [DataGrid](../datagrid.md) control supports the option to automatically generate columns based on the collection data bound through the **ItemsSource** property. 

```xml
<controls:DataGrid AutoGenerateColumns="True"/>
<!-- Autogenerates column headers and columns based on the Data model provided -->
```

You can modify the **DataGrid.Columns** collection at run time regardless of whether it contains generated columns. However, if you specify columns in XAML, you should set AutoGenerateColumns to false in order to avoid duplication.

You can handle the DataGrid's **AutoGeneratingColumn** event to modify, replace, or cancel a column that is being generated. The AutoGeneratingColumn event occurs one time for each public, non-static property in the bound data type when the ItemsSource property is changed and the AutoGenerateColumns property is true.

## To handle the AutoGeneratingColumn event

1. Create an event handler for the DataGrid's **AutoGeneratingColumn** event.
```C#
private void dataGrid1_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
{
    //...
}
```

2. Add the event handler to the DataGrid instances events.
```xml
<controls:DataGrid x:Name="dataGrid1"
            AutoGenerateColumns="True"
            AutoGeneratingColumn="dataGrid1_AutoGeneratingColumn"/>
```

## To modify a generated column

In the AutoGeneratingColumn event handler, access the DataGridColumn properties by referencing the **DataGridAutoGeneratingColumnEventArgs.Column** property.
```C#
//Modify the header of the "Name" column
if (e.Column.Header.ToString() == "Name")
{
    e.Column.Header = "Task";
}
```

## To replace a generated column

1. In the AutoGeneratingColumn event handler, create a new **DataGridColumn**.
```C#
//Replace the DueDate column with a custom template column.
if (e.PropertyName = "DueDate")
{
    //Create a new template column.
    var templateColumn = new DataGridTemplateColumn();
    templateColumn.Header = "Due Date";
    //...
}
```

2. Replace the column from the **DataGridAutoGeneratingColumnEventArgs.Column** property with the new **DataGridColumn** instance.
```C#
//Replace the auto-generated column with the templateColumn.
e.Column = templateColumn;
```

## To cancel generation of a column

Set the **Cancel** property to true.
```C#
//Cancel AutoGeneration of all boolean columns.
if (e.PropertyType == GetType(Boolean))
{
    e.Cancel = True;
}
```

## See Also

* [Add a DataGrid control to a page](datagrid_basics.md)
* [Customize the DataGrid control using styling and formatting options](styling_formatting_options.md)
* [Sizing options in the DataGrid control](sizing_options.md)
* [DataGrid Sample Page Source](https://github.com/Microsoft/WindowsCommunityToolkit/tree/master/Microsoft.Toolkit.Uwp.SampleApp/SamplePages/DataGrid) 
