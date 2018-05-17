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
using System.Threading.Tasks;
using Microsoft.Toolkit.Uwp.Helpers;
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
            Application.Current.Resuming += Application_Resuming;
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            _softwareBitmapSource = new SoftwareBitmapSource();
            CurrentFrameImage.Source = _softwareBitmapSource;

            await InitializeAsync();
        }

        protected async override void OnNavigatedFrom(NavigationEventArgs e)
        {
            await CleanUpAsync();
        }

        private async void Application_Suspending(object sender, SuspendingEventArgs e)
        {
            if (Frame.CurrentSourcePageType == typeof(CameraHelperPage))
            {
                var deferral = e.SuspendingOperation.GetDeferral();
                await CleanUpAsync();
                deferral.Complete();
            }
        }

        private async void Application_Resuming(object sender, object e)
        {
            await InitializeAsync();
        }

        private void CameraHelper_FrameArrived(object sender, FrameEventArgs e)
        {
            _currentVideoFrame = e.VideoFrame;
        }

        private async Task InitializeAsync()
        {
            var frameSourceGroups = await CameraHelper.GetFrameSourceGroupsAsync();            
            if (_cameraHelper == null)
            {
                _cameraHelper = new CameraHelper();
            }

            var result = await _cameraHelper.InitializeAndStartCaptureAsync();
            if (result == CameraHelperResult.Success)
            {
                // Subscribe to the video frame as they arrive
                _cameraHelper.FrameArrived += CameraHelper_FrameArrived;
                FrameSourceGroupCombo.ItemsSource = frameSourceGroups;
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
                _cameraHelper.FrameSourceGroup = selectedGroup;
                var result = await _cameraHelper.InitializeAndStartCaptureAsync();
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

        private async Task CleanUpAsync()
        {
            if (FrameSourceGroupCombo != null)
            {
                FrameSourceGroupCombo.SelectionChanged -= FrameSourceGroupCombo_SelectionChanged;
            }

            if (_cameraHelper != null)
            {
                _cameraHelper.FrameArrived -= CameraHelper_FrameArrived;
                await _cameraHelper.CleanupAsync();
               _cameraHelper = null;
            }
        }
    }
}
