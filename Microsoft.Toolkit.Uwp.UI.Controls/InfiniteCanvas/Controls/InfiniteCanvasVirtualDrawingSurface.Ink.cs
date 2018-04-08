using System;
using System.Linq;
using Windows.Foundation;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// The virtual Drawing surface renderer used to render the ink and text.
    /// </summary>
    internal partial class InfiniteCanvasVirtualDrawingSurface
    {
        public void Erase(Point point, Rect viewPort, float zoomFactor)
        {
            const int tolerance = 5;
            float toleranceWithZoom = tolerance;
            if (zoomFactor > 1)
            {
                toleranceWithZoom /= zoomFactor;
            }

            for (var i = _visibleList.Count - 1; i >= 0; i--)
            {
                var drawable = _visibleList[i];
                if (drawable is InkDrawable inkDrawable && drawable.Bounds.Contains(point))
                {
                    foreach (var stroke in inkDrawable.Strokes)
                    {
                        if (stroke.BoundingRect.Contains(point))
                        {
                            foreach (var inkPoint in stroke.GetInkPoints())
                            {
                                if (Math.Abs(point.X - inkPoint.Position.X) < toleranceWithZoom && Math.Abs(point.Y - inkPoint.Position.Y) < toleranceWithZoom)
                                {
                                    var toRemove = _visibleList.ElementAt(i);
                                    ExecuteEraseInk(toRemove);
                                    ReDraw(viewPort);
                                }
                            }

                            return;
                        }
                    }
                }
            }
        }
    }
}
