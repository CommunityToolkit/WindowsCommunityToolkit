---
title: WebViewCompatible control for Windows Forms and WPF
author: granitestatehacker
description: The Windows Community Toolkit provides a version of the UWP web view control that can be used in WPF and Windows Forms applications. This control embeds a view into your application that renders web content in one of two ways. For client environments that support the WebViewControl (Windows 10), that implementation is used. For legacy systems, System.Windows.Controls.WebBrowser implements the view.
keywords: windows 10, uwp, windows community toolkit, uwp community toolkit, uwp toolkit, WebView, Windows Forms, WPF
---

# WebViewCompatible control for Windows Forms and WPF

The **WebViewCompatible** control shows web content in your Windows Forms or WPF desktop application.

![WebViewCompatible example](../../resources/images/Controls/WebView/web-view-samples.png)

Unlike [WebView](WebView.md), **WebViewCompatible** uses one of two rendering engines to support a broader set of Windows clients:

* On Windows 10 devices, the newer Microsoft Edge rendering engine is used to embed a view that renders richly formatted HTML content from a remote web server, dynamically generated code, or content files.
* On devices running older versions of Windows, the [System.Windows.Controls.WebBrowser](https://docs.microsoft.com/dotnet/api/system.windows.controls.webbrowser?view=netframework-4.7.2) is used, which provides Internet Explorer engine-based rendering.

## About WebViewCompatible control

The WPF version of this control is located in the **Microsoft.Toolkit.Wpf.UI.Controls** namespace. The Windows Forms version is coming soon, and it will be located in the **Microsoft.Toolkit.Forms.UI.Controls** namespace. You can find additional related types (such as event args classes) in the **Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT** namespace.

Internally, these controls wrap the [WebViewControl](https://docs.microsoft.com/uwp/api/windows.web.ui.interop.webviewcontrol) class, so they provide a subset of members from that class. The [WebViewControl](https://docs.microsoft.com/uwp/api/windows.web.ui.interop.webviewcontrol) is similar to the [WebView](https://docs.microsoft.com/uwp/api/windows.ui.xaml.controls.webview) class, but it is designed to run out of process in a desktop application (such as a WPF or Windows Forms application) and it supports a smaller set of members.  Because **WebViewCompatible** wraps [WebViewControl](https://docs.microsoft.com/uwp/api/windows.web.ui.interop.webviewcontrol) and **WebBrowser**, it provides a simpler subset of common functionality.

Unless specified otherwise in this article, the documentation for the [WebViewControl](https://docs.microsoft.com/uwp/api/windows.web.ui.interop.webviewcontrol) class applies to the WPF and Windows Forms **WebView** controls. This article links to reference pages for the UWP [WebViewControl](https://docs.microsoft.com/uwp/api/windows.web.ui.interop.webviewcontrol) class for more information about most members.

## Prerequisites

:heavy_check_mark: Visual Studio 2017.

:heavy_check_mark: Windows 10 Insider Preview Build 17110 or a later release for WebViewControl-based underlying implementation.

:heavy_check_mark: .NET Framework 4.6.2 or a later release.

:heavy_check_mark: Configure your application for high DPI support. To learn how, see [this section](#high-dpi) of the guide.

## Feature limitations

When compared to the UWP **WebView** control, the current release of the WPF and Windows Forms **WebView** controls has some limitations. For the complete list of these limitations, see [Known Issues of the WebView control for Windows Forms and WPF applications](WebView-known-issues.md). The **WebViewCompatible** control has further limitations because it exposes a common interface that is a subset of common functionality across the two underlying implementations. In a system that supports the Windows 10 **WebViewControl**, this control will exhibit those limitations plus further exclusions to the exposed functionality. In a legacy environment, this control will use **WebBrowser**, and have the same additional interface limitations.

## Add the WebViewCompatible control to the Visual Studio Toolbox for Windows Forms applications

1. Install the [Microsoft.Toolkit.Forms.UI.Controls](https://www.nuget.org/packages/Microsoft.Toolkit.Forms.UI.Controls.WebView) NuGet package.

2. Open the Visual Studio **Toolbox**, right-click anywhere in the toolbox, and select the **Choose Items** option.

3. In the **.NET Framework Components** tab of the **Choose Toolbox Items** dialog box, click **Browse** to locate the **Microsoft.Toolkit.Forms.UI.Controls.dll** in your [NuGet package folder](https://docs.microsoft.com/nuget/consume-packages/managing-the-global-packages-and-cache-folders).

4. Add **Microsoft.Toolkit.Forms.UI.Controls.dll** to the list of Toolbox controls, and then close the **Choose Toolbox Items** dialog box.

   The **WebViewCompatible** control appears in the **General** section of the **Toolbox**.

   In **Solution Explorer**, the **Microsoft.Toolkit.Forms.UI.Controls** assembly appears in the **References** list.

### Add the WebViewCompatible control to the Visual Studio Toolbox for WPF applications

1. Install the [Microsoft.Toolkit.Wpf.UI.Controls](https://www.nuget.org/packages/Microsoft.Toolkit.Wpf.UI.Controls.WebView) NuGet package.

2. The **WebViewCompatible** control appears in the **Windows Community Toolkit** section of the **Toolbox** in Visual Studio or Blend and you can drag it directly the designer. You can also create an instance of the **WebViewCompatible** control in code, but we recommend that you do not add **WebViewCompatible** controls to popup windows because support for that scenario will soon be disabled for security reasons.

<a id="high-dpi" />

## Enable the WebViewCompatible control to appear properly on high DPI displays

If users open your application on displays that have a high DPI displays, your **WebViewCompatible** won't appear at the proper scale unless you configure your application first.

### Configure a Windows Forms application

For guidance, see [Configuring your Windows Forms app for high DPI support](https://docs.microsoft.com/dotnet/framework/winforms/high-dpi-support-in-windows-forms#configuring-your-windows-forms-app-for-high-dpi-support).

### Configure a WPF application

Add the following XML to your application manifest file:

```XML
<compatibility xmlns="urn:schemas-microsoft-com:compatibility.v1">
    <application>
      <!-- Windows 10 -->
      <supportedOS Id="{8e0f7a12-bfb3-4fe8-b9a5-48fd50a15a9a}" />
    </application>
  </compatibility>
```
Add the following XML to your application configuration file:

```XML
<application xmlns="urn:schemas-microsoft-com:asm.v3">
   <windowsSettings>
     <!-- The combination of below two tags have the following effect :
     1) Per-Monitor for >= Windows 10 Anniversary Update
     2) System < Windows 10 Anniversary Update -->
     <dpiAwareness xmlns="http://schemas.microsoft.com/SMI/2016/WindowsSettings">PerMonitor</dpiAwareness>
     <dpiAware xmlns="http://schemas.microsoft.com/SMI/2005/WindowsSettings">true/PM</dpiAware>
   </windowsSettings>
 </application>
```

## Modify the appearance of a WebViewCompatible instance

To constrain the display area, set the `Width` and `Height` properties.

This table contains links to each of these members.

| Member | Windows Forms WebView | WPF WebView |
|-------|-------------|---|
|Width property |[Width](https://docs.microsoft.com/dotnet/api/system.windows.forms.control.width)|[Width](https://docs.microsoft.com/dotnet/api/system.windows.frameworkelement.width)|
|Height property|[Height](https://docs.microsoft.com/dotnet/api/system.windows.forms.control.height)|[Height](https://docs.microsoft.com/dotnet/api/system.windows.frameworkelement.height)|


## Input events and tab order

You can use the [InvokeScript](https://docs.microsoft.com/uwp/api/windows.web.ui.interop.webviewcontrol.invokescriptasync) method with the JavaScript **eval** function to use the HTML event handlers, and use **window.external.notify** from the HTML event handlers to notify the application using the [ScriptNotify](https://docs.microsoft.com/uwp/api/windows.web.ui.interop.webviewcontrol.scriptnotify) event.

## Navigate to content

The **WebViewCompatible** control has several APIs for basic navigation:  [GoBack](https://docs.microsoft.com/uwp/api/windows.web.ui.interop.webviewcontrol.goback), [GoForward](https://docs.microsoft.com/uwp/api/windows.web.ui.interop.webviewcontrol.goforward), [Stop](https://docs.microsoft.com/uwp/api/windows.web.ui.interop.webviewcontrol.stop), [Refresh](https://docs.microsoft.com/uwp/api/windows.web.ui.interop.webviewcontrol.refresh), [CanGoBack](https://docs.microsoft.com/uwp/api/windows.web.ui.interop.webviewcontrol.cangoback), and [CanGoForward](https://docs.microsoft.com/uwp/api/windows.web.ui.interop.webviewcontrol.cangoforward). You can use these to add typical web browsing capabilities to your app.

To set the initial content of the **WebViewCompatible** control, you can set the [Source](https://docs.microsoft.com/uwp/api/windows.web.ui.interop.webviewcontrol.source) property in code, XAML, or in the **Properties** window. You can also use the **Navigate** methods to load content in code. Here's an example.

```csharp
webViewCompatible1.Navigate("http://www.contoso.com");
```

## Respond to navigation events

The **WebView** control provides several events that you can use to respond to navigation and content loading states. The events occur in the following order for the root web view content:

1. [NavigationStarting](https://docs.microsoft.com/uwp/api/windows.web.ui.interop.webviewcontrol.navigationstarting)

2. [ContentLoading](https://docs.microsoft.com/uwp/api/windows.web.ui.interop.webviewcontrol.contentloading)

3. [NavigationCompleted](https://docs.microsoft.com/uwp/api/windows.web.ui.interop.webviewcontrol.navigationcompleted)

The **NavigationStarting** event is raised before the web view navigates to new content. You can cancel navigation in a handler for this event by setting the ``WebViewNavigationStartingEventArgs.Cancel`` property to true.

```csharp
webViewCompatible1.NavigationStarting += webViewCompatible1_NavigationStarting;

private void webViewCompatible1_NavigationStarting(object sender, WebViewNavigationStartingEventArgs args)
{
    // Cancel navigation if URL is not allowed. (Implemetation of IsAllowedUri not shown.)
    if (!IsAllowedUri(args.Uri))
        args.Cancel = true;
}
```

The **ContentLoading** is raised when the web view has started loading new content.

```csharp
webViewCompatible1.ContentLoading += webViewCompatible1_ContentLoading;

private void webViewCompatible1_ContentLoading(WebView sender, WebViewContentLoadingEventArgs args)
{
    // Show status.
    if (args.Uri != null)
    {
        statusTextBlock.Text = "Loading content for " + args.Uri.ToString();
    }
}
```


The **NavigationCompleted** event is raised when the web view has finished loading the current content or if navigation has failed. To determine whether navigation has failed, check the **IsSuccess** and **WebErrorStatus** properties of the event args.

```csharp
webViewCompatible1.NavigationCompleted += webViewCompatible1_NavigationCompleted;

private void webViewCompatible1_NavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
{
    if (args.IsSuccess == true)
    {
        statusTextBlock.Text = "Navigation to " + args.Uri.ToString() + " completed successfully.";
    }
    else
    {
        statusTextBlock.Text = "Navigation to: " + args.Uri.ToString() +
                               " failed with error " + args.WebErrorStatus.ToString();
    }
}
```

## Interact with web view content

You can interact with the content of the web view by using the [InvokeScript](https://docs.microsoft.com/uwp/api/windows.web.ui.interop.webviewcontrol.invokescriptasync) method to invoke or inject script into the web view content, and the [ScriptNotify](https://docs.microsoft.com/uwp/api/windows.web.ui.interop.webviewcontrol.scriptnotify) event to get information back from the web view content.

To invoke JavaScript inside the web view content, use the [InvokeScriptAsync](https://docs.microsoft.com/uwp/api/windows.web.ui.interop.webviewcontrol.invokescriptasync) method. The invoked script can return only string values.

For example, if the content of a web view named `webView1` contains a function named `setDate` that takes 3 parameters, you can invoke it like this.

```csharp
string[] args = {"January", "1", "2000"};
string returnValue = await webView1.InvokeScriptAsync("setDate", args);
```

You can use **InvokeScriptAsync** with the JavaScript **eval** function to inject content into the web page.

Here, the text of a XAML text box (`nameTextBox.Text`) is written to a div in an HTML page hosted in `webView1`.

```csharp
private async void Button_Click(object sender, RoutedEventArgs e)
{
    string functionString = String.Format("document.getElementById('nameDiv').innerText = 'Hello, {0}';", nameTextBox.Text);
    await webView1.InvokeScriptAsync("eval", new string[] { functionString });
}
```

Scripts in the web view content can use **window.external.notify** with a string parameter to send information back to your app. To receive these messages, handle the [ScriptNotify](https://docs.microsoft.com/uwp/api/windows.web.ui.interop.webviewcontrol.scriptnotify) event.


## Requirements

| Device family | .NET 4.6.2, Windows 10 (introduced v10.0.17110.0) |
| -- | -- |
| Namespace | Windows Forms: Microsoft.Toolkit.Forms.UI.Controls <br/> WPF: Microsoft.Toolkit.Wpf.UI.Controls |
| NuGet package | Windows Forms: [Microsoft.Toolkit.Forms.UI.Controls.WebView](https://www.nuget.org/packages/Microsoft.Toolkit.Forms.UI.Controls.WebView) <br/> WPF: [Microsoft.Toolkit.Wpf.UI.Controls.WebView](https://www.nuget.org/packages/Microsoft.Toolkit.Wpf.UI.Controls.WebView) |

## API Source Code

- [WebViewCompatible (Windows Forms)](https://github.com/Microsoft/WindowsCommunityToolkit/tree/master/Microsoft.Toolkit.Win32/Microsoft.Toolkit.Forms.UI.Controls.WebView)
- [WebViewCompatible (WPF)](https://github.com/Microsoft/WindowsCommunityToolkit/tree/master/Microsoft.Toolkit.Win32/Microsoft.Toolkit.Wpf.UI.Controls.WebView)
