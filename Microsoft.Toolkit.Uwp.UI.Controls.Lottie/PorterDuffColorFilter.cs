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
using Windows.UI;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Lottie
{
    /// <summary>
    /// A <see cref="ColorFilter"/> that uses the PorterDuff algorithm
    /// </summary>
    public abstract class PorterDuffColorFilter : ColorFilter
    {
        /// <summary>
        /// Gets or sets the color that this <see cref="ColorFilter"/> will use to blend the colors of it's target.
        /// </summary>
        public Color Color { get; set; }

        /// <summary>
        /// Gets the PorterDuff <see cref="PorterDuff.Mode"/> of this <see cref="ColorFilter"/>.
        /// </summary>
        public PorterDuff.Mode Mode { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PorterDuffColorFilter"/> class.
        /// </summary>
        /// <param name="color">The color that this <see cref="ColorFilter"/> will use to blend the colors of it's target.</param>
        /// <param name="mode">The Porter Duff <see cref="PorterDuff.Mode"/> of this <see cref="ColorFilter"/>.</param>
        protected internal PorterDuffColorFilter(Color color, PorterDuff.Mode mode)
        {
            Color = color;
            Mode = mode;
        }

        internal override ICanvasBrush Apply(CanvasDevice device, ICanvasBrush brush)
        {
            var originalColor = Colors.Transparent;
            if (brush is CanvasSolidColorBrush compositionColorBrush)
            {
                originalColor = compositionColorBrush.Color;
                compositionColorBrush.Dispose();
            }

            if (Color == Colors.Transparent)
            {
                return new CanvasSolidColorBrush(device, originalColor);
            }

            return new CanvasSolidColorBrush(device, Blend(originalColor, Color));
        }

        private static Color Blend(Color d, Color s)
        {
            byte a = (byte)(((d.A * s.A) + ((255 - s.A) * d.A)) / 255);
            byte r = (byte)(((d.A * s.R) + ((255 - s.A) * d.R)) / 255);
            byte g = (byte)(((d.A * s.G) + ((255 - s.A) * d.G)) / 255);
            byte b = (byte)(((d.A * s.B) + ((255 - s.A) * d.B)) / 255);
            return Color.FromArgb(a, r, g, b);
        }
    }
}