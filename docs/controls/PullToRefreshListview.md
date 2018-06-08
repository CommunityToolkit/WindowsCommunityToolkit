---
title: PullToRefreshListView XAML Control
author: nmetulev
description: The PullToRefreshListView Control lets the user pull down beyond the top limit on the listview to trigger a refresh of the content.
keywords: windows 10, uwp, windows community toolkit, uwp community toolkit, uwp toolkit, PullToRefreshListView, XAML Control, xaml
---

# PullToRefreshListView XAML Control

> [!NOTE]
The PullToRefreshListView is deprecated and will be removed in a future major release. Please use the [RefreshContainer](https://docs.microsoft.com/en-us/windows/uwp/design/controls-and-patterns/pull-to-refresh) available in the 1803 version of Windows. Read the [Moving to RefreshContainer](#refreshcontainer) section for more info.

The [PullToRefreshListView Control](https://docs.microsoft.com/dotnet/api/microsoft.toolkit.uwp.ui.controls.pulltorefreshlistview), is derived from the built-in List View in XAML. It lets the user pull down beyond the top limit on the listview to trigger a refresh of the content. This control can create rich, animations, and is easy to use. 

This control is very common on mobile devices, where the user can pull from the top to force a content refresh in applications like Twitter.

This control uses the *PullToRefreshLabel* and *ReleaseToRefreshLabel* properties to provide a visual indication to the user.

If you want more than a text to display, you can then use *PullToRefreshContent* and *ReleaseToRefreshContent*. In this case the *PullToRefreshLabel* and *ReleaseToRefreshLabel* properties will be ignored.

The `RefreshIndicatorContent` can be used with the `PullProgressChanged` event to provide a custom visual for the user.

To cancel a refresh request just slide back to a position prior to the *PullThreshold* position. Upon release the *RefreshIntentCanceled* event will
be raised and the *RefreshIntentCanceledCommand*, if any, will be executed.

## Syntax

```xaml
<Page ...
     xmlns:controls="using:Microsoft.Toolkit.Uwp.UI.Controls"/>

<controls:PullToRefreshListView Name="PullToRefreshListViewControl"
    ItemsSource="{x:Bind _items}"
    OverscrollLimit="0.4"
    PullThreshold="100"
    RefreshRequested="ListView_RefreshCommand"
    RefreshIntentCanceled="ListView_RefreshIntentCanceled"
    RefreshIntentCanceledCommand="{x:Bind RefreshIntentCanceled}"
    PullProgressChanged="ListView_PullProgressChanged">
    <controls:PullToRefreshListView.RefreshIndicatorContent>
        <!-- RefreshIndicator Content -->
    </controls:PullToRefreshListView.RefreshIndicatorContent>
</controls:PullToRefreshListView>
```

## Sample Output

![PullToRefreshListView animation](../resources/images/Controls/PullToRefreshListView.gif)

## Properties

| Property | Type | Description |
| -- | -- | -- |
| IsPullToRefreshWithMouseEnabled | bool | Gets or sets a value indicating whether PullToRefresh is enabled with a mouse |
| OverscrollLimit | double | Gets or sets the Overscroll Limit. Value between 0 and 1 where 1 is the height of the control. Default is 0.3 |
| PullThreshold | double | Gets or sets the PullThreshold in pixels for when Refresh should be Requested. Default is 100 |
| PullToRefreshContent | object | Gets or sets the content that will be shown when the user pulls down to refresh |
| PullToRefreshLabel | string | Gets or sets the label that will be shown when the user pulls down to refresh. Note: This label will only show up if `RefreshIndicatorContent` is null |
| RefreshCommand | ICommand | Gets or sets the Command that will be invoked when Refresh is requested |
| RefreshIndicatorContent | object | Gets or sets the Content of the Refresh Indicator |
| RefreshIntentCanceledCommand | ICommand | Gets or sets the Command that will be invoked when a refresh intent is cancled |
| ReleaseToRefreshContent | object | Gets or sets the content that will be shown when the user needs to release to refresh |
| ReleaseToRefreshLabel | string | Gets or sets the label that will be shown when the user needs to release to refresh. Note: This label will only show up if `RefreshIndicatorContent` is null |

## Events

| Events | Description |
| -- | -- |
| PullProgressChanged | Occurs when listview overscroll distance is changed |
| RefreshIntentCanceled | Occurs when the user has cancels an intent for the content to be refreshed |
| RefreshRequested | Occurs when the user has requested content to be refreshed |

## Sample Code

[PullToRefreshListView Sample Page Source](https://github.com/Microsoft/WindowsCommunityToolkit//tree/master/Microsoft.Toolkit.Uwp.SampleApp/SamplePages/PullToRefreshListView). You can see this in action in [Windows Community Toolkit Sample App](https://www.microsoft.com/store/apps/9NBLGGH4TLCQ).

## <a name="refreshcontainer"></a> Moving to RefreshContainer
The 1803 version of Windows now includes its own implementation of [pull-to-refresh](https://docs.microsoft.com/en-us/windows/uwp/design/controls-and-patterns/pull-to-refresh) controls, having [RefreshContainer](https://docs.microsoft.com/en-us/uwp/api/windows.ui.xaml.controls.refreshcontainer) as the main control.

The PullToRefreshListView and the RefreshContainer share the same concepts and provide mostly the same functionality, with the caveat that the RefreshContainer works only with a touch interface.

### What developers need to know to move to RefreshContainer?

* **XAML:** The RefreshContainer is very simple to use. Unlike the PullToRefreshListView, the RefreshContainer isn't based on the ListView control, so to use it you just need to add the XAML element as a parent of the element you'll use as your item container, like a ListView or a ScrollViewer.

* **Code behind:** The RefreshContainer invokes the RefreshRequested event whenever a refresh is triggered. Unlike most event handlers, it has the peculiarity of coming with a [Deferral](https://docs.microsoft.com/en-us/uwp/api/windows.foundation.deferral) object, that you can get from the [RefreshRequestedEventArgs](https://docs.microsoft.com/en-us/uwp/api/windows.ui.xaml.controls.refreshrequestedeventargs) by calling [GetDeferral](https://docs.microsoft.com/en-us/uwp/api/windows.ui.xaml.controls.refreshrequestedeventargs.getdeferral#Windows_UI_Xaml_Controls_RefreshRequestedEventArgs_GetDeferral).
To notify that your refresh code has completed, you can mark the deferral as completed by calling its Complete method or wrap your refresh code with a using statement of the deferral.

> [!NOTE]
Being a touch-only control, it's recommended that you also have a refresh button for users without a touch interface. You can trigger the RefreshRequested event by calling the RefreshContainer's RequestRefresh method.

### Making the transition even easier
Starting with v3.0 of the Windows Community Toolkit, the PullToRefreshListView provides a new property called **UseRefreshContainerWhenPossible**. Setting the value to true will force the PullToRefreshListView to use a template based on the RefreshContainer when running on the 1803 version of Windows and above, and the regular template otherwise.

Using this property will enable you to take advantage of the RefreshContainer on devices that support it, while providing an experience based on PullToRefreshListView on devices that have not yet updated to the 1803 version of Windows. Make sure to test the experience on multiple OS releases and plan to fully transition to the RefreshContainer as the PullToRefreshListView will be removed from the Windows Community Toolkit in a future major release.

> [!NOTE]
When using the RefreshContainer, the RefreshIntentCanceled and the PullProgressChanged events are not invoked. In addition, the RefreshIntentCanceledCommand is not executed.

There are several PullToRefreshListView properties that have no effect when the PullToRefreshListView is using the RefreshContainer:

* OverscrollLimit
* PullThreshold
* RefreshIndicatorContent
* PullToRefreshLabel
* ReleaseToRefreshLabel
* PullToRefreshContent
* ReleaseToRefreshContent
* IsPullToRefreshWithMouseEnabled


## Default Template

[PullToRefreshListView XAML File](https://github.com/Microsoft/WindowsCommunityToolkit//blob/master/Microsoft.Toolkit.Uwp.UI.Controls/PullToRefreshListView/PullToRefreshListView.xaml) is the XAML template used in the toolkit for the default styling.

## Requirements

| Device family | Universal, 10.0.15063.0 or higher |
| -- | -- |
| Namespace | Microsoft.Toolkit.Uwp.UI.Controls |
| NuGet package | [Microsoft.Toolkit.Uwp.UI.Controls](https://www.nuget.org/packages/Microsoft.Toolkit.Uwp.UI.Controls/) |

## API

* [PullToRefreshListView source code](https://github.com/Microsoft/WindowsCommunityToolkit//tree/master/Microsoft.Toolkit.Uwp.UI.Controls/PullToRefreshListView)
