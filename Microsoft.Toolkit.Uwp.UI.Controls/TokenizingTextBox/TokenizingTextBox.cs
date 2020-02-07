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
    [TemplatePart(Name = PART_NormalState, Type = typeof(VisualState))]
    [TemplatePart(Name = PART_PointerOverState, Type = typeof(VisualState))]
    [TemplatePart(Name = PART_FocusedState, Type = typeof(VisualState))]
    [TemplatePart(Name = PART_UnfocusedState, Type = typeof(VisualState))]
    public partial class TokenizingTextBox : ListViewBase
    {
        private const string PART_AutoSuggestBox = "PART_AutoSuggestBox";
        private const string PART_NormalState = "Normal";
        private const string PART_PointerOverState = "PointerOver";
        private const string PART_FocusedState = "Focused";
        private const string PART_UnfocusedState = "Unfocused";

        private AutoSuggestBox _autoSuggestBox;
        private TextBox _autoSuggestTextBox;

        private bool CaretAtStart => _autoSuggestTextBox?.SelectionStart == 0 && Items.Count > 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="TokenizingTextBox"/> class.
        /// </summary>
        public TokenizingTextBox()
        {
            DefaultStyleKey = typeof(TokenizingTextBox);

            // TODO: Do we want to support ItemsSource better? Need to investigate how that works with adding...
            ////RegisterPropertyChangedCallback(ItemsSourceProperty, ItemsSource_PropertyChanged);

            PreviewKeyDown += this.TokenizingTextBox_PreviewKeyDown;
        }

        private void TokenizingTextBox_PreviewKeyDown(object sender, KeyRoutedEventArgs e)
        {
            switch (e.Key)
            {
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

        private void OnASBLoaded(object sender, RoutedEventArgs e)
        {
            if (_autoSuggestTextBox != null)
            {
                _autoSuggestTextBox.PreviewKeyDown -= this.AutoSuggestTextBox_PreviewKeyDown;
            }

            _autoSuggestTextBox = _autoSuggestBox.FindDescendant<TextBox>() as TextBox;
            _autoSuggestTextBox.PreviewKeyDown += this.AutoSuggestTextBox_PreviewKeyDown;
        }

        private void AutoSuggestTextBox_PreviewKeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (CaretAtStart && 
                (e.Key == VirtualKey.Back ||
                 e.Key == VirtualKey.Left))
            {
                // Select last token item (if there is one)
                FocusManager.TryMoveFocus(FocusNavigationDirection.Previous);

                // Clear the selection content
                _autoSuggestTextBox.SelectedText = string.Empty;
                e.Handled = true;
            }
        }

        /// <inheritdoc/>
        protected override DependencyObject GetContainerForItemOverride() => new TokenizingTextBoxItem();

        /// <inheritdoc/>
        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is TokenizingTextBoxItem;
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
                _autoSuggestBox.PointerEntered -= AutoSuggestBox_PointerEntered;
                _autoSuggestBox.PointerExited -= AutoSuggestBox_PointerExited;
                _autoSuggestBox.PointerCanceled -= AutoSuggestBox_PointerExited;
                _autoSuggestBox.PointerCaptureLost -= AutoSuggestBox_PointerExited;
                _autoSuggestBox.GotFocus -= AutoSuggestBox_GotFocus;
                _autoSuggestBox.LostFocus -= AutoSuggestBox_LostFocus;
            }

            _autoSuggestBox = (AutoSuggestBox)GetTemplateChild(PART_AutoSuggestBox);

            if (_autoSuggestBox != null)
            {
                _autoSuggestBox.Loaded += OnASBLoaded;

                _autoSuggestBox.QuerySubmitted += AutoSuggestBox_QuerySubmitted;
                _autoSuggestBox.SuggestionChosen += AutoSuggestBox_SuggestionChosen;
                _autoSuggestBox.TextChanged += AutoSuggestBox_TextChanged;
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
            string t = sender.Text.Trim();
            Text = t;
            TextChanged?.Invoke(sender, args);

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

        /// <inheritdoc/>
        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            base.PrepareContainerForItemOverride(element, item);

            var tokenitem = element as TokenizingTextBoxItem;

            tokenitem.ContentTemplateSelector = TokenItemTemplateSelector;
            tokenitem.ContentTemplate = TokenItemTemplate;
            tokenitem.Style = TokenItemStyle;

            tokenitem.ClearClicked -= TokenizingTextBoxItem_ClearClicked;
            tokenitem.ClearClicked += TokenizingTextBoxItem_ClearClicked;

            var removeMenuItem = new MenuFlyoutItem
            {
                Text = StringExtensions.GetLocalized("WindowsCommunityToolkit_TokenizingTextBoxItem_MenuFlyout_Remove", "Microsoft.Toolkit.Uwp.UI.Controls/Resources")
            };
            removeMenuItem.Click += (s, e) => TokenizingTextBoxItem_ClearClicked(tokenitem, null);

            var menuFlyout = new MenuFlyout();
            menuFlyout.Items.Add(removeMenuItem);
            tokenitem.ContextFlyout = menuFlyout;
        }

        private void TokenizingTextBoxItem_ClearClicked(TokenizingTextBoxItem sender, RoutedEventArgs args)
        {
            bool removeMulti = false;
            foreach (var item in SelectedItems)
            {
                if (item == sender)
                {
                    removeMulti = true;
                    break;
                }
            }

            if (removeMulti)
            {
                foreach (var item in SelectedItems)
                {
                    RemoveToken(item as TokenizingTextBoxItem);
                }
            }
            else
            {
                RemoveToken(sender);
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

            Items.Add(data);

            TokenItemAdded?.Invoke(this, data);
        }

        private void CopySelectedToClipboard()
        {
            if (Items.Count > 0)
            {
                DataPackage dataPackage = new DataPackage();
                dataPackage.RequestedOperation = DataPackageOperation.Copy;

                string tokenString = string.Empty;
                bool addSeparator = false;

                // Copy all items if none selected
                foreach (var item in SelectedItems.Count > 0 ? SelectedItems : Items)
                {
                    if (addSeparator)
                    {
                        tokenString += TokenDelimiter + " ";
                    }
                    else
                    {
                        addSeparator = true;
                    }

                    tokenString += item.ToString();
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

            this.DeselectItem(item); // TODO: Bug? Why do I need this to use Extension?
            Items.Remove(ItemFromContainer(item));
        }

        /// <summary>
        /// Returns the string representation of each token item, concatenated and delimeted.
        /// </summary>
        /// <returns>Untokenized text string</returns>
        public string GetUntokenizedText(string tokenDelimiter = ", ")
        {
            var tokenStrings = new List<string>();
            foreach (var item in Items)
            {
                tokenStrings.Add(item.ToString());
            }

            return string.Join(tokenDelimiter, tokenStrings);
        }
    }
}
