using System;
using System.Threading.Tasks;
using Microsoft.Toolkit.Uwp.Helpers.CameraHelper;
using Windows.Graphics.Imaging;
using Windows.Media;
using Windows.Media.Capture.Frames;
using Windows.Media.Core;
using Windows.Media.Playback;
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
        private CameraHelper _cameraHelper;
        private VideoFrame _currentVideoFrame;
        private SoftwareBitmap _softwareBitmap;
        private SoftwareBitmapSource _softwareBitmapSource;
        private MediaPlayer _mediaPlayer;

        public CameraPreviewPage()
        {
            this.InitializeComponent();
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
            await _cameraHelper?.Cleanup();
            CleanUpMediaPlayer();
        }

        private void CleanUpMediaPlayer()
        {
            if (_mediaPlayer != null)
            {
                _mediaPlayer.Dispose();
                _mediaPlayer = null;
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
            if (e.VideoFrame != null)
            {
                _currentVideoFrame = e.VideoFrame;
                _softwareBitmap = e.SoftwareBitmap;
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
                if (_cameraHelper == null)
                {
                    _cameraHelper = new CameraHelper();

                    // Subscribe to the video frame as they arrive
                    _cameraHelper.VideoFrameArrived += CameraHelper_VideoFrameArrived;
                }

                await _cameraHelper.InitializeAndStartCapture(selectedGroup);

                SetMediaPlayerSource();
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
    }
}
