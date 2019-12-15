// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Toolkit.Parsers.Markdown.Blocks;

namespace Microsoft.Toolkit.Parsers.Markdown.Helpers
{
    /// <summary>
    /// Represents the result of parsing an Block element.
    /// </summary>
    /// <typeparam name="T">The type of the parsed Block.</typeparam>
    public class BlockParseResult<T> : BlockParseResult
        where T : MarkdownBlock
    {
        internal BlockParseResult(T parsedElement, int start, int end)
            : base(parsedElement, start, end)
        {
        }

        /// <summary>
        /// Gets the element that was parsed (can be <c>null</c>).
        /// </summary>
        public new T ParsedElement => (T)base.ParsedElement;
    }
}