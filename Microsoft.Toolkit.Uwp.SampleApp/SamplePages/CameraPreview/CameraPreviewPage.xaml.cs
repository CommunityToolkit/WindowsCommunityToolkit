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
using Microsoft.Toolkit.Uwp.Helpers.CameraHelper;
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
        private VideoFrame _currentVideoFrame;
        private SoftwareBitmap _softwareBitmap;
        private SoftwareBitmapSource _softwareBitmapSource;
        private CameraPreview _cameraPreviewControl;
        private Image _imageControl;

        public CameraPreviewPage()
        {
            this.InitializeComponent();
        }

        public void OnXamlRendered(FrameworkElement control)
        {
            _cameraPreviewControl = control.FindDescendantByName("CameraPreviewControl") as CameraPreview;
            if (_cameraPreviewControl != null)
            {
                _cameraPreviewControl.FrameArrived += CameraPreviewControl_FrameArrived;
            }

            _imageControl = control.FindDescendantByName("CurrentFrameImage") as Image;
            if (_imageControl != null)
            {
                _softwareBitmapSource = new SoftwareBitmapSource();
                _imageControl.Source = _softwareBitmapSource;                
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            Shell.Current.RegisterNewCommand("Capture Current Frame", CaptureButton_Click);
            Application.Current.Suspending += Application_Suspending;
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

        private void CameraPreviewControl_FrameArrived(object sender, FrameEventArgs e)
        {
            _currentVideoFrame = e.VideoFrame;
            _softwareBitmap = e.SoftwareBitmap;
        }

        private async void CaptureButton_Click(object sender, RoutedEventArgs e)
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
            if (_cameraPreviewControl != null)
            {
                _cameraPreviewControl.FrameArrived -= CameraPreviewControl_FrameArrived;
                _cameraPreviewControl.Dispose();
            }
        }
    }
}
