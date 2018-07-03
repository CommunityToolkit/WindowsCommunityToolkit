using System;
using System.Security;
using Microsoft.Windows.Interop;

namespace Microsoft.Toolkit.Win32.UI.Controls.WPF
{
    /// <summary>
    /// Provides data for events. This class cannot be inherited.
    /// </summary>
    /// <remarks>Copy from <see cref="global::Windows.UI.Xaml.Controls.Maps.MapElementPointerExitedEventArgs"/> to avoid requirement to link Windows.winmd</remarks>
    /// <seealso cref="global::Windows.UI.Xaml.Controls.Maps.MapElementPointerExitedEventArgs"/>
    public sealed class MapElementPointerExitedEventArgs : EventArgs
    {
        [SecurityCritical]
        private readonly global::Windows.UI.Xaml.Controls.Maps.MapElementPointerExitedEventArgs _args;

        [SecurityCritical]
        internal MapElementPointerExitedEventArgs(global::Windows.UI.Xaml.Controls.Maps.MapElementPointerExitedEventArgs args)
        {
            _args = args;
        }

        public global::Windows.Devices.Geolocation.Geopoint Location
        {
            [SecurityCritical]
            get => (global::Windows.Devices.Geolocation.Geopoint)_args.Location;
        }

        public global::Windows.UI.Xaml.Controls.Maps.MapElement MapElement
        {
            [SecurityCritical]
            get => (global::Windows.UI.Xaml.Controls.Maps.MapElement)_args.MapElement;
        }

        public global::Windows.Foundation.Point Position
        {
            [SecurityCritical]
            get => (global::Windows.Foundation.Point)_args.Position;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="global::Windows.UI.Xaml.Controls.Maps.MapElementPointerExitedEventArgs"/> to <see cref="Microsoft.Toolkit.Win32.UI.Controls.WPF.MapElementPointerExitedEventArgs"/>.
        /// </summary>
        /// <param name="args">The <see cref="global::Windows.UI.Xaml.Controls.Maps.MapElementPointerExitedEventArgs"/> instance containing the event data.</param>
        /// <returns>The result of the conversion.</returns>
        [SecurityCritical]
        public static implicit operator MapElementPointerExitedEventArgs(
            global::Windows.UI.Xaml.Controls.Maps.MapElementPointerExitedEventArgs args)
        {
            return FromMapElementPointerExitedEventArgs(args);
        }

        /// <summary>
        /// Creates a <see cref="MapElementPointerExitedEventArgs"/> from <see cref="global::Windows.UI.Xaml.Controls.Maps.MapElementPointerExitedEventArgs"/>.
        /// </summary>
        /// <param name="args">The <see cref="global::Windows.UI.Xaml.Controls.Maps.MapElementPointerExitedEventArgs"/> instance containing the event data.</param>
        /// <returns><see cref="MapElementPointerExitedEventArgs"/></returns>
        public static MapElementPointerExitedEventArgs FromMapElementPointerExitedEventArgs(global::Windows.UI.Xaml.Controls.Maps.MapElementPointerExitedEventArgs args)
        {
            return new MapElementPointerExitedEventArgs(args);
        }
    }
}