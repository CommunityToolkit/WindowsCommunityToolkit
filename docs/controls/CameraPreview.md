---
title: CameraPreview
author: skommireddi
description: The CameraPreview control allows to easily preview video in the MediaPlayerElement from available camera frame source groups. You can subscribe and get real time video frames and software bitmaps as they arrive from the selected camera source. It shows only frame sources that support color video preview or video record streams.
keywords: windows 10, uwp, uwp community toolkit, uwp toolkit, CameraPreview, Camera, Preview, Video Frame, Software Bitmap
---

# CameraPreview

The **CameraPreview** control allows to easily preview video in the MediaPlayerElement from available camera frame source groups. You can subscribe and get real time video frames and software bitmaps as they arrive from the selected camera source. It shows only frame sources that support color video preview or video record streams.

> [!IMPORTANT] Make sure you have the webcam capability enabled for your app to access the device's camera.

## Syntax

```xaml
<controls:CameraPreview x:Name="CameraPreviewControl" 
	FrameSourceGroupButtonIcon="ms-appx:///Assets/Photos/CameraSource.png"
	FrameArrived="CameraPreviewControl_FrameArrived"
	PreviewFailed="CameraPreviewControl_PreviewFailed">
</controls:CameraPreview>       
```

```csharp
private void CameraPreviewControl_FrameArrived(object sender, FrameEventArgs e)
{
	var videoFrame = e.VideoFrame;
}

private void CameraPreviewControl_PreviewFailed(object sender, FailedEventArgs e)
{
	var errorMessage = e.Error;
}
```

## Properties

| Property | Type | Description |
| -- | -- | -- |
| FrameSourceGroups | IReadOnlyList<MediaFrameSourceGroup> | Gets a read only list of MediaFrameSourceGroups that support color video record or video preview streams. |
| FrameSourceGroupButtonIcon | ImageSource | You can customize the icon for Frame Source Group button. |


## Methods

| Methods | Return Type | Description |
| -- | -- | -- |
| Dispose() | void | Use this method to dispose the control and media resources. |

## Events

| Events | Description |
| -- | -- |
| FrameArrived | Fires when a new frame arrives.|
| PreviewFailed | Fires when camera preview fails.|

## Sample Code

[CameraPreview Sample Page Source](https://github.com/Microsoft/UWPCommunityToolkit/tree/master/Microsoft.Toolkit.Uwp.SampleApp/SamplePages/CameraPreview). You can see this in action in [UWP Community Toolkit Sample App](https://www.microsoft.com/store/apps/9NBLGGH4TLCQ).


## Requirements

| [Device family] | Universal, 10.0.14393.0 or higher |
| --- | --- |
| Namespace | Microsoft.Toolkit.Uwp.UI.Controls |
| NuGet package | [Microsoft.Toolkit.Uwp.UI.Controls](https://www.nuget.org/packages/Microsoft.Toolkit.Uwp.UI.Controls/) |

## API Source Code

- [CameraPreview source code](https://github.com/Microsoft/UWPCommunityToolkit/blob/master/Microsoft.Toolkit.Uwp.UI.Controls/CameraPreview)


