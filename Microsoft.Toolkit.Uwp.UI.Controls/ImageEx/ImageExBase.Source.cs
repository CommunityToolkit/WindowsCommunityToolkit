// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// Shared Code for ImageEx and RoundImageEx
    /// </summary>
    public partial class ImageExBase
    {
        /// <summary>
        /// Identifies the <see cref="Source"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register(nameof(Source), typeof(object), typeof(ImageExBase), new PropertyMetadata(null, SourceChanged));

        private Uri _uri;
        private bool _isHttpSource;
        private CancellationTokenSource _tokenSource = null;

        /// <summary>
        /// Gets or sets the source used by the image
        /// </summary>
        public object Source
        {
            get { return GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        private static void SourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as ImageExBase;

            if (e.OldValue == null || e.NewValue == null || !e.OldValue.Equals(e.NewValue))
            {
                control?.SetSource(e.NewValue);
            }
        }

        private static bool IsHttpUri(Uri uri)
        {
            return uri.IsAbsoluteUri && (uri.Scheme == "http" || uri.Scheme == "https");
        }

        private void AttachSource(ImageSource source)
        {
            var image = Image as Image;
            var brush = Image as ImageBrush;

            if (image != null)
            {
                image.Source = source;
            }
            else if (brush != null)
            {
                brush.ImageSource = source;
            }
        }

        private async void SetSource(object source)
        {
            if (!IsInitialized)
            {
                return;
            }

            this._tokenSource?.Cancel();

            this._tokenSource = new CancellationTokenSource();

            AttachSource(null);

            if (source == null)
            {
                VisualStateManager.GoToState(this, UnloadedState, true);
                return;
            }

            VisualStateManager.GoToState(this, LoadingState, true);

            var imageSource = source as ImageSource;
            if (imageSource != null)
            {
                AttachSource(imageSource);

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

            await LoadImageAsync(_uri);
        }

        private async Task LoadImageAsync(Uri imageUri)
        {
            if (_uri != null)
            {
                if (IsCacheEnabled && _isHttpSource)
                {
                    try
                    {
                        var propValues = new List<KeyValuePair<string, object>>();

                        if (DecodePixelHeight > 0)
                        {
                            propValues.Add(new KeyValuePair<string, object>(nameof(DecodePixelHeight), DecodePixelHeight));
                        }

                        if (DecodePixelWidth > 0)
                        {
                            propValues.Add(new KeyValuePair<string, object>(nameof(DecodePixelWidth), DecodePixelWidth));
                        }

                        if (propValues.Count > 0)
                        {
                            propValues.Add(new KeyValuePair<string, object>(nameof(DecodePixelType), DecodePixelType));
                        }

                        var img = await ImageCache.Instance.GetFromCacheAsync(imageUri, true, _tokenSource.Token, propValues);

                        lock (LockObj)
                        {
                            // If you have many imageEx in a virtualized listview for instance
                            // controls will be recycled and the uri will change while waiting for the previous one to load
                            if (_uri == imageUri)
                            {
                                AttachSource(img);
                                ImageExOpened?.Invoke(this, new ImageExOpenedEventArgs());
                                VisualStateManager.GoToState(this, LoadedState, true);
                            }
                        }
                    }
                    catch (OperationCanceledException)
                    {
                        // nothing to do as cancellation has been requested.
                    }
                    catch (Exception e)
                    {
                        lock (LockObj)
                        {
                            if (_uri == imageUri)
                            {
                                ImageExFailed?.Invoke(this, new ImageExFailedEventArgs(e));
                                VisualStateManager.GoToState(this, FailedState, true);
                            }
                        }
                    }
                }
                else
                {
                    AttachSource(new BitmapImage(_uri));
                }
            }
        }
    }
}