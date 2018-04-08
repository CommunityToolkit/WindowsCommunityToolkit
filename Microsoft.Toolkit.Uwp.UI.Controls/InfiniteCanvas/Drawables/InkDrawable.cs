using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Graphics.Canvas;
using Windows.Foundation;
using Windows.UI.Input.Inking;
using Windows.UI.Xaml;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    internal class InkDrawable : IDrawable
    {
        public IReadOnlyList<InkStroke> Strokes { get; }

        public Rect Bounds { get; set; }

        public bool IsActive { get; set; }

        private static readonly InkStrokeBuilder StrokeBuilder = new InkStrokeBuilder();

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
            var finalStrokeList = new List<InkStroke>(Strokes.Count);

            foreach (var stroke in Strokes)
            {
                var points = stroke.GetInkPoints();
                var finalPointList = new List<InkPoint>(points.Count);
                foreach (var point in points)
                {
                    finalPointList.Add(MapPointToToSessionBounds(point, sessionBounds));
                }

                StrokeBuilder.SetDefaultDrawingAttributes(stroke.DrawingAttributes);
                var newStroke = StrokeBuilder.CreateStrokeFromInkPoints(finalPointList, stroke.PointTransform);
                finalStrokeList.Add(newStroke);
            }

            drawingSession.DrawInk(finalStrokeList);
        }

        private static InkPoint MapPointToToSessionBounds(InkPoint point, Rect sessionBounds)
        {
            return new InkPoint(new Point(point.Position.X - sessionBounds.X, point.Position.Y - sessionBounds.Y), point.Pressure, point.TiltX, point.TiltY, point.Timestamp);
        }
    }
}
