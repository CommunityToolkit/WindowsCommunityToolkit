using System.Collections.Generic;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    internal class InfiniteCanvasRemoveTextBoxCommand : IInfiniteCanvasCommand
    {
        private readonly List<IDrawable> _drawableList;
        private readonly TextDrawable _drawable;

        public InfiniteCanvasRemoveTextBoxCommand(List<IDrawable> drawableList, TextDrawable drawable)
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