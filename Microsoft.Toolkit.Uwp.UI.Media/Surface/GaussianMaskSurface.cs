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
    /// Class for rendering custom shaped geometries onto ICompositionSurface
    /// so that they can be used as masks on Composition Visuals. These geometries
    /// have a Gaussian Blur applied to them.
    /// </summary>
    internal sealed class GaussianMaskSurface : IGaussianMaskSurface
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

        /// <inheritdoc/>
        public float BlurRadius { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GaussianMaskSurface"/> class.
        /// Constructor
        /// </summary>
        /// <param name="generator">IComposiitonGeneratorInternal object</param>
        /// <param name="size">Size of the GaussianMaskSurface</param>
        /// <param name="geometry">Geometry of the GaussianMaskSurface</param>
        /// <param name="offset">The offset from the top left corner of the ICompositionSurface where
        /// the Geometry is rendered.</param>
        /// <param name="blurRadius">Radius of Gaussian Blur to be applied on the GaussianMaskSurface</param>
        public GaussianMaskSurface(ICompositionGeneratorInternal generator, Size size, CanvasGeometry geometry, Vector2 offset, float blurRadius)
        {
            _generator = generator ?? throw new ArgumentException("Generator cannot be null!", nameof(generator));

            _surfaceLock = new object();
            Geometry = geometry;
            Offset = offset;
            BlurRadius = Math.Abs(blurRadius);

            // Create Mask Surface
            _surface = _generator.CreateDrawingSurface(_surfaceLock, size);

            // Set the size
            Size = _surface?.Size ?? new Size(0, 0);

            // Subscribe to DeviceReplaced event
            _generator.DeviceReplaced += OnDeviceReplaced;
        }

        /// <inheritdoc/>
        public void Redraw()
        {
            // Redraw the mask surface
            RedrawSurface();
        }

        /// <inheritdoc/>
        public void Redraw(float blurRadius)
        {
            Redraw(Size, Geometry, Offset, blurRadius);
        }

        /// <inheritdoc/>
        public void Redraw(CanvasGeometry geometry)
        {
            Redraw(Size, geometry, Offset, BlurRadius);
        }

        /// <inheritdoc/>
        public void Redraw(CanvasGeometry geometry, Vector2 offset)
        {
            Redraw(Size, geometry, offset, BlurRadius);
        }

        /// <inheritdoc/>
        public void Redraw(CanvasGeometry geometry, Vector2 offset, float blurRadius)
        {
            Redraw(Size, geometry, offset, blurRadius);
        }

        /// <inheritdoc/>
        public void Redraw(Size size, CanvasGeometry geometry)
        {
            Redraw(size, geometry, Offset, BlurRadius);
        }

        /// <inheritdoc/>
        public void Redraw(Size size, CanvasGeometry geometry, Vector2 offset)
        {
            Redraw(size, geometry, offset, BlurRadius);
        }

        /// <inheritdoc/>
        public void Redraw(Size size, CanvasGeometry geometry, Vector2 offset, float blurRadius)
        {
            // Resize the mask surface
            _generator.ResizeDrawingSurface(_surfaceLock, _surface, size);

            // Set the size
            Size = _surface?.Size ?? new Size(0, 0);

            // Set the offset
            Offset = offset;

            // Set the new geometry
            Geometry = geometry;

            // Set the new blur radius
            BlurRadius = blurRadius;

            // Redraw the mask surface
            RedrawSurface();
        }

        /// <inheritdoc/>
        public void Resize(Size size)
        {
            // resize the mask surface
            _generator.ResizeDrawingSurface(_surfaceLock, _surface, size);

            // Set the size
            Size = _surface?.Size ?? new Size(0, 0);

            // Redraw the mask surface
            RedrawSurface();
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            _surface?.Dispose();
            Geometry?.Dispose();
            if (_generator != null)
            {
                _generator.DeviceReplaced -= OnDeviceReplaced;
            }

            _surface = null;
            _generator = null;
            Geometry = null;
        }

        /// <summary>
        /// Handles the DeviceReplaced event
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">object</param>
        private void OnDeviceReplaced(object sender, object e)
        {
            // Recreate the GaussianMaskSurface
            _surface = _generator.CreateDrawingSurface(_surfaceLock, Size);

            // Redraw the mask surface
            RedrawSurface();
        }

        /// <summary>
        /// Helper class to redraw the GaussianMaskSurface
        /// </summary>
        private void RedrawSurface()
        {
            _generator.RedrawGaussianMaskSurface(_surfaceLock, _surface, Size, Geometry, Offset, BlurRadius);
        }
    }
}