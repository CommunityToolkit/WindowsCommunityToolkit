// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#pragma warning disable CS0419 // Ambiguous reference in cref attribute

using Microsoft.Graphics.Canvas.Brushes;
using Microsoft.Graphics.Canvas.Geometry;
using Microsoft.Toolkit.Uwp.UI.Media.Geometry;
using Windows.Foundation;
using Windows.UI;

namespace Microsoft.Toolkit.Uwp.UI.Media
{
    /// <summary>
    /// Interface for rendering custom shaped geometries onto ICompositionSurface.
    /// </summary>
    public interface IGeometrySurface : IRenderSurface
    {
        /// <summary>
        /// Gets the Surface <see cref="CanvasGeometry"/>.
        /// </summary>
        CanvasGeometry Geometry { get; }

        /// <summary>
        /// Gets the Stroke with which the Geometry is outlined.
        /// </summary>
        ICanvasStroke Stroke { get; }

        /// <summary>
        /// Gets the Brush with which the Geometry is filled.
        /// </summary>
        ICanvasBrush Fill { get; }

        /// <summary>
        /// Gets the Brush with which the <see cref="IGeometrySurface"/> background is filled.
        /// </summary>
        ICanvasBrush BackgroundBrush { get; }

        /// <summary>
        /// Redraws the <see cref="IGeometrySurface"/> with the new geometry
        /// </summary>
        /// <param name="geometry">New <see cref="CanvasGeometry"/> to be applied to the <see cref="IGeometrySurface"/>.</param>
        void Redraw(CanvasGeometry geometry);

        /// <summary>
        /// Redraws the <see cref="IGeometrySurface"/> by outlining the existing geometry with the given <see cref="ICanvasStroke"/>.
        /// </summary>
        /// <param name="stroke"><see cref="ICanvasStroke"/> defining the outline for the geometry.</param>
        void Redraw(ICanvasStroke stroke);

        /// <summary>
        /// Redraws the <see cref="IGeometrySurface"/> by filling the existing geometry with the fill color.
        /// </summary>
        /// <param name="fillColor"><see cref="Color"/> with which the geometry is to be filled.</param>
        void Redraw(Color fillColor);

        /// <summary>
        /// Redraws the <see cref="IGeometrySurface"/> by filling the existing geometry with the given fill color and outlining it with the given <see cref="ICanvasStroke"/>.
        /// </summary>
        /// <param name="stroke">The <see cref="ICanvasStroke"/> defining the outline for the geometry.</param>
        /// <param name="fillColor"><see cref="Color"/> with which the geometry is to be filled.</param>
        void Redraw(ICanvasStroke stroke, Color fillColor);

        /// <summary>
        /// Redraws the <see cref="IGeometrySurface"/> by filling the existing geometry with the fill color and the background with the background color.
        /// </summary>
        /// <param name="fillColor"><see cref="Color"/> with which the geometry is to be filled.</param>
        /// <param name="backgroundColor"><see cref="Color"/> with which the <see cref="IGeometrySurface"/> background is to be filled.</param>
        void Redraw(Color fillColor, Color backgroundColor);

        /// <summary>
        /// Redraws the <see cref="IGeometrySurface"/> by outlining the existing geometry with the given <see cref="ICanvasStroke"/>, filling it with the fill color and the background with the background color.
        /// </summary>
        /// <param name="stroke">The <see cref="ICanvasStroke"/> defining the outline for the geometry.</param>
        /// <param name="fillColor"><see cref="Color"/> with which the geometry is to be filled.</param>
        /// <param name="backgroundColor"><see cref="Color"/> with which the <see cref="IGeometrySurface"/> background is to be filled.</param>
        void Redraw(ICanvasStroke stroke, Color fillColor, Color backgroundColor);

        /// <summary>
        /// Redraws the <see cref="IGeometrySurface"/> by filling the existing geometry with the fill brush.
        /// </summary>
        /// <param name="fillBrush">Brush with which the geometry is to be filled.</param>
        void Redraw(ICanvasBrush fillBrush);

        /// <summary>
        /// Redraws the <see cref="IGeometrySurface"/> by outlining the existing geometry with the given <see cref="ICanvasStroke"/>, filling it with the fill brush.
        /// </summary>
        /// <param name="stroke">The <see cref="ICanvasStroke"/> defining the outline for the geometry.</param>
        /// <param name="fillBrush">Brush with which the geometry is to be filled.</param>
        void Redraw(ICanvasStroke stroke, ICanvasBrush fillBrush);

        /// <summary>
        /// Redraws the <see cref="IGeometrySurface"/> by filling the existing geometry with the fill brush and the background with the background brush.
        /// </summary>
        /// <param name="fillBrush">Brush with which the geometry is to be filled.</param>
        /// <param name="backgroundBrush">Brush with which the <see cref="IGeometrySurface"/> background is to be filled.</param>
        void Redraw(ICanvasBrush fillBrush, ICanvasBrush backgroundBrush);

        /// <summary>
        /// Redraws the <see cref="IGeometrySurface"/> by outlining the existing geometry with the given <see cref="ICanvasStroke"/>, filling it with the fill brush and the background with the background brush.
        /// </summary>
        /// <param name="stroke">The <see cref="ICanvasStroke"/> defining the outline for the geometry.</param>
        /// <param name="fillBrush">Brush with which the geometry is to be filled.</param>
        /// <param name="backgroundBrush">Brush with which the <see cref="IGeometrySurface"/> background is to be filled.</param>
        void Redraw(ICanvasStroke stroke, ICanvasBrush fillBrush, ICanvasBrush backgroundBrush);

        /// <summary>
        /// Redraws the <see cref="IGeometrySurface"/> by filling the existing geometry with the fill color and the background with the background brush.
        /// </summary>
        /// <param name="fillColor"><see cref="Color"/> with which the geometry is to be filled.</param>
        /// <param name="backgroundBrush">Brush with which the <see cref="IGeometrySurface"/> background is to be filled.</param>
        void Redraw(Color fillColor, ICanvasBrush backgroundBrush);

        /// <summary>
        /// Redraws the <see cref="IGeometrySurface"/> by outlining the existing geometry with the given <see cref="ICanvasStroke"/>, filling it with the fill color and the background with the background brush.
        /// </summary>
        /// <param name="stroke">The <see cref="ICanvasStroke"/> defining the outline for the geometry.</param>
        /// <param name="fillColor"><see cref="Color"/> with which the geometry is to be filled.</param>
        /// <param name="backgroundBrush">Brush with which the <see cref="IGeometrySurface"/> background is to be filled.</param>
        void Redraw(ICanvasStroke stroke, Color fillColor, ICanvasBrush backgroundBrush);

        /// <summary>
        /// Redraws the <see cref="IGeometrySurface"/> by filling the existing geometry with the fill brush and the background with the background color.
        /// </summary>
        /// <param name="fillBrush">Brush with which the geometry is to be filled.</param>
        /// <param name="backgroundColor"><see cref="Color"/> with which the <see cref="IGeometrySurface"/> background is to be filled.</param>
        void Redraw(ICanvasBrush fillBrush, Color backgroundColor);

        /// <summary>
        /// Redraws the <see cref="IGeometrySurface"/> by outlining the existing geometry with the given <see cref="ICanvasStroke"/>, filling it with the fill brush and the background with the background color.
        /// </summary>
        /// <param name="stroke">The <see cref="ICanvasStroke"/> defining the outline for the geometry.</param>
        /// <param name="fillBrush">Brush with which the geometry is to be filled.</param>
        /// <param name="backgroundColor"><see cref="Color"/> with which the <see cref="IGeometrySurface"/> background is to be filled.</param>
        void Redraw(ICanvasStroke stroke, ICanvasBrush fillBrush, Color backgroundColor);

        /// <summary>
        /// Resizes the <see cref="IGeometrySurface"/> with the given size and redraws the <see cref="IGeometrySurface"/> with the new geometry.
        /// </summary>
        /// <param name="size">New size of the <see cref="IGeometrySurface"/>.</param>
        /// <param name="geometry">New <see cref="CanvasGeometry"/> to be applied to the <see cref="IGeometrySurface"/>.</param>
        void Redraw(Size size, CanvasGeometry geometry);

        /// <summary>
        /// Resizes the <see cref="IGeometrySurface"/> with the given size and redraws the <see cref="IGeometrySurface"/> with the new geometry and outlines it with the given <see cref="ICanvasStroke"/>.
        /// </summary>
        /// <param name="size">New size of the <see cref="IGeometrySurface"/>.</param>
        /// <param name="geometry">New <see cref="CanvasGeometry"/> to be applied to the <see cref="IGeometrySurface"/>.</param>
        /// <param name="stroke">The <see cref="ICanvasStroke"/> defining the outline for the geometry.</param>
        void Redraw(Size size, CanvasGeometry geometry, ICanvasStroke stroke);

        /// <summary>
        /// Resizes the <see cref="IGeometrySurface"/> with the given size and redraws the <see cref="IGeometrySurface"/> with the new geometry and fills it with the fill color.
        /// </summary>
        /// <param name="size">New size of the <see cref="IGeometrySurface"/>.</param>
        /// <param name="geometry">New <see cref="CanvasGeometry"/> to be applied to the <see cref="IGeometrySurface"/>.</param>
        /// <param name="fillColor">Fill color for the geometry.</param>
        void Redraw(Size size, CanvasGeometry geometry, Color fillColor);

        /// <summary>
        /// Resizes the <see cref="IGeometrySurface"/> with the given size and redraws the <see cref="IGeometrySurface"/> with the new geometry, outlines it with the given <see cref="ICanvasStroke"/> and fills it with the fill color.
        /// </summary>
        /// <param name="size">New size of the <see cref="IGeometrySurface"/>.</param>
        /// <param name="geometry">New <see cref="CanvasGeometry"/> to be applied to the <see cref="IGeometrySurface"/>.</param>
        /// <param name="stroke">The <see cref="ICanvasStroke"/> defining the outline for the geometry.</param>
        /// <param name="fillColor">Fill color for the geometry.</param>
        void Redraw(Size size, CanvasGeometry geometry, ICanvasStroke stroke, Color fillColor);

        /// <summary>
        /// Resizes the <see cref="IGeometrySurface"/> with the given size and redraws the <see cref="IGeometrySurface"/> with the new geometry and fills it with the fill color and fills the background with the background color.
        /// </summary>
        /// <param name="size">New size of the <see cref="IGeometrySurface"/>.</param>
        /// <param name="geometry">New <see cref="CanvasGeometry"/> to be applied to the <see cref="IGeometrySurface"/>.</param>
        /// <param name="fillColor">Fill color for the geometry.</param>
        /// <param name="backgroundColor">Fill color for the <see cref="IGeometrySurface"/> background.</param>
        void Redraw(Size size, CanvasGeometry geometry, Color fillColor, Color backgroundColor);

        /// <summary>
        /// Resizes the <see cref="IGeometrySurface"/> with the given size and redraws the <see cref="IGeometrySurface"/> with the new geometry, outlines it with the given <see cref="ICanvasStroke"/> and
        /// fills it with the fill color and fills the background with the background color.
        /// </summary>
        /// <param name="size">New size of the <see cref="IGeometrySurface"/>.</param>
        /// <param name="geometry">New <see cref="CanvasGeometry"/> to be applied to the <see cref="IGeometrySurface"/>.</param>
        /// <param name="stroke">The <see cref="ICanvasStroke"/> defining the outline for the geometry.</param>
        /// <param name="fillColor">Fill color for the geometry.</param>
        /// <param name="backgroundColor">Fill color for the <see cref="IGeometrySurface"/> background.</param>
        void Redraw(Size size, CanvasGeometry geometry, ICanvasStroke stroke, Color fillColor, Color backgroundColor);

        /// <summary>
        /// Resizes the <see cref="IGeometrySurface"/> with the given size and redraws the <see cref="IGeometrySurface"/> with the new geometry and fills it with the fill brush.
        /// </summary>
        /// <param name="size">New size of the <see cref="IGeometrySurface"/>.</param>
        /// <param name="geometry">New <see cref="CanvasGeometry"/> to be applied to the <see cref="IGeometrySurface"/>.</param>
        /// <param name="fillBrush">Brush to fill the geometry.</param>
        void Redraw(Size size, CanvasGeometry geometry, ICanvasBrush fillBrush);

        /// <summary>
        /// Resizes the <see cref="IGeometrySurface"/> with the given size and redraws the <see cref="IGeometrySurface"/> with the new geometry and fills it with the fill brush and fills the background with the background brush.
        /// </summary>
        /// <param name="size">New size of the <see cref="IGeometrySurface"/>.</param>
        /// <param name="geometry">New <see cref="CanvasGeometry"/> to be applied to the <see cref="IGeometrySurface"/>.</param>
        /// <param name="fillBrush">Brush to fill the geometry.</param>
        /// <param name="backgroundBrush">Brush to fill the <see cref="IGeometrySurface"/> background.</param>
        void Redraw(Size size, CanvasGeometry geometry, ICanvasBrush fillBrush, ICanvasBrush backgroundBrush);

        /// <summary>
        /// Resizes the <see cref="IGeometrySurface"/> with the given size and redraws the <see cref="IGeometrySurface"/> with the new geometry, outlines it with the given <see cref="ICanvasStroke"/> and
        /// fills it with the fill brush and fills the background with the background brush.
        /// </summary>
        /// <param name="size">New size of the <see cref="IGeometrySurface"/>.</param>
        /// <param name="geometry">New <see cref="CanvasGeometry"/> to be applied to the <see cref="IGeometrySurface"/>.</param>
        /// <param name="stroke">The <see cref="ICanvasStroke"/> defining the outline for the geometry.</param>
        /// <param name="fillBrush">Brush to fill the geometry.</param>
        /// <param name="backgroundBrush">Brush to fill the <see cref="IGeometrySurface"/> background.</param>
        void Redraw(Size size, CanvasGeometry geometry, ICanvasStroke stroke, ICanvasBrush fillBrush, ICanvasBrush backgroundBrush);

        /// <summary>
        /// Resizes the <see cref="IGeometrySurface"/> with the given size and redraws the <see cref="IGeometrySurface"/> with the new geometry and fills it with the fill brush and the background with the background color.
        /// </summary>
        /// <param name="size">New size of the <see cref="IGeometrySurface"/>.</param>
        /// <param name="geometry">New <see cref="CanvasGeometry"/> to be applied to the <see cref="IGeometrySurface"/>.</param>
        /// <param name="fillBrush">Brush to fill the geometry.</param>
        /// <param name="backgroundColor">Fill color for the <see cref="IGeometrySurface"/> background.</param>
        void Redraw(Size size, CanvasGeometry geometry, ICanvasBrush fillBrush, Color backgroundColor);

        /// <summary>
        /// Resizes the <see cref="IGeometrySurface"/> with the given size and redraws the <see cref="IGeometrySurface"/> with the new geometry, outlines it with the given <see cref="ICanvasStroke"/> and
        /// fills it with the fill brush and the background with the background color.
        /// </summary>
        /// <param name="size">New size of the <see cref="IGeometrySurface"/>.</param>
        /// <param name="geometry">New <see cref="CanvasGeometry"/> to be applied to the <see cref="IGeometrySurface"/>.</param>
        /// <param name="stroke">The <see cref="ICanvasStroke"/> defining the outline for the geometry.</param>
        /// <param name="fillBrush">Brush to fill the geometry.</param>
        /// <param name="backgroundColor">Fill color for the <see cref="IGeometrySurface"/> background.</param>
        void Redraw(Size size, CanvasGeometry geometry, ICanvasStroke stroke, ICanvasBrush fillBrush, Color backgroundColor);

        /// <summary>
        /// Resizes the <see cref="IGeometrySurface"/> with the given size and redraws the <see cref="IGeometrySurface"/> with the new geometry and fills it with the fill color and the background with the background brush.
        /// </summary>
        /// <param name="size">New size of the <see cref="IGeometrySurface"/>.</param>
        /// <param name="geometry">New <see cref="CanvasGeometry"/> to be applied to the <see cref="IGeometrySurface"/>.</param>
        /// <param name="fillColor">Fill color for the geometry.</param>
        /// <param name="backgroundBrush">Brush to fill the <see cref="IGeometrySurface"/> background.</param>
        void Redraw(Size size, CanvasGeometry geometry, Color fillColor, ICanvasBrush backgroundBrush);

        /// <summary>
        /// Resizes the <see cref="IGeometrySurface"/> with the given size and redraws the <see cref="IGeometrySurface"/> with the new geometry, outlines it with the given <see cref="ICanvasStroke"/> and
        /// fills it with the fill color and the background with the background brush.
        /// </summary>
        /// <param name="size">New size of the <see cref="IGeometrySurface"/>.</param>
        /// <param name="geometry">New <see cref="CanvasGeometry"/> to be applied to the <see cref="IGeometrySurface"/>.</param>
        /// <param name="stroke">The <see cref="ICanvasStroke"/> defining the outline for the geometry.</param>
        /// <param name="fillColor">Fill color for the geometry.</param>
        /// <param name="backgroundBrush">Brush to fill the <see cref="IGeometrySurface"/> background.</param>
        void Redraw(Size size, CanvasGeometry geometry, ICanvasStroke stroke, Color fillColor, ICanvasBrush backgroundBrush);
    }
}

#pragma warning restore CS0419 // Ambiguous reference in cref attribute
