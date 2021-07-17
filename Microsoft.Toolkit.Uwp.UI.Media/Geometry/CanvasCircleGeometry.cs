// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Numerics;
using Microsoft.Graphics.Canvas.Geometry;
using Windows.UI.Xaml;

namespace Microsoft.Toolkit.Uwp.UI.Media
{
    /// <summary>
    /// Represents a circle geometry object with the specified extents.
    /// </summary>
    public class CanvasCircleGeometry : CanvasCoreGeometry
    {
        /// <summary>
        /// CenterX Dependency Property
        /// </summary>
        public static readonly DependencyProperty CenterXProperty = DependencyProperty.Register(
            "CenterX",
            typeof(double),
            typeof(CanvasCircleGeometry),
            new PropertyMetadata(0d, OnPropertyChanged));

        /// <summary>
        /// Gets or sets the the x coordinate of the center.
        /// </summary>
        public double CenterX
        {
            get => (double)GetValue(CenterXProperty);
            set => SetValue(CenterXProperty, value);
        }

        /// <summary>
        /// CenterY Dependency Property
        /// </summary>
        public static readonly DependencyProperty CenterYProperty = DependencyProperty.Register(
            "CenterY",
            typeof(double),
            typeof(CanvasCircleGeometry),
            new PropertyMetadata(0d, OnPropertyChanged));

        /// <summary>
        /// Gets or sets the y coordinate of the Center.
        /// </summary>
        public double CenterY
        {
            get => (double)GetValue(CenterYProperty);
            set => SetValue(CenterYProperty, value);
        }

        /// <summary>
        /// Radius Dependency Property
        /// </summary>
        public static readonly DependencyProperty RadiusProperty = DependencyProperty.Register(
            "Radius",
            typeof(double),
            typeof(CanvasCircleGeometry),
            new PropertyMetadata(0d, OnPropertyChanged));

        /// <summary>
        /// Gets or sets the radius value of the <see cref="CanvasCircleGeometry"/>.
        /// </summary>
        public double Radius
        {
            get => (double)GetValue(RadiusProperty);
            set => SetValue(RadiusProperty, value);
        }

        /// <summary>
        /// Method that is called whenever the dependency properties of the Brush .
        /// </summary>
        /// <param name="d">The object whose property has changed</param>
        /// <param name="e">Event arguments</param>
        private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var geometry = (CanvasCircleGeometry)d;

            // Recreate the geometry on any property change.
            geometry.OnUpdateGeometry();
        }

        /// <inheritdoc/>
        protected override void OnUpdateGeometry()
        {
            Geometry?.Dispose();

            var center = new Vector2((float)CenterX, (float)CenterY);
            Geometry = CanvasGeometry.CreateCircle(CompositionGenerator.Instance.Device, center, (float)Radius);

            base.OnUpdateGeometry();
        }
    }
}
