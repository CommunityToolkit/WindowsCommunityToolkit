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

        public TextDrawable(double top, double left, float fontSize, double height, double width, string text)
        {
            Bounds = new Rect(left, top, width, height);
            Text = text;
            FontSize = fontSize;
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
                WordWrapping = CanvasWordWrapping.NoWrap
            };

            CanvasTextLayout textLayout = new CanvasTextLayout(drawingSession, Text, format, 0.0f, 0.0f);

            Rect theRectYouAreLookingFor = new Rect(
                Bounds.X + textLayout.DrawBounds.X - sessionBounds.X,
                Bounds.Y + textLayout.DrawBounds.Y - sessionBounds.Y,
                textLayout.DrawBounds.Width,
                textLayout.DrawBounds.Height);

            //Bounds = theRectYouAreLookingFor;

            //drawingSession.DrawRectangle(Bounds, Colors.Gray, 1.0f);
            drawingSession.DrawTextLayout(textLayout, (float)Bounds.X, (float)Bounds.Y, Colors.Black);
        }
    }
}
