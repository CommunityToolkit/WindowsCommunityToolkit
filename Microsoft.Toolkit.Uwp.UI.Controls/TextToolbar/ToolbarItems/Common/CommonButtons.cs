// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

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

        /// <summary>
        /// Gets the <see cref="ToolbarButton"/> for Bold
        /// </summary>
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

        /// <summary>
        /// Gets the <see cref="ToolbarButton"/> for Italics
        /// </summary>
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

        /// <summary>
        /// Gets the <see cref="ToolbarButton"/> for Strikethrough
        /// </summary>
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

        /// <summary>
        /// Gets the <see cref="ToolbarButton"/> for Link
        /// </summary>
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

        /// <summary>
        /// Gets the <see cref="ToolbarButton"/> for List
        /// </summary>
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

        /// <summary>
        /// Gets the <see cref="ToolbarButton"/> for OrderedList
        /// </summary>
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