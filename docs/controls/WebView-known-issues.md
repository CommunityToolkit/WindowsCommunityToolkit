---
title: Known Issues for WebView control for Windows Forms and WPF
author: normesta
description: This guide highlights known limitations with the current release of the WebView control for Windows Forms and WPF applications.
keywords: windows 10, uwp, uwp community toolkit, uwp toolkit, WebView, Windows Forms, WPF, known issues, release notes
---

# Known Issues of the WebView control for Windows Forms and WPF applications

This document describes issues and limitations in this release of the **WebView** control.

## WebView control members

The **WebView** control does not implement these methods of the [IWebViewControl](https://docs.microsoft.com/uwp/api/windows.web.ui.iwebviewcontrol).

* [NavigateToLocalStreamUri](https://docs.microsoft.com/uwp/api/windows.web.ui.iwebviewcontrol.navigatetolocalstreamuri)

* [NavigateWithHttpRequestMessage](https://docs.microsoft.com/uwp/api/windows.web.ui.iwebviewcontrol.navigatewithhttprequestmessage)

* [CapturePreviewToStreamAsync](https://docs.microsoft.com/uwp/api/windows.web.ui.iwebviewcontrol.capturepreviewtostreamasync)

* [CaptureSelectedContentToDataPackageAsync](https://docs.microsoft.com/uwp/api/windows.web.ui.iwebviewcontrol.captureselectedcontenttodatapackageasync)

* [BuildLocalStreamUri](https://docs.microsoft.com/uwp/api/windows.web.ui.iwebviewcontrol.buildlocalstreamuri)

The **WebView** control implements these events of the [IWebViewControl](https://docs.microsoft.com/uwp/api/windows.web.ui.iwebviewcontrol), but they don't work as expected.

* [LongRunningScriptDetected](https://docs.microsoft.com/uwp/api/windows.web.ui.iwebviewcontrol.longrunningscriptdetected)

* [UnsafeContentWarningDisplaying](https://docs.microsoft.com/uwp/api/windows.web.ui.iwebviewcontrol.unsafecontentwarningdisplaying)

* [UnsupportedUriSchemeIdentified](https://docs.microsoft.com/uwp/api/windows.web.ui.iwebviewcontrol.unsupportedurischemeidentified)

* [UnviewableContentIdentified](https://docs.microsoft.com/uwp/api/windows.web.ui.iwebviewcontrol.unviewablecontentidentified)

* [NewWindowRequested](https://docs.microsoft.com/uwp/api/windows.web.ui.iwebviewcontrol.newwindowrequested)

## Rendering and layout

* The **WebView** control doesn't appear at the correct scale when users move the host application between monitors that have different screen resolutions.

* The **WebView** control doesn't appear as expected on some high-resolution displays.

* The [Control.Bounds](https://msdn.microsoft.com/library/system.windows.forms.control.bounds.aspx) property is not supported.

## Performance

* **WebView** controls in WPF applications have longer load times than Windows Forms applications.

* If the web view hangs or stops working, other applications that use the same underlying process (Win32WebViewHost) will hang or stop working as well.

## Security

* **WebView** controls don't render in XAML Browser Applications (XBAP).

  That's because a browser has partial trust to the system while WebView controls have full-trust.

* **WebView** controls won't appear if the Win32WebViewHost application is disabled in the location settings of the user's system.

* **WebView** controls won't function as expected in a WPF-based ClickOnce application.

* **WebView** controls function only in applications that use a Single-Threaded Apartment (STA) model. Multithreaded Apartment (MTA) model is not supported.

* **WebView** controls have not been tested for Windows Information Protection. Therefore, the information that moves in and out of the WebView control might not be properly secured even if an  organizational administrator applies a Windows Information Protection policy to the hosting application.  

* **WebView** controls have not been exhaustively tested for security.

## User interaction

* Accelerator keys don't work in **WebView** controls.

* Users can't use the TAB key to put the **WebView** into focus. They'll have to use the mouse or other pointing device to put the **WebView** control into focus.

## Paths and strings

* You can use only fully qualified paths to resources in members of the **WebView** control that accept string paths. **WebView** controls don't recognize the File:// or ms-appx:/// prefixes so they can't read from the file system or the package (if you've created a package for your application).

* **WebView** controls don't support URI's that are not encoded in UTF-8. Characters between UTF-16 and UTF-32 aren't supported.

* **WebView** controls don't support URIs that are greater than 2048 characters.

## Host container

* **WebView** controls can be hosted in a popup window. We recommend that you do not do this because support for that scenario will soon be disabled for security reasons.

* Web applications that run in a **WebView** control can't initiate downloads.

* Service workers can't run in a **WebView** control.

* Your code can't instantiate more than one instance of a **WebView** in the same application.

## WebView browser

* The [WebBrower.ObjectForScripting](https://msdn.microsoft.com/library/system.windows.controls.webbrowser.objectforscripting.aspx) property is not supported.

  Instead, use the [WebBrowser.InokeScript](https://msdn.microsoft.com/library/cc491132.aspx) method.

* You can't programmatically navigate by using a new window.

* You can't programmatically navigate to a specific frame.

* You can't programmatically navigate to a relative URI.

  To work around this issue, consider reading from a stream into a string, and then calling the NavigateToString(String) method of the WebView control.

* You can't programmatically print information from a WebView control.

* There's no way to programmatically refresh content with cache validation. By default, pages refresh without cache validation by sending a "Pragma:no-cache" header to the server.

 

 
