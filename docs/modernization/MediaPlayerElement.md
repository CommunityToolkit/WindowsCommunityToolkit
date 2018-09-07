---
title: MediaPlayerElement control for Windows Forms and WPF
author: granitestatehacker
description: This control is a wrapper to enable use of the UWP MediaPlayerElement control in Windows Forms or WPF.
keywords: windows 10, uwp, windows community toolkit, uwp community toolkit, uwp toolkit, MediaPlayerElement, Windows Forms, WPF
---

# MediaPlayerElement controls for Windows Forms and WPF applications

The **MediaPlayerElement** controls show media content in your Windows Forms or WPF desktop application.

![Web View Samples](../resources/images/Controls/MediaPlayerElement.png)

These controls use the newer Windows 10 implementation, and is used to embed a view that streams and renders media content such as video.  

## About MediaPlayerElement controls

The Windows Forms version of this control is coming soon. It will be located in the **Microsoft.Toolkit.Forms.UI.Controls** namespace. 

The WPF version is located in the **Microsoft.Toolkit.Wpf.UI.Controls** namespace. 

You can find additional related types (such as event args classes) in the **Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT** namespace.

## Known Limitations
This wrapper does not currently support Full Screen video.  The Source property is exposed as a string, which is interpreted as a URL and bound to the UWP Source property as a UWP-implemented IMediaPlaybackSource.

## Syntax
```xaml
<Window x:Class="TestSample.MainWindow" ...
  xmlns:controls="clr-namespace:Microsoft.Toolkit.Wpf.UI.Controls;assembly=Microsoft.Toolkit.Wpf.UI.Controls"
...>

<controls:MediaPlayerElement x:Name="mediaPlayerElement" 
    Source="https://mediaplatstorage1.blob.core.windows.net/windows-universal-samples-media/elephantsdream-clip-h264_sd-aac_eng-aac_spa-aac_eng_commentary-srt_eng-srt_por-srt_swe.mkv"
    AutoPlay="True" Margin="5" HorizontalAlignment="Stretch"  VerticalAlignment="Stretch" AreTransportControlsEnabled="True" />
```

## Properties

| Property | Type | Description |
| -- | -- | -- |
| TransportControls | Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT.MediaTransportControls | Wrapper for Windows.UI.Xaml.Controls.MediaTransportControls |
| Stretch | Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT.Stretch | Wrapper for Windows.UI.Xaml.Media.Stretch |
| Source | string | Url for media to present. |
| PosterSource | Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT | Wrapper for Windows.UI.Xaml.Media.ImageSource |
| AutoPlay | bool | Gets or sets if the media should start immediately on initialization or not. |
| AreTransportControlsEnabled | bool | Gets or sets if the media control (pause, play, et al) should be shown. |
| MediaPlayer | Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT.MediaPlayer | Wrapper for Windows.Media.Playback.MediaPlayer || static AreTransportControlsEnabledProperty | DependencyProperty | DependencyProperty for AreTransportControlsEnabled |
| static AutoPlayProperty | DependencyProperty | DependencyProperty for AutoPlay property |
| static IsFullWindowProperty | DependencyProperty | DependencyProperty for IsFullWindow property |
| static MediaPlayerProperty | DependencyProperty | DependencyProperty for MediaPlayer property |
| static PosterSourceProperty | DependencyProperty | DependencyProperty for PosterSource property |
| static SourceProperty | DependencyProperty | DependencyProperty for Source property |
| static StretchProperty | DependencyProperty | DependencyProperty for Stretch property |

## Methods


| Methods | Return Type | Description |
| -- | -- | -- |
| SetMediaPlayer(MediaPlayer) | void | Allows the  Windows.Media.Playback.MediaPlayer media player to be set on the underlying UWP control. |

## Events

| Events | Description |
| -- | -- |


## Requirements

| Device family | .NET 4.6.2, Windows 10 (introduced v10.0.17110.0) |
| -- | -- |
| Namespace | Microsoft.Toolkit.Forms.UI.Controls, Microsoft.Toolkit.Wpf.UI.Controls |
| NuGet package | [Microsoft.Toolkit.Win32.UI.Controls](https://www.nuget.org/packages/Microsoft.Toolkit.Win32.UI.Controls/) |

## API Source Code

- [WinForms.MediaPlayerElement](https://github.com/Microsoft/WindowsCommunityToolkit/tree/master/Microsoft.Toolkit.Win32/Microsoft.Toolkit.Win32.UI.Controls/WinForms/MediaPlayerElement)
- [WPF.MediaPlayerElement](https://github.com/Microsoft/WindowsCommunityToolkit/tree/master/Microsoft.Toolkit.Win32/Microsoft.Toolkit.Win32.UI.Controls/WPF/MediaPlayerElement)


## Related Topics

- [MediaPlayerElement (UWP)](https://docs.microsoft.com/en-us/uwp/api/Windows.UI.Xaml.Controls.MediaPlayerElement)
