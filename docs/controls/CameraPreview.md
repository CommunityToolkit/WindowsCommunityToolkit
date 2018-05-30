---
title: CameraPreview
author: skommireddi
description: The CameraPreview control allows to easily preview video in the MediaPlayerElement from available camera frame source groups. You can subscribe and get real time video frames and software bitmaps as they arrive from the selected camera source. It shows only frame sources that support color video preview or video record streams.
keywords: windows 10, uwp, windows community toolkit, windows toolkit, CameraPreview, Camera, Preview, Video Frame, Software Bitmap
---

# CameraPreview

The **CameraPreview** control allows to easily preview video in the MediaPlayerElement from available camera frame source groups. You can subscribe and get real time video frames and software bitmaps as they arrive from the selected camera source. It shows only frame sources that support color video preview or video record streams.

> [!IMPORTANT]
Make sure you have the [webcam capability](https://docs.microsoft.com/en-us/windows/uwp/packaging/app-capability-declarations#device-capabilities) enabled for your app to access the device's camera.

## Syntax

```xaml
<controls:CameraPreview x:Name="CameraPreviewControl">	
</controls:CameraPreview>       
```

```csharp

CameraPreviewControl.PreviewFailed += CameraPreviewControl_PreviewFailed;
await CameraPreviewControl.StartAsync();
CameraPreviewControl.CameraHelper.FrameArrived += CameraPreviewControl_FrameArrived;
     

private void CameraPreviewControl_FrameArrived(object sender, FrameEventArgs e)
{
	var videoFrame = e.VideoFrame;
	var softwareBitmap = videoFrame.SoftwareBitmap;
}

private void CameraPreviewControl_PreviewFailed(object sender, PreviewFailedEventArgs e)
{
	var errorMessage = e.Error;
}
```

> [!IMPORTANT]
As a developer, you will need to make sure the CameraHelper resources used by the control are cleaned up when appropriate. See [CameraHelper documentation](../helpers/CameraHelper.md) for more details

## Properties

| Property | Type | Description |
| -- | -- | -- |
| CameraHelper| [CameraHelper](../helpers/CameraHelper.md) | Gets the CameraHelper associated with the control. |
| IsFrameSourceGroupButtonVisible | bool| Set this property to hide or show Frame Source Group Button. Note: This button is conditionally visible based on more than one source being available. |

```xaml
<controls:CameraPreview x:Name="CameraPreviewControl" IsFrameSourceGroupButtonVisible="false">	
</controls:CameraPreview>       
``` 

## Methods

| Methods | Return Type | Description |
| -- | -- | -- |
| StartAsync() | Task | Initializes camera preview control with a default Camera Helper instance and starts preview and frame capture. |
| StartAsync(CameraHelper cameraHelper) | Task | Initializes camera preview control with provided Camera Helper instance. |
| Stop() | void | Stops camera preview and disposes MediaPlayer. |

## Events

| Events | Description |
| -- | -- |
| PreviewFailed | Fires when camera preview fails. You can get the error reason from the PreviewFailedEventArgs.|

## Examples

Demonstrates using the camera control and camera helper to preview video from a specific media frame source group.

```csharp
var availableFrameSourceGroups = = await CameraHelper.GetFrameSourceGroupsAsync();
if(availableFrameSourceGroups != null)
{
  CameraHelper cameraHelper = new CameraHelper() { FrameSourceGroup = availableFrameSourceGroups.FirstOrDefault() };
  _cameraPreviewControl.PreviewFailed += CameraPreviewControl_PreviewFailed;
  await _cameraPreviewControl.StartAsync(cameraHelper);
  _cameraPreviewControl.CameraHelper.FrameArrived += CameraPreviewControl_FrameArrived; 
}
```

## Sample Code

[CameraPreview Sample Page Source](https://github.com/Microsoft/WindowsCommunityToolkit//tree/master/Microsoft.Toolkit.Uwp.SampleApp/SamplePages/CameraPreview). You can see this in action in [Windows Community Toolkit Sample App](https://www.microsoft.com/store/apps/9NBLGGH4TLCQ).


## Requirements

| [Device family] | Universal, 10.0.17134.0 or higher |
| --- | --- |
| Namespace | Microsoft.Toolkit.Uwp.UI.Controls |
| NuGet package | [Microsoft.Toolkit.Uwp.UI.Controls](https://www.nuget.org/packages/Microsoft.Toolkit.Uwp.UI.Controls/) |

## API Source Code

- [CameraPreview source code](https://github.com/Microsoft/WindowsCommunityToolkit//blob/master/Microsoft.Toolkit.Uwp.UI.Controls/CameraPreview)


