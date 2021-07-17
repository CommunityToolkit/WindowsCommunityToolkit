// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Microsoft.Toolkit.Uwp.Helpers;
using Windows.Foundation;
using Windows.UI.Composition;
using Windows.UI.Core;
using Windows.UI.Xaml;

namespace Microsoft.Toolkit.Uwp.UI.Media
{
    /// <summary>
    /// Represents a brush which uses an Image to create a mask to be applied on a <see cref="RenderSurfaceBrushBase"/> derivative.
    /// </summary>
    public sealed class ImageMaskSurfaceBrush : RenderSurfaceBrushBase
    {
        private WeakEventListener<RenderSurfaceBrushBase, object, EventArgs> _targetUpdateListener;
        private WeakEventListener<ImageSurfaceOptions, object, EventArgs> _imageSurfaceOptionsUpdateListener;

        private CompositionMaskBrush _maskBrush;
        private Uri _uri;

        /// <summary>
        /// Target Dependency Property
        /// </summary>
        public static readonly DependencyProperty TargetProperty = DependencyProperty.Register(
            "Target",
            typeof(RenderSurfaceBrushBase),
            typeof(ImageMaskSurfaceBrush),
            new PropertyMetadata(null, OnTargetChanged));

        /// <summary>
        /// Gets or sets the RenderSurfaceBrush upon which the mask is applied.
        /// </summary>
        public RenderSurfaceBrushBase Target
        {
            get => (RenderSurfaceBrushBase)GetValue(TargetProperty);
            set => SetValue(TargetProperty, value);
        }

        /// <summary>
        /// Handles changes to the Target property.
        /// </summary>
        /// <param name="d"><see cref="ImageMaskSurfaceBrush" /></param>
        /// <param name="e">DependencyProperty changed event arguments</param>
        private static void OnTargetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var imageMaskSurfaceBrush = (ImageMaskSurfaceBrush)d;
            imageMaskSurfaceBrush.OnTargetChanged();
        }

        /// <summary>
        /// Instance handler for the changes to the Target dependency property.
        /// </summary>
        private void OnTargetChanged()
        {
            _targetUpdateListener?.Detach();
            _targetUpdateListener = null;

            if (Target != null)
            {
                _targetUpdateListener = new WeakEventListener<RenderSurfaceBrushBase, object, EventArgs>(Target)
                {
                    OnEventAction = async (instance, source, args) =>
                    {
                        await Window.Current.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                        {
                            OnSurfaceBrushUpdated(true);
                        });
                    }
                };

                Target.Updated += _targetUpdateListener.OnEvent;

                OnSurfaceBrushUpdated(true);
            }
        }

        /// <summary>
        /// Mask Dependency Property
        /// </summary>
        public static readonly DependencyProperty MaskProperty = DependencyProperty.Register(
            "Mask",
            typeof(object),
            typeof(ImageMaskSurfaceBrush),
            new PropertyMetadata(null, OnMaskChanged));

        /// <summary>
        /// Gets or sets the URI of the image that is used to create the mask.
        /// </summary>
        public object Mask
        {
            get => (object)GetValue(MaskProperty);
            set => SetValue(MaskProperty, value);
        }

        /// <summary>
        /// Handles changes to the Mask property.
        /// </summary>
        /// <param name="d"><see cref="ImageMaskSurfaceBrush" /></param>
        /// <param name="e">DependencyProperty changed event arguments</param>
        private static void OnMaskChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var imageMaskSurfaceBrush = (ImageMaskSurfaceBrush)d;
            imageMaskSurfaceBrush.OnMaskChanged();
        }

        /// <summary>
        /// Instance handler for the changes to the Mask dependency property.
        /// </summary>
        private void OnMaskChanged()
        {
            if (Mask == null)
            {
                return;
            }

            var uri = Mask as Uri;
            if (uri == null)
            {
                var url = Mask as string ?? Mask.ToString();
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
            typeof(ImageMaskSurfaceBrush),
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
        /// <param name="d"><see cref="ImageMaskSurfaceBrush" /></param>
        /// <param name="e">DependencyProperty changed event arguments</param>
        private static void OnImageOptionsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var imageMaskSurfaceBrush = (ImageMaskSurfaceBrush)d;
            imageMaskSurfaceBrush.OnImageOptionsChanged();
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

        /// <summary>
        /// Padding Dependency Property
        /// </summary>
        public static readonly DependencyProperty PaddingProperty = DependencyProperty.Register(
            "Padding",
            typeof(Thickness),
            typeof(ImageMaskSurfaceBrush),
            new PropertyMetadata(new Thickness(0d), OnPaddingChanged));

        /// <summary>
        /// Gets or sets the padding between the IImageMaskSurface outer bounds and the bounds of the area where the mask, created from the loaded image's alpha values, should be rendered.
        /// </summary>
        public Thickness Padding
        {
            get => (Thickness)GetValue(PaddingProperty);
            set => SetValue(PaddingProperty, value);
        }

        /// <summary>
        /// Handles changes to the Padding property.
        /// </summary>
        /// <param name="d"><see cref="ImageMaskSurfaceBrush" /></param>
        /// <param name="e">DependencyProperty changed event arguments</param>
        private static void OnPaddingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var imageMaskSurfaceBrush = (ImageMaskSurfaceBrush)d;
            imageMaskSurfaceBrush.OnPaddingChanged();
        }

        /// <summary>
        /// Instance handler for the changes to the Padding dependency property.
        /// </summary>
        private void OnPaddingChanged()
        {
            OnSurfaceBrushUpdated();
        }

        /// <inheritdoc/>
        protected async override void OnSurfaceBrushUpdated(bool createSurface = false)
        {
            if (Generator == null)
            {
                GetGeneratorInstance();
            }

            if (Target == null || Target.Brush == null || _uri == null || Generator == null)
            {
                return;
            }

            if (createSurface || (RenderSurface == null))
            {
                CompositionBrush?.Dispose();

                _maskBrush = Window.Current.Compositor.CreateMaskBrush();
                _maskBrush.Source = Target.Brush;

                RenderSurface = await Generator.CreateImageMaskSurfaceAsync(_uri, new Size(SurfaceWidth, SurfaceHeight), Padding, ImageOptions ?? ImageSurfaceOptions.DefaultImageMaskOptions);
                _maskBrush.Mask = Window.Current.Compositor.CreateSurfaceBrush(RenderSurface.Surface);
                CompositionBrush = _maskBrush;
            }
            else
            {
                ((IImageMaskSurface)RenderSurface)?.Redraw(new Size(SurfaceWidth, SurfaceHeight), Padding, ImageOptions ?? ImageSurfaceOptions.DefaultImageMaskOptions);
            }

            base.OnSurfaceBrushUpdated(createSurface);
        }
    }
}
