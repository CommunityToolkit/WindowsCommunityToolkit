// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    /// <summary>
    /// Sample of strongly-typed data for <see cref="Microsoft.Toolkit.Uwp.UI.Controls.TokenizingTextBox"/>.
    /// </summary>
    public class SampleDataType
    {
        /// <summary>
        /// Gets or sets symbol to display.
        /// </summary>
        public Symbol Icon { get; set; }

        /// <summary>
        /// Gets or sets text to display.
        /// </summary>
        public string Text { get; set; }

        public override string ToString()
        {
            return "Sample Data: " + Text;
        }
    }
}
