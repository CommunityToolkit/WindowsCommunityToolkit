---
title: SharePointFiles Control
author: OGcanviz
description: The SharePointFiles Control displays a simple list of SharePoint Files.
keywords: windows 10, uwp, uwp community toolkit, uwp toolkit, SharePointFiles Control
---

# SharePointFiles Control

The [SharePointFiles Control](https://docs.microsoft.com/dotnet/api/microsoft.toolkit.uwp.ui.controls.graph.sharepointfiles) displays a simple list of SharePoint Files, it relies on the [AadAuthenticationManager](../../docs/graph/AadAuthenticationManager.md) for authentication.

## Syntax

```xaml
<Page ...
    xmlns:controls="using:Microsoft.Toolkit.Uwp.UI.Controls.Graph"/>

<controls:SharePointFiles x:Name="SharePointFiles1"
    DetailPane="Side" />
```

## Example Image

![SharePointFiles animation](../resources/images/Graph/SharePointFileList.png)

## Properties

| Property | Type | Description |
| -- | -- | -- |
| RequiredDelegatedPermissions | String[] | Gets required delegated permissions for Graph API access |
| DriveUrl | String | Full URL of the Drive being displayed |
| DetailPane | [DetailPaneDisplayMode](../../Microsoft.Toolkit.Uwp.UI.Controls/Graph/SharePointFiles/DetailPaneDisplayMode.cs) | Determines whether file details are displayed, when a file is selected |
| PageSize | Int | Page size of each request |
| ShareLinkCopiedMessage | String | The message when share link copied |
| AllFilesMessage | String | The label of All Files |
| DeleteConfirmMessage | String | The message of delete confirm dialog |
| DeleteConfirmOkMessage | String | The caption of ok button in delete confirm dialog |
| DeleteConfirmCancelMessage | String | The caption of cancel button in delete confirm dialog |
| UploadingFilesMessageTemplate | String | The template of uploading files |

## Methods

| Method | Return Type | Description |
| -- | -- | -- |
| GetDriveUrlFromSharePointUrlAsync | String | Retrieves an appropriate Drive URL from a SharePoint document library root URL |

## Events

| Event | Return Type | Description |
| -- | -- | -- |
| FileSelected | EventHandler&lt;FileSelectedEventArgs&gt; | Occurs when one of the menu items in the control is clicked |

## Sample Code

[SharePointFiles Sample Page Source](../../Microsoft.Toolkit.Uwp.SampleApp/SamplePages/SharePointFiles). You can see this in action in [UWP Community Toolkit Sample App](https://www.microsoft.com/store/apps/9NBLGGH4TLCQ).

## Default Template 

[SharePointFiles XAML File](../../Microsoft.Toolkit.Uwp.UI.Controls/Graph/SharePointFiles/SharePointFiles.xaml) is the XAML template used in the toolkit for the default styling.

## Requirements

| Device family | Universal, 10.0.14393.0 or higher |
| -- | -- |
| Namespace | Microsoft.Toolkit.Uwp.UI.Controls.Graph |
| NuGet package | [Microsoft.Toolkit.Uwp.UI.Controls.Graph](https://www.nuget.org/packages/Microsoft.Toolkit.Uwp.UI.Controls/) |

## API

* [SharePointFiles source code](../../Microsoft.Toolkit.Uwp.UI.Controls/Graph/SharePointFiles)
