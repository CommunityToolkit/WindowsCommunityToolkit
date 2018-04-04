using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Graphics.Canvas;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Input.Inking;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Microsoft.Graphics.Canvas.Text;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    internal class TextDrawable : IDrawable
    {
        public readonly List<InfiniteCanvas.InfiniteCanvasTextBlock> CanvasTextBlockList;

        public string OriginalText { get; set; }

        public Rect Bounds { get; set; }

        public bool IsActive { get; set; }

        public TextDrawable(double top, double left, double height, double width, List<InfiniteCanvas.InfiniteCanvasTextBlock> canvasTextBlockList, string text)
        {
            Bounds = new Rect(left, top, width, height);
            CanvasTextBlockList = canvasTextBlockList;
            OriginalText = text;
        }

        public bool IsVisible(Rect viewPort)
        {
            IsActive = RectHelper.Intersect(viewPort, Bounds) != Rect.Empty;
            return IsActive;
        }

        public void Draw(CanvasDrawingSession drawingSession, Rect sessionBounds)
        {
            var currentXLocation = Bounds.X;
            drawingSession.DrawRectangle(Bounds, Colors.Gray, 1.0f);

            foreach (var textBlock in CanvasTextBlockList)
            {
                CanvasTextFormat format = new CanvasTextFormat
                {
                    FontSize = textBlock.FontSize,
                    FontStyle = textBlock.IsItalic ? FontStyle.Italic : FontStyle.Normal,
                    FontWeight = textBlock.IsBold ? FontWeights.Bold : FontWeights.Normal,
                    WordWrapping = CanvasWordWrapping.NoWrap
                };

                CanvasTextLayout textLayout = new CanvasTextLayout(drawingSession, textBlock.Text, format, 0.0f, 0.0f);

                drawingSession.DrawTextLayout(textLayout, (float)currentXLocation, (float)Bounds.Y, Colors.Black);

                currentXLocation += textLayout.DrawBounds.Width;
            }
        }
    }
}
