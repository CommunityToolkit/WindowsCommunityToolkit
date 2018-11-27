// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT
{
    /// <summary>
    /// <see cref="Point"/>
    /// </summary>
    public struct Point : IFormattable
    {
        internal Windows.Foundation.Point UwpInstance { get; set; }

        public Point(double x, double y)
        {
            UwpInstance = new Windows.Foundation.Point(x, y);
        }

        internal Point(Windows.Foundation.Point instance)
        {
            UwpInstance = instance;
        }

        /// <summary>
        /// Gets or sets X
        /// </summary>
        public double X
        {
            get => UwpInstance.X;
            set => UwpInstance = new Windows.Foundation.Point(value, Y);
        }

        /// <summary>
        /// Gets or sets Y
        /// </summary>
        public double Y
        {
            get => UwpInstance.Y;
            set => UwpInstance = new Windows.Foundation.Point(X, value);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="Windows.Devices.Geolocation.BasicGeoposition"/> to <see cref="BasicGeoposition"/>.
        /// </summary>
        /// <param name="args">The <see cref="Windows.Devices.Geolocation.BasicGeoposition"/> instance containing the event data.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator Point(Windows.Foundation.Point args)
        {
            return FromPoint(args);
        }

        /// <summary>
        /// Creates a <see cref="BasicGeoposition"/> from <see cref="Windows.Devices.Geolocation.BasicGeoposition"/>.
        /// </summary>
        /// <param name="args">The <see cref="Windows.Devices.Geolocation.BasicGeoposition"/> instance containing the event data.</param>
        /// <returns><see cref="BasicGeoposition"/></returns>
        public static Point FromPoint(Windows.Foundation.Point args)
        {
            return new Point(args);
        }

        public bool Equals(Point point) => UwpInstance.Equals(point.UwpInstance);

        public override int GetHashCode()
        {
            return UwpInstance.GetHashCode();
        }

        public override string ToString()
        {
            return UwpInstance.ToString();
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            return UwpInstance.ToString(formatProvider);
        }

        public override bool Equals(object obj)
        {
            return UwpInstance.Equals(obj);
        }

        public static bool operator ==(Point point1, Point point2)
        {
            return point1.UwpInstance == point2.UwpInstance;
        }

        public static bool operator !=(Point point1, Point point2)
        {
            return point1.UwpInstance != point2.UwpInstance;
        }
    }
}