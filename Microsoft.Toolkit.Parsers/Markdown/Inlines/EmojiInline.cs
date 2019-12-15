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
            protected override InlineParseResult<EmojiInline> ParseInternal(string markdown, int minStart, int tripPos, int maxEnd, MarkdownDocument document, IEnumerable<Type> ignoredParsers)
            {
                if (tripPos >= maxEnd - 1)
                {
                    return null;
                }

                // Check the start sequence.
                var startSequence = markdown.AsSpan(tripPos, 1);
                if (!startSequence.StartsWith(":".AsSpan()))
                {
                    return null;
                }

                // Find the end of the span.
                var innerStart = tripPos + 1;
                int innerLength = markdown.AsSpan(innerStart, maxEnd - innerStart).IndexOf(startSequence, StringComparison.OrdinalIgnoreCase);
                if (innerLength == -1)
                {
                    return null;
                }

                var innerEnd = innerStart + innerLength;


                // The span must contain at least one character.
                if (innerStart == innerEnd)
                {
                    return null;
                }

                // The first character inside the span must NOT be a space.
                if (ParseHelpers.IsMarkdownWhiteSpace(markdown[innerStart]))
                {
                    return null;
                }

                // The last character inside the span must NOT be a space.
                if (ParseHelpers.IsMarkdownWhiteSpace(markdown[innerEnd - 1]))
                {
                    return null;
                }

                var emojiName = markdown.Substring(innerStart, innerEnd - innerStart);

                if (_emojiCodesDictionary.TryGetValue(emojiName, out var emojiCode))
                {
                    var result = new EmojiInline { Text = char.ConvertFromUtf32(emojiCode), Type = MarkdownInlineType.Emoji };
                    return InlineParseResult.Create(result, tripPos, innerEnd + 1);
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