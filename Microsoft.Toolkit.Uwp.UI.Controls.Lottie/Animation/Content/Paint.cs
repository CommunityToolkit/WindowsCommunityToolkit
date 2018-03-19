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

using Microsoft.Graphics.Canvas.Geometry;
using Windows.UI;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Lottie.Animation.Content
{
    internal class Paint
    {
        public const int AntiAliasFlag = 0b01;
        public const int FilterBitmapFlag = 0b10;

        public int Flags { get; }

        internal Paint(int flags)
        {
            Flags = flags;
        }

        internal Paint()
            : this(0)
        {
        }

        public enum PaintStyle
        {
            Fill,
            Stroke
        }

        public byte Alpha
        {
            get => Color.A;
            set
            {
                var color = Color;
                color.A = value;
                Color = color;
            }
        }

        public Color Color { get; set; } = Colors.Transparent;

        public PaintStyle Style { get; set; }

        public ColorFilter ColorFilter { get; set; }

        public CanvasCapStyle StrokeCap { get; set; }

        public CanvasLineJoin StrokeJoin { get; set; }

        public float StrokeWidth { get; set; }

        public PathEffect PathEffect { get; set; }

        public PorterDuffXfermode Xfermode { get; set; }

        internal Shader Shader { get; set; }

        public Typeface Typeface { get; set; }

        public float TextSize { get; set; }
    }
}