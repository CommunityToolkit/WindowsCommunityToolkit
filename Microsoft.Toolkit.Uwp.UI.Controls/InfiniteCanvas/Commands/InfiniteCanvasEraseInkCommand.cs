using System.Collections.Generic;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    internal class InfiniteCanvasEraseInkCommand : IInfiniteCanvasCommand
    {
        private readonly List<IDrawable> _drawableList;
        private readonly IDrawable _drawable;

        public InfiniteCanvasEraseInkCommand(List<IDrawable> drawableList, IDrawable drawable)
        {
            _drawable = drawable;
            _drawableList = drawableList;
        }

        public void Execute()
        {
            _drawableList.Remove(_drawable);
        }

        public void UnExecute()
        {
            _drawableList.Add(_drawable);
        }
    }
}
