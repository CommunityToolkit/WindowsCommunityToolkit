// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Numerics;
using Microsoft.Graphics.Canvas.Geometry;
using Windows.Foundation;
using Windows.UI.Composition;

namespace Microsoft.Toolkit.Uwp.UI.Media
{
    /// <summary>
    /// Interface for rendering custom shaped geometries onto <see cref="ICompositionSurface"/> so that they can be used as masks on Composition Visuals.
    /// </summary>
    public interface IGeometryMaskSurface : IRenderSurface
    {
        /// <summary>
        /// Gets the <see cref="CanvasGeometry"/> of the <see cref="IGeometryMaskSurface"/>.
        /// </summary>
        CanvasGeometry Geometry { get; }

        /// <summary>
        /// Gets the offset from the top left corner of the <see cref="ICompositionSurface"/> where the <see cref="Geometry"/> is rendered.
        /// </summary>
        Vector2 Offset { get; }

        /// <summary>
        /// Redraws the <see cref="IGeometryMaskSurface"/> with the new geometry
        /// </summary>
        /// <param name="geometry">New <see cref="CanvasGeometry"/> to be applied to the <see cref="IGeometryMaskSurface"/>.</param>
        void Redraw(CanvasGeometry geometry);

        /// <summary>
        /// Resizes the <see cref="IGeometryMaskSurface"/> with the given size and redraws the <see cref="IGeometryMaskSurface"/> with the new geometry and fills it with White color.
        /// </summary>
        /// <param name="size">New size of the mask.</param>
        /// <param name="geometry">New <see cref="CanvasGeometry"/> to be applied to the <see cref="IGeometryMaskSurface"/>.</param>
        void Redraw(Size size, CanvasGeometry geometry);

        /// <summary>
        /// Redraws the <see cref="IGeometryMaskSurface"/> with the new geometry.
        /// </summary>
        /// <param name="geometry">New <see cref="CanvasGeometry"/> to be applied to the <see cref="IGeometryMaskSurface"/>.</param>
        /// <param name="offset">The offset from the top left corner of the <see cref="ICompositionSurface"/> where the <see cref="Geometry"/> is rendered.</param>
        void Redraw(CanvasGeometry geometry, Vector2 offset);

        /// <summary>
        /// Resizes the <see cref="IGeometryMaskSurface"/> with the given size and redraws the <see cref="IGeometryMaskSurface"/> with the new geometry and fills it with White color.
        /// </summary>
        /// <param name="size">New size of the mask.</param>
        /// <param name="geometry">New <see cref="CanvasGeometry"/> to be applied to the <see cref="IGeometryMaskSurface"/>.</param>
        /// <param name="offset">The offset from the top left corner of the <see cref="ICompositionSurface"/> where the <see cref="Geometry"/> is rendered.</param>
        void Redraw(Size size, CanvasGeometry geometry, Vector2 offset);
    }
}
