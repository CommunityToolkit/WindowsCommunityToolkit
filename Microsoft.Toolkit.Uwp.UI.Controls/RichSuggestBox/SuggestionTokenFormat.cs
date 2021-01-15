// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Windows.UI;
using Windows.UI.Text;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    public class SuggestionTokenFormat
    {
        public Color Foreground { get; set; }

        public Color Background { get; set; }

        public FormatEffect Italic { get; set; }

        public FormatEffect Bold { get; set; }

        public UnderlineType Underline { get; set; }
    }
}
