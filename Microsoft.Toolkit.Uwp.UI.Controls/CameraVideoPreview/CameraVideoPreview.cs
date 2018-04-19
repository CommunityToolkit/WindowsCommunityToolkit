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
using System.Threading.Tasks;
using Microsoft.Toolkit.Uwp.Helpers.CameraHelper;
using Windows.Graphics.Imaging;
using Windows.Media;
using Windows.Media.Capture.Frames;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// Camera Control to preview video. Can subscribe to video frames, software bitmap when they arrive.
    /// </summary>
    public sealed class CameraVideoPreview : Control, IDisposable
    {
        private CameraHelper _cameraHelper;
        private MediaPlayer _mediaPlayer;
        private ComboBox _frameSourceGroupCombo;
        private MediaPlayerElement _mediaPlayerElementControl;
        private TextBlock _videoPreviewText;
        private TextBlock _videoPreviewErrorTextBlock;

        /// <summary>
        /// Initializes a new instance of the <see cref="CameraVideoPreview"/> class.
        /// </summary>
        public CameraVideoPreview()
        {
            this.DefaultStyleKey = typeof(CameraVideoPreview);
        }

        /// <summary>
        /// Event raised when a new video frame arrives.
        /// </summary>
        public event EventHandler<VideoFrame> VideoFrameArrived;

        /// <summary>
        /// Event raised when a new software bitmap arrives.
        /// </summary>
        public event EventHandler<SoftwareBitmap> SoftwareBitmapArrived;

        protected async override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.Unloaded += CameraVideoPreview_Unloaded;

            _frameSourceGroupCombo = (ComboBox)this.GetTemplateChild("FrameSourceGroupCombo");
            _mediaPlayerElementControl = (MediaPlayerElement)this.GetTemplateChild("MediaPlayerElementControl");          
            _videoPreviewText = (TextBlock)this.GetTemplateChild("VideoPreviewTextBlock");
            _videoPreviewErrorTextBlock = (TextBlock)this.GetTemplateChild("VideoPreviewErrorTextBlock");

            _frameSourceGroupCombo.SelectionChanged += FrameSourceGroupCombo_SelectionChanged;

            await InitFrameSourcesAsync();
        }

        private async Task InitFrameSourcesAsync()
        {
            var frameSourceGroups = await FrameSourceGroupsHelper.GetAllAvailableFrameSourceGroupsAsync();

            if (frameSourceGroups?.Count > 0)
            {
                _frameSourceGroupCombo.ItemsSource = frameSourceGroups;
            }
            else
            {
                _frameSourceGroupCombo.ItemsSource = new List<object> { new { DisplayName = "No camera sources found." } };
            }
            _frameSourceGroupCombo.SelectedIndex = 0;
        }

        private void SetMediaPlayerSource()
        {
            var frameSource = _cameraHelper?.FrameSource;
            if (frameSource != null)
            {
                if (_mediaPlayer == null)
                {
                    _mediaPlayer = new MediaPlayer
                    {
                        AutoPlay = true,
                        RealTimePlayback = true
                    };
                }

                _mediaPlayer.Source = MediaSource.CreateFromMediaFrameSource(frameSource);
                _mediaPlayerElementControl.SetMediaPlayer(_mediaPlayer);
            }
        }

        private void CameraHelper_VideoFrameArrived(object sender, VideoFrameEventArgs e)
        {
            EventHandler<VideoFrame> vfHandler = VideoFrameArrived;
            vfHandler?.Invoke(sender, e.VideoFrame);

            EventHandler<SoftwareBitmap> sbHandler = SoftwareBitmapArrived;
            sbHandler?.Invoke(sender, e.SoftwareBitmap);
        }

        private async void FrameSourceGroupCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedGroup = _frameSourceGroupCombo.SelectedItem as MediaFrameSourceGroup;
            if (selectedGroup != null)
            {
                if (_cameraHelper == null)
                {
                    _cameraHelper = new CameraHelper();

                    // Subscribe to the video frame as they arrive
                    _cameraHelper.VideoFrameArrived += CameraHelper_VideoFrameArrived;
                }

                var result = await _cameraHelper.InitializeAndStartCaptureAsync(selectedGroup);

                if (result.Status)
                {
                    SetMediaPlayerSource();
                }
                else
                {
                    _mediaPlayerElementControl.SetMediaPlayer(null);
                }

                _videoPreviewErrorTextBlock.Text = result.Message;
                _videoPreviewErrorTextBlock.Visibility = result.Status ? Visibility.Collapsed : Visibility.Visible;
                _videoPreviewText.Visibility = !result.Status ? Visibility.Collapsed : Visibility.Visible;
            }
        }

        private void CameraVideoPreview_Unloaded(object sender, RoutedEventArgs e)
        {
            Dispose();
        }

        /// <summary>
        /// Dispose resources.
        /// </summary>
        public void Dispose()
        {
            if (_cameraHelper != null)
            {
                _cameraHelper.VideoFrameArrived -= CameraHelper_VideoFrameArrived;
                _cameraHelper.Dispose();
                _cameraHelper = null;
            }

            if (_mediaPlayer != null)
            {
                _mediaPlayer.Dispose();
                _mediaPlayer = null;
            }

        }
    }
}
