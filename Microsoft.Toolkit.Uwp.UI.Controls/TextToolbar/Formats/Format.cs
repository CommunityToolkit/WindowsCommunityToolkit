// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.Toolkit.Uwp.UI.Controls.TextToolbarFormats
{
    /// <summary>
    /// Identifies the Format to be used by <see cref="TextToolbar"/>
    /// </summary>
    public enum Format
    {
        /// <summary>
        /// Utilises the Built-In RichText Formatter
        /// </summary>
        RichText,

        /// <summary>
        /// Utilises the Built-In Markdown Formatter
        /// </summary>
        MarkDown,

        /// <summary>
        /// Utilises the provided Custom Formatter using the Formatter Property
        /// </summary>
        Custom
    }
}