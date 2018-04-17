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
using Windows.ApplicationModel;
using Windows.Graphics.Imaging;
using Windows.Media;
using Windows.Media.Capture.Frames;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CameraPreviewPage : Page
    {
        private CameraHelper _cameraHelper;
        private VideoFrame _currentVideoFrame;
        private SoftwareBitmap _softwareBitmap;
        private SoftwareBitmapSource _softwareBitmapSource;
        private MediaPlayer _mediaPlayer;

        public CameraPreviewPage()
        {
            this.InitializeComponent();

            Application.Current.Suspending += Application_Suspending;
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            _softwareBitmapSource = new SoftwareBitmapSource();
            CurrentVideoFrameImage.Source = _softwareBitmapSource;

            await InitFrameSourcesAsync();
        }

        protected override async void OnNavigatedFrom(NavigationEventArgs e)
        {
            await CleanUpCameraAsync();
        }

        private async void Application_Suspending(object sender, SuspendingEventArgs e)
        {
            if (Frame.CurrentSourcePageType == typeof(CameraPreviewPage))
            {
                var deferral = e.SuspendingOperation.GetDeferral();
                await CleanUpCameraAsync();
                deferral.Complete();
            }
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
                MediaPlayerElementControl.SetMediaPlayer(_mediaPlayer);
            }
        }

        private void CameraHelper_VideoFrameArrived(object sender, VideoFrameEventArgs e)
        {
            _currentVideoFrame = e.VideoFrame;
            _softwareBitmap = e.SoftwareBitmap;
        }

        private async Task InitFrameSourcesAsync()
        {
            var frameSourceGroups = await FrameSourceGroupsHelper.GetAllAvailableFrameSourceGroupsAsync();

            if (frameSourceGroups?.Count > 0)
            {
                FrameSourceGroupCombo.ItemsSource = frameSourceGroups;
            }
            else
            {
                FrameSourceGroupCombo.ItemsSource = new List<object> { new { DisplayName = "No camera sources found." } };
                CaptureVideoFrame.Visibility = CaptureVideoFrame.Visibility = VideoPreviewText.Visibility =
                    MediaPlayerElementControl.Visibility = Visibility.Collapsed;
            }
            FrameSourceGroupCombo.SelectedIndex = 0;
        }

        private async void FrameSourceGroupCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedGroup = FrameSourceGroupCombo.SelectedItem as MediaFrameSourceGroup;
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
                    _currentVideoFrame = null;
                    _softwareBitmap = null;
                }

                VideoPreviewErrorMessage.Text = result.Message;
                VideoPreviewErrorMessage.Visibility = result.Status ? Visibility.Collapsed : Visibility.Visible;

                CaptureVideoFrame.IsEnabled = result.Status;
                MediaPlayerElementControl.IsEnabled = result.Status;
                CurrentVideoFrameImage.Opacity = result.Status ? 1 : 0.5;
            }
        }

        private async void CaptureVideoFrame_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var targetSoftwareBitmap = _softwareBitmap;

            if (_softwareBitmap != null)
            {
                if (_softwareBitmap.BitmapPixelFormat != BitmapPixelFormat.Bgra8 || _softwareBitmap.BitmapAlphaMode == BitmapAlphaMode.Straight)
                {
                    targetSoftwareBitmap = SoftwareBitmap.Convert(_softwareBitmap, BitmapPixelFormat.Bgra8, BitmapAlphaMode.Premultiplied);
                }

                await _softwareBitmapSource.SetBitmapAsync(targetSoftwareBitmap);
            }
        }

        private async Task CleanUpCameraAsync()
        {
            if (_cameraHelper != null)
            {
                await _cameraHelper.CleanupAsync();
            }

            if (_mediaPlayer != null)
            {
                _mediaPlayer.Dispose();
                _mediaPlayer = null;
            }
        }
    }
}
