using System;
using System.Security;
using Microsoft.Windows.Interop;

namespace Microsoft.Toolkit.Win32.UI.Controls.WPF
{
    /// <summary>
    /// Provides data for events. This class cannot be inherited.
    /// </summary>
    /// <remarks>Copy from <see cref="global::Windows.UI.Xaml.Controls.Maps.MapElementClickEventArgs"/> to avoid requirement to link Windows.winmd</remarks>
    /// <seealso cref="global::Windows.UI.Xaml.Controls.Maps.MapElementClickEventArgs"/>
    public sealed class MapElementClickEventArgs : EventArgs
    {
        [SecurityCritical]
        private readonly global::Windows.UI.Xaml.Controls.Maps.MapElementClickEventArgs _args;

        [SecurityCritical]
        internal MapElementClickEventArgs(global::Windows.UI.Xaml.Controls.Maps.MapElementClickEventArgs args)
        {
            _args = args;
        }

        public global::Windows.Devices.Geolocation.Geopoint Location
        {
            [SecurityCritical]
            get => (global::Windows.Devices.Geolocation.Geopoint)_args.Location;
        }

        public System.Collections.Generic.IList<global::Windows.UI.Xaml.Controls.Maps.MapElement> MapElements
        {
            [SecurityCritical]
            get => (System.Collections.Generic.IList<global::Windows.UI.Xaml.Controls.Maps.MapElement>)_args.MapElements;
        }

        public global::Windows.Foundation.Point Position
        {
            [SecurityCritical]
            get => (global::Windows.Foundation.Point)_args.Position;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="global::Windows.UI.Xaml.Controls.Maps.MapElementClickEventArgs"/> to <see cref="Microsoft.Toolkit.Win32.UI.Controls.WPF.MapElementClickEventArgs"/>.
        /// </summary>
        /// <param name="args">The <see cref="global::Windows.UI.Xaml.Controls.Maps.MapElementClickEventArgs"/> instance containing the event data.</param>
        /// <returns>The result of the conversion.</returns>
        [SecurityCritical]
        public static implicit operator MapElementClickEventArgs(
            global::Windows.UI.Xaml.Controls.Maps.MapElementClickEventArgs args)
        {
            return FromMapElementClickEventArgs(args);
        }

        /// <summary>
        /// Creates a <see cref="MapElementClickEventArgs"/> from <see cref="global::Windows.UI.Xaml.Controls.Maps.MapElementClickEventArgs"/>.
        /// </summary>
        /// <param name="args">The <see cref="global::Windows.UI.Xaml.Controls.Maps.MapElementClickEventArgs"/> instance containing the event data.</param>
        /// <returns><see cref="MapElementClickEventArgs"/></returns>
        public static MapElementClickEventArgs FromMapElementClickEventArgs(global::Windows.UI.Xaml.Controls.Maps.MapElementClickEventArgs args)
        {
            return new MapElementClickEventArgs(args);
        }
    }
}