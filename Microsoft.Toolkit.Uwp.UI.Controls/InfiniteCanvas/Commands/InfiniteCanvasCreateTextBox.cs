using System.Collections.Generic;
using Windows.UI;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    internal class InfiniteCanvasCreateTextBox : IInfiniteCanvasCommand
    {
        private readonly List<IDrawable> _drawableList;
        private readonly TextDrawable _drawable;

        public InfiniteCanvasCreateTextBox(List<IDrawable> drawableList, double x, double y, double width, double height, int textFontSize, string text, Color color, bool isBold, bool isItalic)
        {
            _drawable = new TextDrawable(
                x,
                y,
                width,
                height,
                textFontSize,
                text,
                color,
                isBold,
                isItalic);
            _drawableList = drawableList;
        }

        public void Execute()
        {
            _drawableList.Add(_drawable);
        }

        public void UnExecute()
        {
            _drawableList.Remove(_drawable);
        }
    }
}