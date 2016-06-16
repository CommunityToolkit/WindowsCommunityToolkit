// *********************************************************
//  Copyright (c) Microsoft. All rights reserved.
//  This code is licensed under the MIT License (MIT).
//  THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
//  INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
//  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. 
//  IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, 
//  DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, 
//  TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH 
//  THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// *********************************************************
using System;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Windows.Toolkit.UI.Controls
{

    /// <summary>
    /// The ImageEx control extends the default Image platform control improving the performance and responsiveness of your Apps. 
    /// Source images are downloaded asynchronously showing a load indicator while in progress. 
    /// Once downloaded, the source image is stored in the App local cache to preserve resources and load time next time the image needs to be displayed.
    /// </summary>
    [TemplatePart(Name = "Image", Type = typeof(Image))]
    [TemplatePart(Name = "Progress", Type = typeof(ProgressRing))]
    public sealed partial class ImageEx : Control
    {
        private Image _image;
        private ProgressRing _progress;

        private bool _isInitialized;

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageEx"/> class.
        /// </summary>
        public ImageEx()
        {
            DefaultStyleKey = typeof(ImageEx);
            Loaded += OnLoaded;
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

            _image = GetTemplateChild("Image") as Image;
            _progress = GetTemplateChild("Progress") as ProgressRing;

            _isInitialized = true;

            SetSource(Source);

            if (_image != null)
            {
                _image.ImageOpened += OnImageOpened;
                _image.ImageFailed += OnImageFailed;
            }

            base.OnApplyTemplate();
        }

        /// <summary>
        /// Measures the size in layout required for child elements and determines a size for the control.
        /// </summary>
        /// <param name="availableSize">The available size that this element can give to child elements. Infinity can be specified as a value to indicate that the element will size to whatever content is available.</param>
        /// <returns>The size that this element determines it needs during layout, based on its calculations of child element sizes.</returns>
        protected override Size MeasureOverride(Size availableSize)
        {
            _progress.Width = _progress.Height = Math.Min(1024, Math.Min(availableSize.Width, availableSize.Height)) / 8.0;
            return base.MeasureOverride(availableSize);
        }

        private void OnImageOpened(object sender, RoutedEventArgs e)
        {
            ImageOpened?.Invoke(this, e);
        }

        private void OnImageFailed(object sender, ExceptionRoutedEventArgs e)
        {
            ImageFailed?.Invoke(this, e);
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
