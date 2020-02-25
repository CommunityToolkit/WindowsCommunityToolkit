// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using Microsoft.Toolkit.Parsers.Core;
using Microsoft.Toolkit.Parsers.Markdown.Helpers;

namespace Microsoft.Toolkit.Parsers.Markdown.Inlines
{
    /// <summary>
    /// Represents a span containing emoji symbol.
    /// </summary>
    public partial class EmojiInline : MarkdownInline, IInlineLeaf
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EmojiInline"/> class.
        /// </summary>
        public EmojiInline()
            : base(MarkdownInlineType.Emoji)
        {
        }

        /// <summary>
        /// Attempts to parse an emoji.
        /// </summary>
        public new class Parser : Parser<EmojiInline>
        {
            /// <inheritdoc/>
            protected override InlineParseResult<EmojiInline> ParseInternal(LineBlock markdown, LineBlockPosition tripPos, MarkdownDocument document, IEnumerable<Type> ignoredParsers)
            {
                if (!tripPos.IsIn(markdown))
                {
                    throw new ArgumentOutOfRangeException(nameof(tripPos));
                }

                var line = markdown[tripPos.Line];

                // Check the start sequence.
                var startSequence = line.Slice(tripPos.Column, 1);
                if (!startSequence.StartsWith(":".AsSpan()))
                {
                    return null;
                }

                // Find the end of the span.
                int innerLength = line.Slice(tripPos.Column + 1).IndexOf(startSequence, StringComparison.OrdinalIgnoreCase);
                if (innerLength == -1)
                {
                    return null;
                }

                // The span must contain at least one character.
                if (innerLength == 0)
                {
                    return null;
                }

                var emojiName = line.Slice(tripPos.Column + 1, innerLength).ToString();

                if (_emojiCodesDictionary.TryGetValue(emojiName, out var emojiCode))
                {
                    var result = new EmojiInline { Text = char.ConvertFromUtf32(emojiCode), Type = MarkdownInlineType.Emoji };
                    return InlineParseResult.Create(result, tripPos, innerLength + 1);
                }

                return null;
            }

            /// <inheritdoc/>
            public override IEnumerable<char> TripChar => ":";
        }

        /// <inheritdoc/>
        public string Text { get; set; }
    }
}