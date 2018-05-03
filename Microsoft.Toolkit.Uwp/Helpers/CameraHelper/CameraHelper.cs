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
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Media;
using Windows.Media.Capture;
using Windows.Media.Capture.Frames;

namespace Microsoft.Toolkit.Uwp.Helpers.CameraHelper
{
    /// <summary>
    /// Camera Helper class to capture frames from available camera sources.
    /// Make sure you have the capability webcam enabled for your app to access the device's camera.
    /// </summary>
    public class CameraHelper : IDisposable
    {
        private IReadOnlyList<MediaFrameSourceGroup> _frameSourceGroups;
        private MediaCapture _mediaCapture;
        private MediaFrameReader _frameReader;
        private MediaFrameSourceGroup _group;
        private MediaFrameSource _frameSource;

        /// <summary>
        /// Gets the currently selected <see cref="MediaFrameSource"/>MediaFrameSource
        /// </summary>
        public MediaFrameSource FrameSource { get => _frameSource; }

        /// <summary>
        /// Gets a read only list of MediaFrameSourceGroups that support color video record or video preview streams.
        /// </summary>
        public IReadOnlyList<MediaFrameSourceGroup> FrameSourceGroups { get => _frameSourceGroups; }

        /// <summary>
        /// Event raised when a new frame arrives.
        /// </summary>
        public event EventHandler<FrameEventArgs> FrameArrived;

        /// <summary>
        /// Initializes Camera Media Capture settings and initializes Frame Reader to capture frames in real time. 
        /// If no MediaFrameSourceGroup is provided, it selects the first available camera source to  use for media capture.
        /// You could select a specific MediaFrameSourceGroup from the available sources using the CameraHelper FrameSourceGroups property.
        /// </summary>
        /// <returns>Result of the async operation.<see cref="CameraHelperResult"/></returns>
        public async Task<CameraHelperResult> InitializeAndStartCaptureAsync(MediaFrameSourceGroup group = null)
        {
            // new selection same as previously selected group, just return
            if (group != null && _group == group)
            {
                return CameraHelperResult.Success;
            }

            await Cleanup();

            _group = group;

            if (_frameSourceGroups == null)
            {
                var groups = await MediaFrameSourceGroup.FindAllAsync();
                _frameSourceGroups = groups.Where(g => g.SourceInfos.Any(s => s.SourceKind == MediaFrameSourceKind.Color &&
                                                                            (s.MediaStreamType == MediaStreamType.VideoPreview
                                                                            || s.MediaStreamType == MediaStreamType.VideoRecord))).ToList();
            }

            if (_group == null)
            {
                _group = _frameSourceGroups.FirstOrDefault();
            }

            // if there is no camera source available, we can't proceed.
            if (_group == null)
            {
                return CameraHelperResult.NoFrameSourceGroupAvailable;
            }

            var result = await InitMediaCaptureAsync();

            if (_frameSource != null)
            {
                _frameReader = await _mediaCapture.CreateFrameReaderAsync(_frameSource);
                _frameReader.AcquisitionMode = MediaFrameReaderAcquisitionMode.Realtime;
                _frameReader.FrameArrived += Reader_FrameArrived;

                if (_frameReader == null)
                {
                    result = CameraHelperResult.CreateFrameReaderFailed;
                }
                else
                {
                    MediaFrameReaderStartStatus statusResult = await _frameReader.StartAsync();
                    if (statusResult != MediaFrameReaderStartStatus.Success)
                    {
                        result = CameraHelperResult.StartFrameReaderFailed;
                    }
                }
            }

            return result;
        }

        private async Task<CameraHelperResult> InitMediaCaptureAsync()
        {
            if (_mediaCapture == null)
            {
                _mediaCapture = new MediaCapture();
            }

            var settings = new MediaCaptureInitializationSettings()
            {
                SourceGroup = _group,
                SharingMode = MediaCaptureSharingMode.SharedReadOnly,
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
                    return CameraHelperResult.NoFrameSourceAvailable;
                }
            }
            catch (UnauthorizedAccessException)
            {
                await Cleanup();
                return CameraHelperResult.CameraAccessDenied;
            }
            catch (Exception)
            {
                await Cleanup();
                return CameraHelperResult.InitializationFailed_UnknownError;
            }

            return CameraHelperResult.Success;
        }

        /// <summary>
        /// Stops reading from the frame reader, disposes of the reader and updates the button state.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        private async Task StopReaderAsync()
        {
            if (_frameReader != null)
            {
                _frameReader.FrameArrived -= Reader_FrameArrived;
                await _frameReader.StopAsync();
                _frameReader.Dispose();
                _frameReader = null;
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
            var frame = sender.TryAcquireLatestFrame();
            if (frame != null)
            {
                var vmf = frame.VideoMediaFrame;
                EventHandler<FrameEventArgs> handler = FrameArrived;
                var frameArgs = new FrameEventArgs() { VideoFrame = vmf.GetVideoFrame() };
                handler?.Invoke(sender, frameArgs);
            }
        }

        /// <summary>
        /// Dispose resources.
        /// </summary>
        public async void Dispose()
        {
            await Cleanup();
        }

        private async Task Cleanup()
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
