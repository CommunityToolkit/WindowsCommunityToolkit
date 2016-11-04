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
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// The ImageEx control extends the default Image platform control improving the performance and responsiveness of your Apps.
    /// Source images are downloaded asynchronously showing a load indicator while in progress.
    /// Once downloaded, the source image is stored in the App local cache to preserve resources and load time next time the image needs to be displayed.
    /// </summary>
    [TemplateVisualState(Name = LoadingState, GroupName = CommonGroup)]
    [TemplateVisualState(Name = LoadedState, GroupName = CommonGroup)]
    [TemplateVisualState(Name = UnloadedState, GroupName = CommonGroup)]
    [TemplateVisualState(Name = FailedState, GroupName = CommonGroup)]
    [TemplatePart(Name = PartImage, Type = typeof(Image))]
    [TemplatePart(Name = PartProgress, Type = typeof(ProgressRing))]
    public partial class ImageEx : Control
    {
        private const string PartImage = "Image";
        private const string PartProgress = "Progress";
        private const string CommonGroup = "CommonStates";
        private const string LoadingState = "Loading";
        private const string LoadedState = "Loaded";
        private const string UnloadedState = "Unloaded";
        private const string FailedState = "Failed";

        private Image _image;
        private ProgressRing _progress;
        private object _lockObj;

        private bool _isInitialized;

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageEx"/> class.
        /// </summary>
        public ImageEx()
        {
            DefaultStyleKey = typeof(ImageEx);
            Loaded += OnLoaded;
            _lockObj = new object();
        }

        /// <summary>
        /// Update the visual state of the control when its template is changed.
        /// </summary>
        protected override void OnApplyTemplate()
        {
            if (_image != null)
            {
                _image.ImageOpened -= OnImageOpened;
                _image.ImageFailed -= OnImageFailed;
            }

            _image = GetTemplateChild(PartImage) as Image;
            _progress = GetTemplateChild(PartProgress) as ProgressRing;

            _isInitialized = true;

            SetSource(Source);

            if (_image != null)
            {
                _image.ImageOpened += OnImageOpened;
                _image.ImageFailed += OnImageFailed;
            }

            base.OnApplyTemplate();
        }

        /// <inheritdoc/>
        protected override Size ArrangeOverride(Size finalSize)
        {
            var newSquareSize = Math.Min(finalSize.Width, finalSize.Height) / 8.0;

            if (_progress?.Width == newSquareSize)
            {
                _progress.Height = newSquareSize;
            }

            return base.ArrangeOverride(finalSize);
        }

        private void OnImageOpened(object sender, RoutedEventArgs e)
        {
            ImageOpened?.Invoke(this, e);
            ImageExOpened?.Invoke(this, new ImageExOpenedEventArgs());
            VisualStateManager.GoToState(this, LoadedState, true);
        }

        private void OnImageFailed(object sender, ExceptionRoutedEventArgs e)
        {
            ImageFailed?.Invoke(this, e);
            ImageExFailed?.Invoke(this, new ImageExFailedEventArgs(new Exception(e.ErrorMessage)));
            VisualStateManager.GoToState(this, FailedState, true);
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (_image != null && _image.Source == null)
            {
                RefreshImage();
            }
        }

        private async void RefreshImage()
        {
            await LoadImageAsync();
        }
    }
}
