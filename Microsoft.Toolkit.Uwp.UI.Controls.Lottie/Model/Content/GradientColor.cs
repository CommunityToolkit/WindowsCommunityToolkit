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

using Microsoft.Toolkit.Uwp.UI.Controls.Lottie.Utils;
using Windows.UI;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Lottie.Model.Content
{
    internal class GradientColor
    {
        private readonly float[] _positions;
        private readonly Color[] _colors;

        internal GradientColor(float[] positions, Color[] colors)
        {
            _positions = positions;
            _colors = colors;
        }

        internal virtual float[] Positions => _positions;

        internal virtual Color[] Colors => _colors;

        internal virtual int Size => _colors.Length;

        internal virtual void Lerp(GradientColor gc1, GradientColor gc2, float progress)
        {
            if (gc1._colors.Length != gc2._colors.Length)
            {
                throw new System.ArgumentException("Cannot interpolate between gradients. Lengths vary (" + gc1._colors.Length + " vs " + gc2._colors.Length + ")");
            }

            for (var i = 0; i < gc1._colors.Length; i++)
            {
                _positions[i] = MiscUtils.Lerp(gc1._positions[i], gc2._positions[i], progress);

                var gamma = GammaEvaluator.Evaluate(progress, gc1._colors[i], gc2._colors[i]);

                _colors[i] = gamma;
            }
        }
    }
}