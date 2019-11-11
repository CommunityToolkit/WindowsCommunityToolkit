// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Toolkit.Uwp.Deferred;
using Microsoft.Toolkit.Uwp.Extensions;
using Microsoft.Toolkit.Uwp.UI.Extensions;
using Windows.ApplicationModel.DataTransfer;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.WebUI;
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
        private TextBox _autoSuggestTextBox;

        /// <summary>
        /// Initializes a new instance of the <see cref="TokenizingTextBox"/> class.
        /// </summary>
        public TokenizingTextBox()
        {
            DefaultStyleKey = typeof(TokenizingTextBox);
            TokenizedItemsInternal.Clear();
        }

        private void OnASBLoaded(object sender, RoutedEventArgs e)
        {
            if (_autoSuggestTextBox != null)
            {
                _autoSuggestTextBox.PreviewKeyDown -= this.AutoSuggestTextBox_PreviewKeyDown;
            }

            _autoSuggestTextBox = _autoSuggestBox.FindDescendant<TextBox>() as TextBox;
            _autoSuggestTextBox.PreviewKeyDown += this.AutoSuggestTextBox_PreviewKeyDown;
        }

        private async void AutoSuggestTextBox_PreviewKeyDown(object sender, KeyRoutedEventArgs e)
        {
            int currentCursorPosition = _autoSuggestTextBox.SelectionStart;
            if (currentCursorPosition == 0 && e.Key == VirtualKey.Back && TokenizedItemsInternal.Count > 0)
            {
                // The last item is the AutoSuggestBox. Get the second to last
                UIElement itemToFocus = _wrapPanel.Children[_wrapPanel.Children.Count - 2];

                // And set focus to it
                await FocusManager.TryFocusAsync(itemToFocus, FocusState.Keyboard);
                e.Handled = true;
            }
        }

        /// <inheritdoc/>
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (_autoSuggestBox != null)
            {
                _autoSuggestBox.Loaded -= OnASBLoaded;

                _autoSuggestBox.QuerySubmitted -= AutoSuggestBox_QuerySubmitted;
                _autoSuggestBox.SuggestionChosen -= AutoSuggestBox_SuggestionChosen;
                _autoSuggestBox.TextChanged -= AutoSuggestBox_TextChanged;
                _autoSuggestBox.KeyDown -= AutoSuggestBox_KeyDown;
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
                _autoSuggestBox.Loaded += OnASBLoaded;

                _autoSuggestBox.QuerySubmitted += AutoSuggestBox_QuerySubmitted;
                _autoSuggestBox.SuggestionChosen += AutoSuggestBox_SuggestionChosen;
                _autoSuggestBox.TextChanged += AutoSuggestBox_TextChanged;
                _autoSuggestBox.KeyDown += AutoSuggestBox_KeyDown;
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

        private void AutoSuggestBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            switch (e.Key)
            {
                case VirtualKey.Left:
                    break;

                case VirtualKey.Right:
                    break;

                case VirtualKey.Up:
                    break;

                case VirtualKey.Down:
                    break;

                case VirtualKey.C:
                    {
                        var controlPressed = CoreWindow.GetForCurrentThread().GetKeyState(VirtualKey.Control).HasFlag(CoreVirtualKeyStates.Down);
                        if (controlPressed)
                        {
                            CopySelectedToClipboard();
                        }

                        break;
                    }
            }
        }

        private async void AutoSuggestBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            QuerySubmitted?.Invoke(sender, args);

            if (args.ChosenSuggestion != null)
            {
                sender.Text = string.Empty;
                await AddToken(args.ChosenSuggestion);
                sender.Focus(FocusState.Programmatic);
            }
            else if (!string.IsNullOrWhiteSpace(args.QueryText))
            {
                sender.Text = string.Empty;
                await AddToken(args.QueryText);
                sender.Focus(FocusState.Programmatic);
            }
        }

        private void AutoSuggestBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            SuggestionChosen?.Invoke(sender, args);
        }

        private void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            TextChanged?.Invoke(sender, args);

            string t = sender.Text.Trim();

            if (!string.IsNullOrEmpty(TokenDelimiter) && t.Contains(TokenDelimiter))
            {
                bool lastDelimited = t[t.Length - 1] == TokenDelimiter[0];

                string[] tokens = t.Split(TokenDelimiter);
                int numberToProcess = lastDelimited ? tokens.Length : tokens.Length - 1;
                for (int position = 0; position < numberToProcess; position++)
                {
                    string token = tokens[position];
                    token = token.Trim();
                    if (token.Length > 0)
                    {
                        _ = AddToken(token);
                    }
                }

                if (lastDelimited)
                {
                    sender.Text = string.Empty;
                }
                else
                {
                    sender.Text = tokens[tokens.Length - 1];
                }
            }
        }

        private void TokenizingTextBoxItem_ClearClicked(TokenizingTextBoxItem sender, RoutedEventArgs args)
        {
            bool removeMulti = false;
            foreach (var item in SelectedItemsInternal)
            {
                if (item == sender)
                {
                    removeMulti = true;
                    break;
                }
            }

            if (removeMulti)
            {
                while (SelectedItemsInternal.Count > 0)
                {
                    var b = SelectedItemsInternal[0] as TokenizingTextBoxItem;
                    RemoveToken(b);
                }
            }
            else
            {
                RemoveToken(sender);
            }
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

                    SelectedItemsInternal.Clear();
                }

                item.IsSelected = !item.IsSelected;

                if (item.IsSelected)
                {
                    SelectedItemsInternal.Add(item);
                }
                else
                {
                    SelectedItemsInternal.Remove(item);
                }

                TokenItemClicked?.Invoke(this, item); // TODO: Do we want to use EventArgs here to have the OriginalSource like ItemClickEventArgs?
            }
        }

        private async Task AddToken(object data)
        {
            if (data is string str && TokenItemCreating != null)
            {
                var ticea = new TokenItemCreatingEventArgs(str);
                await TokenItemCreating.InvokeAsync(this, ticea);

                if (ticea.Cancel)
                {
                    return;
                }

                if (ticea.Item != null)
                {
                    data = ticea.Item; // Transformed by event implementor
                }
            }

            var item = new TokenizingTextBoxItem()
            {
                Content = data,
                ContentTemplateSelector = TokenItemTemplateSelector,
                ContentTemplate = TokenItemTemplate,
                Style = TokenItemStyle
            };
            item.Click += TokenizingTextBoxItem_Click; // TODO: Wonder if this needs to be in a PrepareContainerForItemOverride?
            item.ClearClicked += TokenizingTextBoxItem_ClearClicked;
            item.KeyUp += TokenizingTextBoxItem_KeyUp;

            var removeMenuItem = new MenuFlyoutItem
            {
                Text = StringExtensions.GetLocalized("WindowsCommunityToolkit_TokenizingTextBoxItem_MenuFlyout_Remove", "Microsoft.Toolkit.Uwp.UI.Controls/Resources")
            };
            removeMenuItem.Click += (s, e) => TokenizingTextBoxItem_ClearClicked(item, null);

            var menuFlyout = new MenuFlyout();
            menuFlyout.Items.Add(removeMenuItem);
            item.ContextFlyout = menuFlyout;

            var i = _wrapPanel.Children.Count - 1;
            _wrapPanel.Children.Insert(i, item);

            TokenizedItemsInternal.Add(item);

            TokenItemAdded?.Invoke(this, item);
        }

        private void TokenizingTextBoxItem_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            TokenizingTextBoxItem ttbi = sender as TokenizingTextBoxItem;

            switch (e.Key)
            {
                case VirtualKey.Left:
                    {
                        FocusManager.TryMoveFocus(FocusNavigationDirection.Left);
                        break;
                    }

                case VirtualKey.Right:
                    {
                        FocusManager.TryMoveFocus(FocusNavigationDirection.Right);
                        break;
                    }

                case VirtualKey.Up:
                    {
                        FocusManager.TryMoveFocus(FocusNavigationDirection.Up);
                        break;
                    }

                case VirtualKey.Down:
                    {
                        FocusManager.TryMoveFocus(FocusNavigationDirection.Down);
                        break;
                    }

                case VirtualKey.Space:
                    {
                        ttbi.IsSelected = !ttbi.IsSelected;
                        break;
                    }

                case VirtualKey.C:
                    {
                        var controlPressed = CoreWindow.GetForCurrentThread().GetKeyState(VirtualKey.Control).HasFlag(CoreVirtualKeyStates.Down);
                        if (controlPressed)
                        {
                            CopySelectedToClipboard();
                        }

                        break;
                    }
            }
        }

        private void CopySelectedToClipboard()
        {
            if (SelectedItemsInternal.Count > 0)
            {
                DataPackage dataPackage = new DataPackage();
                dataPackage.RequestedOperation = DataPackageOperation.Copy;

                string tokenString = string.Empty;
                bool addSeparator = false;
                foreach (TokenizingTextBoxItem item in SelectedItemsInternal)
                {
                    if (addSeparator)
                    {
                        tokenString += TokenDelimiter + " ";
                    }
                    else
                    {
                        addSeparator = true;
                    }

                    tokenString += item.Content;
                }

                dataPackage.SetText(tokenString);
                Clipboard.SetContent(dataPackage);
            }
        }

        private void RemoveToken(TokenizingTextBoxItem item)
        {
            var tirea = new TokenItemRemovedEventArgs(item?.Content, item);
            TokenItemRemoved?.Invoke(this, tirea);

            if (tirea.Cancel)
            {
                return;
            }

            SelectedItemsInternal.Remove(item);
            TokenizedItemsInternal.Remove(item);

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
                    SelectedItemsInternal.Add(item);
                }
            }
        }

        /// <summary>
        /// Returns the string representation of each token item, concatenated and delimeted.
        /// </summary>
        /// <returns>Untokenized text string</returns>
        public string GetUntokenizedText(string tokenDelimiter = ", ")
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
