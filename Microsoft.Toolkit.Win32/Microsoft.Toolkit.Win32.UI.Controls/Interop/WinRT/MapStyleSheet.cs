// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT
{
    /// <summary>
    /// <see cref="Windows.UI.Xaml.Controls.Maps.MapStyleSheet"/>
    /// </summary>
    public class MapStyleSheet
    {
        private Windows.UI.Xaml.Controls.Maps.MapStyleSheet UwpInstance { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MapStyleSheet"/> class, a
        /// Wpf-enabled wrapper for <see cref="Windows.UI.Xaml.Controls.Maps.MapStyleSheet"/>
        /// </summary>
        public MapStyleSheet(Windows.UI.Xaml.Controls.Maps.MapStyleSheet instance)
        {
            this.UwpInstance = instance;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="Windows.UI.Xaml.Controls.Maps.MapStyleSheet"/> to <see cref="MapStyleSheet"/>.
        /// </summary>
        /// <param name="args">The <see cref="Windows.UI.Xaml.Controls.Maps.MapStyleSheet"/> instance containing the event data.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator MapStyleSheet(
            Windows.UI.Xaml.Controls.Maps.MapStyleSheet args)
        {
            return FromMapStyleSheet(args);
        }

        /// <summary>
        /// Creates a <see cref="MapStyleSheet"/> from <see cref="Windows.UI.Xaml.Controls.Maps.MapStyleSheet"/>.
        /// </summary>
        /// <param name="args">The <see cref="Windows.UI.Xaml.Controls.Maps.MapStyleSheet"/> instance containing the event data.</param>
        /// <returns><see cref="MapStyleSheet"/></returns>
        public static MapStyleSheet FromMapStyleSheet(Windows.UI.Xaml.Controls.Maps.MapStyleSheet args)
        {
            return new MapStyleSheet(args);
        }
    }
}