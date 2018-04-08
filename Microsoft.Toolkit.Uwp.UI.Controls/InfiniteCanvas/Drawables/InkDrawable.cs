using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Graphics.Canvas;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Input.Inking;
using Windows.UI.Xaml;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    internal class InkDrawable : IDrawable
    {
        public readonly IReadOnlyList<InkStroke> Strokes;

        public Rect Bounds { get; set; }

        public bool IsActive { get; set; }

        private static InkStrokeBuilder _strokeBuilder = new InkStrokeBuilder();

        public InkDrawable(IReadOnlyList<InkStroke> strokes)
        {
            if (!strokes.Any())
            {
                return;
            }

            Strokes = strokes;

            var first = strokes.First();
            double top = first.BoundingRect.Top, bottom = first.BoundingRect.Bottom, left = first.BoundingRect.Left, right = first.BoundingRect.Right;

            for (var index = 1; index < strokes.Count; index++)
            {
                var stroke = strokes[index];
                bottom = Math.Max(stroke.BoundingRect.Bottom, bottom);
                right = Math.Max(stroke.BoundingRect.Right, right);
                top = Math.Min(stroke.BoundingRect.Top, top);
                left = Math.Min(stroke.BoundingRect.Left, left);
            }

            Bounds = new Rect(left, top, right - left, bottom - top);
        }

        public bool IsVisible(Rect viewPort)
        {
            IsActive = RectHelper.Intersect(viewPort, Bounds) != Rect.Empty;
            return IsActive;
        }

        public void Draw(CanvasDrawingSession drawingSession, Rect sessionBounds)
        {
            List<InkStroke> finalStrokeList = new List<InkStroke>(Strokes.Count);

            foreach (InkStroke stroke in Strokes)
            {
                var points = stroke.GetInkPoints();
                var finalPointList = new List<InkPoint>(points.Count);
                foreach (InkPoint point in points)
                {
                    finalPointList.Add(InfiniteCanvas.MapPointToToSessionBounds(point, sessionBounds));
                }

                _strokeBuilder.SetDefaultDrawingAttributes(stroke.DrawingAttributes);
                var newStroke = _strokeBuilder.CreateStrokeFromInkPoints(finalPointList, stroke.PointTransform);
                finalStrokeList.Add(newStroke);
            }

            drawingSession.DrawInk(finalStrokeList);
        }
    }
}
