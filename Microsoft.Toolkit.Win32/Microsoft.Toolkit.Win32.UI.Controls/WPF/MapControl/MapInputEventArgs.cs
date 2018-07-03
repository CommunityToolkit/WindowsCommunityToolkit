using System;
using System.Security;
using Microsoft.Windows.Interop;

namespace Microsoft.Toolkit.Win32.UI.Controls.WPF
{
    /// <summary>
    /// Provides data for events. This class cannot be inherited.
    /// </summary>
    /// <remarks>Copy from <see cref="global::Windows.UI.Xaml.Controls.Maps.MapInputEventArgs"/> to avoid requirement to link Windows.winmd</remarks>
    /// <seealso cref="global::Windows.UI.Xaml.Controls.Maps.MapInputEventArgs"/>
    public sealed class MapInputEventArgs : EventArgs
    {
        [SecurityCritical]
        private readonly global::Windows.UI.Xaml.Controls.Maps.MapInputEventArgs _args;

        [SecurityCritical]
        internal MapInputEventArgs(global::Windows.UI.Xaml.Controls.Maps.MapInputEventArgs args)
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

        public global::Windows.UI.Core.CoreDispatcher Dispatcher
        {
            [SecurityCritical]
            get => (global::Windows.UI.Core.CoreDispatcher)_args.Dispatcher;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="global::Windows.UI.Xaml.Controls.Maps.MapInputEventArgs"/> to <see cref="Microsoft.Toolkit.Win32.UI.Controls.WPF.MapInputEventArgs"/>.
        /// </summary>
        /// <param name="args">The <see cref="global::Windows.UI.Xaml.Controls.Maps.MapInputEventArgs"/> instance containing the event data.</param>
        /// <returns>The result of the conversion.</returns>
        [SecurityCritical]
        public static implicit operator MapInputEventArgs(
            global::Windows.UI.Xaml.Controls.Maps.MapInputEventArgs args)
        {
            return FromMapInputEventArgs(args);
        }

        /// <summary>
        /// Creates a <see cref="MapInputEventArgs"/> from <see cref="global::Windows.UI.Xaml.Controls.Maps.MapInputEventArgs"/>.
        /// </summary>
        /// <param name="args">The <see cref="global::Windows.UI.Xaml.Controls.Maps.MapInputEventArgs"/> instance containing the event data.</param>
        /// <returns><see cref="MapInputEventArgs"/></returns>
        public static MapInputEventArgs FromMapInputEventArgs(global::Windows.UI.Xaml.Controls.Maps.MapInputEventArgs args)
        {
            return new MapInputEventArgs(args);
        }
    }
}