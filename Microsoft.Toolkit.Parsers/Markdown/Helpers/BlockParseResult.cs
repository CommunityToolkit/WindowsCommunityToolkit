﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Toolkit.Parsers.Markdown.Blocks;

namespace Microsoft.Toolkit.Parsers.Markdown.Helpers
{
    /// <summary>
    /// Represents the result of parsing an Block element.
    /// </summary>
    public abstract class BlockParseResult
    {
        private protected BlockParseResult(MarkdownBlock parsedElement, int start, int end)
        {
            ParsedElement = parsedElement;
            Start = start;
            End = end;
        }

        /// <summary>
        /// Gets the element that was parsed (can be <c>null</c>).
        /// </summary>
        public MarkdownBlock ParsedElement { get; }

        /// <summary>
        /// Gets the position of the first character in the parsed element.
        /// </summary>
        public int Start { get; }

        /// <summary>
        /// Gets the position of the character after the last character in the parsed element.
        /// </summary>
        public int End { get; }

        /// <summary>
        /// Instanciates an BlockParserResult.
        /// </summary>
        /// <typeparam name="T">The MarkdownBlock type.</typeparam>
        /// <param name="markdownBlock">The parsed Block.</param>
        /// <param name="start">The start of the Block.</param>
        /// <param name="end">The end of the Block.</param>
        /// <returns>The BlockParseResult.</returns>
        public static BlockParseResult<T> Create<T>(T markdownBlock, int start, int end)
            where T : MarkdownBlock
            => new BlockParseResult<T>(markdownBlock, start, end);
    }
}