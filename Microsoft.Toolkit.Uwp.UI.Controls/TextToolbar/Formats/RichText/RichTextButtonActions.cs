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

namespace Microsoft.Toolkit.Uwp.UI.Controls.TextToolbarFormats.RichText
{
    using System;
    using Microsoft.Toolkit.Uwp.UI.Controls.TextToolbarButtons;
    using Windows.UI.Text;

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
            throw new NotImplementedException();
        }

        public override void FormatList(ToolbarButton button)
        {
            throw new NotImplementedException();
        }

        public override void FormatOrderedList(ToolbarButton button)
        {
            throw new NotImplementedException();
        }

        public RichTextFormatter Formatter { get; }
    }
}