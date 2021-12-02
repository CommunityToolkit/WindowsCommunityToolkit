// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.ComponentModel;
using Microsoft.UI.Text;

namespace CommunityToolkit.WinUI.UI.Controls
{
    /// <summary>
    /// RichSuggestToken describes a suggestion token in the document.
    /// </summary>
    public class RichSuggestToken : INotifyPropertyChanged
    {
        /// <inheritdoc/>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets the token ID.
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// Gets the text displayed in the document.
        /// </summary>
        public string DisplayText { get; }

        /// <summary>
        /// Gets or sets the suggested item associated with this token.
        /// </summary>
        public object Item { get; set; }

        /// <summary>
        /// Gets the start position of the text range.
        /// </summary>
        public int RangeStart { get; private set; }

        /// <summary>
        /// Gets the end position of the text range.
        /// </summary>
        public int RangeEnd { get; private set; }

        /// <summary>
        /// Gets the start position of the token in number of characters.
        /// </summary>
        public int Position => _range?.GetIndex(TextRangeUnit.Character) - 1 ?? 0;

        internal bool Active { get; set; }

        private ITextRange _range;

        /// <summary>
        /// Initializes a new instance of the <see cref="RichSuggestToken"/> class.
        /// </summary>
        /// <param name="id">Token ID</param>
        /// <param name="displayText">Text in the document</param>
        public RichSuggestToken(Guid id, string displayText)
        {
            Id = id;
            DisplayText = displayText;
        }

        internal void UpdateTextRange(ITextRange range)
        {
            bool rangeStartChanged = RangeStart != range.StartPosition;
            bool rangeEndChanged = RangeEnd != range.EndPosition;
            bool positionChanged = _range == null || rangeStartChanged;
            _range = range.GetClone();
            RangeStart = _range.StartPosition;
            RangeEnd = _range.EndPosition;

            if (rangeStartChanged)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(RangeStart)));
            }

            if (rangeEndChanged)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(RangeEnd)));
            }

            if (positionChanged)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Position)));
            }
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"HYPERLINK \"{Id}\"\u200B{DisplayText}\u200B";
        }
    }
}
