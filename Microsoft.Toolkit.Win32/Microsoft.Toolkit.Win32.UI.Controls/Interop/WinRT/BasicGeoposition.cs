// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT
{
    /// <summary>
    /// <see cref="Windows.Devices.Geolocation.BasicGeoposition"/>
    /// </summary>
    public struct BasicGeoposition
    {
        /// <summary>
        /// Latitude
        /// </summary>
        public double Latitude;

        /// <summary>
        /// Longitude
        /// </summary>
        public double Longitude;

        /// <summary>
        /// Altitude
        /// </summary>
        public double Altitude;

        /// <summary>
        /// Performs an implicit conversion from <see cref="Windows.Devices.Geolocation.BasicGeoposition"/> to <see cref="BasicGeoposition"/>.
        /// </summary>
        /// <param name="args">The <see cref="Windows.Devices.Geolocation.BasicGeoposition"/> instance containing the event data.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator BasicGeoposition(
            Windows.Devices.Geolocation.BasicGeoposition args)
        {
            return FromBasicGeoposition(args);
        }

        /// <summary>
        /// Creates a <see cref="BasicGeoposition"/> from <see cref="Windows.Devices.Geolocation.BasicGeoposition"/>.
        /// </summary>
        /// <param name="args">The <see cref="Windows.Devices.Geolocation.BasicGeoposition"/> instance containing the event data.</param>
        /// <returns><see cref="BasicGeoposition"/></returns>
        public static BasicGeoposition FromBasicGeoposition(Windows.Devices.Geolocation.BasicGeoposition args)
        {
            return new BasicGeoposition() { Latitude = args.Latitude, Longitude = args.Longitude, Altitude = args.Altitude };
        }
    }
}