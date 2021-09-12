// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using Windows.UI.Input;
using Windows.UI.Text;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// The RichSuggestBox control extends <see cref="RichEditBox"/> control that suggests and embeds custom data in a rich document.
    /// </summary>
    public partial class RichSuggestBox
    {
        private void CreateSingleEdit(Action editAction)
        {
            _ignoreChange = true;
            editAction.Invoke();
            TextDocument.EndUndoGroup();
            TextDocument.BeginUndoGroup();
            _ignoreChange = false;
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

        private void ValidateTokensInDocument()
        {
            foreach (var (_, token) in _tokens)
            {
                token.Active = false;
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
                var guid = Guid.NewGuid();
                if (TryCommitSuggestionIntoDocument(range, token.DisplayText, guid, CreateTokenFormat(range), false))
                {
                    token = new RichSuggestToken(guid, token.DisplayText) { Active = true, Item = token.Item };
                    token.UpdateTextRange(range);
                    _tokens.Add(range.Link, token);
                }

                return;
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

        private bool TryCommitSuggestionIntoDocument(ITextRange range, string displayText, Guid id, ITextCharacterFormat format, bool addTrailingSpace)
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
    }
}