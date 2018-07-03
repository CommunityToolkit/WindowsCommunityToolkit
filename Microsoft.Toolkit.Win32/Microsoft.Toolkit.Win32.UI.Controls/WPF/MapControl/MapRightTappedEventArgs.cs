using System;
using System.Security;
using Microsoft.Windows.Interop;

namespace Microsoft.Toolkit.Win32.UI.Controls.WPF
{
    /// <summary>
    /// Provides data for events. This class cannot be inherited.
    /// </summary>
    /// <remarks>Copy from <see cref="global::Windows.UI.Xaml.Controls.Maps.MapRightTappedEventArgs"/> to avoid requirement to link Windows.winmd</remarks>
    /// <seealso cref="global::Windows.UI.Xaml.Controls.Maps.MapRightTappedEventArgs"/>
    public sealed class MapRightTappedEventArgs : EventArgs
    {
        [SecurityCritical]
        private readonly global::Windows.UI.Xaml.Controls.Maps.MapRightTappedEventArgs _args;

        [SecurityCritical]
        internal MapRightTappedEventArgs(global::Windows.UI.Xaml.Controls.Maps.MapRightTappedEventArgs args)
        {
            _args = args;
        }

        public global::Windows.Devices.Geolocation.Geopoint Location
        {
            [SecurityCritical]
            get => (global::Windows.Devices.Geolocation.Geopoint)_args.Location;
        }

        public global::Windows.Foundation.Point Position
        {
            [SecurityCritical]
            get => (global::Windows.Foundation.Point)_args.Position;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="global::Windows.UI.Xaml.Controls.Maps.MapRightTappedEventArgs"/> to <see cref="Microsoft.Toolkit.Win32.UI.Controls.WPF.MapRightTappedEventArgs"/>.
        /// </summary>
        /// <param name="args">The <see cref="global::Windows.UI.Xaml.Controls.Maps.MapRightTappedEventArgs"/> instance containing the event data.</param>
        /// <returns>The result of the conversion.</returns>
        [SecurityCritical]
        public static implicit operator MapRightTappedEventArgs(
            global::Windows.UI.Xaml.Controls.Maps.MapRightTappedEventArgs args)
        {
            return FromMapRightTappedEventArgs(args);
        }

        /// <summary>
        /// Creates a <see cref="MapRightTappedEventArgs"/> from <see cref="global::Windows.UI.Xaml.Controls.Maps.MapRightTappedEventArgs"/>.
        /// </summary>
        /// <param name="args">The <see cref="global::Windows.UI.Xaml.Controls.Maps.MapRightTappedEventArgs"/> instance containing the event data.</param>
        /// <returns><see cref="MapRightTappedEventArgs"/></returns>
        public static MapRightTappedEventArgs FromMapRightTappedEventArgs(global::Windows.UI.Xaml.Controls.Maps.MapRightTappedEventArgs args)
        {
            return new MapRightTappedEventArgs(args);
        }
    }
}