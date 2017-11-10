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

<<<<<<< HEAD
        /// <summary>
        /// Gets the <see cref="ToolbarButton"/> for Bold
        /// </summary>
=======
>>>>>>> fb2912293936b8803e6224af5086e6d0c8780bcd
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

<<<<<<< HEAD
        /// <summary>
        /// Gets the <see cref="ToolbarButton"/> for Italics
        /// </summary>
=======
>>>>>>> fb2912293936b8803e6224af5086e6d0c8780bcd
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

<<<<<<< HEAD
        /// <summary>
        /// Gets the <see cref="ToolbarButton"/> for Strikethrough
        /// </summary>
=======
>>>>>>> fb2912293936b8803e6224af5086e6d0c8780bcd
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

<<<<<<< HEAD
        /// <summary>
        /// Gets the <see cref="ToolbarButton"/> for Link
        /// </summary>
=======
>>>>>>> fb2912293936b8803e6224af5086e6d0c8780bcd
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

<<<<<<< HEAD
        /// <summary>
        /// Gets the <see cref="ToolbarButton"/> for List
        /// </summary>
=======
>>>>>>> fb2912293936b8803e6224af5086e6d0c8780bcd
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

<<<<<<< HEAD
        /// <summary>
        /// Gets the <see cref="ToolbarButton"/> for OrderedList
        /// </summary>
=======
>>>>>>> fb2912293936b8803e6224af5086e6d0c8780bcd
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