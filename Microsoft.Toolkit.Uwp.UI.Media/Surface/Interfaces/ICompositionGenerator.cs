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

namespace Microsoft.Toolkit.Uwp.UI.Media
{
    /// <summary>
    /// Interface for the <see cref="CompositionGenerator"/>
    /// </summary>
    public interface ICompositionGenerator : IDisposable
    {
        /// <summary>
        /// Device Replaced Event
        /// </summary>
        event EventHandler<object> DeviceReplaced;

        /// <summary>
        /// Gets the <see cref="Windows.UI.Composition.Compositor"/>.
        /// </summary>
        Compositor Compositor { get; }

        /// <summary>
        /// Gets the <see cref="CanvasDevice"/>.
        /// </summary>
        CanvasDevice Device { get; }

        /// <summary>
        /// <para>Creates an Empty <see cref="IGeometryMaskSurface"/> having the no size and geometry.</para>
        /// <para>NOTE: Use this API if you want to create an Empty <see cref="IGeometryMaskSurface"/> first
        /// and change its geometry and/or size of the <see cref="IGeometryMaskSurface"/> later.</para>
        /// </summary>
        /// <returns><see cref="IGeometryMaskSurface"/></returns>
        IGeometryMaskSurface CreateGeometryMaskSurface();

        /// <summary>
        /// Creates a <see cref="IGeometryMaskSurface"/> having the given size and geometry. The geometry is filled
        /// with white color. The surface not covered by the geometry is transparent.
        /// </summary>
        /// <param name="size">Size of the mask.</param>
        /// <param name="geometry">Geometry of the mask.</param>
        /// <returns><see cref="IGeometryMaskSurface"/></returns>
        IGeometryMaskSurface CreateGeometryMaskSurface(Size size, CanvasGeometry geometry);

        /// <summary>
        /// Creates a <see cref="IGeometryMaskSurface"/> having the given size and geometry. The geometry is filled
        /// with white color. The surface not covered by the geometry is transparent.
        /// </summary>
        /// <param name="size">Size of the mask.</param>
        /// <param name="geometry">Geometry of the mask.</param>
        /// <param name="offset">The offset from the top left corner of the <see cref="ICompositionSurface"/> where the <see cref="IGeometryMaskSurface.Geometry"/> is rendered.</param>
        /// <returns><see cref="IGeometryMaskSurface"/></returns>
        IGeometryMaskSurface CreateGeometryMaskSurface(Size size, CanvasGeometry geometry, Vector2 offset);

        /// <summary>
        /// <para>Creates an Empty <see cref="IGaussianMaskSurface"/> having the no size and geometry.</para>
        /// <para>NOTE: Use this API if you want to create an Empty <see cref="IGaussianMaskSurface"/> first and change its geometry, size and/or blurRadius of the <see cref="IGaussianMaskSurface"/> later.</para>
        /// </summary>
        /// <returns><see cref="IGaussianMaskSurface"/></returns>
        IGaussianMaskSurface CreateGaussianMaskSurface();

        /// <summary>
        /// Creates an <see cref="IGaussianMaskSurface"/> having the given size and geometry. The geometry is filled
        /// with white color and a Gaussian blur is applied to it. The surface not covered by the geometry is transparent.
        /// </summary>
        /// <param name="size">Size of the mask.</param>
        /// <param name="geometry">Geometry of the mask.</param>
        /// <param name="offset">The offset from the top left corner of the <see cref="ICompositionSurface"/> where the <see cref="IGeometryMaskSurface.Geometry"/> is rendered.</param>
        /// <param name="blurRadius">Radius of Gaussian Blur to be applied on the <see cref="IGaussianMaskSurface"/>.</param>
        /// <returns><see cref="IGaussianMaskSurface"/></returns>
        IGaussianMaskSurface CreateGaussianMaskSurface(Size size, CanvasGeometry geometry, Vector2 offset, float blurRadius);

        /// <summary>
        /// <para>Creates an Empty <see cref="IGeometrySurface"/> having the no size and geometry.</para>
        /// <para>NOTE: Use this API if you want to create an Empty <see cref="IGeometrySurface"/> first and change its geometry and/or size, fillColor or stroke later.</para>
        /// </summary>
        /// <returns><see cref="IGeometrySurface"/></returns>
        IGeometrySurface CreateGeometrySurface();

        /// <summary>
        /// Creates an <see cref="IGeometrySurface"/> having the given size, geometry, stroke.
        /// </summary>
        /// <param name="size">Size of the <see cref="IGeometrySurface"/>.</param>
        /// <param name="geometry">Geometry to be rendered on the <see cref="IGeometrySurface"/>.</param>
        /// <param name="stroke"><see cref="ICanvasStroke"/> defining the outline for the geometry.</param>
        /// <returns><see cref="IGeometrySurface"/></returns>
        IGeometrySurface CreateGeometrySurface(Size size, CanvasGeometry geometry, ICanvasStroke stroke);

        /// <summary>
        /// Creates an <see cref="IGeometrySurface"/> having the given size, geometry, fill color.
        /// </summary>
        /// <param name="size">Size of the <see cref="IGeometrySurface"/>.</param>
        /// <param name="geometry">Geometry to be rendered on the <see cref="IGeometrySurface"/>.</param>
        /// <param name="fillColor">Fill color of the geometry.</param>
        /// <returns><see cref="IGeometrySurface"/></returns>
        IGeometrySurface CreateGeometrySurface(Size size, CanvasGeometry geometry, Color fillColor);

        /// <summary>
        /// Creates an <see cref="IGeometrySurface"/> having the given size, geometry, stroke and fill color.
        /// </summary>
        /// <param name="size">Size of the <see cref="IGeometrySurface"/>.</param>
        /// <param name="geometry">Geometry to be rendered on the <see cref="IGeometrySurface"/>.</param>
        /// <param name="stroke"><see cref="ICanvasStroke"/> defining the outline for the geometry.</param>
        /// <param name="fillColor">Fill color of the geometry.</param>
        /// <returns><see cref="IGeometrySurface"/></returns>
        IGeometrySurface CreateGeometrySurface(Size size, CanvasGeometry geometry, ICanvasStroke stroke, Color fillColor);

        /// <summary>
        /// Creates an <see cref="IGeometrySurface"/> having the given size, geometry, fill color and background color.
        /// </summary>
        /// <param name="size">Size of the <see cref="IGeometrySurface"/>.</param>
        /// <param name="geometry">Geometry to be rendered on the <see cref="IGeometrySurface"/>.</param>
        /// <param name="fillColor">Fill color of the geometry.</param>
        /// <param name="backgroundColor">Fill color of the <see cref="IGeometrySurface"/> background which is
        /// not covered by the geometry.</param>
        /// <returns><see cref="IGeometrySurface"/></returns>
        IGeometrySurface CreateGeometrySurface(Size size, CanvasGeometry geometry, Color fillColor, Color backgroundColor);

        /// <summary>
        /// Creates an <see cref="IGeometrySurface"/> having the given size, geometry, stroke, fill color and background color.
        /// </summary>
        /// <param name="size">Size of the <see cref="IGeometrySurface"/>.</param>
        /// <param name="geometry">Geometry to be rendered on the <see cref="IGeometrySurface"/>.</param>
        /// <param name="stroke"><see cref="ICanvasStroke"/> defining the outline for the geometry.</param>
        /// <param name="fillColor">Fill color of the geometry.</param>
        /// <param name="backgroundColor">Fill color of the <see cref="IGeometrySurface"/> background which is not covered by the geometry.</param>
        /// <returns><see cref="IGeometrySurface"/></returns>
        IGeometrySurface CreateGeometrySurface(Size size, CanvasGeometry geometry, ICanvasStroke stroke, Color fillColor, Color backgroundColor);

        /// <summary>
        /// Creates an <see cref="IGeometrySurface"/> having the given size, geometry and fill brush.
        /// </summary>
        /// <param name="size">Size of the <see cref="IGeometrySurface"/>.</param>
        /// <param name="geometry">Geometry to be rendered on the <see cref="IGeometrySurface"/>.</param>
        /// <param name="fillBrush">The brush with which the geometry has to be filled.</param>
        /// <returns><see cref="IGeometrySurface"/></returns>
        IGeometrySurface CreateGeometrySurface(Size size, CanvasGeometry geometry, ICanvasBrush fillBrush);

        /// <summary>
        /// Creates an <see cref="IGeometrySurface"/> having the given size, geometry, stroke and fill brush.
        /// </summary>
        /// <param name="size">Size of the <see cref="IGeometrySurface"/>.</param>
        /// <param name="geometry">Geometry to be rendered on the <see cref="IGeometrySurface"/>.</param>
        /// <param name="stroke"><see cref="ICanvasStroke"/> defining the outline for the geometry.</param>
        /// <param name="fillBrush">The brush with which the geometry has to be filled.</param>
        /// <returns><see cref="IGeometrySurface"/></returns>
        IGeometrySurface CreateGeometrySurface(Size size, CanvasGeometry geometry, ICanvasStroke stroke, ICanvasBrush fillBrush);

        /// <summary>
        /// Creates an <see cref="IGeometrySurface"/> having the given size, geometry, fill brush and background brush.
        /// </summary>
        /// <param name="size">Size of the <see cref="IGeometrySurface"/>.</param>
        /// <param name="geometry">Geometry to be rendered on the <see cref="IGeometrySurface"/>.</param>
        /// <param name="fillBrush">The brush with which the geometry has to be filled.</param>
        /// <param name="backgroundBrush">The brush to fill the <see cref="IGeometrySurface"/> background which is not covered by the geometry.</param>
        /// <returns><see cref="IGeometrySurface"/></returns>
        IGeometrySurface CreateGeometrySurface(Size size, CanvasGeometry geometry, ICanvasBrush fillBrush, ICanvasBrush backgroundBrush);

        /// <summary>
        /// Creates an <see cref="IGeometrySurface"/> having the given size, geometry, stroke, fill brush and background brush.
        /// </summary>
        /// <param name="size">Size of the <see cref="IGeometrySurface"/>.</param>
        /// <param name="geometry">Geometry to be rendered on the <see cref="IGeometrySurface"/>.</param>
        /// <param name="stroke"><see cref="ICanvasStroke"/> defining the outline for the geometry.</param>
        /// <param name="fillBrush">The brush with which the geometry has to be filled.</param>
        /// <param name="backgroundBrush">The brush to fill the <see cref="IGeometrySurface"/> background which is not covered by the geometry.</param>
        /// <returns><see cref="IGeometrySurface"/></returns>
        IGeometrySurface CreateGeometrySurface(Size size, CanvasGeometry geometry, ICanvasStroke stroke, ICanvasBrush fillBrush, ICanvasBrush backgroundBrush);

        /// <summary>
        /// Creates an <see cref="IGeometrySurface"/> having the given size, geometry, fill brush and background color.
        /// </summary>
        /// <param name="size">Size of the <see cref="IGeometrySurface"/>.</param>
        /// <param name="geometry">Geometry to be rendered on the <see cref="IGeometrySurface"/>.</param>
        /// <param name="fillBrush">The brush with which the geometry has to be filled.</param>
        /// <param name="backgroundColor">Fill color of the <see cref="IGeometrySurface"/> background which is not covered by the geometry.</param>
        /// <returns><see cref="IGeometrySurface"/></returns>
        IGeometrySurface CreateGeometrySurface(Size size, CanvasGeometry geometry, ICanvasBrush fillBrush, Color backgroundColor);

        /// <summary>
        /// Creates an <see cref="IGeometrySurface"/> having the given size, geometry, stroke, fill brush and
        /// background color.
        /// </summary>
        /// <param name="size">Size of the <see cref="IGeometrySurface"/>.</param>
        /// <param name="geometry">Geometry to be rendered on the <see cref="IGeometrySurface"/>.</param>
        /// <param name="stroke"><see cref="ICanvasStroke"/> defining the outline for the geometry.</param>
        /// <param name="fillBrush">The brush with which the geometry has to be filled.</param>
        /// <param name="backgroundColor">Fill color of the <see cref="IGeometrySurface"/> background which is not covered by the geometry.</param>
        /// <returns><see cref="IGeometrySurface"/></returns>
        IGeometrySurface CreateGeometrySurface(Size size, CanvasGeometry geometry, ICanvasStroke stroke, ICanvasBrush fillBrush, Color backgroundColor);

        /// <summary>
        /// Creates an <see cref="IGeometrySurface"/> having the given size, geometry, fill color and background brush.
        /// </summary>
        /// <param name="size">Size of the <see cref="IGeometrySurface"/>.</param>
        /// <param name="geometry">Geometry to be rendered on the <see cref="IGeometrySurface"/>.</param>
        /// <param name="fillColor">Fill color of the geometry.</param>
        /// <param name="backgroundBrush">The brush to fill the <see cref="IGeometrySurface"/> background which is not covered by the geometry.</param>
        /// <returns><see cref="IGeometrySurface"/></returns>
        IGeometrySurface CreateGeometrySurface(Size size, CanvasGeometry geometry, Color fillColor, ICanvasBrush backgroundBrush);

        /// <summary>
        /// Creates an <see cref="IGeometrySurface"/> having the given size, geometry, stroke, fill color and background brush.
        /// </summary>
        /// <param name="size">Size of the <see cref="IGeometrySurface"/>.</param>
        /// <param name="geometry">Geometry to be rendered on the <see cref="IGeometrySurface"/>.</param>
        /// <param name="stroke"><see cref="ICanvasStroke"/> defining the outline for the geometry.</param>
        /// <param name="fillColor">Fill color of the geometry.</param>
        /// <param name="backgroundBrush">The brush to fill the <see cref="IGeometrySurface"/> background which is not covered by the geometry.</param>
        /// <returns><see cref="IGeometrySurface"/></returns>
        IGeometrySurface CreateGeometrySurface(Size size, CanvasGeometry geometry, ICanvasStroke stroke, Color fillColor, ICanvasBrush backgroundBrush);

        /// <summary>
        /// Creates an <see cref="IImageSurface"/> having the given size onto which an image (based on the <see cref="System.Uri"/> and the options) is loaded.
        /// </summary>
        /// <param name="uri">Uri of the image to be loaded onto the <see cref="IImageSurface"/>.</param>
        /// <param name="size">New size of the <see cref="IImageSurface"/>.</param>
        /// <param name="options">The image's resize and alignment options in the allocated space.</param>
        /// <returns><see cref="Task"/>&lt;<see cref="IImageSurface"/>&gt;</returns>
        Task<IImageSurface> CreateImageSurfaceAsync(Uri uri, Size size, ImageSurfaceOptions options);

        /// <summary>
        /// Creates an <see cref="IImageSurface"/> having the given size onto which the given image is loaded.
        /// </summary>
        /// <param name="bitmap">Image that will be loaded onto the <see cref="IImageSurface"/>.</param>
        /// <param name="size">Size of the <see cref="IImageSurface"/>.</param>
        /// <param name="options">The image's resize and alignment options in the allocated space.</param>
        /// <returns><see cref="IImageSurface"/></returns>
        IImageSurface CreateImageSurface(CanvasBitmap bitmap, Size size, ImageSurfaceOptions options);

        /// <summary>
        /// Creates a copy of the given <see cref="IImageSurface"/>
        /// </summary>
        /// <param name="imageSurface"><see cref="IImageSurface"/> to copy.</param>
        /// <returns><see cref="IImageSurface"/></returns>
        IImageSurface CreateImageSurface(IImageSurface imageSurface);

        /// <summary>
        /// Creates a copy of the given <see cref="IImageMaskSurface"/>
        /// </summary>
        /// <param name="imageMaskSurface"><see cref="IImageMaskSurface"/> to copy.</param>
        /// <returns><see cref="IImageMaskSurface"/></returns>
        IImageMaskSurface CreateImageMaskSurface(IImageMaskSurface imageMaskSurface);

        /// <summary>
        /// Creates an <see cref="IImageMaskSurface"/> having the given size onto which an image (based on the Uri
        /// and the options) is loaded.
        /// </summary>
        /// <param name="surfaceBitmap">The CanvasBitmap whose alpha values will be used to create the Mask.</param>
        /// <param name="size">Size of the <see cref="IImageSurface"/>.</param>
        /// <param name="padding">The padding between the <see cref="IImageMaskSurface"/> outer bounds and the bounds of the area where the mask, created from the given image's alpha values, should be rendered.</param>
        /// <param name="blurRadius">Radius of the Gaussian blur applied to the the mask.</param>
        /// <returns><see cref="IImageMaskSurface"/></returns>
        IImageMaskSurface CreateImageMaskSurface(CanvasBitmap surfaceBitmap, Size size, Thickness padding, float blurRadius);

        /// <summary>
        /// Creates an <see cref="IImageMaskSurface"/> having the given size onto which an image (based on the Uri
        /// and the options) is loaded.
        /// </summary>
        /// <param name="surfaceBitmap">The CanvasBitmap whose alpha values will be used to create the Mask.</param>
        /// <param name="size">Size of the <see cref="IImageSurface"/>.</param>
        /// <param name="padding">The padding between the <see cref="IImageMaskSurface"/> outer bounds and the bounds of the area where the mask, created from the given image's alpha values, should be rendered.</param>
        /// <param name="options">The image's resize, alignment options and blur radius in the allocated space.</param>
        /// <returns><see cref="IImageMaskSurface"/></returns>
        IImageMaskSurface CreateImageMaskSurface(CanvasBitmap surfaceBitmap, Size size, Thickness padding, ImageSurfaceOptions options);

        /// <summary>
        /// Creates an <see cref="IImageMaskSurface"/> having the given size onto which an image (based on the Uri
        /// and the options) is loaded.
        /// </summary>
        /// <param name="imageSurface">The <see cref="IImageSurface"/> whose image's alpha values will be used to create the Mask.</param>
        /// <param name="size">Size of the <see cref="IImageMaskSurface"/>.</param>
        /// <param name="padding">The padding between the <see cref="IImageMaskSurface"/> outer bounds and the bounds of the area where the mask, created from the given image's alpha values, should be rendered.</param>
        /// <param name="options">The image's resize, alignment options and blur radius in the allocated space.</param>
        /// <returns><see cref="IImageMaskSurface"/></returns>
        IImageMaskSurface CreateImageMaskSurface(IImageSurface imageSurface, Size size, Thickness padding, ImageSurfaceOptions options);

        /// <summary>
        /// Creates a ImageSurface having the given size onto which an image (based on the Uri
        /// and the options) is loaded.
        /// </summary>
        /// <param name="uri">Uri of the image whose alpha values will be used to create the Mask.</param>
        /// <param name="size">Size of the <see cref="IImageMaskSurface"/>.</param>
        /// <param name="padding">The padding between the <see cref="IImageMaskSurface"/> outer bounds and the bounds of the area where the mask, created from the given image's alpha values, should be rendered.</param>
        /// <param name="options">The image's resize, alignment options and blur radius in the allocated space.</param>
        /// <returns><see cref="Task"/>&lt;<see cref="IImageMaskSurface"/>&gt;</returns>
        Task<IImageMaskSurface> CreateImageMaskSurfaceAsync(Uri uri, Size size, Thickness padding, ImageSurfaceOptions options);

        /// <summary>
        /// Creates a reflection of the given <see cref="Visual"/>.
        /// </summary>
        /// <param name="visual"><see cref="Visual"/> whose reflection has to be created.</param>
        /// <param name="reflectionDistance">Distance of the reflection from the visual.</param>
        /// <param name="reflectionLength">Normalized Length of the reflected visual that will be visible.</param>
        /// <param name="location"> <see cref="ReflectionLocation"/> - Location of the reflection with respect to the Visual - Bottom, Top, Left or Right.</param>
        void CreateReflection(ContainerVisual visual, float reflectionDistance = 0f, float reflectionLength = 0.7f, ReflectionLocation location = ReflectionLocation.Bottom);
    }
}
