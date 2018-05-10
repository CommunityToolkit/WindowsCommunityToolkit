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
using System.Collections.ObjectModel;
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

            await InitializeAsync();
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

        private void CameraHelper_FrameArrived(object sender, FrameEventArgs e)
        {
            _currentVideoFrame = e.VideoFrame;
        }

        private async Task InitializeAsync()
        {
            if (_cameraHelper == null)
            {
                _cameraHelper = new CameraHelper();
            }

            var result = await _cameraHelper.InitializeAndStartCaptureAsync();
            if (result == CameraHelperResult.Success)
            {
                // Subscribe to the video frame as they arrive
                _cameraHelper.FrameArrived += CameraHelper_FrameArrived;
                FrameSourceGroupCombo.ItemsSource = _cameraHelper.FrameSourceGroups;
                FrameSourceGroupCombo.SelectionChanged += FrameSourceGroupCombo_SelectionChanged;
                FrameSourceGroupCombo.SelectedIndex = 0;
            }

            SetUIControls(result);
        }

        private async void FrameSourceGroupCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedGroup = FrameSourceGroupCombo.SelectedItem as MediaFrameSourceGroup;
            if (selectedGroup != null)
            {
                var result = await _cameraHelper.InitializeAndStartCaptureAsync(selectedGroup);
                SetUIControls(result);
            }
        }

        private void SetUIControls(CameraHelperResult result)
        {
            var success = result == CameraHelperResult.Success;
            if (!success)
            {
                _currentVideoFrame = null;
            }

            CameraErrorTextBlock.Text = result.ToString();
            CameraErrorTextBlock.Visibility = success ? Visibility.Collapsed : Visibility.Visible;

            CaptureButton.IsEnabled = success;
            CurrentFrameImage.Opacity = success ? 1 : 0.5;
        }

        private async void CaptureButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var softwareBitmap = _currentVideoFrame.SoftwareBitmap;

            if (softwareBitmap != null)
            {
                if (softwareBitmap.BitmapPixelFormat != BitmapPixelFormat.Bgra8 || softwareBitmap.BitmapAlphaMode == BitmapAlphaMode.Straight)
                {
                    softwareBitmap = SoftwareBitmap.Convert(softwareBitmap, BitmapPixelFormat.Bgra8, BitmapAlphaMode.Premultiplied);
                }

                await _softwareBitmapSource.SetBitmapAsync(softwareBitmap);
            }
        }

        private void CleanUp()
        {
            if (FrameSourceGroupCombo != null)
            {
                FrameSourceGroupCombo.SelectionChanged -= FrameSourceGroupCombo_SelectionChanged;
            }

            if (_cameraHelper != null)
            {
               _cameraHelper.Dispose();
               _cameraHelper = null;
            }
        }
    }
}
