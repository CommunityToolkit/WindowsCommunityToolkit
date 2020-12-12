// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Numerics;
using System.Threading.Tasks;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using Microsoft.Graphics.Canvas.Geometry;
using Microsoft.Toolkit.Uwp.UI.Media.Geometry;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Composition;
using Windows.UI.Xaml;

namespace Microsoft.Toolkit.Uwp.UI.Media.Surface
{
    /// <summary>
    /// Interface for the CompositionMaskGenerator
    /// </summary>
    public interface ICompositionGenerator : IDisposable
    {
        /// <summary>
        /// Device Replaced Event
        /// </summary>
        event EventHandler<object> DeviceReplaced;

        /// <summary>
        /// Gets the Compositor
        /// </summary>
        Compositor Compositor { get; }

        /// <summary>
        /// Gets the CanvasDevice
        /// </summary>
        CanvasDevice Device { get; }

        /// <summary>
        /// <para>Creates an Empty MaskSurface having the no size and geometry.</para>
        /// <para>NOTE: Use this API if you want to create an Empty IMaskSurface first
        /// and change its geometry and/or size of the MaskSurface later.</para>
        /// </summary>
        /// <returns>IMaskSurface</returns>
        IMaskSurface CreateMaskSurface();

        /// <summary>
        /// Creates a MaskSurface having the given size and geometry. The geometry is filled
        /// with white color. The surface not covered by the geometry is transparent.
        /// </summary>
        /// <param name="size">Size of the mask</param>
        /// <param name="geometry">Geometry of the mask</param>
        /// <returns>IMaskSurface</returns>
        IMaskSurface CreateMaskSurface(Size size, CanvasGeometry geometry);

        /// <summary>
        /// Creates a MaskSurface having the given size and geometry. The geometry is filled
        /// with white color. The surface not covered by the geometry is transparent.
        /// </summary>
        /// <param name="size">Size of the mask</param>
        /// <param name="geometry">Geometry of the mask</param>
        /// <param name="offset">The offset from the top left corner of the ICompositionSurface where
        /// the Geometry is rendered.</param>
        /// <returns>IMaskSurface</returns>
        IMaskSurface CreateMaskSurface(Size size, CanvasGeometry geometry, Vector2 offset);

        /// <summary>
        /// <para>Creates an Empty IGaussianMaskSurface having the no size and geometry.</para>
        /// <para>NOTE: Use this API if you want to create an Empty IGaussianMaskSurface first
        /// and change its geometry, size and/or blurRadius of the IGaussianMaskSurface later.</para>
        /// </summary>
        /// <returns>IMaskSurface</returns>
        IGaussianMaskSurface CreateGaussianMaskSurface();

        /// <summary>
        /// Creates an IGaussianMaskSurface having the given size and geometry. The geometry is filled
        /// with white color and a Gaussian blur is applied to it. The surface not covered by the geometry is transparent.
        /// </summary>
        /// <param name="size">Size of the mask</param>
        /// <param name="geometry">Geometry of the mask</param>
        /// <param name="offset">The offset from the top left corner of the ICompositionSurface where
        /// the Geometry is rendered.</param>
        /// <param name="blurRadius">Radius of Gaussian Blur to be applied on the IGaussianMaskSurface</param>
        /// <returns>IGaussianMaskSurface</returns>
        IGaussianMaskSurface CreateGaussianMaskSurface(Size size, CanvasGeometry geometry, Vector2 offset, float blurRadius);

        /// <summary>
        /// <para>Creates an Empty IGeometrySurface having the no size and geometry.</para>
        /// <para>NOTE: Use this API if you want to create an Empty IGeometrySurface
        /// first and change its geometry and/or size, fillColor or stroke later.</para>
        /// </summary>
        /// <returns>IGeometrySurface</returns>
        IGeometrySurface CreateGeometrySurface();

        /// <summary>
        /// Creates an IGeometrySurface having the given size, geometry, stroke
        /// </summary>
        /// <param name="size">Size of the IGeometrySurface</param>
        /// <param name="geometry">Geometry to be rendered on the IGeometrySurface</param>
        /// <param name="stroke">ICanvasStroke defining the outline for the geometry</param>
        /// <returns>IGeometrySurface</returns>
        IGeometrySurface CreateGeometrySurface(Size size, CanvasGeometry geometry, ICanvasStroke stroke);

        /// <summary>
        /// Creates an IGeometrySurface having the given size, geometry, fill color
        /// </summary>
        /// <param name="size">Size of the IGeometrySurface</param>
        /// <param name="geometry">Geometry to be rendered on the IGeometrySurface</param>
        /// <param name="fillColor">Fill color of the geometry.</param>
        /// <returns>IGeometrySurface</returns>
        IGeometrySurface CreateGeometrySurface(Size size, CanvasGeometry geometry, Color fillColor);

        /// <summary>
        /// Creates an IGeometrySurface having the given size, geometry, stroke and fill color
        /// </summary>
        /// <param name="size">Size of the IGeometrySurface</param>
        /// <param name="geometry">Geometry to be rendered on the IGeometrySurface</param>
        /// <param name="stroke">ICanvasStroke defining the outline for the geometry</param>
        /// <param name="fillColor">Fill color of the geometry.</param>
        /// <returns>IGeometrySurface</returns>
        IGeometrySurface CreateGeometrySurface(Size size, CanvasGeometry geometry, ICanvasStroke stroke, Color fillColor);

        /// <summary>
        /// Creates an IGeometrySurface having the given size, geometry, fill color and
        /// background color.
        /// </summary>
        /// <param name="size">Size of the IGeometrySurface</param>
        /// <param name="geometry">Geometry to be rendered on the IGeometrySurface</param>
        /// <param name="fillColor">Fill color of the geometry</param>
        /// <param name="backgroundColor">Fill color of the IGeometrySurface background which is
        /// not covered by the geometry</param>
        /// <returns>IGeometrySurface</returns>
        IGeometrySurface CreateGeometrySurface(Size size, CanvasGeometry geometry, Color fillColor, Color backgroundColor);

        /// <summary>
        /// Creates an IGeometrySurface having the given size, geometry, stroke, fill color and
        /// background color.
        /// </summary>
        /// <param name="size">Size of the IGeometrySurface</param>
        /// <param name="geometry">Geometry to be rendered on the IGeometrySurface</param>
        /// <param name="stroke">ICanvasStroke defining the outline for the geometry</param>
        /// <param name="fillColor">Fill color of the geometry</param>
        /// <param name="backgroundColor">Fill color of the IGeometrySurface background which is
        /// not covered by the geometry</param>
        /// <returns>IGeometrySurface</returns>
        IGeometrySurface CreateGeometrySurface(Size size, CanvasGeometry geometry, ICanvasStroke stroke, Color fillColor, Color backgroundColor);

        /// <summary>
        /// Creates an IGeometrySurface having the given size, geometry and fill brush.
        /// </summary>
        /// <param name="size">Size of the IGeometrySurface</param>
        /// <param name="geometry">Geometry to be rendered on the IGeometrySurface</param>
        /// <param name="fillBrush">The brush with which the geometry has to be filled</param>
        /// <returns>IGeometrySurface</returns>
        IGeometrySurface CreateGeometrySurface(Size size, CanvasGeometry geometry, ICanvasBrush fillBrush);

        /// <summary>
        /// Creates an IGeometrySurface having the given size, geometry, stroke and fill brush.
        /// </summary>
        /// <param name="size">Size of the IGeometrySurface</param>
        /// <param name="geometry">Geometry to be rendered on the IGeometrySurface</param>
        /// <param name="stroke">ICanvasStroke defining the outline for the geometry</param>
        /// <param name="fillBrush">The brush with which the geometry has to be filled</param>
        /// <returns>IGeometrySurface</returns>
        IGeometrySurface CreateGeometrySurface(Size size, CanvasGeometry geometry, ICanvasStroke stroke, ICanvasBrush fillBrush);

        /// <summary>
        /// Creates an IGeometrySurface having the given size, geometry, fill brush and
        /// background brush.
        /// </summary>
        /// <param name="size">Size of the IGeometrySurface</param>
        /// <param name="geometry">Geometry to be rendered on the IGeometrySurface</param>
        /// <param name="fillBrush">The brush with which the geometry has to be filled</param>
        /// <param name="backgroundBrush">The brush to fill the IGeometrySurface background which is
        /// not covered by the geometry</param>
        /// <returns>IGeometrySurface</returns>
        IGeometrySurface CreateGeometrySurface(Size size, CanvasGeometry geometry, ICanvasBrush fillBrush, ICanvasBrush backgroundBrush);

        /// <summary>
        /// Creates an IGeometrySurface having the given size, geometry, stroke, fill brush and
        /// background brush.
        /// </summary>
        /// <param name="size">Size of the IGeometrySurface</param>
        /// <param name="geometry">Geometry to be rendered on the IGeometrySurface</param>
        /// <param name="stroke">ICanvasStroke defining the outline for the geometry</param>
        /// <param name="fillBrush">The brush with which the geometry has to be filled</param>
        /// <param name="backgroundBrush">The brush to fill the IGeometrySurface background which is
        /// not covered by the geometry</param>
        /// <returns>IGeometrySurface</returns>
        IGeometrySurface CreateGeometrySurface(Size size, CanvasGeometry geometry, ICanvasStroke stroke, ICanvasBrush fillBrush, ICanvasBrush backgroundBrush);

        /// <summary>
        /// Creates an IGeometrySurface having the given size, geometry, fill brush and
        /// background color.
        /// </summary>
        /// <param name="size">Size of the IGeometrySurface</param>
        /// <param name="geometry">Geometry to be rendered on the IGeometrySurface</param>
        /// <param name="fillBrush">The brush with which the geometry has to be filled</param>
        /// <param name="backgroundColor">Fill color of the IGeometrySurface background which is
        /// not covered by the geometry</param>
        /// <returns>IGeometrySurface</returns>
        IGeometrySurface CreateGeometrySurface(Size size, CanvasGeometry geometry, ICanvasBrush fillBrush, Color backgroundColor);

        /// <summary>
        /// Creates an IGeometrySurface having the given size, geometry, stroke, fill brush and
        /// background color.
        /// </summary>
        /// <param name="size">Size of the IGeometrySurface</param>
        /// <param name="geometry">Geometry to be rendered on the IGeometrySurface</param>
        /// <param name="stroke">ICanvasStroke defining the outline for the geometry</param>
        /// <param name="fillBrush">The brush with which the geometry has to be filled</param>
        /// <param name="backgroundColor">Fill color of the IGeometrySurface background which is
        /// not covered by the geometry</param>
        /// <returns>IGeometrySurface</returns>
        IGeometrySurface CreateGeometrySurface(Size size, CanvasGeometry geometry, ICanvasStroke stroke, ICanvasBrush fillBrush, Color backgroundColor);

        /// <summary>
        /// Creates an IGeometrySurface having the given size, geometry, fill color and
        /// background brush.
        /// </summary>
        /// <param name="size">Size of the IGeometrySurface</param>
        /// <param name="geometry">Geometry to be rendered on the IGeometrySurface</param>
        /// <param name="fillColor">Fill color of the geometry</param>
        /// <param name="backgroundBrush">The brush to fill the IGeometrySurface background which is
        /// not covered by the geometry</param>
        /// <returns>IGeometrySurface</returns>
        IGeometrySurface CreateGeometrySurface(Size size, CanvasGeometry geometry, Color fillColor, ICanvasBrush backgroundBrush);

        /// <summary>
        /// Creates an IGeometrySurface having the given size, geometry, stroke, fill color and
        /// background brush.
        /// </summary>
        /// <param name="size">Size of the IGeometrySurface</param>
        /// <param name="geometry">Geometry to be rendered on the IGeometrySurface</param>
        /// <param name="stroke">ICanvasStroke defining the outline for the geometry</param>
        /// <param name="fillColor">Fill color of the geometry</param>
        /// <param name="backgroundBrush">The brush to fill the IGeometrySurface background which is
        /// not covered by the geometry</param>
        /// <returns>IGeometrySurface</returns>
        IGeometrySurface CreateGeometrySurface(Size size, CanvasGeometry geometry, ICanvasStroke stroke, Color fillColor, ICanvasBrush backgroundBrush);

        /// <summary>
        /// Creates an IImageSurface having the given size onto which an image (based on the Uri
        /// and the options) is loaded.
        /// </summary>
        /// <param name="uri">Uri of the image to be loaded onto the IImageSurface.</param>
        /// <param name="size">New size of the IImageSurface</param>
        /// <param name="options">The image's resize and alignment options in the allocated space.</param>
        /// <returns>Task&lt;IImageSurface&gt;</returns>
        Task<IImageSurface> CreateImageSurfaceAsync(Uri uri, Size size, ImageSurfaceOptions options);

        /// <summary>
        /// Creates an IImageSurface having the given size onto which the given image is loaded.
        /// </summary>
        /// <param name="bitmap">Image that will be loaded onto the IImageSurface.</param>
        /// <param name="size">Size of the IImageSurface</param>
        /// <param name="options">The image's resize and alignment options in the allocated space.</param>
        /// <returns>IImageSurface</returns>
        IImageSurface CreateImageSurface(CanvasBitmap bitmap, Size size, ImageSurfaceOptions options);

        /// <summary>
        /// Creates a copy of the given IImageSurface
        /// </summary>
        /// <param name="imageSurface">IImageSurface to copy</param>
        /// <returns>IImageSurface</returns>
        IImageSurface CreateImageSurface(IImageSurface imageSurface);

        /// <summary>
        /// Creates a copy of the given IImageMaskSurface
        /// </summary>
        /// <param name="imageMaskSurface">IImageMaskSurface to copy</param>
        /// <returns>IImageMaskSurface</returns>
        IImageMaskSurface CreateImageMaskSurface(IImageMaskSurface imageMaskSurface);

        /// <summary>
        /// Creates an IImageMaskSurface having the given size onto which an image (based on the Uri
        /// and the options) is loaded.
        /// </summary>
        /// <param name="surfaceBitmap">The CanvasBitmap whose alpha values will be used to create the Mask.</param>
        /// <param name="size">Size of the IImageSurface</param>
        /// <param name="padding">The padding between the IImageMaskSurface outer bounds and the bounds of the area where
        /// the mask, created from the given image's alpha values, should be rendered.</param>
        /// <param name="blurRadius">Radius of the Gaussian blur applied to the the mask.</param>
        /// <returns>IImageMaskSurface</returns>
        IImageMaskSurface CreateImageMaskSurface(CanvasBitmap surfaceBitmap, Size size, Thickness padding, float blurRadius);

        /// <summary>
        /// Creates an IImageMaskSurface having the given size onto which an image (based on the Uri
        /// and the options) is loaded.
        /// </summary>
        /// <param name="surfaceBitmap">The CanvasBitmap whose alpha values will be used to create the Mask.</param>
        /// <param name="size">Size of the IImageSurface</param>
        /// <param name="padding">The padding between the IImageMaskSurface outer bounds and the bounds of the area where
        /// the mask, created from the given image's alpha values, should be rendered.</param>
        /// <param name="options">The image's resize, alignment options and blur radius in the allocated space.</param>
        /// <returns>IImageMaskSurface</returns>
        IImageMaskSurface CreateImageMaskSurface(CanvasBitmap surfaceBitmap, Size size, Thickness padding, ImageSurfaceOptions options);

        /// <summary>
        /// Creates an IImageMaskSurface having the given size onto which an image (based on the Uri
        /// and the options) is loaded.
        /// </summary>
        /// <param name="imageSurface">The IImageSurface whose image's alpha values will be used to create the Mask.</param>
        /// <param name="size">Size of the IImageMaskSurface</param>
        /// <param name="padding">The padding between the IImageMaskSurface outer bounds and the bounds of the area where
        /// the mask, created from the given image's alpha values, should be rendered.</param>
        /// <param name="options">The image's resize, alignment options and blur radius in the allocated space.</param>
        /// <returns>IImageMaskSurface</returns>
        IImageMaskSurface CreateImageMaskSurface(IImageSurface imageSurface, Size size, Thickness padding, ImageSurfaceOptions options);

        /// <summary>
        /// Creates a ImageSurface having the given size onto which an image (based on the Uri
        /// and the options) is loaded.
        /// </summary>
        /// <param name="uri">Uri of the image whose alpha values will be used to create the Mask.</param>
        /// <param name="size">Size of the IImageMaskSurface</param>
        /// <param name="padding">The padding between the IImageMaskSurface outer bounds and the bounds of the area where
        /// the mask, created from the given image's alpha values, should be rendered.</param>
        /// <param name="options">The image's resize, alignment options and blur radius in the allocated space.</param>
        /// <returns>Task&lt;IImageMaskSurface&gt;</returns>
        Task<IImageMaskSurface> CreateImageMaskSurfaceAsync(Uri uri, Size size, Thickness padding, ImageSurfaceOptions options);

        /// <summary>
        /// Creates a reflection of the given Visual
        /// </summary>
        /// <param name="visual">Visual whose reflection has to be created</param>
        /// <param name="reflectionDistance">Distance of the reflection from the visual</param>
        /// <param name="reflectionLength">Normalized Length of the reflected visual that will be visible.</param>
        /// <param name="location"> <see cref="ReflectionLocation"/> - Location of the reflection with respect
        /// to the Visual - Bottom, Top, Left or Right</param>
        void CreateReflection(ContainerVisual visual, float reflectionDistance = 0f, float reflectionLength = 0.7f, ReflectionLocation location = ReflectionLocation.Bottom);
    }
}
