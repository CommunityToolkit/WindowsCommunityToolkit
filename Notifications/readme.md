Welcome to the Notifications section of the toolkit! This contains the Notifications library, including the object model for tile, toast, and badge XML (previously called NotificationsExtensions).

## Where should I add new code?
Any code for generating notifications that doesn't use WinRT API's should be written in the Microsoft.Toolkit.Uwp.Notifications.Shared project.

If there's UWP-specific code, we can create a new shared WinRT project, so that it can be shared between UWP C# and WinRT.

## What are all the projects for?
There's five notification projects...
 - Shared
 - NETStandard
 - Portable
 - WinRT
 - UWP

The Shared project is where all the code is contained.

The other four projects are simply there so that the correct DLL's (and WINMD) can be built for specific platforms.

We use a Portable library that targets .NET 4.0, since the .NET Standard doesn't support 4.0 (it starts at 4.5). Therefore, projects that target .NET 4.0 can still use our library to generate notifications.

We then have the NET Standard library. This library can be used by any project targeting .NET 4.5, and is future-proof thanks to .NET Standard.

Finally there's the WinRT library and UWP libraries, which include some WinRT API's that only exist in WinRT. The UWP library is only for C#, while the WinRT library is a Windows Runtime Component for JavaScript and C++.


| C#               | JavaScript/C++      |
| ---------------- | ------------------- |
| Portable         | UWP WinRT Component |
| NET Standard 1.0 |                     |
| UWP C# DLL       |                     |


## Scenarios we want to support

Imagine you add this library to a classic portable class library, and you also add it to your UWP app. In this case, your portable class library will receive the NETStandard dll (or the Portable dll if your library only targets 4.0). Your UWP project will receive the UWP dll.

Since these are two separate projects, you actually wouldn't be able to pass a ToastContent object from your portable class library to your UWP - that object is coming from a different DLL.

We fix this by making the names of the outputted DLL's the same. If two DLL's have the same name, only the one from the parent project (the UWP project in this case) is loaded. That means your portable class library will actually be using the UWP dll version at runtime, and you will be able to pass the ToastContent object from your portable class library to your UWP process.



## How are the test projects organized?

If you look in the UnitTests folder of the repo, you'll notice that there's three projects...
 - UnitTests.Notifications.Shared
 - UnitTests.Notifications.Portable
 - UnitTests.Notifications.UWP

That's because in our source code, we have some #IF defs for switching between the different types of reflection that C# uses, since it's different between a PCL and WinRT code.

Therefore, there are two different code paths, one path for NETFX_CORE, and another for when that isn't present. The two test projects exercise both code paths.

The "Portable" project uses linked files since Visual Studio doesn't support adding a shared project to that type of project.
