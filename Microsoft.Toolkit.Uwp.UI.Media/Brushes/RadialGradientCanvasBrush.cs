// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Numerics;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;

namespace Microsoft.Toolkit.Uwp.UI.Media
{
    /// <summary>
    /// XAML equivalent of Win2d's CanvasRadialGradientBrush class which paints in radial gradient.
    /// </summary>
    [ContentProperty(Name = nameof(Stops))]
    public class RadialGradientCanvasBrush : RenderCanvasBrushBase
    {
        /// <summary>
        /// AlphaMode Dependency Property
        /// </summary>
        public static readonly DependencyProperty AlphaModeProperty = DependencyProperty.Register(
            "AlphaMode",
            typeof(CanvasAlphaMode),
            typeof(RadialGradientCanvasBrush),
            new PropertyMetadata(CanvasAlphaMode.Premultiplied, OnPropertyChanged));

        /// <summary>
        /// Gets or sets the way in which the Alpha channel affects color channels.
        /// </summary>
        public CanvasAlphaMode AlphaMode
        {
            get => (CanvasAlphaMode)GetValue(AlphaModeProperty);
            set => SetValue(AlphaModeProperty, value);
        }

        /// <summary>
        /// BufferPrecision Dependency Property
        /// </summary>
        public static readonly DependencyProperty BufferPrecisionProperty = DependencyProperty.Register(
            "BufferPrecision",
            typeof(CanvasBufferPrecision),
            typeof(RadialGradientCanvasBrush),
            new PropertyMetadata(CanvasBufferPrecision.Precision8UIntNormalized, OnPropertyChanged));

        /// <summary>
        /// Gets or sets the precision used for computation.
        /// </summary>
        public CanvasBufferPrecision BufferPrecision
        {
            get => (CanvasBufferPrecision)GetValue(BufferPrecisionProperty);
            set => SetValue(BufferPrecisionProperty, value);
        }

        /// <summary>
        /// Center Dependency Property
        /// </summary>
        public static readonly DependencyProperty CenterProperty = DependencyProperty.Register(
            "Center",
            typeof(Point),
            typeof(RadialGradientCanvasBrush),
            new PropertyMetadata(default(Point), OnPropertyChanged));

        /// <summary>
        /// Gets or sets the center of the brush's radial gradient.
        /// </summary>
        public Point Center
        {
            get => (Point)GetValue(CenterProperty);
            set => SetValue(CenterProperty, value);
        }

        /// <summary>
        /// EdgeBehavior Dependency Property
        /// </summary>
        public static readonly DependencyProperty EdgeBehaviorProperty = DependencyProperty.Register(
            "EdgeBehavior",
            typeof(CanvasEdgeBehavior),
            typeof(RadialGradientCanvasBrush),
            new PropertyMetadata(CanvasEdgeBehavior.Clamp, OnPropertyChanged));

        /// <summary>
        /// Gets or sets the behavior of the pixels which fall outside of the gradient's typical rendering area.
        /// </summary>
        public CanvasEdgeBehavior EdgeBehavior
        {
            get => (CanvasEdgeBehavior)GetValue(EdgeBehaviorProperty);
            set => SetValue(EdgeBehaviorProperty, value);
        }

        /// <summary>
        /// EndPoint Dependency Property
        /// </summary>
        public static readonly DependencyProperty EndPointProperty = DependencyProperty.Register(
            "EndPoint",
            typeof(Point),
            typeof(RadialGradientCanvasBrush),
            new PropertyMetadata(default(Point), OnPropertyChanged));

        /// <summary>
        /// OriginOffset Dependency Property
        /// </summary>
        public static readonly DependencyProperty OriginOffsetProperty = DependencyProperty.Register(
            "OriginOffset",
            typeof(Point),
            typeof(RadialGradientCanvasBrush),
            new PropertyMetadata(default(Point), OnPropertyChanged));

        /// <summary>
        /// Gets or sets the displacement from Center, used to form the brush's radial gradient.
        /// </summary>
        public Point OriginOffset
        {
            get => (Point)GetValue(OriginOffsetProperty);
            set => SetValue(OriginOffsetProperty, value);
        }

        /// <summary>
        /// PostInterpolationSpace Dependency Property
        /// </summary>
        public static readonly DependencyProperty PostInterpolationSpaceProperty = DependencyProperty.Register(
            "PostInterpolationSpace",
            typeof(CanvasColorSpace),
            typeof(RadialGradientCanvasBrush),
            new PropertyMetadata(CanvasColorSpace.Srgb, OnPropertyChanged));

        /// <summary>
        /// Gets or sets the the color space to be used after interpolation.
        /// </summary>
        public CanvasColorSpace PostInterpolationSpace
        {
            get => (CanvasColorSpace)GetValue(PostInterpolationSpaceProperty);
            set => SetValue(PostInterpolationSpaceProperty, value);
        }

        /// <summary>
        /// PreInterpolationSpace Dependency Property
        /// </summary>
        public static readonly DependencyProperty PreInterpolationSpaceProperty = DependencyProperty.Register(
            "PreInterpolationSpace",
            typeof(CanvasColorSpace),
            typeof(RadialGradientCanvasBrush),
            new PropertyMetadata(CanvasColorSpace.Srgb, OnPropertyChanged));

        /// <summary>
        /// Gets or sets the the color space to be used before interpolation.
        /// </summary>
        public CanvasColorSpace PreInterpolationSpace
        {
            get => (CanvasColorSpace)GetValue(PreInterpolationSpaceProperty);
            set => SetValue(PreInterpolationSpaceProperty, value);
        }

        /// <summary>
        /// RadiusX Dependency Property
        /// </summary>
        public static readonly DependencyProperty RadiusXProperty = DependencyProperty.Register(
            "RadiusX",
            typeof(double),
            typeof(RadialGradientCanvasBrush),
            new PropertyMetadata(0d, OnPropertyChanged));

        /// <summary>
        /// Gets or sets the horizontal radius of the brush's radial gradient.
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
            typeof(RadialGradientCanvasBrush),
            new PropertyMetadata(0d, OnPropertyChanged));

        /// <summary>
        /// Gets or sets the vertical radius of the brush's radial gradient.
        /// </summary>
        public double RadiusY
        {
            get => (double)GetValue(RadiusYProperty);
            set => SetValue(RadiusYProperty, value);
        }

        /// <summary>
        /// Stops Dependency Property
        /// </summary>
        public static readonly DependencyProperty StopsProperty = DependencyProperty.Register(
            "Stops",
            typeof(GradientStopCollection),
            typeof(RadialGradientCanvasBrush),
            new PropertyMetadata(null, OnPropertyChanged));

        /// <summary>
        /// Gets or sets the gradient stops that comprise the brush.
        /// </summary>
        public GradientStopCollection Stops
        {
            get => (GradientStopCollection)GetValue(StopsProperty);
            set => SetValue(StopsProperty, value);
        }

        /// <summary>
        /// Method that is called whenever the dependency properties of the Brush changes.
        /// </summary>
        /// <param name="d">The object whose property has changed.</param>
        /// <param name="e">Event arguments.</param>
        private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var brush = (RadialGradientCanvasBrush)d;

            // Recreate the canvas brush on any property change.
            brush.OnUpdated();
        }

        /// <inheritdoc/>
        protected override void OnUpdated()
        {
            if (Stops == null)
            {
                return;
            }

            var canvasGradientStops = new List<CanvasGradientStop>();
            foreach (var stop in Stops)
            {
                canvasGradientStops.Add(new CanvasGradientStop()
                {
                    Color = stop.Color,
                    Position = (float)stop.Offset
                });
            }

            CanvasBrush = new CanvasRadialGradientBrush(
                CompositionGenerator.Instance.Device,
                canvasGradientStops.ToArray(),
                EdgeBehavior,
                AlphaMode,
                PreInterpolationSpace,
                PostInterpolationSpace,
                BufferPrecision)
            {
                Center = Center.ToVector2(),
                RadiusX = (float)RadiusX,
                RadiusY = (float)RadiusY,
                OriginOffset = OriginOffset.ToVector2(),
                Opacity = (float)Opacity,
                Transform = Transform.ToMatrix3x2()
            };

            base.OnUpdated();
        }
    }
}
