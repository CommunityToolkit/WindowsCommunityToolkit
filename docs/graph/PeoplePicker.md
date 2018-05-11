---
title: PeoplePicker Control
author: OGcanviz
description: The PeoplePicker Control is a simple control that allows for selection of one or more users from an organizational AD.
keywords: windows 10, uwp, uwp community toolkit, uwp toolkit, PeoplePicker Control
---

# PeoplePicker Control

The [PeoplePicker Control](https://docs.microsoft.com/dotnet/api/microsoft.toolkit.uwp.ui.controls.graph.peoplepicker) is a simple control that allows for selection of one or more users from an organizational AD, see more details [here](https://developer.microsoft.com/en-us/graph/docs/concepts/people_example), it relies on the [AadAuthenticationManager](../../docs/graph/AadAuthenticationManager.md) for authentication.

## Syntax

```xaml
<Page ...
    xmlns:controls="using:Microsoft.Toolkit.Uwp.UI.Controls.Graph"/>

<controls:PeoplePicker x:Name="PeoplePicker1"
    AllowMultiple="True" />
```

## Example Image

![PeoplePicker animation](../resources/images/Graph/PeoplePicker.png)

## Properties

| Property | Type | Description |
| -- | -- | -- |
| RequiredDelegatedPermissions | String[] | Gets required delegated permissions for Graph API access |
| AllowMultiple | Boolean | Whether multiple people can be selected |
| SearchResultLimit | Int | Max person returned in the search results |
| PlaceholderText | String | Text to be displayed when no user is selected |
| Selections | ObservableCollection<Person> | The selected person list |

## Sample Code

[PeoplePicker Sample Page Source](../../Microsoft.Toolkit.Uwp.SampleApp/SamplePages/PeoplePicker). You can see this in action in [UWP Community Toolkit Sample App](https://www.microsoft.com/store/apps/9NBLGGH4TLCQ).

## Default Template 

[PeoplePicker XAML File](../../Microsoft.Toolkit.Uwp.UI.Controls/Graph/PeoplePicker/PeoplePicker.xaml) is the XAML template used in the toolkit for the default styling.

## Requirements

| Device family | Universal, 10.0.14393.0 or higher |
| -- | -- |
| Namespace | Microsoft.Toolkit.Uwp.UI.Controls.Graph |
| NuGet package | [Microsoft.Toolkit.Uwp.UI.Controls.Graph](https://www.nuget.org/packages/Microsoft.Toolkit.Uwp.UI.Controls/) |

## API

* [PeoplePicker source code](../../Microsoft.Toolkit.Uwp.UI.Controls/Graph/PeoplePicker)
