// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Toolkit.Parsers.Core;
using Microsoft.Toolkit.Parsers.Markdown.Helpers;

namespace Microsoft.Toolkit.Parsers.Markdown.Inlines
{
    /// <summary>
    /// Attempts to parse a text span like bold or strikethrough.
    /// </summary>
    /// <typeparam name="T">The inline Type.</typeparam>
    public abstract class InlineSourundParser<T> : MarkdownInline.Parser<T>
        where T : MarkdownInline
    {
        private readonly string _markerStart;
        private readonly string _markerEnd;

        /// <summary>
        /// Initializes a new instance of the <see cref="InlineSourundParser{T}"/> class.
        /// </summary>
        /// <param name="marker">the text that sourunds.</param>
        public InlineSourundParser(string marker)
            : this(marker, marker)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InlineSourundParser{T}"/> class.
        /// </summary>
        /// <param name="markerStart">The startTag.</param>
        /// <param name="markerEnd">The endTag.</param>
        public InlineSourundParser(string markerStart, string markerEnd)
        {
            this._markerStart = markerStart;
            this._markerEnd = markerEnd;
        }

        /// <inheritdoc/>
        public sealed override IEnumerable<char> TripChar => _markerStart.Take(1);

        /// <inheritdoc/>
        protected sealed override InlineParseResult<T> ParseInternal(LineBlock markdown, LineBlockPosition tripPos, MarkdownDocument document, IEnumerable<Type> ignoredParsers)
        {
            if (!tripPos.IsIn(markdown))
            {
                throw new ArgumentOutOfRangeException(nameof(tripPos));
            }

            // Ignore escaped sequences
            if (tripPos.Column > 0 && markdown[tripPos.Line][tripPos.Column - 1] == '\\')
            {
                return null;
            }

            // discard not nedded parts.
            markdown = markdown.SliceText(tripPos);

            // Check the start sequence.
            if (!markdown[0].StartsWith(_markerStart.AsSpan()))
            {
                return null;
            }

            // Find the end of the span.  The end sequence (either '**' or '__') must be the same
            // as the start sequence.
            var subBlock = markdown.SliceText(_markerStart.Length);
            var endPosition = GetEndPosition(subBlock);
            if (endPosition == LineBlockPosition.NotFound)
            {
                return null;
            }

            var innerBlock = subBlock.SliceText(0, endPosition.FromStart);

            // The span must contain at least one character.
            if (innerBlock.TextLength == 0)
            {
                return null;
            }

            // The first character inside the span must NOT be a space.
            if (ParseHelpers.IsMarkdownWhiteSpace(innerBlock[0][0]))
            {
                return null;
            }

            // The last character inside the span must NOT be a space.
            if (ParseHelpers.IsMarkdownWhiteSpace(innerBlock[innerBlock.LineCount - 1][innerBlock[innerBlock.LineCount - 1].Length - 1]))
            {
                return null;
            }

            var result = MakeInline(document.ParseInlineChildren(innerBlock, false, false, ignoredParsers));

            // We found something!
            return InlineParseResult.Create(result, tripPos, endPosition.FromStart + _markerStart.Length + _markerEnd.Length);
        }

        private LineBlockPosition GetEndPosition(LineBlock subBlock)
        {
            var position = LineBlockPosition.NotFound;
            do
            {
                if (position == LineBlockPosition.NotFound)
                {
                    position = default;
                }
                else
                {
                    position = position.Add(1, subBlock);
                }

                var positionOffset = subBlock.SliceText(position).IndexOf(_markerEnd.AsSpan(), StringComparison.OrdinalIgnoreCase);
                if (positionOffset == LineBlockPosition.NotFound)
                {
                    return LineBlockPosition.NotFound;
                }

                position = positionOffset.Add(position.FromStart, subBlock);
            }
            while (position.Column > 0 && subBlock[position.Line, position.Column - 1] == '\\');

            return position;
        }

        /// <summary>
        /// Generates the InlineElement.
        /// </summary>
        /// <param name="inlines">The childrean.</param>
        /// <returns>The Inline element.</returns>
        protected abstract T MakeInline(List<MarkdownInline> inlines);
    }
}