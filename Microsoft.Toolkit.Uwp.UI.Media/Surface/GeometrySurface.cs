// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Microsoft.Graphics.Canvas.Brushes;
using Microsoft.Graphics.Canvas.Geometry;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Composition;

namespace Microsoft.Toolkit.Uwp.UI.Media
{
    /// <summary>
    /// Class for rendering custom shaped geometries onto ICompositionSurface.
    /// </summary>
    internal sealed class GeometrySurface : IGeometrySurface
    {
        private readonly object _surfaceLock;
        private ICompositionGeneratorInternal _generator;
        private CompositionDrawingSurface _surface;
        private CanvasGeometry _geometry;
        private ICanvasStroke _stroke;
        private ICanvasBrush _fill;
        private ICanvasBrush _backgroundBrush;

        /// <inheritdoc/>
        public ICompositionGenerator Generator => _generator;

        /// <inheritdoc/>
        public ICompositionSurface Surface => _surface;

        /// <inheritdoc/>
        public CanvasGeometry Geometry => _geometry;

        /// <inheritdoc/>
        public ICanvasStroke Stroke => _stroke;

        /// <inheritdoc/>
        public ICanvasBrush Fill => _fill;

        /// <inheritdoc/>
        public ICanvasBrush BackgroundBrush => _backgroundBrush;

        /// <inheritdoc/>
        public Size Size { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeometrySurface"/> class.
        /// Constructor
        /// </summary>
        /// <param name="generator">IComposiitonGeneratorInternal object.</param>
        /// <param name="size">Size of the GeometrySurface.</param>
        /// <param name="geometry">Geometry of the GeometrySurface.</param>
        /// <param name="stroke">Stroke for the geometry.</param>
        /// <param name="fillColor">Fill color of the geometry.</param>
        /// <param name="backgroundColor">Brush to fill the GeometrySurface background surface which is not covered by the geometry.</param>
        public GeometrySurface(ICompositionGeneratorInternal generator, Size size, CanvasGeometry geometry, ICanvasStroke stroke, Color fillColor, Color backgroundColor)
        {
            _generator = generator ?? throw new ArgumentException("Generator cannot be null!", nameof(generator));

            _surfaceLock = new object();
            _geometry = geometry;
            _stroke = stroke;
            _fill = new CanvasSolidColorBrush(_generator.Device, fillColor);
            _backgroundBrush = new CanvasSolidColorBrush(_generator.Device, backgroundColor);

            // Create GeometrySurface
            _surface = _generator.CreateDrawingSurface(_surfaceLock, size);

            // Set the size
            Size = _surface?.Size ?? new Size(0, 0);

            // Subscribe to DeviceReplaced event
            _generator.DeviceReplaced += OnDeviceReplaced;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeometrySurface"/> class.
        /// Constructor
        /// </summary>
        /// <param name="generator">IComposiitonGeneratorInternal object.</param>
        /// <param name="size">Size of the GeometrySurface.</param>
        /// <param name="geometry">Geometry of the GeometrySurface.</param>
        /// <param name="stroke">Stroke for the geometry.</param>
        /// <param name="fill">Brush to fill the geometry.</param>
        /// <param name="backgroundBrush">Brush to fill the GeometrySurface background surface which is not covered by the geometry.</param>
        public GeometrySurface(ICompositionGeneratorInternal generator, Size size, CanvasGeometry geometry, ICanvasStroke stroke, ICanvasBrush fill, ICanvasBrush backgroundBrush)
        {
            _generator = generator ?? throw new ArgumentException("Generator cannot be null!", nameof(generator));

            _generator = generator;
            _surfaceLock = new object();
            _geometry = geometry;
            _stroke = stroke;
            _fill = fill;
            _backgroundBrush = backgroundBrush;

            // Create GeometrySurface
            _surface = _generator.CreateDrawingSurface(_surfaceLock, size);

            // Set the size
            Size = _surface?.Size ?? new Size(0, 0);

            // Subscribe to DeviceReplaced event
            _generator.DeviceReplaced += OnDeviceReplaced;
        }

        /// <inheritdoc/>
        public void Redraw()
        {
            // Redraw the GeometrySurface
            RedrawSurface();
        }

        /// <inheritdoc/>
        public void Redraw(CanvasGeometry geometry)
        {
            // Set the new geometry
            _geometry = geometry;

            // Redraw the GeometrySurface
            RedrawSurface();
        }

        /// <inheritdoc/>
        public void Redraw(ICanvasStroke stroke)
        {
            // Set the new stroke
            _stroke = stroke;

            // Redraw the GeometrySurface
            RedrawSurface();
        }

        /// <inheritdoc/>
        public void Redraw(Color fillColor)
        {
            // Set the fill
            if (_fill is CanvasSolidColorBrush fillBrush)
            {
                fillBrush.Color = fillColor;
            }
            else
            {
                _fill = new CanvasSolidColorBrush(_generator.Device, fillColor);
            }

            // Redraw the GeometrySurface
            RedrawSurface();
        }

        /// <inheritdoc/>
        public void Redraw(ICanvasStroke stroke, Color fillColor)
        {
            // Set the new stroke
            _stroke = stroke;

            // Set the fill
            if (_fill is CanvasSolidColorBrush fillBrush)
            {
                fillBrush.Color = fillColor;
            }
            else
            {
                _fill = new CanvasSolidColorBrush(_generator.Device, fillColor);
            }

            // Redraw the GeometrySurface
            RedrawSurface();
        }

        /// <inheritdoc/>
        public void Redraw(Color fillColor, Color backgroundColor)
        {
            // Set the fill
            if (_fill is CanvasSolidColorBrush fillBrush)
            {
                fillBrush.Color = fillColor;
            }
            else
            {
                _fill = new CanvasSolidColorBrush(_generator.Device, fillColor);
            }

            // Set the backgroundBrush
            if (_backgroundBrush is CanvasSolidColorBrush backBrush)
            {
                backBrush.Color = backgroundColor;
            }
            else
            {
                _backgroundBrush = new CanvasSolidColorBrush(_generator.Device, backgroundColor);
            }

            // Redraw the GeometrySurface
            RedrawSurface();
        }

        /// <inheritdoc/>
        public void Redraw(ICanvasStroke stroke, Color fillColor, Color backgroundColor)
        {
            // Set the new stroke
            _stroke = stroke;

            // Set the fill
            if (_fill is CanvasSolidColorBrush fillBrush)
            {
                fillBrush.Color = fillColor;
            }
            else
            {
                _fill = new CanvasSolidColorBrush(_generator.Device, fillColor);
            }

            // Set the backgroundBrush
            if (_backgroundBrush is CanvasSolidColorBrush backBrush)
            {
                backBrush.Color = backgroundColor;
            }
            else
            {
                _backgroundBrush = new CanvasSolidColorBrush(_generator.Device, backgroundColor);
            }

            // Redraw the GeometrySurface
            RedrawSurface();
        }

        /// <inheritdoc/>
        public void Redraw(ICanvasBrush fillBrush)
        {
            // Set the fill
            _fill = fillBrush ?? new CanvasSolidColorBrush(_generator.Device, Colors.Transparent);

            // Redraw the GeometrySurface
            RedrawSurface();
        }

        /// <inheritdoc/>
        public void Redraw(ICanvasStroke stroke, ICanvasBrush fillBrush)
        {
            // Set the new stroke
            _stroke = stroke;

            // Set the fill
            _fill = fillBrush ?? new CanvasSolidColorBrush(_generator.Device, Colors.Transparent);

            // Redraw the GeometrySurface
            RedrawSurface();
        }

        /// <inheritdoc/>
        public void Redraw(ICanvasBrush fillBrush, ICanvasBrush backgroundBrush)
        {
            // Set the fill
            _fill = fillBrush ?? new CanvasSolidColorBrush(_generator.Device, Colors.Transparent);

            // Set the backgroundBrush
            _backgroundBrush = backgroundBrush ?? new CanvasSolidColorBrush(_generator.Device, Colors.Transparent);

            // Redraw the GeometrySurface
            RedrawSurface();
        }

        /// <inheritdoc/>
        public void Redraw(ICanvasStroke stroke, ICanvasBrush fillBrush, ICanvasBrush backgroundBrush)
        {
            // Set the new stroke
            _stroke = stroke;

            // Set the fill
            _fill = fillBrush ?? new CanvasSolidColorBrush(_generator.Device, Colors.Transparent);

            // Set the backgroundBrush
            _backgroundBrush = backgroundBrush ?? new CanvasSolidColorBrush(_generator.Device, Colors.Transparent);

            // Redraw the GeometrySurface
            RedrawSurface();
        }

        /// <inheritdoc/>
        public void Redraw(Color fillColor, ICanvasBrush backgroundBrush)
        {
            // Set the fill
            if (_fill is CanvasSolidColorBrush fillBrush)
            {
                fillBrush.Color = fillColor;
            }
            else
            {
                _fill = new CanvasSolidColorBrush(_generator.Device, fillColor);
            }

            // Set the backgroundBrush
            _backgroundBrush = backgroundBrush ?? new CanvasSolidColorBrush(_generator.Device, Colors.Transparent);

            // Redraw the GeometrySurface
            RedrawSurface();
        }

        /// <inheritdoc/>
        public void Redraw(ICanvasStroke stroke, Color fillColor, ICanvasBrush backgroundBrush)
        {
            // Set the new stroke
            _stroke = stroke;

            // Set the fill
            if (_fill is CanvasSolidColorBrush fillBrush)
            {
                fillBrush.Color = fillColor;
            }
            else
            {
                _fill = new CanvasSolidColorBrush(_generator.Device, fillColor);
            }

            // Set the backgroundBrush
            _backgroundBrush = backgroundBrush ?? new CanvasSolidColorBrush(_generator.Device, Colors.Transparent);

            // Redraw the GeometrySurface
            RedrawSurface();
        }

        /// <inheritdoc/>
        public void Redraw(ICanvasBrush fillBrush, Color backgroundColor)
        {
            // Set the fill
            _fill = fillBrush ?? new CanvasSolidColorBrush(_generator.Device, Colors.Transparent);

            // Set the backgroundBrush
            if (_backgroundBrush is CanvasSolidColorBrush backBrush)
            {
                backBrush.Color = backgroundColor;
            }
            else
            {
                _backgroundBrush = new CanvasSolidColorBrush(_generator.Device, backgroundColor);
            }

            // Redraw the GeometrySurface
            RedrawSurface();
        }

        /// <inheritdoc/>
        public void Redraw(ICanvasStroke stroke, ICanvasBrush fillBrush, Color backgroundColor)
        {
            // Set the new stroke
            _stroke = stroke;

            // Set the fill
            _fill = fillBrush ?? new CanvasSolidColorBrush(_generator.Device, Colors.Transparent);

            // Set the backgroundBrush
            if (_backgroundBrush is CanvasSolidColorBrush backBrush)
            {
                backBrush.Color = backgroundColor;
            }
            else
            {
                _backgroundBrush = new CanvasSolidColorBrush(_generator.Device, backgroundColor);
            }

            // Redraw the GeometrySurface
            RedrawSurface();
        }

        /// <inheritdoc/>
        public void Redraw(Size size, CanvasGeometry geometry)
        {
            // Resize the GeometrySurface
            _generator.ResizeDrawingSurface(_surfaceLock, _surface, size);

            // Set the size
            Size = _surface?.Size ?? new Size(0, 0);

            // Set the new geometry
            _geometry = geometry;

            // Redraw the GeometrySurface
            RedrawSurface();
        }

        /// <inheritdoc/>
        public void Redraw(Size size, CanvasGeometry geometry, ICanvasStroke stroke)
        {
            // Resize the GeometrySurface
            _generator.ResizeDrawingSurface(_surfaceLock, _surface, size);

            // Set the size
            Size = _surface?.Size ?? new Size(0, 0);

            // Set the new geometry
            _geometry = geometry;

            // Set the new stroke
            _stroke = stroke;

            // Redraw the GeometrySurface
            RedrawSurface();
        }

        /// <inheritdoc/>
        public void Redraw(Size size, CanvasGeometry geometry, Color fillColor)
        {
            // Resize the GeometrySurface
            _generator.ResizeDrawingSurface(_surfaceLock, _surface, size);

            // Set the size
            Size = _surface?.Size ?? new Size(0, 0);

            // Set the new geometry
            _geometry = geometry;

            // Set the fill
            if (_fill is CanvasSolidColorBrush fillBrush)
            {
                fillBrush.Color = fillColor;
            }
            else
            {
                _fill = new CanvasSolidColorBrush(_generator.Device, fillColor);
            }

            // Redraw the GeometrySurface
            RedrawSurface();
        }

        /// <inheritdoc/>
        public void Redraw(Size size, CanvasGeometry geometry, ICanvasStroke stroke, Color fillColor)
        {
            // Resize the GeometrySurface
            _generator.ResizeDrawingSurface(_surfaceLock, _surface, size);

            // Set the size
            Size = _surface?.Size ?? new Size(0, 0);

            // Set the new geometry
            _geometry = geometry;

            // Set the new stroke
            _stroke = stroke;

            // Set the fill
            if (_fill is CanvasSolidColorBrush fillBrush)
            {
                fillBrush.Color = fillColor;
            }
            else
            {
                _fill = new CanvasSolidColorBrush(_generator.Device, fillColor);
            }

            // Redraw the GeometrySurface
            RedrawSurface();
        }

        /// <inheritdoc/>
        public void Redraw(Size size, CanvasGeometry geometry, Color fillColor, Color backgroundColor)
        {
            // Resize the GeometrySurface
            _generator.ResizeDrawingSurface(_surfaceLock, _surface, size);

            // Set the size
            Size = _surface?.Size ?? new Size(0, 0);

            // Set the new geometry
            _geometry = geometry;

            // Set the fill
            if (_fill is CanvasSolidColorBrush fillBrush)
            {
                fillBrush.Color = fillColor;
            }
            else
            {
                _fill = new CanvasSolidColorBrush(_generator.Device, fillColor);
            }

            // Set the backgroundBrush
            if (_backgroundBrush is CanvasSolidColorBrush backBrush)
            {
                backBrush.Color = backgroundColor;
            }
            else
            {
                _backgroundBrush = new CanvasSolidColorBrush(_generator.Device, backgroundColor);
            }

            // Redraw the GeometrySurface
            RedrawSurface();
        }

        /// <inheritdoc/>
        public void Redraw(Size size, CanvasGeometry geometry, ICanvasStroke stroke, Color fillColor, Color backgroundColor)
        {
            // Resize the GeometrySurface
            _generator.ResizeDrawingSurface(_surfaceLock, _surface, size);

            // Set the size
            Size = _surface?.Size ?? new Size(0, 0);

            // Set the new geometry
            _geometry = geometry;

            // Set the new stroke
            _stroke = stroke;

            // Set the fill
            if (_fill is CanvasSolidColorBrush fillBrush)
            {
                fillBrush.Color = fillColor;
            }
            else
            {
                _fill = new CanvasSolidColorBrush(_generator.Device, fillColor);
            }

            // Set the backgroundBrush
            if (_backgroundBrush is CanvasSolidColorBrush backBrush)
            {
                backBrush.Color = backgroundColor;
            }
            else
            {
                _backgroundBrush = new CanvasSolidColorBrush(_generator.Device, backgroundColor);
            }

            // Redraw the GeometrySurface
            RedrawSurface();
        }

        /// <inheritdoc/>
        public void Redraw(Size size, CanvasGeometry geometry, ICanvasBrush fillBrush)
        {
            // Resize the GeometrySurface
            _generator.ResizeDrawingSurface(_surfaceLock, _surface, size);

            // Set the size
            Size = _surface?.Size ?? new Size(0, 0);

            // Set the new geometry
            _geometry = geometry;

            // Set the fill
            _fill = fillBrush ?? new CanvasSolidColorBrush(_generator.Device, Colors.Transparent);

            // Redraw the GeometrySurface
            RedrawSurface();
        }

        /// <inheritdoc/>
        public void Redraw(Size size, CanvasGeometry geometry, ICanvasBrush fillBrush, ICanvasBrush backgroundBrush)
        {
            _generator.ResizeDrawingSurface(_surfaceLock, _surface, size);

            // Set the size
            Size = _surface?.Size ?? new Size(0, 0);

            // Set the new geometry
            _geometry = geometry;

            // Set the fill
            _fill = fillBrush ?? new CanvasSolidColorBrush(_generator.Device, Colors.Transparent);

            // Set the backgroundBrush
            _backgroundBrush = backgroundBrush ?? new CanvasSolidColorBrush(_generator.Device, Colors.Transparent);

            // Redraw the GeometrySurface
            RedrawSurface();
        }

        /// <inheritdoc/>
        public void Redraw(Size size, CanvasGeometry geometry, ICanvasStroke stroke, ICanvasBrush fillBrush, ICanvasBrush backgroundBrush)
        {
            // Resize the GeometrySurface
            _generator.ResizeDrawingSurface(_surfaceLock, _surface, size);

            // Set the size
            Size = _surface?.Size ?? new Size(0, 0);

            // Set the new geometry
            _geometry = geometry;

            // Set the new stroke
            _stroke = stroke;

            // Set the fill
            _fill = fillBrush ?? new CanvasSolidColorBrush(_generator.Device, Colors.Transparent);

            // Set the backgroundBrush
            _backgroundBrush = backgroundBrush ?? new CanvasSolidColorBrush(_generator.Device, Colors.Transparent);

            // Redraw the GeometrySurface
            RedrawSurface();
        }

        /// <inheritdoc/>
        public void Redraw(Size size, CanvasGeometry geometry, ICanvasBrush fillBrush, Color backgroundColor)
        {
            _generator.ResizeDrawingSurface(_surfaceLock, _surface, size);

            // Set the size
            Size = _surface?.Size ?? new Size(0, 0);

            // Set the new geometry
            _geometry = geometry;

            // Set the fill
            _fill = fillBrush ?? new CanvasSolidColorBrush(_generator.Device, Colors.Transparent);

            // Set the backgroundBrush
            if (_backgroundBrush is CanvasSolidColorBrush backBrush)
            {
                backBrush.Color = backgroundColor;
            }
            else
            {
                _backgroundBrush = new CanvasSolidColorBrush(_generator.Device, backgroundColor);
            }

            // Redraw the GeometrySurface
            RedrawSurface();
        }

        /// <inheritdoc/>
        public void Redraw(Size size, CanvasGeometry geometry, ICanvasStroke stroke, ICanvasBrush fillBrush, Color backgroundColor)
        {
            // Resize the GeometrySurface
            _generator.ResizeDrawingSurface(_surfaceLock, _surface, size);

            // Set the size
            Size = _surface?.Size ?? new Size(0, 0);

            // Set the new geometry
            _geometry = geometry;

            // Set the new stroke
            _stroke = stroke;

            // Set the fill
            _fill = fillBrush ?? new CanvasSolidColorBrush(_generator.Device, Colors.Transparent);

            // Set the backgroundBrush
            if (_backgroundBrush is CanvasSolidColorBrush backBrush)
            {
                backBrush.Color = backgroundColor;
            }
            else
            {
                _backgroundBrush = new CanvasSolidColorBrush(_generator.Device, backgroundColor);
            }

            // Redraw the GeometrySurface
            RedrawSurface();
        }

        /// <inheritdoc/>
        public void Redraw(Size size, CanvasGeometry geometry, Color fillColor, ICanvasBrush backgroundBrush)
        {
            _generator.ResizeDrawingSurface(_surfaceLock, _surface, size);

            // Set the size
            Size = _surface?.Size ?? new Size(0, 0);

            // Set the new geometry
            _geometry = geometry;

            // Set the fill
            if (_fill is CanvasSolidColorBrush fillBrush)
            {
                fillBrush.Color = fillColor;
            }
            else
            {
                _fill = new CanvasSolidColorBrush(_generator.Device, fillColor);
            }

            // Set the backgroundBrush
            _backgroundBrush = backgroundBrush ?? new CanvasSolidColorBrush(_generator.Device, Colors.Transparent);

            // Redraw the GeometrySurface
            RedrawSurface();
        }

        /// <inheritdoc/>
        public void Redraw(Size size, CanvasGeometry geometry, ICanvasStroke stroke, Color fillColor, ICanvasBrush backgroundBrush)
        {
            // Resize the GeometrySurface
            _generator.ResizeDrawingSurface(_surfaceLock, _surface, size);

            // Set the size
            Size = _surface?.Size ?? new Size(0, 0);

            // Set the new geometry
            _geometry = geometry;

            // Set the new stroke
            _stroke = stroke;

            // Set the fill
            if (_fill is CanvasSolidColorBrush fillBrush)
            {
                fillBrush.Color = fillColor;
            }
            else
            {
                _fill = new CanvasSolidColorBrush(_generator.Device, fillColor);
            }

            // Set the backgroundBrush
            _backgroundBrush = backgroundBrush ?? new CanvasSolidColorBrush(_generator.Device, Colors.Transparent);

            // Redraw the GeometrySurface
            RedrawSurface();
        }

        /// <inheritdoc/>
        public void Resize(Size size)
        {
            // resize the GeometrySurface
            _generator.ResizeDrawingSurface(_surfaceLock, _surface, size);

            // Set the size
            Size = _surface?.Size ?? new Size(0, 0);

            // Redraw the GeometrySurface
            RedrawSurface();
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            _surface?.Dispose();
            _geometry?.Dispose();
            if (_generator != null)
            {
                _generator.DeviceReplaced -= OnDeviceReplaced;
            }

            _stroke.Brush.Dispose();
            _fill.Dispose();
            _backgroundBrush.Dispose();

            _stroke = null;
            _fill = null;
            _backgroundBrush = null;
            _surface = null;
            _generator = null;
            _geometry = null;
        }

        /// <summary>
        /// Handles the DeviceReplaced event.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">object.</param>
        private void OnDeviceReplaced(object sender, object e)
        {
            // Recreate the GeometrySurface
            _surface = _generator.CreateDrawingSurface(_surfaceLock, Size);

            // Redraw the GeometrySurface
            RedrawSurface();
        }

        /// <summary>
        /// Helper class to redraw the surface.
        /// </summary>
        private void RedrawSurface()
        {
            _generator.RedrawGeometrySurface(_surfaceLock, _surface, Size, _geometry, _stroke, _fill, _backgroundBrush);
        }
    }
}
