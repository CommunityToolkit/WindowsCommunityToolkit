using System;
using System.Security;
using Microsoft.Windows.Interop;

namespace Microsoft.Toolkit.Win32.UI.Controls.WPF
{
    /// <summary>
    /// Provides data for events. This class cannot be inherited.
    /// </summary>
    /// <remarks>Copy from <see cref="global::Windows.UI.Xaml.Controls.Maps.MapActualCameraChangingEventArgs"/> to avoid requirement to link Windows.winmd</remarks>
    /// <seealso cref="global::Windows.UI.Xaml.Controls.Maps.MapActualCameraChangingEventArgs"/>
    public sealed class MapActualCameraChangingEventArgs : EventArgs
    {
        [SecurityCritical]
        private readonly global::Windows.UI.Xaml.Controls.Maps.MapActualCameraChangingEventArgs _args;

        [SecurityCritical]
        internal MapActualCameraChangingEventArgs(global::Windows.UI.Xaml.Controls.Maps.MapActualCameraChangingEventArgs args)
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
        /// Performs an implicit conversion from <see cref="global::Windows.UI.Xaml.Controls.Maps.MapActualCameraChangingEventArgs"/> to <see cref="Microsoft.Toolkit.Win32.UI.Controls.WPF.MapActualCameraChangingEventArgs"/>.
        /// </summary>
        /// <param name="args">The <see cref="global::Windows.UI.Xaml.Controls.Maps.MapActualCameraChangingEventArgs"/> instance containing the event data.</param>
        /// <returns>The result of the conversion.</returns>
        [SecurityCritical]
        public static implicit operator MapActualCameraChangingEventArgs(
            global::Windows.UI.Xaml.Controls.Maps.MapActualCameraChangingEventArgs args)
        {
            return FromMapActualCameraChangingEventArgs(args);
        }

        /// <summary>
        /// Creates a <see cref="MapActualCameraChangingEventArgs"/> from <see cref="global::Windows.UI.Xaml.Controls.Maps.MapActualCameraChangingEventArgs"/>.
        /// </summary>
        /// <param name="args">The <see cref="global::Windows.UI.Xaml.Controls.Maps.MapActualCameraChangingEventArgs"/> instance containing the event data.</param>
        /// <returns><see cref="MapActualCameraChangingEventArgs"/></returns>
        public static MapActualCameraChangingEventArgs FromMapActualCameraChangingEventArgs(global::Windows.UI.Xaml.Controls.Maps.MapActualCameraChangingEventArgs args)
        {
            return new MapActualCameraChangingEventArgs(args);
        }
    }
}