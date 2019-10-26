// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Microsoft.Toolkit.Parsers.Markdown.Blocks;

namespace Microsoft.Toolkit.Parsers.Markdown
{
    public interface IDocumentBuilder
    {
       MarkdownDocument.DocumentBuilder.DocumentBuilderConfigurator<TFactory> AddParser<TFactory>(Action<TFactory> configurationCallback = null) where TFactory : MarkdownBlock.Factory, new();
        MarkdownDocument Build();
        MarkdownDocument.DocumentBuilder RemoveParser<TFactory>()
            where TFactory : MarkdownBlock.Factory, new();
    }

}