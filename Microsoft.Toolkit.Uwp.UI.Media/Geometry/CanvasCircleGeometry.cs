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
    /// Represents a circle geometry object with the specified extents.
    /// </summary>
    public class CanvasCircleGeometry : CanvasCoreGeometry
    {
        /// <summary>
        /// Center Dependency Property
        /// </summary>
        public static readonly DependencyProperty CenterProperty = DependencyProperty.Register(
            "Center",
            typeof(Vector2),
            typeof(CanvasCircleGeometry),
            new PropertyMetadata(Vector2.Zero, OnCenterChanged));

        /// <summary>
        /// Gets or sets the <see cref="Vector2"/> structure that describes the position and size of the <see cref="CanvasCircleGeometry"/>. The default is <see cref="Vector2.Zero"/>.
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
        /// <param name="d">CanvasCircleGeometry</param>
        /// <param name="e">DependencyProperty changed event arguments</param>
        private static void OnCenterChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var circleGeometry = (CanvasCircleGeometry)d;
            circleGeometry.OnCenterChanged();
        }

        /// <summary>
        /// Instance handler for the changes to the Center dependency property.
        /// </summary>
        private void OnCenterChanged()
        {
            UpdateGeometry();
        }

        /// <summary>
        /// Radius Dependency Property
        /// </summary>
        public static readonly DependencyProperty RadiusProperty = DependencyProperty.Register(
            "Radius",
            typeof(double),
            typeof(CanvasCircleGeometry),
            new PropertyMetadata(0, OnRadiusChanged));

        /// <summary>
        /// Gets or sets the radius value of the <see cref="CanvasCircleGeometry"/>.
        /// </summary>
        public double Radius
        {
            get => (double)GetValue(RadiusProperty);
            set => SetValue(RadiusProperty, value);
        }

        /// <summary>
        /// Handles changes to the Radius property.
        /// </summary>
        /// <param name="d"><see cref="CanvasCircleGeometry"/></param>
        /// <param name="e">DependencyProperty changed event arguments</param>
        private static void OnRadiusChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var circleGeometry = (CanvasCircleGeometry)d;
            circleGeometry.OnRadiusChanged();
        }

        /// <summary>
        /// Instance handler for the changes to the Radius dependency property.
        /// </summary>
        private void OnRadiusChanged()
        {
            UpdateGeometry();
        }

        /// <inheritdoc/>
        protected override void UpdateGeometry()
        {
            Geometry?.Dispose();

            Geometry = CanvasGeometry.CreateCircle(CompositionGenerator.Instance.Device, Center, (float)Radius);
        }
    }
}
