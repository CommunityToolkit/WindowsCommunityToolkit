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

using Microsoft.Toolkit.Uwp.UI.Controls.TextToolbarButtons;
using Windows.UI.Text;

namespace Microsoft.Toolkit.Uwp.UI.Controls.TextToolbarFormats.RichText
{
    public class RichTextButtonActions : ButtonActions
    {
        public RichTextButtonActions(RichTextFormatter formatter)
        {
            Formatter = formatter;
        }

        public override void FormatBold(ToolbarButton button)
        {
            var format = Formatter.SelectionFormat;
            if (!button.IsToggled)
            {
                format.Bold = FormatEffect.On;
                Formatter.SelectionFormat = format;
            }
            else
            {
                format.Bold = FormatEffect.Off;
                Formatter.SelectionFormat = format;
            }

            button.IsToggled = button.IsToggled != true;
        }

        public override void FormatItalics(ToolbarButton button)
        {
            var format = Formatter.SelectionFormat;
            if (!button.IsToggled)
            {
                format.Italic = FormatEffect.On;
                Formatter.SelectionFormat = format;
            }
            else
            {
                format.Italic = FormatEffect.Off;
                Formatter.SelectionFormat = format;
            }

            button.IsToggled = button.IsToggled != true;
        }

        public override void FormatStrikethrough(ToolbarButton button)
        {
            var format = Formatter.SelectionFormat;
            if (!button.IsToggled)
            {
                format.Strikethrough = FormatEffect.On;
                Formatter.SelectionFormat = format;
            }
            else
            {
                format.Strikethrough = FormatEffect.Off;
                Formatter.SelectionFormat = format;
            }

            button.IsToggled = button.IsToggled != true;
        }

        public override void FormatLink(ToolbarButton button, string label, string formattedText, string link)
        {
            var selected = Formatter.Selected;
            if (!string.IsNullOrWhiteSpace(label))
            {
                selected.SetText(TextSetOptions.FormatRtf, formattedText);
            }
            else
            {
                selected.Text = link;
            }

            // Fixes Link Replacement
            if (!string.IsNullOrWhiteSpace(selected.Link))
            {
                selected.Link = string.Empty;
            }

            selected.Link = $"\"{link}\"";

            var doc = Formatter.Model.Editor.Document;
            doc.ApplyDisplayUpdates(); // doesn't seem to work
        }

        public override void FormatList(ToolbarButton button)
        {
            var selected = Formatter.Selected;
            if (!button.IsToggled)
            {
                Formatter.OrderedListButton.IsToggled = false;
                selected.ParagraphFormat.ListType = MarkerType.Bullet;
                selected.ParagraphFormat.ListStyle = MarkerStyle.Plain;
            }
            else
            {
                selected.ParagraphFormat.ListType = MarkerType.None;
            }

            button.IsToggled = button.IsToggled != true;
        }

        public override void FormatOrderedList(ToolbarButton button)
        {
            var selected = Formatter.Selected;
            if (!button.IsToggled)
            {
                Formatter.ListButton.IsToggled = false;
                selected.ParagraphFormat.ListType = MarkerType.UnicodeSequence;
                selected.ParagraphFormat.ListStyle = MarkerStyle.Period;
            }
            else
            {
                selected.ParagraphFormat.ListType = MarkerType.None;
            }

            button.IsToggled = button.IsToggled != true;
        }

        public void FormatUnderline(ToolbarButton button)
        {
            var format = Formatter.SelectionFormat;
            if (!button.IsToggled)
            {
                format.Underline = UnderlineType.Single;
                Formatter.SelectionFormat = format;
            }
            else
            {
                format.Underline = UnderlineType.None;
                Formatter.SelectionFormat = format;
            }

            button.IsToggled = button.IsToggled != true;
        }

        public RichTextFormatter Formatter { get; }
    }
}