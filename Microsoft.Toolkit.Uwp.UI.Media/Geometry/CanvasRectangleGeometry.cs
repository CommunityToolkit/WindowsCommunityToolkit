// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Graphics.Canvas.Geometry;
using Windows.UI.Xaml;

namespace Microsoft.Toolkit.Uwp.UI.Media
{
    /// <summary>
    /// Represents a rectangular geometry object with the specified extents.
    /// </summary>
    public class CanvasRectangleGeometry : CanvasCoreGeometry
    {
        /// <summary>
        /// X Dependency Property
        /// </summary>
        public static readonly DependencyProperty XProperty = DependencyProperty.Register(
            "X",
            typeof(double),
            typeof(CanvasRectangleGeometry),
            new PropertyMetadata(0d, OnPropertyChanged));

        /// <summary>
        /// Gets or sets the x-coordinate of the upper-left corner of the rectangle geometry.
        /// </summary>
        public double X
        {
            get => (double)GetValue(XProperty);
            set => SetValue(XProperty, value);
        }

        /// <summary>
        /// Y Dependency Property
        /// </summary>
        public static readonly DependencyProperty YProperty = DependencyProperty.Register(
            "Y",
            typeof(double),
            typeof(CanvasRectangleGeometry),
            new PropertyMetadata(0d, OnPropertyChanged));

        /// <summary>
        /// Gets or sets the y-coordinate of the upper-left corner of the rectangle geometry.
        /// </summary>
        public double Y
        {
            get => (double)GetValue(YProperty);
            set => SetValue(YProperty, value);
        }

        /// <summary>
        /// Width Dependency Property
        /// </summary>
        public static readonly DependencyProperty WidthProperty = DependencyProperty.Register(
            "Width",
            typeof(double),
            typeof(CanvasRectangleGeometry),
            new PropertyMetadata(0d, OnPropertyChanged));

        /// <summary>
        /// Gets or sets the width of the rectangle geometry.
        /// </summary>
        public double Width
        {
            get => (double)GetValue(WidthProperty);
            set => SetValue(WidthProperty, value);
        }

        /// <summary>
        /// Height Dependency Property
        /// </summary>
        public static readonly DependencyProperty HeightProperty = DependencyProperty.Register(
            "Height",
            typeof(double),
            typeof(CanvasRectangleGeometry),
            new PropertyMetadata(0d, OnPropertyChanged));

        /// <summary>
        /// Gets or sets the height of the rectangle geometry.
        /// </summary>
        public double Height
        {
            get => (double)GetValue(HeightProperty);
            set => SetValue(HeightProperty, value);
        }

        /// <summary>
        /// Method that is called whenever the dependency properties of the Brush changes
        /// </summary>
        /// <param name="d">The object whose property has changed</param>
        /// <param name="e">Event arguments</param>
        private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var geometry = (CanvasRectangleGeometry)d;

            // Recreate the geometry on any property change.
            geometry.OnUpdateGeometry();
        }

        /// <inheritdoc/>
        protected override void OnUpdateGeometry()
        {
            Geometry = CanvasGeometry.CreateRectangle(CompositionGenerator.Instance.Device, (float)X, (float)Y, (float)Width, (float)Height);

            RaiseUpdatedEvent();
        }
    }
}
