using Microsoft.Windows.Toolkit.UI;
using System;
using System.Threading.Tasks;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace Microsoft.Windows.Toolkit.UI.Controls
{
    partial class ImageEx
    {
        /// <summary>
        /// Identifies the <see cref="Source"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register("Source", typeof(object), typeof(ImageEx), new PropertyMetadata(null, SourceChanged));

        private Uri _uri;
        private bool _isHttpSource;
        private bool _isLoadingImage;

        /// <summary>
        /// Get or set the source used by the image
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

        private async void SetSource(object source)
        {
            if (_isInitialized)
            {
                _image.Source = null;
                if (source == null)
                {
                    return;
                }

                var sourceString = source as string;
                if (sourceString != null)
                {
                    string url = sourceString;
                    if (Uri.TryCreate(url, UriKind.RelativeOrAbsolute, out _uri))
                    {
                        _isHttpSource = IsHttpUri(_uri);
                        if (_isHttpSource)
                        {
                            _image.Opacity = 0.0;
                            _progress.IsActive = true;
                        }
                        else
                        {
                            if (!_uri.IsAbsoluteUri)
                            {
                                _uri = new Uri("ms-appx:///" + url.TrimStart('/'));
                            }
                        }
                        await LoadImageAsync();
                    }
                }
                else
                {
                    _image.Source = source as ImageSource;
                }

                _progress.IsActive = false;
               //TODO: need to call this when animations will be merged _image.FadeIn();
                _image.Opacity = 1.0;
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

        private static bool IsHttpUri(Uri uri)
        {
            return uri.IsAbsoluteUri && (uri.Scheme == "http" || uri.Scheme == "https");
        }
    }
}
