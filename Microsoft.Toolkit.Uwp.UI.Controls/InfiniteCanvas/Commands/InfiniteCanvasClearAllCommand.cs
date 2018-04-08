using System.Collections.Generic;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    internal class InfiniteCanvasClearAllCommand : IInfiniteCanvasCommand
    {
        private List<IDrawable> _drawableList;
        private IDrawable[] _storeList;

        public InfiniteCanvasClearAllCommand(List<IDrawable> drawableList)
        {
            _drawableList = drawableList;
        }

        public void Execute()
        {
            _storeList = new IDrawable[_drawableList.Count];
            for (int i = 0; i < _drawableList.Count; i++)
            {
                _storeList[i] = _drawableList[i];
            }

            _drawableList.Clear();
        }

        public void UnExecute()
        {
            foreach (var drawable in _storeList)
            {
                _drawableList.Add(drawable);
            }
        }
    }
}
