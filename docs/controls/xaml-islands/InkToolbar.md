---
title: InkToolbar control for Windows Forms and WPF
author: granitestatehacker
description: This control is a wrapper to enable use of the UWP InkToolbar control in Windows Forms or WPF.
keywords: windows 10, uwp, windows community toolkit, uwp community toolkit, uwp toolkit, InkToolbar, Windows Forms, WPF
---

# InkToolbar controls for Windows Forms and WPF applications

The **InkToolbar** control provides an interface to manage an [InkCanvas](InkCanvas.md) for Windows Ink-based user interaction in your Windows Forms or WPF desktop application.

![InkToolbar example](../../resources/images/Controls/InkCanvas.png)

## About InkToolbar control

The WPF version of this control is located in the **Microsoft.Toolkit.Wpf.UI.Controls** namespace. The Windows Forms version is coming soon, and it will be located in the **Microsoft.Toolkit.Forms.UI.Controls** namespace. You can find additional related types (such as event args classes) in the **Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT** namespace.

Internally, these controls wrap the UWP [Windows.UI.Xaml.Controls.InkToolbar](https://docs.microsoft.com/uwp/api/Windows.UI.Xaml.Controls.InkToolbar) class.

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
| static ActiveToolProperty | DependencyProperty | Dependency property for the **ActiveTool** property. |
| static InitialControlsProperty | DependencyProperty | Dependency property for the **InitialControls** property. |
| static InkDrawingAttributesProperty | DependencyProperty | Dependency property for the **InkDrawingAttributes** property. |
| static IsRulerButtonCheckedProperty | DependencyProperty | Dependency property for the **IsRulerButtonChecked** property. |
| static TargetInkCanvasProperty | DependencyProperty | Dependency property for the **TargetInkCanvas** property. |
| static ButtonFlyoutPlacementProperty | DependencyProperty | Dependency property for the **ButtonFlyoutPlacement** property. |
| static IsStencilButtonCheckedProperty | DependencyProperty | Dependency property for the **IsStencilButtonChecked** property. |
| static OrientationProperty | DependencyProperty | Dependency property for the **Orientation** property. |
| ActiveTool | WindowsXamlHostBaseExt | Wraps the [ActiveTool](https://docs.microsoft.com/uwp/api/windows.ui.xaml.controls.inktoolbar.activetool) property of the internal UWP **InkToolbar** control. |
| InkDrawingAttributes | InkDrawingAttributes | Wraps the [InkDrawingAttributes](https://docs.microsoft.com/uwp/api/windows.ui.xaml.controls.inktoolbar.inkdrawingattributes) property of the internal UWP **InkToolbar** control.  |
| Orientation | Orientation | Wraps the [Orientation](https://docs.microsoft.com/uwp/api/windows.ui.xaml.controls.inktoolbar.orientation) property of the internal UWP **InkToolbar** control. |
| IsStencilButtonChecked | bool | Wraps the [IsStencilButtonChecked](https://docs.microsoft.com/uwp/api/windows.ui.xaml.controls.inktoolbar.isstencilbuttonchecked) property of the internal UWP **InkToolbar** control. |
| ButtonFlyoutPlacement | InkToolbarButtonFlyoutPlacement | Wraps the [ButtonFlyoutPlacement](https://docs.microsoft.com/uwp/api/windows.ui.xaml.controls.inktoolbar.buttonflyoutplacement) property of the internal UWP **InkToolbar** control. |
| Children | ObservableCollection<DependencyObject> | Wraps the [Children](https://docs.microsoft.com/uwp/api/windows.ui.xaml.controls.inktoolbar.children) property of the internal UWP **InkToolbar** control. |


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
