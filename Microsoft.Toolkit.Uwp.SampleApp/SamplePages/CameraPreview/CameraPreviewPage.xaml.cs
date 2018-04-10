using Microsoft.Toolkit.Uwp.Helpers.CameraHelper;
using System;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Media;
using Windows.Media.Capture.Frames;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CameraPreviewPage : Page
    {
        private CameraHelper cameraHelper;
        private VideoFrame _currentVideoFrame;

        public CameraPreviewPage()
        {
            this.InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            await InitFrameSourcesAsync();
        }

        private async void InitCameraHelper(MediaFrameSourceGroup group)
        {
            if (cameraHelper == null)
            {
                cameraHelper = new CameraHelper(group);
                await cameraHelper.InitMediaCaptureAndStartFrameReaderAsync();

                // Subscribe to the video frame as they arrive
                cameraHelper.VideoFrameArrived += CameraHelper_VideoFrameArrived;
            }
        }

        private void CameraHelper_VideoFrameArrived(object sender, VideoFrameEventArgs e)
        {
            if (e.VideoFrame != null)
            {
                _currentVideoFrame = e.VideoFrame;
            }
        }

        private async Task InitFrameSourcesAsync()
        {
            var frameSourceGroups = await FrameSourceGroupsHelper.GetAllAvailableFrameSourceGroups();
            if (frameSourceGroups.Count > 0)
            {
                FrameSourceGroupCombo.ItemsSource = frameSourceGroups;
                FrameSourceGroupCombo.SelectedIndex = 0;
                var selectedGroup = FrameSourceGroupCombo.SelectedItem as MediaFrameSourceGroup;
                InitCameraHelper(selectedGroup);
            }
            else
            {
                FrameSourceGroupCombo.ItemsSource = new { DisplayName = "No valid sources found" };
            }
        }

        private void FrameSourceGroupCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private async void CaptureVideoFrame_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (_currentVideoFrame != null)
            {
                var softwareBitmap = _currentVideoFrame.SoftwareBitmap;
                if (softwareBitmap.BitmapPixelFormat != BitmapPixelFormat.Bgra8 || softwareBitmap.BitmapAlphaMode == BitmapAlphaMode.Straight)
                {
                    softwareBitmap = SoftwareBitmap.Convert(softwareBitmap, BitmapPixelFormat.Bgra8, BitmapAlphaMode.Premultiplied);
                }

                var source = new SoftwareBitmapSource();
                await source.SetBitmapAsync(softwareBitmap);

                CurrentVideoFrameImage.Source = source;
            }
        }
    }
}
