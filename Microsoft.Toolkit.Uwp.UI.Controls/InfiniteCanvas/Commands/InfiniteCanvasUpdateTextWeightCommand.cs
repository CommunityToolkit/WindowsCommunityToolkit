namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    internal class InfiniteCanvasUpdateTextWeightCommand : IInfiniteCanvasCommand
    {
        private readonly bool _oldValue;
        private readonly bool _newValue;
        private readonly TextDrawable _drawable;

        public InfiniteCanvasUpdateTextWeightCommand(TextDrawable drawable, bool oldValue, bool newValue)
        {
            _oldValue = oldValue;
            _newValue = newValue;
            _drawable = drawable;
        }

        public void Execute()
        {
            _drawable.IsBold = _newValue;
        }

        public void UnExecute()
        {
            _drawable.IsBold = _oldValue;
        }
    }
}