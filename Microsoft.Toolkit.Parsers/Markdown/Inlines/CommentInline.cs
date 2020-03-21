// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using Microsoft.Toolkit.Parsers.Markdown.Helpers;

namespace Microsoft.Toolkit.Parsers.Markdown.Inlines
{
    /// <summary>
    /// Represents a span that contains comment.
    /// </summary>
    public class CommentInline : MarkdownInline, IInlineLeaf
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommentInline"/> class.
        /// </summary>
        public CommentInline()
            : base(MarkdownInlineType.Comment)
        {
        }

        /// <summary>
        /// Gets or sets the Content of the Comment.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Attempts to parse a comment text span.
        /// </summary>
        public new class Parser : Parser<CommentInline>
        {
            /// <inheritdoc/>
            protected override InlineParseResult<CommentInline> ParseInternal(in LineBlock markdown, LineBlockPosition tripPos, MarkdownDocument document, HashSet<Type> ignoredParsers)
            {
                if (!tripPos.IsIn(markdown))
                {
                    throw new ArgumentOutOfRangeException(nameof(tripPos));
                }

                var line = markdown[tripPos.Line];

                var startSequence = line.Slice(tripPos.Column);
                if (!startSequence.StartsWith("<!--".AsSpan()))
                {
                    return null;
                }

                // Find the end of the span.  The end sequence ('-->')
                var subBlock = markdown.SliceText(tripPos).SliceText(4);

                var innerEnd = subBlock.IndexOf("-->".AsSpan());
                if (innerEnd == LineBlockPosition.NotFound)
                {
                    return null;
                }

                var contents = subBlock.SliceText(0, innerEnd.FromStart).ToString();

                var result = new CommentInline
                {
                    Text = contents,
                };

                return InlineParseResult.Create(result, tripPos, innerEnd.FromStart + 3);
            }

            /// <inheritdoc/>
            public override ReadOnlySpan<char> TripChar => "<".AsSpan();
        }

        /// <summary>
        /// Converts the object into it's textual representation.
        /// </summary>
        /// <returns> The textual representation of this object. </returns>
        public override string ToString()
        {
            return "<!--" + Text + "-->";
        }
    }
}