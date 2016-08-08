UWP Community Toolkit
===========

The UWP Community Toolkit is a collection of helper functions, custom controls and app services that simplify or demonstrate common developer tasks. Build UWP applications for Windows 10 faster and easier than ever before.

![Control Overview](githubresources/images/UWP-community-toolkit-overview.png "Sample Image")

## Getting started

Please read the [getting Started with the UWP Toolkit](en-us/uwp-community-toolkit/get-started/get-started.md) page for more detailed information about using the toolkit.

## UWP Community Toolkit Sample App

Want to see the toolkit in action before jumping into the code?  Download and play with the [UWP Toolkit Sample App](https://www.microsoft.com/store/apps/9nblggh4tlcq) from the Store.

## Nuget Packages

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

## Features

### Animations

- [Blur](animations/blur.md)
- [Offset](animations/offset.md)
- [Fade](animations/fade.md)
- [Parallel](animations/parallax.md)
- [Rotate](animations/rotate.md)
- [Scale](animations/scale.md)

### Controls

- [HamburgerMenu](controls/hamburgerMenu.md)
- [HeaderedTextBlock](controls/headeredTextBlock.md)
- [ImageEX](controls/ImageEx.md)
- [PullToRefreshListView](controls/PullToRefreshListView.md)
- [RadialGauge](controls/RadialGauge.md)
- [RangeSelecter](controls/RangeSelector.md)
- [AdaptiveGridView](controls/AdaptiveGridView.md)
- [RotatorTile](controls/RotatorTile.md)
- [SlideableListItem](controls/SlideableListItem.md)

### Code Helpers

- [Colors](helpers/colors.md)
- [Connection](helpers/connectionHelper.md)
- [ImageCache] (helpers/imagecache.md)
- [StorageFiles](helpers/storagefiles.md)
- [Streams](helpers/streams.md)
- [VisualTreeExtensions](helpers/visualtreeextensions.md)
- [WeakEventListener](helpers/weakeventlistener.md)

### Services

- [Bing](services/bing.md)
- [Facebook](services/facebook.md)
- [Twitter](services/twitter.md)

### Notifications
- [Tiles](https://blogs.msdn.microsoft.com/tiles_and_toasts/2015/06/30/adaptive-tile-templates-schema-and-documentation/)
- [Toasts](https://blogs.msdn.microsoft.com/tiles_and_toasts/2015/07/02/adaptive-and-interactive-toast-notifications-for-windows-10/)

## Feedback and Requests

Please use [Github issues](https://github.com/Microsoft/UWPCommunityToolkit/issues) for questions or comments.

If you have specific feature requests or would like to vote on what others are recommending visit our [UWP Community Toolkit User Voice](https://aka.ms/uwpcommunitytoolkituservoice).

## Contributing
Do you want to contribute? Here are our [contribution guidelines](https://github.com/Microsoft/UWPCommunityToolkit/blob/master/contributing.md)

## Principles

 - Principle #1: The toolkit will be kept simple.
 - Principle #2: As soon as a comparable feature is available in the Windows SDK for Windows 10, it will be marked as deprecated.
 - Principle #3: All features will be supported for two Windows SDK for Windows 10 release cycles or until another principle supersedes it.

This project has adopted the [Microsoft Open Source Code of Conduct](https://opensource.microsoft.com/codeofconduct/). For more information see the [Code of Conduct FAQ](https://opensource.microsoft.com/codeofconduct/faq/) or contact [opencode@microsoft.com](mailto:opencode@microsoft.com) with any additional questions or comments.

## Roadmap

Read what we [plan for next iteration](https://github.com/Microsoft/UWPCommunityToolkit/issues?q=is%3Aopen+is%3Aissue+milestone%3Av1.1), and feel free to ask questions.
