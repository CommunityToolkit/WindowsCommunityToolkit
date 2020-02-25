// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using Microsoft.Toolkit.Parsers.Markdown.Helpers;

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
        /// Initializes a new instance of the <see cref="MarkdownInline"/> class with Type <see cref="MarkdownInlineType.Other"/>.
        /// </summary>
        public MarkdownInline()
            : this(MarkdownInlineType.Other)
        {
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
        /// Helper class to configure default parser behavior.
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
            /// This parser should run before <typeparamref name="T"/>.
            /// </summary>
            /// <typeparam name="T">The Parser.</typeparam>
            public void Before<T>()
                where T : Parser
            {
                this.BeforeParsers.Add(typeof(T));
            }

            /// <summary>
            /// This parser should run after <typeparamref name="T"/>.
            /// </summary>
            /// <typeparam name="T">The Parser.</typeparam>
            public void After<T>()
                where T : Parser
            {
                this.AfterParsers.Add(typeof(T));
            }
        }

        /// <summary>
        /// An Abstract base class of Block Parsers.
        /// </summary>
        public abstract class Parser
        {
            private IEnumerable<Type> defaultBeforeParsers;
            private IEnumerable<Type> defaultAfterParsers;

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
            /// Gets the Default ordering of this Parser (ever parser that comes after this one).
            /// </summary>
            public IEnumerable<Type> DefaultBeforeParsers { get => defaultBeforeParsers ?? (defaultBeforeParsers = InitBefore()); }

            /// <summary>
            /// Gets the Default ordering of this Parser (ever parser that comes before this one).
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
            /// Parses an Inline.
            /// </summary>
            /// <param name="markdown">The markdown text.</param>
            /// <param name="tripPos">The position where the triping char matched.</param>
            /// <param name="document">The current parsing document.</param>
            /// <param name="ignoredParsers">Parsers that may not be invoked in subsequent calls.</param>
            /// <returns>The Parsed inline. <code>null</code> if the text does not this inline.</returns>
            /// <remarks>May only be called if TripChar is empty or markdown[tripPos] is contained in TripChar.</remarks>
            public abstract InlineParseResult Parse(LineBlock markdown, LineBlockPosition tripPos, MarkdownDocument document, IEnumerable<Type> ignoredParsers);

            /// <summary>
            /// Gets the chars that if found means we might have a match. Empty if Tripchars are not supported.
            /// </summary>
            public virtual IEnumerable<char> TripChar => Array.Empty<char>();
        }

        /// <summary>
        /// An Abstract Base class for parsing Blocks.
        /// </summary>
        /// <typeparam name="TInline">The Type of inline that will be parsed.</typeparam>
        public abstract class Parser<TInline> : Parser
            where TInline : MarkdownInline
        {
            /// <summary>
            /// Parses an Inline.
            /// </summary>
            /// <param name="markdown">The markdown text.</param>
            /// <param name="tripPos">The position where the triping char matched.</param>
            /// <param name="document">The current parsing document.</param>
            /// <param name="ignoredParsers">Parsers that may not be invoked in subsequent calls.</param>
            /// <returns>The Parsed inline. <code>null</code> if the text does not this inline.</returns>
            protected abstract InlineParseResult<TInline> ParseInternal(LineBlock markdown, LineBlockPosition tripPos, MarkdownDocument document, IEnumerable<Type> ignoredParsers);

            /// <inheritdoc/>
            public sealed override InlineParseResult Parse(LineBlock markdown, LineBlockPosition tripPos, MarkdownDocument document, IEnumerable<Type> ignoredParsers) => this.ParseInternal(markdown, tripPos, document, ignoredParsers);
        }
    }
}