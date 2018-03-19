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

namespace Microsoft.Toolkit.Uwp.UI.Controls.Lottie
{
    internal class PathInterpolator : IInterpolator
    {
        internal PathInterpolator(float controlX1, float controlY1, float controlX2, float controlY2)
        {
            InitCubic(controlX1, controlY1, controlX2, controlY2);
        }

        private static readonly float Precision = 0.002f;

        private float[] _mX; // x coordinates in the line

        private float[] _mY; // y coordinates in the line

        private void InitCubic(float x1, float y1, float x2, float y2)
        {
            Path path = new Path();
            path.MoveTo(0, 0);
            path.CubicTo(x1, y1, x2, y2, 1f, 1f);
            InitPath(path);
        }

        private void InitPath(Path path)
        {
            float[] pointComponents = path.Approximate(Precision);

            int numPoints = pointComponents.Length / 3;
            if (pointComponents[1] != 0 || pointComponents[2] != 0
                                        || pointComponents[pointComponents.Length - 2] != 1
                                        || pointComponents[pointComponents.Length - 1] != 1)
            {
                // throw new ArgumentException("The Path must start at (0,0) and end at (1,1)");
            }

            _mX = new float[numPoints];
            _mY = new float[numPoints];
            float prevX = 0;
            float prevFraction = 0;
            int componentIndex = 0;
            for (int i = 0; i < numPoints; i++)
            {
                float fraction = pointComponents[componentIndex++];
                float x = pointComponents[componentIndex++];
                float y = pointComponents[componentIndex++];
                if (fraction == prevFraction && x != prevX)
                {
                    throw new ArgumentException("The Path cannot have discontinuity in the X axis.");
                }

                if (x < prevX)
                {
                    // throw new ArgumentException("The Path cannot loop back on itself.");
                }

                _mX[i] = x;
                _mY[i] = y;
                prevX = x;
                prevFraction = fraction;
            }
        }

        public float GetInterpolation(float t)
        {
            if (t <= 0 || float.IsNaN(t))
            {
                return 0;
            }

            if (t >= 1)
            {
                return 1;
            }

            // Do a binary search for the correct x to interpolate between.
            int startIndex = 0;
            int endIndex = _mX.Length - 1;

            while (endIndex - startIndex > 1)
            {
                int midIndex = (startIndex + endIndex) / 2;
                if (t < _mX[midIndex])
                {
                    endIndex = midIndex;
                }
                else
                {
                    startIndex = midIndex;
                }
            }

            float xRange = _mX[endIndex] - _mX[startIndex];
            if (xRange == 0)
            {
                return _mY[startIndex];
            }

            float tInRange = t - _mX[startIndex];
            float fraction = tInRange / xRange;

            float startY = _mY[startIndex];
            float endY = _mY[endIndex];
            return startY + (fraction * (endY - startY));
        }
    }
}