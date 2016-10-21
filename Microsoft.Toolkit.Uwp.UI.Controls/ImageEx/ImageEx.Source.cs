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
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace Microsoft.Toolkit.Uwp.UI.Controls
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
        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register(nameof(Source), typeof(object), typeof(ImageEx), new PropertyMetadata(null, SourceChanged));

        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1);

        private Uri _uri;
        private bool _isHttpSource;

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
            await _semaphore.WaitAsync();

            try
            {
                if (_isInitialized)
                {
                    _image.Source = null;

                    if (source == null)
                    {
                        VisualStateManager.GoToState(this, UnloadedState, true);
                        return;
                    }

                    VisualStateManager.GoToState(this, LoadingState, true);

                    var imageSource = source as ImageSource;
                    if (imageSource != null)
                    {
                        _image.Source = imageSource;
                        ImageExOpened?.Invoke(this, new ImageExOpenedEventArgs());
                        VisualStateManager.GoToState(this, LoadedState, true);
                        return;
                    }

                    _uri = source as Uri;
                    if (_uri == null)
                    {
                        var url = source as string ?? source.ToString();
                        if (!Uri.TryCreate(url, UriKind.RelativeOrAbsolute, out _uri))
                        {
                            VisualStateManager.GoToState(this, FailedState, true);
                            return;
                        }
                    }

                    _isHttpSource = IsHttpUri(_uri);
                    if (!_isHttpSource && !_uri.IsAbsoluteUri)
                    {
                        _uri = new Uri("ms-appx:///" + _uri.OriginalString.TrimStart('/'));
                    }

                    await LoadImageAsync();
                }
            }
            finally
            {
                _semaphore.Release();
            }
        }

        private async Task LoadImageAsync()
        {
            if (_uri != null)
            {
                if (IsCacheEnabled && _isHttpSource)
                {
                    try
                    {
                        _image.Source = await ImageCache.Instance.GetFromCacheAsync(_uri, Path.GetFileName(_uri.ToString()), true);
                        ImageExOpened?.Invoke(this, new ImageExOpenedEventArgs());
                        VisualStateManager.GoToState(this, LoadedState, true);
                    }
                    catch (Exception e)
                    {
                        ImageExFailed?.Invoke(this, new ImageExFailedEventArgs(e));
                        VisualStateManager.GoToState(this, FailedState, true);
                    }
                }
                else
                {
                    _image.Source = new BitmapImage(_uri);
                }
            }
        }
    }
}
