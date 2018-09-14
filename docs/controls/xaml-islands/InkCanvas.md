---
title: InkCanvas control for Windows Forms and WPF
author: granitestatehacker
description: This control is a wrapper to enable use of the UWP InkCanvas control in Windows Forms or WPF.
keywords: windows 10, uwp, windows community toolkit, uwp community toolkit, uwp toolkit, InkCanvas, Windows Forms, WPF
---

# InkCanvas controls for Windows Forms and WPF applications

The **InkCanvas** controls provide a surface for Ink-based user interaction in your Windows Forms or WPF desktop application.

![Web View Samples](../resources/images/Controls/InkCanvas.png)

These controls use the Windows 10 implementation, and are used to embed a panel that can be drawn on, in Ink-style user interaction.  

## About InkCanvas controls

The Windows Forms version of this control is coming soon. It will be located in the **Microsoft.Toolkit.Forms.UI.Controls** namespace.

The WPF version is located in the **Microsoft.Toolkit.Wpf.UI.Controls** namespace.

You can find additional related types (such as event args classes) in the **Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT** namespace.

## Known Limitations
These controls, like the UWP base, provides no interaction without an associated InkToolbar with the interaction mode set.  You'll also find it may not show ink properly while running on a client that's in Windows 10 dark theme.

## Syntax
```xaml
<Window x:Class="TestSample.MainWindow" ...
  xmlns:controls="clr-namespace:Microsoft.Toolkit.Wpf.UI.Controls;assembly=Microsoft.Toolkit.Wpf.UI.Controls"
...>


<controls:InkCanvas x:Name="inkCanvas" DockPanel.Dock="Top" Loaded="inkCanvas_Loaded"/>
```

## Properties

| Property | Type | Description |
| -- | -- | -- |
| InkPresenter | Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT.InkPresenter | Wrapper for Windows.UI.Input.Inking.InkPresenter |

## Methods


| Methods | Return Type | Description |
| -- | -- | -- |

## Events

| Events | Description |
| -- | -- |


## Requirements

| Device family | .NET 4.6.2, Windows 10 (introduced v10.0.17110.0) |
| -- | -- |
| Namespace | Microsoft.Toolkit.Forms.UI.Controls, Microsoft.Toolkit.Wpf.UI.Controls |
| NuGet package | [Microsoft.Toolkit.Win32.UI.Controls](https://www.nuget.org/packages/Microsoft.Toolkit.Win32.UI.Controls/) |

## API Source Code

- [InkCanvas (Windows Forms)](https://github.com/Microsoft/WindowsCommunityToolkit/tree/master/Microsoft.Toolkit.Win32/Microsoft.Toolkit.Forms.UI.Controls/InkCanvas)
- [InkCanvas (WPF)](https://github.com/Microsoft/WindowsCommunityToolkit/tree/master/Microsoft.Toolkit.Win32/Microsoft.Toolkit.WPF.UI.Controls/InkCanvas)


## Related Topics

- [InkCanvas (UWP)](https://docs.microsoft.com/en-us/uwp/api/Windows.UI.Xaml.Controls.InkCanvas)
