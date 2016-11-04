# DispatcherHelper

The DispatcherHelper class enables easy interaction with [CoreDispatcher](https://msdn.microsoft.com/en-us/library/windows/apps/windows.ui.core.coredispatcher.aspx), mainly in the case of executing a block of code in UI thread from a non-UI thread.

_What is included in the helper?_
- Extension method with overloads for [CoreDispatcher](https://msdn.microsoft.com/en-us/library/windows/apps/windows.ui.core.coredispatcher.aspx) class.
- Static helper methods of executing a specific function in UI thread current application's main window (configu)


## Example

```csharp

    // Executing from a non-UI thread with helper method
     int returnedFromUIThread = await DispatcherHelper.ExecuteOnUIThreadAsync<int>(() =>
     {
         // Code to execute with UI thread
         NormalTextBlock.Text = "Updated from a random thread!";
         return 1;
     });

     // returnedFromUIThread now is 1, execution can go on from the non-UI thread

     // Or update it manually via the Extension method for CoreDispatcher
     returnedFromUIThread = await CoreApplication.MainView.Dispatcher.AwaitableRunAsync<int>( () =>
     {
         NormalTextBlock.Text = "Updated from a random thread with extension method!";
         return 1;
     });


```

## Requirements (Windows 10 Device Family)

| [Device family](http://go.microsoft.com/fwlink/p/?LinkID=526370) | Universal, 10.0.10586.0 or higher |
| --- | --- |
| Namespace | Microsoft.Toolkit.Uwp |

## API

* [Connection Helper source code](https://github.com/Microsoft/UWPCommunityToolkit/blob/master/Microsoft.Toolkit.Uwp/Helpers/DispatcherHelper.cs)

