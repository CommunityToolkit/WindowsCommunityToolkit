// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Graphics.Canvas.Geometry;
using Windows.UI.Xaml;

namespace Microsoft.Toolkit.Uwp.UI.Media
{
    /// <summary>
    /// Represents a rounded rectangle geometry object with the specified extents.
    /// </summary>
    public class CanvasRoundedRectangleGeometry : CanvasRectangleGeometry
    {
        /// <summary>
        /// RadiusX Dependency Property
        /// </summary>
        public static readonly DependencyProperty RadiusXProperty = DependencyProperty.Register(
            "RadiusX",
            typeof(double),
            typeof(CanvasRoundedRectangleGeometry),
            new PropertyMetadata(0d, OnPropertyChanged));

        /// <summary>
        /// Gets or sets the radius of the corners in the x-axis.
        /// </summary>
        public double RadiusX
        {
            get => (double)GetValue(RadiusXProperty);
            set => SetValue(RadiusXProperty, value);
        }

        /// <summary>
        /// RadiusY Dependency Property
        /// </summary>
        public static readonly DependencyProperty RadiusYProperty = DependencyProperty.Register(
            "RadiusY",
            typeof(double),
            typeof(CanvasRoundedRectangleGeometry),
            new PropertyMetadata(0d, OnPropertyChanged));

        /// <summary>
        /// Gets or sets the radius of the corners in the x-axis.
        /// </summary>
        public double RadiusY
        {
            get => (double)GetValue(RadiusYProperty);
            set => SetValue(RadiusYProperty, value);
        }

        /// <summary>
        /// Method that is called whenever the dependency properties of the Brush changes
        /// </summary>
        /// <param name="d">The object whose property has changed</param>
        /// <param name="e">Event arguments</param>
        private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var geometry = (CanvasRoundedRectangleGeometry)d;

            // Recreate the geometry on any property change.
            geometry.OnUpdateGeometry();
        }

        /// <inheritdoc/>
        protected override void OnUpdateGeometry()
        {
            Geometry = CanvasGeometry.CreateRoundedRectangle(CompositionGenerator.Instance.Device, (float)X, (float)Y, (float)Width, (float)Height, (float)RadiusX, (float)RadiusY);

            RaiseUpdatedEvent();
        }
    }
}
