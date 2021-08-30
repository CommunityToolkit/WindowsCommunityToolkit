// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Toolkit.Uwp.Deferred;
using Windows.Foundation.Metadata;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// The RichSuggestBox control extends <see cref="RichEditBox"/> control that suggests and embeds custom data in a rich document.
    /// </summary>
    public partial class RichSuggestBox
    {
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
                    Prefix = prefix, QueryText = query, Range = range, CancellationTokenSource = tokenSource
                };

                var cancellationToken = tokenSource.Token;
                var eventArgs = new SuggestionRequestedEventArgs { QueryText = query, Prefix = prefix };
                if (SuggestionRequested != null)
                {
                    try
                    {
                        await SuggestionRequested.InvokeAsync(this, eventArgs, cancellationToken);
                    }
                    catch (OperationCanceledException)
                    {
                        return;
                    }
                }

                if (!eventArgs.Cancel)
                {
                    _suggestionChoice = 0;
                    ShowSuggestionsPopup(_suggestionsList?.Items?.Count > 0);
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

            var displayText = prefix + text;

            void RealizeToken()
            {
                if (TryCommitSuggestionIntoDocument(range, displayText, id, eventArgs.Format ?? format, true))
                {
                    var token = new RichSuggestToken(id, displayText) { Active = true, Item = selectedItem };
                    token.UpdateTextRange(range);
                    _tokens.Add(range.Link, token);
                }
            }

            lock (_tokensLock)
            {
                this.CreateSingleEdit(RealizeToken);
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
    }
}