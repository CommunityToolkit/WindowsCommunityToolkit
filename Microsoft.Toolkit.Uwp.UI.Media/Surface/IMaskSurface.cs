// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Numerics;
using Microsoft.Graphics.Canvas.Geometry;
using Windows.Foundation;

namespace Microsoft.Toolkit.Uwp.UI.Media.Surface
{
    /// <summary>
    /// Interface for rendering custom shaped geometries onto ICompositionSurface
    /// so that they can be used as masks on Composition Visuals.
    /// </summary>
    public interface IMaskSurface : IRenderSurface
    {
        /// <summary>
        /// Gets the Geometry of the MaskSurface
        /// </summary>
        CanvasGeometry Geometry { get; }

        /// <summary>
        /// Gets the offset from the top left corner of the ICompositionSurface where
        /// the Geometry is rendered.
        /// </summary>
        Vector2 Offset { get; }

        /// <summary>
        /// Redraws the MaskSurface with the new geometry
        /// </summary>
        /// <param name="geometry">New CanvasGeometry to be applied to the IMaskSurface</param>
        void Redraw(CanvasGeometry geometry);

        /// <summary>
        /// Resizes the MaskSurface with the given size and redraws the IMaskSurface
        /// with the new geometry and fills it with White color
        /// </summary>
        /// <param name="size">New size of the mask</param>
        /// <param name="geometry">New CanvasGeometry to be applied to the IMaskSurface</param>
        void Redraw(Size size, CanvasGeometry geometry);

        /// <summary>
        /// Redraws the IMaskSurface with the new geometry
        /// </summary>
        /// <param name="geometry">New CanvasGeometry to be applied to the IMaskSurface</param>
        /// <param name="offset">The offset from the top left corner of the ICompositionSurface where
        /// the Geometry is rendered.</param>
        void Redraw(CanvasGeometry geometry, Vector2 offset);

        /// <summary>
        /// Resizes the IMaskSurface with the given size and redraws the IMaskSurface
        /// with the new geometry and fills it with White color
        /// </summary>
        /// <param name="size">New size of the mask</param>
        /// <param name="geometry">New CanvasGeometry to be applied to the IMaskSurface</param>
        /// <param name="offset">The offset from the top left corner of the ICompositionSurface where
        /// the Geometry is rendered.</param>
        void Redraw(Size size, CanvasGeometry geometry, Vector2 offset);
    }
}
