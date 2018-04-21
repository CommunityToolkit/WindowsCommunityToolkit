---
title: Print Helper
author: nmetulev
description: The PrintHelper is a UWP Community Toolkit helper class that enables the rendering of a framework element per page for printing purposes
keywords: windows 10, uwp, uwp community toolkit, uwp toolkit, PrintHelper
---

# Print Helper

The PrintHelper is a class used to simplify document printing.
It allows you to render a framework element per page.

To use it, you only have to instantiate a `PrintHelper` object and call `AddFrameworkElementToPrint` method to add the XAML controls you want to print.
Please note that controls cannot be linked to a visual tree. This means that their parent property has to be null.
If you want to use a control from your current XAML page, you can disconnect it before sending it to print (by removing it from its container) or you can create just create a new one from scratch.

Please check the sample app code to see how to disconnect/reconnect a control that you want to print:
https://github.com/Microsoft/UWPCommunityToolkit/blob/master/Microsoft.Toolkit.Uwp.SampleApp/SamplePages/PrintHelper/PrintHelperPage.xaml.cs

Several events are available to control the printing process:

* OnPrintFailed will be triggered if the user cancels the print or if something goes wrong
* OnPrintSucceeded will be triggered after a successful print
* OnPreviewPagesCreated will be triggered after print preview pages are generated. This allows you to control the look and feel of your page before they are sent to the spooler.

In addition, you can customize the printing dialog using the `PrintHelperOptions` class. To use it, create an instance of the class, add the options you'd like to display on the printing dialog and set the default options. Then, you can use it as a parameter in the `PrintHelper` class constructor to set them as the default for the instance, or send them as parameters to `ShowPrintUIAsync` to use them for a single print job.

> [!NOTE] Page breaks are not supported. Every control will be printed on a single page.

*****

> [!IMPORTANT] Printing in Dark Theme will likely print white text, which won't be visible. To fix this, ensure the text is a visible color before printing, then restore the original color.

Since version 1.3, you can also call `ShowPrintUIAsync` with a second parameter to determine that the list of controls to print should directly be taken from the content of the container passed to the PrintHelper constructor.
In this mode you are responsible for the sizing and the layout.

## Example

```csharp
// Create a new PrintHelper instance
// "container" is a XAML panel that will be used to host printable control.
// It needs to be in your visual tree but can be hidden with Opacity = 0
var printHelper = new PrintHelper(container);

// Add controls that you want to print
printHelper.AddFrameworkElementToPrint(await PrepareWebViewForPrintingAsync());

// Connect to relevant events
printHelper.OnPrintFailed += PrintHelper_OnPrintFailed;
printHelper.OnPrintSucceeded += PrintHelper_OnPrintSucceeded;

// Start printing process
await printHelper.ShowPrintUIAsync("UWP Community Toolkit Sample App");

// Event handlers

private async void PrintHelper_OnPrintSucceeded()
{
  printHelper.Dispose();
  var dialog = new MessageDialog("Printing done.");
  await dialog.ShowAsync();
}

private async void PrintHelper_OnPrintFailed()
{
  printHelper.Dispose();
  var dialog = new MessageDialog("Printing failed.");
  await dialog.ShowAsync();
}
```

Direct print example:

```csharp
// Create a new PrintHelper instance
// "container" is a XAML panel that will be used to get the list of printable controls.
var printHelper = new PrintHelper(container);

// Start printing process
await printHelper.ShowPrintUIAsync("UWP Community Toolkit Sample App", true);
```

Using custom default settings:

```csharp
// Create a new PrintHelperOptions instance
var defaultPrintHelperOptions = new PrintHelperOptions();

//Add options that you want to be displayed on the print dialog
defaultPrintHelperOptions.AddDisplayOption(StandardPrintTaskOptions.Orientation);

//Set preselected settings
defaultPrintHelperOptions.Orientation = PrintOrientation.Landscape;

// Create a new PrintHelper instance
// "container" is a XAML panel that will be used to get the list of printable controls.
var printHelper = new PrintHelper(container, defaultPrintHelperOptions);
```

Using custom settings for one print job:

```csharp
// Create a new PrintHelper instance
// "container" is a XAML panel that will be used to get the list of printable controls.
// "defaultPrintHelperOptions" is a PrintHelperOptions instance that will be used to get the default options for printing.
var printHelper = new PrintHelper(container, defaultPrintHelperOptions);

// Create a new PrintHelperOptions instance
var printHelperOptions = new PrintHelperOptions();

//Add options that you want to be displayed on the print dialog
printHelperOptions.AddDisplayOption(StandardPrintTaskOptions.Orientation);

//Set preselected settings
printHelperOptions.Orientation = PrintOrientation.Landscape;

// Start printing process
await _printHelper.ShowPrintUIAsync("UWP Community Toolkit Sample App", printHelperOptions);
```

Print a list with each item on a separate page with static header and page number:

```csharp
// Create a new PrintHelper instance
// "container" is a XAML panel that will be used to get the list of printable controls.
var printHelper = new PrintHelper(container);

var pageNumber = 0;

foreach (var item in PrintSampleItems)
{
    var grid = new Grid();
    grid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
    grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
    grid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });

    // Static header
    var header = new TextBlock { Text = "UWP Community Toolkit Sample App - Print Helper - Custom Print", Margin = new Thickness(0, 0, 0, 20) };
    Grid.SetRow(header, 0);
    grid.Children.Add(header);

    // Main content with layout from data template
    var cont = new ContentControl();
    cont.ContentTemplate = Resources["CustomPrintTemplate"] as DataTemplate;
    cont.DataContext = item;
    Grid.SetRow(cont, 1);
    grid.Children.Add(cont);

    // Footer with page number
    pageNumber++;
    var footer = new TextBlock { Text = string.Format("page {0}", pageNumber), Margin = new Thickness(0, 20, 0, 0) };
    Grid.SetRow(footer, 2);
    grid.Children.Add(footer);

    printHelper.AddFrameworkElementToPrint(grid);
}

// Start printing process
await printHelper.ShowPrintUIAsync("UWP Community Toolkit Sample App", printHelperOptions);
```

## Requirements (Windows 10 Device Family)

| [Device family](http://go.microsoft.com/fwlink/p/?LinkID=526370) | Universal, 10.0.14393.0 or higher |
| --- | --- |
| Namespace | Microsoft.Toolkit.Uwp |

## API
* [Print Helper source code](https://github.com/Microsoft/UWPCommunityToolkit/blob/master/Microsoft.Toolkit.Uwp/Helpers/PrintHelper/)
