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
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    /// <summary>
    /// Sample page for Camera Helper
    /// </summary>
    public sealed partial class CameraHelperPage : Page
    {
        private CameraHelper _cameraHelper;
        private VideoFrame _currentVideoFrame;
        private SoftwareBitmap _softwareBitmap;
        private SoftwareBitmapSource _softwareBitmapSource;

        public CameraHelperPage()
        {
            this.InitializeComponent();

            Application.Current.Suspending += Application_Suspending;
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            _softwareBitmapSource = new SoftwareBitmapSource();
            CurrentFrameImage.Source = _softwareBitmapSource;
            FrameSourceGroupCombo.SelectionChanged += FrameSourceGroupCombo_SelectionChanged;

            await InitFrameSourcesAsync();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            CleanUp();
        }

        private void Application_Suspending(object sender, SuspendingEventArgs e)
        {
            if (Frame.CurrentSourcePageType == typeof(CameraPreviewPage))
            {
                var deferral = e.SuspendingOperation.GetDeferral();
                CleanUp();
                deferral.Complete();
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
                CaptureButton.Visibility = Visibility.Collapsed;
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

                if (!result.Status)
                {
                    _currentVideoFrame = null;
                    _softwareBitmap = null;
                }

                CameraErrorTextBlock.Text = result.Message;
                CameraErrorTextBlock.Visibility = result.Status ? Visibility.Collapsed : Visibility.Visible;

                CaptureButton.IsEnabled = result.Status;
                CurrentFrameImage.Opacity = result.Status ? 1 : 0.5;
            }
        }

        private async void CaptureButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
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

        private void CleanUp()
        {
            if (_cameraHelper != null)
            {
               _cameraHelper.Dispose();
            }
        }
    }
}
