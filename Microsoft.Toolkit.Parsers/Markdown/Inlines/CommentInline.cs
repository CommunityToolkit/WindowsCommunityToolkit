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
            protected override InlineParseResult<CommentInline> ParseInternal(LineBlock markdown, int tripLine, int tripPos, MarkdownDocument document, IEnumerable<Type> ignoredParsers)
            {
                if (tripPos >= maxEnd - 1)
                {
                    return null;
                }

                var startSequence = markdown.AsSpan(tripPos);
                if (!startSequence.StartsWith("<!--".AsSpan()))
                {
                    return null;
                }

                // Find the end of the span.  The end sequence ('-->')
                var innerStart = tripPos + 4;
                int innerEnd = Common.IndexOf(markdown, "-->", innerStart, maxEnd);
                if (innerEnd == -1)
                {
                    return null;
                }

                var length = innerEnd - innerStart;
                var contents = markdown.Substring(innerStart, length);

                var result = new CommentInline
                {
                    Text = contents
                };

                return InlineParseResult.Create(result, tripPos, innerEnd + 3);
            }

            /// <inheritdoc/>
            public override IEnumerable<char> TripChar => "<";
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