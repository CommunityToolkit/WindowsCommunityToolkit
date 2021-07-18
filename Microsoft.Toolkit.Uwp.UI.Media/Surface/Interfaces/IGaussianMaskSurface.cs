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
    /// These geometries have a Gaussian Blur applied to them.
    /// </summary>
    public interface IGaussianMaskSurface : IGeometryMaskSurface
    {
        /// <summary>
        /// Gets radius of Gaussian Blur to be applied on the <see cref="IGaussianMaskSurface"/>.
        /// </summary>
        float BlurRadius { get; }

        /// <summary>
        /// Applies the given blur radius to the <see cref="IGaussianMaskSurface"/>.
        /// </summary>
        /// <param name="blurRadius">Radius of Gaussian Blur to be applied on the <see cref="IGaussianMaskSurface"/>.</param>
        void Redraw(float blurRadius);

        /// <summary>
        /// Redraws the <see cref="IGaussianMaskSurface"/> with the new geometry and fills it with White color after applying the Gaussian blur with given blur radius.
        /// </summary>
        /// <param name="geometry">New <see cref="CanvasGeometry"/> to be applied to the <see cref="IGaussianMaskSurface"/>.</param>
        /// <param name="offset">The offset from the top left corner of the <see cref="ICompositionSurface"/> where the <see cref="IGeometrySurface.Geometry"/> is rendered.</param>
        /// <param name="blurRadius">Radius of Gaussian Blur to be applied on the <see cref="IGaussianMaskSurface"/>.</param>
        void Redraw(CanvasGeometry geometry, Vector2 offset, float blurRadius);

        /// <summary>
        /// Resizes the <see cref="IGaussianMaskSurface"/> with the given size and redraws the <see cref="IGaussianMaskSurface"/> with the new geometry and fills it with White color.
        /// </summary>
        /// <param name="size">New size of the mask</param>
        /// <param name="geometry">New <see cref="CanvasGeometry"/> to be applied to the <see cref="IGaussianMaskSurface"/>.</param>
        /// <param name="offset">The offset from the top left corner of the <see cref="ICompositionSurface"/> where the <see cref="IGeometrySurface.Geometry"/> is rendered.</param>
        /// <param name="blurRadius">Radius of Gaussian Blur to be applied on the <see cref="IGaussianMaskSurface"/>.</param>
        void Redraw(Size size, CanvasGeometry geometry, Vector2 offset, float blurRadius);
    }
}