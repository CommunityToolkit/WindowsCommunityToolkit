namespace Microsoft.Toolkit.Uwp.UI.Controls.TextToolbarFormats
{
    using System;
    using Windows.UI.Text;

    /// <summary>
    /// Manipulates Selected Text into an applied format according to default buttons.
    /// </summary>
    public abstract class Formatter
    {
        public Formatter(TextToolbar model)
        {
            Model = model;
        }

        /// <summary>
        /// Applies Bold
        /// </summary>
        public abstract void FormatBold();

        /// <summary>
        /// Applies Italics
        /// </summary>
        public abstract void FormatItalics();

        /// <summary>
        /// Applies Strikethrough
        /// </summary>
        public abstract void FormatStrikethrough();

        /// <summary>
        /// Applies Code
        /// </summary>
        public abstract void FormatCode();

        /// <summary>
        /// Applies Quote
        /// </summary>
        public abstract void FormatQuote();

        /// <summary>
        /// Applies Link
        /// </summary>
        public abstract void FormatLink(string label, string link);

        /// <summary>
        /// Applies List
        /// </summary>
        public abstract void FormatList();

        /// <summary>
        /// Applies Ordered List
        /// </summary>
        public abstract void FormatOrderedList();

        /// <summary>
        /// Gets the source Toolbar
        /// </summary>
        public TextToolbar Model { get; }

        /// <summary>
        /// Gets the formatted version of the Editor's Text
        /// </summary>
        public abstract string Text { get; }

        /// <summary>
        /// Gets shortcut to Carriage Return
        /// </summary>
        protected string Return { get => "\r"; }

        /// <summary>
        /// Gets the current Editor Selection
        /// </summary>
        protected ITextSelection Select { get => Model.Editor.Document.Selection; }

        /// <summary>
        /// Determines the Position of the Selector, if not at a New Line, it will move the Selector to a new line.
        /// </summary>
        public void EnsureAtNewLine()
        {
            int val = Select.StartPosition;
            int counter = 0;
            bool atNewLine = false;

            Model.Editor.Document.GetText(TextGetOptions.NoHidden, out string DocText);
            var lines = DocText.Split(new string[] { Return }, StringSplitOptions.None);

            foreach (var line in lines)
            {
                if (counter == val)
                {
                    atNewLine = true;
                }

                foreach (var c in line)
                {
                    counter++;
                    if (counter >= val)
                    {
                        break;
                    }
                }

                counter++;
            }

            if (!atNewLine)
            {
                Select.Text += Return;
                Select.StartPosition += 1;
                Select.EndPosition = Select.StartPosition;
            }
        }
    }
}