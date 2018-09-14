using System;

namespace Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT
{
    /// <summary>
    /// Provides data for events. This class cannot be inherited.
    /// </summary>
    /// <remarks>Copy from <see cref="Windows.UI.Xaml.Controls.Maps.MapContextRequestedEventArgs"/> to avoid requirement to link Windows.winmd</remarks>
    /// <seealso cref="Windows.UI.Xaml.Controls.Maps.MapContextRequestedEventArgs"/>
    public sealed class MapContextRequestedEventArgs : EventArgs
    {
        private readonly Windows.UI.Xaml.Controls.Maps.MapContextRequestedEventArgs _args;

        internal MapContextRequestedEventArgs(Windows.UI.Xaml.Controls.Maps.MapContextRequestedEventArgs args)
        {
            _args = args;
        }

        public Windows.Devices.Geolocation.Geopoint Location
        {
            get => (Windows.Devices.Geolocation.Geopoint)_args.Location;
        }

        public System.Collections.Generic.IReadOnlyList<Windows.UI.Xaml.Controls.Maps.MapElement> MapElements
        {
            get => (System.Collections.Generic.IReadOnlyList<Windows.UI.Xaml.Controls.Maps.MapElement>)_args.MapElements;
        }

        public Windows.Foundation.Point Position
        {
            get => (Windows.Foundation.Point)_args.Position;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="Windows.UI.Xaml.Controls.Maps.MapContextRequestedEventArgs"/> to <see cref="Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT.MapContextRequestedEventArgs"/>.
        /// </summary>
        /// <param name="args">The <see cref="Windows.UI.Xaml.Controls.Maps.MapContextRequestedEventArgs"/> instance containing the event data.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator MapContextRequestedEventArgs(
            Windows.UI.Xaml.Controls.Maps.MapContextRequestedEventArgs args)
        {
            return FromMapContextRequestedEventArgs(args);
        }

        /// <summary>
        /// Creates a <see cref="MapContextRequestedEventArgs"/> from <see cref="Windows.UI.Xaml.Controls.Maps.MapContextRequestedEventArgs"/>.
        /// </summary>
        /// <param name="args">The <see cref="Windows.UI.Xaml.Controls.Maps.MapContextRequestedEventArgs"/> instance containing the event data.</param>
        /// <returns><see cref="MapContextRequestedEventArgs"/></returns>
        public static MapContextRequestedEventArgs FromMapContextRequestedEventArgs(Windows.UI.Xaml.Controls.Maps.MapContextRequestedEventArgs args)
        {
            return new MapContextRequestedEventArgs(args);
        }
    }
}