using System.Collections.Generic;
using Windows.UI.Input.Inking;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    internal class InfiniteCanvasCreateInkCommand : IInfiniteCanvasCommand
    {
        private readonly List<IDrawable> _drawableList;
        private readonly InkDrawable _drawable;

        public InfiniteCanvasCreateInkCommand(List<IDrawable> drawableList, IReadOnlyList<InkStroke> strokes)
        {
            _drawable = new InkDrawable(strokes);
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
