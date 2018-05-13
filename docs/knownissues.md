---
title: Windows Community Toolkit Known Issues
author: nmetulev
description: The Windows Community Toolkit is a collection of helper functions, custom controls, and app services. It simplifies and demonstrates common developer tasks building UWP apps for Windows 10. 
keywords: windows 10, uwp, windows community toolkit, uwp community toolkit, uwp toolkit, known issues
---

# Windows Community Toolkit Known Issues

> [!NOTE]
For an accurate list of known bugs and issues, take a look at the [issues on GitHub](https://github.com/Microsoft/UWPCommunityToolkit/issues)


## Services

* UnresolvableAssemblyReference when building for Release when using the Microsoft.Toolkit.Uwp.Services package
This error will not be fixed until the 3.0 release as it requires a breaking change. The workaround is to manually add a reference to the Microsoft.IdentityModel.Clients.ActiveDirectory package, version 2.29.0 (or any version between 2.22 and 3.0) - [see issue for more details](https://github.com/Microsoft/UWPCommunityToolkit/issues/1788)


## Controls

### MasterDetailsView
* Back button does not close the details view in some cases - [see issue for detials](https://github.com/Microsoft/UWPCommunityToolkit/issues/1589)
* Setting background to transparent causes Master and Detail view to overlap in narrow view - [see issue for detials](https://github.com/Microsoft/UWPCommunityToolkit/issues/1117)
* MasterDetailsView does not adapt to keyboard control in details view - [see issue for detials](https://github.com/Microsoft/UWPCommunityToolkit/issues/791)

### ScrollHeader
* ScrollHeader with `Mode="Sticky"` inside a ListView with grouped items causes the groups' headers to be displayed in front of the header - [see issue for details](https://github.com/Microsoft/UWPCommunityToolkit/issues/1446)

### HamburgerMenu
* HamburgerMenu selected indicator moves with keyboard focus - [see issue for details](https://github.com/Microsoft/UWPCommunityToolkit/issues/1306)


## Extensions

### Alternating rows 
* inserting items does not work propertly - [see issue for details](https://github.com/Microsoft/UWPCommunityToolkit/issues/1837)

### TextBoxRegEx
* Some phone number formats are not supported - [see issue for details](https://github.com/Microsoft/UWPCommunityToolkit/issues/1821)


## Parsers

### Markdown
* parser combines quotes with any white space in between - [see issue for details](https://github.com/Microsoft/UWPCommunityToolkit/issues/1761)


## Notifications

The Notifications packages do not work with C++ and Javascrpt projects and throw an exception - [see issue for details](https://github.com/Microsoft/UWPCommunityToolkit/issues/1760)