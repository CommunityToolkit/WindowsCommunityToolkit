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
            /// <summary>
            /// Gets or sets a value indicating whether gets a value indicating if the default emojis should be used.
            /// </summary>
            public bool UseDefaultEmojis { get; set; } = true;

            private readonly Dictionary<string, int> customEmojiCodesDictionary = new Dictionary<string, int>();
            private readonly int maxEmojiLength;
            private int maxCustomEmojiLength;

            /// <summary>
            /// Initializes a new instance of the <see cref="Parser"/> class.
            /// </summary>
            public Parser()
            {
                this.maxEmojiLength = _emojiCodesDictionary.Keys.Max(x => x.Length);
            }

            /// <summary>
            /// Adds an Emoji sequence.
            /// </summary>
            /// <param name="name">The sequence that should be machted. (without collon.)</param>
            /// <param name="character">The number representing the unicode character.</param>
            public void AddEmoji(string name, int character)
            {
                this.customEmojiCodesDictionary.Add(name, character);
                this.maxCustomEmojiLength = Math.Max(name.Length, this.maxCustomEmojiLength);
            }

            /// <inheritdoc/>
            protected override InlineParseResult<EmojiInline> ParseInternal(in LineBlock markdown, LineBlockPosition tripPos, MarkdownDocument document, HashSet<Type> ignoredParsers)
            {
                var line = markdown[tripPos.Line];

                // Check the start sequence.
                var startSequence = line.Slice(tripPos.Column, 1);

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

                // Can this be a emoji?
                if (innerLength > this.maxCustomEmojiLength && (innerLength > this.maxEmojiLength || !this.UseDefaultEmojis))
                {
                    return null;
                }

                var emojiName = line.Slice(tripPos.Column + 1, innerLength).ToString();

                if (this.customEmojiCodesDictionary.TryGetValue(emojiName, out var emojiCode))
                {
                    var result = new EmojiInline { Text = char.ConvertFromUtf32(emojiCode), Type = MarkdownInlineType.Emoji };
                    return InlineParseResult.Create(result, tripPos, innerLength + 2);
                }

                if (this.UseDefaultEmojis && _emojiCodesDictionary.TryGetValue(emojiName, out emojiCode))
                {
                    var result = new EmojiInline { Text = char.ConvertFromUtf32(emojiCode), Type = MarkdownInlineType.Emoji };
                    return InlineParseResult.Create(result, tripPos, innerLength + 2);
                }

                return null;
            }

            /// <inheritdoc/>
            public override ReadOnlySpan<char> TripChar => ":".AsSpan();
        }

        /// <inheritdoc/>
        public string Text { get; set; }
    }
}