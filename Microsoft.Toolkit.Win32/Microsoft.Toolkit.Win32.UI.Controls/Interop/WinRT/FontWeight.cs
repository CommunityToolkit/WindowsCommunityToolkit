// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT
{
    /// <summary>
    /// <see cref="Windows.UI.Text.FontWeight"/>
    /// </summary>
    public class FontWeight
    {
        private Windows.UI.Text.FontWeight UwpInstance { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FontWeight"/> class, a
        /// Wpf-enabled wrapper for <see cref="Windows.UI.Text.FontWeight"/>
        /// </summary>
        public FontWeight(Windows.UI.Text.FontWeight instance)
        {
            this.UwpInstance = instance;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="Windows.UI.Text.FontWeight"/> to <see cref="FontWeight"/>.
        /// </summary>
        /// <param name="args">The <see cref="Windows.UI.Text.FontWeight"/> instance containing the event data.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator FontWeight(
            Windows.UI.Text.FontWeight args)
        {
            return FromFontWeight(args);
        }

        /// <summary>
        /// Creates a <see cref="FontWeight"/> from <see cref="Windows.UI.Text.FontWeight"/>.
        /// </summary>
        /// <param name="args">The <see cref="Windows.UI.Text.FontWeight"/> instance containing the event data.</param>
        /// <returns><see cref="FontWeight"/></returns>
        public static FontWeight FromFontWeight(Windows.UI.Text.FontWeight args)
        {
            return new FontWeight(args);
        }
    }
}