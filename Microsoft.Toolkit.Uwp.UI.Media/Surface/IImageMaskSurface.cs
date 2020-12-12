// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;
using Microsoft.Graphics.Canvas;
using Windows.Foundation;
using Windows.UI.Xaml;

namespace Microsoft.Toolkit.Uwp.UI.Media.Surface
{
    /// <summary>
    /// Interface for rendering a mask, using an Image's alpha values, onto an ICompositionSurface
    /// </summary>
    public interface IImageMaskSurface : IImageSurface
    {
        /// <summary>
        /// Gets the padding between the IImageMaskSurface outer bounds and the bounds of the area where
        /// the mask, created from the loaded image's alpha values, should be rendered.
        /// </summary>
        Thickness MaskPadding { get; }

        /// <summary>
        /// Redraws the IImageMaskSurface using the given CanvasBitmap's alpha values with the given padding.
        /// </summary>
        /// <param name="imageSurface">ImageSurface whose image's alpha values are to be used to create the mask.</param>
        /// <param name="padding">The padding between the IImageMaskSurface outer bounds and the bounds of the area where
        /// the mask, created from the loaded image's alpha values, should be rendered.</param>
        void Redraw(IImageSurface imageSurface, Thickness padding);

        /// <summary>
        /// Redraws the IImageMaskSurface using the given CanvasBitmap's alpha values with the given padding
        /// using the given options.
        /// </summary>
        /// <param name="imageSurface">ImageSurface whose image's alpha values are to be used to create the mask.</param>
        /// <param name="padding">The padding between the IImageMaskSurface outer bounds and the bounds of the area where
        /// the mask, created from the loaded image's alpha values, should be rendered.</param>
        /// <param name="options">Describes the image's resize, alignment and blur radius options in the allocated space.</param>
        void Redraw(IImageSurface imageSurface, Thickness padding, ImageSurfaceOptions options);

        /// <summary>
        /// Resizes and redraws the IImageMaskSurface using the given CanvasBitmap's alpha values with the given padding
        /// using the given options.
        /// </summary>
        /// <param name="imageSurface">ImageSurface whose image's alpha values are to be used to create the mask.</param>
        /// <param name="size">New size of the IImageMaskSurface.</param>
        /// <param name="padding">The padding between the IImageMaskSurface outer bounds and the bounds of the area where
        /// the mask, created from the loaded image's alpha values, should be rendered.</param>
        /// <param name="options">Describes the image's resize, alignment and blur radius options in the allocated space.</param>
        void Redraw(IImageSurface imageSurface, Size size, Thickness padding, ImageSurfaceOptions options);

        /// <summary>
        /// Redraws the IImageMaskSurface using the given CanvasBitmap's alpha values with the given padding.
        /// </summary>
        /// <param name="surfaceBitmap">Image whose alpha values are to be used to create the mask.</param>
        /// <param name="padding">The padding between the IImageMaskSurface outer bounds and the bounds of the area where
        /// the mask, created from the loaded image's alpha values, should be rendered.</param>
        void Redraw(CanvasBitmap surfaceBitmap, Thickness padding);

        /// <summary>
        /// Redraws the IImageMaskSurface using the given CanvasBitmap's alpha values with the given padding
        /// using the given options.
        /// </summary>
        /// <param name="surfaceBitmap">Image whose alpha values are to be used to create the mask.</param>
        /// <param name="padding">The padding between the IImageMaskSurface outer bounds and the bounds of the area where
        /// the mask, created from the loaded image's alpha values, should be rendered.</param>
        /// <param name="options">Describes the image's resize, alignment and blur radius options in the allocated space.</param>
        void Redraw(CanvasBitmap surfaceBitmap, Thickness padding,  ImageSurfaceOptions options);

        /// <summary>
        /// Redraws the IImageMaskSurface using the alpha values of the image in the given IImageMaskSurface.
        /// </summary>
        /// <param name="imageMaskSurface">IImageMaskSurface whose image's alpha values are to be used to create the mask.</param>
        void Redraw(IImageMaskSurface imageMaskSurface);

        /// <summary>
        /// Redraws the IImageMaskSurface using the alpha values of the image in the given IImageMaskSurface with the given padding
        /// and options.
        /// </summary>
        /// <param name="imageMaskSurface">IImageMaskSurface whose image's alpha values are to be used to create the mask.</param>
        /// <param name="padding">The padding between the IImageMaskSurface outer bounds and the bounds of the area where
        /// the mask, created from the loaded image's alpha values, should be rendered.</param>
        void Redraw(IImageMaskSurface imageMaskSurface, Thickness padding);

        /// <summary>
        /// Redraws the IImageMaskSurface using the alpha values of the image in the given IImageMaskSurface with the given options.
        /// </summary>
        /// <param name="imageMaskSurface">IImageMaskSurface whose image's alpha values are to be used to create the mask.</param>
        /// <param name="options">Describes the image's resize, alignment and blur radius options in the allocated space.</param>
        void Redraw(IImageMaskSurface imageMaskSurface, ImageSurfaceOptions options);

        /// <summary>
        /// Redraws the IImageMaskSurface using the alpha values of the image in the given IImageMaskSurface using the given padding
        /// and options.
        /// </summary>
        /// <param name="imageMaskSurface">IImageMaskSurface whose image's alpha values are to be used to create the mask.</param>
        /// <param name="padding">The padding between the IImageMaskSurface outer bounds and the bounds of the area where
        /// the mask, created from the loaded image's alpha values, should be rendered.</param>
        /// <param name="options">Describes the image's resize, alignment and blur radius options in the allocated space.</param>
        void Redraw(IImageMaskSurface imageMaskSurface, Thickness padding, ImageSurfaceOptions options);

        /// <summary>
        /// Resizes and redraws the IImageMaskSurface using the alpha values of the image in the given IImageMaskSurface
        /// with the given padding and options.
        /// </summary>
        /// <param name="imageMaskSurface">IImageMaskSurface whose image's alpha values are to be used to create the mask.</param>
        /// <param name="size">New size of the IImageMaskSurface.</param>
        /// <param name="padding">The padding between the IImageMaskSurface outer bounds and the bounds of the area where
        /// the mask, created from the loaded image's alpha values, should be rendered.</param>
        /// <param name="options">Describes the image's resize, alignment and blur radius options in the allocated space.</param>
        void Redraw(IImageMaskSurface imageMaskSurface, Size size, Thickness padding, ImageSurfaceOptions options);

        /// <summary>
        /// Resizes and redraws the IImageMaskSurface using the given CanvasBitmap's alpha values with the given padding
        /// using the given options.
        /// </summary>
        /// <param name="surfaceBitmap">Image whose alpha values are to be used to create the mask.</param>
        /// <param name="size">New size of the IImageMaskSurface.</param>
        /// <param name="padding">The padding between the IImageMaskSurface outer bounds and the bounds of the area where
        /// the mask, created from the loaded image's alpha values, should be rendered.</param>
        /// <param name="options">Describes the image's resize, alignment and blur radius options in the allocated space.</param>
        void Redraw(CanvasBitmap surfaceBitmap, Size size, Thickness padding, ImageSurfaceOptions options);

        /// <summary>
        /// Redraws the IImageMaskSurface by loading image from the new Uri and image options
        /// </summary>
        /// <param name="uri">Uri of the image to be loaded on to the image surface.</param>
        /// <param name="padding">The padding between the IImageMaskSurface outer bounds and the bounds of the area where
        /// the mask, created from the loaded image's alpha values, should be rendered.</param>
        /// <param name="options">Describes the image's resize, alignment and blur radius options in the allocated space.</param>
        /// <returns>Task</returns>
        Task RedrawAsync(Uri uri, Thickness padding, ImageSurfaceOptions options);

        /// <summary>
        /// Resizes the IImageMaskSurface with the given size and loads the image from the new Uri and uses its
        /// alpha values to redraw the IImageMaskSurface with the given padding using the given options.
        /// </summary>
        /// <param name="uri">Uri of the image to be loaded onto the IImageMaskSurface.</param>
        /// <param name="size">New size of the IImageMaskSurface</param>
        /// <param name="padding">The padding between the IImageMaskSurface outer bounds and the bounds of the area where
        /// the mask, created from the loaded image's alpha values, should be rendered.</param>
        /// <param name="options">Describes the image's resize, alignment and blur radius options in the allocated space.</param>
        /// <returns>Task</returns>
        Task RedrawAsync(Uri uri, Size size, Thickness padding, ImageSurfaceOptions options);

        /// <summary>
        /// Resizes the IImageMaskSurface to the new size and redraws it with the given padding using the given options.
        /// </summary>
        /// <param name="size">New size of the IImageMaskSurface</param>
        /// <param name="padding">The padding between the IImageMaskSurface outer bounds and the bounds of the area where
        /// the mask, created from the loaded image's alpha values, should be rendered.</param>
        /// <param name="options">The image's resize, alignment and blur radius options in the allocated space.</param>
        void Resize(Size size, Thickness padding, ImageSurfaceOptions options);
    }
}