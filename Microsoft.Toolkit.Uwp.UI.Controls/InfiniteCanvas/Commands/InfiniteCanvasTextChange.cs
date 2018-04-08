namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    internal class InfiniteCanvasTextChange : IInfiniteCanvasCommand
    {
        private readonly string _oldText;
        private readonly string _newText;
        private readonly TextDrawable _drawable;

        public InfiniteCanvasTextChange(TextDrawable drawable, string oldText, string newText)
        {
            _oldText = oldText;
            _newText = newText;
            _drawable = drawable;
        }

        public void Execute()
        {
            _drawable.Text = _newText;
        }

        public void UnExecute()
        {
            _drawable.Text = _oldText;
        }
    }
}