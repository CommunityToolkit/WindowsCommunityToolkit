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
using Windows.UI;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Lottie.Model
{
    internal class DocumentData
    {
        private readonly string _text;
        private readonly string _fontName;
        private readonly double _size;
        private readonly int _justification;
        private readonly int _tracking;
        private readonly double _lineHeight;
        private readonly double _baselineShift;
        private readonly Color _color;
        private readonly Color _strokeColor;
        private readonly int _strokeWidth;
        private readonly bool _strokeOverFill;

        internal string Text => _text;

        internal string FontName => _fontName;

        internal double Size => _size;

        internal int Justification => _justification;

        internal int Tracking => _tracking;

        internal double LineHeight => _lineHeight;

        internal double BaselineShift => _baselineShift;

        internal Color Color => _color;

        internal Color StrokeColor => _strokeColor;

        internal int StrokeWidth => _strokeWidth;

        internal bool StrokeOverFill => _strokeOverFill;

        internal DocumentData(string text, string fontName, double size, int justification, int tracking, double lineHeight, double baselineShift, Color color, Color strokeColor, int strokeWidth, bool strokeOverFill)
        {
            _text = text;
            _fontName = fontName;
            _size = size;
            _justification = justification;
            _tracking = tracking;
            _lineHeight = lineHeight;
            _baselineShift = baselineShift;
            _color = color;
            _strokeColor = strokeColor;
            _strokeWidth = strokeWidth;
            _strokeOverFill = strokeOverFill;
        }

        public override int GetHashCode()
        {
            int result;
            long temp;
            result = Text.GetHashCode();
            result = (31 * result) + FontName.GetHashCode();
            result = (int)((31 * result) + Size);
            result = (31 * result) + Justification;
            result = (31 * result) + Tracking;
            temp = BitConverter.DoubleToInt64Bits(LineHeight);
            result = (31 * result) + (int)(temp ^ ((long)((ulong)temp >> 32)));
            result = (31 * result) + Color.GetHashCode();
            return result;
        }
    }
}
