// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Microsoft.Toolkit.Uwp.UI.Controls.TextToolbarButtons;
using Microsoft.Toolkit.Uwp.UI.Controls.TextToolbarButtons.Common;
using Windows.System;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace Microsoft.Toolkit.Uwp.UI.Controls.TextToolbarFormats.RichText
{
    /// <summary>
    /// Rudimentary showcase of RichText and Toggleable Toolbar Buttons.
    /// </summary>
    public class RichTextFormatter : Formatter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RichTextFormatter"/> class.
        /// </summary>
        /// <param name="model">The <see cref="TextToolbar"/></param>
        public RichTextFormatter(TextToolbar model)
            : base(model)
        {
            CommonButtons = new CommonButtons(model);
            ButtonActions = new RichTextButtonActions(this);
        }

        /// <summary>
        /// Invoked the flyout to change the font size
        /// </summary>
        /// <param name="button">The button pressed</param>
        public void FormatFontSize(ToolbarButton button)
        {
            FontSizeButton.IsToggled = true;

            if (fontSizeList == null)
            {
                fontSizeList = new ListBox { Margin = new Thickness(0), Padding = new Thickness(0), SelectionMode = SelectionMode.Single };

                var flyoutPresenterStyle = new Style(typeof(FlyoutPresenter));
                flyoutPresenterStyle.Setters.Add(new Setter(FrameworkElement.MaxHeightProperty, 300));
                fontSizeFlyout = new Flyout { Content = fontSizeList, FlyoutPresenterStyle = flyoutPresenterStyle };

                var fontSizes = new double[] { 8, 9, 10, 10.5, 11, 12, 14, 16, 18, 20, 24, 26, 28, 36, 48, 72 };

                foreach (double i in fontSizes)
                {
                    var item = new ListBoxItem
                    {
                        Content = new MarkdownTextBlock
                        {
                            Text = i.ToString(),
                            IsTextSelectionEnabled = false
                        },
                        Tag = i,
                        Padding = new Thickness(5, 2, 5, 2),
                        Margin = new Thickness(0)
                    };

                    item.Tapped += FontSizeSelected;
                    fontSizeList.Items.Add(item);
                }
            }

            // Matching most common text editor behavior
            // Nothing is selected means no font sizes selected
            // Same font size across the selection means the current font size selected
            // Different font sizes across the selection mean no font sizes selected
            if (Selected.Length == 0)
            {
                fontSizeList.SelectedIndex = -1;
            }
            else
            {
                var fontMatching = false;
                foreach (var listBoxItem in fontSizeList.Items)
                {
                    var item = listBoxItem as FrameworkElement;
                    if (SelectionFormat.Size == Convert.ToSingle(item.Tag))
                    {
                        selectedFontSize = item;
                        fontSizeList.SelectedItem = selectedFontSize;
                        fontMatching = true;
                    }
                    else if (!fontMatching)
                    {
                        fontSizeList.SelectedIndex = -1;
                    }
                }
            }

            fontSizeFlyout.ShowAt(button);
            fontSizeFlyout.Closed += FontSizeFlyout_Closed;
        }

        private void FontSizeSelected(object sender, TappedRoutedEventArgs e)
        {
            var item = sender as FrameworkElement;

            var format = SelectionFormat;
            format.Size = Convert.ToSingle(item.Tag);
            SelectionFormat = format;

            fontSizeFlyout?.Hide();
        }

        private void FontSizeFlyout_Closed(object sender, object e)
        {
            FontSizeButton.IsToggled = false;
        }

        /// <inheritdoc/>
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
                UnderlineButton.IsToggled = true;
            }
            else
            {
                UnderlineButton.IsToggled = false;
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

        /// <inheritdoc/>
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

        internal ToolbarButton FontSizeButton { get; set; }

        internal ToolbarButton StrikeButton { get; set; }

        internal ToolbarButton UnderlineButton { get; set; }

        internal ToolbarButton ListButton { get; set; }

        internal ToolbarButton OrderedListButton { get; set; }

        /// <inheritdoc/>
        public override ButtonMap DefaultButtons
        {
            get
            {
                BoldButton = CommonButtons.Bold;
                ItalicButton = CommonButtons.Italics;
                StrikeButton = CommonButtons.Strikethrough;
                UnderlineButton = new ToolbarButton
                {
                    ToolTip = Model.Labels.UnderlineLabel,
                    Icon = new SymbolIcon { Symbol = Symbol.Underline },
                    ShortcutKey = VirtualKey.U,
                    Activation = ((RichTextButtonActions)ButtonActions).FormatUnderline
                };
                FontSizeButton = new ToolbarButton
                {
                    ToolTip = Model.Labels.FontSizeLabel,
                    Icon = new SymbolIcon { Symbol = Symbol.FontSize },
                    ShortcutKey = VirtualKey.F,
                    Activation = FormatFontSize
                };
                ListButton = CommonButtons.List;
                OrderedListButton = CommonButtons.OrderedList;

                return new ButtonMap
                {
                    BoldButton,
                    ItalicButton,
                    UnderlineButton,
                    FontSizeButton,

                    new ToolbarSeparator(),

                    CommonButtons.Link,
                    StrikeButton,

                    new ToolbarSeparator(),

                    ListButton,
                    OrderedListButton
                };
            }
        }

        /// <summary>
        /// Gets or sets format used for formatting selection in editor
        /// </summary>
        public ITextCharacterFormat SelectionFormat
        {
            get { return Selected.CharacterFormat; }
            set { Selected.CharacterFormat = value; }
        }

        /// <inheritdoc/>
        public override string NewLineChars => "\r\n";

        private FrameworkElement selectedFontSize;

        private ListBox fontSizeList;

        private Flyout fontSizeFlyout;
    }
}