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

namespace Microsoft.Toolkit.Uwp.UI.Media.Brushes
{
    /// <summary>
    /// XAML equivalent of Win2d's CanvasLinearGradientBrush class which paints in linear gradient.
    /// </summary>
    [ContentProperty(Name = nameof(Stops))]
    public class LinearGradientCanvasBrush : RenderCanvasBrushBase
    {
        /// <summary>
        /// AlphaMode Dependency Property
        /// </summary>
        public static readonly DependencyProperty AlphaModeProperty = DependencyProperty.Register(
            "AlphaMode",
            typeof(CanvasAlphaMode),
            typeof(LinearGradientCanvasBrush),
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
            typeof(LinearGradientCanvasBrush),
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
        /// EdgeBehavior Dependency Property
        /// </summary>
        public static readonly DependencyProperty EdgeBehaviorProperty = DependencyProperty.Register(
            "EdgeBehavior",
            typeof(CanvasEdgeBehavior),
            typeof(LinearGradientCanvasBrush),
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
            typeof(LinearGradientCanvasBrush),
            new PropertyMetadata(default(Point), OnPropertyChanged));

        /// <summary>
        /// Gets or sets the point on the Canvas where the gradient stops.
        /// </summary>
        public Point EndPoint
        {
            get => (Point)GetValue(EndPointProperty);
            set => SetValue(EndPointProperty, value);
        }

        /// <summary>
        /// PostInterpolationSpace Dependency Property
        /// </summary>
        public static readonly DependencyProperty PostInterpolationSpaceProperty = DependencyProperty.Register(
            "PostInterpolationSpace",
            typeof(CanvasColorSpace),
            typeof(LinearGradientCanvasBrush),
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
            typeof(LinearGradientCanvasBrush),
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
        /// StartPoint Dependency Property
        /// </summary>
        public static readonly DependencyProperty StartPointProperty = DependencyProperty.Register(
            "StartPoint",
            typeof(Point),
            typeof(LinearGradientCanvasBrush),
            new PropertyMetadata(default(Point), OnPropertyChanged));

        /// <summary>
        /// Gets or sets the point on the Canvas where the gradient starts.
        /// </summary>
        public Point StartPoint
        {
            get => (Point)GetValue(StartPointProperty);
            set => SetValue(StartPointProperty, value);
        }

        /// <summary>
        /// Stops Dependency Property
        /// </summary>
        public static readonly DependencyProperty StopsProperty = DependencyProperty.Register(
            "Stops",
            typeof(GradientStopCollection),
            typeof(LinearGradientCanvasBrush),
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
        /// Method that is called whenever the dependency properties of the Brush changes
        /// </summary>
        /// <param name="d">The object whose property has changed.</param>
        /// <param name="e">Event arguments.</param>
        private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var brush = (LinearGradientCanvasBrush)d;

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

            CanvasBrush = new CanvasLinearGradientBrush(
                CompositionGenerator.Instance.Device,
                canvasGradientStops.ToArray(),
                EdgeBehavior,
                AlphaMode,
                PreInterpolationSpace,
                PostInterpolationSpace,
                BufferPrecision)
            {
                StartPoint = StartPoint.ToVector2(),
                EndPoint = EndPoint.ToVector2(),
                Opacity = (float)Opacity,
                Transform = Transform.ToMatrix3x2()
            };

            base.OnUpdated();
        }
    }
}
