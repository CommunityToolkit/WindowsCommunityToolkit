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

using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Microsoft.Toolkit.Uwp.UI.Controls.TextToolbarButtons.Common
{
    /// <summary>
    /// Provides access to Generic Buttons that activate Formatter Methods
    /// </summary>
    public partial class CommonButtons
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommonButtons"/> class. <para/>
        /// Requires a TextToolbar Instance to Populate from <see cref="TextToolbarStrings"/> Instance.
        /// </summary>
        /// <param name="model">TextToolbar Instance</param>
        public CommonButtons(TextToolbar model)
        {
            Model = model;
        }

        private TextToolbar Model { get; }

        public ToolbarButton Bold
        {
            get
            {
                return new ToolbarButton
                {
                    Name = TextToolbar.BoldElement,
                    ToolTip = Model.Labels.BoldLabel,
                    Icon = new SymbolIcon { Symbol = Symbol.Bold },
                    ShortcutKey = VirtualKey.B,
                    Activation = MakeBold
                };
            }
        }

        public ToolbarButton Italics
        {
            get
            {
                return new ToolbarButton
                {
                    Name = TextToolbar.ItalicsElement,
                    ToolTip = Model.Labels.ItalicsLabel,
                    Icon = new SymbolIcon { Symbol = Symbol.Italic },
                    ShortcutKey = VirtualKey.I,
                    Activation = MakeItalics
                };
            }
        }

        public ToolbarButton Strikethrough
        {
            get
            {
                return new ToolbarButton
                {
                    Name = TextToolbar.StrikethoughElement,
                    ToolTip = Model.Labels.StrikethroughLabel,
                    Icon = new FontIcon { Glyph = "\u0335a\u0335b\u0335c\u0335", FontFamily = new FontFamily("Segoe UI"), Margin = new Thickness(0, -5, 0, 0) },
                    Activation = MakeStrike,
                    ShortcutKey = VirtualKey.Subtract,
                    ShortcutFancyName = "-"
                };
            }
        }

        public ToolbarButton Link
        {
            get
            {
                return new ToolbarButton
                {
                    Name = TextToolbar.LinkElement,
                    ToolTip = Model.Labels.LinkLabel,
                    Icon = new SymbolIcon { Symbol = Symbol.Link },
                    ShortcutKey = VirtualKey.K,
                    Activation = OpenLinkCreator,
                    ShiftActivation = MakeLink
                };
            }
        }

        public ToolbarButton List
        {
            get
            {
                return new ToolbarButton
                {
                    Name = TextToolbar.ListElement,
                    ToolTip = Model.Labels.ListLabel,
                    Content = new TextToolbarSymbols.List(),
                    Activation = MakeList
                };
            }
        }

        public ToolbarButton OrderedList
        {
            get
            {
                return new ToolbarButton
                {
                    Name = TextToolbar.OrderedElement,
                    ToolTip = Model.Labels.OrderedListLabel,
                    Content = new TextToolbarSymbols.NumberedList(),
                    Activation = MakeOList
                };
            }
        }
    }
}