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
    using Windows.System;
    using Windows.UI.Text;
    using Windows.UI.Xaml.Controls;

    // Rudimentary showcase of RichText and Toggleable Toolbar Buttons.
    public class RichTextFormatter : Formatter
    {
        public RichTextFormatter(TextToolbar model)
            : base(model)
        {
            ButtonActions = new RichTextButtonActions(this);
        }

        public void FormatUnderline(ToolbarButton button)
        {
            var format = SelectionFormat;
            if (!button.IsToggled)
            {
                button.IsToggled = true;
                format.Underline = UnderlineType.Single;
                SelectionFormat = format;
            }
            else
            {
                format.Underline = UnderlineType.None;
                SelectionFormat = format;
            }
        }

        public override void OnSelectionChanged()
        {
            if (Selected.CharacterFormat.Bold == FormatEffect.On)
            {
                BoldButton.IsToggled = true;
            }
            else
            {
                BoldButton.IsToggled = false;
            }

            if (Selected.CharacterFormat.Italic == FormatEffect.On)
            {
                ItalicButton.IsToggled = true;
            }
            else
            {
                ItalicButton.IsToggled = false;
            }

            if (Selected.CharacterFormat.Strikethrough == FormatEffect.On)
            {
                StrikeButton.IsToggled = true;
            }
            else
            {
                StrikeButton.IsToggled = false;
            }

            if (Selected.CharacterFormat.Underline != UnderlineType.None)
            {
                Underline.IsToggled = true;
            }
            else
            {
                Underline.IsToggled = false;
            }

            base.OnSelectionChanged();
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

        private ToolbarButton BoldButton { get; set; }

        private ToolbarButton ItalicButton { get; set; }

        private ToolbarButton StrikeButton { get; set; }

        private ToolbarButton Underline
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

        public override ButtonMap DefaultButtons
        {
            get
            {
                BoldButton = Model.CommonButtons.Bold;
                ItalicButton = Model.CommonButtons.Italics;
                StrikeButton = Model.CommonButtons.Strikethrough;

                return new ButtonMap
                {
                    BoldButton,
                    ItalicButton,
                    Underline,

                    new ToolbarSeparator(),

                    Model.CommonButtons.Link,
                    StrikeButton,
                    new ToolbarButton { Content = "WIP" }
                };
            }
        }

        public ITextCharacterFormat SelectionFormat
        {
            get { return Selected.CharacterFormat; }
            set { Selected.CharacterFormat = value; }
        }
    }
}