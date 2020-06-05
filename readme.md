---
topic: sample
languages:
- csharp
products:
- windows
---

# Windows Community Toolkit
The Windows Community Toolkit is a collection of helper functions, custom controls, and app services. It simplifies and demonstrates common developer patterns when building experiences for Windows 10.

## Build Status
| Target | Branch | Status | Recommended package version |
| ------ | ------ | ------ | ------ |
| Production | rel/6.0.0 | [![Build Status](https://dev.azure.com/dotnet/WindowsCommunityToolkit/_apis/build/status/Toolkit-CI?branchName=rel/6.0.0)](https://dev.azure.com/dotnet/WindowsCommunityToolkit/_build/latest?definitionId=10&branchName=rel/6.0.0) | [![NuGet](https://img.shields.io/nuget/v/Microsoft.Toolkit.Uwp.svg)](https://www.nuget.org/profiles/Microsoft.Toolkit) | 
| Pre-release beta testing | master | [![Build Status](https://dev.azure.com/dotnet/WindowsCommunityToolkit/_apis/build/status/Toolkit-CI?branchName=master)](https://dev.azure.com/dotnet/WindowsCommunityToolkit/_build/latest?definitionId=10) | [![MyGet](https://img.shields.io/dotnet.myget/uwpcommunitytoolkit/vpre/Microsoft.Toolkit.Uwp.svg)](https://dotnet.myget.org/gallery/uwpcommunitytoolkit) |

## Getting Started
Please read the [getting Started with the Windows Community Toolkit](https://docs.microsoft.com/windows/communitytoolkit/getting-started) page for more detailed information about using the toolkit.

## Documentation
All documentation for the toolkit is hosted on [Microsoft Docs](https://docs.microsoft.com/windows/communitytoolkit/). All API documentation can be found at the [.NET API Browser](https://docs.microsoft.com/dotnet/api/?view=win-comm-toolkit-dotnet-stable).

## Windows Community Toolkit Sample App
Want to see the toolkit in action before jumping into the code? Download and play with the [Windows Community Toolkit Sample App](https://www.microsoft.com/store/apps/9nblggh4tlcq) from the Store.

## NuGet Packages
NuGet is a standard package manager for .NET applications which is built into Visual Studio. To open the UI, from your open solution, choose the *Tools* menu > *NuGet Package Manager* > *Manage NuGet packages for solution...* . Enter one of the package names below to search for it online.

Once you do a search, you should see a list similar to the one below (versions may be different, but names should be the same).

![nuget packages](githubresources/images/NugetPackages.png "Nuget Packages")

| NuGet Package Name | Description |
| --- | --- |
| Microsoft.Toolkit | .NET Standard NuGet package containing common code |
| Microsoft.Toolkit.HighPerformance | .NET Standard and .NET Core NuGet package with performance oriented helpers, extensions, etc. |
| Microsoft.Toolkit.Parsers | .NET Standard NuGet package containing cross-platform parsers, such as Markdown and RSS |
| Microsoft.Toolkit.Services | .NET Standard NuGet package containing cross-platform services |
| Microsoft.Toolkit.Uwp | Main NuGet package includes code only helpers such as Colors conversion tool, Storage file handling, a Stream helper class, etc. |
| Microsoft.Toolkit.Uwp.Notifications | Notifications Package - Generate tile, toast, and badge notifications for Windows 10 via code.  Includes intellisense support to avoid having to use the XML syntax |
| Microsoft.Toolkit.Uwp.Notifications.Javascript | Notification Packages for JavaScript |
| Microsoft.Toolkit.Uwp.Services | Services Package - This NuGet package includes the service helpers for Facebook, LinkedIn, Microsoft Graph, Twitter and more |
| Microsoft.Toolkit.Uwp.UI | UI Packages - XAML converters, Visual tree extensions, and other extensions and helpers for your XAML UI |
| Microsoft.Toolkit.Uwp.UI.Animations | Animations and Composition behaviors such as Blur, Fade, Rotate, etc. |
| Microsoft.Toolkit.Uwp.UI.Controls | XAML Controls such as RadialGauge, RangeSelector, etc. | 
| Microsoft.Toolkit.Uwp.UI.Controls.DataGrid | XAML DataGrid control | 
| Microsoft.Toolkit.Uwp.UI.Controls.Layout | XAML layout controls such as WrapLayout, StaggeredLayout, etc. |
| Microsoft.Toolkit.Uwp.UI.Lottie | Library for rendering Adobe AfterEffects animations natively in Windows apps |
| Microsoft.Toolkit.Uwp.UI.Media | Brushes, Win2D/Composition effects, and helpers to create visual effects  |
| Microsoft.Toolkit.Uwp.Connectivity | API helpers such as BluetoothLEHelper and Networking | 
| Microsoft.Toolkit.Uwp.DeveloperTools | XAML user controls and services to help developer building their app | 

## <a name="supported"></a> Features
The [Features list](https://github.com/MicrosoftDocs/WindowsCommunityToolkitDocs/blob/master/docs/toc.md#controls) refers to all the currently available features that can be found in the Windows Community Toolkit. Most features should work with the Falls Creator Update SDK 16299 and above; however, refer to specific documentation on each feature for more information.

## Feedback and Requests
Please use [GitHub Issues](https://github.com/windows-toolkit/WindowsCommunityToolkit/issues) for bug reports and feature requests.
For general questions and support, please use [Stack Overflow](https://stackoverflow.com/questions/tagged/windows-community-toolkit) where questions should be tagged with the tag `windows-community-toolkit`.

## <a name="dependencies"></a> Required Dependencies
The following dependencies are required for building the Windows Community Toolkit repo and sample app:

* [Visual Studio 2019](https://visualstudio.microsoft.com/downloads/) with UWP and .NET workloads
  * You'll also need to check the `C++ (v142) Universal Windows Platform tools` option under the UWP workload
* [Windows SDK October 2018 Update 17763](https://developer.microsoft.com/en-us/windows/downloads/sdk-archive)
* [Sample App - Windows SDK May 2019 Update 18362](https://developer.microsoft.com/en-us/windows/downloads/sdk-archive) - **Important:** If you're building the Sample App on 2004, you must install the 19041 SDK and re-target the app [ref](https://github.com/microsoft/microsoft-ui-xaml/issues/1286).
* [.NET Core 3.1 SDK](https://dotnet.microsoft.com/download/visual-studio-sdks)
* [.NET Framework 4.6.2 Developer Pack](https://dotnet.microsoft.com/download/visual-studio-sdks)

## Contributing
Do you want to contribute? Here are our [contribution guidelines](https://github.com/windows-toolkit/WindowsCommunityToolkit/blob/master/contributing.md).

## Principles
* Principle **#1**: The toolkit will be kept simple.
* Principle **#2**: As soon as a comparable feature is available in the Windows SDK for Windows 10, it will be marked as deprecated.
* Principle **#3**: All features will be supported for two Windows SDK for Windows 10 release cycles or until another principle supersedes it.

This project has adopted the code of conduct defined by the [Contributor Covenant](http://contributor-covenant.org/)
to clarify expected behavior in our community.
For more information see the [.NET Foundation Code of Conduct](CODE_OF_CONDUCT.md).

## Roadmap
Read what we [plan for next iterations](https://github.com/windows-toolkit/WindowsCommunityToolkit/milestones), and feel free to ask questions.

By adding this ([NuGet repo](https://dotnet.myget.org/F/uwpcommunitytoolkit/api/v3/index.json) | [Gallery](https://dotnet.myget.org/gallery/uwpcommunitytoolkit)) to your NuGet sources in Visual Studio, you can also get pre-release packages of upcoming versions.

## .NET Foundation
This project is supported by the [.NET Foundation](http://dotnetfoundation.org).
