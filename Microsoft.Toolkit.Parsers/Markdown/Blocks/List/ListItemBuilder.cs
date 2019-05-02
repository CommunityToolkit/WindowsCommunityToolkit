// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Text;

namespace Microsoft.Toolkit.Parsers.Markdown.Blocks
{
    internal class ListItemBuilder : MarkdownBlock
    {
        public StringBuilder Builder { get; } = new StringBuilder();

        public ListItemBuilder()
            : base(MarkdownBlockType.ListItemBuilder)
        {
        }
    }
}