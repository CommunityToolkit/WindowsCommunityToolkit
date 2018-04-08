using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.UI.Composition;
using Windows.Foundation;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// The virtual Drawing surface renderer used to render the ink and text.
    /// </summary>
    internal partial class InfiniteCanvasVirtualDrawingSurface
    {
        private readonly List<IDrawable> _visibleList = new List<IDrawable>();
        private readonly List<IDrawable> _drawableList = new List<IDrawable>();

        public void ReDraw(Rect viewPort)
        {
            _visibleList.Clear();
            double top = double.MaxValue,
                   bottom = double.MinValue,
                   left = double.MaxValue,
                   right = double.MinValue;

            foreach (var drawable in _drawableList)
            {
                if (drawable.IsVisible(viewPort))
                {
                    _visibleList.Add(drawable);

                    bottom = Math.Max(drawable.Bounds.Bottom, bottom);
                    right = Math.Max(drawable.Bounds.Right, right);
                    top = Math.Min(drawable.Bounds.Top, top);
                    left = Math.Min(drawable.Bounds.Left, left);
                }
            }

            Rect toDraw;
            if (_visibleList.Any())
            {
                toDraw = new Rect(Math.Max(left, 0), Math.Max(top, 0), Math.Max(right - left, 0), Math.Max(bottom - top, 0));

                toDraw.Union(viewPort);
            }
            else
            {
                toDraw = viewPort;
            }

            using (CanvasDrawingSession drawingSession = CanvasComposition.CreateDrawingSession(_drawingSurface, toDraw))
            {
                drawingSession.Clear(DrawingCanvasBackground);
                foreach (var drawable in _visibleList)
                {
                    drawable.Draw(drawingSession, toDraw);
                }
            }
        }

        public void ClearAll(Rect viewPort)
        {
            _visibleList.Clear();
            ExecuteClearAll();

            using (CanvasDrawingSession drawingSession = CanvasComposition.CreateDrawingSession(_drawingSurface, viewPort))
            {
                drawingSession.Clear(DrawingCanvasBackground);
            }
        }
    }
}
