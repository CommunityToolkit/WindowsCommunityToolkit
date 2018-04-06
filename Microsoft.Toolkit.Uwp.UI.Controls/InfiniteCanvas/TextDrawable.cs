using System.Collections.Generic;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Text;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Text;
using Windows.UI.Xaml;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    internal class TextDrawable : IDrawable
    {
        public string Text { get; set; }

        public Rect Bounds { get; set; }

        public bool IsActive { get; set; }

        public float FontSize { get; set; }
        public Color TextColor { get; set; }
        public bool IsBold { get; set; }
        public bool IsItalic { get; set; }

        public TextDrawable(double left, double top, double width, double height, float fontSize, string text, Color textColor, bool isBold, bool isItalic)
        {
            Bounds = new Rect(left, top, width, height);
            Text = text;
            FontSize = fontSize;
            TextColor = textColor;
            IsBold = isBold;
            IsItalic = isItalic;
        }

        public bool IsVisible(Rect viewPort)
        {
            IsActive = RectHelper.Intersect(viewPort, Bounds) != Rect.Empty;
            return IsActive;
        }

        public void Draw(CanvasDrawingSession drawingSession, Rect sessionBounds)
        {
            CanvasTextFormat format = new CanvasTextFormat
            {
                FontSize = FontSize,
                WordWrapping = CanvasWordWrapping.NoWrap,
                FontWeight = IsBold ? FontWeights.Bold : FontWeights.Normal,
                FontStyle = IsItalic ? FontStyle.Italic : FontStyle.Normal
            };

            CanvasTextLayout textLayout = new CanvasTextLayout(drawingSession, Text, format, 0.0f, 0.0f);

            drawingSession.DrawTextLayout(textLayout, (float)(Bounds.X - sessionBounds.X), (float)(Bounds.Y - sessionBounds.Y), TextColor);
        }
    }
}
