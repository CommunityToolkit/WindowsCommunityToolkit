using System;
using System.Security;
using Microsoft.Windows.Interop;

namespace Microsoft.Toolkit.Win32.UI.Controls.WPF
{
    /// <summary>
    /// Provides data for events. This class cannot be inherited.
    /// </summary>
    /// <remarks>Copy from <see cref="global::Windows.UI.Xaml.Controls.Maps.MapElementPointerEnteredEventArgs"/> to avoid requirement to link Windows.winmd</remarks>
    /// <seealso cref="global::Windows.UI.Xaml.Controls.Maps.MapElementPointerEnteredEventArgs"/>
    public sealed class MapElementPointerEnteredEventArgs : EventArgs
    {
        [SecurityCritical]
        private readonly global::Windows.UI.Xaml.Controls.Maps.MapElementPointerEnteredEventArgs _args;

        [SecurityCritical]
        internal MapElementPointerEnteredEventArgs(global::Windows.UI.Xaml.Controls.Maps.MapElementPointerEnteredEventArgs args)
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
        /// Performs an implicit conversion from <see cref="global::Windows.UI.Xaml.Controls.Maps.MapElementPointerEnteredEventArgs"/> to <see cref="Microsoft.Toolkit.Win32.UI.Controls.WPF.MapElementPointerEnteredEventArgs"/>.
        /// </summary>
        /// <param name="args">The <see cref="global::Windows.UI.Xaml.Controls.Maps.MapElementPointerEnteredEventArgs"/> instance containing the event data.</param>
        /// <returns>The result of the conversion.</returns>
        [SecurityCritical]
        public static implicit operator MapElementPointerEnteredEventArgs(
            global::Windows.UI.Xaml.Controls.Maps.MapElementPointerEnteredEventArgs args)
        {
            return FromMapElementPointerEnteredEventArgs(args);
        }

        /// <summary>
        /// Creates a <see cref="MapElementPointerEnteredEventArgs"/> from <see cref="global::Windows.UI.Xaml.Controls.Maps.MapElementPointerEnteredEventArgs"/>.
        /// </summary>
        /// <param name="args">The <see cref="global::Windows.UI.Xaml.Controls.Maps.MapElementPointerEnteredEventArgs"/> instance containing the event data.</param>
        /// <returns><see cref="MapElementPointerEnteredEventArgs"/></returns>
        public static MapElementPointerEnteredEventArgs FromMapElementPointerEnteredEventArgs(global::Windows.UI.Xaml.Controls.Maps.MapElementPointerEnteredEventArgs args)
        {
            return new MapElementPointerEnteredEventArgs(args);
        }
    }
}