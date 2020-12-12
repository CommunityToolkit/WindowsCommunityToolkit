// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;
using Microsoft.Graphics.Canvas;
using Windows.Foundation;

namespace Microsoft.Toolkit.Uwp.UI.Media.Surface
{
    /// <summary>
    /// Enumeration to describe the status of the loading of an image
    /// on the IImageSurface
    /// </summary>
    public enum ImageSurfaceLoadStatus
    {
        /// <summary>
        /// Indicates that no image has been loaded on the IImageSurface
        /// </summary>
        None,

        /// <summary>
        /// Indicates that the image was successfully loaded on the IImageSurface.
        /// </summary>
        Success,

        /// <summary>
        /// Indicates that the image could not be loaded on the IImageSurface.
        /// </summary>
        Error
    }

    /// <summary>
    /// Interface for rendering an image onto an ICompositionSurface
    /// </summary>
    public interface IImageSurface : IRenderSurface
    {
        /// <summary>
        /// Event that is raised when the image has been downloaded, decoded and loaded
        /// to the underlying IImageSurface. This event fires regardless of success or failure.
        /// </summary>
        event TypedEventHandler<IImageSurface, ImageSurfaceLoadStatus> LoadCompleted;

        /// <summary>
        /// Gets the Uri of the image to be loaded onto the IImageSurface
        /// </summary>
        Uri Uri { get; }

        /// <summary>
        /// Gets the CanvasBitmap representing the loaded image
        /// </summary>
        CanvasBitmap SurfaceBitmap { get; }

        /// <summary>
        /// Gets the image's resize and alignment options in the allocated space.
        /// </summary>
        ImageSurfaceOptions Options { get; }

        /// <summary>
        /// Gets the size of the decoded image in physical pixels.
        /// </summary>
        Size DecodedPhysicalSize { get; }

        /// <summary>
        /// Gets the size of the decoded image in device independent pixels.
        /// </summary>
        Size DecodedSize { get; }

        /// <summary>
        /// Gets the status whether the image was loaded successfully or not.
        /// </summary>
        ImageSurfaceLoadStatus Status { get; }

        /// <summary>
        /// Redraws the IImageSurface or IImageMaskSurface with the given image options.
        /// </summary>
        /// <param name="options">The image's resize and alignment options in the allocated space.</param>
        void Redraw(ImageSurfaceOptions options);

        /// <summary>
        /// Redraws the IImageSurface (using the image in the given imageSurface) or the IImageMaskSurface
        /// (using the alpha values of image in the given imageSurface).
        /// </summary>
        /// <param name="imageSurface">IImageSurface whose image is to be loaded on the surface.</param>
        void Redraw(IImageSurface imageSurface);

        /// <summary>
        /// Redraws the IImageSurface (using the given CanvasBitmap) or the IImageMaskSurface
        /// (using the given CanvasBitmap's alpha values) using the given options.
        /// </summary>
        /// <param name="imageSurface">IImageSurface whose image is to be loaded on the surface.</param>
        /// <param name="options">Describes the image's resize, alignment options in the allocated space.</param>
        void Redraw(IImageSurface imageSurface, ImageSurfaceOptions options);

        /// <summary>
        /// Resizes and redraws the IImageSurface (using the given CanvasBitmap) or the IImageMaskSurface
        /// (using the given CanvasBitmap's alpha values) using the given options.
        /// </summary>
        /// <param name="imageSurface">IImageSurface whose image is to be loaded on the surface.</param>
        /// <param name="size">New size of the IImageMaskSurface.</param>
        /// <param name="options">Describes the image's resize, alignment options in the allocated space.</param>
        void Redraw(IImageSurface imageSurface, Size size, ImageSurfaceOptions options);

        /// <summary>
        /// Redraws the IImageSurface (using the given CanvasBitmap) or the IImageMaskSurface
        /// (using the given CanvasBitmap's alpha values).
        /// </summary>
        /// <param name="surfaceBitmap">Image to be loaded on the surface.</param>
        void Redraw(CanvasBitmap surfaceBitmap);

        /// <summary>
        /// Redraws the IImageSurface (using the given CanvasBitmap) or the IImageMaskSurface
        /// (using the given CanvasBitmap's alpha values) using the given options.
        /// </summary>
        /// <param name="surfaceBitmap">Image to be loaded on the surface.</param>
        /// <param name="options">Describes the image's resize, alignment options in the allocated space.</param>
        void Redraw(CanvasBitmap surfaceBitmap, ImageSurfaceOptions options);

        /// <summary>
        /// Resizes and redraws the IImageSurface (using the given CanvasBitmap) or the IImageMaskSurface
        /// (using the given CanvasBitmap's alpha values) using the given options.
        /// </summary>
        /// <param name="surfaceBitmap">Image to be loaded on the surface..</param>
        /// <param name="size">New size of the IImageMaskSurface.</param>
        /// <param name="options">Describes the image's resize, alignment options in the allocated space.</param>
        void Redraw(CanvasBitmap surfaceBitmap, Size size, ImageSurfaceOptions options);

        /// <summary>
        /// Redraws the IImageSurface or IImageMaskSurface by loading image from the new Uri and applying the image options.
        /// </summary>
        /// <param name="uri">Uri of the image to be loaded on to the surface.</param>
        /// <param name="options">The image's resize and alignment options in the allocated space.</param>
        /// <returns>Task</returns>
        Task RedrawAsync(Uri uri, ImageSurfaceOptions options);

        /// <summary>
        /// Resizes the IImageSurface or IImageMaskSurface with the given size and redraws it by loading
        /// image from the new Uri.
        /// </summary>
        /// <param name="uri">Uri of the image to be loaded onto the IImageSurface.</param>
        /// <param name="size">New size of the IImageSurface</param>
        /// <param name="options">The image's resize and alignment options in the allocated space.</param>
        /// <returns>Task</returns>
        Task RedrawAsync(Uri uri, Size size, ImageSurfaceOptions options);

        /// <summary>
        /// Resizes the IImageSurface or IImageMaskSurface to the new size with the given image options.
        /// </summary>
        /// <param name="size">New size of the IImageSurface</param>
        /// <param name="options">The image's resize and alignment options in the allocated space.</param>
        void Resize(Size size, ImageSurfaceOptions options);
    }
}
