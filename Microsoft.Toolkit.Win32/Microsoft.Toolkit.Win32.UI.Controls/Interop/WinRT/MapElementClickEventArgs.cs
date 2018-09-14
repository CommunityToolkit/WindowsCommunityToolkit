using System;
using System.Security;

namespace Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT
{
    /// <summary>
    /// Provides data for events. This class cannot be inherited.
    /// </summary>
    /// <remarks>Copy from <see cref="Windows.UI.Xaml.Controls.Maps.MapElementClickEventArgs"/> to avoid requirement to link Windows.winmd</remarks>
    /// <seealso cref="Windows.UI.Xaml.Controls.Maps.MapElementClickEventArgs"/>
    public sealed class MapElementClickEventArgs : EventArgs
    {
        private readonly Windows.UI.Xaml.Controls.Maps.MapElementClickEventArgs _args;

        internal MapElementClickEventArgs(Windows.UI.Xaml.Controls.Maps.MapElementClickEventArgs args)
        {
            _args = args;
        }

        public Windows.Devices.Geolocation.Geopoint Location
        {
            get => (Windows.Devices.Geolocation.Geopoint)_args.Location;
        }

        public System.Collections.Generic.IList<Windows.UI.Xaml.Controls.Maps.MapElement> MapElements
        {
            get => (System.Collections.Generic.IList<Windows.UI.Xaml.Controls.Maps.MapElement>)_args.MapElements;
        }

        public Windows.Foundation.Point Position
        {
            get => (Windows.Foundation.Point)_args.Position;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="Windows.UI.Xaml.Controls.Maps.MapElementClickEventArgs"/> to <see cref="Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT.MapElementClickEventArgs"/>.
        /// </summary>
        /// <param name="args">The <see cref="Windows.UI.Xaml.Controls.Maps.MapElementClickEventArgs"/> instance containing the event data.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator MapElementClickEventArgs(
            Windows.UI.Xaml.Controls.Maps.MapElementClickEventArgs args)
        {
            return FromMapElementClickEventArgs(args);
        }

        /// <summary>
        /// Creates a <see cref="MapElementClickEventArgs"/> from <see cref="Windows.UI.Xaml.Controls.Maps.MapElementClickEventArgs"/>.
        /// </summary>
        /// <param name="args">The <see cref="Windows.UI.Xaml.Controls.Maps.MapElementClickEventArgs"/> instance containing the event data.</param>
        /// <returns><see cref="MapElementClickEventArgs"/></returns>
        public static MapElementClickEventArgs FromMapElementClickEventArgs(Windows.UI.Xaml.Controls.Maps.MapElementClickEventArgs args)
        {
            return new MapElementClickEventArgs(args);
        }
    }
}