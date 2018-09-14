// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT
{
    /// <summary>
    /// Provides data for events. This class cannot be inherited.
    /// </summary>
    /// <remarks>Copy from <see cref="Windows.UI.Xaml.Controls.Maps.MapRightTappedEventArgs"/> to avoid requirement to link Windows.winmd</remarks>
    /// <seealso cref="Windows.UI.Xaml.Controls.Maps.MapRightTappedEventArgs"/>
    public sealed class MapRightTappedEventArgs : EventArgs
    {
        private readonly Windows.UI.Xaml.Controls.Maps.MapRightTappedEventArgs _args;

        internal MapRightTappedEventArgs(Windows.UI.Xaml.Controls.Maps.MapRightTappedEventArgs args)
        {
            _args = args;
        }

        public Windows.Devices.Geolocation.Geopoint Location
        {
            get => (Windows.Devices.Geolocation.Geopoint)_args.Location;
        }

        public Windows.Foundation.Point Position
        {
            get => (Windows.Foundation.Point)_args.Position;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="Windows.UI.Xaml.Controls.Maps.MapRightTappedEventArgs"/> to <see cref="Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT.MapRightTappedEventArgs"/>.
        /// </summary>
        /// <param name="args">The <see cref="Windows.UI.Xaml.Controls.Maps.MapRightTappedEventArgs"/> instance containing the event data.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator MapRightTappedEventArgs(
            Windows.UI.Xaml.Controls.Maps.MapRightTappedEventArgs args)
        {
            return FromMapRightTappedEventArgs(args);
        }

        /// <summary>
        /// Creates a <see cref="MapRightTappedEventArgs"/> from <see cref="Windows.UI.Xaml.Controls.Maps.MapRightTappedEventArgs"/>.
        /// </summary>
        /// <param name="args">The <see cref="Windows.UI.Xaml.Controls.Maps.MapRightTappedEventArgs"/> instance containing the event data.</param>
        /// <returns><see cref="MapRightTappedEventArgs"/></returns>
        public static MapRightTappedEventArgs FromMapRightTappedEventArgs(Windows.UI.Xaml.Controls.Maps.MapRightTappedEventArgs args)
        {
            return new MapRightTappedEventArgs(args);
        }
    }
}