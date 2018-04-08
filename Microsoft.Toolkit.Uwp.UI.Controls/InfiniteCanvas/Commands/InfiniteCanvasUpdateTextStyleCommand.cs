namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    internal class InfiniteCanvasUpdateTextStyleCommand : IInfiniteCanvasCommand
    {
        private readonly bool _oldValue;
        private readonly bool _newValue;
        private readonly TextDrawable _drawable;

        public InfiniteCanvasUpdateTextStyleCommand(TextDrawable drawable, bool oldValue, bool newValue)
        {
            _oldValue = oldValue;
            _newValue = newValue;
            _drawable = drawable;
        }

        public void Execute()
        {
            _drawable.IsItalic = _newValue;
        }

        public void UnExecute()
        {
            _drawable.IsItalic = _oldValue;
        }
    }
}