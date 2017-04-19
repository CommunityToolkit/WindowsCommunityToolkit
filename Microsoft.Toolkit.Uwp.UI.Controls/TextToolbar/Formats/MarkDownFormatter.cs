namespace Microsoft.Toolkit.Uwp.UI.Controls.TextToolbarFormats
{
    using System;
    using System.Linq;
    using Windows.UI.Text;

    public class MarkDownFormatter : Formatter
    {
        public MarkDownFormatter(TextToolbar model)
            : base(model)
        {
        }

        public override string Text
        {
            get
            {
                Model.Editor.Document.GetText(TextGetOptions.UseCrlf, out string currentvalue);
                return currentvalue.Replace('\n', '\r'); // Converts CRLF into double Return for Markdown new line.
            }
        }

        public override void FormatBold()
        {
            SetSelection("**", "**");
        }

        public override void FormatItalics()
        {
            SetSelection("_", "_");
        }

        public override void FormatStrikethrough()
        {
            SetSelection("~~", "~~");
        }

        public override void FormatCode()
        {
            SetSelection("```", string.Empty);
        }

        public override void FormatQuote()
        {
            SetList(() => "> ");
        }

        public override void FormatLink(string label, string link)
        {
            int originalStart = Select.StartPosition;

            if (string.IsNullOrWhiteSpace(label))
            {
                if (!string.IsNullOrWhiteSpace(link))
                {
                    SetSelection($"[{Model.LabelLabel}](", ")", false, link);
                    Select.StartPosition = Select.EndPosition;
                    Select.EndPosition = Select.StartPosition;
                }
                else
                {
                    string startChars = $"[{Model.LabelLabel}](";
                    string filler = Model.UrlLabel;
                    SetSelection(startChars, ")", false, filler);
                    Select.StartPosition = originalStart + startChars.Length;
                    Select.EndPosition = Select.StartPosition + filler.Length;
                }
            }
            else if (string.IsNullOrWhiteSpace(link))
            {
                SetSelection("[", $"]({Model.UrlLabel})", false, label);
            }
            else
            {
                Select.Text = $"[{label}]({link})";
                Select.StartPosition = Select.EndPosition;
                Select.EndPosition = Select.StartPosition;
            }
        }

        public override void FormatList()
        {
            SetList(() => "- ");
        }

        public override void FormatOrderedList()
        {
            string Iterate()
            {
                ListLineIterator++;
                return ListLineIterator + ". ";
            }

            SetList(() => Iterate());
        }

        /// <summary>
        /// Applies formatting to Selected Text, or Removes formatting if already applied.
        /// </summary>
        /// <param name="start">Formatting in front of Text</param>
        /// <param name="end">Formatting at end of Text</param>
        /// <param name="reversible">Is the Text reversible?</param>
        /// <param name="contents">Text to insert between Start and End (Overwrites Current Text)</param>
        public void SetSelection(string start, string end, bool reversible = true, string contents = null)
        {
            if (Model.Editor == null)
            {
                return;
            }

            if (!reversible || !DetermineSimpleReverse(start, end))
            {
                int originalStartPos = Select.StartPosition;

                string originalText = contents ?? Select.Text;
                Select.Text = start + originalText + end;

                if (string.IsNullOrWhiteSpace(originalText))
                {
                    Select.StartPosition = originalStartPos + start.Length;
                    Select.EndPosition = Select.StartPosition;
                }
                else
                {
                    Select.StartPosition = originalStartPos + start.Length;
                    Select.EndPosition = Select.StartPosition + originalText.Length;
                }
            }
        }

        /// <summary>
        /// Determines if formatting is to be reversed
        /// </summary>
        /// <param name="start">Formatting in front of Text</param>
        /// <param name="end">Formatting at end of Text</param>
        /// <returns>True if formatting is reversing, otherwise false</returns>
        private bool DetermineSimpleReverse(string start, string end)
        {
            if (!DetermineSimpleInlineReverse(start, end))
            {
                try
                {
                    int startpos = Select.StartPosition - start.Length;
                    int endpos = Select.EndPosition;

                    Model.Editor.Document.GetText(TextGetOptions.NoHidden, out string text);
                    if (text.Substring(startpos, start.Length) == start)
                    {
                        string endofstring = text.Substring(endpos, end.Length);
                        if (endofstring == end)
                        {
                            text = text.Remove(startpos, start.Length);
                            endpos -= start.Length;
                            Model.Editor.Document.SetText(TextSetOptions.None, text.Remove(endpos, end.Length));

                            Select.StartPosition = startpos;
                            Select.EndPosition = endpos;
                            return true;
                        }
                    }
                }
                catch
                {
                }

                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Determines if formatting is to be reversed, when the formatting is located inside the Selectors.
        /// </summary>
        /// <param name="start">Formatting in front of Text</param>
        /// <param name="end">Formatting at end of Text</param>
        /// <returns>True if formatting is reversing, otherwise false</returns>
        private bool DetermineSimpleInlineReverse(string start, string end)
        {
            try
            {
                if (Select.Text.Substring(0, start.Length) == start)
                {
                    if (Select.Text.Substring(Select.Text.Length - end.Length, end.Length) == end)
                    {
                        Select.Text = Select.Text.Substring(start.Length, Select.Text.Length - end.Length - start.Length);
                        return true;
                    }
                }
            }
            catch
            {
            }

            return false;
        }

        /// <summary>
        /// Gets or sets the value of the Line Number Iterator (Must be Iterated Manually). Use this for generating Numbered Lists.
        /// </summary>
        public int ListLineIterator { get; set; } = 0;

        /// <summary>
        ///  This function will either add List Characters to lines of text, or Remove List Characters from Lines of Text, if already applied.
        /// </summary>
        /// <param name="listChar">A function for generating a List Character, use ListLineIterator to generate a Numbered Style List, or return a string Result, e.g. () => "- "</param>
        public void SetList(Func<string> listChar)
        {
            if (Model.Editor == null)
            {
                return;
            }

            ListLineIterator = 0;
            if (!DetermineListReverse(listChar))
            {
                ListLineIterator = 0;

                EnsureAtNewLine();
                string text = listChar();
                var lines = Select.Text.Split(new string[] { Return }, StringSplitOptions.RemoveEmptyEntries).ToList();
                for (int i = 0; i < lines.Count; i++)
                {
                    var element = lines[i];
                    text += element;

                    if (lines.Count > 1)
                    {
                        text += Return;
                    }

                    bool atEnd = i + 1 >= lines.Count;
                    if (!atEnd)
                    {
                        text += listChar();
                    }
                }

                Select.Text = text;

                if (!lines.Any(line => !string.IsNullOrWhiteSpace(line)))
                {
                    Select.StartPosition = Select.EndPosition;
                }
            }
        }

        /// <summary>
        /// Determines whether a List already has the formatting applied.
        /// </summary>
        /// <param name="listChar">Function to generate the List Character</param>
        /// <returns>True if List formatting is reversing, otherwise false</returns>
        private bool DetermineListReverse(Func<string> listChar)
        {
            if (string.IsNullOrWhiteSpace(Select.Text))
            {
                return false;
            }

            string text = string.Empty;
            int startpos = Select.StartPosition;
            var lines = Select.Text.Split(new string[] { Return }, StringSplitOptions.RemoveEmptyEntries).ToList();

            for (int i = 0; i < lines.Count; i++)
            {
                var line = lines[i];
                string listchar = listChar();
                try
                {
                    if (line.Substring(0, listchar.Length) == listchar)
                    {
                        text += line.Remove(0, listchar.Length);
                        if (lines.Count > 1)
                        {
                            text += Return;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                catch
                {
                    return false;
                }
            }

            Select.Text = text;
            return true;
        }
    }
}