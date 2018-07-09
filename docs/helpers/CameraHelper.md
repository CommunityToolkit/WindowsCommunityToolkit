---
title: CameraHelper
author: skommireddi
description: The CameraHelper provides helper methods to easily use the available camera frame sources to preview video, capture video frames and software bitmaps.
keywords: windows 10, uwp, windows community toolkit, windows toolkit, CameraHelper, Camera, Frame Source, Video Frame, Software Bitmap
---

# CameraHelper

The **CameraHelper** provides helper methods to easily use the available camera frame sources to preview video, capture video frames and software bitmaps. The helper currently shows camera frame sources that support color video preview or video record streams. 

> [!IMPORTANT]
Make sure you have the [webcam capability](https://docs.microsoft.com/en-us/windows/uwp/packaging/app-capability-declarations#device-capabilities) enabled for your app to access the device's camera.

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
else 
{
  // Subscribe to get frames as they arrive
  cameraHelper.FrameArrived += CameraHelper_FrameArrived;
}

private void CameraHelper_FrameArrived(object sender, FrameEventArgs e)
{
  // Gets the current video frame
  VideoFrame currentVideoFrame  = e.VideoFrame;

  // Gets the software bitmap image
  SoftwareBitmap softwareBitmap = currentVideoFrame.SoftwareBitmap;
}
```

## Cleaning up resources

As a developer, you will need to make sure the CameraHelper resources are cleaned up when appropriate. For example, if the CameraHelper is only used on one page, make sure to clean up the CameraHelper when navigating away from the page.

Likewise, make sure to handle app [suspending](https://docs.microsoft.com/windows/uwp/launch-resume/suspend-an-app) and [resuming](https://docs.microsoft.com/en-us/windows/uwp/launch-resume/resume-an-app) - CameraHelper should be cleaned up when suspending and re-initialized when resuming.

Call `CameraHelper.CleanupAsync()` to clean up all internal resources. See the [CameraHelper sample page in the sample app](https://github.com/Microsoft/WindowsCommunityToolkit//tree/master/Microsoft.Toolkit.Uwp.SampleApp/SamplePages/CameraHelper) for full example.

## Properties

| Property | Type | Description |
| -- | -- | -- |
| FrameSourceGroup | MediaFrameSourceGroup | Gets the currently selected MediaFrameSourceGroup for video preview. User can set this property to preview video from a specific source. If no MediaFrameSourceGroup is provided, Camera Helper selects the first available camera source to  use for media capture. |
| FrameSource | MediaFrameSource | Gets the currently selected MediaFrameSource for video preview. |

## Methods

| Methods | Return Type | Description |
| -- | -- | -- |
| GetFrameSourceGroupsAsync() | Task<IReadOnlyList<MediaFrameSourceGroup>> | Gets a read only list of MediaFrameSourceGroups that support color video record or video preview streams.
| InitializeAndStartCaptureAsync() | Task<CameraHelperResult>| Initializes Media Capture and Frame Reader for video preview and capture frames in real time. |
| CleanUpAsync() | Task | Use this asynchronous method to dispose Camera Helper resources |
| Dispose() | void | Use this method to dispose Camera Helper resources |

## Events

| Events | Description |
| -- | -- |
| FrameArrived| Fires when a new frame arrives.|

## Examples

Demonstrates using Camera Helper to get video frames from a specific media frame source group.

```csharp

using Microsoft.Toolkit.Uwp.Helpers.CameraHelper;

var availableFrameSourceGroups == await CameraHelper.GetFrameSourceGroupsAsync();
if(availableFrameSourceGroups != null)
{
  CameraHelper cameraHelper = new CameraHelper() { FrameSourceGroup = availableFrameSourceGroups.FirstOrDefault() };
  var result = await _cameraHelper.InitializeAndStartCaptureAsync();

  // Camera Initialization succeeded
  if(result == CameraHelperResult.Success)
  {
    // Subscribe to get frames as they arrive
    cameraHelper.FrameArrived += CameraHelper_FrameArrived;
	
	// Optionally set a different frame source format
	var newFormat = _cameraHelper.FrameFormatsAvailable.Find((format) => format.VideoFormat.Width == 640);
	if (newFormat != null)
	{
		await _cameraHelper.FrameSource.SetFormatAsync(newFormat);
	}
  }
}

```

## Sample Code

[CameraHelper Sample Page Source](https://github.com/Microsoft/WindowsCommunityToolkit//tree/master/Microsoft.Toolkit.Uwp.SampleApp/SamplePages/CameraHelper). You can see this in action in [Windows Community Toolkit Sample App](https://www.microsoft.com/store/apps/9NBLGGH4TLCQ).


## Requirements

| Device family | Universal, 10.0.17134.0 or higher |
| --- | --- |
| Namespace | Microsoft.Toolkit.Uwp |
| NuGet package | [Microsoft.Toolkit.Uwp](https://www.nuget.org/packages/Microsoft.Toolkit.Uwp/) |

## API Source Code

- [CameraHelper source code](https://github.com/Microsoft/WindowsCommunityToolkit//blob/master/Microsoft.Toolkit.Uwp/Helpers/CameraHelper)

