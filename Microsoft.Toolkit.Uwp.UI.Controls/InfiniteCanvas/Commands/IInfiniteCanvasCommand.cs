using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    internal interface IInfiniteCanvasCommand
    {
        void Execute();

        void UnExecute();
    }

    internal class InfiniteCanvasTextChange : IInfiniteCanvasCommand
    {
        public string OldText { get; set; }

        public string NewText { get; set; }

        public TextDrawable Drawable { get; set; }

        public InfiniteCanvasTextChange(TextDrawable drawable, string oldText, string newText)
        {
            OldText = oldText;
            NewText = newText;
            Drawable = drawable;
        }

        public void Execute()
        {
            Drawable.Text = NewText;
        }

        public void UnExecute()
        {
            Drawable.Text = OldText;
        }
    }
}
