
# ✨ New Repository ✨

❗ We moved development to [a new repository, 'Windows'](https://aka.ms/toolkit/windows). It contains the new infrastructure and codebase for the 8.0+ versions of the Windows Community Toolkit that contains a single-codebase for [UWP/WinUI2](https://aka.ms/winui2), [WindowsAppSDK/WinUI3](https://aka.ms/winui3), and [Uno Platform](https://platform.uno) compatible components. _Please visit it for the latest information._

📝 You can find out more about this latest release [on our blog post here](https://devblogs.microsoft.com/ifdef-windows/announcing-windows-community-toolkit-v8-0/). The Sample Gallery has also been [updated in the Microsoft Store](https://aka.ms/windowstoolkitapp).

🧪 For new feature discussion and development, see [Windows Community Toolkit Labs](https://aka.ms/toolkit/labs/windows)

🐞 For new issues, please try and reproduce on the latest packages, and then open a discussion or issue in the [new repository](https://aka.ms/toolkit/windows).

📼 This repo contains the prior 7.x UWP-only based code for the Windows Community Toolkit's prior releases; the Windows App SDK compatible code is in the [`winui` branch](https://github.com/CommunityToolkit/WindowsCommunityToolkit/tree/winui). Much of this code has been merged and ported to the single-codebase version in the new repository; however, this repo will be archived for history and reference to enable migrating other components or issues, if needed.

## 🧰 Windows Community Toolkit 7.x

The Windows Community Toolkit is a collection of helper functions, custom controls, and app services. It simplifies and demonstrates common developer patterns when building experiences for Windows 10 and Windows 11. It contains components and helpers for both UWP and WinUI 3/WinAppSDK applications. Find out more about our support for [WinUI 3 here](https://aka.ms/wct-winui3).

**Note**: Our general helpers for .NET are now the [.NET Community Toolkit](https://aka.ms/toolkit/dotnet), including the [MVVM Toolkit](https://aka.ms/mvvmtoolkit).

| Target | Branch | Status | Recommended package version |
| ------ | ------ | ------ | ------ |
| Production (UWP) | rel/7.1.2 | [![Build Status](https://dev.azure.com/dotnet/CommunityToolkit/_apis/build/status/Toolkit-CI?branchName=rel/7.1.2)](https://dev.azure.com/dotnet/CommunityToolkit/_build/latest?definitionId=10&branchName=rel/7.1.2) | [![NuGet](https://img.shields.io/nuget/v/Microsoft.Toolkit.Uwp.svg)](https://www.nuget.org/profiles/Microsoft.Toolkit) |
| Production (WinAppSDK/WinUI 3) | rel/winui/7.1.2 | [![Build Status](https://dev.azure.com/dotnet/CommunityToolkit/_apis/build/status/Toolkit-CI?branchName=rel/winui/7.1.2)](https://dev.azure.com/dotnet/CommunityToolkit/_build/latest?definitionId=10&branchName=rel/winui/7.1.2) | [![NuGet](https://img.shields.io/nuget/v/CommunityToolkit.WinUI.svg)](https://www.nuget.org/packages?q=CommunityToolkit.WinUI) |
| Previews | main | [![Build Status](https://dev.azure.com/dotnet/CommunityToolkit/_apis/build/status/Toolkit-CI?branchName=main)](https://dev.azure.com/dotnet/CommunityToolkit/_build/latest?definitionId=10) | [![DevOps](https://vsrm.dev.azure.com/dotnet/_apis/public/Release/badge/696bc9fd-f160-4e97-a1bd-7cbbb3b58f66/1/1)](https://dev.azure.com/dotnet/CommunityToolkit/_packaging?_a=feed&feed=CommunityToolkit-MainLatest) |

## 📢 WCT vNext Preview

The toolkit is being migrated to new infrastructure in [this repository](https://github.com/CommunityToolkit/Windows).

[Read the blog post](https://devblogs.microsoft.com/ifdef-windows/the-windows-community-toolkit-2023-update/)

## 🧪 Windows Community Toolkit Labs 💻
Labs makes it easy to contribute to the Windows Community Toolkit, try out new features still in development, and collaborate with others.

Find out more [here](https://aka.ms/toolkit/labs/windows).

## 🙌 Getting Started

Please read the [Getting Started with the Windows Community Toolkit](https://docs.microsoft.com/windows/communitytoolkit/getting-started) page for more detailed information about using the toolkit.

## 📃 Documentation

All documentation for the toolkit is hosted on [Microsoft Docs](https://docs.microsoft.com/windows/communitytoolkit/). All API documentation can be found at the [.NET API Browser](https://docs.microsoft.com/dotnet/api/?view=win-comm-toolkit-dotnet-stable).

## 📱 Windows Community Toolkit Sample App

Want to see the toolkit in action before jumping into the code? Download and play with the [Windows Community Toolkit Sample App](https://www.microsoft.com/store/apps/9nblggh4tlcq) from the Store.

## ⁉ Support

If you need help with something or have an idea, feel free to start a [Discussion](https://github.com/CommunityToolkit/WindowsCommunityToolkit/discussions) or find us on [Discord](https://aka.ms/wct/discord). If you have detailed repro steps, open an [issue here instead](https://github.com/CommunityToolkit/WindowsCommunityToolkit/issues/new/choose).

## 🚀 Contribution

Do you want to contribute? Check out our [Windows Community Toolkit Wiki](https://aka.ms/wct/wiki) page to learn more about contribution and guidelines.

## 📦 NuGet Packages

NuGet is a standard package manager for .NET applications which is built into Visual Studio. When you open solution in Visual Studio, choose the *Tools* menu > *NuGet Package Manager* > *Manage NuGet packages for solution…* Enter one of the package names mentioned in [Windows Community Toolkit NuGet Packages](https://docs.microsoft.com/windows/communitytoolkit/nuget-packages) table to search for it online.

## 📫 Features <a name="supported"></a>

The [Features list](https://github.com/MicrosoftDocs/WindowsCommunityToolkitDocs/blob/master/docs/toc.md#controls) refers to all the currently available features that can be found in the Windows Community Toolkit. Most features should work with the October 2018 Update (1809) SDK 17763 and above; however, refer to specific documentation on each feature for more information.

## 💠 Principles

1. The toolkit will be kept simple.
2. As soon as a comparable feature is available in the Windows SDK for Windows, it will be marked as deprecated.
3. All features will be supported for two Windows SDK for Windows release cycles or until another principle supersedes it.

## 🌍 Roadmap

[See our 2022 Plans here](https://github.com/CommunityToolkit/WindowsCommunityToolkit/discussions/4486).

Read what we [plan for next iterations](https://github.com/CommunityToolkit/WindowsCommunityToolkit/milestones), and feel free to ask questions.

Check out our [Preview Packages Wiki Page](https://github.com/CommunityToolkit/WindowsCommunityToolkit/wiki/Preview-Packages) to learn more about updating your NuGet sources in Visual Studio, then you can also get pre-release packages of upcoming versions to try.

## 📄 Code of Conduct

This project has adopted the code of conduct defined by the [Contributor Covenant](http://contributor-covenant.org/)
to clarify expected behavior in our community.
For more information see the [.NET Foundation Code of Conduct](CODE_OF_CONDUCT.md).

## 🏢 .NET Foundation

This project is supported by the [.NET Foundation](http://dotnetfoundation.org).

## 🏆 Contributors

[![Toolkit Contributors](https://contrib.rocks/image?repo=CommunityToolkit/WindowsCommunityToolkit)](https://github.com/CommunityToolkit/WindowsCommunityToolkit/graphs/contributors)

Made with [contrib.rocks](https://contrib.rocks).
