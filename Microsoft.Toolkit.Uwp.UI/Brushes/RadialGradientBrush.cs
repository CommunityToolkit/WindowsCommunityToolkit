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

//// UWP Replacement for WPF RadialGradientBrush: https://msdn.microsoft.com/en-us/library/system.windows.media.radialgradientbrush(v=vs.110).aspx.

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
    /// RadialGradientBrush - This GradientBrush defines its Gradient as an interpolation
    /// within an Ellipse.
    /// </summary>
    [ContentProperty(Name = nameof(GradientStops))]
    public partial class RadialGradientBrush : Win2DCanvasBrushBase
    {
        private CanvasRadialGradientBrush _gradientBrush;

        private static void OnGradientStopsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RadialGradientBrush"/> class.
        /// </summary>
        public RadialGradientBrush()
        {
            this.FallbackColor = Colors.Transparent;

            // Rendering surface size, if this is too small the gradient will be pixelated.
            // This seems like a good compromise.
            this.SURFACE_RESOLUTION_X = 512;
            this.SURFACE_RESOLUTION_Y = 512;

            GradientStops = new GradientStopCollection();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RadialGradientBrush"/> class
        /// with with two colors specified for GradientStops at
        /// offsets 0.0 and 1.0.
        /// </summary>
        /// <param name="startColor"> The Color at offset 0.0. </param>
        /// <param name="endColor"> The Color at offset 1.0. </param>
        public RadialGradientBrush(Color startColor, Color endColor)
            : this()
        {
            GradientStops = new GradientStopCollection();
            GradientStops.Add(new GradientStop() { Color = startColor, Offset = 0.0 });
            GradientStops.Add(new GradientStop() { Color = endColor, Offset = 1.0 });
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RadialGradientBrush"/> class with GradientStops set to the passed-in collection.
        /// </summary>
        /// <param name="gradientStopCollection"> GradientStopCollection to set on this brush. </param>
        public RadialGradientBrush(GradientStopCollection gradientStopCollection)
            : this()
        {
            GradientStops = gradientStopCollection;
        }

        protected override void OnDraw(CanvasDevice device, CanvasDrawingSession session, Vector2 size)
        {
            if (_gradientBrush != null)
            {
                _gradientBrush.Dispose();
                _gradientBrush = null;
            }

            // Create our Brush
            _gradientBrush = new CanvasRadialGradientBrush(
                                    device,
                                    this.GradientStops.ToWin2DGradientStops(),
                                    SpreadMethod.ToEdgeBehavior(),
                                    CanvasAlphaMode.Premultiplied)
            {
                // Calculate Surface coordinates from 0.0-1.0 range given in WPF brush
                RadiusX = size.X * (float)RadiusX,
                RadiusY = size.Y * (float)RadiusY,
                Center = size * Center.ToVector2(),

                // Calculate Win2D Offset from origin/center used in WPF brush
                OriginOffset = size * (GradientOrigin.ToVector2() - Center.ToVector2()),

                // TODO: Need to adjust the opacity to better match output from WPF - maybe have to play with AlphaMode?
                Opacity = (float)Opacity
            };

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
}
