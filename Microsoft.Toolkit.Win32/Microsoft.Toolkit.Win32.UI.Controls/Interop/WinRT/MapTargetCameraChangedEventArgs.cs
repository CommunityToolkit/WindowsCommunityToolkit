// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT
{
    /// <summary>
    /// Provides data for events. This class cannot be inherited.
    /// </summary>
    /// <remarks>Copy from <see cref="Windows.UI.Xaml.Controls.Maps.MapTargetCameraChangedEventArgs"/> to avoid requirement to link Windows.winmd</remarks>
    /// <seealso cref="Windows.UI.Xaml.Controls.Maps.MapTargetCameraChangedEventArgs"/>
    public sealed class MapTargetCameraChangedEventArgs : EventArgs
    {
        private readonly Windows.UI.Xaml.Controls.Maps.MapTargetCameraChangedEventArgs _args;

        internal MapTargetCameraChangedEventArgs(Windows.UI.Xaml.Controls.Maps.MapTargetCameraChangedEventArgs args)
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
        /// Performs an implicit conversion from <see cref="Windows.UI.Xaml.Controls.Maps.MapTargetCameraChangedEventArgs"/> to <see cref="Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT.MapTargetCameraChangedEventArgs"/>.
        /// </summary>
        /// <param name="args">The <see cref="Windows.UI.Xaml.Controls.Maps.MapTargetCameraChangedEventArgs"/> instance containing the event data.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator MapTargetCameraChangedEventArgs(
            Windows.UI.Xaml.Controls.Maps.MapTargetCameraChangedEventArgs args)
        {
            return FromMapTargetCameraChangedEventArgs(args);
        }

        /// <summary>
        /// Creates a <see cref="MapTargetCameraChangedEventArgs"/> from <see cref="Windows.UI.Xaml.Controls.Maps.MapTargetCameraChangedEventArgs"/>.
        /// </summary>
        /// <param name="args">The <see cref="Windows.UI.Xaml.Controls.Maps.MapTargetCameraChangedEventArgs"/> instance containing the event data.</param>
        /// <returns><see cref="MapTargetCameraChangedEventArgs"/></returns>
        public static MapTargetCameraChangedEventArgs FromMapTargetCameraChangedEventArgs(Windows.UI.Xaml.Controls.Maps.MapTargetCameraChangedEventArgs args)
        {
            return new MapTargetCameraChangedEventArgs(args);
        }
    }
}