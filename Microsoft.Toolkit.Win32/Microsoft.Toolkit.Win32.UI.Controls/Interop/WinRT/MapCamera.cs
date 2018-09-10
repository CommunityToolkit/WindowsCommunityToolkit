// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT
{
    /// <summary>
    /// <see cref="Windows.UI.Xaml.Controls.Maps.MapCamera"/>
    /// </summary>
    public class MapCamera
    {
        private Windows.UI.Xaml.Controls.Maps.MapCamera UwpInstance { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MapCamera"/> class, a
        /// Wpf-enabled wrapper for <see cref="Windows.UI.Xaml.Controls.Maps.MapCamera"/>
        /// </summary>
        public MapCamera(Windows.UI.Xaml.Controls.Maps.MapCamera instance)
        {
            this.UwpInstance = instance;
        }

        /// <summary>
        /// Gets or sets <see cref="Windows.UI.Xaml.Controls.Maps.MapCamera.Roll"/>
        /// </summary>
        public double Roll
        {
            get => UwpInstance.Roll;
            set => UwpInstance.Roll = value;
        }

        /// <summary>
        /// Gets or sets <see cref="Windows.UI.Xaml.Controls.Maps.MapCamera.Pitch"/>
        /// </summary>
        public double Pitch
        {
            get => UwpInstance.Pitch;
            set => UwpInstance.Pitch = value;
        }

        /// <summary>
        /// Gets or sets <see cref="Windows.UI.Xaml.Controls.Maps.MapCamera.Location"/>
        /// </summary>
        public Geopoint Location
        {
            get => UwpInstance.Location;
            set => UwpInstance.Location = value.UwpInstance;
        }

        /// <summary>
        /// Gets or sets <see cref="Windows.UI.Xaml.Controls.Maps.MapCamera.Heading"/>
        /// </summary>
        public double Heading
        {
            get => UwpInstance.Heading;
            set => UwpInstance.Heading = value;
        }

        /// <summary>
        /// Gets or sets <see cref="Windows.UI.Xaml.Controls.Maps.MapCamera.FieldOfView"/>
        /// </summary>
        public double FieldOfView
        {
            get => UwpInstance.FieldOfView;
            set => UwpInstance.FieldOfView = value;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="Windows.UI.Xaml.Controls.Maps.MapCamera"/> to <see cref="MapCamera"/>.
        /// </summary>
        /// <param name="args">The <see cref="Windows.UI.Xaml.Controls.Maps.MapCamera"/> instance containing the event data.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator MapCamera(
            Windows.UI.Xaml.Controls.Maps.MapCamera args)
        {
            return FromMapCamera(args);
        }

        /// <summary>
        /// Creates a <see cref="MapCamera"/> from <see cref="Windows.UI.Xaml.Controls.Maps.MapCamera"/>.
        /// </summary>
        /// <param name="args">The <see cref="Windows.UI.Xaml.Controls.Maps.MapCamera"/> instance containing the event data.</param>
        /// <returns><see cref="MapCamera"/></returns>
        public static MapCamera FromMapCamera(Windows.UI.Xaml.Controls.Maps.MapCamera args)
        {
            return new MapCamera(args);
        }
    }
}