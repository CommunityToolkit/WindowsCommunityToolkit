---
title: CameraPreview
author: skommireddi
description: The **CameraPreview** control allows to preview video in the MediaPlayerElement from available camera frame source groups. You can subscribe to real time video frames and software bitmaps as they arrive from the selected camera source. It currently filters out frame sources that support color video preview or video record streams for preview.
keywords: windows 10, uwp, uwp community toolkit, uwp toolkit, CameraPreview, Camera, Preview, Video Frame, Software Bitmap
---

# CameraPreview

The **CameraPreview** control allows to easily preview video in the MediaPlayerElement from available camera frame source groups. You can subscribe and get real time video frames and software bitmaps as they arrive from the selected camera source. It currently filters frame sources that support color video preview or video record streams. 
 
## Syntax

```xaml
<controls:CameraPreview x:Name="CameraPreviewControl" 
                                     VideoFrameArrived="CameraPreviewControl_VideoFrameArrived"
                                     SoftwareBitmapArrived="CameraPreviewControl_SoftwareBitmapArrived">
</controls:CameraPreview>       
```

```csharp
private void CameraPreviewControl_SoftwareBitmapArrived(object sender, SoftwareBitmap e)
{
	var softwareBitmap = e;
}
        
private void CameraPreviewControl_VideoFrameArrived(object sender, VideoFrame e)
{
	var currentVideoFrame = e;
}
```

## Example

[CameraPreview Sample Page](https://github.com/Microsoft/UWPCommunityToolkit/tree/master/Microsoft.Toolkit.Uwp.SampleApp/SamplePages/CameraPreview)

## Requirements (Windows 10 Device Family)

| [Device family](http://go.microsoft.com/fwlink/p/?LinkID=526370) | Universal, 10.0.14393.0 or higher |
| --- | --- |
| Namespace | Microsoft.Toolkit.Uwp.UI.Controls |

## API

* [CameraPreview source code](https://github.com/Microsoft/UWPCommunityToolkit/blob/master/Microsoft.Toolkit.Uwp.UI.Controls/CameraPreview)


