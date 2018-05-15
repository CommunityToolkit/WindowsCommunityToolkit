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
using Microsoft.Toolkit.Uwp.Helpers;
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
    public partial class CameraPreview : Control
    {
        private CameraHelper _cameraHelper;
        private MediaPlayer _mediaPlayer;
        private MediaPlayerElement _mediaPlayerElementControl;
        private Button _frameSourceGroupButton;
        private int _selectedSourceIndex = 0;

        private IReadOnlyList<MediaFrameSourceGroup> _frameSourceGroups;

        private bool IsFrameSourceGroupButtonAvailable => _frameSourceGroups != null && _frameSourceGroups.Count > 1;

        /// <summary>
        /// Gets Camera Helper
        /// </summary>
        public CameraHelper CameraHelper { get => _cameraHelper; private set => _cameraHelper = value; }

        /// <summary>
        /// Initialize control with a CameraHelper instance
        /// </summary>
        /// <param name="cameraHelper"><see cref="CameraHelper"/></param>
        public async Task SetCameraHelperAsync(CameraHelper cameraHelper)
        {
            _cameraHelper = cameraHelper;
            _frameSourceGroups = await CameraHelper.GetFrameSourceGroupsAsync();
            await InitializeAsync();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CameraPreview"/> class.
        /// </summary>
        public CameraPreview()
        {
            this.DefaultStyleKey = typeof(CameraPreview);
        }

        protected async override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (_frameSourceGroupButton != null)
            {
                _frameSourceGroupButton.Click -= FrameSourceGroupButton_ClickAsync;
            }

            _mediaPlayerElementControl = (MediaPlayerElement)GetTemplateChild("MediaPlayerElementControl");
            _frameSourceGroupButton = (Button)GetTemplateChild("FrameSourceGroupButton");

            if (_frameSourceGroupButton != null)
            {
                _frameSourceGroupButton.Click += FrameSourceGroupButton_ClickAsync;
                _frameSourceGroupButton.IsEnabled = false;
            }
        }

        private async Task InitializeAsync()
        {
            var result = await _cameraHelper.InitializeAndStartCaptureAsync();
            if (result != CameraHelperResult.Success)
            {
                InvokePreviewFailed(result.ToString());
            }

            SetUIControls(result);
        }

        private async void FrameSourceGroupButton_ClickAsync(object sender, RoutedEventArgs e)
        {
            _selectedSourceIndex = _selectedSourceIndex < (_frameSourceGroups.Count - 1) ? _selectedSourceIndex + 1 : 0;
            var group = _frameSourceGroups[_selectedSourceIndex];
            _frameSourceGroupButton.IsEnabled = false;
            _cameraHelper.FrameSourceGroup = group;
            await InitializeAsync();
        }

        private void InvokePreviewFailed(string error)
        {
            EventHandler<PreviewFailedEventArgs> handler = PreviewFailed;
            handler?.Invoke(this, new PreviewFailedEventArgs { Error = error });
        }

        private void SetMediaPlayerSource()
        {
            try
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
            catch (Exception ex)
            {
                InvokePreviewFailed(ex.Message);
            }
        }

        private void SetUIControls(CameraHelperResult result)
        {
            var success = result == CameraHelperResult.Success;
            if (success)
            {
                SetMediaPlayerSource();
            }
            else
            {
                _mediaPlayerElementControl.SetMediaPlayer(null);
            }

            _frameSourceGroupButton.IsEnabled = IsFrameSourceGroupButtonAvailable;
            SetFrameSourceGroupButtonVisibility();
        }

        private void SetFrameSourceGroupButtonVisibility()
        {
            _frameSourceGroupButton.Visibility = IsFrameSourceGroupButtonAvailable && IsFrameSourceGroupButtonVisible
                                                                ? Visibility.Visible
                                                                : Visibility.Collapsed;
        }

        /// <summary>
        /// Dispose resources.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task CleanupAsync()
        {
            if (_cameraHelper != null)
            {
                await _cameraHelper.CleanupAsync();
                _cameraHelper = null;
            }

            if (_mediaPlayerElementControl != null)
            {
                _mediaPlayerElementControl.SetMediaPlayer(null);
            }

            if (_mediaPlayer != null)
            {
                _mediaPlayer.Dispose();
                _mediaPlayer = null;
            }
        }
    }
}
