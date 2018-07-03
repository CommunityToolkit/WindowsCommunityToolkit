using System;
using System.Security;
using Microsoft.Windows.Interop;

namespace Microsoft.Toolkit.Win32.UI.Controls.WPF
{
    /// <summary>
    /// Provides data for events. This class cannot be inherited.
    /// </summary>
    /// <remarks>Copy from <see cref="global::Windows.UI.Xaml.Controls.Maps.MapContextRequestedEventArgs"/> to avoid requirement to link Windows.winmd</remarks>
    /// <seealso cref="global::Windows.UI.Xaml.Controls.Maps.MapContextRequestedEventArgs"/>
    public sealed class MapContextRequestedEventArgs : EventArgs
    {
        [SecurityCritical]
        private readonly global::Windows.UI.Xaml.Controls.Maps.MapContextRequestedEventArgs _args;

        [SecurityCritical]
        internal MapContextRequestedEventArgs(global::Windows.UI.Xaml.Controls.Maps.MapContextRequestedEventArgs args)
        {
            _args = args;
        }

        public global::Windows.Devices.Geolocation.Geopoint Location
        {
            [SecurityCritical]
            get => (global::Windows.Devices.Geolocation.Geopoint)_args.Location;
        }

        public System.Collections.Generic.IReadOnlyList<global::Windows.UI.Xaml.Controls.Maps.MapElement> MapElements
        {
            [SecurityCritical]
            get => (System.Collections.Generic.IReadOnlyList<global::Windows.UI.Xaml.Controls.Maps.MapElement>)_args.MapElements;
        }

        public global::Windows.Foundation.Point Position
        {
            [SecurityCritical]
            get => (global::Windows.Foundation.Point)_args.Position;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="global::Windows.UI.Xaml.Controls.Maps.MapContextRequestedEventArgs"/> to <see cref="Microsoft.Toolkit.Win32.UI.Controls.WPF.MapContextRequestedEventArgs"/>.
        /// </summary>
        /// <param name="args">The <see cref="global::Windows.UI.Xaml.Controls.Maps.MapContextRequestedEventArgs"/> instance containing the event data.</param>
        /// <returns>The result of the conversion.</returns>
        [SecurityCritical]
        public static implicit operator MapContextRequestedEventArgs(
            global::Windows.UI.Xaml.Controls.Maps.MapContextRequestedEventArgs args)
        {
            return FromMapContextRequestedEventArgs(args);
        }

        /// <summary>
        /// Creates a <see cref="MapContextRequestedEventArgs"/> from <see cref="global::Windows.UI.Xaml.Controls.Maps.MapContextRequestedEventArgs"/>.
        /// </summary>
        /// <param name="args">The <see cref="global::Windows.UI.Xaml.Controls.Maps.MapContextRequestedEventArgs"/> instance containing the event data.</param>
        /// <returns><see cref="MapContextRequestedEventArgs"/></returns>
        public static MapContextRequestedEventArgs FromMapContextRequestedEventArgs(global::Windows.UI.Xaml.Controls.Maps.MapContextRequestedEventArgs args)
        {
            return new MapContextRequestedEventArgs(args);
        }
    }
}