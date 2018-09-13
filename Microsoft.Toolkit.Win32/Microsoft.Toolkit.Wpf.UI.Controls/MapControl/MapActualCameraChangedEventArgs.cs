using System;

namespace Microsoft.Toolkit.Wpf.UI.Controls
{
    /// <summary>
    /// Provides data for events. This class cannot be inherited.
    /// </summary>
    /// <remarks>Copy from <see cref="Windows.UI.Xaml.Controls.Maps.MapActualCameraChangedEventArgs"/> to avoid requirement to link Windows.winmd</remarks>
    /// <seealso cref="Windows.UI.Xaml.Controls.Maps.MapActualCameraChangedEventArgs"/>
    public sealed class MapActualCameraChangedEventArgs : EventArgs
    {
        private readonly Windows.UI.Xaml.Controls.Maps.MapActualCameraChangedEventArgs _args;

        internal MapActualCameraChangedEventArgs(Windows.UI.Xaml.Controls.Maps.MapActualCameraChangedEventArgs args)
        {
            _args = args;
        }

        public Windows.UI.Xaml.Controls.Maps.MapCamera Camera
        {
            get => (Windows.UI.Xaml.Controls.Maps.MapCamera)_args.Camera;
        }

        public Windows.UI.Xaml.Controls.Maps.MapCameraChangeReason ChangeReason
        {
            get => (Windows.UI.Xaml.Controls.Maps.MapCameraChangeReason)_args.ChangeReason;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="Windows.UI.Xaml.Controls.Maps.MapActualCameraChangedEventArgs"/> to <see cref="Microsoft.Toolkit.Wpf.UI.Controls.MapActualCameraChangedEventArgs"/>.
        /// </summary>
        /// <param name="args">The <see cref="Windows.UI.Xaml.Controls.Maps.MapActualCameraChangedEventArgs"/> instance containing the event data.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator MapActualCameraChangedEventArgs(
            Windows.UI.Xaml.Controls.Maps.MapActualCameraChangedEventArgs args)
        {
            return FromMapActualCameraChangedEventArgs(args);
        }

        /// <summary>
        /// Creates a <see cref="MapActualCameraChangedEventArgs"/> from <see cref="Windows.UI.Xaml.Controls.Maps.MapActualCameraChangedEventArgs"/>.
        /// </summary>
        /// <param name="args">The <see cref="Windows.UI.Xaml.Controls.Maps.MapActualCameraChangedEventArgs"/> instance containing the event data.</param>
        /// <returns><see cref="MapActualCameraChangedEventArgs"/></returns>
        public static MapActualCameraChangedEventArgs FromMapActualCameraChangedEventArgs(Windows.UI.Xaml.Controls.Maps.MapActualCameraChangedEventArgs args)
        {
            return new MapActualCameraChangedEventArgs(args);
        }
    }
}