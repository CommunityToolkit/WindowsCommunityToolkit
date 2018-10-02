---
title: DirectWriteTextBlock
author: juma-msft
description: Defines a textblock which uses [DirectWrite](https://docs.microsoft.com/en-us/windows/desktop/directwrite/direct-write-portal) to render the font so that it can be oriented vertically
keywords: windows 10, uwp, windows community toolkit, uwp community toolkit, uwp toolkit, DockPanel, XAML Control, xaml
---

# DirectWriteTextBlock XAML Control

The [DirectWriteTextBlock Control](https://docs.microsoft.com/dotnet/api/microsoft.toolkit.uwp.ui.controls.winrt.DirectWriteTextBlock) defines a textblock which can render text vertically with limited DirectWrite support.

Do not use this to render horizontal text unless you need specific support from DirectWrite which isn't currently supported by XAML, it is less efficient than standard [TextBlock](https://docs.microsoft.com/uwp/api/windows.ui.xaml.controls.textblock) overall.

## Syntax

```xaml
<Page ...
     xmlns:controlsWinRT="using:Microsoft.Toolkit.Uwp.UI.Controls.WinRT"/>

	 <!-- Renders "Hello World" oriented vertically -->
	<controlsWinRT:DirectWriteTextBlock Text="Hello World" />
```

## Sample Output

## Properties

| Property | Type | Description |
| -- | -- | -- |
| Text | String | The text to render using DirectWrite |
| TextLocale | String | The bcp-47 text locale tag to pass to DirectWrite, en-US by default |
| TextOrientation | String | The orientation of the text, Vertical by default |
| TextWrap | [Windows.UI.Xaml.TextWrapping](https://docs.microsoft.com/uwp/api/windows.ui.xaml.textwrapping) | How to wrap the text, NoWrap by default |
| FlowDirection | [Windows.UI.Xaml.FlowDirection](https://docs.microsoft.com/uwp/api/windows.ui.xaml.flowdirection) | Inherited from [Windows.UI.Xaml.Controls.Control](https://docs.microsoft.com/uwp/api/windows.ui.xaml.controls.control). the flow direction of the text, LeftToRight by default |
| Foreground | [Windows.UI.Xaml.Media.Brush](https://docs.microsoft.com/en-us/uwp/api/windows.ui.xaml.media.brush) | Inherited from [Windows.UI.Xaml.Controls.Control](https://docs.microsoft.com/uwp/api/windows.ui.xaml.controls.control). The font color of the text to render, this only supports [SolidColorBrush](https://docs.microsoft.com/uwp/api/windows.ui.xaml.media.solidcolorbrush) default is [Windows.UI.Colors.Black](https://docs.microsoft.com/en-us/uwp/api/windows.ui.colors.black#Windows_UI_Colors_Black) . |
| FontSize | double | Inherited from [Windows.UI.Xaml.Controls.Control](https://docs.microsoft.com/uwp/api/windows.ui.xaml.controls.control). The font size of the text to render, 15 by default |
| FontFamily | [Windows.UI.Xaml.Media.FontFamily](https://docs.microsoft.com/uwp/api/windows.ui.xaml.media.fontfamily) | Inherited from [Windows.UI.Xaml.Controls.Control](https://docs.microsoft.com/uwp/api/windows.ui.xaml.controls.control). The font family of the text to render, Segoe UI by default. This supports custom fonts with syntax like /Assets/FontFile.ttf#Font Name |
| FontStretch | [Windows.UI.Text.FontStretch](https://docs.microsoft.com/uwp/api/windows.ui.text.fontstretch) | Inherited from [Windows.UI.Xaml.Controls.Control](https://docs.microsoft.com/uwp/api/windows.ui.xaml.controls.control). The font stretch of the text to render, Normal by default |
| FontStyle | [Windows.UI.Text.FontStyle](https://docs.microsoft.com/uwp/api/windows.ui.text.fontstyle) | Inherited from [Windows.UI.Xaml.Controls.Control](https://docs.microsoft.com/uwp/api/windows.ui.xaml.controls.control). The font stretch of the text to render, Normal by default |
| FontWeight | [Windows.UI.Text.FontWeight](https://docs.microsoft.com/uwp/api/windows.ui.text.fontweight) | Inherited from [Windows.UI.Xaml.Controls.Control](https://docs.microsoft.com/uwp/api/windows.ui.xaml.controls.control). The font stretch of the text to render, Normal by default |

## Sample Code

## Requirements

| Device family | Universal, 10.0.17134.0 or higher |
| -- | -- |
| Namespace | Microsoft.Toolkit.Uwp.UI.Controls.WinRT |
| NuGet package | [Microsoft.Toolkit.Uwp.UI.Controls.WinRT](https://www.nuget.org/packages/Microsoft.Toolkit.Uwp.UI.Controls.WinRT/) |

## API Source Code

* [DirectWriteTextBlock source code in C++/WinRT](https://github.com/Microsoft/WindowsCommunityToolkit//tree/master/Microsoft.Toolkit.Uwp.UI.Controls.WinRT)
