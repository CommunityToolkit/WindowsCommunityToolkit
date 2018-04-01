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
        public readonly IReadOnlyList<InkStroke> Strokes;

        public Rect Bounds { get; set; }

        public bool IsActive { get; set; }

        public InkDrawable(IReadOnlyList<InkStroke> strokes)
        {
            if (!strokes.Any())
            {
                return;
            }

            Strokes = strokes;

            var first = strokes.First();
            double top = first.BoundingRect.Top, bottom = first.BoundingRect.Bottom, left = first.BoundingRect.Left, right = first.BoundingRect.Right;

            foreach (var stroke in strokes)
            {
                bottom = Math.Max(stroke.BoundingRect.Bottom, bottom);
                right = Math.Max(stroke.BoundingRect.Right, right);
                top = Math.Min(stroke.BoundingRect.Top, top);
                left = Math.Min(stroke.BoundingRect.Left, left);
            }

            Bounds = new Rect(top, left, bottom - top, right - left);
        }

        public bool IsVisible(Rect viewPort)
        {
            IsActive = RectHelper.Intersect(viewPort, Bounds) != Rect.Empty;
            return IsActive;
        }

        public void Draw(CanvasDrawingSession drawingSession)
        {
            drawingSession.DrawInk(Strokes);
        }
    }
}
