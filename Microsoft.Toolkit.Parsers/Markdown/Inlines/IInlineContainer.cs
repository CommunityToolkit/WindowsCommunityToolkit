// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;

namespace Microsoft.Toolkit.Parsers.Markdown.Inlines
{
    /// <summary>
    /// Initializes a new instance of the <see cref="IInlineContainer"/> class.
    /// </summary>
    public interface IInlineContainer
    {
        /// <summary>
        /// Gets or sets the contents of the inline.
        /// </summary>
        IList<MarkdownInline> Inlines { get; set; }
    }
}