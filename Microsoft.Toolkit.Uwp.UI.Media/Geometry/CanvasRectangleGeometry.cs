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
    /// Represents a rectangular shape
    /// </summary>
    public class CanvasRectangleGeometry : CanvasCoreGeometry
    {
        /// <summary>
        /// Rect Dependency Property
        /// </summary>
        public static readonly DependencyProperty RectProperty = DependencyProperty.Register(
            "Rect",
            typeof(Vector4),
            typeof(CanvasRectangleGeometry),
            new PropertyMetadata(Vector4.Zero, OnRectChanged));

        /// <summary>
        /// Gets or sets the <see cref="Vector4"/> structure that describes the position and size of the geometry. The default is <see cref="Vector4.Zero"/>.
        /// </summary>
        [TypeConverter(typeof(Vector4Converter))]
        public Vector4 Rect
        {
            get => (Vector4)GetValue(RectProperty);
            set => SetValue(RectProperty, value);
        }

        /// <summary>
        /// Handles changes to the Rect property.
        /// </summary>
        /// <param name="d">CanvasRectangleGeometry</param>
        /// <param name="e">DependencyProperty changed event arguments</param>
        private static void OnRectChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var rectangleGeometry = (CanvasRectangleGeometry)d;
            rectangleGeometry.OnRectChanged();
        }

        /// <summary>
        /// Instance handler for the changes to the Rect dependency property.
        /// </summary>
        private void OnRectChanged()
        {
            UpdateGeometry();
        }

        /// <inheritdoc/>
        protected override void UpdateGeometry()
        {
            Geometry = CanvasGeometry.CreateRectangle(CompositionGenerator.Instance.Device, Rect.X, Rect.Y, Rect.Z, Rect.W);
        }
    }
}
