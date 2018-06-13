---
title: Windows Community Toolkit Known Issues
author: nmetulev
description: The Windows Community Toolkit is a collection of helper functions, custom controls, and app services. It simplifies and demonstrates common developer tasks building UWP apps for Windows 10. 
keywords: windows 10, uwp, windows community toolkit, uwp community toolkit, uwp toolkit, known issues
---

# Windows Community Toolkit Known Issues

> [!NOTE]
For an accurate list of known bugs and issues, take a look at the [issues on GitHub](https://github.com/Microsoft/WindowsCommunityToolkit//issues)


## Controls

### InfiniteCanvas
* InfiniteCanvas is not supported on Creators Update - [see issue for details](https://github.com/Microsoft/WindowsCommunityToolkit/issues/2162)
* Visual Studio designer crashes with InfiniteCanvas - [see issue for details](https://github.com/Microsoft/WindowsCommunityToolkit/issues/2160) 

### ScrollHeader
* ScrollHeader with `Mode="Sticky"` inside a ListView with grouped items causes the groups' headers to be displayed in front of the header - [see issue for details](https://github.com/Microsoft/WindowsCommunityToolkit//issues/1446)

### HamburgerMenu
* HamburgerMenu selected indicator moves with keyboard focus - [see issue for details](https://github.com/Microsoft/WindowsCommunityToolkit//issues/1306)

### WebView
For the complete list of issues and limitations in this release of the **WebView** control, see [Known Issues of the WebView control for Windows Forms and WPF applications](controls/WebView-known-issues.md).

## Extensions

### Alternating rows
* inserting items does not work properly - [see issue for details](https://github.com/Microsoft/WindowsCommunityToolkit//issues/1837)

### TextBoxRegEx
* Some phone number formats are not supported - [see issue for details](https://github.com/Microsoft/WindowsCommunityToolkit//issues/1821)
