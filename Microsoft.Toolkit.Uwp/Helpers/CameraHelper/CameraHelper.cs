// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Media;
using Windows.Media.Capture;
using Windows.Media.Capture.Frames;
using Windows.Media.MediaProperties;

namespace Microsoft.Toolkit.Uwp.Helpers.CameraHelper
{
    /// <summary>
    /// Camera Helper class to capture frames from available camera sources.
    /// </summary>
    public class CameraHelper
    {
        private MediaCapture _mediaCapture;
        private MediaFrameReader _frameReader;
        private MediaFrameSourceGroup _group;
        private MediaFrameSource _frameSource;

        /// <summary>
        /// Gets the current MediaFrameSource
        /// </summary>
        public MediaFrameSource FrameSource { get => _frameSource; }

        /// <summary>
        /// Event raised when a new video frame arrives.
        /// </summary>
        public event EventHandler<VideoFrameEventArgs> VideoFrameArrived;

        /// <summary>
        /// Initializes a new instance of the <see cref="CameraHelper"/> class.
        /// </summary>
        public CameraHelper()
        {
        }

        /// <summary>
        /// Initializes Media Capture settings and starts video capture using Frame Reader.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task<CameraHelperResult> InitializeAndStartCapture(MediaFrameSourceGroup group)
        {
            await Cleanup();
            _group = group;
            var result = await InitMediaCaptureAsync();

            if (_frameSource != null)
            {
                _frameReader = await _mediaCapture.CreateFrameReaderAsync(_frameSource);
                _frameReader.AcquisitionMode = MediaFrameReaderAcquisitionMode.Realtime;
                _frameReader.FrameArrived += Reader_FrameArrived;
                Debug.WriteLine($"Frame Reader created on source: {_frameSource.Info.Id}");

                if (_frameReader == null)
                {
                    result.Status = false;
                    result.Message = "Failed to create Frame Reader";
                }
                else
                {
                    MediaFrameReaderStartStatus statusResult = await _frameReader.StartAsync();
                    Debug.WriteLine($"Start reader with result: {statusResult}");
                    if (statusResult != MediaFrameReaderStartStatus.Success)
                    {
                        result.Status = false;
                        result.Message = $"Unable to start Frame Reader Reason: {statusResult}";
                    }
                }
            }

            return result;
        }

        private async Task<CameraHelperResult> InitMediaCaptureAsync()
        {
            CameraHelperResult result = new CameraHelperResult
            {
                Status = true
            };

            if (_group == null)
            {
                // try to get the first available camera
                _group = await FrameSourceGroupsHelper.GetFirstAvailableFrameSourceGroup();
            }

            // if there is no camera available, we can't proceed.
            if (_group == null)
            {
                result.Message = "No camera source available";
                result.Status = false;
                return result;
            }

            if (_mediaCapture == null)
            {
                _mediaCapture = new MediaCapture();
            }

            var settings = new MediaCaptureInitializationSettings()
            {
                SourceGroup = _group,
                SharingMode = MediaCaptureSharingMode.ExclusiveControl,
                MemoryPreference = MediaCaptureMemoryPreference.Cpu,
                StreamingCaptureMode = StreamingCaptureMode.Video
            };

            try
            {
                await _mediaCapture.InitializeAsync(settings);

                // Find the first video preview or record stream available
                _frameSource = _mediaCapture.FrameSources.FirstOrDefault(source => source.Value.Info.MediaStreamType == MediaStreamType.VideoPreview
                                                                                      && source.Value.Info.SourceKind == MediaFrameSourceKind.Color).Value;
                if (_frameSource == null)
                {
                    _frameSource = _mediaCapture.FrameSources.FirstOrDefault(source => source.Value.Info.MediaStreamType == MediaStreamType.VideoRecord
                                                                                          && source.Value.Info.SourceKind == MediaFrameSourceKind.Color).Value;
                }

                if (_frameSource == null)
                {
                    result.Message = "No preview stream available";
                    result.Status = false;
                    Debug.WriteLine(result.Message);
                    return result;
                }
            }
            catch (Exception ex)
            {
                result.Message = "Failed to initialize media capture: " + ex.Message;
                result.Status = false;
                Debug.WriteLine(result.Message);
                await Cleanup();
                return result;
            }

            return result;
        }

        /// <summary>
        /// Stops reading from the frame reader, disposes of the reader and updates the button state.
        /// </summary>
        private async Task StopReaderAsync()
        {
            if (_frameReader != null)
            {
                await _frameReader.StopAsync();
                _frameReader.FrameArrived -= Reader_FrameArrived;
                _frameReader.Dispose();
                _frameReader = null;

                Debug.WriteLine("Frame Reader stopped.");
            }
        }

        /// <summary>
        /// Handles the frame arrived event by converting the frame to a displayable
        /// format and rendering it to the screen.
        /// </summary>
        private void Reader_FrameArrived(MediaFrameReader sender, MediaFrameArrivedEventArgs args)
        {
            // TryAcquireLatestFrame will return the latest frame that has not yet been acquired.
            // This can return null if there is no such frame, or if the reader is not in the
            // "Started" state. The latter can occur if a FrameArrived event was in flight
            // when the reader was stopped.
            using (var frame = sender.TryAcquireLatestFrame())
            {
                if (frame == null)
                {
                    Debug.WriteLine("Frame is null");
                    return;
                }

                var vmf = frame.VideoMediaFrame;

                EventHandler<VideoFrameEventArgs> handler = VideoFrameArrived;
                var frameArgs = new VideoFrameEventArgs() { VideoFrame = vmf.GetVideoFrame(), SoftwareBitmap = vmf.SoftwareBitmap };
                handler?.Invoke(sender, frameArgs);
            }
        }

        /// <summary>
        /// Clean up and dispose resources
        /// </summary>
        /// <returns>>A <see cref="Task"/> representing the asynchronous operation.></returns>
        public async Task Cleanup()
        {
            await StopReaderAsync();

            if (_mediaCapture != null)
            {
                _mediaCapture.Dispose();
                _mediaCapture = null;
            }
        }
    }
}
