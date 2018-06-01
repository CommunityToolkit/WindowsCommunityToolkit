---
title: Theme Listener
author: williamabradley
description: The Theme Listener allows you to determine the current Application Theme, and when it is changed via System Theme changes.
keywords: windows 10, uwp, windows community toolkit, uwp community toolkit, uwp toolkit, theme listener, themeing, themes, system theme, helpers
dev_langs:
  - csharp
  - vb
---

# Theme Listener

The [Theme Listener](https://docs.microsoft.com/en-us/dotnet/api/microsoft.toolkit.uwp.ui.themelistener) class allows you to determine the current Application Theme, and when it is changed via System Theme changes.

## Syntax

```csharp
var Listener = new ThemeListener();
Listener.ThemeChanged += Listener_ThemeChanged;

private void Listener_ThemeChanged(ThemeListener sender)
{
    var theme = sender.CurrentTheme;
    // Use theme dependent code.
}
```
```vb
Dim listener = New ThemeListener()
AddHandler listener.ThemeChanged, AddressOf Listener_ThemeChanged

Private Sub Listener_ThemeChanged(ByVal sender As ThemeListener)
    Dim theme = sender.CurrentTheme
    ' Use theme dependent code.
End Sub
```

## Properties

| Property | Type | Description |
| -- | -- | -- |
| CurrentTheme | [ApplicationTheme](https://docs.microsoft.com/uwp/api/Windows.UI.Xaml.ApplicationTheme) | Gets or sets the Current Theme. |
| CurrentThemeName | string | Gets the Name of the Current Theme. |
| IsHighContrast | bool | Gets or sets a value indicating whether the current theme is high contrast. |

## Events

| Events | Description |
| -- | -- |
| ThemeChanged | An event that fires if the Theme changes. |

## Sample Code

[Theme Listener Sample Page Source](https://github.com/Microsoft/UWPCommunityToolkit/blob/master/Microsoft.Toolkit.Uwp.SampleApp/SamplePages/ThemeListener/ThemeListenerPage.xaml.cs).

You can see this in action in [Windows Community Toolkit Sample App](https://www.microsoft.com/store/apps/9NBLGGH4TLCQ).

## Requirements

| Device family | Universal, 10.0.14393.0 or higher |
| --- | --- |
| Namespace | Microsoft.Toolkit.Uwp.UI |
| NuGet package | [Microsoft.Toolkit.Uwp.UI](https://www.nuget.org/packages/Microsoft.Toolkit.Uwp.UI/)  |

## API Source Code

- [Theme Listener source code](https://github.com/Microsoft/UWPCommunityToolkit/tree/master/Microsoft.Toolkit.Uwp.UI/Helpers/ThemeListener.cs)