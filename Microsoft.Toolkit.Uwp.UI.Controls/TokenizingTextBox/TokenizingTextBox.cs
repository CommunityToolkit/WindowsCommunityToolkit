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
        private bool _pauseTokenClearOnFocus = false;

        /// <summary>
        /// Gets a value indicating whether the textbox caret is in the first position. False otherwise
        /// </summary>
        private bool IsCaretAtStart => _autoSuggestTextBox?.SelectionStart == 0;

        /// <summary>
        /// Gets a value indicating whether all text in the text box is currently selected. False otherwise.
        /// </summary>
        private bool IsAllSelected => _autoSuggestTextBox?.SelectedText == _autoSuggestTextBox?.Text && !string.IsNullOrEmpty(_autoSuggestTextBox?.Text);

        /// <summary>
        /// Initializes a new instance of the <see cref="TokenizingTextBox"/> class.
        /// </summary>
        public TokenizingTextBox()
        {
            DefaultStyleKey = typeof(TokenizingTextBox);

            // TODO: Do we want to support ItemsSource better? Need to investigate how that works with adding...
            ////RegisterPropertyChangedCallback(ItemsSourceProperty, ItemsSource_PropertyChanged);

            PreviewKeyDown += this.TokenizingTextBox_PreviewKeyDown;
            PreviewKeyUp += this.TokenizingTextBox_PreviewKeyUp;
        }

        private void TokenizingTextBox_PreviewKeyUp(object sender, KeyRoutedEventArgs e)
        {
            // Global handlers on control regardless if focused on item or in textbox.
            switch (e.Key)
            {
                case VirtualKey.Escape:
                    {
                        // Clear any selection and place the focus back into the text box
                        this.DeselectAll();

                        // Clear any selection in the text box
                        if (IsAllSelected)
                        {
                            _autoSuggestTextBox.SelectionLength = 0;
                        }

                        // Ensure the focus is in the text box part.
                        _autoSuggestBox?.Focus(FocusState.Programmatic);
                        break;
                    }
            }
        }

        private enum MoveDirection
        {
            Next, Previous
        }

        private void TokenizingTextBox_PreviewKeyDown(object sender, KeyRoutedEventArgs e)
        {
            // Global handlers on control regardless if focused on item or in textbox.
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

                case Windows.System.VirtualKey.Left:
                    e.Handled = MoveFocusAndSelection(MoveDirection.Previous);
                    break;

                case Windows.System.VirtualKey.Right:
                    e.Handled = MoveFocusAndSelection(MoveDirection.Next);
                    break;

                case Windows.System.VirtualKey.Up:
                case Windows.System.VirtualKey.Down:
                    e.Handled = !FocusManager.GetFocusedElement().Equals(_autoSuggestTextBox);
                    break;

                case VirtualKey.A:
                {
                    // modify the select-all behaviour to ensure the text in the edit box gets selected.
                    var controlPressed = CoreWindow.GetForCurrentThread().GetKeyState(VirtualKey.Control).HasFlag(CoreVirtualKeyStates.Down);
                    if (controlPressed)
                    {
                        this.SelectAllTokensAndText();
                        e.Handled = true;
                    }

                    break;
                }
            }
        }

        /// <summary>
        /// Adjust the selected item and range based on keyboard input.
        /// This is used to override the listview behavious for up/down arrow manipulation vs left/right for a horizontal control
        /// </summary>
        /// <param name="direction">direction to move the selection</param>
        /// <returns>True if the focus was moved, false otherwise</returns>
        private bool MoveFocusAndSelection(MoveDirection direction)
        {
            bool retVal = false;
            var currentContainerItem = FocusManager.GetFocusedElement() as TokenizingTextBoxItem;

            if (currentContainerItem != null && !FocusManager.GetFocusedElement().Equals(_autoSuggestTextBox))
            {
                var currentItem = ItemFromContainer(currentContainerItem);
                var previousIndex = Items.IndexOf(currentItem);
                var index = previousIndex;

                if (direction == MoveDirection.Previous)
                {
                    if (previousIndex > 0)
                    {
                        index -= 1;
                    }
                    else
                    {
                        FocusManager.TryMoveFocus(FocusNavigationDirection.Previous);
                        retVal = true;
                    }
                }
                else if (direction == MoveDirection.Next)
                {
                    if (previousIndex < Items.Count - 1)
                    {
                        index += 1;
                    }
                    else
                    {
                        _autoSuggestTextBox.Focus(FocusState.Keyboard);
                        retVal = true;
                    }
                }

                // Only do stuff if the index is actually changing
                if (index != previousIndex)
                {
                    var newItem = ContainerFromIndex(index) as TokenizingTextBoxItem;
                    newItem.Focus(FocusState.Keyboard);

                    // if no control keys are selected then the seleciton also becomes just this item
                    var controlPressed = CoreWindow.GetForCurrentThread().GetKeyState(VirtualKey.Control).HasFlag(CoreVirtualKeyStates.Down);
                    var shiftPressed = CoreWindow.GetForCurrentThread().GetKeyState(VirtualKey.Shift).HasFlag(CoreVirtualKeyStates.Down);

                    if (shiftPressed)
                    {
                        // What we do here depends on where the selection started
                        // if the previous item is between the start and new position then we add the new item to the selected range
                        // if the new item is between the start and the previous position then we remove the previous position
                        int newDistance = Math.Abs(SelectedIndex - index);
                        int oldDistance = Math.Abs(SelectedIndex - previousIndex);

                        if (newDistance > oldDistance)
                        {
                            SelectedItems.Add(Items[index]);
                        }
                        else
                        {
                            SelectedItems.Remove(Items[previousIndex]);
                        }
                    }
                    else if (!controlPressed)
                    {
                        SelectedIndex = index;
                    }

                    retVal = true;
                }
            }

            return retVal;
        }

        private void OnASBLoaded(object sender, RoutedEventArgs e)
        {
            // Local function for Selection changed
            void AutoSuggestTextBox_SelectionChanged(object box, RoutedEventArgs args)
            {
                if (!this.IsAllSelected)
                {
                    this.DeselectAll();
                }
            }

            // local function for clearing selection on interaction with text box
            async void AutoSuggestTextBox_TextChangingAsync(TextBox o, TextBoxTextChangingEventArgs args)
            {
                // remove any selected tokens.
                if (this.SelectedItems.Count > 0)
                {
                    await this.ClearAllSelected();
                }
            }

            if (_autoSuggestTextBox != null)
            {
                _autoSuggestTextBox.PreviewKeyDown -= this.AutoSuggestTextBox_PreviewKeyDown;
                _autoSuggestTextBox.TextChanging -= AutoSuggestTextBox_TextChangingAsync;
                _autoSuggestTextBox.SelectionChanged -= AutoSuggestTextBox_SelectionChanged;
            }

            _autoSuggestTextBox = _autoSuggestBox.FindDescendant<TextBox>() as TextBox;

            if (_autoSuggestTextBox != null)
            {
                _autoSuggestTextBox.PreviewKeyDown += this.AutoSuggestTextBox_PreviewKeyDown;
                _autoSuggestTextBox.TextChanging += AutoSuggestTextBox_TextChangingAsync;
                _autoSuggestTextBox.SelectionChanged += AutoSuggestTextBox_SelectionChanged;
            }
        }

        private void AutoSuggestTextBox_PreviewKeyDown(object sender, KeyRoutedEventArgs e)
        {
            // Handlers for the textbox only
            // Handlers for items in TokenizingTextBoxItem
            if (IsCaretAtStart && !IsAllSelected &&
                (e.Key == VirtualKey.Back ||
                 e.Key == VirtualKey.Left))
            {
                // Select last token item (if there is one)
                if (Items.Count > 0)
                {
                    var item = ContainerFromItem(Items[Items.Count - 1]);
                    SelectedIndex = Items.Count - 1;
                    (item as TokenizingTextBoxItem).Focus(FocusState.Keyboard);

                    // Clear any text box selection
                    _autoSuggestTextBox.SelectionLength = 0;
                }
                else
                {
                    FocusManager.TryMoveFocus(FocusNavigationDirection.Previous);
                }

                e.Handled = true;
            }
            else if (e.Key == VirtualKey.A && CoreWindow.GetForCurrentThread().GetKeyState(VirtualKey.Control).HasFlag(CoreVirtualKeyStates.Down))
            {
                // Need to provide this shortcut from the textbox only, as ListViewBase will do it for us on token.
                this.SelectAllTokensAndText();
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
            selectAllMenuItem.Click += (s, e) => this.SelectAllTokensAndText();
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
            // Verify if the usual behaviour of clearing token selection is required
            if (_pauseTokenClearOnFocus == false)
            {
                // Clear any selected tokens
                this.DeselectAll();
            }

            _pauseTokenClearOnFocus = false;

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

        private async void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            var t = sender.Text.Trim();

            TextChanged?.Invoke(sender, args);

            // Look for Token Delimiters to create new tokens when text changes.
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

            tokenitem.KeyPressAction -= Tokenitem_KeyPressAction;
            tokenitem.KeyPressAction += Tokenitem_KeyPressAction;

            tokenitem.ClearAllAction -= Tokenitem_ClearAllAction;
            tokenitem.ClearAllAction += Tokenitem_ClearAllAction;

            var menuFlyout = new MenuFlyout();

            var removeMenuItem = new MenuFlyoutItem
            {
                Text = StringExtensions.GetLocalized("WindowsCommunityToolkit_TokenizingTextBoxItem_MenuFlyout_Remove", "Microsoft.Toolkit.Uwp.UI.Controls/Resources")
            };
            removeMenuItem.Click += (s, e) => TokenizingTextBoxItem_ClearClicked(tokenitem, null);

            menuFlyout.Items.Add(removeMenuItem);

            var selectAllMenuItem = new MenuFlyoutItem
            {
                Text = StringExtensions.GetLocalized("WindowsCommunityToolkit_TokenizingTextBox_MenuFlyout_SelectAll", "Microsoft.Toolkit.Uwp.UI.Controls/Resources")
            };
            selectAllMenuItem.Click += (s, e) => this.SelectAllTokensAndText();

            menuFlyout.Items.Add(selectAllMenuItem);

            tokenitem.ContextFlyout = menuFlyout;
        }

        private void SelectAllTokensAndText()
        {
            _ = Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
            {
                this.SelectAllSafe();

                // need to synchronize the select all and the focus behaviour on the text box
                // because there is no way to identify that the focus has been set from this point
                // to avoid instantly clearing the selection of tokens
                _pauseTokenClearOnFocus = true;
                this._autoSuggestTextBox.Focus(FocusState.Programmatic);
                this._autoSuggestTextBox.SelectAll();
            });
        }

        private async void Tokenitem_ClearAllAction(TokenizingTextBoxItem sender, RoutedEventArgs args)
        {
            // find the first item selected
            int newSelectedIndex = -1;

            if (SelectedRanges.Count > 0)
            {
                newSelectedIndex = SelectedRanges[0].FirstIndex - 1;
            }

            await ClearAllSelected();

            SelectedIndex = newSelectedIndex;

            if (newSelectedIndex == -1)
            {
                // Focus the text box
                _autoSuggestTextBox.Focus(FocusState.Keyboard);
            }
            else
            {
                // focus the item prior to the first selected item
                (ContainerFromIndex(newSelectedIndex) as TokenizingTextBoxItem).Focus(FocusState.Keyboard);
            }
        }

        private async void Tokenitem_KeyPressAction(TokenizingTextBoxItem sender, RoutedEventArgs args)
        {
            // set focus to the text box
            await ClearAllSelected();
            _autoSuggestTextBox.Focus(FocusState.Keyboard);
        }

        private async void TokenizingTextBoxItem_ClearClicked(TokenizingTextBoxItem sender, RoutedEventArgs args)
        {
            await RemoveToken(sender);
        }

        private async Task ClearAllSelected()
        {
            for (int i = SelectedItems.Count - 1; i >= 0; i--)
            {
                await RemoveToken(ContainerFromItem(SelectedItems[i]) as TokenizingTextBoxItem);
            }
        }

        private async Task AddToken(object data)
        {
            if (data is string str && TokenItemAdding != null)
            {
                var tiaea = new TokenItemAddingEventArgs(str);
                await TokenItemAdding.InvokeAsync(this, tiaea);

                if (tiaea.Cancel)
                {
                    return;
                }

                if (tiaea.Item != null)
                {
                    data = tiaea.Item; // Transformed by event implementor
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

        private async Task RemoveToken(TokenizingTextBoxItem item)
        {
            var data = ItemFromContainer(item);

            if (TokenItemRemoving != null)
            {
                var tirea = new TokenItemRemovingEventArgs(data, item);
                await TokenItemRemoving.InvokeAsync(this, tirea);

                if (tirea.Cancel)
                {
                    return;
                }
            }

            Items.Remove(data);

            TokenItemRemoved?.Invoke(this, data);
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
