// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT
{
    /// <summary>
    /// <see cref="Windows.UI.Xaml.Media.FontFamily"/>
    /// </summary>
    public class FontFamily
    {
        private Windows.UI.Xaml.Media.FontFamily UwpInstance { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FontFamily"/> class, a
        /// Wpf-enabled wrapper for <see cref="Windows.UI.Xaml.Media.FontFamily"/>
        /// </summary>
        public FontFamily(Windows.UI.Xaml.Media.FontFamily instance)
        {
            this.UwpInstance = instance;
        }

        /// <summary>
        /// Gets <see cref="Windows.UI.Xaml.Media.FontFamily.Source"/>
        /// </summary>
        public string Source
        {
            get => UwpInstance.Source;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="Windows.UI.Xaml.Media.FontFamily"/> to <see cref="FontFamily"/>.
        /// </summary>
        /// <param name="args">The <see cref="Windows.UI.Xaml.Media.FontFamily"/> instance containing the event data.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator FontFamily(
            Windows.UI.Xaml.Media.FontFamily args)
        {
            return FromFontFamily(args);
        }

        /// <summary>
        /// Creates a <see cref="FontFamily"/> from <see cref="Windows.UI.Xaml.Media.FontFamily"/>.
        /// </summary>
        /// <param name="args">The <see cref="Windows.UI.Xaml.Media.FontFamily"/> instance containing the event data.</param>
        /// <returns><see cref="FontFamily"/></returns>
        public static FontFamily FromFontFamily(Windows.UI.Xaml.Media.FontFamily args)
        {
            return new FontFamily(args);
        }
    }
}