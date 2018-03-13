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

using System;
using System.Numerics;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using Windows.UI;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Lottie.Animation.Content
{
    internal class RadialGradient : Gradient, IDisposable
    {
        private readonly float _x0;
        private readonly float _y0;
        private readonly float _r;
        private readonly CanvasGradientStop[] _canvasGradientStopCollection;
        private CanvasRadialGradientBrush _canvasRadialGradientBrush;

        public RadialGradient(float x0, float y0, float r, Color[] colors, float[] positions)
        {
            _x0 = x0;
            _y0 = y0;
            _r = r;
            _canvasGradientStopCollection = new CanvasGradientStop[colors.Length];
            for (var i = 0; i < colors.Length; i++)
            {
                _canvasGradientStopCollection[i] = new CanvasGradientStop
                {
                    Color = colors[i],
                    Position = positions[i]
                };
            }
        }

        public override ICanvasBrush GetBrush(CanvasDevice device, byte alpha)
        {
            if (_canvasRadialGradientBrush == null)
            {
                var center = new Vector2(_x0, _y0);
                center = LocalMatrix.Transform(center);

                _canvasRadialGradientBrush = new CanvasRadialGradientBrush(device, _canvasGradientStopCollection, CanvasEdgeBehavior.Clamp, CanvasAlphaMode.Straight)
                {
                    Center = center,
                    RadiusX = _r,
                    RadiusY = _r
                };
            }

            _canvasRadialGradientBrush.Opacity = alpha / 255f;

            return _canvasRadialGradientBrush;
        }

        private void Dispose(bool disposing)
        {
            if (_canvasRadialGradientBrush != null)
            {
                _canvasRadialGradientBrush.Dispose();
                _canvasRadialGradientBrush = null;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~RadialGradient()
        {
            Dispose(false);
        }
    }
}