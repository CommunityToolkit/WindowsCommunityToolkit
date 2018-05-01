Welcome to the Notifications section of the toolkit! This contains the Notifications library, including the object model for tile, toast, and badge XML (previously called NotificationsExtensions).

## Where should I add new code?
Any code for generating notifications should be written in the Microsoft.Toolkit.Uwp.Notifications.Shared project.

If there's UWP-specific code, use the appropriate `#ifdef`, `WINDOWS_UWP` or `WINRT`.

## What are all the projects for?
There are two notification projects...
 - Microsoft.Toolkit.Uwp.Notifications (used to for netstandard1.4, uap10.0, and native for WinRT) 
 - Microsoft.Toolkit.Uwp.Notifications.JavaScript (used to for the WinRT binary for JavaScript apps)

All code goes in the `Microsoft.Toolkit.Uwp.Notifications` project and will be automatically included in the JavaScript project.

| C#               | JavaScript/C++      |
| ---------------- | ------------------- |
| NET Standard 1.4 | UWP WinRT Component |
| UWP C# DLL |                     |



## Scenarios we want to support

Imagine you add this library to a .NET Standard class library, and you also add it to your UWP app. In this case, your .NET Standard class library will receive the NETStandard dll. Your UWP project will receive the UWP dll.

## How are the test projects organized?

If you look in the UnitTests folder of the repo, you'll notice that there's three projects...
 - UnitTests.Notifications.Shared
 - UnitTests.Notifications.NetCore
 - UnitTests.Notifications.UWP
 - UnitTests.Notifications.WinRT

That's because in our source code, we have some #IF defs for switching between the different types of reflection that C# uses, since it's different between a .NET Standard and WinRT code.

Therefore, there are two different code paths, one path for NETFX_CORE, and another for when that isn't present. The two test projects exercise both code paths.
