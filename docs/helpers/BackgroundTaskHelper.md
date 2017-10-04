---
title: Background Task Helper
author: nmetulev
ms.date: 08/20/2017
description: The Background Task Helper helps users interacting with background tasks in an easier manner. 
keywords: windows 10, uwp, uwp community toolkit, uwp toolkit, Background Task Helper
---

# Background Task Helper

The **Background Task Helper** helps users interacting with background tasks in an easier manner. 

## Example

### Using Multi-Process Model

Using MPM (Multi-Process Model) is the classic way of using Background Task.
To make it work, you will need : 

* To create Background Tasks in a Windows Runtime Component
* To register the Background Tasks in the package manifest (appxmanifest file)

Once it is done, you can register your Background Tasks.

```csharp
	
    // Be sure to include the using at the top of the file:
    //using Microsoft.Toolkit.Uwp;
    //using Windows.ApplicationModel.Background;

    // Register a normal, seperate process, background task
    BackgroundTaskRegistration registered = BackgroundTaskHelper.Register("TaskName", "TaskEntryPoint", new TimeTrigger(15, true));

    // This can also be written using the overload of Register with Type parameter.
    BackgroundTaskRegistration registered = BackgroundTaskHelper.Register(typeof(BackgroundTaskClass), new TimeTrigger(15, true));

    // With condition
    BackgroundTaskRegistration registered = 
        BackgroundTaskHelper.Register(typeof(BackgroundTaskClass), 
                                      new TimeTrigger(15, true), 
                                      false, true, 
                                      new SystemCondition(SystemConditionType.InternetAvailable));

    // 2 or more conditions
    BackgroundTaskRegistration registered = 
        BackgroundTaskHelper.Register(typeof(BackgroundTaskClass), 
                                      new TimeTrigger(15, true), 
                                      false, true, 
                                      new SystemCondition(SystemConditionType.InternetAvailable), 
                                      new SystemCondition(SystemConditionType.UserPresent));

```

### Using Single-Process Model

Using MPM (Single-Process Model) is the new and simple way of using Background Task.
It is required to target Anniversary Update (SDK 14393) or later.

Using SPM, you can declare your Background Tasks inside your own project, no need to create a Windows Runtime Component.
Moreover, it is no longer required to register the Background Tasks in the package manifest (appxmanifest file).

Once you have created the Background Task, you can register it by calling the `Register` method.

```csharp

    // Be sure to include the using at the top of the file:
    //using Microsoft.Toolkit.Uwp;
    //using Windows.ApplicationModel.Background;

    // Register a single process background task (Anniversary Update and later ONLY)
    BackgroundTaskRegistration registered = BackgroundTaskHelper.Register("Name of the Background Task", new TimeTrigger(15, true));

```

The other difference between SPM and MPM is that in SPM, you have to handle your Background Tasks inside the `OnBackgroundActivated` event of `App.xaml.cs` class.
Here is an example of how to handle Background Tasks in SPM.

```csharp

    /// <summary>
    /// Event fired when a Background Task is activated (in Single Process Model)
    /// </summary>
    /// <param name="args">Arguments that describe the BackgroundTask activated</param>
    protected override void OnBackgroundActivated(BackgroundActivatedEventArgs args)
    {
        base.OnBackgroundActivated(args);

        var deferral = args.TaskInstance.GetDeferral();

        switch (args.TaskInstance.Task.Name)
        {
            case "Name of the Background Task":
                new TestBackgroundTask().Run(args.TaskInstance);
                break;
        }

        deferral.Complete();
    }

```

### Resources

You can find more examples in our [unit tests](https://github.com/Microsoft/UWPCommunityToolkit/blob/master/UnitTests/Helpers/Test_BackgroundTaskHelper.cs)

## Requirements (Windows 10 Device Family)

| [Device family](http://go.microsoft.com/fwlink/p/?LinkID=526370) | Universal, 10.0.14393.0 or higher |
| --- | --- |
| Namespace | Microsoft.Toolkit.Uwp |

## API

* [Background Task source code](https://github.com/Microsoft/UWPCommunityToolkit/blob/master/Microsoft.Toolkit.Uwp/Helpers/BackgroundTaskHelper.cs)

