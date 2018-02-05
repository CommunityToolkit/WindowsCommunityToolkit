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

namespace Microsoft.Toolkit.Uwp.UI.Controls.TextToolbarFormats.MarkDown
{
    public class MarkDownButtonActions : ButtonActions
    {
        public MarkDownButtonActions(MarkDownFormatter formatter)
        {
            Formatter = formatter;
        }

        public override void FormatBold(ToolbarButton button)
        {
            Formatter.SetSelection("**", "**");
        }

        public override void FormatItalics(ToolbarButton button)
        {
            Formatter.SetSelection("_", "_");
        }

        public override void FormatStrikethrough(ToolbarButton button)
        {
            Formatter.SetSelection("~~", "~~");
        }

        public override void FormatLink(ToolbarButton button, string label, string formattedText, string link)
        {
            var select = Formatter.Selected;
            int originalStart = Formatter.Selected.StartPosition;

            if (string.IsNullOrWhiteSpace(label))
            {
                if (!string.IsNullOrWhiteSpace(link))
                {
                    Formatter.SetSelection($"[{Formatter.Model.Labels.LabelLabel}](", ")", false, link);
                    select.StartPosition = select.EndPosition;
                    select.EndPosition = select.StartPosition;
                }
                else
                {
                    string startChars = $"[{Formatter.Model.Labels.LabelLabel}](";
                    string filler = Formatter.Model.Labels.UrlLabel;
                    Formatter.SetSelection(startChars, ")", false, filler);
                    select.StartPosition = originalStart + startChars.Length;
                    select.EndPosition = select.StartPosition + filler.Length;
                }
            }
            else if (string.IsNullOrWhiteSpace(link))
            {
                Formatter.SetSelection("[", $"]({Formatter.Model.Labels.UrlLabel})", false, label);
            }
            else
            {
                select.Text = $"[{label}]({link})";
                select.StartPosition = select.EndPosition;
                select.EndPosition = select.StartPosition;
            }
        }

        public override void FormatList(ToolbarButton button)
        {
            Formatter.SetList(() => "- ", button);
        }

        public override void FormatOrderedList(ToolbarButton button)
        {
            Formatter.SetList(Formatter.OrderedListIterate, button);
        }

        public MarkDownFormatter Formatter { get; }
    }
}