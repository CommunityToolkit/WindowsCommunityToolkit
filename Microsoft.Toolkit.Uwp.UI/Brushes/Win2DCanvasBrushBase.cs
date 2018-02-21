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
using Microsoft.Graphics.Canvas.UI.Composition;
using Windows.Graphics.DirectX;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace Microsoft.Toolkit.Uwp.UI.Brushes
{
    /// <summary>
    /// Helper Brush class to interop with Win2D Canvas calls.
    /// </summary>
    public abstract class Win2DCanvasBrushBase : XamlCompositionBrushBase
    {
        /// <summary>
        /// Gets or sets the internal surface render width.  Modify during construction.
        /// </summary>
        protected float SURFACE_RESOLUTION_X { get; set; }

        /// <summary>
        /// Gets or sets the internal surface render height.  Modify during construction.
        /// </summary>
        protected float SURFACE_RESOLUTION_Y { get; set; }

        private CompositionSurfaceBrush _surfaceBrush;

        /// <summary>
        /// Implemented by parent class and called when canvas is being constructed for brush.
        /// </summary>
        /// <param name="device">Canvas device.</param>
        /// <param name="session">Canvas drawing session.</param>
        /// <param name="size">Size of surface to draw on.</param>
        protected abstract bool OnDraw(CanvasDevice device, CanvasDrawingSession session, Vector2 size);

        /// <summary>
        /// Initializes the Composition Brush.
        /// </summary>
        protected override void OnConnected()
        {
            base.OnConnected();

            // Delay creating composition resources until they're required.
            if (CompositionBrush == null)
            {
                // Abort if effects aren't supported.
                if (!CompositionCapabilities.GetForCurrentView().AreEffectsSupported())
                {
                    return;
                }

                var visual = Window.Current.Compositor.CreateSpriteVisual();
                visual.Size = new Vector2(SURFACE_RESOLUTION_X, SURFACE_RESOLUTION_Y);

                var device = CanvasDevice.GetSharedDevice();
                var graphics = CanvasComposition.CreateCompositionGraphicsDevice(Window.Current.Compositor, device);

                var surface = graphics.CreateDrawingSurface(visual.Size.ToSize(), DirectXPixelFormat.B8G8R8A8UIntNormalized, DirectXAlphaMode.Premultiplied);

                using (var session = CanvasComposition.CreateDrawingSession(surface))
                {
                    // Call Implementor to draw on session.
                    if (!OnDraw(device, session, visual.Size))
                    {
                        return;
                    }
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
            base.OnDisconnected();

            // Dispose of composition resources when no longer in use.
            if (CompositionBrush != null)
            {
                CompositionBrush.Dispose();
                CompositionBrush = null;
            }

            if (_surfaceBrush != null)
            {
                _surfaceBrush.Dispose();
                _surfaceBrush = null;
            }
        }
    }
}
