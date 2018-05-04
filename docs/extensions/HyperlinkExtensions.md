---
title: HyperlinkExtensions
author: nmetulev
description: The HyperlinkExtensions allows for a Hyperlink element to invoke the execute method on a bound ICommand instance when clicked.
keywords: windows 10, uwp, windows community toolkit, uwp community toolkit, uwp toolkit, Hyperlink, extensions
---

# HyperlinkExtensions

The [HyperlinkExtension](https://docs.microsoft.com/dotnet/api/microsoft.toolkit.uwp.ui.extensions.hyperlink) allows for a Hyperlink element to invoke the execute method on a bound [ICommand](https://docs.microsoft.com/uwp/api/Windows.UI.Xaml.Input.ICommand) instance when clicked.

## Example

```xaml
// Use Hyperlink in a wrapped TextBlock with text either side and ensure it executes a command when
// clicked passing the current data context as the command parameter.
<TextBlock>
	<Run>Some leading text with a</Run>
		<Hyperlink extensions:Hyperlink.Command="{Binding HyperlinkClicked}"
				extensions:Hyperlink.CommandParameter="{Binding}">hyperlink</Hyperlink>
	<Run>in the middle.</Run>
</TextBlock>
```

## Properties

| Property | Description |
| --| -- |
| Command | Attached `DependencyProperty` for binding an ICommand instance to a Hyperlink |
| CommandParameter | Attached `DependencyProperty` for binding a command parameter to a Hyperlink |

## Requirements

| Device family | Universal, 10.0.14393.0 or higher |
| --- | --- |
| Namespace | Microsoft.Toolkit.Uwp.Extensions |
| NuGet package | [Microsoft.Toolkit.Uwp.UI](https://www.nuget.org/packages/Microsoft.Toolkit.Uwp.UI/) |

## API

* [Hyperlink source code](https://github.com/Microsoft/UWPCommunityToolkit/blob/master/Microsoft.Toolkit.Uwp.UI/Extensions/Hyperlink)

