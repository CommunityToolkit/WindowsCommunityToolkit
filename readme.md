UWP Community Toolkit
===========

The UWP Community Toolkit is a collection of helper functions, custom controls, and app services. It simplifies and demonstrates common developer tasks building UWP apps for Windows 10.

## Build status

| Target | Branch | Status | Recommended Nuget packages version |
| ------ | ------ | ------ | ------ |
| Production | rel/2.0.0 | [![Build status](https://ci.appveyor.com/api/projects/status/o60lv2tt1nbtklw8/branch/rel/2.0.0?svg=true)](https://ci.appveyor.com/project/dotnetfoundation/uwpcommunitytoolkit/branch/rel/2.0.0) | [![NuGet](https://img.shields.io/nuget/v/Microsoft.Toolkit.Uwp.svg)](https://www.nuget.org/packages/Microsoft.Toolkit.Uwp/) |
| Pre-release beta testing | master | [![Build status](https://ci.appveyor.com/api/projects/status/o60lv2tt1nbtklw8/branch/master?svg=true)](https://ci.appveyor.com/project/dotnetfoundation/uwpcommunitytoolkit/branch/master) | [![MyGet](https://img.shields.io/dotnet.myget/uwpcommunitytoolkit/vpre/Microsoft.Toolkit.Uwp.svg)](https://dotnet.myget.org/gallery/uwpcommunitytoolkit) |

## Getting started

Please read the [getting Started with the UWP Toolkit](http://uwpcommunitytoolkit.readthedocs.io/en/master/Getting-Started/) page for more detailed information about using the toolkit.

## UWP Community Toolkit Sample App

Want to see the toolkit in action before jumping into the code?  Download and play with the [UWP Toolkit Sample App](https://www.microsoft.com/store/apps/9nblggh4tlcq) from the Store.

## Nuget Packages

NuGet is a standard package manager for .NET applications that is built into Visual Studio. From your open solution choose the *Tools* menu, *NuGet Package Manager*, *Manage NuGet packages for solution...* to open the UI.  Enter one of the package names below to search for it online.

Once you search you should see a list similar to the one below (versions may be different, but names should be the same).

![nuget packages](githubresources/images/NugetPackages.png "Nuget Packages")

| NuGet Package Name | Description |
| --- | --- |
| Microsoft.Toolkit | .NET Standard NuGet package containing common code |
| Microsoft.Toolkit.Services | .NET Standard NuGet package containing cross-platform services |
| Microsoft.Toolkit.Uwp | Main NuGet package includes code only helpers such as Colors conversion tool, Storage file handling, a Stream helper class, etc. |
| Microsoft.Toolkit.Uwp.Notifications | Notifications Package - Generate tile, toast, and badge notifications for Windows 10 via code.  Includes intellisense support to avoid having to use the XML syntax. |
| Microsoft.Toolkit.Uwp.Notifications.Javascript | Notification Packages for JavaScript |
| Microsoft.Toolkit.Uwp.Services | Services Package - This NuGet package includes the service helpers for Facebook, LinkedIn, Microsoft Graph, Twitter and more |
| Microsoft.Toolkit.Uwp.UI | UI Packages - XAML converters, Visual tree extensions and helpers for your XAML UI. |
| Microsoft.Toolkit.Uwp.UI.Animations | Animations and Composition behaviors such as Blur, Fade, Rotate, etc. |
| Microsoft.Toolkit.Uwp.UI.Controls | XAML Controls such as RadialGauge, RangeSelector, etc. | 
| Microsoft.Toolkit.Uwp.Connectivity | API helpers such as BluetoothLEHelper and Networking | 
| Microsoft.Toolkit.Uwp.DeveloperTools | XAML user controls and services to help developer building their app | 

## Features

### Animations

* [Blur](http://docs.uwpcommunitytoolkit.com/en/master/animations/Blur/)
* [ExpressionBuilder](http://www.uwpcommunitytoolkit.com/en/master/animations/Expressions/)
* [Fade](http://docs.uwpcommunitytoolkit.com/en/master/animations/Fade/)
* [FadeHeader](http://docs.uwpcommunitytoolkit.com/en/master/animations/FadeHeader/)
* [Light](http://docs.uwpcommunitytoolkit.com/en/master/animations/Light/)
* [Offset](http://docs.uwpcommunitytoolkit.com/en/master/animations/Offset/)
* [ParallaxService](http://docs.uwpcommunitytoolkit.com/en/master/animations/ParallaxService/)
* [ReorderGrid](http://docs.uwpcommunitytoolkit.com/en/master/animations/ReorderGrid/)
* [Rotate](http://docs.uwpcommunitytoolkit.com/en/master/animations/Rotate/)
* [Saturation](http://www.uwpcommunitytoolkit.com/en/master/animations/Saturation/)
* [Scale](http://docs.uwpcommunitytoolkit.com/en/master/animations/Scale/)

### Controls

* [AdaptiveGridView](http://docs.uwpcommunitytoolkit.com/en/master/controls/AdaptiveGridView/)
* [BladeView](http://docs.uwpcommunitytoolkit.com/en/master/controls/BladeView/)
* [Carousel](http://www.uwpcommunitytoolkit.com/en/master/controls/Carousel/)
* [DropShadowPanel](http://docs.uwpcommunitytoolkit.com/en/master/controls/DropShadowPanel/)
* [Expander](http://docs.uwpcommunitytoolkit.com/en/master/controls/Expander/)
* [GridSplitter](http://docs.uwpcommunitytoolkit.com/en/master/controls/GridSplitter/)
* [HamburgerMenu](http://docs.uwpcommunitytoolkit.com/en/master/controls/HamburgerMenu/)
* [HeaderedTextBlock](http://docs.uwpcommunitytoolkit.com/en/master/controls/HeaderedTextBlock/)
* [ImageEx](http://docs.uwpcommunitytoolkit.com/en/master/controls/ImageEx/)
* [InAppNotification](http://docs.uwpcommunitytoolkit.com/en/master/controls/InAppNotification/)
* [Loading](http://docs.uwpcommunitytoolkit.com/en/master/controls/Loading/)
* [MarkdownTextBlock](http://docs.uwpcommunitytoolkit.com/en/master/controls/MarkdownTextBlock/)
* [MasterDetailsView](http://docs.uwpcommunitytoolkit.com/en/master/controls/MasterDetailsView/)
* [Menu](http://docs.uwpcommunitytoolkit.com/en/master/controls/Menu/)
* [OrbitView](http://docs.uwpcommunitytoolkit.com/en/master/controls/OrbitView/)
* [PullToRefreshListView](http://docs.uwpcommunitytoolkit.com/en/master/controls/PullToRefreshListview/)
* [RadialGauge](http://docs.uwpcommunitytoolkit.com/en/master/controls/RadialGauge/)
* [RadialProgressBar](http://www.uwpcommunitytoolkit.com/en/master/controls/RadialProgressBar/)
* [RangeSelector](http://docs.uwpcommunitytoolkit.com/en/master/controls/RangeSelector/)
* [RotatorTile](http://docs.uwpcommunitytoolkit.com/en/master/controls/RotatorTile/)
* [ScrollHeader](http://docs.uwpcommunitytoolkit.com/en/master/controls/ScrollHeader/)
* [SlideableListItem](http://docs.uwpcommunitytoolkit.com/en/master/controls/SlidableListItem/)
* [TextToolbar](http://docs.uwpcommunitytoolkit.com/en/master/controls/TextToolbar/)
* [TileControl](http://docs.uwpcommunitytoolkit.com/en/master/controls/TileControl/)
* [WrapPanel](http://docs.uwpcommunitytoolkit.com/en/master/controls/WrapPanel/)

### Developer Tools
* [AlignmentGrid](http://www.uwpcommunitytoolkit.com/en/master/developer-tools/AlignmentGrid/)
* [FocusTracker](http://www.uwpcommunitytoolkit.com/en/master/developer-tools/FocusTracker/)

### Extensions
* [Hyperlink](http://docs.uwpcommunitytoolkit.com/en/master/extensions/Hyperlink/)
* [ListViewBase](http://docs.uwpcommunitytoolkit.com/en/master/extensions/ListViewBase/)
* [LogicalTree](http://docs.uwpcommunitytoolkit.com/en/master/extensions/LogicalTree/)
* [SurfaceDialTextbox](http://docs.uwpcommunitytoolkit.com/en/master/extensions/SurfaceDialTextbox/)
* [TextBoxMask](http://docs.uwpcommunitytoolkit.com/en/master/extensions/TextBoxMask/)
* [TextBoxRegex](http://docs.uwpcommunitytoolkit.com/en/master/extensions/TextBoxRegex/)
* [ViewExtensions](http://www.uwpcommunitytoolkit.com/en/master/extensions/ViewExtensions/)
* [VisualTree](http://docs.uwpcommunitytoolkit.com/en/master/extensions/VisualTree/)
* [WebView](http://docs.uwpcommunitytoolkit.com/en/master/extensions/WebView/)

### Code Helpers
* [AdvancedCollectionView](http://docs.uwpcommunitytoolkit.com/en/master/helpers/AdvancedCollectionView/)
* [BackgroundTaskHelper](http://docs.uwpcommunitytoolkit.com/en/master/helpers/BackgroundTaskHelper/)
* [BindableValueHolder](http://docs.uwpcommunitytoolkit.com/en/master/helpers/BindableValueHolder/)
* [BluetoothLEHelper](http://www.uwpcommunitytoolkit.com/en/master/helpers/BluetoothLEHelper/)
* [Colors](http://docs.uwpcommunitytoolkit.com/en/master/helpers/Colors/)
* [Converters](http://docs.uwpcommunitytoolkit.com/en/master/helpers/Converters/)
* [DeepLinkParsers](http://docs.uwpcommunitytoolkit.com/en/master/helpers/DeepLinkParsers/)
* [DispatcherHelper](http://docs.uwpcommunitytoolkit.com/en/master/helpers/DispatcherHelper/)
* [HttpHelper](http://docs.uwpcommunitytoolkit.com/en/master/helpers/HttpHelper/)
* [ImageCache](http://docs.uwpcommunitytoolkit.com/en/master/helpers/ImageCache/)
* [Incremental Loading Collection](http://docs.uwpcommunitytoolkit.com/en/master/helpers/IncrementalLoadingCollection/)
* [NetworkHelper](http://www.uwpcommunitytoolkit.com/en/master/helpers/NetworkHelper/)
* [Object Storage](http://docs.uwpcommunitytoolkit.com/en/master/helpers/ObjectStorage/)
* [PrinterHelper](http://docs.uwpcommunitytoolkit.com/en/master/helpers/PrinterHelper/)
* [StorageFiles](http://docs.uwpcommunitytoolkit.com/en/master/helpers/StorageFiles/)
* [Streams](http://docs.uwpcommunitytoolkit.com/en/master/helpers/Streams/)
* [SystemInformation](http://docs.uwpcommunitytoolkit.com/en/master/helpers/SystemInformation/)
* [WeakEventListener](http://docs.uwpcommunitytoolkit.com/en/master/helpers/WeakEventListener/)

### Services

* [Bing](http://docs.uwpcommunitytoolkit.com/en/master/services/Bing/)
* [Facebook](http://docs.uwpcommunitytoolkit.com/en/master/services/Facebook/)
* [LinkedIn](http://docs.uwpcommunitytoolkit.com/en/master/services/Linkedin/)
* [Microsoft Graph](http://docs.uwpcommunitytoolkit.com/en/master/services/MicrosoftGraph/)
* [Microsoft Translator Service](http://docs.uwpcommunitytoolkit.com/en/master/services/MicrosoftTranslator/)
* [OneDrive](http://docs.uwpcommunitytoolkit.com/en/master/services/OneDrive/)
* [Twitter](http://docs.uwpcommunitytoolkit.com/en/master/services/Twitter/)

### Notifications
- [Tiles](https://blogs.msdn.microsoft.com/tiles_and_toasts/2015/06/30/adaptive-tile-templates-schema-and-documentation/)
- [Toasts](https://blogs.msdn.microsoft.com/tiles_and_toasts/2015/07/02/adaptive-and-interactive-toast-notifications-for-windows-10/)

## Feedback and Requests

Please use [GitHub issues](https://github.com/Microsoft/UWPCommunityToolkit/issues) for questions and comments.
For feature requests, please create en entry in our [Uservoice](https://wpdev.uservoice.com/forums/110705-universal-windows-platform/category/193402-uwp-community-toolkit).

## Contributing
Do you want to contribute? Here are our [contribution guidelines](https://github.com/Microsoft/UWPCommunityToolkit/blob/master/contributing.md).

## Principles

 - Principle #1: The toolkit will be kept simple.
 - Principle #2: As soon as a comparable feature is available in the Windows SDK for Windows 10, it will be marked as deprecated.
 - Principle #3: All features will be supported for two Windows SDK for Windows 10 release cycles or until another principle supersedes it.

This project has adopted the code of conduct defined by the [Contributor Covenant](http://contributor-covenant.org/)
to clarify expected behavior in our community.
For more information see the [.NET Foundation Code of Conduct](http://www.dotnetfoundation.org/code-of-conduct). 

## Roadmap

Read what we [plan for next iterations](https://github.com/Microsoft/UWPCommunityToolkit/milestones), and feel free to ask questions.

By adding this ([Nuget repo](https://dotnet.myget.org/F/uwpcommunitytoolkit/api/v3/index.json) | [Gallery](https://dotnet.myget.org/gallery/uwpcommunitytoolkit)) to your Visual Studio, you can also get pre-release packages of upcoming version.

## .NET Foundation

This project is supported by the [.NET Foundation](http://www.dotnetfoundation.org).
