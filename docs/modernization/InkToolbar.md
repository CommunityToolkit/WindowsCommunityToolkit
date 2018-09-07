---
title: InkToolbar control for Windows Forms and WPF
author: granitestatehacker
description: This control is a wrapper to enable use of the UWP InkToolbar control in Windows Forms or WPF.
keywords: windows 10, uwp, windows community toolkit, uwp community toolkit, uwp toolkit, InkToolbar, Windows Forms, WPF
---

# InkToolbar controls for Windows Forms and WPF applications

The **InkToolbar** controls provide a common interface to manage an InkCanvas for Ink-based user interaction in your Windows Forms or WPF desktop application.

![Web View Samples](../resources/images/Controls/InkCanvas.png)

These controls use the Windows 10 implementation, and are used to embed a panel that can be drawn on, in Ink-style user interaction.  

## About InkToolbar controls

The Windows Forms version of this control is coming soon. It will be located in the **Microsoft.Toolkit.Win32.UI.Controls.WinForms** namespace. 

The WPF version is located in the **Microsoft.Toolkit.Wpf.UI.Controls** namespace. 

You can find additional related types (such as event args classes) in the **Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT** namespace.

## Known Limitations
This control provides little interaction other than 

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
| static ActiveToolProperty | DependencyProperty | DependencyProperty for ActiveTool Property |
| static InitialControlsProperty | DependencyProperty | DependencyProperty for InitialControls Property |
| static InkDrawingAttributesProperty | DependencyProperty | DependencyProperty for InkDrawingAttributes Property |
| static IsRulerButtonCheckedProperty | DependencyProperty | DependencyProperty for IsRulerButtonChecked Property |
| static TargetInkCanvasProperty | DependencyProperty | DependencyProperty for TargetInkCanvas Property |
| static ButtonFlyoutPlacementProperty | DependencyProperty | DependencyProperty for ButtonFlyoutPlacement Property |
| static IsStencilButtonCheckedProperty | DependencyProperty | DependencyProperty for IsStencilButtonChecked Property |
| static OreentationProperty | DependencyProperty | DependencyProperty for Orientation Property |
| ActiveTool | WindowsXamlHostBaseExt | Gets or sets the ActiveTool as a wrapped control. |
| InkDrawingAttributes | InkDrawingAttributes | Wrapper for Windows.UI.Input.Inking.InkDrawingAttributes |
| Orientation | Orientation | wrapper for Windows.UI.Xaml.Controls.InkToolbar.Orientation |
| IsStencilButtonChecked | bool | True if a stencil button is checked | 
| ButtonFlyoutPlacement | InkToolbarButtonFlyoutPlacement | Windows.UI.Xaml.Controls.InkToolbar.ButtonFlyoutPlacement |
| Children | ObservableCollection<DependencyObject> | Child controls |


## Methods


| Methods | Return Type | Description |
| -- | -- | -- |

## Events

| Events | Description |
| -- | -- |
| ActiveToolChanged | Fires when a different active tool has been selected by the user |
| EraseAllClicked | Fires when the user has clicked Erase All |
| IsRulerButtonCheckedChanged | Fires when the state of IsRulerButtonChecked has changed |
| IsStencilButtonCheckedChanged | Fires when the state of IsStencilButtonChecked has changed |



## Requirements

| Device family | .NET 4.6.2, Windows 10 (introduced v10.0.17110.0) |
| -- | -- |
| Namespace | Microsoft.Toolkit.Win32.UI.Controls.WinForms, Microsoft.Toolkit.Wpf.UI.Controls |
| NuGet package | [Microsoft.Toolkit.Win32.UI.Controls](https://www.nuget.org/packages/Microsoft.Toolkit.Win32.UI.Controls/) |

## API Source Code

- [WinForms.InkToolbar](https://github.com/Microsoft/WindowsCommunityToolkit/tree/master/Microsoft.Toolkit.Win32/Microsoft.Toolkit.Win32.UI.Controls/WinForms/InkToolbar)
- [WPF.InkToolbar](https://github.com/Microsoft/WindowsCommunityToolkit/tree/master/Microsoft.Toolkit.Win32/Microsoft.Toolkit.Win32.UI.Controls/WPF/InkToolbar)


## Related Topics

- [InkToolbar (UWP)](https://docs.microsoft.com/en-us/uwp/api/Windows.UI.Xaml.Controls.InkToolbar)
