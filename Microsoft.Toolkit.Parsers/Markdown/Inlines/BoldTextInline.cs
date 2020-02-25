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
    /// Represents a span that contains bold text.
    /// </summary>
    public class BoldTextInline : MarkdownInline, IInlineContainer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BoldTextInline"/> class.
        /// </summary>
        public BoldTextInline()
            : base(MarkdownInlineType.Bold)
        {
        }

        /// <summary>
        /// Gets or sets the contents of the inline.
        /// </summary>
        public IList<MarkdownInline> Inlines { get; set; }

        /// <summary>
        /// Attempts to parse a bold text span.
        /// </summary>
        public class ParserAsterix : InlineSourundParser<BoldTextInline>
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="ParserAsterix"/> class.
            /// </summary>
            public ParserAsterix()
                : base("**")
            {
            }

            /// <inheritdoc/>
            protected override BoldTextInline MakeInline(List<MarkdownInline> inlines) => new BoldTextInline
            {
                Inlines = inlines,
            };
        }

        /// <summary>
        /// Attempts to parse a bold text span.
        /// </summary>
        public class ParserUnderscore : InlineSourundParser<BoldTextInline>
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="ParserUnderscore"/> class.
            /// </summary>
            public ParserUnderscore()
                : base("__")
            {
            }

            /// <inheritdoc/>
            protected override BoldTextInline MakeInline(List<MarkdownInline> inlines) => new BoldTextInline
            {
                Inlines = inlines,
            };
        }

        /// <summary>
        /// Converts the object into it's textual representation.
        /// </summary>
        /// <returns> The textual representation of this object. </returns>
        public override string ToString()
        {
            if (Inlines == null)
            {
                return base.ToString();
            }

            return "**" + string.Join(string.Empty, Inlines) + "**";
        }
    }
}
