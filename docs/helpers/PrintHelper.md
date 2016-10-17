# Print Helper

The PrintHelper is a class used to simplify document printing.
It allows you to render a framework element per page.

To use it, you only have to instantiate a `PrintHelper` object and call `AddFrameworkElementToPrint` method to add the XAML controls you want to print.
Please note that controls cannot be linked to a visual tree. This means that their parent property has to be null. 
If you want to use a control from your current XAML page, you can disconnect it before sending it to print (by removing it from its container) or you can create just create a new one from scratch.

Please check the sample app code to see how to disconnect/reconnect a control that you want to print: 
https://github.com/Microsoft/UWPCommunityToolkit/blob/dev/Microsoft.Toolkit.Uwp.SampleApp/SamplePages/PrintHelper/PrintHelperPage.xaml.cs 

Several events are available to control the printing process:
* OnPrintFailed will be triggered if the user cancels the print or if something goes wrong
* OnPrintSucceeded will be triggered after a successful print
* OnPreviewPagesCreated will be triggered after print preview pages are generated. This allows you to control the look and feel of your page before they are sent to the spooler.

** Please note that page breaks are not supported. Every control will be printed on a single page**

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

## Requirements (Windows 10 Device Family)

| [Device family](http://go.microsoft.com/fwlink/p/?LinkID=526370) | Universal, 10.0.10586.0 or higher |
| --- | --- |
| Namespace | Microsoft.Toolkit.Uwp |

## API
* [Print Helper source code](https://github.com/Microsoft/UWPCommunityToolkit/blob/master/Microsoft.Toolkit.Uwp/Helpers/PrintHelper/)

