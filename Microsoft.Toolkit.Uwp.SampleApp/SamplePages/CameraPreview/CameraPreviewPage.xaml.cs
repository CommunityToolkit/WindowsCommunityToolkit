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
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Toolkit.Uwp.Helpers;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Microsoft.Toolkit.Uwp.UI.Extensions;
using Windows.ApplicationModel;
using Windows.Graphics.Imaging;
using Windows.Media;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    /// <summary>
    /// CameraPreviewPage
    /// </summary>
    public sealed partial class CameraPreviewPage : Page, IXamlRenderListener
    {
        private static SemaphoreSlim semaphoreSlim;
        private VideoFrame _currentVideoFrame;
        private SoftwareBitmapSource _softwareBitmapSource;
        private CameraPreview _cameraPreviewControl;
        private Image _imageControl;
        private TextBlock _errorMessageText;

        public CameraPreviewPage()
        {
            this.InitializeComponent();
            semaphoreSlim = new SemaphoreSlim(1);
        }

        public async void OnXamlRendered(FrameworkElement control)
        {
            // Using a semaphore lock for synchronocity. 
            // This method gets called multiple times when accessing the page from Latest Pages
            // and creates unused duplicate references to Camera and memory leaks.
            await semaphoreSlim.WaitAsync();

            await CleanUpAsync();
            _cameraPreviewControl = control.FindDescendantByName("CameraPreviewControl") as CameraPreview;

            if (_cameraPreviewControl != null)
            {
                await _cameraPreviewControl.StartAsync();
                _cameraPreviewControl.CameraHelper.FrameArrived += CameraPreviewControl_FrameArrived;
                _cameraPreviewControl.PreviewFailed += CameraPreviewControl_PreviewFailed;
            }

            _imageControl = control.FindDescendantByName("CurrentFrameImage") as Image;
            if (_imageControl != null)
            {
                _softwareBitmapSource = new SoftwareBitmapSource();
                _imageControl.Source = _softwareBitmapSource;
            }

            _errorMessageText = control.FindDescendantByName("ErrorMessage") as TextBlock;

            semaphoreSlim.Release();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            Shell.Current.RegisterNewCommand("Capture Current Frame", CaptureButton_Click);
            Application.Current.Suspending += Application_Suspending;
            Application.Current.Resuming += Application_Resuming;
        }

        protected async override void OnNavigatedFrom(NavigationEventArgs e)
        {
            await CleanUpAsync();
        }

        private async void Application_Suspending(object sender, SuspendingEventArgs e)
        {
            if (Frame.CurrentSourcePageType == typeof(CameraPreviewPage))
            {
                var deferral = e.SuspendingOperation.GetDeferral();
                await CleanUpAsync();
                deferral.Complete();
            }
        }

        private async void Application_Resuming(object sender, object e)
        {
            var cameraHelper = new CameraHelper();
            await _cameraPreviewControl.StartAsync(cameraHelper);
            _cameraPreviewControl.CameraHelper.FrameArrived += CameraPreviewControl_FrameArrived;
            _cameraPreviewControl.PreviewFailed += CameraPreviewControl_PreviewFailed;
        }

        private void CameraPreviewControl_FrameArrived(object sender, FrameEventArgs e)
        {
            _currentVideoFrame = e.VideoFrame;
        }

        private void CameraPreviewControl_PreviewFailed(object sender, PreviewFailedEventArgs e)
        {
            _errorMessageText.Text = e.Error;
        }

        private async void CaptureButton_Click(object sender, RoutedEventArgs e)
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
            if (_cameraPreviewControl != null)
            {
                if (_cameraPreviewControl.CameraHelper != null)
                {
                    _cameraPreviewControl.CameraHelper.FrameArrived -= CameraPreviewControl_FrameArrived;
                }

                _cameraPreviewControl.PreviewFailed -= CameraPreviewControl_PreviewFailed;
                await _cameraPreviewControl.CleanupAsync();
            }
        }
    }
}
