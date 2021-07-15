// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Microsoft.Toolkit.Uwp.Helpers;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;

namespace Microsoft.Toolkit.Uwp.UI.Media
{
    /// <summary>
    /// Creates a Render Surface brush using an image
    /// </summary>
    public class ImageSurfaceBrush : RenderSurfaceBrushBase
    {
        private WeakEventListener<ImageSurfaceOptions, object, EventArgs> _imageSurfaceOptionsUpdateListener;

        private Uri _uri;

        /// <summary>
        /// Background Dependency Property
        /// </summary>
        public static readonly DependencyProperty BackgroundProperty = DependencyProperty.Register(
            "Background",
            typeof(Color),
            typeof(ImageSurfaceBrush),
            new PropertyMetadata(Colors.Transparent, OnBackgroundChanged));

        /// <summary>
        /// Gets or sets the color that is rendered in the transparent areas of the Image. The default value is Colors.Transparent.
        /// </summary>
        public Color Background
        {
            get => (Color)GetValue(BackgroundProperty);
            set => SetValue(BackgroundProperty, value);
        }

        /// <summary>
        /// Handles changes to the Background property.
        /// </summary>
        /// <param name="d"><see cref="ImageSurfaceBrush" /></param>
        /// <param name="e">DependencyProperty changed event arguments</param>
        private static void OnBackgroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var imageSurfaceBrush = (ImageSurfaceBrush)d;
            imageSurfaceBrush.OnBackgroundChanged();
        }

        /// <summary>
        /// Instance handler for the changes to the Background dependency property.
        /// </summary>
        private void OnBackgroundChanged()
        {
            OnSurfaceBrushUpdated();
        }

        /// <summary>
        /// Source Dependency Property
        /// </summary>
        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register(
            "Source",
            typeof(object),
            typeof(ImageSurfaceBrush),
            new PropertyMetadata(null, OnSourceChanged));

        /// <summary>
        /// Gets or sets the the .
        /// </summary>
        public object Source
        {
            get => (object)GetValue(SourceProperty);
            set => SetValue(SourceProperty, value);
        }

        /// <summary>
        /// Handles changes to the Source property.
        /// </summary>
        /// <param name="d"><see cref="ImageSurfaceBrush" /></param>
        /// <param name="e">DependencyProperty changed event arguments</param>
        private static void OnSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var target = (ImageSurfaceBrush)d;
            target.OnSourceChanged();
        }

        /// <summary>
        /// Instance handler for the changes to the Source dependency property.
        /// </summary>
        private void OnSourceChanged()
        {
            if (Source == null)
            {
                return;
            }

            var uri = Source as Uri;

            if (uri == null)
            {
                var url = Source as string ?? Source.ToString();
                if (!Uri.TryCreate(url, UriKind.RelativeOrAbsolute, out uri))
                {
                    _uri = null;
                    return;
                }
            }

            if (!IsHttpUri(uri) && !uri.IsAbsoluteUri)
            {
                _uri = new Uri("ms-appx:///" + uri.OriginalString.TrimStart('/'));
            }

            _uri = uri;

            OnSurfaceBrushUpdated(true);
        }

        /// <summary>
        /// ImageOptions Dependency Property
        /// </summary>
        public static readonly DependencyProperty ImageOptionsProperty = DependencyProperty.Register(
            "ImageOptions",
            typeof(ImageSurfaceOptions),
            typeof(ImageSurfaceBrush),
            new PropertyMetadata(null, OnImageOptionsChanged));

        /// <summary>
        /// Gets or sets the additional options that can be used to configure the image used to create the brush.
        /// </summary>
        public ImageSurfaceOptions ImageOptions
        {
            get => (ImageSurfaceOptions)GetValue(ImageOptionsProperty);
            set => SetValue(ImageOptionsProperty, value);
        }

        /// <summary>
        /// Handles changes to the ImageOptions property.
        /// </summary>
        /// <param name="d"><see cref="ImageSurfaceBrush" /></param>
        /// <param name="e">DependencyProperty changed event arguments</param>
        private static void OnImageOptionsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var imageSurfaceBrush = (ImageSurfaceBrush)d;
            imageSurfaceBrush.OnImageOptionsChanged();
        }

        /// <summary>
        /// Instance handler for the changes to the ImageOptions dependency property.
        /// </summary>
        private void OnImageOptionsChanged()
        {
            _imageSurfaceOptionsUpdateListener?.Detach();
            _imageSurfaceOptionsUpdateListener = null;

            if (ImageOptions != null)
            {
                _imageSurfaceOptionsUpdateListener = new WeakEventListener<ImageSurfaceOptions, object, EventArgs>(ImageOptions)
                {
                    OnEventAction = async (instance, source, args) =>
                    {
                        await Window.Current.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                        {
                            OnSurfaceBrushUpdated();
                        });
                    }
                };

                ImageOptions.Updated += _imageSurfaceOptionsUpdateListener.OnEvent;

                OnSurfaceBrushUpdated();
            }
        }

        /// <inheritdoc/>
        protected async override void OnSurfaceBrushUpdated(bool createSurface = false)
        {
            if (Generator == null)
            {
                GetGeneratorInstance();
            }

            if (_uri == null || Generator == null)
            {
                return;
            }

            if (createSurface || (RenderSurface == null))
            {
                CompositionBrush?.Dispose();
                RenderSurface = await Generator?.CreateImageSurfaceAsync(_uri, new Size(SurfaceWidth, SurfaceHeight), ImageOptions ?? ImageSurfaceOptions.Default);
                CompositionBrush = Window.Current.Compositor.CreateSurfaceBrush(RenderSurface.Surface);
            }
            else
            {
                ((IImageSurface)RenderSurface)?.Redraw(new Size(SurfaceWidth, SurfaceHeight), ImageOptions ?? ImageSurfaceOptions.Default);
            }

            base.OnSurfaceBrushUpdated(createSurface);
        }
    }
}
