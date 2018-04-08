using Windows.UI;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    internal class InfiniteCanvasUpdateTextColorCommand : IInfiniteCanvasCommand
    {
        private readonly Color _oldColor;
        private readonly Color _newColor;
        private readonly TextDrawable _drawable;

        public InfiniteCanvasUpdateTextColorCommand(TextDrawable drawable, Color oldText, Color newText)
        {
            _oldColor = oldText;
            _newColor = newText;
            _drawable = drawable;
        }

        public void Execute()
        {
            _drawable.TextColor = _newColor;
        }

        public void UnExecute()
        {
            _drawable.TextColor = _oldColor;
        }
    }
}