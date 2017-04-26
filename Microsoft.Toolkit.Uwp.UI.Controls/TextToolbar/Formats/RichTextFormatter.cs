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
    using Microsoft.Toolkit.Uwp.UI.Controls.TextToolbarButtons;
    using Windows.System;
    using Windows.UI.Text;
    using Windows.UI.Xaml.Controls;

    // Rudimentary showcase of RichText and Toggleable Toolbar Buttons, requires a detection of what current formatting the selected Text has, when the selection changes, and then reflecting that in the ToggleState of the Button.
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
                string currentvalue = string.Empty;
                Model.Editor.Document.GetText(TextGetOptions.FormatRtf, out currentvalue);
                return currentvalue;
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
                    Underline,

                    new ToolbarSeparator(),

                    Model.CommonButtons.Strikethrough,
                    new ToolbarButton { Content = "WIP" }
                };
            }
        }

        public ToolbarButton Underline
        {
            get
            {
                return new ToolbarButton
                {
                    ToolTip = Model.Labels.UnderlineLabel,
                    Icon = new SymbolIcon { Symbol = Symbol.Underline },
                    ShortcutKey = VirtualKey.U,
                    Activation = FormatUnderline
                };
            }
        }

        public ITextCharacterFormat SelectionFormat
        {
            get { return Select.CharacterFormat; }
            set { Select.CharacterFormat = value; }
        }

        public override void FormatBold(ToolbarButton button)
        {
            button.IsToggleable = true;
            button.IsToggled = true;

            var format = SelectionFormat;
            format.Bold = FormatEffect.On;
            SelectionFormat = format;

            button.ToggleEnded += (s, e) =>
            {
                var finishedFormat = SelectionFormat;
                format.Bold = FormatEffect.Off;
                SelectionFormat = format;
            };
        }

        public override void FormatItalics(ToolbarButton button)
        {
            button.IsToggleable = true;
            button.IsToggled = true;

            var format = SelectionFormat;
            format.Italic = FormatEffect.On;
            SelectionFormat = format;

            button.ToggleEnded += (s, e) =>
            {
                var finishedFormat = SelectionFormat;
                format.Italic = FormatEffect.Off;
                SelectionFormat = format;
            };
        }

        public void FormatUnderline(ToolbarButton button)
        {
            button.IsToggleable = true;
            button.IsToggled = true;

            var format = SelectionFormat;
            format.Underline = UnderlineType.Single;
            SelectionFormat = format;

            button.ToggleEnded += (s, e) =>
            {
                var finishedFormat = SelectionFormat;
                format.Underline = UnderlineType.None;
                SelectionFormat = format;
            };
        }

        public override void FormatStrikethrough(ToolbarButton button)
        {
            button.IsToggleable = true;
            button.IsToggled = true;

            var format = SelectionFormat;
            format.Strikethrough = FormatEffect.On;
            SelectionFormat = format;

            button.ToggleEnded += (s, e) =>
            {
                var finishedFormat = SelectionFormat;
                format.Strikethrough = FormatEffect.Off;
                SelectionFormat = format;
            };
        }

        public override void FormatLink(ToolbarButton button, string label, string link)
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
    }
}