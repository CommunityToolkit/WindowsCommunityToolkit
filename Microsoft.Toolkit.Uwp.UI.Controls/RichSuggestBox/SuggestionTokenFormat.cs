// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Windows.UI;
using Windows.UI.Text;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// SuggestionTokenFormat describes how a token should be formatted.
    /// </summary>
    public class SuggestionTokenFormat
    {
        /// <summary>
        /// Gets or sets token foreground color.
        /// </summary>
        public Color Foreground { get; set; }

        /// <summary>
        /// Gets or sets token background color.
        /// </summary>
        public Color Background { get; set; }

        /// <summary>
        /// Gets or sets token italic style.
        /// </summary>
        public FormatEffect Italic { get; set; }

        /// <summary>
        /// Gets or sets token bold style.
        /// </summary>
        public FormatEffect Bold { get; set; }

        /// <summary>
        /// Gets or sets token underline style.
        /// </summary>
        public UnderlineType Underline { get; set; }
    }
}
