// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using Microsoft.Graphics.Canvas.Effects;
using Microsoft.Graphics.Canvas.Geometry;
using Microsoft.Graphics.Canvas.UI.Composition;
using Microsoft.Toolkit.Uwp.UI.Media.Geometry;
using Windows.Foundation;
using Windows.Graphics.DirectX;
using Windows.UI;
using Windows.UI.Composition;
using Windows.UI.Xaml;

namespace Microsoft.Toolkit.Uwp.UI.Media.Surface
{
    /// <summary>
    /// Class to create mask which can be used to create custom shaped
    /// Composition Visuals.
    /// </summary>
    internal sealed class CompositionGenerator : ICompositionGeneratorInternal
    {
        /// <summary>
        /// Device Replaced event
        /// </summary>
        public event EventHandler<object> DeviceReplaced;

        private readonly object _disposingLock;
        private readonly bool _useSharedCanvasDevice;
        private readonly bool _forceSoftwareRenderer;
        private CompositionGraphicsDevice _compositionDevice;

        /// <summary>
        /// Gets the Compositor
        /// </summary>
        public Compositor Compositor { get; private set; }

        /// <summary>
        /// Gets the CanvasDevice
        /// </summary>
        public CanvasDevice Device { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositionGenerator"/> class.
        /// Constructor
        /// </summary>
        /// <param name="compositor">Compositor</param>
        /// <param name="useSharedCanvasDevice">Indicates whether to use a shared CanvasDevice or to create a new one.</param>
        /// <param name="forceSoftwareRenderer">Indicates whether to use Software Renderer when creating a new CanvasDevice.</param>
        public CompositionGenerator(Compositor compositor, bool useSharedCanvasDevice = true, bool forceSoftwareRenderer = false)
        {
            // Compositor
            Compositor = compositor ?? throw new ArgumentNullException(nameof(compositor), "Compositor cannot be null!");

            // Disposing Lock
            _disposingLock = new object();

            _useSharedCanvasDevice = useSharedCanvasDevice;
            _forceSoftwareRenderer = forceSoftwareRenderer;
        }

        /// <summary>
        /// Renders the CanvasBitmap on the CompositionDrawingSurface based on the given options.
        /// </summary>
        /// <param name="surfaceLock">The object to lock to prevent multiple threads
        /// from accessing the surface at the same time.</param>
        /// <param name="surface">CompositionDrawingSurface on which the CanvasBitmap has to be rendered.</param>
        /// <param name="canvasBitmap">CanvasBitmap created by loading the image from the Uri</param>
        /// <param name="options">Describes the image's resize and alignment options in the allocated space.</param>
        private static void RenderBitmap(object surfaceLock, CompositionDrawingSurface surface, CanvasBitmap canvasBitmap, ImageSurfaceOptions options)
        {
            var surfaceSize = surface.Size;

            // If the canvasBitmap is null, then just fill the surface with the SurfaceBackgroundColor
            if (canvasBitmap == null)
            {
                // No need to render if the width and/or height of the surface is zero
                if (surfaceSize.IsEmpty || surfaceSize.Width.IsZero() || surfaceSize.Height.IsZero())
                {
                    return;
                }

                // Since multiple threads could be trying to get access to the device/surface
                // at the same time, we need to do any device/surface work under a lock.
                lock (surfaceLock)
                {
                    using var session = CanvasComposition.CreateDrawingSession(surface);

                    // Clear the surface with the SurfaceBackgroundColor
                    session.Clear(options.SurfaceBackgroundColor);

                    // No need to proceed further
                    return;
                }
            }

            // Since multiple threads could be trying to get access to the device/surface
            // at the same time, we need to do any device/surface work under a lock.
            lock (surfaceLock)
            {
                // Is AutoResize Enabled?
                if (options.AutoResize)
                {
                    // If AutoResize is allowed and the canvasBitmap size and surface size are
                    // not matching then resize the surface to match the canvasBitmap size.
                    //
                    // NOTE: HorizontalAlignment, Vertical Alignment and Stretch will be
                    // handled by the CompositionSurfaceBrush created using this surface.
                    if (canvasBitmap.Size != surfaceSize)
                    {
                        // Resize the surface
                        CanvasComposition.Resize(surface, canvasBitmap.Size);
                        surfaceSize = canvasBitmap.Size;
                    }

                    // No need to render if the width and/or height of the surface is zero
                    if (surfaceSize.IsEmpty || surface.Size.Width.IsZero() || surface.Size.Height.IsZero())
                    {
                        return;
                    }

                    // Draw the image to the surface
                    using var session = CanvasComposition.CreateDrawingSession(surface);

                    // Clear the surface with the SurfaceBackgroundColor
                    session.Clear(options.SurfaceBackgroundColor);

                    // Render the image
                    session.DrawImage(
                        canvasBitmap,                                                       // CanvasBitmap
                        new Rect(0, 0, surfaceSize.Width, surfaceSize.Height),              // Target Rectangle
                        new Rect(0, 0, canvasBitmap.Size.Width, canvasBitmap.Size.Height),  // Source Rectangle
                        options.Opacity,                                                    // Opacity
                        options.Interpolation);                                             // Interpolation
                }
                else
                {
                    // No need to render if the width and/or height of the surface is zero
                    if (surfaceSize.IsEmpty || surface.Size.Width.IsZero() || surface.Size.Height.IsZero())
                    {
                        return;
                    }

                    // Get the optimum size that can fit the surface
                    var targetRect = Utils.GetOptimumSize(
                        canvasBitmap.Size.Width,
                        canvasBitmap.Size.Height,
                        surfaceSize.Width,
                        surfaceSize.Height,
                        options.Stretch,
                        options.HorizontalAlignment,
                        options.VerticalAlignment);

                    // Draw the image to the surface
                    using var session = CanvasComposition.CreateDrawingSession(surface);

                    // Clear the surface with the SurfaceBackgroundColor
                    session.Clear(options.SurfaceBackgroundColor);

                    // Render the image
                    session.DrawImage(
                        canvasBitmap,               // CanvasBitmap
                        targetRect,                 // Target Rectangle
                        canvasBitmap.Bounds,        // Source Rectangle
                        options.Opacity,            // Opacity
                        options.Interpolation);     // Interpolation
                }
            }
        }

        /// <summary>
        /// Renders the mask using the CanvasBitmap's alpha values on the CompositionDrawingSurface based on the given options.
        /// </summary>
        /// <param name="device">CanvasDevice</param>
        /// <param name="surfaceLock">The object to lock to prevent multiple threads
        /// from accessing the surface at the same time.</param>
        /// <param name="surface">CompositionDrawingSurface on which the CanvasBitmap has to be rendered.</param>
        /// <param name="canvasBitmap">CanvasBitmap created by loading the image from the Uri</param>
        /// <param name="padding">The padding between the IImageMaskSurface outer bounds and the bounds of the area where
        /// the mask, created from the loaded image's alpha values, should be rendered.</param>
        /// <param name="options">Describes the image's resize and alignment options in the allocated space.</param>
        private static void RenderBitmapMask(CanvasDevice device, object surfaceLock, CompositionDrawingSurface surface, CanvasBitmap canvasBitmap, Thickness padding, ImageSurfaceOptions options)
        {
            var surfaceSize = surface.Size;

            // If the canvasBitmap is null, then just fill the surface with transparent color
            if (canvasBitmap == null)
            {
                // No need to render if the width and/or height of the surface is zero
                if (surfaceSize.IsEmpty || surfaceSize.Width.IsZero() || surfaceSize.Height.IsZero())
                {
                    return;
                }

                // Since multiple threads could be trying to get access to the device/surface
                // at the same time, we need to do any device/surface work under a lock.
                lock (surfaceLock)
                {
                    using var session = CanvasComposition.CreateDrawingSession(surface);

                    // Clear the surface with the transparent color
                    session.Clear(options.SurfaceBackgroundColor);

                    // No need to proceed further
                    return;
                }
            }

            // Since multiple threads could be trying to get access to the device/surface
            // at the same time, we need to do any device/surface work under a lock.
            lock (surfaceLock)
            {
                // No need to render if the width and/or height of the surface is zero
                if (surfaceSize.IsEmpty || surface.Size.Width.IsZero() || surface.Size.Height.IsZero())
                {
                    return;
                }

                // Get the available size on the surface
                var paddingSize = padding.CollapseThickness();
                var availableWidth = Math.Max(0, surfaceSize.Width - paddingSize.Width);
                var availableHeight = Math.Max(0, surfaceSize.Height - paddingSize.Height);

                if (availableWidth.IsZero() || availableHeight.IsZero())
                {
                    return;
                }

                // Get the optimum size that can fit the available size on the surface
                var targetRect = Utils.GetOptimumSize(
                    canvasBitmap.Size.Width,
                    canvasBitmap.Size.Height,
                    availableWidth,
                    availableHeight,
                    options.Stretch,
                    options.HorizontalAlignment,
                    options.VerticalAlignment);

                // Add the padding to the targetRect
                targetRect.X += padding.Left;
                targetRect.Y += padding.Top;

                // Resize the image to the target size
                var imageCmdList = new CanvasCommandList(device);
                using (var ds = imageCmdList.CreateDrawingSession())
                {
                    ds.DrawImage(canvasBitmap, targetRect, canvasBitmap.Bounds, 1f, options.Interpolation);
                }

                // Fill the entire surface with White
                var surfaceBounds = new Rect(0, 0, (float)surfaceSize.Width, (float)surfaceSize.Height);

                var rectCmdList = new CanvasCommandList(device);
                using (var ds = rectCmdList.CreateDrawingSession())
                {
                    ds.FillRectangle(surfaceBounds, Colors.White);
                }

                // Create the mask using the image's alpha values
                var alphaEffect = new AlphaMaskEffect
                {
                    Source = rectCmdList,
                    AlphaMask = imageCmdList
                };

                // Apply Gaussian blur on the mask to create the final mask
                var blurEffect = new GaussianBlurEffect
                {
                    Source = alphaEffect,
                    BlurAmount = options.BlurRadius
                };

                // Draw the final mask to the surface
                using var session = CanvasComposition.CreateDrawingSession(surface);

                // Clear the surface with the SurfaceBackgroundColor
                session.Clear(options.SurfaceBackgroundColor);

                // Render the mask
                session.DrawImage(
                    blurEffect,               // CanvasBitmap
                    surfaceBounds,            // Target Rectangle
                    surfaceBounds,            // Source Rectangle
                    options.Opacity,          // Opacity
                    options.Interpolation);   // Interpolation
            }
        }

        /// <summary>
        /// Reloads the <see cref="Device"/> and <see cref="_compositionDevice"/> fields.
        /// </summary>
        /// <param name="raiseEvent">Indicates whether the DeviceReplacedEvent should be raised.</param>
        private void InitializeDevices(bool raiseEvent = false)
        {
            lock (this._disposingLock)
            {
                if (!(this.Device is null))
                {
                    this.Device.DeviceLost -= this.OnDeviceLost;
                }

                if (!(this._compositionDevice is null))
                {
                    this._compositionDevice.RenderingDeviceReplaced -= this.OnRenderingDeviceReplaced;
                }

                // Canvas Device
                Device = _useSharedCanvasDevice
                    ? CanvasDevice.GetSharedDevice(_forceSoftwareRenderer)
                    : new CanvasDevice(_forceSoftwareRenderer);

                // Composition Graphics Device
                _compositionDevice = CanvasComposition.CreateCompositionGraphicsDevice(Compositor, Device);

                _compositionDevice.RenderingDeviceReplaced += this.OnRenderingDeviceReplaced;

                this.Device.DeviceLost += this.OnDeviceLost;
                this._compositionDevice.RenderingDeviceReplaced += this.OnRenderingDeviceReplaced;

                if (raiseEvent)
                {
                    // Raise the device replaced event
                    RaiseDeviceReplacedEvent();
                }
            }
        }

        /// <summary>
        /// Invokes <see cref="InitializeDevices"/> when the current <see cref="CanvasDevice"/> is lost and raises the DeviceReplacedEvent.
        /// </summary>
        private void OnDeviceLost(CanvasDevice sender, object args)
        {
            InitializeDevices(true);
        }

        /// <summary>
        /// Invokes <see cref="InitializeDevices"/> when the current <see cref="CompositionGraphicsDevice"/> changes rendering device and raises the DeviceReplacedEvent.
        /// </summary>
        private void OnRenderingDeviceReplaced(CompositionGraphicsDevice sender, RenderingDeviceReplacedEventArgs args)
        {
            InitializeDevices(true);
        }

        /// <summary>
        /// Raises the DeviceReplacedEvent
        /// </summary>
        private void RaiseDeviceReplacedEvent()
        {
            var deviceEvent = DeviceReplaced;
            deviceEvent?.Invoke(this, new EventArgs());
        }

        /// <summary>
        /// <para>Creates an Empty IMaskSurface having the no size and geometry.</para>
        /// <para>NOTE: Use this API if you want to create an Empty IMaskSurface first
        /// and change its geometry and/or size of the IMaskSurface later.</para>
        /// </summary>
        /// <returns>IMaskSurface</returns>
        public IMaskSurface CreateMaskSurface()
        {
            return CreateMaskSurface(default, null, Vector2.Zero);
        }

        /// <summary>
        /// Creates an IMaskSurface having the given size and geometry with MaskMode as True.
        /// The geometry is filled with white color. The surface not covered by the geometry is
        /// transparent.
        /// </summary>
        /// <param name="size">Size of the mask</param>
        /// <param name="geometry">Geometry of the mask</param>
        /// <returns>IMaskSurface</returns>
        public IMaskSurface CreateMaskSurface(Size size, CanvasGeometry geometry)
        {
            return CreateMaskSurface(size, geometry, Vector2.Zero);
        }

        /// <summary>
        /// Creates an IMaskSurface having the given size and geometry with MaskMode as True.
        /// The geometry is filled with white color. The surface not covered by the geometry is
        /// transparent.
        /// </summary>
        /// <param name="size">Size of the mask</param>
        /// <param name="geometry">Geometry of the mask</param>
        /// <param name="offset">The offset from the top left corner of the ICompositionSurface where
        /// the Geometry is rendered.</param>
        /// <returns>IMaskSurface</returns>
        public IMaskSurface CreateMaskSurface(Size size, CanvasGeometry geometry, Vector2 offset)
        {
            // Initialize the mask
            IMaskSurface mask = new MaskSurface(this, size, geometry, offset);

            // Render the mask
            mask.Redraw();

            return mask;
        }

        /// <summary>
        /// <para>Creates an Empty IGaussianMaskSurface having the no size and geometry.</para>
        /// <para>NOTE: Use this API if you want to create an Empty IGaussianMaskSurface first
        /// and change its geometry, size, offset and/or blurRadius of the IGaussianMaskSurface later.</para>
        /// </summary>
        /// <returns>IGaussianMaskSurface</returns>
        public IGaussianMaskSurface CreateGaussianMaskSurface()
        {
            return CreateGaussianMaskSurface(default, null, Vector2.Zero, 0);
        }

        /// <summary>
        /// Creates a IGaussianMaskSurface having the given size and geometry. The geometry is filled
        /// with white color and a Gaussian blur is applied to it. The surface not covered by the geometry is transparent.
        /// </summary>
        /// <param name="size">Size of the mask</param>
        /// <param name="geometry">Geometry of the mask</param>
        /// <param name="offset">The offset from the top left corner of the ICompositionSurface where
        /// the Geometry is rendered.</param>
        /// <param name="blurRadius">Radius of Gaussian Blur to be applied on the IGaussianMaskSurface</param>
        /// <returns>IGaussianMaskSurface</returns>
        public IGaussianMaskSurface CreateGaussianMaskSurface(Size size, CanvasGeometry geometry, Vector2 offset, float blurRadius)
        {
            // Initialize the mask
            IGaussianMaskSurface mask = new GaussianMaskSurface(this, size, geometry, offset, blurRadius);

            // Render the mask
            mask.Redraw();

            return mask;
        }

        /// <summary>
        /// <para>Creates an empty GeometrySurface having the no size and geometry.</para>
        /// <para>NOTE: Use this API if you want to create an Empty IGeometrySurface
        /// first and change its geometry and/or size, fillColor or stroke later.</para>
        /// </summary>
        /// <returns>IGeometrySurface</returns>
        public IGeometrySurface CreateGeometrySurface()
        {
            // Initialize the geometrySurface
            IGeometrySurface geometrySurface = new GeometrySurface(this, default, null, null, Colors.Transparent, Colors.Transparent);

            // Render the geometrySurface
            geometrySurface.Redraw();

            return geometrySurface;
        }

        /// <summary>
        /// Creates a GeometrySurface having the given size, geometry, stroke
        /// </summary>
        /// <param name="size">Size of the GeometrySurface</param>
        /// <param name="geometry">Geometry to be rendered on the GeometrySurface</param>
        /// <param name="stroke">ICanvasStroke defining the outline for the geometry</param>
        /// <returns>IGeometrySurface</returns>
        public IGeometrySurface CreateGeometrySurface(Size size, CanvasGeometry geometry, ICanvasStroke stroke)
        {
            // Initialize the geometrySurface
            IGeometrySurface geometrySurface = new GeometrySurface(this, size, geometry, stroke, Colors.Transparent, Colors.Transparent);

            // Render the geometrySurface
            geometrySurface.Redraw();

            return geometrySurface;
        }

        /// <summary>
        /// Creates a GeometrySurface having the given size, geometry, foreground color with
        /// MaskMode as False.
        /// </summary>
        /// <param name="size">Size of the mask</param>
        /// <param name="geometry">Geometry of the mask</param>
        /// <param name="fillColor">Fill color of the geometry.</param>
        /// <returns>IGeometrySurface</returns>
        public IGeometrySurface CreateGeometrySurface(Size size, CanvasGeometry geometry, Color fillColor)
        {
            // Initialize the geometrySurface
            IGeometrySurface geometrySurface = new GeometrySurface(this, size, geometry, null, fillColor, Colors.Transparent);

            // Render the geometrySurface
            geometrySurface.Redraw();

            return geometrySurface;
        }

        /// <summary>
        /// Creates a GeometrySurface having the given size, geometry, stroke and fill color
        /// </summary>
        /// <param name="size">Size of the GeometrySurface</param>
        /// <param name="geometry">Geometry to be rendered on the GeometrySurface</param>
        /// <param name="stroke">ICanvasStroke defining the outline for the geometry</param>
        /// <param name="fillColor">Fill color of the geometry.</param>
        /// <returns>IGeometrySurface</returns>
        public IGeometrySurface CreateGeometrySurface(Size size, CanvasGeometry geometry, ICanvasStroke stroke, Color fillColor)
        {
            // Initialize the geometrySurface
            IGeometrySurface geometrySurface = new GeometrySurface(this, size, geometry, stroke, fillColor, Colors.Transparent);

            // Render the geometrySurface
            geometrySurface.Redraw();

            return geometrySurface;
        }

        /// <summary>
        /// Creates a GeometrySurface having the given size, geometry, foreground color and
        /// background color.
        /// </summary>
        /// <param name="size">Size of the mask</param>
        /// <param name="geometry">Geometry of the mask</param>
        /// <param name="fillColor">Fill color of the geometry</param>
        /// <param name="backgroundColor">Fill color of the Mask surface background which is
        /// not covered by the geometry</param>
        /// <returns>IGeometrySurface</returns>
        public IGeometrySurface CreateGeometrySurface(Size size, CanvasGeometry geometry, Color fillColor, Color backgroundColor)
        {
            // Initialize the geometrySurface
            IGeometrySurface geometrySurface = new GeometrySurface(this, size, geometry, null, fillColor, backgroundColor);

            // Render the geometrySurface
            geometrySurface.Redraw();

            return geometrySurface;
        }

        /// <summary>
        /// Creates a GeometrySurface having the given size, geometry, stroke, fill color and
        /// background color.
        /// </summary>
        /// <param name="size">Size of the GeometrySurface</param>
        /// <param name="geometry">Geometry to be rendered on the GeometrySurface</param>
        /// <param name="stroke">ICanvasStroke defining the outline for the geometry</param>
        /// <param name="fillColor">Fill color of the geometry</param>
        /// <param name="backgroundColor">Fill color of the GeometrySurface background which is
        /// not covered by the geometry</param>
        /// <returns>IGeometrySurface</returns>
        public IGeometrySurface CreateGeometrySurface(Size size, CanvasGeometry geometry, ICanvasStroke stroke, Color fillColor, Color backgroundColor)
        {
            // Initialize the geometrySurface
            IGeometrySurface geometrySurface = new GeometrySurface(this, size, geometry, stroke, fillColor, backgroundColor);

            // Render the geometrySurface
            geometrySurface.Redraw();

            return geometrySurface;
        }

        /// <summary>
        /// Creates a GeometrySurface having the given size, geometry and foreground brush with
        /// MaskMode as False.
        /// </summary>
        /// <param name="size">Size of the mask</param>
        /// <param name="geometry">Geometry of the mask</param>
        /// <param name="fillBrush">The brush with which the geometry has to be filled</param>
        /// <returns>IGeometrySurface</returns>
        public IGeometrySurface CreateGeometrySurface(Size size, CanvasGeometry geometry, ICanvasBrush fillBrush)
        {
            // Create the background brush
            var backgroundBrush = new CanvasSolidColorBrush(Device, Colors.Transparent);

            // Initialize the geometrySurface
            IGeometrySurface geometrySurface = new GeometrySurface(this, size, geometry, null, fillBrush, backgroundBrush);

            // Render the geometrySurface
            geometrySurface.Redraw();

            return geometrySurface;
        }

        /// <summary>
        /// Creates a GeometrySurface having the given size, geometry, stroke and fill brush.
        /// </summary>
        /// <param name="size">Size of the GeometrySurface</param>
        /// <param name="geometry">Geometry to be rendered on the GeometrySurface</param>
        /// <param name="stroke">ICanvasStroke defining the outline for the geometry</param>
        /// <param name="fillBrush">The brush with which the geometry has to be filled</param>
        /// <returns>IGeometrySurface</returns>
        public IGeometrySurface CreateGeometrySurface(Size size, CanvasGeometry geometry, ICanvasStroke stroke, ICanvasBrush fillBrush)
        {
            // Create the background brush
            var backgroundBrush = new CanvasSolidColorBrush(Device, Colors.Transparent);

            // Initialize the geometrySurface
            IGeometrySurface geometrySurface = new GeometrySurface(this, size, geometry, stroke, fillBrush, backgroundBrush);

            // Render the geometrySurface
            geometrySurface.Redraw();

            return geometrySurface;
        }

        /// <summary>
        /// Creates a GeometrySurface having the given size, geometry, foreground brush and
        /// background brush with MaskMode as False.
        /// </summary>
        /// <param name="size">Size of the mask</param>
        /// <param name="geometry">Geometry of the mask</param>
        /// <param name="fillBrush">The brush with which the geometry has to be filled</param>
        /// <param name="backgroundBrush">The brush to fill the Mask background surface which is
        /// not covered by the geometry</param>
        /// <returns>IGeometrySurface</returns>
        public IGeometrySurface CreateGeometrySurface(Size size, CanvasGeometry geometry, ICanvasBrush fillBrush, ICanvasBrush backgroundBrush)
        {
            // Initialize the geometrySurface
            IGeometrySurface geometrySurface = new GeometrySurface(this, size, geometry, null, fillBrush, backgroundBrush);

            // Render the geometrySurface
            geometrySurface.Redraw();

            return geometrySurface;
        }

        /// <summary>
        /// Creates a GeometrySurface having the given size, geometry, stroke, fill brush and
        /// background brush.
        /// </summary>
        /// <param name="size">Size of the GeometrySurface</param>
        /// <param name="geometry">Geometry to be rendered on the GeometrySurface</param>
        /// <param name="stroke">ICanvasStroke defining the outline for the geometry</param>
        /// <param name="fillBrush">The brush with which the geometry has to be filled</param>
        /// <param name="backgroundBrush">The brush to fill the GeometrySurface background which is
        /// not covered by the geometry</param>
        /// <returns>IGeometrySurface</returns>
        public IGeometrySurface CreateGeometrySurface(Size size, CanvasGeometry geometry, ICanvasStroke stroke, ICanvasBrush fillBrush, ICanvasBrush backgroundBrush)
        {
            // Initialize the geometrySurface
            IGeometrySurface geometrySurface = new GeometrySurface(this, size, geometry, stroke, fillBrush, backgroundBrush);

            // Render the geometrySurface
            geometrySurface.Redraw();

            return geometrySurface;
        }

        /// <summary>
        /// Creates a GeometrySurface having the given size, geometry, foreground brush and
        /// background color with MaskMode as False.
        /// </summary>
        /// <param name="size">Size of the mask</param>
        /// <param name="geometry">Geometry of the mask</param>
        /// <param name="fillBrush">The brush with which the geometry has to be filled</param>
        /// <param name="backgroundColor">Fill color of the Mask background surface which is
        /// not covered by the geometry</param>
        /// <returns>IGeometrySurface</returns>
        public IGeometrySurface CreateGeometrySurface(Size size, CanvasGeometry geometry, ICanvasBrush fillBrush, Color backgroundColor)
        {
            // Create the background brush
            var backgroundBrush = new CanvasSolidColorBrush(Device, backgroundColor);

            // Initialize the geometrySurface
            IGeometrySurface geometrySurface = new GeometrySurface(this, size, geometry, null, fillBrush, backgroundBrush);

            // Render the geometrySurface
            geometrySurface.Redraw();

            return geometrySurface;
        }

        /// <summary>
        /// Creates a GeometrySurface having the given size, geometry, stroke, fill brush and
        /// background color.
        /// </summary>
        /// <param name="size">Size of the GeometrySurface</param>
        /// <param name="geometry">Geometry to be rendered on the GeometrySurface</param>
        /// <param name="stroke">ICanvasStroke defining the outline for the geometry</param>
        /// <param name="fillBrush">The brush with which the geometry has to be filled</param>
        /// <param name="backgroundColor">Fill color of the GeometrySurface background which is
        /// not covered by the geometry</param>
        /// <returns>IGeometrySurface</returns>
        public IGeometrySurface CreateGeometrySurface(Size size, CanvasGeometry geometry, ICanvasStroke stroke, ICanvasBrush fillBrush, Color backgroundColor)
        {
            // Create the background brush
            var backgroundBrush = new CanvasSolidColorBrush(Device, backgroundColor);

            // Initialize the geometrySurface
            IGeometrySurface geometrySurface = new GeometrySurface(this, size, geometry, stroke, fillBrush, backgroundBrush);

            // Render the geometrySurface
            geometrySurface.Redraw();

            return geometrySurface;
        }

        /// <summary>
        /// Creates a GeometrySurface having the given size, geometry, foreground color and
        /// background brush.
        /// </summary>
        /// <param name="size">Size of the mask</param>
        /// <param name="geometry">Geometry of the mask</param>
        /// <param name="fillColor">Fill color of the geometry</param>
        /// <param name="backgroundBrush">The brush to fill the Mask background surface which is
        /// not covered by the geometry</param>
        /// <returns>IGeometrySurface</returns>
        public IGeometrySurface CreateGeometrySurface(Size size, CanvasGeometry geometry, Color fillColor, ICanvasBrush backgroundBrush)
        {
            // Create the foreground brush
            var foregroundBrush = new CanvasSolidColorBrush(Device, fillColor);

            // Initialize the geometrySurface
            IGeometrySurface geometrySurface = new GeometrySurface(this, size, geometry, null, foregroundBrush, backgroundBrush);

            // Render the geometrySurface
            geometrySurface.Redraw();

            return geometrySurface;
        }

        /// <summary>
        /// Creates a GeometrySurface having the given size, geometry, stroke, fill color and
        /// background brush.
        /// </summary>
        /// <param name="size">Size of the GeometrySurface</param>
        /// <param name="geometry">Geometry to be rendered on the GeometrySurface</param>
        /// <param name="stroke">ICanvasStroke defining the outline for the geometry</param>
        /// <param name="fillColor">Fill color of the geometry</param>
        /// <param name="backgroundBrush">The brush to fill the GeometrySurface background which is
        /// not covered by the geometry</param>
        /// <returns>IGeometrySurface</returns>
        public IGeometrySurface CreateGeometrySurface(Size size, CanvasGeometry geometry, ICanvasStroke stroke, Color fillColor, ICanvasBrush backgroundBrush)
        {
            // Create the foreground brush
            var foregroundBrush = new CanvasSolidColorBrush(Device, fillColor);

            // Initialize the geometrySurface
            IGeometrySurface geometrySurface = new GeometrySurface(this, size, geometry, stroke, foregroundBrush, backgroundBrush);

            // Render the geometrySurface
            geometrySurface.Redraw();

            return geometrySurface;
        }

        /// <summary>
        /// Creates an IImageSurface having the given size onto which an image (based on the Uri
        /// and the options) is loaded.
        /// </summary>
        /// <param name="uri">Uri of the image to be loaded onto the IImageSurface.</param>
        /// <param name="size">New size of the IImageSurface</param>
        /// <param name="options">Describes the image's resize and alignment options in the allocated space.</param>
        /// <returns>ICompositionSurfaceImage</returns>
        public async Task<IImageSurface> CreateImageSurfaceAsync(Uri uri, Size size, ImageSurfaceOptions options)
        {
            // Initialize the IImageSurface
            var imageSurface = new ImageSurface(this, uri, size, options);

            // Render the image onto the surface
            await imageSurface.RedrawAsync();

            return imageSurface;
        }

        /// <summary>
        /// Creates an IImageSurface having the given size onto which the given image is loaded.
        /// </summary>
        /// <param name="bitmap">Image that will be loaded onto the IImageSurface.</param>
        /// <param name="size">Size of the IImageSurface</param>
        /// <param name="options">The image's resize and alignment options in the allocated space.</param>
        /// <returns>IImageSurface</returns>
        public IImageSurface CreateImageSurface(CanvasBitmap bitmap, Size size, ImageSurfaceOptions options)
        {
            // Create a new IImageSurface using the given imageSurface
            var imageSurface = new ImageSurface(this, bitmap, size, options);

            // Render the image onto the surface
            imageSurface.Redraw();

            return imageSurface;
        }

        /// <summary>
        /// Creates a copy of the given IImageSurface
        /// </summary>
        /// <param name="imageSurface">IImageSurface to copy</param>
        /// <returns>Copy of the given IImageSurface</returns>
        public IImageSurface CreateImageSurface(IImageSurface imageSurface)
        {
            if (imageSurface != null)
            {
                // Create a new IImageSurface using the given imageSurface
                var newImageSurface = new ImageSurface(this, imageSurface.SurfaceBitmap, imageSurface.Size, imageSurface.Options);

                // Render the image onto the surface
                newImageSurface.Redraw();

                return newImageSurface;
            }

            // return an empty ImageSurface
            return CreateImageSurface(null, new Size(0, 0), ImageSurfaceOptions.Default);
        }

        /// <summary>
        /// Creates a copy of the given IImageMaskSurface
        /// </summary>
        /// <param name="imageMaskSurface">IImageMaskSurface to copy</param>
        /// <returns>IImageMaskSurface</returns>
        public IImageMaskSurface CreateImageMaskSurface(IImageMaskSurface imageMaskSurface)
        {
            if (imageMaskSurface != null)
            {
                // Create a new IImageMaskSurface using the given imageMaskSurface
                var newImageSurface = new ImageMaskSurface(
                    this,
                    imageMaskSurface.SurfaceBitmap,
                    imageMaskSurface.Size,
                    imageMaskSurface.MaskPadding,
                    imageMaskSurface.Options);

                // Render the image onto the surface
                newImageSurface.Redraw();

                return newImageSurface;
            }

            // return an empty ImageSurface
            return CreateImageMaskSurface(surfaceBitmap: null, new Size(0, 0), new Thickness(0), ImageSurfaceOptions.DefaultImageMaskOptions);
        }

        /// <summary>
        /// Creates an IImageMaskSurface using the alpha values of the given image at the specified offset using
        /// the given blur radius.
        /// </summary>
        /// <param name="surfaceBitmap">The CanvasBitmap whose alpha values will be used to create the Mask.</param>
        /// <param name="size">Size of the IImageMaskSurface</param>
        /// <param name="padding">The padding between the IImageMaskSurface outer bounds and the bounds of the area where
        /// the mask, created from the given image's alpha values, should be rendered.</param>
        /// <param name="blurRadius">Radius of the Gaussian blur applied to the the mask.</param>
        /// <returns>IImageMaskSurface</returns>
        public IImageMaskSurface CreateImageMaskSurface(CanvasBitmap surfaceBitmap, Size size, Thickness padding, float blurRadius)
        {
            return CreateImageMaskSurface(surfaceBitmap, size, padding, ImageSurfaceOptions.GetDefaultImageMaskOptionsForBlur(blurRadius));
        }

        /// <summary>
        /// Creates an IImageMaskSurface using the alpha values of the given image at the specified offset using
        /// the given options.
        /// </summary>
        /// <param name="surfaceBitmap">The CanvasBitmap whose alpha values will be used to create the Mask.</param>
        /// <param name="size">Size of the IImageMaskSurface</param>
        /// <param name="padding">The padding between the IImageMaskSurface outer bounds and the bounds of the area where
        /// the mask, created from the given image's alpha values, should be rendered.</param>
        /// <param name="options">The image's resize, alignment and blur radius options in the allocated space.</param>
        /// <returns>IImageMaskSurface</returns>
        public IImageMaskSurface CreateImageMaskSurface(CanvasBitmap surfaceBitmap, Size size, Thickness padding, ImageSurfaceOptions options)
        {
            var imageMaskSurface = new ImageMaskSurface(this, surfaceBitmap, size, padding, options);

            // Render the image onto the surface
            imageMaskSurface.Redraw();

            return imageMaskSurface;
        }

        /// <summary>
        /// Creates an IImageMaskSurface using the alpha values of the given IImageSurface's image at the specified offset using
        /// the given options.
        /// </summary>
        /// <param name="imageSurface">The IImageSurface whose image's alpha values will be used to create the Mask.</param>
        /// <param name="size">Size of the IImageMaskSurface</param>
        /// <param name="padding">The padding between the IImageMaskSurface outer bounds and the bounds of the area where
        /// the mask, created from the given image's alpha values, should be rendered.</param>
        /// <param name="options">The image's resize, alignment and blur radius options in the allocated space.</param>
        /// <returns>IImageMaskSurface</returns>
        public IImageMaskSurface CreateImageMaskSurface(IImageSurface imageSurface, Size size, Thickness padding, ImageSurfaceOptions options)
        {
            if (imageSurface != null)
            {
                // Create a new IImageSurface using the given imageSurface
                return CreateImageMaskSurface(imageSurface.SurfaceBitmap, size, padding, options);
            }

            // Create an empty ImageMaskSurface
            return CreateImageMaskSurface(surfaceBitmap: null, size, padding, options);
        }

        /// <summary>
        /// Creates an IImageMaskSurface using the alpha values of the image loaded from the Uri
        /// and rendered at the specified offset using the given options.
        /// </summary>
        /// <param name="uri">The URI of the image whose alpha values will be used to create the Mask.</param>
        /// <param name="size">Size of the IImageMaskSurface</param>
        /// <param name="padding">The padding between the IImageMaskSurface outer bounds and the bounds of the area where
        /// the mask, created from the given image's alpha values, should be rendered.</param>
        /// <param name="options">The image's resize, alignment and blur radius options in the allocated space.</param>
        /// <returns>IImageMaskSurface</returns>
        public async Task<IImageMaskSurface> CreateImageMaskSurfaceAsync(Uri uri, Size size, Thickness padding, ImageSurfaceOptions options)
        {
            var imageMaskSurface = new ImageMaskSurface(this, uri, size, padding, options);

            // Render the image onto the surface
            await imageMaskSurface.RedrawAsync();

            return imageMaskSurface;
        }

        /// <summary>
        /// Creates a reflection of the given Visual
        /// </summary>
        /// <param name="visual">Visual whose reflection has to be created</param>
        /// <param name="reflectionDistance">Distance of the reflection from the visual</param>
        /// <param name="reflectionLength">Normalized Length of the reflected visual that will be visible.</param>
        /// <param name="location"> <see cref="ReflectionLocation"/> - Location of the reflection with respect
        /// to the Visual - Bottom, Top, Left or Right</param>
        public void CreateReflection(ContainerVisual visual, float reflectionDistance = 0f, float reflectionLength = 0.7f, ReflectionLocation location = ReflectionLocation.Bottom)
        {
            // Create the visual layer that will contained the visual's reflection
            var reflectionLayer = Compositor.CreateLayerVisual();
            reflectionLayer.Size = visual.Size;
            reflectionLayer.CenterPoint = new Vector3(visual.Size * 0.5f, 0);

            // Create the effect to create the opacity mask
            var effect = new CompositeEffect
            {
                // CanvasComposite.DestinationIn - Intersection of source and mask.
                // Equation: O = MA * S
                // where O - Output pixel, MA - Mask Alpha, S - Source pixel.
                Mode = CanvasComposite.DestinationIn,
                Sources =
                        {
                                new CompositionEffectSourceParameter("source"),
                                new CompositionEffectSourceParameter("mask")
                        }
            };

            var effectFactory = Compositor.CreateEffectFactory(effect);
            var effectBrush = effectFactory.CreateBrush();

            // Create the gradient brush for the effect
            var gradientBrush = new CanvasLinearGradientBrush(Device, Colors.White, Colors.Transparent);

            // Based on the reflection location,
            // Set the Offset, RotationAxis and RotationAngleInDegrees of the reflectionLayer and
            // set the StartPoint and EndPoint of the gradientBrush
            switch (location)
            {
                case ReflectionLocation.Bottom:
                    reflectionLayer.RotationAxis = new Vector3(1, 0, 0);
                    reflectionLayer.RotationAngleInDegrees = 180;
                    reflectionLayer.Offset = new Vector3(0, visual.Size.Y + reflectionDistance, 0);
                    gradientBrush.StartPoint = new Vector2(visual.Size.X * 0.5f, 0);
                    gradientBrush.EndPoint = new Vector2(visual.Size.X * 0.5f, visual.Size.Y * reflectionLength);
                    break;
                case ReflectionLocation.Top:
                    reflectionLayer.RotationAxis = new Vector3(1, 0, 0);
                    reflectionLayer.RotationAngleInDegrees = -180;
                    reflectionLayer.Offset = new Vector3(0, -visual.Size.Y - reflectionDistance, 0);
                    gradientBrush.StartPoint = new Vector2(visual.Size.X * 0.5f, visual.Size.Y);
                    gradientBrush.EndPoint = new Vector2(visual.Size.X * 0.5f, visual.Size.Y * (1f - reflectionLength));
                    break;
                case ReflectionLocation.Left:
                    reflectionLayer.RotationAxis = new Vector3(0, 1, 0);
                    reflectionLayer.RotationAngleInDegrees = -180;
                    reflectionLayer.Offset = new Vector3(-visual.Size.X - reflectionDistance, 0, 0);
                    gradientBrush.StartPoint = new Vector2(visual.Size.X, visual.Size.Y * 0.5f);
                    gradientBrush.EndPoint = new Vector2(visual.Size.X * (1f - reflectionLength), visual.Size.Y * 0.5f);
                    break;
                case ReflectionLocation.Right:
                    reflectionLayer.RotationAxis = new Vector3(0, 1, 0);
                    reflectionLayer.RotationAngleInDegrees = 180;
                    reflectionLayer.Offset = new Vector3(visual.Size.X + reflectionDistance, 0, 0);
                    gradientBrush.StartPoint = new Vector2(0, visual.Size.Y * 0.5f);
                    gradientBrush.EndPoint = new Vector2(visual.Size.X * reflectionLength, visual.Size.Y * 0.5f);
                    break;
            }

            // Create a mask filled with gradientBrush
            var mask = CreateGeometrySurface(visual.Size.ToSize(), null, Colors.Transparent, gradientBrush);

            // Set the 'mask' parameter of the effectBrush
            effectBrush.SetSourceParameter("mask", Compositor.CreateSurfaceBrush(mask.Surface));

            // Set the effect for the reflection layer
            reflectionLayer.Effect = effectBrush;

            // Now we need to duplicate the visual tree of the visual
            ArrangeVisualReflection(visual, reflectionLayer, true);

            visual.Children.InsertAtTop(reflectionLayer);
        }

        /// <summary>
        /// Creates a CompositionDrawingSurface of given size
        /// </summary>
        /// <param name="surfaceLock">The object to lock to prevent multiple threads
        /// from accessing the surface at the same time.</param>
        /// <param name="size">Size of the CompositionDrawingSurface</param>
        /// <returns>CompositionDrawingSurface</returns>
        public CompositionDrawingSurface CreateDrawingSurface(object surfaceLock, Size size)
        {
            var surfaceSize = size;
            if (surfaceSize.IsEmpty)
            {
                // We start out with a size of 0,0 for the surface, because we don't know
                // the size of the image at this time. We resize the surface later.
                surfaceSize = new Size(0, 0);
            }

            // Since multiple threads could be trying to get access to the device/surface
            // at the same time, we need to do any device/surface work under a lock.
            lock (surfaceLock)
            {
                return this._compositionDevice.CreateDrawingSurface(surfaceSize, DirectXPixelFormat.B8G8R8A8UIntNormalized, DirectXAlphaMode.Premultiplied);
            }
        }

        /// <summary>
        /// Resizes the Mask Surface to the given size
        /// </summary>
        /// <param name="surfaceLock">The object to lock to prevent multiple threads
        /// from accessing the surface at the same time.</param>
        /// <param name="surface">CompositionDrawingSurface</param>
        /// <param name="size">New size of the Mask Surface</param>
        public void ResizeDrawingSurface(object surfaceLock, CompositionDrawingSurface surface, Size size)
        {
            // Cannot resize to Size.Empty. Will throw exception!
            if (size.IsEmpty)
            {
                return;
            }

            // Ensuring that the size contains positive values
            size.Width = Math.Max(0, size.Width);
            size.Height = Math.Max(0, size.Height);

            // Since multiple threads could be trying to get access to the device/surface
            // at the same time, we need to do any device/surface work under a lock.
            lock (surfaceLock)
            {
                CanvasComposition.Resize(surface, size);
            }
        }

        /// <summary>
        /// Redraws the IMaskSurface with the given size and geometry
        /// </summary>
        /// <param name="surfaceLock">The object to lock to prevent multiple threads
        /// from accessing the surface at the same time.</param>
        /// <param name="surface">CompositionDrawingSurface</param>
        /// <param name="size">Size of the IMaskSurface</param>
        /// <param name="geometry">Geometry of the IMaskSurface</param>
        /// <param name="offset">The offset from the top left corner of the ICompositionSurface where
        /// the Geometry is rendered.</param>
        public void RedrawMaskSurface(object surfaceLock, CompositionDrawingSurface surface, Size size, CanvasGeometry geometry, Vector2 offset)
        {
            // If the surface is not created, create it
            surface ??= this.CreateDrawingSurface(surfaceLock, size);

            // No need to render if the width and/or height of the surface is zero
            if (surface.Size.Width.IsZero() || surface.Size.Height.IsZero())
            {
                return;
            }

            // Since multiple threads could be trying to get access to the device/surface
            // at the same time, we need to do any device/surface work under a lock.
            lock (surfaceLock)
            {
                // Render the mask to the surface
                using var session = CanvasComposition.CreateDrawingSession(surface);
                session.Clear(Colors.Transparent);
                if (geometry != null)
                {
                    // If the geometry is not null then fill the geometry area at the given offset
                    // with White color. The rest of the area on the surface will be transparent.
                    // When this mask is applied to a visual, only the area that is white will be visible.
                    session.FillGeometry(geometry, offset, Colors.White);
                }
                else
                {
                    // If the geometry is null, then the entire mask with a padding, provided by the offset,
                    // should be filled with White. If the color is White.
                    // When this mask is applied to a visual, only the area that is white will be visible.
                    var width = Math.Max(0, (float)size.Width - (2 * offset.X));
                    var height = Math.Max(0, (float)size.Height - (2 * offset.Y));
                    var maskRect = new Rect(offset.X, offset.Y, width, height);
                    session.FillRectangle(maskRect, Colors.White);
                }
            }
        }

        /// <summary>
        /// Redraws the GaussianMaskSurface with the given size and geometry
        /// </summary>
        /// <param name="surfaceLock">The object to lock to prevent multiple threads
        /// from accessing the surface at the same time.</param>
        /// <param name="surface">CompositionDrawingSurface</param>
        /// <param name="size">Size of the GaussianMaskSurface</param>
        /// <param name="geometry">Geometry of the GaussianMaskSurface</param>
        /// <param name="offset">The offset from the top left corner of the ICompositionSurface where
        /// the Geometry is rendered.</param>
        /// <param name="blurRadius">Radius of Gaussian Blur to be applied.</param>
        public void RedrawGaussianMaskSurface(object surfaceLock, CompositionDrawingSurface surface, Size size, CanvasGeometry geometry, Vector2 offset, float blurRadius)
        {
            // If the surface is not created, create it
            surface ??= this.CreateDrawingSurface(surfaceLock, size);

            // No need to render if the width and/or height of the surface is zero
            if (surface.Size.Width.IsZero() || surface.Size.Height.IsZero())
            {
                return;
            }

            // Since multiple threads could be trying to get access to the device/surface
            // at the same time, we need to do any device/surface work under a lock.
            lock (surfaceLock)
            {
                // Render the mask to the surface
                using var session = CanvasComposition.CreateDrawingSession(surface);
                var cl = new CanvasCommandList(Device);
                using (var ds = cl.CreateDrawingSession())
                {
                    ds.Clear(Colors.Transparent);
                    if (geometry != null)
                    {
                        // If the geometry is not null then fill the geometry area with the White color at the specified offset.
                        // The rest of the area on the surface will be transparent.
                        // When this mask is applied to a visual, only the area that is white will be visible.
                        ds.FillGeometry(geometry, offset, Colors.White);
                    }
                    else
                    {
                        // If the geometry is null, then the entire mask with a padding, provided by the offset,
                        // should be filled with  white color.
                        // When this mask is applied to a visual, only the area that is white will be visible.
                        var width = Math.Max(0, (float)size.Width - (2 * offset.X));
                        var height = Math.Max(0, (float)size.Height - (2 * offset.Y));
                        var maskRect = new Rect(offset.X, offset.Y, width, height);
                        ds.FillRectangle(maskRect, Colors.White);
                    }
                }

                // Apply the Gaussian blur
                var blurGeometry = new GaussianBlurEffect()
                {
                    BlurAmount = blurRadius,
                    Source = cl,
                    BorderMode = EffectBorderMode.Soft,
                    Optimization = EffectOptimization.Quality
                };

                // Clear previously rendered mask (if any)
                session.Clear(Colors.Transparent);

                // Render the mask
                session.DrawImage(blurGeometry);
            }
        }

        /// <summary>
        /// Redraws the GeometrySurface with the given size, geometry, foreground brush and background brush
        /// </summary>
        /// <param name="surfaceLock">The object to lock to prevent multiple threads
        /// from accessing the surface at the same time.</param>
        /// <param name="surface">CompositionDrawingSurface</param>
        /// <param name="size">Size of the GeometrySurface</param>
        /// <param name="geometry">Geometry of the GeometrySurface</param>
        /// <param name="stroke">ICanvasStroke defining the outline for the geometry</param>
        /// <param name="fillBrush">The brush with which the geometry has to be filled</param>
        /// <param name="backgroundBrush">The brush with which the GeometrySurface background has to be filled</param>
        public void RedrawGeometrySurface(object surfaceLock, CompositionDrawingSurface surface, Size size, CanvasGeometry geometry, ICanvasStroke stroke, ICanvasBrush fillBrush, ICanvasBrush backgroundBrush)
        {
            // If the surface is not created, create it
            surface ??= this.CreateDrawingSurface(surfaceLock, size);

            // No need to render if the width and/or height of the surface is zero
            if (surface.Size.Width.IsZero() || surface.Size.Height.IsZero())
            {
                return;
            }

            // Since multiple threads could be trying to get access to the device/surface
            // at the same time, we need to do any device/surface work under a lock.
            lock (surfaceLock)
            {
                // Render the geometry to the surface
                using var session = CanvasComposition.CreateDrawingSession(surface);

                // Clear the surface
                session.Clear(Colors.Transparent);

                // First fill the background
                if (backgroundBrush is CanvasSolidColorBrush brush)
                {
                    // If the backgroundBrush is a SolidColorBrush then use the Clear()
                    // method to fill the surface with background color. It is faster.
                    // Clear the surface with the background color
                    session.Clear(brush.Color);
                }
                else
                {
                    // Fill the surface with the background brush.
                    session.FillRectangle(0, 0, (float)size.Width, (float)size.Height, backgroundBrush);
                }

                // If the geometry is not null then render the geometry
                if (geometry != null)
                {
                    // If there is a stroke, then scale back the geometry to fit the stroke in the
                    // surface.
                    if (stroke != null)
                    {
                        var scaleX = (float)((surface.Size.Width - stroke.Width) / surface.Size.Width);
                        var scaleY = (float)((surface.Size.Height - stroke.Width) / surface.Size.Height);

                        geometry = geometry.Transform(
                            Matrix3x2.CreateScale(new Vector2(scaleX, scaleY), surface.Size.ToVector2() * 0.5f));
                    }

                    // If fillBrush is defined then fill the geometry area
                    if (fillBrush != null)
                    {
                        session.FillGeometry(geometry, fillBrush);
                    }

                    // If stroke is defined then outline the geometry area
                    if (stroke != null)
                    {
                        session.DrawGeometry(geometry, stroke.Brush, stroke.Width, stroke.Style);
                    }
                }
            }
        }

        /// <summary>
        /// Resizes the ImageSurface to the given size and redraws the ImageSurface
        /// by rendering the canvasBitmap onto the surface.
        /// </summary>
        /// <param name="surfaceLock">The object to lock to prevent multiple threads
        /// from accessing the surface at the same time.</param>
        /// <param name="surface">CompositionDrawingSurface</param>
        /// <param name="options">Describes the image's resize and alignment options in the allocated space.</param>
        /// <param name="canvasBitmap">The CanvasBitmap on which the image is loaded.</param>
        public void RedrawImageSurface(object surfaceLock, CompositionDrawingSurface surface, ImageSurfaceOptions options, CanvasBitmap canvasBitmap)
        {
            // Render the image to the surface
            RenderBitmap(surfaceLock, surface, canvasBitmap, options);
        }

        /// <summary>
        /// Resizes the ImageSurface with the given size and redraws the ImageSurface by loading
        /// image from the new Uri.
        /// </summary>
        /// <param name="surfaceLock">The object to lock to prevent multiple threads
        /// from accessing the surface at the same time.</param>
        /// <param name="surface">CompositionDrawingSurface</param>
        /// <param name="uri">Uri of the image to be loaded onto the IImageSurface.</param>
        /// <param name="options">Describes the image's resize and alignment options in the allocated space.</param>
        /// <param name="canvasBitmap">The CanvasBitmap on which the image is loaded.</param>
        /// <returns>CanvasBitmap</returns>
        public async Task<CanvasBitmap> RedrawImageSurfaceAsync(object surfaceLock, CompositionDrawingSurface surface, Uri uri, ImageSurfaceOptions options, CanvasBitmap canvasBitmap)
        {
            if ((canvasBitmap == null) && (uri != null))
            {
                try
                {
                    canvasBitmap = await CanvasBitmap.LoadAsync(Device, uri);
                }
                catch (Exception)
                {
                    // Do nothing here as RenderBitmap method will fill the surface
                    // with options.SurfaceBackgroundColor as the image failed to load
                    // from Uri
                }
            }

            // Render the image to the surface
            RenderBitmap(surfaceLock, surface, canvasBitmap, options);

            return canvasBitmap;
        }

        /// <summary>
        /// Resizes the ImageMaskSurface to the given size and redraws the ImageMaskSurface
        /// by rendering the mask using the image's alpha values onto the surface.
        /// </summary>
        /// <param name="surfaceLock">The object to lock to prevent multiple threads from accessing the surface at the same time.</param>
        /// <param name="surface">CompositionDrawingSurface</param>
        /// <param name="padding">The padding between the IImageMaskSurface outer bounds and the bounds of the area where
        /// the mask, created from the loaded image's alpha values, should be rendered.</param>
        /// <param name="options">Describes the image's resize and alignment options and blur radius in the allocated space.</param>
        /// <param name="surfaceBitmap">The image whose alpha values is used to create the IImageMaskSurface.</param>
        public void RedrawImageMaskSurface(object surfaceLock, CompositionDrawingSurface surface, Thickness padding, ImageSurfaceOptions options, CanvasBitmap surfaceBitmap)
        {
            // Render the image mask to the surface
            RenderBitmapMask(Device, surfaceLock, surface, surfaceBitmap, padding, options);
        }

        /// <summary>
        /// Resizes the ImageMaskSurface to the given size and redraws the ImageMaskSurface by loading the image from the new Uri and
        /// rendering the mask using the image's alpha values onto the surface.
        /// </summary>
        /// <param name="surfaceLock">The object to lock to prevent multiple threads
        /// from accessing the surface at the same time.</param>
        /// <param name="surface">CompositionDrawingSurface</param>
        /// <param name="uri">Uri of the image to be loaded onto the IImageMaskSurface.</param>
        /// <param name="padding">The padding between the IImageMaskSurface outer bounds and the bounds of the area where
        /// the mask, created from the given image's alpha values, should be rendered.</param>
        /// <param name="options">Describes the image's resize and alignment options and blur radius in the allocated space.</param>
        /// <param name="surfaceBitmap">The CanvasBitmap on which the image is loaded.</param>
        /// <returns>The CanvasBitmap whose alpha values is used to create the IImageMaskSurface.</returns>
        public async Task<CanvasBitmap> RedrawImageMaskSurfaceAsync(object surfaceLock, CompositionDrawingSurface surface, Uri uri, Thickness padding, ImageSurfaceOptions options, CanvasBitmap surfaceBitmap)
        {
            if ((surfaceBitmap == null) && (uri != null))
            {
                try
                {
                    surfaceBitmap = await CanvasBitmap.LoadAsync(Device, uri);
                }
                catch (Exception)
                {
                    // Do nothing here as RenderBitmap method will fill the surface
                    // with options.SurfaceBackgroundColor as the image failed to load
                    // from Uri
                }
            }

            // Render the image mask to the surface
            RenderBitmapMask(Device, surfaceLock, surface, surfaceBitmap, padding, options);

            return surfaceBitmap;
        }

        /// <summary>
        /// Disposes the resources used by the CompositionMaskGenerator
        /// </summary>
        public void Dispose()
        {
            lock (_disposingLock)
            {
                Compositor = null;

                if (Device != null)
                {
                    // Only dispose the canvas device if we own the device.
                    if (this._useSharedCanvasDevice)
                    {
                        Device.Dispose();
                    }

                    Device = null;
                }

                if (this._compositionDevice == null)
                {
                    return;
                }

                this._compositionDevice.RenderingDeviceReplaced -= this.OnRenderingDeviceReplaced;

                // Only dispose the composition graphics device if we own the device.
                if (!this._useSharedCanvasDevice)
                {
                    this._compositionDevice.Dispose();
                }

                this._compositionDevice = null;
            }
        }

        /// <summary>
        /// Creates a duplicate of the visual tree of the given visual and arranges them within the reflectedParent.
        /// </summary>
        /// <param name="visual">Visual whose visual tree has to be duplicated</param>
        /// <param name="reflectedParent">Visual in which will host the duplicated visual tree</param>
        /// <param name="isRoot">Flag to indicate whether the given visual is the root of the visual tree to be duplicated.</param>
        private void ArrangeVisualReflection(ContainerVisual visual, ContainerVisual reflectedParent, bool isRoot = false)
        {
            if (visual == null)
            {
                return;
            }

            ContainerVisual reflectedVisual;

            if (visual is LayerVisual layerVisual)
            {
                reflectedVisual = Compositor.CreateLayerVisual();
                ((LayerVisual)reflectedVisual).Effect = layerVisual.Effect;
            }
            else if (visual is SpriteVisual spriteVisual)
            {
                reflectedVisual = Compositor.CreateSpriteVisual();
                ((SpriteVisual)reflectedVisual).Brush = spriteVisual.Brush;
                ((SpriteVisual)reflectedVisual).Shadow = spriteVisual.Shadow;
            }
            else
            {
                reflectedVisual = Compositor.CreateContainerVisual();
            }

            // Copy the Visual properties
            reflectedVisual.AnchorPoint = visual.AnchorPoint;
            reflectedVisual.BackfaceVisibility = visual.BackfaceVisibility;
            reflectedVisual.BorderMode = visual.BorderMode;
            reflectedVisual.CenterPoint = visual.CenterPoint;
            reflectedVisual.Clip = visual.Clip;
            reflectedVisual.CompositeMode = visual.CompositeMode;
            reflectedVisual.ImplicitAnimations = visual.ImplicitAnimations;
            reflectedVisual.IsVisible = visual.IsVisible;
            reflectedVisual.Offset = isRoot ? Vector3.One : visual.Offset;
            reflectedVisual.Opacity = visual.Opacity;
            reflectedVisual.Orientation = visual.Orientation;
            reflectedVisual.RotationAngle = visual.RotationAngle;
            reflectedVisual.RotationAngleInDegrees = visual.RotationAngleInDegrees;
            reflectedVisual.RotationAxis = visual.RotationAxis;
            reflectedVisual.Scale = visual.Scale;
            reflectedVisual.Size = visual.Size;
            reflectedVisual.TransformMatrix = visual.TransformMatrix;

            // Add the reflectedVisual to the reflectedParent's Children (at the Top)
            reflectedParent.Children.InsertAtTop(reflectedVisual);

            if (!visual.Children.Any())
            {
                return;
            }

            // Iterate each of the visual's Children and add them to
            // the reflectedVisual's Children (at the Top so that the
            // correct order is obtained)
            foreach (var child in visual.Children)
            {
                ArrangeVisualReflection((ContainerVisual)child, reflectedVisual);
            }
        }
    }
}
