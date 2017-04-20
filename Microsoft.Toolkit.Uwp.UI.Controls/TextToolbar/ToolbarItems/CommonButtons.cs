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

namespace Microsoft.Toolkit.Uwp.UI.Controls.TextToolbarButtons
{
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Media;

    /// <summary>
    /// Provides access to Generic Buttons that activate Formatter Methods
    /// </summary>
    public partial class CommonButtons
    {
        internal CommonButtons(TextToolbar model)
        {
            Model = model;
        }

        private TextToolbar Model { get; }

        public ToolbarButton Bold
        {
            get
            {
                var button = new ToolbarButton { Name = TextToolbar.BoldElement, ToolTip = Model.BoldLabel, Icon = new SymbolIcon { Symbol = Symbol.Bold } };
                button.Click += MakeBold;
                return button;
            }
        }

        public ToolbarButton Italics
        {
            get
            {
                var button = new ToolbarButton { Name = TextToolbar.ItalicsElement, ToolTip = Model.ItalicsLabel, Icon = new SymbolIcon { Symbol = Symbol.Italic } };
                button.Click += MakeItalics;
                return button;
            }
        }

        public ToolbarButton Strikethrough
        {
            get
            {
                var button = new ToolbarButton
                {
                    Name = TextToolbar.StrikethoughElement,
                    ToolTip = Model.StrikethroughLabel,
                    Icon = new FontIcon { Glyph = "\u0335a\u0335b\u0335c\u0335", FontFamily = new FontFamily("Segoe UI"), Margin = new Thickness(0, -5, 0, 0) }
                };
                button.Click += MakeStrike;
                return button;
            }
        }

        public ToolbarButton Link
        {
            get
            {
                var button = new ToolbarButton { Name = TextToolbar.LinkElement, ToolTip = Model.LinkLabel, Icon = new SymbolIcon { Symbol = Symbol.Link } };
                button.Click += MakeLink;
                return button;
            }
        }

        public ToolbarButton List
        {
            get
            {
                var button = new ToolbarButton
                {
                    Name = TextToolbar.ListElement,
                    ToolTip = Model.ListLabel,
                    Icon = new FontIcon { Glyph = "\uF0CA", FontFamily = new FontFamily("ms-appx:///Microsoft.Toolkit.Uwp.UI.Controls/TextToolbar/Font/FontAwesome.otf#FontAwesome") }
                };
                button.Click += MakeList;
                return button;
            }
        }

        public ToolbarButton OrderedList
        {
            get
            {
                var button = new ToolbarButton
                {
                    Name = TextToolbar.OrderedElement,
                    ToolTip = Model.OrderedListLabel,
                    Icon = new FontIcon { Glyph = "\uF0CB", FontFamily = new FontFamily("ms-appx:///Microsoft.Toolkit.Uwp.UI.Controls/TextToolbar/Font/FontAwesome.otf#FontAwesome") }
                };
                button.Click += MakeOList;
                return button;
            }
        }
    }
}