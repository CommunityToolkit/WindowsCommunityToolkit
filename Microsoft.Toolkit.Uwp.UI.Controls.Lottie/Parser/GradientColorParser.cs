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

using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Toolkit.Uwp.UI.Controls.Lottie.Model.Content;
using Microsoft.Toolkit.Uwp.UI.Controls.Lottie.Utils;
using Newtonsoft.Json;
using Windows.UI;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Lottie.Parser
{
    internal class GradientColorParser : IValueParser<GradientColor>
    {
        /// <summary>
        /// The number of colors if it exists in the json or -1 if it doesn't (legacy bodymovin)
        /// </summary>
        private int _colorPoints;

        public GradientColorParser(int colorPoints)
        {
            _colorPoints = colorPoints;
        }

        // Both the color stops and opacity stops are in the same array.
        // There are <see cref="_colorPoints"/> colors sequentially as:
        // [
        //     ...,
        //     position,
        //     red,
        //     green,
        //     blue,
        //     ...
        // ]
        //
        // The remainder of the array is the opacity stops sequentially as:
        // [
        //     ...,
        //     position,
        //     opacity,
        //     ...
        // ]
        public GradientColor Parse(JsonReader reader, float scale)
        {
            List<float> array = new List<float>();

            // The array was started by Keyframe because it thought that this may be an array of keyframes
            // but peek returned a number so it considered it a static array of numbers.
            bool isArray = reader.Peek() == JsonToken.StartArray;
            if (isArray)
            {
                reader.BeginArray();
            }

            while (reader.HasNext())
            {
                array.Add(reader.NextDouble());
            }

            if (isArray)
            {
                reader.EndArray();
            }

            if (_colorPoints == -1)
            {
                _colorPoints = array.Count / 4;
            }

            float[] positions = new float[_colorPoints];
            Color[] colors = new Color[_colorPoints];

            byte r = 0;
            byte g = 0;
            if (array.Count != _colorPoints * 4)
            {
                Debug.WriteLine(
                    $"Unexpected gradient length: {array.Count}. Expected {_colorPoints * 4}. This may affect the appearance of the gradient. Make sure to save your After Effects file before exporting an animation with gradients.", LottieLog.Tag);
            }

            for (int i = 0; i < _colorPoints * 4; i++)
            {
                int colorIndex = i / 4;
                double value = array[i];
                switch (i % 4)
                {
                    case 0:
                        // position
                        positions[colorIndex] = (float)value;
                        break;
                    case 1:
                        r = (byte)(value * 255);
                        break;
                    case 2:
                        g = (byte)(value * 255);
                        break;
                    case 3:
                        byte b = (byte)(value * 255);
                        colors[colorIndex] = Color.FromArgb(255, r, g, b);
                        break;
                }
            }

            GradientColor gradientColor = new GradientColor(positions, colors);
            AddOpacityStopsToGradientIfNeeded(gradientColor, array);
            return gradientColor;
        }

        // This cheats a little bit.
        // Opacity stops can be at arbitrary intervals independent of color stops.
        // This uses the existing color stops and modifies the opacity at each existing color stop
        // based on what the opacity would be.
        //
        // This should be a good approximation is nearly all cases.However, if there are many more
        // opacity stops than color stops, information will be lost.
        private void AddOpacityStopsToGradientIfNeeded(GradientColor gradientColor, List<float> array)
        {
            int startIndex = _colorPoints * 4;
            if (array.Count <= startIndex)
            {
                return;
            }

            int opacityStops = (array.Count - startIndex) / 2;
            double[] positions = new double[opacityStops];
            double[] opacities = new double[opacityStops];

            for (int i = startIndex, j = 0; i < array.Count; i++)
            {
                if (i % 2 == 0)
                {
                    positions[j] = array[i];
                }
                else
                {
                    opacities[j] = array[i];
                    j++;
                }
            }

            for (int i = 0; i < gradientColor.Size; i++)
            {
                Color color = gradientColor.Colors[i];
                color = Color.FromArgb(
                    GetOpacityAtPosition(gradientColor.Positions[i], positions, opacities),
                    color.R,
                    color.G,
                    color.B);
                gradientColor.Colors[i] = color;
            }
        }

        private byte GetOpacityAtPosition(double position, double[] positions, double[] opacities)
        {
            for (int i = 1; i < positions.Length; i++)
            {
                double lastPosition = positions[i - 1];
                double thisPosition = positions[i];
                if (positions[i] >= position)
                {
                    double progress = (position - lastPosition) / (thisPosition - lastPosition);
                    return (byte)(255 * MiscUtils.Lerp(opacities[i - 1], opacities[i], progress));
                }
            }

            return (byte)(255 * opacities[opacities.Length - 1]);
        }
    }
}
