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
using global::Windows.UI.Xaml.Controls;
using global::Windows.UI.Xaml.Data;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace Microsoft.Windows.Toolkit.UI.Controls
{
    /// <summary>
    /// The ImageEx control extends the default Image platform control improving the performance and responsiveness of your Apps.
    /// Source images are downloaded asynchronously showing a load indicator while in progress.
    /// Once downloaded, the source image is stored in the App local cache to preserve resources and load time next time the image needs to be displayed.
    /// </summary>
    public partial class ImageEx
    {
        /// <summary>
        /// Identifies the <see cref="Source"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register("Source", typeof(object), typeof(ImageEx), new PropertyMetadata(null, SourceChanged));

        private Uri _uri;
        private bool _isHttpSource;
        private bool _isLoadingImage;

        /// <summary>
        /// Gets or sets get or set the source used by the image
        /// </summary>
        public object Source
        {
            get { return GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        private static void SourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as ImageEx;
            control?.SetSource(e.NewValue);
        }

        private static bool IsHttpUri(Uri uri)
        {
            return uri.IsAbsoluteUri && (uri.Scheme == "http" || uri.Scheme == "https");
        }

        private async void SetSource(object source)
        {
            if (_isInitialized)
            {
                _image.Source = null;

                if (source == null)
                {
                    VisualStateManager.GoToState(this, "Unloaded", true);
                    return;
                }

                VisualStateManager.GoToState(this, "Loading", true);

                var sourceString = source as string;
                if (sourceString != null)
                {
                    string url = sourceString;
                    if (Uri.TryCreate(url, UriKind.RelativeOrAbsolute, out _uri))
                    {
                        _isHttpSource = IsHttpUri(_uri);
                        if (!_isHttpSource && !_uri.IsAbsoluteUri)
                        {
                            _uri = new Uri("ms-appx:///" + url.TrimStart('/'));
                        }

                        await LoadImageAsync();
                    }
                }
                else
                {
                    _image.Source = source as ImageSource;
                }

                VisualStateManager.GoToState(this, "Loaded", true);
            }
        }

        private async Task LoadImageAsync()
        {
            if (!_isLoadingImage && _uri != null)
            {
                _isLoadingImage = true;
                if (IsCacheEnabled && _isHttpSource)
                {
                    _image.Source = await ImageCache.LoadFromCacheAsync(_uri);
                }
                else
                {
                    _image.Source = new BitmapImage(_uri);
                }

                _isLoadingImage = false;
            }
        }
    }
}
