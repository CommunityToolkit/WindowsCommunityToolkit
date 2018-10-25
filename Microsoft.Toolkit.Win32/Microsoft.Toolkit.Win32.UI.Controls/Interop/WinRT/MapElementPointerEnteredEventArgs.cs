// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT
{
    /// <summary>
    /// Provides data for events. This class cannot be inherited.
    /// </summary>
    /// <remarks>Copy from <see cref="Windows.UI.Xaml.Controls.Maps.MapElementPointerEnteredEventArgs"/> to avoid requirement to link Windows.winmd</remarks>
    /// <seealso cref="Windows.UI.Xaml.Controls.Maps.MapElementPointerEnteredEventArgs"/>
    public sealed class MapElementPointerEnteredEventArgs : EventArgs
    {
        private readonly Windows.UI.Xaml.Controls.Maps.MapElementPointerEnteredEventArgs _args;

        internal MapElementPointerEnteredEventArgs(Windows.UI.Xaml.Controls.Maps.MapElementPointerEnteredEventArgs args)
        {
            _args = args;
        }

        public Windows.Devices.Geolocation.Geopoint Location
        {
            get => (Windows.Devices.Geolocation.Geopoint)_args.Location;
        }

        public Windows.UI.Xaml.Controls.Maps.MapElement MapElement
        {
            get => (Windows.UI.Xaml.Controls.Maps.MapElement)_args.MapElement;
        }

        public Windows.Foundation.Point Position
        {
            get => (Windows.Foundation.Point)_args.Position;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="Windows.UI.Xaml.Controls.Maps.MapElementPointerEnteredEventArgs"/> to <see cref="Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT.MapElementPointerEnteredEventArgs"/>.
        /// </summary>
        /// <param name="args">The <see cref="Windows.UI.Xaml.Controls.Maps.MapElementPointerEnteredEventArgs"/> instance containing the event data.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator MapElementPointerEnteredEventArgs(
            Windows.UI.Xaml.Controls.Maps.MapElementPointerEnteredEventArgs args)
        {
            return FromMapElementPointerEnteredEventArgs(args);
        }

        /// <summary>
        /// Creates a <see cref="MapElementPointerEnteredEventArgs"/> from <see cref="Windows.UI.Xaml.Controls.Maps.MapElementPointerEnteredEventArgs"/>.
        /// </summary>
        /// <param name="args">The <see cref="Windows.UI.Xaml.Controls.Maps.MapElementPointerEnteredEventArgs"/> instance containing the event data.</param>
        /// <returns><see cref="MapElementPointerEnteredEventArgs"/></returns>
        public static MapElementPointerEnteredEventArgs FromMapElementPointerEnteredEventArgs(Windows.UI.Xaml.Controls.Maps.MapElementPointerEnteredEventArgs args)
        {
            return new MapElementPointerEnteredEventArgs(args);
        }
    }
}