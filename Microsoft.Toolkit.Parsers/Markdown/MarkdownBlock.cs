// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Toolkit.Parsers.Markdown.Blocks
{
    /// <summary>
    /// A Block Element is an element that is a container for other structures.
    /// </summary>
    public abstract class MarkdownBlock : MarkdownElement
    {
        /// <summary>
        /// Gets or sets tells us what type this element is.
        /// </summary>
        public MarkdownBlockType Type { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MarkdownBlock"/> class.
        /// </summary>
        internal MarkdownBlock(MarkdownBlockType type)
        {
            Type = type;
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="obj"> The object to compare with the current object. </param>
        /// <returns> <c>true</c> if the specified object is equal to the current object; otherwise, <c>false.</c> </returns>
        public override bool Equals(object obj)
        {
            if (!base.Equals(obj) || !(obj is MarkdownBlock))
            {
                return false;
            }

            return Type == ((MarkdownBlock)obj).Type;
        }

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        /// <returns> A hash code for the current object. </returns>
        public override int GetHashCode()
        {
            return base.GetHashCode() ^ Type.GetHashCode();
        }

        public abstract class Factory
        {

            internal Factory() { }
            public virtual IEnumerable<Type> DefaultBeforeFactorys => Array.Empty<Type>();
            public virtual IEnumerable<Type> DefaultAfterFactorys => Array.Empty<Type>();

            public abstract MarkdownBlock Parse(string markdown, int startOfLine, int firstNonSpace, int realStartOfLine, int endOfFirstLine, int maxEnd, int quoteDepth, out int actualEnd, StringBuilder paragraphText, bool lineStartsNewParagraph, MarkdownDocument document);

        }

        public abstract class Factory<TBlock> : Factory
            where TBlock : MarkdownBlock
        {

            /// <summary>
            /// Parse a block.
            /// </summary>
            /// <param name="markdown">The markdown text. </param>
            /// <param name="startOfLine">The location of the first hash character (without quotes). </param>
            /// <param name="firstNonSpace">The first character that is not a space.</param>
            /// <param name="realStartOfLine">The position of the Start of the line including the qute characters.</param>
            /// <param name="endOfFirstLine">The position of the end of the line</param>
            /// <param name="maxEnd">The maximum position untill we parsed.</param>
            /// <param name="quoteDepth">Current quote characters</param>
            /// <param name="actualEnd">The position untill this block was parsed.</param>
            /// <param name="paragraphText">The text that was parsed before the block, but was not yed assigned a block</param>
            /// <param name="lineStartsNewParagraph"></param>
            /// <returns>The Parsed block. <code>null</code> if the text does not this block.</returns>

            protected abstract TBlock ParseInternal(string markdown, int startOfLine, int firstNonSpace, int realStartOfLine, int endOfFirstLine, int maxEnd, int quoteDepth, out int actualEnd, StringBuilder paragraphText, bool lineStartsNewParagraph, MarkdownDocument document);

            public override MarkdownBlock Parse(string markdown, int startOfLine, int firstNonSpace, int realStartOfLine, int endOfFirstLine, int maxEnd, int quoteDepth, out int actualEnd, StringBuilder paragraphText, bool lineStartsNewParagraph, MarkdownDocument document) => this.ParseInternal(markdown, startOfLine, firstNonSpace, realStartOfLine, endOfFirstLine, maxEnd, quoteDepth, out actualEnd, paragraphText, lineStartsNewParagraph, document);
        }
    }
}