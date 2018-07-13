using System;
using System.Security;
using Microsoft.Windows.Interop;

namespace Microsoft.Toolkit.Win32.UI.Controls.WPF
{
    /// <summary>
    /// Provides data for events. This class cannot be inherited.
    /// </summary>
    /// <remarks>Copy from <see cref="global::Windows.UI.Xaml.Controls.Maps.MapTargetCameraChangedEventArgs"/> to avoid requirement to link Windows.winmd</remarks>
    /// <seealso cref="global::Windows.UI.Xaml.Controls.Maps.MapTargetCameraChangedEventArgs"/>
    public sealed class MapTargetCameraChangedEventArgs : EventArgs
    {
        [SecurityCritical]
        private readonly global::Windows.UI.Xaml.Controls.Maps.MapTargetCameraChangedEventArgs _args;

        [SecurityCritical]
        internal MapTargetCameraChangedEventArgs(global::Windows.UI.Xaml.Controls.Maps.MapTargetCameraChangedEventArgs args)
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
        /// Performs an implicit conversion from <see cref="global::Windows.UI.Xaml.Controls.Maps.MapTargetCameraChangedEventArgs"/> to <see cref="Microsoft.Toolkit.Win32.UI.Controls.WPF.MapTargetCameraChangedEventArgs"/>.
        /// </summary>
        /// <param name="args">The <see cref="global::Windows.UI.Xaml.Controls.Maps.MapTargetCameraChangedEventArgs"/> instance containing the event data.</param>
        /// <returns>The result of the conversion.</returns>
        [SecurityCritical]
        public static implicit operator MapTargetCameraChangedEventArgs(
            global::Windows.UI.Xaml.Controls.Maps.MapTargetCameraChangedEventArgs args)
        {
            return FromMapTargetCameraChangedEventArgs(args);
        }

        /// <summary>
        /// Creates a <see cref="MapTargetCameraChangedEventArgs"/> from <see cref="global::Windows.UI.Xaml.Controls.Maps.MapTargetCameraChangedEventArgs"/>.
        /// </summary>
        /// <param name="args">The <see cref="global::Windows.UI.Xaml.Controls.Maps.MapTargetCameraChangedEventArgs"/> instance containing the event data.</param>
        /// <returns><see cref="MapTargetCameraChangedEventArgs"/></returns>
        public static MapTargetCameraChangedEventArgs FromMapTargetCameraChangedEventArgs(global::Windows.UI.Xaml.Controls.Maps.MapTargetCameraChangedEventArgs args)
        {
            return new MapTargetCameraChangedEventArgs(args);
        }
    }
}