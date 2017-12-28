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
    /// <summary>
    /// Default button Actions for MarkDown Formatter
    /// </summary>
    public class MarkDownButtonActions : ButtonActions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MarkDownButtonActions"/> class.
        /// </summary>
        /// <param name="formatter">The Formatter to use</param>
        public MarkDownButtonActions(MarkDownFormatter formatter)
        {
            Formatter = formatter;
        }

        /// <inheritdoc/>
        public override void FormatBold(ToolbarButton button)
        {
            Formatter.SetSelection("**", "**");
        }

        /// <inheritdoc/>
        public override void FormatItalics(ToolbarButton button)
        {
            Formatter.SetSelection("_", "_");
        }

        /// <inheritdoc/>
        public override void FormatStrikethrough(ToolbarButton button)
        {
            Formatter.SetSelection("~~", "~~");
        }

        /// <inheritdoc/>
        public override void FormatLink(ToolbarButton button, string label, string formattedText, string link)
        {
            var select = Formatter.Selected;
            int originalStart = Formatter.Selected.StartPosition;

            // Replaces Selection of first Line only.
            if (select.Text.Contains("\r"))
            {
                select.EndPosition = select.Text.IndexOf("\r");
            }

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
                Formatter.SetSelection($"[{label}](", ")", false, Formatter.Model.Labels.UrlLabel);
            }
            else
            {
                select.Text = $"[{label}]({link})";
                select.StartPosition = select.EndPosition;
            }
        }

        /// <inheritdoc/>
        public override void FormatList(ToolbarButton button)
        {
            Formatter.SetList(() => "- ", button);
        }

        /// <inheritdoc/>
        public override void FormatOrderedList(ToolbarButton button)
        {
            Formatter.SetList(Formatter.OrderedListIterate, button);
        }

        /// <summary>
        /// Gets the Formatter used
        /// </summary>
        public MarkDownFormatter Formatter { get; }
    }
}
