namespace Microsoft.Toolkit.Uwp.UI.Controls.TextToolbarFormats
{
    using System;
    using Windows.UI.Text;

    public class RichTextFormatter : Formatter
    {
        public RichTextFormatter(TextToolbar model)
            : base(model)
        {
        }

        public override string Text
        {
            get
            {
                Model.Editor.Document.GetText(TextGetOptions.FormatRtf, out string currentvalue);
                return currentvalue;
            }
        }

        public override void FormatBold()
        {
            throw new NotImplementedException();
        }

        public override void FormatItalics()
        {
            throw new NotImplementedException();
        }

        public override void FormatStrikethrough()
        {
            throw new NotImplementedException();
        }

        public override void FormatCode()
        {
            throw new NotImplementedException();
        }

        public override void FormatQuote()
        {
            throw new NotImplementedException();
        }

        public override void FormatLink(string label, string link)
        {
            throw new NotImplementedException();
        }

        public override void FormatList()
        {
            throw new NotImplementedException();
        }

        public override void FormatOrderedList()
        {
            throw new NotImplementedException();
        }
    }
}