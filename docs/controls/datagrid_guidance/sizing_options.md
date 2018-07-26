---
title: How to - Use the different sizing options in the DataGrid control
author: harinik
description: Guidance document that shows how to size the rows, columns and headers of the DataGrid control
keywords: windows 10, uwp, windows community toolkit, windows toolkit, DataGrid, xaml control, xaml
---

# Sizing Options in the DataGrid Control

Various options are available to control how the [DataGrid](../datagrid.md) sizes itself. The DataGrid, and individual rows and columns in the DataGrid, can be set to size automatically to their contents or can be set to specific values. By default, the DataGrid will grow and shrink to fit the size of its contents.

## Sizing the DataGrid

### Cautions When Using Automatic Sizing
By default, the **Height** and **Width** properties of the [DataGrid](../datagrid.md) are set to *Double.NaN* ("Auto" in XAML), and the DataGrid will adjust to the size of its contents.

When placed inside a container that does not restrict the size of its children, such as a [StackPanel](https://docs.microsoft.com/en-us/uwp/api/windows.ui.xaml.controls.stackpanel), the DataGrid, like [ListView](https://docs.microsoft.com/en-us/uwp/api/windows.ui.xaml.controls.listview) and other scrollable controls, will stretch beyond the visible bounds of the container and scrollbars will not be shown. This condition has both usability and performance implications.

When bound to a data set, if the Height of the DataGrid is not restricted, it will continue to add a row for each data item in the bound data set. This can cause the DataGrid to grow outside the visible bounds of your application as rows are added. The DataGrid will not show scrollbars in this case because its Height will continue to grow to accommodate the new rows.

An object is created for each row in the DataGrid. If you are working with a large data set and you allow the DataGrid to automatically size itself, the creation of a large number of objects may affect the performance of your application.

To avoid these issues when you work with large data sets, it is recommended that you specifically set the Height of the DataGrid or place it in a container that will restrict its Height, such as a [Grid](https://docs.microsoft.com/en-us/uwp/api/windows.ui.xaml.controls.grid) or [RelativePanel](https://docs.microsoft.com/en-us/uwp/api/windows.ui.xaml.controls.relativepanel). When the Height is restricted, the DataGrid will only create the rows that will fit within its specified Height, and will recycle those rows as needed to display new data.

### Setting the DataGrid Size
The DataGrid can be set to automatically size within specified boundaries, or the DataGrid can be set to a specific size. The following table shows the properties that can be set to control the DataGrid size.

| Property | Description |
|---|---|
| Height | Sets a specific height for the DataGrid. |
| MaxHeight | Sets the upper bound for the height of the DataGrid. The DataGrid will grow vertically until it reaches this height. |
| MinHeight | Sets the lower bound for the height of the DataGrid. The DataGrid will shrink vertically until it reaches this height. |
| Width | Sets a specific width for the DataGrid. |
| MaxWidth | Sets the upper bound for the width of the DataGrid. The DataGrid will grow horizontally until it reaches this width.|
| MinWidth | Sets the lower bound for the width of the DataGrid. The DataGrid will shrink horizontally until it reaches this width. |

## Sizing Rows and Row Headers

### DataGrid Rows
By default, a DataGrid row's Height property is set to Double.NaN ("Auto" in XAML), and the row height will expand to the size of its contents. The height of all rows in the DataGrid can be specified by setting the **DataGrid.RowHeight** property. Users cannot change the row height by dragging the row header dividers.

### DataGrid Row Headers
By default, DataGrid row headers are not displayed. To display row headers, the **HeadersVisibility** property must be set to *DataGridHeadersVisibility.Row* or *DataGridHeadersVisibility.All* and the DataGrid's ControlTemplate should be altered to provide a visual for the RowHeader as desired. By default, when row headers are displayed, they automatically size to fit their content. The row headers can be given a specific width by setting the **DataGrid.RowHeaderWidth** property.

## Sizing Columns and Column Headers

### DataGrid Columns
The DataGrid uses values of the **DataGridLength** and the **DataGridLengthUnitType** structure to specify absolute or automatic sizing modes.
The following table shows the values provided by the DataGridLengthUnitType structure.

| Name | Description |
|---|---|
| Auto | The default automatic sizing mode sizes DataGrid columns based on the contents of both cells and column headers. | 
| SizeToCells | The cell-based automatic sizing mode sizes DataGrid columns based on the contents of cells in the column, not including column headers.| 
| SizeToHeader | The header-based automatic sizing mode sizes DataGrid columns based on the contents of column headers only.| 
| Pixel | The pixel-based sizing mode sizes DataGrid columns based on the numeric value provided.| 
| Star | The star sizing mode is used to distribute available space by weighted proportions. In XAML, star values are expressed as n* where n represents a numeric value. 1* is equivalent to *. For example, if two columns in a DataGrid had widths of * and 2*, the first column would receive one portion of the available space and the second column would receive two portions of the available space.

The **DataGridLengthConverter** class can be used to convert data between numeric or string values and DataGridLength values.

By default, the **DataGrid.ColumnWidth** property is set to Auto, and the **DataGridColumn.Width** property is null. When the sizing mode is set to Auto or SizeToCells, columns will grow to the width of their widest visible content. When scrolling, these sizing modes will cause columns to expand if content that is larger than the current column size is scrolled into view. The column will not shrink after the content is scrolled out of view.

Columns in the DataGrid can also be set to automatically size only within specified boundaries, or columns can be set to a specific size. The following table shows the properties that can be set to control column sizes.

| Property | Description | 
|---|---|
| DataGrid.MaxColumnWidth | Sets the upper bound for all columns in the DataGrid.| 
| DataGridColumn.MaxWidth | Sets the upper bound for an individual column. Overrides DataGrid.MaxColumnWidth.| 
| DataGrid.MinColumnWidth | Sets the lower bound for all columns in the DataGrid.| 
| DataGridColumn.MinWidth | Sets the lower bound for an individual column. Overrides DataGrid.MinColumnWidth.| 
| DataGrid.ColumnWidth | Sets a specific width for all columns in the DataGrid.| 
| DataGridColumn.Width | Sets a specific width for an individual column. Overrides DataGrid.ColumnWidth.| 

### DataGrid Column Headers
By default, DataGrid column headers are displayed. To hide column headers, the **HeadersVisibility** property must be set to *DataGridHeadersVisibility.Row* or *DataGridHeadersVisibility.None*. By default, when column headers are displayed, they automatically size to fit their content. The column headers can be given a specific height by setting the **DataGrid.ColumnHeaderHeight** property.

### Resizing columns by the end user
Users can resize DataGrid columns by dragging the column header dividers with mouse/touch/pen. The DataGrid does not support automatic resizing of columns by double-clicking the column header divider. To prevent a user from resizing particular columns, set the **DataGridColumn.CanUserResize** property to false for the individual columns. To prevent users from resizing all columns, set the **DataGrid.CanUserResizeColumns** property to false.

## See Also

* [Add a DataGrid control to a page](datagrid_basics.md)
* [Customize the DataGrid control using styling and formatting options](styling_formatting_options.md)
* [DataGrid Sample Page Source](https://github.com/Microsoft/WindowsCommunityToolkit//tree/harinikmsft/datagrid/Microsoft.Toolkit.Uwp.SampleApp/SamplePages/DataGrid)
