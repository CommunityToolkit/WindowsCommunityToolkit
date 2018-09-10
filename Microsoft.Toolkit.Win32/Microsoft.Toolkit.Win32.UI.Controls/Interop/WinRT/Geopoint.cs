// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Windows.Devices.Geolocation;

namespace Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT
{
    /// <summary>
    /// <see cref="Windows.Devices.Geolocation.Geopoint"/>
    /// </summary>
    public class Geopoint
    {
        private BasicGeoposition basicGeoposition;

        private Windows.Devices.Geolocation.Geopoint UwpInstance { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Geopoint"/> class, a
        /// Wpf-enabled wrapper for <see cref="Windows.Devices.Geolocation.Geopoint"/>
        /// </summary>
        public Geopoint(Windows.Devices.Geolocation.Geopoint instance)
        {
            this.UwpInstance = instance;
        }

        public Geopoint(BasicGeoposition basicGeoposition)
        {
            this.basicGeoposition = basicGeoposition;
        }

        /// <summary>
        /// Gets <see cref="Windows.Devices.Geolocation.Geopoint.Position"/>
        /// </summary>
        public Windows.Devices.Geolocation.BasicGeoposition Position
        {
            get => UwpInstance.Position;
        }

        /// <summary>
        /// Gets <see cref="Windows.Devices.Geolocation.Geopoint.AltitudeReferenceSystem"/>
        /// </summary>
        public Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT.AltitudeReferenceSystem AltitudeReferenceSystem
        {
            get => (Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT.AltitudeReferenceSystem)(int)UwpInstance.AltitudeReferenceSystem;
        }

        /// <summary>
        /// Gets <see cref="Windows.Devices.Geolocation.Geopoint.GeoshapeType"/>
        /// </summary>
        public Windows.Devices.Geolocation.GeoshapeType GeoshapeType
        {
            get => UwpInstance.GeoshapeType;
        }

        /// <summary>
        /// Gets <see cref="Windows.Devices.Geolocation.Geopoint.SpatialReferenceId"/>
        /// </summary>
        public uint SpatialReferenceId
        {
            get => UwpInstance.SpatialReferenceId;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="Windows.Devices.Geolocation.Geopoint"/> to <see cref="Geopoint"/>.
        /// </summary>
        /// <param name="args">The <see cref="Windows.Devices.Geolocation.Geopoint"/> instance containing the event data.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator Geopoint(
            Windows.Devices.Geolocation.Geopoint args)
        {
            return FromGeopoint(args);
        }

        /// <summary>
        /// Creates a <see cref="Geopoint"/> from <see cref="Windows.Devices.Geolocation.Geopoint"/>.
        /// </summary>
        /// <param name="args">The <see cref="Windows.Devices.Geolocation.Geopoint"/> instance containing the event data.</param>
        /// <returns><see cref="Geopoint"/></returns>
        public static Geopoint FromGeopoint(Windows.Devices.Geolocation.Geopoint args)
        {
            return new Geopoint(args);
        }
    }
}