---
title: CameraHelper
author: skommireddi
description: The CameraHelper provides helper methods to easily use the available camera frame sources to preview video, capture video frames and software bitmaps.
keywords: windows 10, uwp, uwp community toolkit, uwp toolkit, CameraHelper, Camera, Frame Source, Video Frame, Software Bitmap
---

# CameraHelper

The **CameraHelper** provides helper methods to easily use the available camera frame sources to preview video, capture video frames and software bitmaps. The helper currently shows camera frame sources that support color video preview or video record streams. 

> [!IMPORTANT] Make sure you have the webcam capability enabled for your app to access the device's camera.

## Syntax

```csharp
// Creates a Camera Helper and gets video frames from an available frame source.
using Microsoft.Toolkit.Uwp.Helpers.CameraHelper;

CameraHelper cameraHelper = new CameraHelper();
var result = await _cameraHelper.InitializeAndStartCaptureAsync();

// Camera Initialization and Capture failed for some reason
if(result != CameraHelperResult.Success)
{
	// get error information
	var errorMessage = result.ToString();
}

// Subscribe to get frames as they arrive
cameraHelper.FrameArrived += CameraHelper_FrameArrived;

private void CameraHelper_FrameArrived(object sender, FrameEventArgs e)
{
	// Gets the current video frame
	VideoFrame currentVideoFrame  = e.VideoFrame;
}
```

## Properties

| Property | Type | Description |
| -- | -- | -- |
| FrameSource| MediaFrameSource| Gets the currently selected camera MediaFrameSource|
| FrameSourceGroups| IReadOnlyList<MediaFrameSourceGroup>| Gets a read only list of MediaFrameSourceGroups that support color video record or video preview streams.|

## Methods

| Methods | Return Type | Description |
| -- | -- | -- |
| InitializeAndStartCaptureAsync(MediaFrameSourceGroup group = null) | Task<CameraHelperResult>| Initializes Camera Media Capture settings and initializes Frame Reader to capture frames in real time. If no MediaFrameSourceGroup is provided, it selects the first available camera source to  use for media capture. 
| Dispose() | void | Use this method to dispose resources |

## Events

| Events | Description |
| -- | -- |
| FrameArrived| Fires when a new frame arrives.|

## Sample Code

[CameraHelper Sample Page Source]
((https://github.com/Microsoft/UWPCommunityToolkit/tree/master/Microsoft.Toolkit.Uwp.SampleApp/SamplePages/CameraHelper)). You can see this in action in [UWP Community Toolkit Sample App](https://www.microsoft.com/store/apps/9NBLGGH4TLCQ).


## Requirements

| Device family | Universal, 10.0.14393.0 or higher |
| --- | --- |
| Namespace | Microsoft.Toolkit.Uwp |
| NuGet package | [Microsoft.Toolkit.Uwp](https://www.nuget.org/packages/Microsoft.Toolkit.Uwp/) |

## API Source Code

- [CameraHelper source code](https://github.com/Microsoft/UWPCommunityToolkit/blob/master/Microsoft.Toolkit.Uwp/Helpers/CameraHelper)


