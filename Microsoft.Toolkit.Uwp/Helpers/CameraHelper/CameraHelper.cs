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
using Windows.Devices.Enumeration;
using Windows.Media.Capture;
using Windows.Media.Capture.Frames;
using Windows.Media.MediaProperties;

namespace Microsoft.Toolkit.Uwp.Helpers.CameraHelper
{
    /// <summary>
    /// Camera Helper class to capture frames from available camera sources.
    /// Make sure you have the capability webcam enabled for your app to access the device's camera.
    /// </summary>
    public class CameraHelper
    {
        private static IReadOnlyList<MediaFrameSourceGroup> _frameSourceGroups;
        private MediaCapture _mediaCapture;
        private MediaFrameReader _frameReader;
        private MediaFrameSourceGroup _group;
        private MediaFrameSource _frameSource;
        private List<MediaFrameFormat> _frameFormatsAvailable;

        /// <summary>
        /// Gets a list of MediaFrameSourceGroups available for video preview or video record.
        /// </summary>
        /// <returns>A <see cref="MediaFrameSourceGroup"/> list.</returns>
        public static async Task<IReadOnlyList<MediaFrameSourceGroup>> GetFrameSourceGroupsAsync()
        {
            if (_frameSourceGroups == null)
            {
                var videoDevices = await DeviceInformation.FindAllAsync(DeviceClass.VideoCapture);
                var groups = await MediaFrameSourceGroup.FindAllAsync();

                // Filter out color video preview and video record type sources and remove duplicates video devices.
                _frameSourceGroups = groups.Where(g => g.SourceInfos.Any(s => s.SourceKind == MediaFrameSourceKind.Color &&
                                                                            (s.MediaStreamType == MediaStreamType.VideoPreview || s.MediaStreamType == MediaStreamType.VideoRecord))
                                                                            && g.SourceInfos.All(sourceInfo => videoDevices.Any(vd => vd.Id == sourceInfo.DeviceInformation.Id))).ToList();
            }

            return _frameSourceGroups;
        }

        /// <summary>
        /// Gets the available MediaFrameFormats on the sources
        /// </summary>
        public List<MediaFrameFormat> FrameFormatsAvailable { get => _frameFormatsAvailable; }

        /// <summary>
        /// Gets or sets the source group for camera video preview.
        /// </summary>
        public MediaFrameSourceGroup FrameSourceGroup { get => _group; set => _group = value; }

        /// <summary>
        /// Gets the currently selected <see cref="MediaFrameSource"/>.
        /// </summary>
        public MediaFrameSource FrameSource { get => _frameSource; }

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
        public async Task<CameraHelperResult> InitializeAndStartCaptureAsync()
        {
            await CleanupAsync();

            if (_frameSourceGroups == null)
            {
                _frameSourceGroups = await GetFrameSourceGroupsAsync();
            }

            if (_group == null)
            {
                _group = _frameSourceGroups.FirstOrDefault();
            }
            else
            {
                // verify selected group is part of existing FrameSourceGroups
                _group = _frameSourceGroups.FirstOrDefault(g => g.Id == _group.Id);
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

                // get only formats of a certain framerate and compatible subtype for previewing, order them by resolution
                _frameFormatsAvailable = _frameSource.SupportedFormats.Where(format =>
                    format.FrameRate.Numerator / format.FrameRate.Denominator >= 15 // fps
                    && (string.Compare(format.Subtype, MediaEncodingSubtypes.Nv12, true) == 0
                        || string.Compare(format.Subtype, MediaEncodingSubtypes.Bgra8, true) == 0
                        || string.Compare(format.Subtype, MediaEncodingSubtypes.Yuy2, true) == 0
                        || string.Compare(format.Subtype, MediaEncodingSubtypes.Rgb32, true) == 0))?.OrderBy(format => format.VideoFormat.Width * format.VideoFormat.Height).ToList();

                if (_frameFormatsAvailable == null)
                {
                    return CameraHelperResult.NoCompatibleFrameFormatAvailable;
                }

                // set the format with the higest resolution available by default
                var defaultFormat = _frameFormatsAvailable.Last();
                await _frameSource.SetFormatAsync(defaultFormat);
            }
            catch (UnauthorizedAccessException ex)
            {
                await CleanupAsync();
                return CameraHelperResult.CameraAccessDenied;
            }
            catch (Exception ex)
            {
                await CleanupAsync();
                return CameraHelperResult.InitializationFailed_UnknownError;
            }

            return CameraHelperResult.Success;
        }

        /// <summary>
        /// Stops reading from the frame reader, disposes of the reader.
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
        /// Clean up the Camera Helper resources
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task CleanupAsync()
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
