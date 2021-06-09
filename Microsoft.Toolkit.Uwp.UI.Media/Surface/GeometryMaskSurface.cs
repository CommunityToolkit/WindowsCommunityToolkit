// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Numerics;
using Microsoft.Graphics.Canvas.Geometry;
using Windows.Foundation;
using Windows.UI.Composition;

namespace Microsoft.Toolkit.Uwp.UI.Media
{
    /// <summary>
    /// Class for rendering custom shaped geometries onto ICompositionSurface so that they can be used as masks on Composition Visuals.
    /// </summary>
    internal sealed class GeometryMaskSurface : IGeometryMaskSurface
    {
        private readonly object _surfaceLock;
        private ICompositionGeneratorInternal _generator;
        private CompositionDrawingSurface _surface;

        /// <inheritdoc/>
        public ICompositionGenerator Generator => _generator;

        /// <inheritdoc/>
        public ICompositionSurface Surface => _surface;

        /// <inheritdoc/>
        public CanvasGeometry Geometry { get; private set; }

        /// <inheritdoc/>
        public Size Size { get; private set; }

        /// <inheritdoc/>
        public Vector2 Offset { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeometryMaskSurface"/> class.
        /// </summary>
        /// <param name="generator">IComposiitonGeneratorInternal object</param>
        /// <param name="size">Size of the GeometryMaskSurface</param>
        /// <param name="geometry">Geometry of the GeometryMaskSurface</param>
        /// <param name="offset">The offset from the top left corner of the ICompositionSurface where the Geometry is rendered.</param>
        public GeometryMaskSurface(ICompositionGeneratorInternal generator, Size size, CanvasGeometry geometry, Vector2 offset)
        {
            _generator = generator ?? throw new ArgumentException("Generator cannot be null!", nameof(generator));

            _generator = generator;
            _surfaceLock = new object();
            Geometry = geometry;
            Offset = offset;

            // Create Mask Surface
            _surface = _generator.CreateDrawingSurface(_surfaceLock, size);

            // Set the size
            Size = _surface?.Size ?? new Size(0, 0);

            // Subscribe to DeviceReplaced event
            _generator.DeviceReplaced += OnDeviceReplaced;
        }

        /// <summary>
        /// Redraws the GeometryMaskSurface
        /// </summary>
        public void Redraw()
        {
            // Redraw the mask surface
            RedrawSurface();
        }

        /// <summary>
        /// Redraws the GeometryMaskSurface with the new geometry.
        /// </summary>
        /// <param name="geometry">New CanvasGeometry to be applied to the mask.</param>
        public void Redraw(CanvasGeometry geometry)
        {
            Redraw(Size, geometry, Offset);
        }

        /// <summary>
        /// Redraws the IGeometryMaskSurface with the new geometry
        /// </summary>
        /// <param name="geometry">New CanvasGeometry to be applied to the IGeometryMaskSurface</param>
        /// <param name="offset">The offset from the top left corner of the ICompositionSurface where the Geometry is rendered.</param>
        public void Redraw(CanvasGeometry geometry, Vector2 offset)
        {
            Redraw(Size, geometry, offset);
        }

        /// <summary>
        /// Resizes the GeometryMaskSurface with the given size and redraws the GeometryMaskSurface with the new geometry and fills it either with White color
        /// (if the MaskMode is True) or with the foreground brush (if the MaskMode is False).
        /// </summary>
        /// <param name="size">New size of the mask</param>
        /// <param name="geometry">New CanvasGeometry to be applied to the mask</param>
        public void Redraw(Size size, CanvasGeometry geometry)
        {
            Redraw(size, geometry, Offset);
        }

        /// <summary>
        /// Resizes the IGeometryMaskSurface with the given size and redraws the IGeometryMaskSurface with the new geometry and fills it with White color.
        /// </summary>
        /// <param name="size">New size of the mask</param>
        /// <param name="geometry">New CanvasGeometry to be applied to the IGeometryMaskSurface</param>
        /// <param name="offset">The offset from the top left corner of the ICompositionSurface where
        /// the Geometry is rendered.</param>
        public void Redraw(Size size, CanvasGeometry geometry, Vector2 offset)
        {
            if (Size != size)
            {
                // Resize the mask surface
                _generator.ResizeDrawingSurface(_surfaceLock, _surface, size);

                // Set the size
                Size = _surface?.Size ?? new Size(0, 0);
            }

            // Set the new geometry
            Geometry = geometry;

            // Set the offset
            Offset = offset;

            // Redraw the mask surface
            RedrawSurface();
        }

        /// <summary>
        /// Resizes the GeometryMaskSurface to the new size.
        /// </summary>
        /// <param name="size">New size of the mask</param>
        public void Resize(Size size)
        {
            // resize the mask surface
            _generator.ResizeDrawingSurface(_surfaceLock, _surface, size);

            // Set the size
            Size = _surface?.Size ?? new Size(0, 0);

            // Redraw the mask surface
            RedrawSurface();
        }

        /// <summary>
        /// Disposes the resources used by the GeometryMaskSurface.
        /// </summary>
        public void Dispose()
        {
            _surface?.Dispose();
            Geometry?.Dispose();
            Geometry = null;
            if (_generator != null)
            {
                _generator.DeviceReplaced -= OnDeviceReplaced;
            }

            _surface = null;
            _generator = null;
        }

        /// <summary>
        /// Handles the DeviceReplaced event.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">object</param>
        private void OnDeviceReplaced(object sender, object e)
        {
            // Recreate the GeometryMaskSurface
            _surface = _generator.CreateDrawingSurface(_surfaceLock, Size);

            // Redraw the mask surface
            RedrawSurface();
        }

        /// <summary>
        /// Helper class to redraw the GeometryMaskSurface.
        /// </summary>
        private void RedrawSurface()
        {
            _generator.RedrawGeometryMaskSurface(_surfaceLock, _surface, Size, Geometry, Offset);
        }
    }
}
