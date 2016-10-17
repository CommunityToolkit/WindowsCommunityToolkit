# PrintHelper

The StorageFileHelper is a class used to simplify document printing.
Based on https://github.com/Microsoft/Windows-universal-samples/blob/master/Samples/Printing/cs/PrintHelper.cs

It allows you to render a framework element per page.
To use it, you only need to instanciate a PrintHelper object and call the `AddFrameworkElementToPrint` method.

**Please note that there is no page break support. Each control will be printed in a single page.**.

When ready to print, just call `ShowPrintUIAsync` and let the helper do the work for you.

You can register to 3 events to control the printing process:
* OnPrintFailed will be called if user cancels the print or if the print has an issue
* OnPrintSucceeded will be called after a successful print
* PreviewPagesCreated


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
* [PrintHelper source code](https://github.com/Microsoft/UWPCommunityToolkit/blob/master/Microsoft.Toolkit.Uwp/Helpers/PrintHelper/)

