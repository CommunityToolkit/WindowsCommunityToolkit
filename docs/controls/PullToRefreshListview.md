# PullToRefreshListView XAML Control

The **PullToRefreshListView Control**, is derived from the built-in List View in XAML. It lets the user pull down beyond the top limit on the listview to trigger a refresh of the content. This control can create rich, animations, and is easy to use. 

This control is very common on mobile devices, where the user can pull from the top to force a content refresh in applications like Twitter.

By default, the control uses the properties *PullToRefreshLabel* and *ReleaseToRefreshLabel* to provide a visual indication to the user. The *RefreshIndicatorContent* can be used with the *PullProgressChanged* event to provide a custom visual for the user.  

## Syntax

```xaml

<controls:PullToRefreshListView Name="PullToRefreshListViewControl"
	ItemsSource="{x:Bind _items}"	
	OverscrollLimit="0.4"
	PullThreshold="100"
	RefreshRequested="ListView_RefreshCommand" 
	PullProgressChanged="ListView_PullProgressChanged">
</controls:PullToRefreshListView.ItemTemplate>

```

## Example Image

![PullToRefreshListView animation](../resources/images/Controls-PullToRefreshListView.gif "PullToRefreshListView")

## Example Code

[PullToRefreshListView Sample Page](https://github.com/Microsoft/UWPCommunityToolkit/tree/master/Microsoft.Toolkit.Uwp.SampleApp/SamplePages/PullToRefreshListView)

## Default Template 

[PullToRefreshListView XAML File](https://github.com/Microsoft/UWPCommunityToolkit/blob/master/Microsoft.Toolkit.Uwp.UI.Controls/PullToRefreshListView/PullToRefreshListView.xaml) is the XAML template used in the toolkit for the default styling.

## Requirements (Windows 10 Device Family)

| [Device family](http://go.microsoft.com/fwlink/p/?LinkID=526370) | Universal, 10.0.10586.0 or higher |
| --- | --- |
| Namespace | Microsoft.Toolkit.Uwp.UI.Controls |

## API

* [PullToRefreshListView source code](https://github.com/Microsoft/UWPCommunityToolkit/tree/master/Microsoft.Toolkit.Uwp.UI.Controls/PullToRefreshListView)
