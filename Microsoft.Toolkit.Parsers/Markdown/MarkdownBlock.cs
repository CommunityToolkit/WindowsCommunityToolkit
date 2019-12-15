// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using Microsoft.Toolkit.Parsers.Markdown.Helpers;

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
            private IEnumerable<Type> defaultAfterParsers;
            private IEnumerable<Type> defaultBeforeParsers;

            internal Parser()
            {
            }

            private IEnumerable<Type> InitBefore()
            {
                var config = new DefaultParserConfiguration();
                this.ConfigureDefaults(config);
                return config.BeforeParsers.AsReadOnly();
            }

            private IEnumerable<Type> InitAfter()
            {
                var config = new DefaultParserConfiguration();
                this.ConfigureDefaults(config);
                return config.AfterParsers.AsReadOnly();
            }

            /// <summary>
            /// Gets the Default ordering of this Parser (ever parser that comes after this one)
            /// </summary>
            public IEnumerable<Type> DefaultBeforeParsers { get => defaultBeforeParsers ?? (defaultBeforeParsers = InitBefore()); }

            /// <summary>
            /// Gets the Default ordering of this Parser (ever parser that comes before this one)
            /// </summary>
            public IEnumerable<Type> DefaultAfterParsers { get => defaultAfterParsers ?? (defaultAfterParsers = InitAfter()); }

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
            /// <param name="endOfFirstLine">The position of the end of the line</param>
            /// <param name="maxStart">The furthest the parser may look before the current position</param>
            /// <param name="maxEnd">The maximum position untill we parsed.</param>
            /// <param name="lineStartsNewParagraph">Specifies if a new paragraph will start.</param>
            /// <param name="document">The Document which is parsing</param>
            /// <returns>The Parsed block. <code>null</code> if the text does not this block.</returns>
            public abstract BlockParseResult Parse(string markdown, int startOfLine, int firstNonSpace, int endOfFirstLine, int maxStart, int maxEnd, bool lineStartsNewParagraph, MarkdownDocument document);
        }

        /// <summary>
        /// An Abstract Base class for parsing Blocks
        /// </summary>
        /// <typeparam name="TBlock">The Type of Block that will be parsed.</typeparam>
        public abstract class Parser<TBlock> : Parser
            where TBlock : MarkdownBlock
        {
            /// <summary>
            /// Parse a block.
            /// </summary>
            /// <param name="markdown">The markdown text. </param>
            /// <param name="startOfLine">The location of the first hash character (without quotes). </param>
            /// <param name="firstNonSpace">The first character that is not a space.</param>
            /// <param name="endOfFirstLine">The position of the end of the line</param>
            /// <param name="maxStart">The furthest the parser may look before the current position</param>
            /// <param name="maxEnd">The maximum position untill we parsed.</param>
            /// <param name="lineStartsNewParagraph">Specifies if a new paragraph will start.</param>
            /// <param name="document">The Document which is parsing</param>
            /// <returns>The Parsed block. <code>null</code> if the text does not this block.</returns>
            protected abstract BlockParseResult<TBlock> ParseInternal(string markdown, int startOfLine, int firstNonSpace, int endOfFirstLine, int maxStart, int maxEnd, bool lineStartsNewParagraph, MarkdownDocument document);

            /// <inheritdoc/>
            public sealed override BlockParseResult Parse(string markdown, int startOfLine, int firstNonSpace, int endOfFirstLine, int maxStart, int maxEnd, bool lineStartsNewParagraph, MarkdownDocument document) => this.ParseInternal(markdown, startOfLine, firstNonSpace, endOfFirstLine, maxStart, maxEnd, lineStartsNewParagraph, document);
        }
    }
}