// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Toolkit.Uwp.Deferred;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Metadata;
using Windows.System;
using Windows.UI.Input;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// The RichSuggestBox control extends <see cref="RichEditBox"/> control that suggests and embeds custom data in a rich document.
    /// </summary>
    [TemplatePart(Name = PartRichEditBox, Type = typeof(RichEditBox))]
    [TemplatePart(Name = PartSuggestionsPopup, Type = typeof(Popup))]
    [TemplatePart(Name = PartSuggestionsList, Type = typeof(ListViewBase))]
    [TemplatePart(Name = PartSuggestionsContainer, Type = typeof(Border))]
    [TemplatePart(Name = PartHeaderContentPresenter, Type = typeof(ContentPresenter))]
    [TemplatePart(Name = PartDescriptionPresenter, Type = typeof(ContentPresenter))]
    public partial class RichSuggestBox : ItemsControl
    {
        private const string PartRichEditBox = "RichEditBox";
        private const string PartSuggestionsPopup = "SuggestionsPopup";
        private const string PartSuggestionsList = "SuggestionsList";
        private const string PartSuggestionsContainer = "SuggestionsContainer";
        private const string PartHeaderContentPresenter = "HeaderContentPresenter";
        private const string PartDescriptionPresenter = "DescriptionPresenter";

        private readonly object _tokensLock;
        private readonly Dictionary<string, RichSuggestToken> _tokens;
        private readonly ObservableCollection<RichSuggestToken> _visibleTokens;

        private Popup _suggestionPopup;
        private RichEditBox _richEditBox;
        private ScrollViewer _scrollViewer;
        private ListViewBase _suggestionsList;
        private Border _suggestionsContainer;

        private int _suggestionChoice;
        private bool _ignoreChange;
        private bool _popupOpenDown;
        private bool _textCompositionActive;
        private RichSuggestQuery _currentQuery;

        /// <summary>
        /// Initializes a new instance of the <see cref="RichSuggestBox"/> class.
        /// </summary>
        public RichSuggestBox()
        {
            _tokensLock = new object();
            _tokens = new Dictionary<string, RichSuggestToken>();
            _visibleTokens = new ObservableCollection<RichSuggestToken>();
            Tokens = new ReadOnlyObservableCollection<RichSuggestToken>(_visibleTokens);

            DefaultStyleKey = typeof(RichSuggestBox);

            RegisterPropertyChangedCallback(CornerRadiusProperty, OnCornerRadiusChanged);
            RegisterPropertyChangedCallback(PopupCornerRadiusProperty, OnCornerRadiusChanged);
            LostFocus += OnLostFocus;
            Loaded += OnLoaded;
        }

        /// <summary>
        /// Clear unused tokens and undo/redo history. <see cref="RichSuggestBox"/> saves all of previously committed tokens
        /// even when they are removed from the text. They have to be manually removed using this method.
        /// </summary>
        public void ClearUndoRedoSuggestionHistory()
        {
            TextDocument.ClearUndoRedoHistory();
            lock (_tokensLock)
            {
                if (_tokens.Count == 0)
                {
                    return;
                }

                var keysToDelete = _tokens.Where(pair => !pair.Value.Active).Select(pair => pair.Key).ToArray();
                foreach (var key in keysToDelete)
                {
                    _tokens.Remove(key);
                }
            }
        }

        /// <summary>
        /// Clear the document and token list. This will also clear the undo/redo history.
        /// </summary>
        public void Clear()
        {
            lock (_tokensLock)
            {
                _tokens.Clear();
                _visibleTokens.Clear();
                TextDocument.Selection.Expand(TextRangeUnit.Story);
                TextDocument.Selection.Delete(TextRangeUnit.Story, 0);
                TextDocument.ClearUndoRedoHistory();
            }
        }

        /// <summary>
        /// Add tokens to be tracked against the document. Duplicate tokens will not be updated.
        /// </summary>
        /// <param name="tokens">The collection of tokens to be tracked.</param>
        public void AddTokens(IEnumerable<RichSuggestToken> tokens)
        {
            lock (_tokensLock)
            {
                foreach (var token in tokens)
                {
                    _tokens.TryAdd($"\"{token.Id}\"", token);
                }
            }
        }

        /// <summary>
        /// Populate the <see cref="RichSuggestBox"/> with an existing Rich Text Format (RTF) document and a collection of tokens.
        /// </summary>
        /// <param name="rtf">The Rich Text Format (RTF) text to be imported.</param>
        /// <param name="tokens">The collection of tokens embedded in the document.</param>
        public void Load(string rtf, IEnumerable<RichSuggestToken> tokens)
        {
            Clear();
            AddTokens(tokens);
            TextDocument.SetText(TextSetOptions.FormatRtf, rtf);
        }

        /// <summary>
        /// Try getting the token associated with a text range.
        /// </summary>
        /// <param name="range">The range of the token to get.</param>
        /// <param name="token">When this method returns, contains the token associated with the specified range; otherwise, it is null.</param>
        /// <returns>true if there is a token associated with the text range; otherwise false.</returns>
        public bool TryGetTokenFromRange(ITextRange range, out RichSuggestToken token)
        {
            token = null;
            range = range.GetClone();
            if (range != null && !string.IsNullOrEmpty(range.Link))
            {
                lock (_tokensLock)
                {
                    return _tokens.TryGetValue(range.Link, out token);
                }
            }

            return false;
        }

        /// <summary>
        /// Retrieves the bounding rectangle that encompasses the text range
        /// with position measured from the top left of the <see cref="RichSuggestBox"/> control.
        /// </summary>
        /// <param name="range">Text range to retrieve the bounding box from.</param>
        /// <returns>The bounding rectangle.</returns>
        public Rect GetRectFromRange(ITextRange range)
        {
            var padding = _richEditBox.Padding;
            range.GetRect(PointOptions.None, out var rect, out var hit);
            rect.X += padding.Left - HorizontalOffset;
            rect.Y += padding.Top - VerticalOffset;
            var transform = _richEditBox.TransformToVisual(this);
            return transform.TransformBounds(rect);
        }

        /// <inheritdoc/>
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            PointerEventHandler pointerPressedHandler = RichEditBox_OnPointerPressed;
            PointerEventHandler pointerMovedHandler = RichEditBox_OnPointerMoved;

            _suggestionPopup = (Popup)GetTemplateChild(PartSuggestionsPopup);
            _richEditBox = (RichEditBox)GetTemplateChild(PartRichEditBox);
            _suggestionsList = (ListViewBase)GetTemplateChild(PartSuggestionsList);
            _suggestionsContainer = (Border)GetTemplateChild(PartSuggestionsContainer);
            ConditionallyLoadElement(Header, PartHeaderContentPresenter);
            ConditionallyLoadElement(Description, PartDescriptionPresenter);

            if (_richEditBox != null)
            {
                _richEditBox.SizeChanged -= RichEditBox_SizeChanged;
                _richEditBox.TextChanging -= RichEditBox_TextChanging;
                _richEditBox.TextChanged -= RichEditBox_TextChanged;
                _richEditBox.TextCompositionStarted -= RichEditBox_TextCompositionStarted;
                _richEditBox.TextCompositionChanged -= RichEditBox_TextCompositionChanged;
                _richEditBox.TextCompositionEnded -= RichEditBox_TextCompositionEnded;
                _richEditBox.SelectionChanging -= RichEditBox_SelectionChanging;
                _richEditBox.SelectionChanged -= RichEditBox_SelectionChanged;
                _richEditBox.Paste -= RichEditBox_Paste;
                _richEditBox.PreviewKeyDown -= RichEditBox_PreviewKeyDown;
                _richEditBox.RemoveHandler(PointerMovedEvent, pointerMovedHandler);
                _richEditBox.RemoveHandler(PointerPressedEvent, pointerPressedHandler);
                _richEditBox.ProcessKeyboardAccelerators -= RichEditBox_ProcessKeyboardAccelerators;

                _richEditBox.SizeChanged += RichEditBox_SizeChanged;
                _richEditBox.TextChanging += RichEditBox_TextChanging;
                _richEditBox.TextChanged += RichEditBox_TextChanged;
                _richEditBox.TextCompositionStarted += RichEditBox_TextCompositionStarted;
                _richEditBox.TextCompositionChanged += RichEditBox_TextCompositionChanged;
                _richEditBox.TextCompositionEnded += RichEditBox_TextCompositionEnded;
                _richEditBox.SelectionChanging += RichEditBox_SelectionChanging;
                _richEditBox.SelectionChanged += RichEditBox_SelectionChanged;
                _richEditBox.Paste += RichEditBox_Paste;
                _richEditBox.PreviewKeyDown += RichEditBox_PreviewKeyDown;
                _richEditBox.AddHandler(PointerMovedEvent, pointerMovedHandler, true);
                _richEditBox.AddHandler(PointerPressedEvent, pointerPressedHandler, true);
                _richEditBox.ProcessKeyboardAccelerators += RichEditBox_ProcessKeyboardAccelerators;
            }

            if (_suggestionsList != null)
            {
                _suggestionsList.ItemClick -= SuggestionsList_ItemClick;
                _suggestionsList.SizeChanged -= SuggestionsList_SizeChanged;
                _suggestionsList.GotFocus -= SuggestionList_GotFocus;

                _suggestionsList.ItemClick += SuggestionsList_ItemClick;
                _suggestionsList.SizeChanged += SuggestionsList_SizeChanged;
                _suggestionsList.GotFocus += SuggestionList_GotFocus;
            }
        }

        private static void OnHeaderChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var view = (RichSuggestBox)d;
            view.ConditionallyLoadElement(e.NewValue, PartHeaderContentPresenter);
        }

        private static void OnDescriptionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var view = (RichSuggestBox)d;
            view.ConditionallyLoadElement(e.NewValue, PartDescriptionPresenter);
        }

        private static void OnSuggestionPopupPlacementChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var view = (RichSuggestBox)d;
            view.UpdatePopupWidth();
        }

        private static void OnPrefixesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var view = (RichSuggestBox)d;

            var newValue = (string)e.NewValue;
            var prefixes = EnforcePrefixesRequirements(newValue);

            if (newValue != prefixes)
            {
                view.SetValue(PrefixesProperty, prefixes);
            }
        }

        private void OnCornerRadiusChanged(DependencyObject sender, DependencyProperty dp)
        {
            UpdateCornerRadii();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            _scrollViewer = _richEditBox?.FindDescendant<ScrollViewer>();
        }

        private void OnLostFocus(object sender, RoutedEventArgs e)
        {
            ShowSuggestionsPopup(false);
        }

        private void SuggestionsList_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (this._suggestionPopup.IsOpen)
            {
                this.UpdatePopupOffset();
            }
        }

        private void SuggestionList_GotFocus(object sender, RoutedEventArgs e)
        {
            if (_richEditBox != null)
            {
                _richEditBox.Focus(FocusState.Programmatic);
            }
        }

        private void RichEditBox_OnPointerMoved(object sender, PointerRoutedEventArgs e)
        {
            var pointer = e.GetCurrentPoint(this);
            if (this.TokenPointerOver != null)
            {
                this.InvokeTokenPointerOver(pointer);
            }
        }

        private void RichEditBox_SelectionChanging(RichEditBox sender, RichEditBoxSelectionChangingEventArgs args)
        {
            var selection = TextDocument.Selection;

            if (selection.Type != SelectionType.InsertionPoint && selection.Type != SelectionType.Normal)
            {
                return;
            }

            var range = selection.GetClone();
            range.Expand(TextRangeUnit.Link);
            lock (_tokensLock)
            {
                if (!_tokens.ContainsKey(range.Link))
                {
                    return;
                }
            }

            ExpandSelectionOnPartialTokenSelect(selection, range);
        }

        private async void RichEditBox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            SelectionChanged?.Invoke(this, e);

            // During text composition changing (e.g. user typing with an IME),
            // SelectionChanged event is fired multiple times with each keystroke.
            // To reduce the number of suggestion requests, the request is made
            // in TextCompositionChanged handler instead.
            if (_textCompositionActive)
            {
                return;
            }

            await RequestSuggestionsAsync();
        }

        private void RichEditBox_OnPointerPressed(object sender, PointerRoutedEventArgs e)
        {
            ShowSuggestionsPopup(false);
        }

        private async void RichEditBox_ProcessKeyboardAccelerators(UIElement sender, ProcessKeyboardAcceleratorEventArgs args)
        {
            var itemsList = _suggestionsList.Items;
            if (!_suggestionPopup.IsOpen || itemsList == null || itemsList.Count == 0)
            {
                return;
            }

            var key = args.Key;
            switch (key)
            {
                case VirtualKey.Up when itemsList.Count == 1:
                case VirtualKey.Down when itemsList.Count == 1:
                    args.Handled = true;
                    UpdateSuggestionsListSelectedItem(1);
                    break;

                case VirtualKey.Up:
                    args.Handled = true;
                    _suggestionChoice = _suggestionChoice <= 0 ? itemsList.Count : _suggestionChoice - 1;
                    UpdateSuggestionsListSelectedItem(this._suggestionChoice);
                    break;

                case VirtualKey.Down:
                    args.Handled = true;
                    _suggestionChoice = _suggestionChoice >= itemsList.Count ? 0 : _suggestionChoice + 1;
                    UpdateSuggestionsListSelectedItem(this._suggestionChoice);
                    break;

                case VirtualKey.Enter when _suggestionsList.SelectedItem != null:
                    args.Handled = true;
                    await CommitSuggestionAsync(_suggestionsList.SelectedItem);
                    break;

                case VirtualKey.Escape:
                    args.Handled = true;
                    ShowSuggestionsPopup(false);
                    break;
            }
        }

        private async void RichEditBox_PreviewKeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Tab && _suggestionPopup.IsOpen && _suggestionsList.SelectedItem != null)
            {
                e.Handled = true;
                await CommitSuggestionAsync(_suggestionsList.SelectedItem);
            }
        }

        private async void SuggestionsList_ItemClick(object sender, ItemClickEventArgs e)
        {
            var selectedItem = e.ClickedItem;
            await CommitSuggestionAsync(selectedItem);
        }

        private void RichEditBox_TextChanging(RichEditBox sender, RichEditBoxTextChangingEventArgs args)
        {
            if (_ignoreChange || !args.IsContentChanging)
            {
                return;
            }

            _ignoreChange = true;
            ValidateTokensInDocument();
            TextDocument.EndUndoGroup();
            TextDocument.BeginUndoGroup();
            _ignoreChange = false;
        }

        private void RichEditBox_TextChanged(object sender, RoutedEventArgs e)
        {
            UpdateVisibleTokenList();
            TextChanged?.Invoke(this, e);
        }

        private void RichEditBox_TextCompositionStarted(RichEditBox sender, TextCompositionStartedEventArgs args)
        {
            _textCompositionActive = true;
        }

        private async void RichEditBox_TextCompositionChanged(RichEditBox sender, TextCompositionChangedEventArgs args)
        {
            var range = TextDocument.GetRange(args.StartIndex == 0 ? 0 : args.StartIndex - 1, args.StartIndex + args.Length);
            await RequestSuggestionsAsync(range);
        }

        private void RichEditBox_TextCompositionEnded(RichEditBox sender, TextCompositionEndedEventArgs args)
        {
            _textCompositionActive = false;
        }

        private void RichEditBox_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.UpdatePopupWidth();
            this.UpdatePopupOffset();
        }

        private async void RichEditBox_Paste(object sender, TextControlPasteEventArgs e)
        {
            Paste?.Invoke(this, e);

            if (e.Handled || TextDocument == null || ClipboardPasteFormat != RichEditClipboardFormat.PlainText)
            {
                return;
            }

            e.Handled = true;
            var dataPackageView = Clipboard.GetContent();
            if (dataPackageView.Contains(StandardDataFormats.Text))
            {
                var text = await dataPackageView.GetTextAsync();
                TextDocument.Selection.SetText(TextSetOptions.Unhide, text);
                TextDocument.Selection.Collapse(false);
            }
        }

        private void ExpandSelectionOnPartialTokenSelect(ITextSelection selection, ITextRange tokenRange)
        {
            switch (selection.Type)
            {
                case SelectionType.InsertionPoint:
                    // Snap selection to token on click
                    if (tokenRange.StartPosition < selection.StartPosition && selection.EndPosition < tokenRange.EndPosition)
                    {
                        selection.Expand(TextRangeUnit.Link);
                        InvokeTokenSelected(selection);
                    }

                    break;

                case SelectionType.Normal:
                    // We do not want user to partially select a token since pasting to a partial token can break
                    // the token tracking system, which can result in unwanted character formatting issues.
                    if ((tokenRange.StartPosition <= selection.StartPosition && selection.EndPosition < tokenRange.EndPosition) ||
                        (tokenRange.StartPosition < selection.StartPosition && selection.EndPosition <= tokenRange.EndPosition))
                    {
                        // TODO: Figure out how to expand selection without breaking selection flow (with Shift select or pointer sweep select)
                        selection.Expand(TextRangeUnit.Link);
                        InvokeTokenSelected(selection);
                    }

                    break;
            }
        }

        private void InvokeTokenSelected(ITextSelection selection)
        {
            if (TokenSelected == null || !TryGetTokenFromRange(selection, out var token) || token.RangeEnd != selection.EndPosition)
            {
                return;
            }

            TokenSelected.Invoke(this, new RichSuggestTokenSelectedEventArgs
            {
                Token = token,
                Range = selection.GetClone()
            });
        }

        private void InvokeTokenPointerOver(PointerPoint pointer)
        {
            var pointerPosition = TransformToVisual(_richEditBox).TransformPoint(pointer.Position);
            var padding = _richEditBox.Padding;
            pointerPosition.X += HorizontalOffset - padding.Left;
            pointerPosition.Y += VerticalOffset - padding.Top;
            var range = TextDocument.GetRangeFromPoint(pointerPosition, PointOptions.ClientCoordinates);
            var linkRange = range.GetClone();
            range.Expand(TextRangeUnit.Character);
            range.GetRect(PointOptions.None, out var hitTestRect, out _);
            hitTestRect.X -= hitTestRect.Width;
            hitTestRect.Width *= 2;
            if (hitTestRect.Contains(pointerPosition) && linkRange.Expand(TextRangeUnit.Link) > 0 &&
                TryGetTokenFromRange(linkRange, out var token))
            {
                this.TokenPointerOver.Invoke(this, new RichSuggestTokenPointerOverEventArgs
                {
                    Token = token,
                    Range = linkRange,
                    CurrentPoint = pointer
                });
            }
        }

        private async Task RequestSuggestionsAsync(ITextRange range = null)
        {
            string prefix;
            string query;
            var currentQuery = _currentQuery;
            var queryFound = range == null
                ? TryExtractQueryFromSelection(out prefix, out query, out range)
                : TryExtractQueryFromRange(range, out prefix, out query);

            if (queryFound && prefix == currentQuery?.Prefix && query == currentQuery?.QueryText &&
                range.EndPosition == currentQuery?.Range.EndPosition && _suggestionPopup.IsOpen)
            {
                return;
            }

            var previousTokenSource = currentQuery?.CancellationTokenSource;
            if (!(previousTokenSource?.IsCancellationRequested ?? true))
            {
                previousTokenSource.Cancel();
            }

            if (queryFound)
            {
                using var tokenSource = new CancellationTokenSource();
                _currentQuery = new RichSuggestQuery
                {
                    Prefix = prefix,
                    QueryText = query,
                    Range = range,
                    CancellationTokenSource = tokenSource
                };

                if (SuggestionRequested != null)
                {
                    var cancellationToken = tokenSource.Token;
                    var eventArgs = new SuggestionRequestedEventArgs { QueryText = query, Prefix = prefix };
                    try
                    {
                        await SuggestionRequested.InvokeAsync(this, eventArgs, cancellationToken);
                    }
                    catch (OperationCanceledException)
                    {
                        return;
                    }

                    if (!eventArgs.Cancel)
                    {
                        _suggestionChoice = 0;
                        ShowSuggestionsPopup(_suggestionsList?.Items?.Count > 0);
                    }
                }

                tokenSource.Cancel();
            }
            else
            {
                ShowSuggestionsPopup(false);
            }
        }

        internal async Task CommitSuggestionAsync(object selectedItem)
        {
            var currentQuery = _currentQuery;
            var range = currentQuery?.Range.GetClone();
            var id = Guid.NewGuid();
            var prefix = currentQuery?.Prefix;
            var query = currentQuery?.QueryText;

            // range has length of 0 at the end of the commit.
            // Checking length == 0 to avoid committing twice.
            if (prefix == null || query == null || range == null || range.Length == 0)
            {
                return;
            }

            var textBefore = range.Text;
            var format = CreateTokenFormat(range);
            var eventArgs = new SuggestionChosenEventArgs
            {
                Id = id,
                Prefix = prefix,
                QueryText = query,
                SelectedItem = selectedItem,
                DisplayText = query,
                Format = format
            };

            if (SuggestionChosen != null)
            {
                await SuggestionChosen.InvokeAsync(this, eventArgs);
            }

            var text = eventArgs.DisplayText;

            // Since this operation is async, the document may have changed at this point.
            // Double check if the range still has the expected query.
            if (string.IsNullOrEmpty(text) || textBefore != range.Text ||
                !TryExtractQueryFromRange(range, out var testPrefix, out var testQuery) ||
                testPrefix != prefix || testQuery != query)
            {
                return;
            }

            lock (_tokensLock)
            {
                var displayText = prefix + text;

                _ignoreChange = true;
                var committed = TryCommitSuggestionIntoDocument(range, displayText, id, eventArgs.Format ?? format);
                TextDocument.EndUndoGroup();
                TextDocument.BeginUndoGroup();
                _ignoreChange = false;

                if (committed)
                {
                    var token = new RichSuggestToken(id, displayText) { Active = true, Item = selectedItem };
                    token.UpdateTextRange(range);
                    _tokens.TryAdd(range.Link, token);
                }
            }
        }

        private bool TryCommitSuggestionIntoDocument(ITextRange range, string displayText, Guid id, ITextCharacterFormat format, bool addTrailingSpace = true)
        {
            // We don't want to set text when the display text doesn't change since it may lead to unexpected caret move.
            range.GetText(TextGetOptions.NoHidden, out var existingText);
            if (existingText != displayText)
            {
                range.SetText(TextSetOptions.Unhide, displayText);
            }

            var formatBefore = range.CharacterFormat.GetClone();
            range.CharacterFormat.SetClone(format);
            PadRange(range, formatBefore);
            range.Link = $"\"{id}\"";

            // In some rare case, setting Link can fail. Only observed when interacting with Undo/Redo feature.
            if (range.Link != $"\"{id}\"")
            {
                range.Delete(TextRangeUnit.Story, -1);
                return false;
            }

            if (addTrailingSpace)
            {
                var clone = range.GetClone();
                clone.Collapse(false);
                clone.SetText(TextSetOptions.Unhide, " ");
                clone.Collapse(false);
                TextDocument.Selection.SetRange(clone.EndPosition, clone.EndPosition);
            }

            return true;
        }

        private void ValidateTokensInDocument()
        {
            lock (_tokensLock)
            {
                foreach (var (_, token) in _tokens)
                {
                    token.Active = false;
                }
            }

            ForEachLinkInDocument(TextDocument, ValidateTokenFromRange);
        }

        private void ValidateTokenFromRange(ITextRange range)
        {
            if (range.Length == 0 || !TryGetTokenFromRange(range, out var token))
            {
                return;
            }

            // Check for duplicate tokens. This can happen if the user copies and pastes the token multiple times.
            if (token.Active && token.RangeStart != range.StartPosition && token.RangeEnd != range.EndPosition)
            {
                lock (_tokensLock)
                {
                    var guid = Guid.NewGuid();
                    if (TryCommitSuggestionIntoDocument(range, token.DisplayText, guid, CreateTokenFormat(range), false))
                    {
                        token = new RichSuggestToken(guid, token.DisplayText) { Active = true, Item = token.Item };
                        token.UpdateTextRange(range);
                        _tokens.Add(range.Link, token);
                    }

                    return;
                }
            }

            if (token.ToString() != range.Text)
            {
                range.Delete(TextRangeUnit.Story, 0);
                token.Active = false;
                return;
            }

            token.UpdateTextRange(range);
            token.Active = true;
        }

        private void ConditionallyLoadElement(object property, string elementName)
        {
            if (property != null && GetTemplateChild(elementName) is UIElement presenter)
            {
                presenter.Visibility = Visibility.Visible;
            }
        }

        private void UpdateSuggestionsListSelectedItem(int choice)
        {
            var itemsList = _suggestionsList.Items;
            if (itemsList == null)
            {
                return;
            }

            _suggestionsList.SelectedItem = choice == 0 ? null : itemsList[choice - 1];
            _suggestionsList.ScrollIntoView(_suggestionsList.SelectedItem);
        }

        private void ShowSuggestionsPopup(bool show)
        {
            if (_suggestionPopup == null)
            {
                return;
            }

            this._suggestionPopup.IsOpen = show;
            if (!show)
            {
                this._suggestionChoice = 0;
                this._suggestionPopup.VerticalOffset = 0;
                this._suggestionPopup.HorizontalOffset = 0;
                this._suggestionsList.SelectedItem = null;
                this._suggestionsList.ScrollIntoView(this._suggestionsList.Items?.FirstOrDefault());
                UpdateCornerRadii();
            }
        }

        private void UpdatePopupWidth()
        {
            if (this._suggestionsContainer == null)
            {
                return;
            }

            if (this.PopupPlacement == SuggestionPopupPlacementMode.Attached)
            {
                this._suggestionsContainer.MaxWidth = double.PositiveInfinity;
                this._suggestionsContainer.Width = this._richEditBox.ActualWidth;
            }
            else
            {
                this._suggestionsContainer.MaxWidth = this._richEditBox.ActualWidth;
                this._suggestionsContainer.Width = double.NaN;
            }
        }

        /// <summary>
        /// Calculate whether to open the suggestion list up or down depends on how much screen space is available
        /// </summary>
        private void UpdatePopupOffset()
        {
            if (this._suggestionsContainer == null || this._suggestionPopup == null || this._richEditBox == null)
            {
                return;
            }

            this._richEditBox.TextDocument.Selection.GetRect(PointOptions.None, out var selectionRect, out _);
            Thickness padding = this._richEditBox.Padding;
            selectionRect.X -= HorizontalOffset;
            selectionRect.Y -= VerticalOffset;

            // Update horizontal offset
            if (this.PopupPlacement == SuggestionPopupPlacementMode.Attached)
            {
                this._suggestionPopup.HorizontalOffset = 0;
            }
            else
            {
                double editBoxWidth = this._richEditBox.ActualWidth - padding.Left - padding.Right;
                if (this._suggestionPopup.HorizontalOffset == 0 && editBoxWidth > 0)
                {
                    var normalizedX = selectionRect.X / editBoxWidth;
                    this._suggestionPopup.HorizontalOffset =
                        (this._richEditBox.ActualWidth - this._suggestionsContainer.ActualWidth) * normalizedX;
                }
            }

            // Update vertical offset
            double downOffset = this._richEditBox.ActualHeight;
            double upOffset = -this._suggestionsContainer.ActualHeight;
            if (this.PopupPlacement == SuggestionPopupPlacementMode.Floating)
            {
                downOffset = selectionRect.Bottom + padding.Top + padding.Bottom;
                upOffset += selectionRect.Top;
            }

            if (this._suggestionPopup.VerticalOffset == 0)
            {
                if (IsElementOnScreen(this._suggestionsContainer, offsetY: downOffset) &&
                    (IsElementInsideWindow(this._suggestionsContainer, offsetY: downOffset) ||
                     !IsElementInsideWindow(this._suggestionsContainer, offsetY: upOffset) ||
                     !IsElementOnScreen(this._suggestionsContainer, offsetY: upOffset)))
                {
                    this._suggestionPopup.VerticalOffset = downOffset;
                    this._popupOpenDown = true;
                }
                else
                {
                    this._suggestionPopup.VerticalOffset = upOffset;
                    this._popupOpenDown = false;
                }

                UpdateCornerRadii();
            }
            else
            {
                this._suggestionPopup.VerticalOffset = this._popupOpenDown ? downOffset : upOffset;
            }
        }

        /// <summary>
        /// Set corner radii so that inner corners, where suggestion list and text box connect, are square.
        /// This only applies when <see cref="PopupPlacement"/> is set to <see cref="SuggestionPopupPlacementMode.Attached"/>.
        /// </summary>
        /// https://docs.microsoft.com/en-us/windows/apps/design/style/rounded-corner#when-not-to-round
        private void UpdateCornerRadii()
        {
            if (this._richEditBox == null || this._suggestionsContainer == null ||
                !ApiInformation.IsApiContractPresent("Windows.Foundation.UniversalApiContract", 7))
            {
                return;
            }

            this._richEditBox.CornerRadius = CornerRadius;
            this._suggestionsContainer.CornerRadius = PopupCornerRadius;

            if (this._suggestionPopup.IsOpen && PopupPlacement == SuggestionPopupPlacementMode.Attached)
            {
                if (this._popupOpenDown)
                {
                    var cornerRadius = new CornerRadius(CornerRadius.TopLeft, CornerRadius.TopRight, 0, 0);
                    this._richEditBox.CornerRadius = cornerRadius;
                    var popupCornerRadius =
                        new CornerRadius(0, 0, PopupCornerRadius.BottomRight, PopupCornerRadius.BottomLeft);
                    this._suggestionsContainer.CornerRadius = popupCornerRadius;
                }
                else
                {
                    var cornerRadius = new CornerRadius(0, 0, CornerRadius.BottomRight, CornerRadius.BottomLeft);
                    this._richEditBox.CornerRadius = cornerRadius;
                    var popupCornerRadius =
                        new CornerRadius(PopupCornerRadius.TopLeft, PopupCornerRadius.TopRight, 0, 0);
                    this._suggestionsContainer.CornerRadius = popupCornerRadius;
                }
            }
        }

        private bool TryExtractQueryFromSelection(out string prefix, out string query, out ITextRange range)
        {
            prefix = string.Empty;
            query = string.Empty;
            range = null;
            if (TextDocument.Selection.Type != SelectionType.InsertionPoint)
            {
                return false;
            }

            // Check if selection is on existing link (suggestion)
            var expandCount = TextDocument.Selection.GetClone().Expand(TextRangeUnit.Link);
            if (expandCount != 0)
            {
                return false;
            }

            var selection = TextDocument.Selection.GetClone();
            selection.MoveStart(TextRangeUnit.Word, -1);
            if (selection.Length == 0)
            {
                return false;
            }

            range = selection;
            if (TryExtractQueryFromRange(selection, out prefix, out query))
            {
                return true;
            }

            selection.MoveStart(TextRangeUnit.Word, -1);
            if (TryExtractQueryFromRange(selection, out prefix, out query))
            {
                return true;
            }

            range = null;
            return false;
        }

        private bool TryExtractQueryFromRange(ITextRange range, out string prefix, out string query)
        {
            prefix = string.Empty;
            query = string.Empty;
            range.GetText(TextGetOptions.NoHidden, out var possibleQuery);
            if (possibleQuery.Length > 0 && Prefixes.Contains(possibleQuery[0]) &&
                !possibleQuery.Any(char.IsWhiteSpace) && string.IsNullOrEmpty(range.Link))
            {
                if (possibleQuery.Length == 1)
                {
                    prefix = possibleQuery;
                    return true;
                }

                prefix = possibleQuery[0].ToString();
                query = possibleQuery.Substring(1);
                return true;
            }

            return false;
        }

        private ITextCharacterFormat CreateTokenFormat(ITextRange range)
        {
            var format = range.CharacterFormat.GetClone();
            if (this.TokenBackground != null)
            {
                format.BackgroundColor = this.TokenBackground.Color;
            }

            if (this.TokenForeground != null)
            {
                format.ForegroundColor = this.TokenForeground.Color;
            }

            return format;
        }

        private void UpdateVisibleTokenList()
        {
            lock (_tokensLock)
            {
                var toBeRemoved = _visibleTokens.Where(x => !x.Active || !_tokens.ContainsKey($"\"{x.Id}\"")).ToArray();

                foreach (var elem in toBeRemoved)
                {
                    _visibleTokens.Remove(elem);
                }

                var toBeAdded = _tokens.Where(pair => pair.Value.Active && !_visibleTokens.Contains(pair.Value))
                    .Select(pair => pair.Value).ToArray();

                foreach (var elem in toBeAdded)
                {
                    _visibleTokens.Add(elem);
                }
            }
        }
    }
}
