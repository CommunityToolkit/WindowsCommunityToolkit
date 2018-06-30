using System;
using System.Security;
using Microsoft.Windows.Interop;

namespace Microsoft.Toolkit.Win32.UI.Controls.WPF
{
    /// <summary>
    /// Provides data for events. This class cannot be inherited.
    /// </summary>
    /// <remarks>Copy from <see cref="global::Windows.UI.Xaml.Controls.Maps.MapActualCameraChangedEventArgs"/> to avoid requirement to link Windows.winmd</remarks>
    /// <seealso cref="global::Windows.UI.Xaml.Controls.Maps.MapActualCameraChangedEventArgs"/>
    public sealed class MapActualCameraChangedEventArgs : EventArgs
    {
        [SecurityCritical]
        private readonly global::Windows.UI.Xaml.Controls.Maps.MapActualCameraChangedEventArgs _args;

        [SecurityCritical]
        internal MapActualCameraChangedEventArgs(global::Windows.UI.Xaml.Controls.Maps.MapActualCameraChangedEventArgs args)
        {
            _args = args;
        }

        public global::Windows.UI.Xaml.Controls.Maps.MapCamera Camera
        {
            [SecurityCritical]
            get => (global::Windows.UI.Xaml.Controls.Maps.MapCamera)_args.Camera;
        }

        public global::Windows.UI.Xaml.Controls.Maps.MapCameraChangeReason ChangeReason
        {
            [SecurityCritical]
            get => (global::Windows.UI.Xaml.Controls.Maps.MapCameraChangeReason)_args.ChangeReason;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="global::Windows.UI.Xaml.Controls.Maps.MapActualCameraChangedEventArgs"/> to <see cref="Microsoft.Toolkit.Win32.UI.Controls.WPF.MapActualCameraChangedEventArgs"/>.
        /// </summary>
        /// <param name="args">The <see cref="global::Windows.UI.Xaml.Controls.Maps.MapActualCameraChangedEventArgs"/> instance containing the event data.</param>
        /// <returns>The result of the conversion.</returns>
        [SecurityCritical]
        public static implicit operator MapActualCameraChangedEventArgs(
            global::Windows.UI.Xaml.Controls.Maps.MapActualCameraChangedEventArgs args)
        {
            return FromMapActualCameraChangedEventArgs(args);
        }

        /// <summary>
        /// Creates a <see cref="MapActualCameraChangedEventArgs"/> from <see cref="global::Windows.UI.Xaml.Controls.Maps.MapActualCameraChangedEventArgs"/>.
        /// </summary>
        /// <param name="args">The <see cref="global::Windows.UI.Xaml.Controls.Maps.MapActualCameraChangedEventArgs"/> instance containing the event data.</param>
        /// <returns><see cref="MapActualCameraChangedEventArgs"/></returns>
        public static MapActualCameraChangedEventArgs FromMapActualCameraChangedEventArgs(global::Windows.UI.Xaml.Controls.Maps.MapActualCameraChangedEventArgs args)
        {
            return new MapActualCameraChangedEventArgs(args);
        }
    }
}