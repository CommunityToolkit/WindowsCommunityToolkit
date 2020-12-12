// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Graphics.Canvas;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace Microsoft.Toolkit.Uwp.UI.Media.Surface
{
    /// <summary>
    /// Enum to define the location of the
    /// Reflection of a Visual.
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
    /// Class to define the various options that would
    /// influence the rendering of the image on the ImageSurface
    /// </summary>
    public class ImageSurfaceOptions
    {
        /// <summary>
        /// Gets default ImageSurfaceOptions when AutoResize is True
        /// Uniform Stretch and Center alignment
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
        /// Gets default ImageSurfaceOptions when AutoResize is False
        /// Uniform Stretch and Center alignment
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
        /// Gets default ImageSurfaceOptions for IImageMaskSurface
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
        /// <para>Gets or sets a value indicating whether specifies whether the IImageSurface should resize itself automatically to match the loaded image size.</para>
        /// <para>NOTE: This property is not used by IImageMaskSurface.</para>
        /// </summary>
        public bool AutoResize { get; set; }

        /// <summary>
        /// <para>Gets or sets describes how image is resized to fill its allocated space.</para>
        /// <para>NOTE: This property is taken into consideration only if AutoResize is False.</para>
        /// </summary>
        public Stretch Stretch { get; set; } = Stretch.None;

        /// <summary>
        /// <para>Gets or sets describes how image is positioned horizontally in the IImageSurface or IImageMaskSurface.</para>
        /// <para>NOTE: This property is taken into consideration only if AutoResize is False.</para>
        /// </summary>
        public AlignmentX HorizontalAlignment { get; set; } = AlignmentX.Center;

        /// <summary>
        /// <para>Gets or sets describes how image is positioned vertically in the IImageSurface or IImageMaskSurface.</para>
        /// <para>NOTE: This property is taken into consideration only if AutoResize is False.</para>
        /// </summary>
        public AlignmentY VerticalAlignment { get; set; } = AlignmentY.Center;

        /// <summary>
        /// Gets or sets specifies the opacity of the rendered the image in an IImageSurface or the mask in an IImageMaskSurface.
        /// </summary>
        public float Opacity { get; set; } = 1f;

        /// <summary>
        /// Gets or sets specifies the interpolation used to render the image in an IImageSurface or the mask in an IImageMaskSurface.
        /// </summary>
        public CanvasImageInterpolation Interpolation { get; set; } = CanvasImageInterpolation.HighQualityCubic;

        /// <summary>
        /// Gets or sets color which will be used to fill the IImageSurface in an IImageSurface or the mask in an IImageMaskSurface
        /// in case the image is not rendered.
        /// </summary>
        public Color SurfaceBackgroundColor { get; set; } = Colors.Transparent;

        /// <summary>
        /// <para>Gets or sets radius of the Gaussian blur to be applied to the IImageMaskSurface.</para>
        /// <para>NOTE: This property is not used by IImageSurface.</para>
        /// </summary>
        public float BlurRadius { get; set; }
    }
}
