// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.System;
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
        /// Clear unused tokens and undo/redo history.
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

        private async void SuggestionsList_ItemClick(object sender, ItemClickEventArgs e)
        {
            var selectedItem = e.ClickedItem;
            await CommitSuggestionAsync(selectedItem);
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
            _richEditBox?.Focus(FocusState.Programmatic);
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

        private void RichEditBox_TextChanging(RichEditBox sender, RichEditBoxTextChangingEventArgs args)
        {
            if (_ignoreChange || !args.IsContentChanging)
            {
                return;
            }

            lock (_tokensLock)
            {
                this.CreateSingleEdit(ValidateTokensInDocument);
            }
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

        private void ConditionallyLoadElement(object property, string elementName)
        {
            if (property != null && GetTemplateChild(elementName) is UIElement presenter)
            {
                presenter.Visibility = Visibility.Visible;
            }
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
