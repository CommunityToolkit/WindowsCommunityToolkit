// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.Toolkit.Parsers.Markdown
{
    /// <summary>
    /// The alignment of content in a table column.
    /// </summary>
    public enum ColumnAlignment
    {
        /// <summary>
        /// The alignment was not specified.
        /// </summary>
        Unspecified = 0,

        /// <summary>
        /// Content should be left aligned.
        /// </summary>
        Left = 1,

        /// <summary>
        /// Content should be right aligned.
        /// </summary>
        Right = 2,

        /// <summary>
        /// Content should be centered.
        /// </summary>
        Center = 3,
    }
}