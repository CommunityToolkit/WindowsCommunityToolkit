// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using Microsoft.Toolkit.Uwp.Extensions;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// A text input control that auto-suggests and displays token items.
    /// </summary>
    [TemplatePart(Name = PART_AutoSuggestBox, Type = typeof(AutoSuggestBox))]
    [TemplatePart(Name = PART_WrapPanel, Type = typeof(WrapPanel))]
    [TemplatePart(Name = PART_NormalState, Type = typeof(VisualState))]
    [TemplatePart(Name = PART_PointerOverState, Type = typeof(VisualState))]
    [TemplatePart(Name = PART_FocusedState, Type = typeof(VisualState))]
    [TemplatePart(Name = PART_UnfocusedState, Type = typeof(VisualState))]
    public partial class TokenizingTextBox : Control
    {
        private const string PART_AutoSuggestBox = "PART_AutoSuggestBox";
        private const string PART_WrapPanel = "PART_WrapPanel";
        private const string PART_NormalState = "Normal";
        private const string PART_PointerOverState = "PointerOver";
        private const string PART_FocusedState = "Focused";
        private const string PART_UnfocusedState = "Unfocused";

        private AutoSuggestBox _autoSuggestBox;
        private WrapPanel _wrapPanel;

        /// <summary>
        /// Initializes a new instance of the <see cref="TokenizingTextBox"/> class.
        /// </summary>
        public TokenizingTextBox()
        {
            DefaultStyleKey = typeof(TokenizingTextBox);
        }

        /// <inheritdoc/>
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (_autoSuggestBox != null)
            {
                _autoSuggestBox.QuerySubmitted -= AutoSuggestBox_QuerySubmitted;
                _autoSuggestBox.SuggestionChosen -= AutoSuggestBox_SuggestionChosen;
                _autoSuggestBox.TextChanged -= AutoSuggestBox_TextChanged;
                _autoSuggestBox.KeyDown -= AutoSuggestBox_KeyDown;
                _autoSuggestBox.CharacterReceived -= AutoSuggestBox_CharacterReceived;
                _autoSuggestBox.PointerEntered -= AutoSuggestBox_PointerEntered;
                _autoSuggestBox.PointerExited -= AutoSuggestBox_PointerExited;
                _autoSuggestBox.PointerCanceled -= AutoSuggestBox_PointerExited;
                _autoSuggestBox.PointerCaptureLost -= AutoSuggestBox_PointerExited;
                _autoSuggestBox.GotFocus -= AutoSuggestBox_GotFocus;
                _autoSuggestBox.LostFocus -= AutoSuggestBox_LostFocus;
            }

            _autoSuggestBox = (AutoSuggestBox)GetTemplateChild(PART_AutoSuggestBox);
            _wrapPanel = (WrapPanel)GetTemplateChild(PART_WrapPanel);

            if (_autoSuggestBox != null)
            {
                _autoSuggestBox.QuerySubmitted += AutoSuggestBox_QuerySubmitted;
                _autoSuggestBox.SuggestionChosen += AutoSuggestBox_SuggestionChosen;
                _autoSuggestBox.TextChanged += AutoSuggestBox_TextChanged;
                _autoSuggestBox.KeyDown += AutoSuggestBox_KeyDown;
                _autoSuggestBox.CharacterReceived += AutoSuggestBox_CharacterReceived;
                _autoSuggestBox.PointerEntered += AutoSuggestBox_PointerEntered;
                _autoSuggestBox.PointerExited += AutoSuggestBox_PointerExited;
                _autoSuggestBox.PointerCanceled += AutoSuggestBox_PointerExited;
                _autoSuggestBox.PointerCaptureLost += AutoSuggestBox_PointerExited;
                _autoSuggestBox.GotFocus += AutoSuggestBox_GotFocus;
                _autoSuggestBox.LostFocus += AutoSuggestBox_LostFocus;
            }

            var selectAllMenuItem = new MenuFlyoutItem
            {
                Text = StringExtensions.GetLocalized("WindowsCommunityToolkit_TokenizingTextBox_MenuFlyout_SelectAll", "Microsoft.Toolkit.Uwp.UI.Controls/Resources")
            };
            selectAllMenuItem.Click += (s, e) => SelectAll();
            var menuFlyout = new MenuFlyout();
            menuFlyout.Items.Add(selectAllMenuItem);
            ContextFlyout = menuFlyout;
        }

        private void AutoSuggestBox_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, PART_PointerOverState, true);
        }

        private void AutoSuggestBox_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, PART_NormalState, true);
        }

        private void AutoSuggestBox_LostFocus(object sender, RoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, PART_UnfocusedState, true);
        }

        private void AutoSuggestBox_GotFocus(object sender, RoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, PART_FocusedState, true);
        }

        private void AutoSuggestBox_CharacterReceived(UIElement sender, CharacterReceivedRoutedEventArgs args)
        {
            if (args.Character.ToString() == TokenDelimiter && sender is AutoSuggestBox autoSuggestBox)
            {
                AddToken(autoSuggestBox.Text);
                autoSuggestBox.Text = string.Empty;
                autoSuggestBox.Focus(FocusState.Programmatic);
            }
        }

        private void AutoSuggestBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            switch (e.Key)
            {
                case VirtualKey.Back:
                case VirtualKey.Delete:

                    if (_autoSuggestBox.Text != string.Empty || _wrapPanel.Children.Count <= 1)
                    {
                        break;
                    }

                    // The last item is the AutoSuggestBox. Get the second to last.
                    var lastTokenIndex = _wrapPanel.Children.Count - 2;
                    var lastToken = _wrapPanel.Children[lastTokenIndex];
                    RemoveToken(lastToken as TokenizingTextBoxItem);
                    break;

                case VirtualKey.Left:
                    break;

                case VirtualKey.Right:
                    break;

                case VirtualKey.Up:
                    break;

                case VirtualKey.Down:
                    break;
            }
        }

        private void AutoSuggestBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            if (args.QueryText != string.Empty)
            {
                AddToken(args.QueryText);
                sender.Text = string.Empty;
                sender.Focus(FocusState.Programmatic);
            }

            QuerySubmitted?.Invoke(sender, args);
        }

        private void AutoSuggestBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            AddToken(args.SelectedItem);
            sender.Text = string.Empty;
            sender.Focus(FocusState.Programmatic);
            SuggestionChosen?.Invoke(sender, args);
        }

        private void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            QueryTextChanged?.Invoke(sender, args);
        }

        private void TokenizingTextBoxItem_ClearClicked(TokenizingTextBoxItem sender, RoutedEventArgs args)
        {
            RemoveToken(sender);
        }

        private void TokenizingTextBoxItem_Click(object sender, RoutedEventArgs e)
        {
            if (sender is TokenizingTextBoxItem item)
            {
                bool isControlDown = CoreWindow.GetForCurrentThread().GetKeyState(VirtualKey.Control).HasFlag(CoreVirtualKeyStates.Down);
                if (!item.IsSelected && !isControlDown)
                {
                    foreach (var child in _wrapPanel.Children)
                    {
                        if (child is TokenizingTextBoxItem childItem)
                        {
                            childItem.IsSelected = false;
                        }
                    }

                    SelectedItems.Clear();
                }

                item.IsSelected = !item.IsSelected;

                var clickedItem = item.Content;

                if (item.IsSelected)
                {
                    SelectedItems.Add(clickedItem);
                }
                else
                {
                    SelectedItems.Remove(clickedItem);
                }

                TokenItemClicked?.Invoke(this, item); // TODO: Do we want to use EventArgs here to have the OriginalSource like ItemClickEventArgs?
            }
        }

        private void AddToken(object data)
        {
            var item = new TokenizingTextBoxItem()
            {
                Content = data,
                ContentTemplateSelector = TokenItemTemplateSelector,
                ContentTemplate = TokenItemTemplate,
                Style = TokenItemStyle
            };
            item.Click += TokenizingTextBoxItem_Click; // TODO: Wonder if this needs to be in a PrepareContainerForItemOverride?
            item.ClearClicked += TokenizingTextBoxItem_ClearClicked;

            var removeMenuItem = new MenuFlyoutItem
            {
                Text = StringExtensions.GetLocalized("WindowsCommunityToolkit_TokenizingTextBoxItem_MenuFlyout_Remove", "Microsoft.Toolkit.Uwp.UI.Controls/Resources")
            };
            removeMenuItem.Click += (s, e) => RemoveToken(item);
            var menuFlyout = new MenuFlyout();
            menuFlyout.Items.Add(removeMenuItem);
            item.ContextFlyout = menuFlyout;

            var i = _wrapPanel.Children.Count - 1;
            _wrapPanel.Children.Insert(i, item);

            TokenItemAdded?.Invoke(this, item);
        }

        private void RemoveToken(TokenizingTextBoxItem item)
        {
            var tirea = new TokenItemRemovedEventArgs(item?.Content, item);
            TokenItemRemoved?.Invoke(this, tirea);

            if (tirea.Cancel)
            {
                return;
            }

            SelectedItems.Remove(item.Content);

            var itemIndex = Math.Max(_wrapPanel.Children.IndexOf(item) - 1, 0);
            _wrapPanel.Children.Remove(item);

            if (_wrapPanel.Children[itemIndex] is Control control)
            {
                control.Focus(FocusState.Programmatic);
            }
        }

        private void SelectAll()
        {
            foreach (var child in _wrapPanel.Children)
            {
                if (child is TokenizingTextBoxItem item)
                {
                    item.IsSelected = true;
                    SelectedItems.Add(item.Content);
                }
            }
        }

        /// <summary>
        /// Returns the string representation of each token item, concatenated and delimeted.
        /// </summary>
        /// <returns>Untokenized text string</returns>
        public string GetUntokenizedText(string tokenDelimiter = " ,")
        {
            var tokenStrings = new List<string>();
            foreach (var child in _wrapPanel.Children)
            {
                if (child is TokenizingTextBoxItem item)
                {
                    tokenStrings.Add(item.Content.ToString());
                }
            }

            return string.Join(tokenDelimiter, tokenStrings);
        }
    }
}
