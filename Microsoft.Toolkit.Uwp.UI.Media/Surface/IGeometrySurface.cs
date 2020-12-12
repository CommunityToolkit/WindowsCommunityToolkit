// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Graphics.Canvas.Brushes;
using Microsoft.Graphics.Canvas.Geometry;
using Microsoft.Toolkit.Uwp.UI.Media.Geometry;
using Windows.Foundation;
using Windows.UI;

namespace Microsoft.Toolkit.Uwp.UI.Media.Surface
{
    /// <summary>
    /// Interface for rendering custom shaped geometries onto ICompositionSurface
    /// </summary>
    public interface IGeometrySurface : IRenderSurface
    {
        /// <summary>
        /// Gets the Surface Geometry
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
        /// Gets the Brush with which the IGeometrySurface background is filled.
        /// </summary>
        ICanvasBrush BackgroundBrush { get; }

        /// <summary>
        /// Redraws the IGeometrySurface with the new geometry
        /// </summary>
        /// <param name="geometry">New CanvasGeometry to be applied to the IGeometrySurface</param>
        void Redraw(CanvasGeometry geometry);

        /// <summary>
        /// Redraws the IGeometrySurface by outlining the existing geometry with
        /// the given ICanvasStroke
        /// </summary>
        /// <param name="stroke">ICanvasStroke defining the outline for the geometry</param>
        void Redraw(ICanvasStroke stroke);

        /// <summary>
        /// Redraws the IGeometrySurface by filling the existing geometry with
        /// the fill color.
        /// </summary>
        /// <param name="fillColor">Color with which the geometry is to be filled</param>
        void Redraw(Color fillColor);

        /// <summary>
        /// Redraws the IGeometrySurface by filling the existing geometry with
        /// the given fill color and outlining it with the given ICanvasStroke.
        /// </summary>
        /// <param name="stroke">ICanvasStroke defining the outline for the geometry</param>
        /// <param name="fillColor">Color with which the geometry is to be filled</param>
        void Redraw(ICanvasStroke stroke, Color fillColor);

        /// <summary>
        /// Redraws the IGeometrySurface by filling the existing geometry with
        /// the fill color and the background with the background color.
        /// </summary>
        /// <param name="fillColor">Color with which the geometry is to be filled</param>
        /// <param name="backgroundColor">Color with which the IGeometrySurface background is to be filled</param>
        void Redraw(Color fillColor, Color backgroundColor);

        /// <summary>
        /// Redraws the IGeometrySurface by outlining the existing geometry with the
        /// given ICanvasStroke, filling it with the fill color and the background with the background color.
        /// </summary>
        /// <param name="stroke">ICanvasStroke defining the outline for the geometry</param>
        /// <param name="fillColor">Color with which the geometry is to be filled</param>
        /// <param name="backgroundColor">Color with which the IGeometrySurface background is to be filled</param>
        void Redraw(ICanvasStroke stroke, Color fillColor, Color backgroundColor);

        /// <summary>
        /// Redraws the IGeometrySurface by filling the existing geometry with
        /// the fill brush.
        /// </summary>
        /// <param name="fillBrush">Brush with which the geometry is to be filled</param>
        void Redraw(ICanvasBrush fillBrush);

        /// <summary>
        /// Redraws the IGeometrySurface by outlining the existing geometry with the
        /// given ICanvasStroke, filling it with the fill brush.
        /// </summary>
        /// <param name="stroke">ICanvasStroke defining the outline for the geometry</param>
        /// <param name="fillBrush">Brush with which the geometry is to be filled</param>
        void Redraw(ICanvasStroke stroke, ICanvasBrush fillBrush);

        /// <summary>
        /// Redraws the IGeometrySurface by filling the existing geometry with
        /// the fill brush and the background with the background brush.
        /// </summary>
        /// <param name="fillBrush">Brush with which the geometry is to be filled</param>
        /// <param name="backgroundBrush">Brush with which the IGeometrySurface background is to be filled</param>
        void Redraw(ICanvasBrush fillBrush, ICanvasBrush backgroundBrush);

        /// <summary>
        /// Redraws the IGeometrySurface by outlining the existing geometry with the
        /// given ICanvasStroke, filling it with the fill brush and the background with the background brush.
        /// </summary>
        /// <param name="stroke">ICanvasStroke defining the outline for the geometry</param>
        /// <param name="fillBrush">Brush with which the geometry is to be filled</param>
        /// <param name="backgroundBrush">Brush with which the IGeometrySurface background is to be filled</param>
        void Redraw(ICanvasStroke stroke, ICanvasBrush fillBrush, ICanvasBrush backgroundBrush);

        /// <summary>
        /// Redraws the IGeometrySurface by filling the existing geometry with
        /// the fill color and the background with the background brush.
        /// </summary>
        /// <param name="fillColor">Color with which the geometry is to be filled</param>
        /// <param name="backgroundBrush">Brush with which the IGeometrySurface background is to be filled</param>
        void Redraw(Color fillColor, ICanvasBrush backgroundBrush);

        /// <summary>
        /// Redraws the IGeometrySurface by outlining the existing geometry with the
        /// given ICanvasStroke, filling it with the fill color and the background with the background brush.
        /// </summary>
        /// <param name="stroke">ICanvasStroke defining the outline for the geometry</param>
        /// <param name="fillColor">Color with which the geometry is to be filled</param>
        /// <param name="backgroundBrush">Brush with which the IGeometrySurface background is to be filled</param>
        void Redraw(ICanvasStroke stroke, Color fillColor, ICanvasBrush backgroundBrush);

        /// <summary>
        /// Redraws the IGeometrySurface by filling the existing geometry with
        /// the fill brush and the background with the background color.
        /// </summary>
        /// <param name="fillBrush">Brush with which the geometry is to be filled</param>
        /// <param name="backgroundColor">Color with which the IGeometrySurface background is to be filled</param>
        void Redraw(ICanvasBrush fillBrush, Color backgroundColor);

        /// <summary>
        /// Redraws the IGeometrySurface by outlining the existing geometry with the
        /// given ICanvasStroke, filling it with the fill brush and the background with the background color.
        /// </summary>
        /// <param name="stroke">ICanvasStroke defining the outline for the geometry</param>
        /// <param name="fillBrush">Brush with which the geometry is to be filled</param>
        /// <param name="backgroundColor">Color with which the IGeometrySurface background is to be filled</param>
        void Redraw(ICanvasStroke stroke, ICanvasBrush fillBrush, Color backgroundColor);

        /// <summary>
        /// Resizes the IGeometrySurface with the given size and redraws the IGeometrySurface
        /// with the new geometry.
        /// </summary>
        /// <param name="size">New size of the IGeometrySurface</param>
        /// <param name="geometry">New CanvasGeometry to be applied to the IGeometrySurface</param>
        void Redraw(Size size, CanvasGeometry geometry);

        /// <summary>
        /// Resizes the IGeometrySurface with the given size and redraws the IGeometrySurface
        /// with the new geometry and outlines it with the given ICanvasStroke.
        /// </summary>
        /// <param name="size">New size of the IGeometrySurface</param>
        /// <param name="geometry">New CanvasGeometry to be applied to the IGeometrySurface</param>
        /// <param name="stroke">ICanvasStroke defining the outline for the geometry</param>
        void Redraw(Size size, CanvasGeometry geometry, ICanvasStroke stroke);

        /// <summary>
        /// Resizes the IGeometrySurface with the given size and redraws the IGeometrySurface
        /// with the new geometry and fills it with the fill color.
        /// </summary>
        /// <param name="size">New size of the IGeometrySurface</param>
        /// <param name="geometry">New CanvasGeometry to be applied to the IGeometrySurface</param>
        /// <param name="fillColor">Fill color for the geometry</param>
        void Redraw(Size size, CanvasGeometry geometry, Color fillColor);

        /// <summary>
        /// Resizes the IGeometrySurface with the given size and redraws the IGeometrySurface
        /// with the new geometry, outlines it with the given ICanvasStroke and fills
        /// it with the fill color.
        /// </summary>
        /// <param name="size">New size of the IGeometrySurface</param>
        /// <param name="geometry">New CanvasGeometry to be applied to the IGeometrySurface</param>
        /// <param name="stroke">ICanvasStroke defining the outline for the geometry</param>
        /// <param name="fillColor">Fill color for the geometry</param>
        void Redraw(Size size, CanvasGeometry geometry, ICanvasStroke stroke, Color fillColor);

        /// <summary>
        /// Resizes the IGeometrySurface with the given size and redraws the IGeometrySurface
        /// with the new geometry and fills it with the fill color and
        /// fills the background with the background color.
        /// </summary>
        /// <param name="size">New size of the IGeometrySurface</param>
        /// <param name="geometry">New CanvasGeometry to be applied to the IGeometrySurface</param>
        /// <param name="fillColor">Fill color for the geometry</param>
        /// <param name="backgroundColor">Fill color for the IGeometrySurface background</param>
        void Redraw(Size size, CanvasGeometry geometry, Color fillColor, Color backgroundColor);

        /// <summary>
        /// Resizes the IGeometrySurface with the given size and redraws the IGeometrySurface
        /// with the new geometry, outlines it with the given ICanvasStroke and fills it with
        /// the fill color and fills the background with the background color.
        /// </summary>
        /// <param name="size">New size of the IGeometrySurface</param>
        /// <param name="geometry">New CanvasGeometry to be applied to the IGeometrySurface</param>
        /// <param name="stroke">ICanvasStroke defining the outline for the geometry</param>
        /// <param name="fillColor">Fill color for the geometry</param>
        /// <param name="backgroundColor">Fill color for the IGeometrySurface background</param>
        void Redraw(Size size, CanvasGeometry geometry, ICanvasStroke stroke, Color fillColor, Color backgroundColor);

        /// <summary>
        /// Resizes the IGeometrySurface with the given size and redraws the IGeometrySurface
        /// with the new geometry and fills it with the fill brush.
        /// </summary>
        /// <param name="size">New size of the IGeometrySurface</param>
        /// <param name="geometry">New CanvasGeometry to be applied to the IGeometrySurface</param>
        /// <param name="fillBrush">Brush to fill the geometry</param>
        void Redraw(Size size, CanvasGeometry geometry, ICanvasBrush fillBrush);

        /// <summary>
        /// Resizes the IGeometrySurface with the given size and redraws the IGeometrySurface
        /// with the new geometry and fills it with the fill brush and fills
        /// the background with the background brush.
        /// </summary>
        /// <param name="size">New size of the IGeometrySurface</param>
        /// <param name="geometry">New CanvasGeometry to be applied to the IGeometrySurface</param>
        /// <param name="fillBrush">Brush to fill the geometry</param>
        /// <param name="backgroundBrush">Brush to fill the IGeometrySurface background</param>
        void Redraw(Size size, CanvasGeometry geometry, ICanvasBrush fillBrush, ICanvasBrush backgroundBrush);

        /// <summary>
        /// Resizes the IGeometrySurface with the given size and redraws the IGeometrySurface
        /// with the new geometry, outlines it with the given ICanvasStroke and fills it with the
        /// fill brush and fills the background with the background brush.
        /// </summary>
        /// <param name="size">New size of the IGeometrySurface</param>
        /// <param name="geometry">New CanvasGeometry to be applied to the IGeometrySurface</param>
        /// <param name="stroke">ICanvasStroke defining the outline for the geometry</param>
        /// <param name="fillBrush">Brush to fill the geometry</param>
        /// <param name="backgroundBrush">Brush to fill the IGeometrySurface background</param>
        void Redraw(Size size, CanvasGeometry geometry, ICanvasStroke stroke, ICanvasBrush fillBrush, ICanvasBrush backgroundBrush);

        /// <summary>
        /// Resizes the IGeometrySurface with the given size and redraws the IGeometrySurface
        /// with the new geometry and fills it with the fill brush and the background
        /// with the background color.
        /// </summary>
        /// <param name="size">New size of the IGeometrySurface</param>
        /// <param name="geometry">New CanvasGeometry to be applied to the IGeometrySurface</param>
        /// <param name="fillBrush">Brush to fill the geometry</param>
        /// <param name="backgroundColor">Fill color for the IGeometrySurface background</param>
        void Redraw(Size size, CanvasGeometry geometry, ICanvasBrush fillBrush, Color backgroundColor);

        /// <summary>
        /// Resizes the IGeometrySurface with the given size and redraws the IGeometrySurface
        /// with the new geometry, outlines it with the given ICanvasStroke and fills it with
        /// the fill brush and the background with the background color.
        /// </summary>
        /// <param name="size">New size of the IGeometrySurface</param>
        /// <param name="geometry">New CanvasGeometry to be applied to the IGeometrySurface</param>
        /// <param name="stroke">ICanvasStroke defining the outline for the geometry</param>
        /// <param name="fillBrush">Brush to fill the geometry</param>
        /// <param name="backgroundColor">Fill color for the IGeometrySurface background</param>
        void Redraw(Size size, CanvasGeometry geometry, ICanvasStroke stroke, ICanvasBrush fillBrush, Color backgroundColor);

        /// <summary>
        /// Resizes the IGeometrySurface with the given size and redraws the IGeometrySurface
        /// with the new geometry and fills it with the fill color and the background
        /// with the background brush.
        /// </summary>
        /// <param name="size">New size of the IGeometrySurface</param>
        /// <param name="geometry">New CanvasGeometry to be applied to the IGeometrySurface</param>
        /// <param name="fillColor">Fill color for the geometry</param>
        /// <param name="backgroundBrush">Brush to fill the IGeometrySurface background</param>
        void Redraw(Size size, CanvasGeometry geometry, Color fillColor, ICanvasBrush backgroundBrush);

        /// <summary>
        /// Resizes the IGeometrySurface with the given size and redraws the IGeometrySurface
        /// with the new geometry, outlines it with the given ICanvasStroke and fills it with
        /// the fill color and the background with the background brush.
        /// </summary>
        /// <param name="size">New size of the IGeometrySurface</param>
        /// <param name="geometry">New CanvasGeometry to be applied to the IGeometrySurface</param>
        /// <param name="stroke">ICanvasStroke defining the outline for the geometry</param>
        /// <param name="fillColor">Fill color for the geometry</param>
        /// <param name="backgroundBrush">Brush to fill the IGeometrySurface background</param>
        void Redraw(Size size, CanvasGeometry geometry, ICanvasStroke stroke, Color fillColor, ICanvasBrush backgroundBrush);
    }
}
