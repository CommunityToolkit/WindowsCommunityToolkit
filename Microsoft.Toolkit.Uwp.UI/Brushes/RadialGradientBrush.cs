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

using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using Microsoft.Graphics.Canvas.Effects;
using Microsoft.Graphics.Canvas.UI.Composition;
using Microsoft.Toolkit.Uwp.UI.Extensions;
using System.Numerics;
using Windows.Foundation;
using Windows.Graphics.DirectX;
using Windows.UI;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace Microsoft.Toolkit.Uwp.UI.Brushes
{
    /// <summary>
    /// Goal: Drop-in replacement for https://msdn.microsoft.com/en-us/library/system.windows.media.radialgradientbrush(v=vs.110).aspx.
    /// </summary>
    public class RadialGradientBrush : XamlCompositionBrushBase
    {
        private CanvasRadialGradientBrush _gradientBrush;
        private CompositionSurfaceBrush _surfaceBrush;

        /// <summary>
        /// Initializes a new instance of the <see cref="RadialGradientBrush"/> class.
        /// </summary>
        public RadialGradientBrush()
        {
            this.FallbackColor = Colors.Transparent;
        }

        /// <summary>
        /// Initializes the Composition Brush.
        /// </summary>
        protected override void OnConnected()
        {
            // Delay creating composition resources until they're required.
            if (CompositionBrush == null)
            {
                // Abort if effects aren't supported.
                if (!CompositionCapabilities.GetForCurrentView().AreEffectsSupported())
                {
                    return;
                }

                var visual = Window.Current.Compositor.CreateSpriteVisual();
                visual.Size = new Vector2(64.0f, 64.0f);

                var size = visual?.Parent?.Size;

                if (visual.Size.X == 0 || visual.Size.Y == 0)
                {
                    // We have no dimension, so we can't create a surface yet.
                    return;
                }

                var device = new CanvasDevice();
                var graphics = CanvasComposition.CreateCompositionGraphicsDevice(Window.Current.Compositor, device);

                var surface = graphics.CreateDrawingSurface(visual.Size.ToSize(), DirectXPixelFormat.B8G8R8A8UIntNormalized, DirectXAlphaMode.Premultiplied);

                _gradientBrush = new CanvasRadialGradientBrush(device, Colors.Red, Colors.Green);
                _gradientBrush.RadiusX = visual.Size.X * 0.5f;
                _gradientBrush.RadiusY = visual.Size.Y * 0.5f;
                _gradientBrush.Center = visual.Size * new Vector2(0.6f, 0.5f);
                _gradientBrush.OriginOffset = visual.Size * new Vector2(0.1f, 0.4f);

                using (var session = CanvasComposition.CreateDrawingSession(surface))
                {
                    session.FillRectangle(visual.Size.ToRect(), _gradientBrush);
                }

                _surfaceBrush = Window.Current.Compositor.CreateSurfaceBrush(surface);
                _surfaceBrush.Stretch = CompositionStretch.Fill;

                CompositionBrush = _surfaceBrush;
            }
        }

        /// <summary>
        /// Deconstructs the Composition Brush.
        /// </summary>
        protected override void OnDisconnected()
        {
            // Dispose of composition resources when no longer in use.
            if (CompositionBrush != null)
            {
                CompositionBrush.Dispose();
                CompositionBrush = null;
            }
        }
    }
}
