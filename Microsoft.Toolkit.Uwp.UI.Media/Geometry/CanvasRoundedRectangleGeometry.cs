// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Graphics.Canvas.Geometry;
using Microsoft.Toolkit.Uwp.UI.Media.Surface;
using Windows.UI.Xaml;

namespace Microsoft.Toolkit.Uwp.UI.Media.Geometry
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
            typeof(float),
            typeof(CanvasRoundedRectangleGeometry),
            new PropertyMetadata(0f, OnRadiusXChanged));

        /// <summary>
        /// Gets or sets the radius of the corners in the x-axis.
        /// </summary>
        public float RadiusX
        {
            get => (float)GetValue(RadiusXProperty);
            set => SetValue(RadiusXProperty, value);
        }

        /// <summary>
        /// Handles changes to the RadiusX property.
        /// </summary>
        /// <param name="d">CanvasRoundedRectangleGeometry</param>
        /// <param name="e">DependencyProperty changed event arguments</param>
        private static void OnRadiusXChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var roundedRectangleGeometry = (CanvasRoundedRectangleGeometry)d;
            roundedRectangleGeometry.OnRadiusXChanged();
        }

        /// <summary>
        /// Instance handler for the changes to the RadiusX dependency property.
        /// </summary>
        private void OnRadiusXChanged()
        {
            UpdateGeometry();
        }

        /// <summary>
        /// RadiusY Dependency Property
        /// </summary>
        public static readonly DependencyProperty RadiusYProperty = DependencyProperty.Register(
            "RadiusY",
            typeof(float),
            typeof(CanvasRoundedRectangleGeometry),
            new PropertyMetadata(0f, OnRadiusYChanged));

        /// <summary>
        /// Gets or sets the radius of the corners in the x-axis.
        /// </summary>
        public float RadiusY
        {
            get => (float)GetValue(RadiusYProperty);
            set => SetValue(RadiusYProperty, value);
        }

        /// <summary>
        /// Handles changes to the RadiusY property.
        /// </summary>
        /// <param name="d">CanvasRoundedRectangleGeometry</param>
        /// <param name="e">DependencyProperty changed event arguments</param>
        private static void OnRadiusYChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var roundedRectangleGeometry = (CanvasRoundedRectangleGeometry)d;
            roundedRectangleGeometry.OnRadiusYChanged();
        }

        /// <summary>
        /// Instance handler for the changes to the RadiusY dependency property.
        /// </summary>
        private void OnRadiusYChanged()
        {
            UpdateGeometry();
        }

        /// <inheritdoc/>
        protected override void UpdateGeometry()
        {
            Geometry = CanvasGeometry.CreateRoundedRectangle(CompositionGenerator.Instance.Device, Rect.X, Rect.Y, Rect.Z, Rect.W, RadiusX, RadiusY);
        }
    }
}
