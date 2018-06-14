---
title: Windows Community Toolkit Nuget Packages
author: nmetulev
description: The Windows Community Toolkit is updated regularly with new controls, services, APIs, and more importantly, bug fixes. Make sure to regularly update your nuget packages
keywords: windows 10, uwp, windows community toolkit, uwp community toolkit, uwp toolkit, nuget, visual studio
---

# Windows Community Toolkit Nuget Packages

NuGet is a standard package manager for .Net applications that is built into Visual Studio. From your open solution choose the *Tools* menu, *NuGet Package Manager*, *Manage NuGet packages for solution...* to open the UI.  Enter one of the package names below to search for it online.

| NuGet Package Name | Description |
| --- | --- |
| Microsoft.Toolkit | .NET Standard NuGet package containing common code |
| Microsoft.Toolkit.Parsers | .NET Standard NuGet package containing cross-platform parsers, such as Markdown and RSS |
| Microsoft.Toolkit.Services | .NET Standard NuGet package containing cross-platform services |
| Microsoft.Toolkit.Uwp | Main NuGet package includes code only helpers such as Colors conversion tool, Storage file handling, a Stream helper class, etc. |
| Microsoft.Toolkit.Uwp.Notifications | Notifications Package - Generate tile, toast, and badge notifications for Windows 10 via code.  Includes intellisense support to avoid having to use the XML syntax. |
| Microsoft.Toolkit.Uwp.Notifications.Javascript | Notification Packages for JavaScript |
| Microsoft.Toolkit.Uwp.Services | Services Package - This NuGet package includes the service helpers for Facebook, LinkedIn, Microsoft Graph, Twitter and more |
| Microsoft.Toolkit.Uwp.UI | UI Packages - Brushes, XAML converters, Visual tree extensions, and other extensions and helpers for your XAML UI. |
| Microsoft.Toolkit.Uwp.UI.Animations | Animations and Composition behaviors such as Blur, Fade, Rotate, etc. |
| Microsoft.Toolkit.Uwp.UI.Controls | XAML Controls such as RadialGauge, RangeSelector, etc. | 
| Microsoft.Toolkit.Uwp.Connectivity | API helpers such as BluetoothLEHelper and Networking | 
| Microsoft.Toolkit.Uwp.DeveloperTools | XAML user controls and services to help developer building their app | 


## Search in Visual Studio

Searching in Visual Studio package manager you should see a list similar to the one below (version numbers may be different, but names should be the same).

![NuGet packages](resources/images/NugetPackages.png "Nuget Packages")

## Update Nuget Packages

The Windows Community Toolkit is updated regularly with new controls, services, APIs, and more importantly, bug fixes. To make sure you are on the latest version, open your project in Visual Studio, choose the **Tools** menu, select **NuGet Package Manager** -> **Manage NuGet Packages for Solution...** and select the *Updates* tab. Select the package you want to update and click Instal to update to the latest version.

## Getting Started

Read the [getting Started with the Windows Community Toolkit](getting-started.md) for more instructions on using these NuGet Packages in your own projects. 

## Windows 10 Store App

Want to see the controls and animations in action before jumping into the code?  We have published the [Windows Community Toolkit Sample App](http://aka.ms/uwptoolkitapp) to the Windows 10 store.  Download the app and play with the controls live to see what they do before ever writing a line of code.

## GitHub Repository

Visit the [Windows Community Toolkit Github Repository](http://aka.ms/uwptoolkit) to see the current source code, what is coming next, and to clone the repository.  Community contributions are welcome!
