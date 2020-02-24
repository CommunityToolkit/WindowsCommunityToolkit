﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Microsoft.Toolkit.Parsers.Markdown.Blocks;
using Microsoft.Toolkit.Parsers.Markdown.Inlines;

namespace Microsoft.Toolkit.Parsers.Markdown
{
    /// <summary>
    /// Defines the Builder methods to create a MarkdownDocument.
    /// </summary>
    public interface IDocumentBuilder
    {
        /// <summary>
        /// Add a Parser with an optional configuration. Every Parser may only be used once.
        /// </summary>
        /// <typeparam name="TParser">The Parser of a Block.</typeparam>
        /// <param name="configurationCallback">A callback to configure the instance of the parser.</param>
        /// <returns>This instance.</returns>
        MarkdownDocument.DocumentBuilder.DocumentBuilderBlockConfigurator<TParser> AddBlockParser<TParser>(Action<TParser> configurationCallback = null)
            where TParser : MarkdownBlock.Parser, new();

        /// <summary>
        /// Add a Parser with an optional configuration. Every Parser may only be used once.
        /// </summary>
        /// <typeparam name="TParser">The Parser of an Inline.</typeparam>
        /// <param name="configurationCallback">A callback to configure the instance of the parser.</param>
        /// <returns>This instance.</returns>
        MarkdownDocument.DocumentBuilder.DocumentBuilderInlineConfigurator<TParser> AddInlineParser<TParser>(Action<TParser> configurationCallback = null)
            where TParser : MarkdownInline.Parser, new();

        /// <summary>
        /// Creates the Markdown Document.
        /// </summary>
        /// <returns>The MarkdownDocument.</returns>
        MarkdownDocument Build();

        /// <summary>
        /// Removes a Parser. This will no longer be used when parsing with the MarkdownDocument.
        /// If the Parser is not present this method does nothing.
        /// </summary>
        /// <typeparam name="TParser">The Parser of a Block.</typeparam>
        /// <returns>This instance.</returns>
        MarkdownDocument.DocumentBuilder RemoveBlockParser<TParser>()
            where TParser : MarkdownBlock.Parser, new();

        /// <summary>
        /// Removes a Parser. This will no longer be used when parsing with the MarkdownDocument.
        /// If the Parser is not present this method does nothing.
        /// </summary>
        /// <typeparam name="TParser">The Parser of an Inline.</typeparam>
        /// <returns>This instance.</returns>
        MarkdownDocument.DocumentBuilder RemoveInlineParser<TParser>()
            where TParser : MarkdownInline.Parser, new();
    }
}