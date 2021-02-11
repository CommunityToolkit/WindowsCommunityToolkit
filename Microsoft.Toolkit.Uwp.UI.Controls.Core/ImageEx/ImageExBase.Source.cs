// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// Base code for ImageEx
    /// </summary>
    public partial class ImageExBase
    {
        /// <summary>
        /// Identifies the <see cref="Source"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register(nameof(Source), typeof(object), typeof(ImageExBase), new PropertyMetadata(null, SourceChanged));

        /// <summary>
        /// Gets value tracking the currently requested source Uri. This can be helpful to use when implementing <see cref="AttachCachedResourceAsync(Uri)"/> where loading an image from a cache takes longer and the current container has been recycled and is no longer valid since a new image has been set.
        /// </summary>
        protected Uri CurrentSourceUri { get; private set; }

        private object _lazyLoadingSource;

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

            if (control == null)
            {
                return;
            }

            if (e.OldValue == null || e.NewValue == null || !e.OldValue.Equals(e.NewValue))
            {
                if (e.NewValue == null || !control.EnableLazyLoading || control._isInViewport)
                {
                    control._lazyLoadingSource = null;
                    control.SetSource(e.NewValue);
                }
                else
                {
                    control._lazyLoadingSource = e.NewValue;
                }
            }
        }

        private static bool IsHttpUri(Uri uri)
        {
            return uri.IsAbsoluteUri && (uri.Scheme == "http" || uri.Scheme == "https");
        }

        /// <summary>
        /// Method to call to assign an <see cref="ImageSource"/> value to the underlying <see cref="Image"/> powering <see cref="ImageExBase"/>.
        /// </summary>
        /// <param name="source"><see cref="ImageSource"/> to assign to the image.</param>
        protected void AttachSource(ImageSource source)
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

            OnNewSourceRequested(source);

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

            CurrentSourceUri = source as Uri;
            if (CurrentSourceUri == null)
            {
                var url = source as string ?? source.ToString();
                if (!Uri.TryCreate(url, UriKind.RelativeOrAbsolute, out Uri uri))
                {
                    VisualStateManager.GoToState(this, FailedState, true);
                    return;
                }

                CurrentSourceUri = uri;
            }

            if (!IsHttpUri(CurrentSourceUri) && !CurrentSourceUri.IsAbsoluteUri)
            {
                CurrentSourceUri = new Uri("ms-appx:///" + CurrentSourceUri.OriginalString.TrimStart('/'));
            }

            await LoadImageAsync(CurrentSourceUri);
        }

        private async Task LoadImageAsync(Uri imageUri)
        {
            if (imageUri != null)
            {
                if (IsCacheEnabled)
                {
                    await AttachCachedResourceAsync(imageUri);
                }
                else if (string.Equals(imageUri.Scheme, "data", StringComparison.OrdinalIgnoreCase))
                {
                    var source = imageUri.OriginalString;
                    const string base64Head = "base64,";
                    var index = source.IndexOf(base64Head);
                    if (index >= 0)
                    {
                        var bytes = Convert.FromBase64String(source.Substring(index + base64Head.Length));
                        var bitmap = new BitmapImage();
                        AttachSource(bitmap);
                        await bitmap.SetSourceAsync(new MemoryStream(bytes).AsRandomAccessStream());
                    }
                }
                else
                {
                    AttachSource(new BitmapImage(imageUri)
                    {
                        CreateOptions = BitmapCreateOptions.IgnoreImageCache
                    });
                }
            }
        }

        /// <summary>
        /// This method is provided in case a developer would like their own custom caching strategy for <see cref="ImageExBase"/>.
        /// By default it uses the built-in UWP cache provided by <see cref="BitmapImage"/> and
        /// the <see cref="Image"/> control itself. This method should call <see cref="AttachSource(ImageSource)"/>
        /// to set the retrieved cache value to the image. <see cref="CurrentSourceUri"/> may be checked
        /// after retrieving a cached image to ensure that the current resource requested matches the one
        /// requested by the <see cref="AttachCachedResourceAsync(Uri)"/> parameter.
        /// <see cref="OnNewSourceRequested(object)"/> may be used in order to signal any cancellation events
        /// using a <see cref="CancellationToken"/> to the call to the cache, for instance like the Toolkit's
        /// own <see cref="CacheBase{T}.GetFromCacheAsync(Uri, bool, CancellationToken, List{KeyValuePair{string, object}})"/> in <see cref="ImageCache"/>.
        /// </summary>
        /// <example>
        /// <code>
        /// try
        /// {
        ///     var propValues = new List&lt;KeyValuePair&lt;string, object>>();
        ///
        ///     if (DecodePixelHeight > 0)
        ///     {
        ///         propValues.Add(new KeyValuePair&lt;string, object>(nameof(DecodePixelHeight), D ecodePixelHeight));
        ///     }
        ///     if (DecodePixelWidth > 0)
        ///     {
        ///         propValues.Add(new KeyValuePair&lt;string, object>(nameof(DecodePixelWidth), D ecodePixelWidth));
        ///     }
        ///     if (propValues.Count > 0)
        ///     {
        ///         propValues.Add(new KeyValuePair&lt;string, object>(nameof(DecodePixelType), DecodePixelType));
        ///     }
        ///
        ///     // A token could be provided here as well to cancel the request to the cache,
        ///     // if a new image is requested. That token can be canceled in the OnNewSourceRequested method.
        ///     var img = await ImageCache.Instance.GetFromCacheAsync(imageUri, true, initializerKeyValues: propValues);
        ///
        ///     lock (LockObj)
        ///     {
        ///         // If you have many imageEx in a virtualized ListView for instance
        ///         // controls will be recycled and the uri will change while waiting for the previous one to load
        ///         if (CurrentSourceUri == imageUri)
        ///         {
        ///             AttachSource(img);
        ///             ImageExOpened?.Invoke(this, new ImageExOpenedEventArgs());
        ///             VisualStateManager.GoToState(this, LoadedState, true);
        ///         }
        ///     }
        /// }
        /// catch (OperationCanceledException)
        /// {
        ///     // nothing to do as cancellation has been requested.
        /// }
        /// catch (Exception e)
        /// {
        ///     lock (LockObj)
        ///     {
        ///         if (CurrentSourceUri == imageUri)
        ///         {
        ///             ImageExFailed?.Invoke(this, new ImageExFailedEventArgs(e));
        ///             VisualStateManager.GoToState(this, FailedState, true);
        ///         }
        ///     }
        /// }
        /// </code>
        /// </example>
        /// <param name="imageUri"><see cref="Uri"/> of the image to load from the cache.</param>
        /// <returns><see cref="Task"/></returns>
        protected virtual Task AttachCachedResourceAsync(Uri imageUri)
        {
            // By default we just use the built-in UWP image cache provided within the Image control.
            AttachSource(new BitmapImage(imageUri));

            return Task.CompletedTask;
        }

        /// <summary>
        /// This method is called when a new source is requested by the control. This can be useful when
        /// implementing a custom caching strategy to cancel any open request on the cache if a new
        /// request comes in due to container recycling before the previous one has completed.
        /// Be default, this method does nothing.
        /// </summary>
        /// <param name="source">Incoming requested source.</param>
        protected virtual void OnNewSourceRequested(object source)
        {
        }
    }
}