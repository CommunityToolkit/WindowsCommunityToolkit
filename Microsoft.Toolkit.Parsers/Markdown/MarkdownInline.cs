// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;

namespace Microsoft.Toolkit.Parsers.Markdown.Inlines
{
    /// <summary>
    /// An internal class that is the base class for all inline elements.
    /// </summary>
    public abstract class MarkdownInline : MarkdownElement
    {
        /// <summary>
        /// Gets or sets this element is.
        /// </summary>
        public MarkdownInlineType Type { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MarkdownInline"/> class.
        /// </summary>
        internal MarkdownInline(MarkdownInlineType type)
        {
            Type = type;
        }


        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        /// <returns> A hash code for the current object. </returns>
        public override int GetHashCode()
        {
            return base.GetHashCode() ^ Type.GetHashCode();
        }

        /// <summary>
        /// Helper class to configure default parser behavior
        /// </summary>
        public class DefaultParserConfiguration
        {
            internal List<Type> BeforeParsers { get; }

            internal List<Type> AfterParsers { get; }

            internal DefaultParserConfiguration()
            {
                this.BeforeParsers = new List<Type>();
                this.AfterParsers = new List<Type>();
            }

            /// <summary>
            /// This parser should run before <typeparamref name="T"/>
            /// </summary>
            /// <typeparam name="T">The Parser</typeparam>
            public void Before<T>()
                where T : Parser
            {
                this.BeforeParsers.Add(typeof(T));
            }

            /// <summary>
            /// This parser should run after <typeparamref name="T"/>
            /// </summary>
            /// <typeparam name="T">The Parser</typeparam>
            public void After<T>()
                where T : Parser
            {
                this.AfterParsers.Add(typeof(T));
            }
        }

        /// <summary>
        /// An Abstract base class of Block Parsers
        /// </summary>
        public abstract class Parser
        {
            internal Parser()
            {
                var config = new DefaultParserConfiguration();
                this.ConfigureDefaults(config);
                this.DefaultBeforeParsers = config.BeforeParsers.AsReadOnly();
                this.DefaultAfterParsers = config.AfterParsers.AsReadOnly();
            }

            /// <summary>
            /// Gets the Default ordering of this Parser (ever parser that comes after this one)
            /// </summary>
            public IEnumerable<Type> DefaultBeforeParsers { get; }

            /// <summary>
            /// Gets the Default ordering of this Parser (ever parser that comes before this one)
            /// </summary>
            public IEnumerable<Type> DefaultAfterParsers { get; }

            /// <summary>
            /// Override this Method to order this Parser relative to others.
            /// </summary>
            /// <param name="configuration">A configuration class which methods can be used to order this parser.</param>
            protected virtual void ConfigureDefaults(DefaultParserConfiguration configuration)
            {
            }

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
            /// <param name="lineStartsNewParagraph">Specifies if a new paragraph will start.</param>
            /// <param name="document">The Document which is parsing</param>
            /// <returns>The Parsed block. <code>null</code> if the text does not this block.</returns>
            public abstract MarkdownInline Parse(string markdown, int startOfLine, int firstNonSpace, int realStartOfLine, int endOfFirstLine, int maxEnd, int quoteDepth, out int actualEnd, StringBuilder paragraphText, bool lineStartsNewParagraph, MarkdownDocument document);
        }

        /// <summary>
        /// An Abstract Base class for parsing Blocks
        /// </summary>
        /// <typeparam name="TBlock">The Type of Block that will be parsed.</typeparam>
        public abstract class Parser<TBlock> : Parser
            where TBlock : MarkdownInline
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
            /// <param name="lineStartsNewParagraph">Specifies if a new paragraph will start.</param>
            /// <param name="document">The Document which is parsing</param>
            /// <returns>The Parsed block. <code>null</code> if the text does not this block.</returns>
            protected abstract TBlock ParseInternal(string markdown, int startOfLine, int firstNonSpace, int realStartOfLine, int endOfFirstLine, int maxEnd, int quoteDepth, out int actualEnd, StringBuilder paragraphText, bool lineStartsNewParagraph, MarkdownDocument document);

            /// <inheritdoc/>
            public sealed override MarkdownInline Parse(string markdown, int startOfLine, int firstNonSpace, int realStartOfLine, int endOfFirstLine, int maxEnd, int quoteDepth, out int actualEnd, StringBuilder paragraphText, bool lineStartsNewParagraph, MarkdownDocument document) => this.ParseInternal(markdown, startOfLine, firstNonSpace, realStartOfLine, endOfFirstLine, maxEnd, quoteDepth, out actualEnd, paragraphText, lineStartsNewParagraph, document);
        }
    }
}