// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Microsoft.Graphics.Canvas;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace Microsoft.Toolkit.Uwp.UI.Media
{
    /// <summary>
    /// Enum to define the location of the Reflection of a Visual.
    /// </summary>
    public enum ReflectionLocation
    {
        /// <summary>
        /// Reflection at bottom of visual
        /// </summary>
        Bottom = 0,

        /// <summary>
        /// Reflection at top of visual
        /// </summary>
        Top = 1,

        /// <summary>
        /// Reflection at left of visual
        /// </summary>
        Left = 2,

        /// <summary>
        /// Reflection at right of visual
        /// </summary>
        Right = 3
    }

    /// <summary>
    /// Class to define the various options that would influence the rendering of the image on the ImageSurface
    /// </summary>
    public class ImageSurfaceOptions : DependencyObject
    {
        /// <summary>
        /// Event which indicates that the components of the <see cref="ImageSurfaceOptions"/> have changed.
        /// </summary>
        public event EventHandler<EventArgs> Updated;

        /// <summary>
        /// Gets default ImageSurfaceOptions when AutoResize is True.
        /// Uniform Stretch and Center alignment.
        /// </summary>
        public static ImageSurfaceOptions Default =>
                    new ImageSurfaceOptions()
                    {
                        AutoResize = true,
                        Interpolation = CanvasImageInterpolation.HighQualityCubic,
                        Opacity = 1f,
                        Stretch = Stretch.Uniform,
                        HorizontalAlignment = AlignmentX.Center,
                        VerticalAlignment = AlignmentY.Center,
                        SurfaceBackgroundColor = Colors.Transparent,
                        BlurRadius = 0f
                    };

        /// <summary>
        /// Gets default ImageSurfaceOptions when AutoResize is False.
        /// Uniform Stretch and Center alignment.
        /// </summary>
        public static ImageSurfaceOptions DefaultOptimized =>
                    new ImageSurfaceOptions()
                    {
                        AutoResize = false,
                        Interpolation = CanvasImageInterpolation.HighQualityCubic,
                        Opacity = 1f,
                        Stretch = Stretch.Uniform,
                        HorizontalAlignment = AlignmentX.Center,
                        VerticalAlignment = AlignmentY.Center,
                        SurfaceBackgroundColor = Colors.Transparent,
                        BlurRadius = 0f
                    };

        /// <summary>
        /// Gets default ImageSurfaceOptions for IImageMaskSurface.
        /// Uniform Stretch and Center alignment
        /// </summary>
        public static ImageSurfaceOptions DefaultImageMaskOptions =>
            new ImageSurfaceOptions()
            {
                AutoResize = false,
                Interpolation = CanvasImageInterpolation.HighQualityCubic,
                Opacity = 1f,
                Stretch = Stretch.Uniform,
                HorizontalAlignment = AlignmentX.Center,
                VerticalAlignment = AlignmentY.Center,
                SurfaceBackgroundColor = Colors.Transparent,
                BlurRadius = 0f
            };

        /// <summary>
        /// Creates ImageSurfaceOptions for IImageMaskSurface for the given blurRadius -
        /// Uniform Stretch and Center alignment
        /// </summary>
        /// <param name="blurRadius">Radius of the Gaussian Blur to be applied on the IImageMaskSurface.</param>
        /// <returns><see cref="ImageSurfaceOptions"/> instance.</returns>
        public static ImageSurfaceOptions GetDefaultImageMaskOptionsForBlur(float blurRadius)
        {
            return new ImageSurfaceOptions()
            {
                AutoResize = false,
                Interpolation = CanvasImageInterpolation.HighQualityCubic,
                Opacity = 1f,
                Stretch = Stretch.Uniform,
                HorizontalAlignment = AlignmentX.Center,
                VerticalAlignment = AlignmentY.Center,
                SurfaceBackgroundColor = Colors.Transparent,
                BlurRadius = blurRadius
            };
        }

        /// <summary>
        /// AutoResize Dependency Property
        /// </summary>
        public static readonly DependencyProperty AutoResizeProperty = DependencyProperty.Register(
            "AutoResize",
            typeof(bool),
            typeof(ImageSurfaceOptions),
            new PropertyMetadata(true, OnPropertyChanged));

        /// <summary>
        /// <para>Gets or sets a value indicating whether specifies whether the IImageSurface should resize itself automatically to match the loaded image size.</para>
        /// <para>NOTE: This property is not used by ImageMaskSurfaceBrush or IImageMaskSurface.</para>
        /// </summary>
        public bool AutoResize
        {
            get => (bool)GetValue(AutoResizeProperty);
            set => SetValue(AutoResizeProperty, value);
        }

        /// <summary>
        /// Stretch Dependency Property
        /// </summary>
        public static readonly DependencyProperty StretchProperty = DependencyProperty.Register(
            "Stretch",
            typeof(Stretch),
            typeof(ImageSurfaceOptions),
            new PropertyMetadata(Stretch.Uniform, OnPropertyChanged));

        /// <summary>
        /// <para>Gets or sets a value describing how image is resized to fill its allocated space.</para>
        /// <para>NOTE: This property is taken into consideration only if AutoResize is false.</para>
        /// </summary>
        public Stretch Stretch
        {
            get => (Stretch)GetValue(StretchProperty);
            set => SetValue(StretchProperty, value);
        }

        /// <summary>
        /// HorizontalAlignment Dependency Property
        /// </summary>
        public static readonly DependencyProperty HorizontalAlignmentProperty = DependencyProperty.Register(
            "HorizontalAlignment",
            typeof(AlignmentX),
            typeof(ImageSurfaceOptions),
            new PropertyMetadata(AlignmentX.Center, OnPropertyChanged));

        /// <summary>
        /// <para>Gets or sets a value describing how image is positioned horizontally in the IImageSurface or IImageMaskSurface.</para>
        /// <para>NOTE: This property is taken into consideration only if AutoResize is False.</para>
        /// </summary>
        public AlignmentX HorizontalAlignment
        {
            get => (AlignmentX)GetValue(HorizontalAlignmentProperty);
            set => SetValue(HorizontalAlignmentProperty, value);
        }

        /// <summary>
        /// VerticalAlignment Dependency Property
        /// </summary>
        public static readonly DependencyProperty VerticalAlignmentProperty = DependencyProperty.Register(
            "VerticalAlignment",
            typeof(AlignmentY),
            typeof(ImageSurfaceOptions),
            new PropertyMetadata(AlignmentY.Center, OnPropertyChanged));

        /// <summary>
        /// <para>Gets or sets a value describing how image is positioned vertically in the IImageSurface or IImageMaskSurface.</para>
        /// <para>NOTE: This property is taken into consideration only if AutoResize is False.</para>
        /// </summary>
        public AlignmentY VerticalAlignment
        {
            get => (AlignmentY)GetValue(VerticalAlignmentProperty);
            set => SetValue(VerticalAlignmentProperty, value);
        }

        /// <summary>
        /// Opacity Dependency Property
        /// </summary>
        public static readonly DependencyProperty OpacityProperty = DependencyProperty.Register(
            "Opacity",
            typeof(double),
            typeof(ImageSurfaceOptions),
            new PropertyMetadata(1d, OnPropertyChanged));

        /// <summary>
        /// Gets or sets the the opacity of the rendered the image in an IImageSurface or the mask in an IImageMaskSurface.
        /// </summary>
        public double Opacity
        {
            get => (double)GetValue(OpacityProperty);
            set => SetValue(OpacityProperty, value);
        }

        /// <summary>
        /// Interpolation Dependency Property
        /// </summary>
        public static readonly DependencyProperty InterpolationProperty = DependencyProperty.Register(
            "Interpolation",
            typeof(CanvasImageInterpolation),
            typeof(ImageSurfaceOptions),
            new PropertyMetadata(CanvasImageInterpolation.HighQualityCubic, OnPropertyChanged));

        /// <summary>
        /// Gets or sets the the interpolation used to render the image in an IImageSurface or the mask in an IImageMaskSurface.
        /// </summary>
        public CanvasImageInterpolation Interpolation
        {
            get => (CanvasImageInterpolation)GetValue(InterpolationProperty);
            set => SetValue(InterpolationProperty, value);
        }

        /// <summary>
        /// SurfaceBackgroundColor Dependency Property
        /// </summary>
        public static readonly DependencyProperty SurfaceBackgroundColorProperty = DependencyProperty.Register(
            "SurfaceBackgroundColor",
            typeof(Color),
            typeof(ImageSurfaceOptions),
            new PropertyMetadata(Colors.Transparent, OnPropertyChanged));

        /// <summary>
        /// Gets or sets the color which will be used to fill the IImageSurface in an IImageSurface or the mask in an IImageMaskSurface
        /// in case the image is not rendered.
        /// </summary>
        public Color SurfaceBackgroundColor
        {
            get => (Color)GetValue(SurfaceBackgroundColorProperty);
            set => SetValue(SurfaceBackgroundColorProperty, value);
        }

        /// <summary>
        /// BlurRadius Dependency Property
        /// </summary>
        public static readonly DependencyProperty BlurRadiusProperty = DependencyProperty.Register(
            "BlurRadius",
            typeof(double),
            typeof(ImageSurfaceOptions),
            new PropertyMetadata(0d, OnPropertyChanged));

        /// <summary>
        /// <para>Gets or sets the radius of the Gaussian blur to be applied to the IImageMaskSurface.</para>
        /// <para>NOTE: This property is not used by IImageSurface.</para>
        /// </summary>
        public double BlurRadius
        {
            get => (double)GetValue(BlurRadiusProperty);
            set => SetValue(BlurRadiusProperty, value);
        }

        /// <summary>
        /// Method that is called whenever the dependency properties of the ImageSurfaceOptions changes.
        /// </summary>
        /// <param name="d">The object whose property has changed.</param>
        /// <param name="e">Event arguments.</param>
        private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var options = (ImageSurfaceOptions)d;

            options.OnUpdated();
        }

        /// <summary>
        /// Raises the Updated event.
        /// </summary>
        private void OnUpdated()
        {
            Updated?.Invoke(this, null);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageSurfaceOptions"/> class.
        /// </summary>
        public ImageSurfaceOptions()
        {
            AutoResize = false;
            Interpolation = CanvasImageInterpolation.HighQualityCubic;
            Opacity = 1f;
            Stretch = Stretch.Uniform;
            HorizontalAlignment = AlignmentX.Center;
            VerticalAlignment = AlignmentY.Center;
            SurfaceBackgroundColor = Colors.Transparent;
            BlurRadius = 0f;
        }
    }
}
