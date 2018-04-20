---
title: CameraHelper
author: skommireddi
description: The CameraHelper provides helper methods to get camera frame sources which can be used to preview video, capture video frames and software bitmaps.
keywords: windows 10, uwp, uwp community toolkit, uwp toolkit, CameraHelper, Camera, Frame Source, Video Frame
---

# CameraHelper

The **CameraHelper** provides helper methods to get camera frame sources which can be used to preview video, capture video frames and software bitmaps. The helper currently filters frame sources that support color video preview or video record streams. 

## Syntax

```csharp
// Creates a Camera Helper and gets video frames from an available frame source.
using Microsoft.Toolkit.Uwp.Helpers.CameraHelper;

// Get all available camera frame source groups
var frameSourceGroups = await FrameSourceGroupsHelper.GetAllAvailableFrameSourceGroupsAsync();

// Get first available camera frame source group
var frameSourceGroup = await FrameSourceGroupsHelper.GetFirstAvailableFrameSourceGroupAsync();
if (frameSourceGroup != null)
{
  // Initialize Camera Helper with selected frame source group
  CameraHelper cameraHelper = new CameraHelper();
  var result = await _cameraHelper.InitializeAndStartCaptureAsync(frameSourceGroup);

  // Camera Initialization and Capture failed for some reason
  if(result.Status == false)
  {
	// log the error
	var errorMessage = result.Message;
  }

  // Subscribe to the video frames and software bitmaps as they arrive
  cameraHelper.VideoFrameArrived += CameraHelper_VideoFrameArrived;
}
else
{
  // No frame source available.
}

private void CameraHelper_VideoFrameArrived(object sender, VideoFrameEventArgs e)
{
  // Gets the current video frame
  VideoFrame currentVideoFrame  = e.VideoFrame;

  // Gets the software bitmap image
  SoftwareBitmap softwareBitmap = e.SoftwareBitmap;
}
```

## Example

[CameraHelper Sample Page]
((https://github.com/Microsoft/UWPCommunityToolkit/tree/master/Microsoft.Toolkit.Uwp.SampleApp/SamplePages/CameraHelper))

## Requirements (Windows 10 Device Family)

| [Device family](http://go.microsoft.com/fwlink/p/?LinkID=526370) | Universal, 10.0.14393.0 or higher |
| --- | --- |
| Namespace | Microsoft.Toolkit.Uwp |


## API

* [CameraHelper source code](https://github.com/Microsoft/UWPCommunityToolkit/blob/master/Microsoft.Toolkit.Uwp/Helpers/CameraHelper)


