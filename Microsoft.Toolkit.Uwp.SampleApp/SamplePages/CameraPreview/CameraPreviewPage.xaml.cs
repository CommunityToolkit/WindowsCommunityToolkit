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
        private SoftwareBitmapSource softwareBitmapSource;

        public CameraPreviewPage()
        {
            this.InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            softwareBitmapSource = new SoftwareBitmapSource();
            CurrentVideoFrameImage.Source = softwareBitmapSource;

            await InitFrameSourcesAsync();
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
                var selectedGroup = FrameSourceGroupCombo.SelectedItem as MediaFrameSourceGroup;
                FrameSourceGroupCombo.SelectedIndex = 0;
            }
            else
            {
                FrameSourceGroupCombo.ItemsSource = new { DisplayName = "No valid sources found" };
                FrameSourceGroupCombo.SelectedIndex = 0;
            }
        }

        private async void FrameSourceGroupCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedGroup = FrameSourceGroupCombo.SelectedItem as MediaFrameSourceGroup;
            if (selectedGroup != null)
            {
                if (cameraHelper == null)
                {
                    cameraHelper = new CameraHelper();

                    // Subscribe to the video frame as they arrive
                    cameraHelper.VideoFrameArrived += CameraHelper_VideoFrameArrived;
                }

                await cameraHelper.InitializeAndStartCapture(selectedGroup);
            }
        }

        private async void CaptureVideoFrame_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (_currentVideoFrame != null)
            {
                var softwareBitmap = _currentVideoFrame.SoftwareBitmap;
                if (softwareBitmap != null &&
                    (softwareBitmap.BitmapPixelFormat != BitmapPixelFormat.Bgra8 || softwareBitmap.BitmapAlphaMode == BitmapAlphaMode.Straight))
                {
                    softwareBitmap = SoftwareBitmap.Convert(softwareBitmap, BitmapPixelFormat.Bgra8, BitmapAlphaMode.Premultiplied);
                    await softwareBitmapSource.SetBitmapAsync(softwareBitmap);
                }
            }
        }
    }
}
