using System;

namespace Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT
{
    /// <summary>
    /// Provides data for events. This class cannot be inherited.
    /// </summary>
    /// <remarks>Copy from <see cref="Windows.UI.Xaml.Controls.Maps.MapInputEventArgs"/> to avoid requirement to link Windows.winmd</remarks>
    /// <seealso cref="Windows.UI.Xaml.Controls.Maps.MapInputEventArgs"/>
    public sealed class MapInputEventArgs : EventArgs
    {
        private readonly Windows.UI.Xaml.Controls.Maps.MapInputEventArgs _args;

        internal MapInputEventArgs(Windows.UI.Xaml.Controls.Maps.MapInputEventArgs args)
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

        public Windows.UI.Core.CoreDispatcher Dispatcher
        {
            get => (Windows.UI.Core.CoreDispatcher)_args.Dispatcher;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="Windows.UI.Xaml.Controls.Maps.MapInputEventArgs"/> to <see cref="Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT.MapInputEventArgs"/>.
        /// </summary>
        /// <param name="args">The <see cref="Windows.UI.Xaml.Controls.Maps.MapInputEventArgs"/> instance containing the event data.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator MapInputEventArgs(
            Windows.UI.Xaml.Controls.Maps.MapInputEventArgs args)
        {
            return FromMapInputEventArgs(args);
        }

        /// <summary>
        /// Creates a <see cref="MapInputEventArgs"/> from <see cref="Windows.UI.Xaml.Controls.Maps.MapInputEventArgs"/>.
        /// </summary>
        /// <param name="args">The <see cref="Windows.UI.Xaml.Controls.Maps.MapInputEventArgs"/> instance containing the event data.</param>
        /// <returns><see cref="MapInputEventArgs"/></returns>
        public static MapInputEventArgs FromMapInputEventArgs(Windows.UI.Xaml.Controls.Maps.MapInputEventArgs args)
        {
            return new MapInputEventArgs(args);
        }
    }
}