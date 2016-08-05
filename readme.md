UWP Community Toolkit
===========

The UWP Community Toolkit is a collection of XAML controls, behaviors and services collected to make building UWP applications for Windows 10 faster and easier than ever before.

![Control Overview](githubresources/images/UWP-community-toolkit-overview.png "Sample Image")

## Windows 10 Store App

Want to see the controls and behaviors in action before jumping into the code?  We have published the [UWP Toolkit Sample Application](https://www.microsoft.com/store/apps/9nblggh4tlcq) to the Windows 10 store.  Download the app and play with the controls live to see what they do before ever writing a line of code.

## Feedback and Requests

Please use [Github issues](https://github.com/Microsoft/UWPCommunityToolkit/issues) for questions or comments.

If you have specific feature requests or would like to vote on what others are recommending visit our [UWP Community Toolkit User Voice](https://aka.ms/uwpcommunitytoolkituservoice).

Please use [Github issues](https://github.com/Microsoft/UWPCommunityToolkit/issues) for questions or comments.

## Principles

 - Principle #1: As soon as a comparable toolkit control or a service is available in official SDK, we will plan to mark it as deprecated
 - Principle #2: We want to keep the toolkit useful and not bloated with too specific services that will become hard to maintain
 - Principle #3: All controls / services are supported for 2 Windows SDK release cycles or until another Principle supersedes it.

This project has adopted the [Microsoft Open Source Code of Conduct](https://opensource.microsoft.com/codeofconduct/). For more information see the [Code of Conduct FAQ](https://opensource.microsoft.com/codeofconduct/faq/) or contact [opencode@microsoft.com](mailto:opencode@microsoft.com) with any additional questions or comments.

## Roadmap

Read what we [plan for next iteration](https://github.com/Microsoft/UWPCommunityToolkit/issues?q=is%3Aopen+is%3Aissue+milestone%3Av1.1), and feel free to ask questions.

## Getting started

Please read the [getting Started with the UWP Toolkit](en-us/uwp-community-toolkit/get-started/get-started.md) page for more detailed information about using the toolkit.

# Nuget Packages

NuGet is a standard package manager for .Net applications that is built into Visual Studio. From your open solution choose the *Tools* menu, *NuGet Package Manger*, *Mange NuGet packages for solution...* to open the UI.  Enter one of the package names below to search for it online.

Once you search you should see a list similar to the one below (versions may be different, but names should be the same).

![nuget packages](githubresources/images/NugetPackages.png "Nuget Packages")

### Base Toolkit (code only) 

UWP Community Toolkit main NuGet package includes code only helpers for Colors, Internet Connection detection, Storage file handling, and a Stream helper class.

**Microsoft.Toolkit.Uwp**

### Notifications Package

Generate tile, toast, and badge notifications for Windows 10 via code.  Includes intellisense support to avoid having to use the XML syntax.

**Microsoft.Toolkit.Uwp.Notifications**

**Microsoft.Toolkit.Uwp.Notifications.Javascript**

### Services Package
This NuGet package includes the service helpers for Bing, Facebook, and Twitter.

**Microsoft.Toolkit.Uwp.Services**

### UI Packages
Helpers and services that are built on top of Windows Composition layer to provide animations and XAML controls.

**Microsoft.Toolkit.Uwp.UI**

**Microsoft.Toolkit.Uwp.UI.Animations**

**Microsoft.Toolkit.Uwp.UI.Controls**


# Controls, Animations and Code Helpers

## Animations

- [Blur](animations/blur.md)
- [Offset](animations/offset.md)
- [Fade](animations/fade.md)
- [Parallel](animations/parallax.md)
- [Rotate](animations/rotate.md)
- [Scale](animations/scale.md)

## Controls

- [HamburgerMenu](controls/hamburgerMenu.md)
- [HeaderedTextBlock](controls/headeredTextBlock.md)
- [ImageEX](controls/ImageEx.md)
- [PullToRefreshListView](controls/PullToRefreshListView.md)
- [RadialGauge](controls/RadialGauge.md)
- [RangeSelecter](controls/RangeSelector.md)
- [ResponsiveGridView](controls/ResponsiveGridView.md)
- [RotatorTile](controls/RotatorTile.md)
- [SlideableListItem](controls/SlideableListItem.md)
- [VariableSizeGridView](controls/VariableSizeGridView.md)

## Code Helpers

- [Colors](helpers/colors.md)
- [Connection](helpers/connectionHelper.md)
- [ImageCache] (helpers/imagecache.md)
- [StorageFiles](helpers/storagefiles.md)
- [Streams](helpers/streams.md)
- [WeakEventListener](helpers/weakeventlistener.md)

## Services

- [Bing](services/bing.md)
- [Facebook](services/facebook.md)
- [Twitter](services/twitter.md)

## Notifications

### Tiles
- [Adaptive Tile Notifications Documentation](https://blogs.msdn.microsoft.com/tiles_and_toasts/2015/06/30/adaptive-tile-templates-schema-and-documentation/)
- [Quickstart: Sending a local tile notification](https://blogs.msdn.microsoft.com/tiles_and_toasts/2015/10/05/quickstart-sending-a-local-tile-notification-in-windows-10/)

### Toasts
- [Interactive Toast Notifications Documentation](https://blogs.msdn.microsoft.com/tiles_and_toasts/2015/07/02/adaptive-and-interactive-toast-notifications-for-windows-10/)
- [Quickstart: Sending a local toast notification](https://blogs.msdn.microsoft.com/tiles_and_toasts/2015/07/08/quickstart-sending-a-local-toast-notification-and-handling-activations-from-it-windows-10/)

## Contributing
Do you want to contribute? Here are our [contribution guidelines](https://github.com/Microsoft/UWPCommunityToolkit/blob/master/contributing.md)

## Controls and Features
To get a complete list of supported controls, services and features, please visit our [documentation website](https://developer.microsoft.com/en-us/windows/windows-apps/uwp-community-toolkit).
