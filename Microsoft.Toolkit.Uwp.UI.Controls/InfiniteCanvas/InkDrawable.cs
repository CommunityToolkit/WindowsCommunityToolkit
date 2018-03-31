using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.Graphics.Canvas;
using Windows.Foundation;
using Windows.UI.Input.Inking;
using Windows.UI.Xaml;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    internal class InkDrawable : IDrawable
    {
        private readonly Rect _bounds;
        private readonly IReadOnlyList<InkStroke> _strokes;

        public InkDrawable(IReadOnlyList<InkStroke> strokes)
        {
            if (!strokes.Any())
            {
                return;
            }

            _strokes = strokes;

            var first = strokes.First();
            double top = first.BoundingRect.Top, bottom = first.BoundingRect.Bottom, left = first.BoundingRect.Left, right = first.BoundingRect.Right;

            foreach (var stroke in strokes)
            {
                bottom = Math.Max(stroke.BoundingRect.Bottom, bottom);
                right = Math.Max(stroke.BoundingRect.Right, right);
                top = Math.Min(stroke.BoundingRect.Top, top);
                left = Math.Min(stroke.BoundingRect.Left, left);
            }

            _bounds = new Rect(top, left, bottom - top, right - left);
        }

        public bool IsVisible(Rect viewPort)
        {
            return RectHelper.Intersect(viewPort, _bounds) != Rect.Empty;
        }

        public void Draw(CanvasDrawingSession drawingSession)
        {
            drawingSession.DrawInk(_strokes);
        }
    }
}
