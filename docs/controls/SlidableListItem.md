---
title: SlidableListItem XAML Control
author: nmetulev
description: The SlidableListItem Control is a UI control that enables actions to be triggered by sliding the content left or right.
keywords: windows 10, uwp, windows community toolkit, uwp community toolkit, uwp toolkit, SlidableListItem, XAML Control, xaml
dev_langs:
  - csharp
  - vb
---

# SlidableListItem XAML Control

> [!WARNING]
The SlidableListItem is deprecated and will be removed in a future major release. Please use the [SwipeControl](https://docs.microsoft.com/en-us/windows/uwp/controls-and-patterns/swipe) available in the Fall Creators Update. Read the [Moving to SwipeControl](#swipe) section for more info.

The [SlidableListItem Control](https://docs.microsoft.com/dotnet/api/microsoft.toolkit.uwp.ui.controls.slidablelistitem) is a UI control that enables actions to be triggered by sliding the content left or right. This effect can be forced to ignore the mouse if only touch screen interaction is desired.

This control can be used as a ListView Data Template root to create effects similar to those common in mobile email apps like Outlook.

## Syntax

```xaml
<controls:SlidableListItem LeftIcon="Favorite" RightIcon="Delete" LeftLabel="Set Favorite" 
	RightLabel="Delete" LeftBackground="Green" RightBackground="Red" LeftForeground="White" 
	RightForeground="Black" ActivationWidth="100" MouseSlidingEnabled="True"
	LeftCommand="ToggleFavorite" RightCommandRequested="SlidableListItem_RightCommandActivated">
	
	<StackPanel Column="1" Margin="10">
		<CheckBox IsChecked="False"></CheckBox>
		<TextBlock Text="My Great Text" TextWrapping="NoWrap"/>            
	</StackPanel>
</controls:SlidableListItem> 
```

## Sample Output

![SlidableListItem animation](../resources/images/Controls/SlidableListItem.gif)

## Properties

| Property | Type | Description |
| -- | -- | -- |
| ActivationWidth | double | Gets or sets the amount of pixels the content needs to be swiped for an action to be requested |
| ExtraSwipeThreshold | int | Gets or sets the amount of extra pixels for swipe threshold when `IsOffsetLimited` is enabled |
| IsLeftCommandEnabled | bool | Gets or sets a value indicating whether left command is enabled or not |
| IsOffsetLimited | bool | Gets or sets a value indicating whether maximum swipe offset is limited or not |
| IsPointerReleasedOnSwipingHandled | bool | Gets or sets a value indicating whether the PointerReleased event is handled when swiping. Set this to true to prevent ItemClicked or selection to occur when swiping in a [ListView](https://docs.microsoft.com/uwp/api/Windows.UI.Xaml.Controls.ListView) |
| IsRightCommandEnabled | bool | Gets or sets a value indicating whether right command is enabled or not |
| LeftBackground | Brush | Gets or sets the left background color |
| LeftCommand | ICommand | Gets or sets the ICommand for left command request |
| LeftCommandParameter | object | Gets or sets the CommandParameter for left command request |
| LeftForeground | Brush | Gets or sets the left foreground color |
| LeftIcon | Symbol | Gets or sets the left icon symbol |
| LeftLabel | string | Gets or sets the left label |
| MouseSlidingEnabled | bool | Gets or sets a value indicating whether it has the ability to slide the control with the mouse. False by default |
| RightBackground | Brush | Gets or sets the right background color |
| RightCommand | ICommand | Gets or sets the ICommand for right command request |
| RightCommandParameter | object | Gets or sets the CommandParameter for right command request |
| RightForeground | Brush | Gets or sets the right foreground color |
| RightIcon | Symbol | Gets or sets the right icon symbol |
| RightLabel | string | Gets or sets the right label |
| SwipeStatus | SwipeStatus | Gets the SwipeStatus for current swipe status |
| UseSwipeControlWhenPossible | bool | Set `true` to use a template based on the SwipeControl when running on the Fall Creators Update and above, and the regular template otherwise. |

## Events

| Events | Description |
| -- | -- |
| LeftCommandRequested | Occurs when the user swipes to the right to activate the left action |
| RightCommandRequested | Occurs when the user swipes to the left to activate the right action |
| SwipeStatusChanged | Occurs when SwipeStatus has changed |

## Examples

The `LeftCommand` and the `LeftCommandRequested` event is executed when the control has been swiped to the right, and `RightCommand` and the `RightCommandRequested` event is executed when the control has been swiped to the left. If you need more detailed control you can subscribe to the `SwipeStatusChanged` event. This is triggered when swiping starts, stops, swiping below and above `ActivationWidth` and some other cases. The following code shows how to detect some important events:

```csharp
private void SlidableListItem_SwipeStatusChanged(SlidableListItem sender, SwipeStatusChangedEventArgs args)
{
    if (args.NewValue == SwipeStatus.Starting)
    {
        // Swiping starting
    }
    else if (args.NewValue == SwipeStatus.Idle)
    {
        if (args.OldValue == SwipeStatus.SwipingPassedLeftThreshold)
        {
            // Swiping to the left completed
        }
        else if (args.OldValue == SwipeStatus.SwipingPassedRightThreshold)
        {
            // Swiping to the right completed
        }
        else
        {
            // Swiping cancelled
        }
    }
}
```
```vb
Private Sub SlidableListItem_SwipeStatusChanged(ByVal sender As SlidableListItem, ByVal args As SwipeStatusChangedEventArgs)
    If args.NewValue = SwipeStatus.Starting Then
        ' Swiping starting
    ElseIf args.NewValue = SwipeStatus.Idle Then
        If args.OldValue = SwipeStatus.SwipingPassedLeftThreshold Then
            ' Swiping to the left completed
        ElseIf args.OldValue = SwipeStatus.SwipingPassedRightThreshold Then
            ' Swiping to the right completed
        Else
            ' Swiping cancelled
        End If
    End If
End Sub
```

If you use `SlidableListItem` in a `ListView` with the `ItemClick` event, you need to be aware the `ItemClick` event is triggered by default when the control has been swiped. If you don't want this behavior you can set `IsPointerReleasedOnSwipingHandled` to `true` to suppress the `ItemClick` event. If you need more control you can instead check the `SwipeStatus` property in the `ItemClick` event. The following code shows how to do that:

```csharp
private void ListView_ItemClick(object sender, ItemClickEventArgs e)
{
    var listView = sender as ListView;
    var listViewItem = listView.ContainerFromItem(e.ClickedItem) as ListViewItem;
    var slidableListItem = listViewItem.ContentTemplateRoot as SlidableListItem;

    // Don't do anything unless the SwipeStatus is Idle.
    if (slidableListItem.SwipeStatus != SwipeStatus.Idle)
        return;

    ...
}
```
```vb
Private Sub ListView_ItemClick(ByVal sender As Object, ByVal e As ItemClickEventArgs)
    Dim listView = TryCast(sender, ListView)
    Dim listViewItem = TryCast(listView.ContainerFromItem(e.ClickedItem), ListViewItem)
    Dim slidableListItem = TryCast(listViewItem.ContentTemplateRoot, SlidableListItem)

    ' Don't do anything unless the SwipeStatus is Idle.
    If slidableListItem.SwipeStatus <> SwipeStatus.Idle Then Return

    ...
End Sub
```

## <a name="swipe"></a> Moving to SwipeControl
The Windows 10 Fall Creators Update SDK now includes the [SwipeControl](https://docs.microsoft.com/en-us/windows/uwp/controls-and-patterns/swipe) control among other new controls and APIs. This is great news for the Windows Community Toolkit as it means that one of its most popular controls has a comparable counterpart in the Windows SDK and it is very easy to transition to the SwipeControl if you are already using the SlidableListItem.

The SlidableListItem and SwipeControl share the same concepts and provide the same functionality. In fact, the SwipeControl adds even more functionality and can be used in even more scenarios.

### What developers need to know to move to the SwipeControl?

* **Two different modes:** The SwipeControl has two different modes of commanding:
    * Execute mode - works the same way as the commanding on the SlidableListItem, where the user executes a command with a single swipe
    * Reveal mode - the user swipes an item to open a menu where the commands can be executed by tapping them
* **Swipe direction:** SlidableListItem only supports left and right swiping while the SwipeControl supports all four directions (Up, Down, Left, Right)
* **SwipeItem:** The Fall Creators Update defines new objects to help define the swipe commands. Unlike the SlidableListItem where each command is defined through properties on the control itself, the SwipeControl accepts a collection of SwipeItems that define the commands. This is where you can specify properties such as background, foreground, icon, label, and invoked events.

### Making the transition easier
Starting with v2.1 of the Windows Community Toolkit, the SwipeControl provides a new property called `UseSwipeControlWhenPossible`. Setting the value to true will force the SlidableListItem to use a template based on the SwipeControl when running on the Fall Creators Update and above, and the regular template otherwise.

Using this property will enable you to take advantage of the SwipeControl on devices that supported it, while providing an experience based on SlidableListItem on devices that have not yet updated to the Fall Creators Update. Make sure to test the experience on multiple OS releases and plan to fully transition to the SwipeControl as the SlidableListItem will be removed from the Windows Community Toolkit in a future major release.

There are several SlidableListItem properties that have no effect when the SlidableListItem is using the SwipeControl:

* ActivationWidth
* IsOffsetLimited
* IsPointerReleasedOnSwipingHandled
* MouseSlidingEnabled

## Sample Code

[SlidableListItem Sample Page Source](https://github.com/Microsoft/WindowsCommunityToolkit//tree/master/Microsoft.Toolkit.Uwp.SampleApp/SamplePages/SlidableListItem). You can see this in action in [Windows Community Toolkit Sample App](https://www.microsoft.com/store/apps/9NBLGGH4TLCQ).

## Default Template 

[SlidableListItem XAML File](https://github.com/Microsoft/WindowsCommunityToolkit//blob/master/Microsoft.Toolkit.Uwp.UI.Controls/SlidableListItem/SlidableListItem.xaml) is the XAML template used in the toolkit for the default styling.

## Requirements

| Device family | Universal, 10.0.15063.0 or higher |
| -- | -- |
| Namespace | Microsoft.Toolkit.Uwp.UI.Controls |
| NuGet package | [Microsoft.Toolkit.Uwp.UI.Controls](https://www.nuget.org/packages/Microsoft.Toolkit.Uwp.UI.Controls/) |

## API

* [SlidableListItem source code](https://github.com/Microsoft/WindowsCommunityToolkit//tree/master/Microsoft.Toolkit.Uwp.UI.Controls/SlidableListItem)
