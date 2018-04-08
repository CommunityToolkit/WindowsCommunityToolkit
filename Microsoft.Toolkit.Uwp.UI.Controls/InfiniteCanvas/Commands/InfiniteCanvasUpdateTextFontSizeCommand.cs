namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    internal class InfiniteCanvasUpdateTextFontSizeCommand : IInfiniteCanvasCommand
    {
        private readonly float _oldValue;
        private readonly float _newValue;
        private readonly TextDrawable _drawable;

        public InfiniteCanvasUpdateTextFontSizeCommand(TextDrawable drawable, float oldValue, float newValue)
        {
            _oldValue = oldValue;
            _newValue = newValue;
            _drawable = drawable;
        }

        public void Execute()
        {
            _drawable.FontSize = _newValue;
        }

        public void UnExecute()
        {
            _drawable.FontSize = _oldValue;
        }
    }
}