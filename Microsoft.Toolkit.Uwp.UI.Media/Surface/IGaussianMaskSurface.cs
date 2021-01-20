// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Numerics;
using Microsoft.Graphics.Canvas.Geometry;
using Windows.Foundation;

namespace Microsoft.Toolkit.Uwp.UI.Media.Surface
{
    /// <summary>
    /// Interface for rendering custom shaped geometries onto ICompositionSurface so that they can be used as masks on Composition Visuals.
    /// These geometries have a Gaussian Blur applied to them.
    /// </summary>
    public interface IGaussianMaskSurface : IMaskSurface
    {
        /// <summary>
        /// Gets radius of Gaussian Blur to be applied on the GaussianMaskSurface.
        /// </summary>
        float BlurRadius { get; }

        /// <summary>
        /// Applies the given blur radius to the IGaussianMaskSurface.
        /// </summary>
        /// <param name="blurRadius">Radius of Gaussian Blur to be applied on the GaussianMaskSurface.</param>
        void Redraw(float blurRadius);

        /// <summary>
        /// Redraws the IGaussianMaskSurface with the new geometry and fills it with White color after applying the Gaussian blur with given blur radius.
        /// </summary>
        /// <param name="geometry">New CanvasGeometry to be applied to the GaussianMaskSurface.</param>
        /// <param name="offset">The offset from the top left corner of the ICompositionSurface where the Geometry is rendered.</param>
        /// <param name="blurRadius">Radius of Gaussian Blur to be applied on the IGaussianMaskSurface.</param>
        void Redraw(CanvasGeometry geometry, Vector2 offset, float blurRadius);

        /// <summary>
        /// Resizes the IGaussianMaskSurface with the given size and redraws the IGaussianMaskSurface with the new geometry and fills it with White color.
        /// </summary>
        /// <param name="size">New size of the mask</param>
        /// <param name="geometry">New CanvasGeometry to be applied to the IGaussianMaskSurface.</param>
        /// <param name="offset">The offset from the top left corner of the ICompositionSurface where the Geometry is rendered.</param>
        /// <param name="blurRadius">Radius of Gaussian Blur to be applied on the IGaussianMaskSurface.</param>
        void Redraw(Size size, CanvasGeometry geometry, Vector2 offset, float blurRadius);
    }
}