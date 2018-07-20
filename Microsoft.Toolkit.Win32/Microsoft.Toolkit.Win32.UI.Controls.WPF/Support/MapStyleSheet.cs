// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.Toolkit.Win32.UI.Controls.WPF
{
    /// <summary>
    /// <see cref="global::Windows.UI.Xaml.Controls.Maps.MapStyleSheet"/>
    /// </summary>
    public class MapStyleSheet
    {
        internal global::Windows.UI.Xaml.Controls.Maps.MapStyleSheet UwpInstance { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MapStyleSheet"/> class, a
        /// Wpf-enabled wrapper for <see cref="global::Windows.UI.Xaml.Controls.Maps.MapStyleSheet"/>
        /// </summary>
        public MapStyleSheet(global::Windows.UI.Xaml.Controls.Maps.MapStyleSheet instance)
        {
            this.UwpInstance = instance;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="global::Windows.UI.Xaml.Controls.Maps.MapStyleSheet"/> to <see cref="Microsoft.Toolkit.Win32.UI.Controls.WPF.MapStyleSheet"/>.
        /// </summary>
        /// <param name="args">The <see cref="global::Windows.UI.Xaml.Controls.Maps.MapStyleSheet"/> instance containing the event data.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator MapStyleSheet(
            global::Windows.UI.Xaml.Controls.Maps.MapStyleSheet args)
        {
            return FromMapStyleSheet(args);
        }

        /// <summary>
        /// Creates a <see cref="MapStyleSheet"/> from <see cref="global::Windows.UI.Xaml.Controls.Maps.MapStyleSheet"/>.
        /// </summary>
        /// <param name="args">The <see cref="global::Windows.UI.Xaml.Controls.Maps.MapStyleSheet"/> instance containing the event data.</param>
        /// <returns><see cref="MapStyleSheet"/></returns>
        public static MapStyleSheet FromMapStyleSheet(global::Windows.UI.Xaml.Controls.Maps.MapStyleSheet args)
        {
            return new MapStyleSheet(args);
        }
    }
}