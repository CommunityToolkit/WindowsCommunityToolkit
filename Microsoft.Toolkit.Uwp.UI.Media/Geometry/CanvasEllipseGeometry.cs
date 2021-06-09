// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Numerics;
using Microsoft.Graphics.Canvas.Geometry;
using Windows.UI.Xaml;

namespace Microsoft.Toolkit.Uwp.UI.Media
{
    /// <summary>
    /// Represents an Ellipse geometry object with the specified extents.
    /// </summary>
    public class CanvasEllipseGeometry : CanvasCoreGeometry
    {
        /// <summary>
        /// CenterX Dependency Property
        /// </summary>
        public static readonly DependencyProperty CenterXProperty = DependencyProperty.Register(
            "CenterX",
            typeof(double),
            typeof(CanvasEllipseGeometry),
            new PropertyMetadata(0d, OnPropertyChanged));

        /// <summary>
        /// Gets or sets the coordinate of the center of the <see cref="CanvasEllipseGeometry"/> on the x-axis.
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
            typeof(CanvasEllipseGeometry),
            new PropertyMetadata(0d, OnPropertyChanged));

        /// <summary>
        /// Gets or sets the coordinate of the center of the <see cref="CanvasEllipseGeometry"/> on the y-axis.
        /// </summary>
        public double CenterY
        {
            get => (double)GetValue(CenterYProperty);
            set => SetValue(CenterYProperty, value);
        }

        /// <summary>
        /// RadiusX Dependency Property
        /// </summary>
        public static readonly DependencyProperty RadiusXProperty = DependencyProperty.Register(
            "RadiusX",
            typeof(float),
            typeof(CanvasEllipseGeometry),
            new PropertyMetadata(0d, OnPropertyChanged));

        /// <summary>
        /// Gets or sets the x-radius value of the <see cref="CanvasEllipseGeometry"/>.
        /// </summary>
        public float RadiusX
        {
            get => (float)GetValue(RadiusXProperty);
            set => SetValue(RadiusXProperty, value);
        }

        /// <summary>
        /// RadiusY Dependency Property
        /// </summary>
        public static readonly DependencyProperty RadiusYProperty = DependencyProperty.Register(
            "RadiusY",
            typeof(float),
            typeof(CanvasEllipseGeometry),
            new PropertyMetadata(0d, OnPropertyChanged));

        /// <summary>
        /// Gets or sets the y-radius value of the <see cref="CanvasEllipseGeometry"/>.
        /// </summary>
        public float RadiusY
        {
            get => (float)GetValue(RadiusYProperty);
            set => SetValue(RadiusYProperty, value);
        }

        /// <summary>
        /// Method that is called whenever the dependency properties of the Brush changes
        /// </summary>
        /// <param name="d">The object whose property has changed</param>
        /// <param name="e">Event arguments</param>
        private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var geometry = (CanvasEllipseGeometry)d;

            // Recreate the geometry on any property change.
            geometry.OnUpdateGeometry();
        }

        /// <inheritdoc/>
        protected override void OnUpdateGeometry()
        {
            Geometry = CanvasGeometry.CreateEllipse(CompositionGenerator.Instance.Device, new Vector2((float)CenterX, (float)CenterY), RadiusX, RadiusY);

            RaiseUpdatedEvent();
        }
    }
}
