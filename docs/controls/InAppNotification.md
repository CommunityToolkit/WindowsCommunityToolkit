# In App Notification 

The *In App Notification* control offers the ability to show local notifications in your application.

## Syntax

The control should be placed where you want your notification to be displayed in the page, generally in the root grid.

```xml

<controls:InAppNotification
    x:Name="ExampleInAppNotification" />

```

### Show notification

You have multiple options to show an in-app notification.

1. By simply displaying the notification using the current template

```c#
await ExampleInAppNotification.ShowAsync();
```

2. By using a simple text content.

```c#
await ExampleInAppNotification.ShowAsync("Some text.");
```

3. By using a UIElement (with a container as parent, ex: Grid)

```c#
var grid = new Grid();

// TODO : Construct the Grid in C#

await ExampleInAppNotification.ShowAsync(grid);
```

4. By using a DataTemplate

```c#
object inAppNotificationWithButtonsTemplate;
bool isTemplatePresent = Resources.TryGetValue("InAppNotificationWithButtonsTemplate", out inAppNotificationWithButtonsTemplate);

if (isTemplatePresent && inAppNotificationWithButtonsTemplate is DataTemplate)
{
    await ExampleInAppNotification.ShowAsync(inAppNotificationWithButtonsTemplate as DataTemplate);
}
```

### Notification duration

By passing a second argument to the `ShowAsync()` method, you can set the duration of the notification (in milliseconds).

```c#
await ExampleInAppNotification.ShowAsync("Some text.", 2000); // the notification will appear for 2 seconds
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

## Example Code

[InAppNotification Sample Page](../../Microsoft.Toolkit.Uwp.SampleApp/SamplePages/InAppNotification)

## Default Template 

The default template is based on Microsoft Edge in-app notification template. You can override it for your own needs.

[InAppNotification XAML File](../..//Microsoft.Toolkit.Uwp.UI.Controls/InAppNotification/InAppNotification.xaml) is the XAML template used in the toolkit for the default styling.

## Requirements (Windows 10 Device Family)

| [Device family](http://go.microsoft.com/fwlink/p/?LinkID=526370) | Universal, 10.0.10586.0 or higher |
| --- | --- |
| Namespace | Microsoft.Toolkit.Uwp.UI.Controls |

## API

* [InAppNotification source code](../..//Microsoft.Toolkit.Uwp.UI.Controls/InAppNotification)

