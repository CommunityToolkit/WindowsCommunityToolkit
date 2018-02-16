// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

using System.Numerics;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using Microsoft.Toolkit.Uwp.UI.Extensions;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;

namespace Microsoft.Toolkit.Uwp.UI.Brushes
{
    /// <summary>
    /// Goal: Drop-in replacement for https://msdn.microsoft.com/en-us/library/system.windows.media.radialgradientbrush(v=vs.110).aspx.
    /// </summary>
    [ContentProperty(Name = nameof(GradientStops))]
    public class RadialGradientBrush : Win2DCanvasBrushBase
    {
        private CanvasRadialGradientBrush _gradientBrush;

        /// <summary>
        /// Gets or sets the brush's gradient stops.
        /// </summary>
        public GradientStopCollection GradientStops
        {
            get { return (GradientStopCollection)GetValue(GradientStopsProperty); }
            set { SetValue(GradientStopsProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="GradientStops"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty GradientStopsProperty =
            DependencyProperty.Register(nameof(GradientStops), typeof(GradientStopCollection), typeof(RadialGradientBrush), new PropertyMetadata(null, new PropertyChangedCallback(OnGradientStopsChanged)));

        /// <summary>
        /// Gets or sets the center of the outermost circle of the radial gradient.  The defaults is 0.5,0.5.
        /// </summary>
        public Point Center
        {
            get { return (Point)GetValue(CenterProperty); }
            set { SetValue(CenterProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="Center"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CenterProperty =
            DependencyProperty.Register(nameof(Center), typeof(Point), typeof(RadialGradientBrush), new PropertyMetadata(new Point(0.5, 0.5)));

        /// <summary>
        /// Gets or sets the location of the two-dimensional focal point that defines the beginning of the gradient.  The default is 0.5,0.5.
        /// </summary>
        public Point GradientOrigin
        {
            get { return (Point)GetValue(GradientOriginProperty); }
            set { SetValue(GradientOriginProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="GradientOrigin"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty GradientOriginProperty =
            DependencyProperty.Register(nameof(GradientOrigin), typeof(Point), typeof(RadialGradientBrush), new PropertyMetadata(new Point(0.5, 0.5)));

        /// <summary>
        /// Gets or sets the horizontal radius of the outermost circle of the radial gradient.  The default is 0.5.
        /// </summary>
        public double RadiusX
        {
            get { return (double)GetValue(RadiusXProperty); }
            set { SetValue(RadiusXProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="RadiusX"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty RadiusXProperty =
            DependencyProperty.Register(nameof(RadiusX), typeof(double), typeof(RadialGradientBrush), new PropertyMetadata(0.5));

        /// <summary>
        /// Gets or sets the vertical radius of the outermost circle of the radial gradient.  The default is 0.5.
        /// </summary>
        public double RadiusY
        {
            get { return (double)GetValue(RadiusYProperty); }
            set { SetValue(RadiusYProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="RadiusX"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty RadiusYProperty =
            DependencyProperty.Register(nameof(RadiusY), typeof(double), typeof(RadialGradientBrush), new PropertyMetadata(0.5));

        private static void OnGradientStopsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RadialGradientBrush"/> class.
        /// </summary>
        public RadialGradientBrush()
        {
            this.FallbackColor = Colors.Transparent;

            // For this brush, as long as we're greater than 1x1 it doesn't seem to effect quality of output.
            this.SURFACE_RESOLUTION_X = 512;
            this.SURFACE_RESOLUTION_Y = 512;

            GradientStops = new GradientStopCollection();
        }

        public RadialGradientBrush(Color startColor, Color endColor)
            : this()
        {
            GradientStops = new GradientStopCollection();
            GradientStops.Add(new GradientStop() { Color = startColor, Offset = 0.0 });
            GradientStops.Add(new GradientStop() { Color = endColor, Offset = 1.0 });
        }

        public RadialGradientBrush(GradientStopCollection gradientStopCollection)
            : this()
        {
            GradientStops = gradientStopCollection;
        }

        public override void OnDraw(CanvasDevice device, CanvasDrawingSession session, Vector2 size)
        {
            if (_gradientBrush != null)
            {
                _gradientBrush.Dispose();
                _gradientBrush = null;
            }

            // Create our Brush
            _gradientBrush = new CanvasRadialGradientBrush(device, this.GradientStops.ToWin2DGradientStops());
            _gradientBrush.RadiusX = size.X * (float)RadiusX;
            _gradientBrush.RadiusY = size.Y * (float)RadiusY;
            _gradientBrush.Center = size * Center.ToVector2();
            _gradientBrush.OriginOffset = size * (GradientOrigin.ToVector2() - Center.ToVector2());
            _gradientBrush.Opacity = (float)Opacity;

            // Use brush to draw on our canvas
            session.FillRectangle(size.ToRect(), _gradientBrush);
        }

        protected override void OnDisconnected()
        {
            base.OnDisconnected();

            if (_gradientBrush != null)
            {
                _gradientBrush.Dispose();
                _gradientBrush = null;

                // Our GradientStops don't seem to reset between initializations...?
                GradientStops = null;
            }
        }
    }

    /// <summary>
    /// Helper extensions for <see cref="RadialGradientBrush"/>.
    /// </summary>
    public static class RadialGradientBrushHelpers
    {
        /// <summary>
        /// Converts a <see cref="GradientStopCollection"/> to an array of <see cref="CanvasGradientStop"/>.
        /// </summary>
        /// <param name="stops"><see cref="GradientStopCollection"/> collection of gradient stops.</param>
        /// <returns>New array of <see cref="CanvasGradientStop"/> stops.</returns>
        public static CanvasGradientStop[] ToWin2DGradientStops(this GradientStopCollection stops)
        {
            var canvasStops = new CanvasGradientStop[stops.Count];

            int x = 0;
            foreach (var stop in stops)
            {
                canvasStops[x++] = new CanvasGradientStop() { Color = stop.Color, Position = (float)stop.Offset };
            }

            return canvasStops;
        }
    }
}
