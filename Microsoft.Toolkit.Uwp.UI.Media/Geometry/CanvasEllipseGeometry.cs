// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.ComponentModel;
using System.Numerics;
using Microsoft.Graphics.Canvas.Geometry;
using Microsoft.Toolkit.Uwp.UI.Converters;
using Microsoft.Toolkit.Uwp.UI.Media.Surface;
using Windows.UI.Xaml;

namespace Microsoft.Toolkit.Uwp.UI.Media.Geometry
{
    /// <summary>
    /// Represents an Ellipse geometry object with the specified extents.
    /// </summary>
    public class CanvasEllipseGeometry : CanvasCoreGeometry
    {
        /// <summary>
        /// Center Dependency Property
        /// </summary>
        public static readonly DependencyProperty CenterProperty = DependencyProperty.Register(
            "Center",
            typeof(Vector2),
            typeof(CanvasEllipseGeometry),
            new PropertyMetadata(Vector2.Zero, OnCenterChanged));

        /// <summary>
        /// Gets or sets the <see cref="Vector2"/> structure that describes the position and size of the <see cref="CanvasEllipseGeometry"/>. The default is <see cref="Vector2.Zero"/>.
        /// </summary>
        [TypeConverter(typeof(Vector2Converter))]
        public Vector2 Center
        {
            get => (Vector2)GetValue(CenterProperty);
            set => SetValue(CenterProperty, value);
        }

        /// <summary>
        /// Handles changes to the Center property.
        /// </summary>
        /// <param name="d"><see cref="CanvasEllipseGeometry"/></param>
        /// <param name="e">DependencyProperty changed event arguments</param>
        private static void OnCenterChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ellipseGeometry = (CanvasEllipseGeometry)d;
            ellipseGeometry.OnCenterChanged();
        }

        /// <summary>
        /// Instance handler for the changes to the Center dependency property.
        /// </summary>
        private void OnCenterChanged()
        {
            UpdateGeometry();
        }

        /// <summary>
        /// RadiusX Dependency Property
        /// </summary>
        public static readonly DependencyProperty RadiusXProperty = DependencyProperty.Register(
            "RadiusX",
            typeof(float),
            typeof(CanvasEllipseGeometry),
            new PropertyMetadata(0f, OnRadiusXChanged));

        /// <summary>
        /// Gets or sets the x-radius value of the <see cref="CanvasEllipseGeometry"/>.
        /// </summary>
        public float RadiusX
        {
            get => (float)GetValue(RadiusXProperty);
            set => SetValue(RadiusXProperty, value);
        }

        /// <summary>
        /// Handles changes to the RadiusX property.
        /// </summary>
        /// <param name="d"><see cref="CanvasEllipseGeometry"/></param>
        /// <param name="e">DependencyProperty changed event arguments</param>
        private static void OnRadiusXChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var roundedRectangleGeometry = (CanvasEllipseGeometry)d;
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
            typeof(CanvasEllipseGeometry),
            new PropertyMetadata(0f, OnRadiusYChanged));

        /// <summary>
        /// Gets or sets the y-radius value of the <see cref="CanvasEllipseGeometry"/>.
        /// </summary>
        public float RadiusY
        {
            get => (float)GetValue(RadiusYProperty);
            set => SetValue(RadiusYProperty, value);
        }

        /// <summary>
        /// Handles changes to the RadiusY property.
        /// </summary>
        /// <param name="d"><see cref="CanvasEllipseGeometry"/></param>
        /// <param name="e">DependencyProperty changed event arguments</param>
        private static void OnRadiusYChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var roundedRectangleGeometry = (CanvasEllipseGeometry)d;
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
            Geometry = CanvasGeometry.CreateEllipse(CompositionGenerator.Instance.Device, Center, RadiusX, RadiusY);
        }
    }
}
