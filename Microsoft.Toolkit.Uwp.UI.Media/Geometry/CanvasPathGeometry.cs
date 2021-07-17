// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Numerics;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using Microsoft.Graphics.Canvas.Geometry;
using Windows.UI;
using Windows.UI.Xaml;

namespace Microsoft.Toolkit.Uwp.UI.Media
{
    /// <summary>
    /// Represents a complex vector-based shape geometry that may be composed of arcs, curves, ellipses, lines, rectangles, rounded rectangles, squircles.
    /// Also provides several helper methods to create Win2d objects.
    /// </summary>
    public class CanvasPathGeometry : CanvasCoreGeometry
    {
        /// <summary>
        /// Data Dependency Property
        /// </summary>
        public static readonly DependencyProperty DataProperty = DependencyProperty.Register(
            "Data",
            typeof(string),
            typeof(CanvasPathGeometry),
            new PropertyMetadata(string.Empty, OnDataChanged));

        /// <summary>
        /// Gets or sets the path data for the associated Win2d CanvasGeometry defined in the Win2d Path Mini Language.
        /// </summary>
        public string Data
        {
            get => (string)GetValue(DataProperty);
            set => SetValue(DataProperty, value);
        }

        /// <summary>
        /// Handles changes to the Data property.
        /// </summary>
        /// <param name="d">CanvasPathGeometry</param>
        /// <param name="e">DependencyProperty changed event arguments</param>
        private static void OnDataChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var pathGeometry = (CanvasPathGeometry)d;
            pathGeometry.OnDataChanged();
        }

        /// <summary>
        /// Instance handler for the changes to the Data dependency property.
        /// </summary>
        protected virtual void OnDataChanged()
        {
            OnUpdateGeometry();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CanvasPathGeometry"/> class.
        /// </summary>
        public CanvasPathGeometry()
        {
            this.Geometry = null;
        }

        /// <inheritdoc/>
        protected override void OnUpdateGeometry()
        {
            // Dispose previous CanvasGeometry (if any)
            Geometry?.Dispose();
            Geometry = null;

            try
            {
                Geometry = CreateGeometry(CompositionGenerator.Instance.Device, Data);
            }
            catch (Exception)
            {
                Geometry = null;
            }

            RaiseUpdatedEvent();
        }

        /// <summary>
        /// Parses the Path data string and converts it to CanvasGeometry.
        /// </summary>
        /// <param name="pathData">Path data</param>
        /// <returns><see cref="CanvasGeometry"/></returns>
        public static CanvasGeometry CreateGeometry(string pathData)
        {
            return CreateGeometry(CompositionGenerator.Instance.Device, pathData);
        }

        /// <summary>
        /// Parses the Path data string and converts it to CanvasGeometry.
        /// </summary>
        /// <param name="resourceCreator"><see cref="ICanvasResourceCreator"/></param>
        /// <param name="pathData">Path data</param>
        /// <returns><see cref="CanvasGeometry"/></returns>
        public static CanvasGeometry CreateGeometry(ICanvasResourceCreator resourceCreator, string pathData)
        {
            using (new CultureShield("en-US"))
            {
                // Get the CanvasGeometry from the path data
                return CanvasGeometryParser.Parse(resourceCreator, pathData);
            }
        }

        /// <summary>
        /// Creates a Squircle geometry with the specified extents.
        /// </summary>
        /// <param name="resourceCreator">Resource creator</param>
        /// <param name="x">X offset of the TopLeft corner of the Squircle</param>
        /// <param name="y">Y offset of the TopLeft corner of the Squircle</param>
        /// <param name="width">Width of the Squircle</param>
        /// <param name="height">Height of the Squircle</param>
        /// <param name="radiusX">Corner Radius on the x-axis</param>
        /// <param name="radiusY">Corner Radius on the y-axis</param>
        /// <returns><see cref="CanvasGeometry"/></returns>
        public static CanvasGeometry CreateSquircle(ICanvasResourceCreator resourceCreator, float x, float y, float width, float height, float radiusX, float radiusY)
        {
            using var pathBuilder = new CanvasPathBuilder(resourceCreator);
            pathBuilder.AddSquircleFigure(x, y, width, height, radiusX, radiusY);
            return CanvasGeometry.CreatePath(pathBuilder);
        }

        /// <summary>
        /// Parses the given Brush data string and converts it to ICanvasBrush.
        /// </summary>
        /// <param name="resourceCreator">ICanvasResourceCreator</param>
        /// <param name="brushData">Brush data in string format</param>
        /// <returns><see cref="ICanvasBrush"/></returns>
        public static ICanvasBrush CreateBrush(ICanvasResourceCreator resourceCreator, string brushData)
        {
            using (new CultureShield("en-US"))
            {
                return CanvasBrushParser.Parse(resourceCreator, brushData);
            }
        }

        /// <summary>
        /// Parses the given Stroke data string and converts it to ICanvasStroke.
        /// </summary>
        /// <param name="resourceCreator">ICanvasResourceCreator</param>
        /// <param name="strokeData">Stroke data in string format</param>
        /// <returns><see cref="ICanvasStroke"/></returns>
        public static ICanvasStroke CreateStroke(ICanvasResourceCreator resourceCreator, string strokeData)
        {
            using (new CultureShield("en-US"))
            {
                return CanvasStrokeParser.Parse(resourceCreator, strokeData);
            }
        }

        /// <summary>
        /// Parses the give CanvasStrokeStyle data string and converts it to CanvasStrokeStyle.
        /// </summary>
        /// <param name="styleData">CanvasStrokeStyle data in string format</param>
        /// <returns><see cref="CanvasStrokeStyle"/> object</returns>
        public static CanvasStrokeStyle CreateStrokeStyle(string styleData)
        {
            using (new CultureShield("en-US"))
            {
                return CanvasStrokeStyleParser.Parse(styleData);
            }
        }

        /// <summary>
        /// Converts the color string in Hexadecimal or HDR color format to the corresponding Color object.
        /// The hexadecimal color string should be in #RRGGBB or #AARRGGBB format.
        /// The '#' character is optional.
        /// The HDR color string should be in R G B A format.
        /// (R, G, B &amp; A should have value in the range between 0 and 1, inclusive)
        /// </summary>
        /// <param name="colorString">Color string in Hexadecimal or HDR format</param>
        /// <returns>Color</returns>
        public static Color CreateColor(string colorString)
        {
            using (new CultureShield("en-US"))
            {
                return ColorParser.Parse(colorString);
            }
        }

        /// <summary>
        /// Converts a Vector4 High Dynamic Range Color to Color object.
        /// Negative components of the Vector4 will be sanitized by taking the absolute
        /// value of the component. The HDR Color components should have value in
        /// the range between 0 and 1, inclusive. If they are more than 1, they
        /// will be clamped at 1.
        /// Vector4's X, Y, Z, W components match to Color's R, G, B, A components respectively.
        /// </summary>
        /// <param name="hdrColor">High Dynamic Range Color</param>
        /// <returns>Color</returns>
        public static Color CreateColor(Vector4 hdrColor)
        {
            using (new CultureShield("en-US"))
            {
                return ColorParser.Parse(hdrColor);
            }
        }
    }
}