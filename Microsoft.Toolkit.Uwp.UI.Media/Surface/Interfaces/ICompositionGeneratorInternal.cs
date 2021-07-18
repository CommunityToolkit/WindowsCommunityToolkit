// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Numerics;
using System.Threading.Tasks;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using Microsoft.Graphics.Canvas.Geometry;
using Windows.Foundation;
using Windows.UI.Composition;
using Windows.UI.Xaml;

namespace Microsoft.Toolkit.Uwp.UI.Media
{
    /// <summary>
    /// Internal interface for the ComposiitonGenerator
    /// </summary>
    internal interface ICompositionGeneratorInternal : ICompositionGenerator
    {
        /// <summary>
        /// Creates a <see cref="CompositionDrawingSurface"/> for the given size.
        /// </summary>
        /// <param name="surfaceLock">The object to lock to prevent multiple threads from accessing the surface at the same time.</param>
        /// <param name="size">Size of the <see cref="CompositionDrawingSurface"/>.</param>
        /// <returns><see cref="CompositionDrawingSurface"/></returns>
        CompositionDrawingSurface CreateDrawingSurface(object surfaceLock, Size size);

        /// <summary>
        /// Resizes the <see cref="CompositionDrawingSurface"/> to the given size.
        /// </summary>
        /// <param name="surfaceLock">The object to lock to prevent multiple threads from accessing the surface at the same time.</param>
        /// <param name="surface"><see cref="CompositionDrawingSurface"/>.</param>
        /// <param name="size">New size of the <see cref="CompositionDrawingSurface"/>.</param>
        void ResizeDrawingSurface(object surfaceLock, CompositionDrawingSurface surface, Size size);

        /// <summary>
        /// Redraws the <see cref="IGeometryMaskSurface"/> with the given size and geometry.
        /// </summary>
        /// <param name="surfaceLock">The object to lock to prevent multiple threads from accessing the surface at the same time.</param>
        /// <param name="surface"><see cref="CompositionDrawingSurface"/>.</param>
        /// <param name="size">Size of the <see cref="IGeometryMaskSurface"/>.</param>
        /// <param name="geometry">Geometry of the <see cref="IGeometryMaskSurface"/>.</param>
        /// <param name="offset">The offset from the top left corner of the <see cref="ICompositionSurface"/> where the <see cref="IGeometryMaskSurface.Geometry"/> is rendered.</param>
        void RedrawGeometryMaskSurface(object surfaceLock, CompositionDrawingSurface surface, Size size, CanvasGeometry geometry, Vector2 offset);

        /// <summary>
        /// Redraws the <see cref="IGaussianMaskSurface"/> with the given size and geometry.
        /// </summary>
        /// <param name="surfaceLock">The object to lock to prevent multiple threads from accessing the surface at the same time.</param>
        /// <param name="surface"><see cref="CompositionDrawingSurface"/>.</param>
        /// <param name="size">Size of the <see cref="IGeometryMaskSurface"/>.</param>
        /// <param name="geometry">Geometry of the <see cref="IGeometryMaskSurface"/>.</param>
        /// <param name="offset">The offset from the top left corner of the ICompositionSurface where the <see cref="IGeometryMaskSurface.Geometry"/> is rendered.</param>
        /// <param name="blurRadius">Radius of Gaussian Blur to be applied on the <see cref="IGaussianMaskSurface"/>.</param>
        void RedrawGaussianMaskSurface(object surfaceLock, CompositionDrawingSurface surface, Size size, CanvasGeometry geometry, Vector2 offset, float blurRadius);

        /// <summary>
        /// Redraws the <see cref="IGeometrySurface"/> with the given size, geometry, foreground brush and background brush.
        /// </summary>
        /// <param name="surfaceLock">The object to lock to prevent multiple threads from accessing the surface at the same time.</param>
        /// <param name="surface"><see cref="CompositionDrawingSurface"/>.</param>
        /// <param name="size">Size of the <see cref="IGeometrySurface"/>.</param>
        /// <param name="geometry">Geometry of the <see cref="IGeometrySurface"/>.</param>
        /// <param name="stroke"><see cref="ICanvasStroke"/> defining the outline for the geometry.</param>
        /// <param name="fillBrush">The brush with which the geometry has to be filled.</param>
        /// <param name="backgroundBrush">The brush with which the <see cref="IGeometrySurface"/> background has to be filled.</param>
        void RedrawGeometrySurface(object surfaceLock, CompositionDrawingSurface surface, Size size, CanvasGeometry geometry, ICanvasStroke stroke, ICanvasBrush fillBrush, ICanvasBrush backgroundBrush);

        /// <summary>
        /// Resizes the ImageSurface to the given size and redraws the <see cref="IImageSurface"/> by rendering the canvasBitmap onto the surface.
        /// </summary>
        /// <param name="surfaceLock">The object to lock to prevent multiple threads from accessing the surface at the same time.</param>
        /// <param name="surface"><see cref="CompositionDrawingSurface"/>.</param>
        /// <param name="options">Describes the image's resize and alignment options in the allocated space.</param>
        /// <param name="canvasBitmap">The <see cref="CanvasBitmap"/> on which the image is loaded.</param>
        void RedrawImageSurface(object surfaceLock, CompositionDrawingSurface surface, ImageSurfaceOptions options, CanvasBitmap canvasBitmap);

        /// <summary>
        /// Resizes the <see cref="IImageSurface"/> with the given size and redraws the <see cref="IImageSurface"/> by loading image from the new Uri.
        /// </summary>
        /// <param name="surfaceLock">The object to lock to prevent multiple threads from accessing the surface at the same time.</param>
        /// <param name="surface"><see cref="CompositionDrawingSurface"/>.</param>
        /// <param name="uri">Uri of the image to be loaded onto the <see cref="IImageSurface"/>.</param>
        /// <param name="options">Describes the image's resize and alignment options in the allocated space.</param>
        /// <param name="canvasBitmap">The <see cref="CanvasBitmap"/> on which the image is loaded.</param>
        /// <returns><see cref="CanvasBitmap"/></returns>
        Task<CanvasBitmap> RedrawImageSurfaceAsync(object surfaceLock, CompositionDrawingSurface surface, Uri uri, ImageSurfaceOptions options, CanvasBitmap canvasBitmap);

        /// <summary>
        /// Resizes the <see cref="IImageMaskSurface"/> to the given size and redraws the IImageMaskSurface by rendering the mask using the image's alpha values onto the surface.
        /// </summary>
        /// <param name="surfaceLock">The object to lock to prevent multiple threads from accessing the surface at the same time.</param>
        /// <param name="surface"><see cref="CompositionDrawingSurface"/>.</param>
        /// <param name="padding">The padding between the <see cref="IImageMaskSurface"/> outer bounds and the bounds of the area where the mask, created from the loaded image's alpha values, should be rendered.</param>
        /// <param name="options">Describes the image's resize and alignment options and blur radius in the allocated space.</param>
        /// <param name="surfaceBitmap">The image whose alpha values is used to create the <see cref="IImageMaskSurface"/>.</param>
        void RedrawImageMaskSurface(object surfaceLock, CompositionDrawingSurface surface, Thickness padding, ImageSurfaceOptions options, CanvasBitmap surfaceBitmap);

        /// <summary>
        /// Resizes the <see cref="IImageMaskSurface"/> to the given size and redraws the <see cref="IImageMaskSurface"/> by loading the image from the new <see cref="System.Uri"/> and rendering the mask using the image's alpha values onto the surface.
        /// </summary>
        /// <param name="surfaceLock">The object to lock to prevent multiple threads from accessing the surface at the same time.</param>
        /// <param name="surface"><see cref="CompositionDrawingSurface"/>.</param>
        /// <param name="uri">Uri of the image to be loaded onto the <see cref="IImageMaskSurface"/>.</param>
        /// <param name="padding">The padding between the <see cref="IImageMaskSurface"/> outer bounds and the bounds of the area where the mask, created from the loaded image's alpha values, should be rendered.</param>
        /// <param name="options">Describes the image's resize and alignment options and blur radius in the allocated space.</param>
        /// <param name="surfaceBitmap">The <see cref="CanvasBitmap"/> on which the image is loaded.</param>
        /// <returns>The <see cref="CanvasBitmap"/> whose alpha values is used to create the <see cref="IImageMaskSurface"/>.</returns>
        Task<CanvasBitmap> RedrawImageMaskSurfaceAsync(object surfaceLock, CompositionDrawingSurface surface, Uri uri, Thickness padding, ImageSurfaceOptions options, CanvasBitmap surfaceBitmap);
    }
}
