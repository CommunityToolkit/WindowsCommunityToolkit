// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

namespace Microsoft.Toolkit.Uwp.UI.Controls.TextToolbarFormats
{
    using System;
    using System.Linq;
    using Microsoft.Toolkit.Uwp.UI.Controls.TextToolbarButtons;
    using Windows.UI.Text;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Input;
    using Windows.UI.Xaml.Media;

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

        public override ButtonMap DefaultButtons
        {
            get
            {
                return new ButtonMap
                {
                    Model.CommonButtons.Bold,
                    Model.CommonButtons.Italics,
                    Model.CommonButtons.Strikethrough,

                    new ToolbarSeparator(),

                    new ToolbarButton
                    {
                        Name = TextToolbar.HeadersElement,
                        Icon = new SymbolIcon { Symbol = Symbol.FontSize },
                        ToolTip = Model.HeaderLabel,
                        Click = StyleHeader
                    },
                    new ToolbarButton
                    {
                        Name = TextToolbar.CodeElement,
                        ToolTip = Model.CodeLabel,
                        Icon = new FontIcon { Glyph = "{}", FontFamily = new FontFamily("Segoe UI"), Margin = new Thickness(0, -5, 0, 0) },
                        Click = FormatCode
                    },
                    new ToolbarButton
                    {
                        Name = TextToolbar.QuoteElement,
                        ToolTip = Model.QuoteLabel,
                        Icon = new SymbolIcon { Symbol = Symbol.Message },
                        Click = FormatQuote
                    },
                    Model.CommonButtons.Link,

                    new ToolbarSeparator(),

                    Model.CommonButtons.List,
                    Model.CommonButtons.OrderedList
                };
            }
        }

        public void StyleHeader(ToolbarButton button)
        {
            var list = new ListBox { Margin = new Thickness(0), Padding = new Thickness(0) };
            var flyout = new Flyout { Content = list };

            void HeaderSelected(object sender, TappedRoutedEventArgs e)
            {
                var item = sender as ListBoxItem;
                SetSelection(item.Tag as string, string.Empty, false);
                flyout.Hide();
            }

            string headerVal = "#";
            for (int i = 1; i <= 5; i++)
            {
                string val = string.Concat(Enumerable.Repeat(headerVal, i));
                var item = new ListBoxItem
                {
                    Content = new MarkdownTextBlock
                    {
                        Text = val + Model.HeaderLabel,
                        IsTextSelectionEnabled = false
                    },
                    Tag = val,
                    Padding = new Thickness(5, 2, 5, 2),
                    Margin = new Thickness(0)
                };
                item.Tapped += HeaderSelected;
                list.Items.Add(item);
            }

            flyout.ShowAt(button);
        }

        public override void FormatBold(ToolbarButton button)
        {
            SetSelection("**", "**");
        }

        public override void FormatItalics(ToolbarButton button)
        {
            SetSelection("_", "_");
        }

        public override void FormatStrikethrough(ToolbarButton button)
        {
            SetSelection("~~", "~~");
        }

        public void FormatCode(ToolbarButton button)
        {
            if (DetermineSimpleReverse("`", "`"))
            {
                return;
            }
            else if (!Select.Text.Contains(Return))
            {
                SetSelection("`", "`");
            }
            else
            {
                string CodeLines()
                {
                    return ListLineIterator == 1 || ReachedEndLine ? "```" : string.Empty;
                }

                SetList(CodeLines, button, wrapNewLines: true, enableToggle: false);
            }
        }

        public void FormatQuote(ToolbarButton button)
        {
            SetList(() => "> ", button);
        }

        public override void FormatLink(ToolbarButton button, string label, string link)
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

        public override void FormatList(ToolbarButton button)
        {
            SetList(() => "- ", button);
        }

        public override void FormatOrderedList(ToolbarButton button)
        {
            string Iterate()
            {
                return ListLineIterator + ". ";
            }

            SetList(Iterate, button);
        }

        /// <summary>
        /// Applies formatting to Selected Text, or Removes formatting if already applied.
        /// </summary>
        /// <param name="start">Formatting in front of Text</param>
        /// <param name="end">Formatting at end of Text</param>
        /// <param name="reversible">Is the Text reversible?</param>
        /// <param name="contents">Text to insert between Start and End (Overwrites Current Text)</param>
        public virtual void SetSelection(string start, string end, bool reversible = true, string contents = null)
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
        protected virtual bool DetermineSimpleReverse(string start, string end)
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
        protected virtual bool DetermineSimpleInlineReverse(string start, string end)
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
        /// Gets the value of the Line Number Iterator. Use this for generating Numbered Lists.
        /// </summary>
        public int ListLineIterator { get; private set; } = 1;

        /// <summary>
        /// Gets a value indicating whether gets whether it is the last line of the list.
        /// </summary>
        public bool ReachedEndLine { get; private set; } = false;

        /// <summary>
        ///  This function will either add List Characters to lines of text, or Remove List Characters from Lines of Text, if already applied.
        /// </summary>
        /// <param name="listChar">A function for generating a List Character, use ListLineIterator to generate a Numbered Style List, or return a string Result, e.g. () => "- "</param>
        /// <param name="button">Button that activated the Set List</param>
        /// <param name="wrapNewLines">Adds New Lines to Start and End of Selected Text</param>
        /// <param name="enableToggle">Is this a Toggleable element?</param>
        public virtual void SetList(Func<string> listChar, ToolbarButton button, bool wrapNewLines = false, bool enableToggle = true)
        {
            void SetListTextChanged(object sender, RoutedEventArgs e)
            {
                Select.StartPosition -= 1;
                var lastEntered = Select.Text;
                Select.StartPosition += 1;

                if (lastEntered == Return)
                {
                    ListLineIterator++;
                    Select.Text += listChar();

                    Select.StartPosition = Select.EndPosition;
                }
            }

            if (Model.Editor == null)
            {
                return;
            }
            else if (enableToggle)
            {
                button.IsToggleable = true;
                button.IsToggled = true;

                Model.Editor.TextChanged += SetListTextChanged;
                button.ToggleEnded += (s, ee) =>
                {
                    Model.Editor.TextChanged -= SetListTextChanged;
                };
            }

            ListLineIterator = 1;
            ReachedEndLine = false;
            if (!DetermineListReverse(listChar, wrapNewLines))
            {
                ListLineIterator = 1;
                ReachedEndLine = false;

                EnsureAtNewLine();
                string text = listChar();

                var lines = Select.Text.Split(new string[] { Return }, StringSplitOptions.None).ToList();
                if (!wrapNewLines)
                {
                    lines.RemoveAt(lines.Count - 1); // remove last escape as selected end of last line
                }
                else
                {
                    lines.Insert(0, string.Empty);
                }

                for (int i = 0; i < lines.Count; i++)
                {
                    ListLineIterator++;

                    var element = lines[i];
                    text += element;

                    ReachedEndLine = i + 1 >= lines.Count;

                    if (lines.Count > 1 && !ReachedEndLine)
                    {
                        text += Return;
                    }

                    if (!ReachedEndLine || wrapNewLines)
                    {
                        text += listChar();

                        if (ReachedEndLine)
                        {
                            text += Return;
                        }
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
        /// <param name="wrapNewLines">Adds New Lines to Start and End of Selected Text</param>
        /// <returns>True if List formatting is reversing, otherwise false</returns>
        protected virtual bool DetermineListReverse(Func<string> listChar, bool wrapNewLines)
        {
            if (string.IsNullOrWhiteSpace(Select.Text))
            {
                return false;
            }

            if (wrapNewLines && DetermineInlineWrapListReverse(listChar))
            {
                return true;
            }

            string text = string.Empty;
            int startpos = Select.StartPosition;

            var lines = Select.Text.Split(new string[] { Return }, StringSplitOptions.None).ToList();
            if (wrapNewLines)
            {
                lines.RemoveAt(lines.Count - 1); // removes the line kept from Wrapping with NewLines.
            }

            for (int i = 0; i < lines.Count; i++)
            {
                var line = lines[i];

                ReachedEndLine = i + 1 >= lines.Count;
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

                ListLineIterator++;
            }

            if (wrapNewLines)
            {
                text = text.Remove(0, 1);
                text = text.Remove(text.Length - 1);
            }

            Select.Text = text;
            return true;
        }

        protected virtual bool DetermineInlineWrapListReverse(Func<string> listChar)
        {
            try
            {
                ListLineIterator = 1;
                string start = listChar();
                int startpos = Select.StartPosition - start.Length - 1; // removing newline char as well

                ReachedEndLine = true;
                string end = listChar();

                Model.Editor.Document.GetText(TextGetOptions.None, out string text);

                string startText = text.Substring(startpos, start.Length);
                if (startText == start)
                {
                    string endText = text.Substring(Select.EndPosition + end.Length - 3, end.Length);
                    if (endText == end)
                    {
                        return true; // works if Line Chars are only on the first and last lines, this would need to check all the other line chars for other NewLine Wrap methods that change the line char for all lines.
                    }
                }
            }
            catch
            {
            }

            ListLineIterator = 1;
            ReachedEndLine = false;

            return false;
        }
    }
}