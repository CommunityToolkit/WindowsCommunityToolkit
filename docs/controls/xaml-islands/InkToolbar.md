---
title: InkToolbar control for Windows Forms and WPF
author: granitestatehacker
description: This control is a wrapper to enable use of the UWP InkToolbar control in Windows Forms or WPF.
keywords: windows 10, uwp, windows community toolkit, uwp community toolkit, uwp toolkit, InkToolbar, Windows Forms, WPF
---

# InkToolbar control for Windows Forms and WPF

The **InkToolbar** control provides an interface to manage an [InkCanvas](InkCanvas.md) for Windows Ink-based user interaction in your Windows Forms or WPF desktop application.

![InkToolbar example](../../resources/images/Controls/InkCanvas.png)

## About InkToolbar control

The WPF version of this control is located in the **Microsoft.Toolkit.Wpf.UI.Controls** namespace. The Windows Forms version is coming soon, and it will be located in the **Microsoft.Toolkit.Forms.UI.Controls** namespace. You can find additional related types (such as event args classes) in the **Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT** namespace.

Internally, this control wraps the UWP [Windows.UI.Xaml.Controls.InkToolbar](https://docs.microsoft.com/uwp/api/Windows.UI.Xaml.Controls.InkToolbar) control.

## Syntax
```xaml
<Window x:Class="TestSample.MainWindow" ...
  xmlns:controls="clr-namespace:Microsoft.Toolkit.Wpf.UI.Controls;assembly=Microsoft.Toolkit.Wpf.UI.Controls"
...>


<controls:InkToolbar  DockPanel.Dock="Top" x:Name="inkToolbar" Grid.Row="0" TargetInkCanvas="{x:Reference Name=inkCanvas}"    
      Initialized="inkToolbar_Initialized" ActiveToolChanged="inkToolbar_ActiveToolChanged"
      InkDrawingAttributesChanged="inkToolbar_InkDrawingAttributesChanged"
      IsStencilButtonCheckedChanged="inkToolbar_IsStencilButtonCheckedChanged"  >
    <controls:InkToolbarCustomToolButton x:Name="toolButtonLasso" />
</controls:InkToolbar>
```

## Properties

| Property | Type | Description |
| -- | -- | -- |
| ActiveTool | Microsoft.Toolkit.Wpf.UI.Controls.WindowsXamlHostBaseExt (WPF)<br/>Microsoft.Toolkit.Forms.UI.Controls.WindowsXamlHostBaseExt (Windows Forms) | Wraps the [ActiveTool](https://docs.microsoft.com/uwp/api/windows.ui.xaml.controls.inktoolbar.activetool) property of the internal UWP **InkToolbar** control. |
| ActiveToolProperty | DependencyProperty | Dependency property for the **ActiveTool** property. |
| ButtonFlyoutPlacement | Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT.InkToolbarButtonFlyoutPlacement | Wraps the [ButtonFlyoutPlacement](https://docs.microsoft.com/uwp/api/windows.ui.xaml.controls.inktoolbar.buttonflyoutplacement) property of the internal UWP **InkToolbar** control. |
| ButtonFlyoutPlacementProperty | DependencyProperty | Dependency property for the **ButtonFlyoutPlacement** property. |
| Children | ObservableCollection<DependencyObject> | Wraps the [Children](https://docs.microsoft.com/uwp/api/windows.ui.xaml.controls.inktoolbar.children) property of the internal UWP **InkToolbar** control. |
| InitialControls | Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT.InkToolbarInitialControls  | Wraps the [InitialControls](https://docs.microsoft.com/uwp/api/windows.ui.xaml.controls.inktoolbar.initialcontrols) property of the internal UWP **InkToolbar** control. |
| InitialControlsProperty | DependencyProperty | Dependency property for the **InitialControls** property. |
| InkDrawingAttributes | Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT.InkDrawingAttributes | Wraps the [InkDrawingAttributes](https://docs.microsoft.com/uwp/api/windows.ui.xaml.controls.inktoolbar.inkdrawingattributes) property of the internal UWP **InkToolbar** control.  |
| InkDrawingAttributesProperty | DependencyProperty | Dependency property for the **InkDrawingAttributes** property. |
| IsRulerButtonChecked | bool | Wraps the [IsRulerButtonChecked](https://docs.microsoft.com/uwp/api/windows.ui.xaml.controls.inktoolbar.isrulerbuttonchecked) property of the internal UWP **InkToolbar** control. |
| IsRulerButtonCheckedProperty | DependencyProperty | Dependency property for the **IsRulerButtonChecked** property. |
| IsStencilButtonChecked | bool | Wraps the [IsStencilButtonChecked](https://docs.microsoft.com/uwp/api/windows.ui.xaml.controls.inktoolbar.isstencilbuttonchecked) property of the internal UWP **InkToolbar** control. |
| IsStencilButtonCheckedProperty | DependencyProperty | Dependency property for the **IsStencilButtonChecked** property. |
| Orientation | Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT.Orientation | Wraps the [Orientation](https://docs.microsoft.com/uwp/api/windows.ui.xaml.controls.inktoolbar.orientation) property of the internal UWP **InkToolbar** control. |
| OrientationProperty | DependencyProperty | Dependency property for the **Orientation** property. |
| TargetInkCanvas | Microsoft.Toolkit.Wpf.UI.Controls.InkCanvas (WPF)<br/>Microsoft.Toolkit.Forms.UI.Controls.InkCanvas (Windows Forms) | Wraps the [TargetInkCanvas](https://docs.microsoft.com/uwp/api/windows.ui.xaml.controls.inktoolbar.inkcanvas) property of the internal UWP **InkToolbar** control. |
| TargetInkCanvasProperty | DependencyProperty | Dependency property for the **TargetInkCanvas** property. |

## Events

| Events | Description |
| -- | -- |
| ActiveToolChanged | Wraps the [ActiveToolChanged](https://docs.microsoft.com/uwp/api/windows.ui.xaml.controls.inktoolbar.activetoolchanged) event of the internal UWP **InkToolbar** control. |
| EraseAllClicked | Wraps the [EraseAllClicked](https://docs.microsoft.com/uwp/api/windows.ui.xaml.controls.inktoolbar.eraseallclicked) event of the internal UWP **InkToolbar** control. |
| IsRulerButtonCheckedChanged | Wraps the [IsRulerButtonCheckedChanged](https://docs.microsoft.com/uwp/api/windows.ui.xaml.controls.inktoolbar.isrulerbuttoncheckedchanged) event of the internal UWP **InkToolbar** control. |
| IsStencilButtonCheckedChanged | Wraps the [IsStencilButtonCheckedChanged](https://docs.microsoft.com/uwp/api/windows.ui.xaml.controls.inktoolbar.isstencilbuttoncheckedchanged) event of the internal UWP **InkToolbar** control. |



## Requirements

| Device family | .NET 4.6.2, Windows 10 (introduced v10.0.17110.0) |
| -- | -- |
| Namespace | Microsoft.Toolkit.Forms.UI.Controls, Microsoft.Toolkit.Wpf.UI.Controls |
| NuGet package | [Microsoft.Toolkit.Win32.UI.Controls](https://www.nuget.org/packages/Microsoft.Toolkit.Win32.UI.Controls/) |

## API Source Code

- [InkToolbar (Windows Forms)](https://github.com/Microsoft/WindowsCommunityToolkit/tree/master/Microsoft.Toolkit.Win32/Microsoft.Toolkit.Forms.UI.Controls/InkToolbar)
- [InkToolbar (WPF)](https://github.com/Microsoft/WindowsCommunityToolkit/tree/master/Microsoft.Toolkit.Win32/Microsoft.Toolkit.WPF.UI.Controls/InkToolbar)


## Related Topics

- [InkToolbar (UWP)](https://docs.microsoft.com/en-us/uwp/api/Windows.UI.Xaml.Controls.InkToolbar)
- [Pen and Windows Ink](https://docs.microsoft.com/windows/uwp/design/input/pen-and-stylus-interactions)
