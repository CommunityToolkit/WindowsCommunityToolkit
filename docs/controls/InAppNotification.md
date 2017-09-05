---
title: InAppNotification XAML Control
author: nmetulev
ms.date: 08/20/2017
description: The InAppNotification control offers the ability to show local notifications in your application.
keywords: windows 10, uwp, uwp community toolkit, uwp toolkit, InAppNotification, in app notification, xaml control, xaml
---

# InAppNotification 

The *InAppNotification* control offers the ability to show local notifications in your application.

## Syntax

The control should be placed where you want your notification to be displayed in the page, generally in the root grid.

```xml

<controls:InAppNotification
    x:Name="ExampleInAppNotification" />

```

**Note:** Since the control is part of the page visual tree, it will render in the order it was added in the parent control, and might be hidden by other elements. For the control to render on top of other elements, add it as the last child of the parent control or set the Canvas.ZIndex to a high number.

### Show notification

You have multiple options to show an in-app notification.

1. By simply displaying the notification using the current template

```c#
ExampleInAppNotification.Show();
```

2. By using a simple text content.

```c#
ExampleInAppNotification.Show("Some text.");
```

3. By using a UIElement (with a container as parent, ex: Grid)

```c#
var grid = new Grid();

// TODO : Construct the Grid in C#

ExampleInAppNotification.Show(grid);
```

4. By using a DataTemplate

```c#
object inAppNotificationWithButtonsTemplate;
bool isTemplatePresent = Resources.TryGetValue("InAppNotificationWithButtonsTemplate", out inAppNotificationWithButtonsTemplate);

if (isTemplatePresent && inAppNotificationWithButtonsTemplate is DataTemplate)
{
    ExampleInAppNotification.Show(inAppNotificationWithButtonsTemplate as DataTemplate);
}
```

### Notification duration

By passing a second argument to the `Show()` method, you can set the duration of the notification (in milliseconds).

```c#
ExampleInAppNotification.Show("Some text.", 2000); // the notification will appear for 2 seconds
```

### Dismiss notification

```c#
ExampleInAppNotification.Dismiss();
```

## Example Image

![InAppNotification animation](../resources/images/Controls-InAppNotification.gif "InAppNotification")

## Properties

### ShowDismissButton

If you want to fully customize the in-app notification, you will see that the Dismiss button is still visible.
To hide it, simply set the property to `ShowDismissButton="False"`.

## Events

### Dismissed

This event is raised when the system or your user dismissed the notification.

```c#
private void InAppNotification_OnDismissed(object sender, EventArgs e)
{
    // TODO
}
```

## Animation

By default, the popup animation of the control is a bottom to top animation. You can update the popup animation using the `RenderTransformOrigin` property of the control. See examples:

```xml

<controls:InAppNotification RenderTransformOrigin="0.5,1" />

```

The default value (X: 0.5, Y:1) will show popup animation from bottom to top.

```xml

<controls:InAppNotification RenderTransformOrigin="0.5,0" />

```

An alternate version of the popup animation (used by the vscode-like notification) will start from top to bottom.

## Example Code

[InAppNotification Sample Page](https://github.com/Microsoft/UWPCommunityToolkit/tree/master/Microsoft.Toolkit.Uwp.SampleApp/SamplePages/InAppNotification)

## Default Template 

[InAppNotification XAML File](https://github.com/Microsoft/UWPCommunityToolkit/blob/master/Microsoft.Toolkit.Uwp.UI.Controls/InAppNotification/InAppNotification.xaml) is the XAML template used in the toolkit for the default styling.

## Requirements (Windows 10 Device Family)

| [Device family](http://go.microsoft.com/fwlink/p/?LinkID=526370) | Universal, 10.0.14393.0 or higher |
| --- | --- |
| Namespace | Microsoft.Toolkit.Uwp.UI.Controls |

## API

* [InAppNotification source code](https://github.com/Microsoft/UWPCommunityToolkit/tree/master/Microsoft.Toolkit.Uwp.UI.Controls/InAppNotification)

