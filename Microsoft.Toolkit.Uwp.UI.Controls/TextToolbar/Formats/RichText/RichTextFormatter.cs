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
using Microsoft.Toolkit.Uwp.UI.Controls.TextToolbarButtons.Common;
using Windows.System;
using Windows.UI.Text;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.UI.Controls.TextToolbarFormats.RichText
{
    // Rudimentary showcase of RichText and Toggleable Toolbar Buttons.
    public class RichTextFormatter : Formatter
    {
        public RichTextFormatter(TextToolbar model)
            : base(model)
        {
            CommonButtons = new CommonButtons(model);
            ButtonActions = new RichTextButtonActions(this);
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

            switch (Selected.ParagraphFormat.ListType)
            {
                case MarkerType.Bullet:
                    ListButton.IsToggled = true;
                    OrderedListButton.IsToggled = false;
                    break;

                default:
                    OrderedListButton.IsToggled = true;
                    ListButton.IsToggled = false;
                    break;

                case MarkerType.Undefined:
                case MarkerType.None:
                    ListButton.IsToggled = false;
                    OrderedListButton.IsToggled = false;
                    break;
            }

            base.OnSelectionChanged();
        }

        private CommonButtons CommonButtons { get; }

        public override string Text
        {
            get
            {
                string currentvalue = string.Empty;
                Model.Editor.Document.GetText(TextGetOptions.FormatRtf, out currentvalue);
                return currentvalue;
            }
        }

        internal ToolbarButton BoldButton { get; set; }

        internal ToolbarButton ItalicButton { get; set; }

        internal ToolbarButton StrikeButton { get; set; }

        internal ToolbarButton Underline { get; set; }

        internal ToolbarButton ListButton { get; set; }

        internal ToolbarButton OrderedListButton { get; set; }

        public override ButtonMap DefaultButtons
        {
            get
            {
                BoldButton = CommonButtons.Bold;
                ItalicButton = CommonButtons.Italics;
                StrikeButton = CommonButtons.Strikethrough;
                Underline = new ToolbarButton
                {
                    ToolTip = Model.Labels.UnderlineLabel,
                    Icon = new SymbolIcon { Symbol = Symbol.Underline },
                    ShortcutKey = VirtualKey.U,
                    Activation = ((RichTextButtonActions)ButtonActions).FormatUnderline
                };
                ListButton = CommonButtons.List;
                OrderedListButton = CommonButtons.OrderedList;

                return new ButtonMap
                {
                    BoldButton,
                    ItalicButton,
                    Underline,

                    new ToolbarSeparator(),

                    CommonButtons.Link,
                    StrikeButton,

                    new ToolbarSeparator(),

                    ListButton,
                    OrderedListButton
                };
            }
        }

        public ITextCharacterFormat SelectionFormat
        {
            get { return Selected.CharacterFormat; }
            set { Selected.CharacterFormat = value; }
        }

        public override string NewLineChars => "\r\n";
    }
}