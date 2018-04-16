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

namespace Microsoft.Toolkit.Uwp.UI.Controls.Lottie.Model
{
    internal class CubicCurveData
    {
        private Vector2 _controlPoint1;
        private Vector2 _controlPoint2;
        private Vector2 _vertex;

        internal CubicCurveData()
        {
            _controlPoint1 = default(Vector2);
            _controlPoint2 = default(Vector2);
            _vertex = default(Vector2);
        }

        internal CubicCurveData(Vector2 controlPoint1, Vector2 controlPoint2, Vector2 vertex)
        {
            _controlPoint1 = controlPoint1;
            _controlPoint2 = controlPoint2;
            _vertex = vertex;
        }

        internal virtual void SetControlPoint1(float x, float y)
        {
            _controlPoint1.X = x;
            _controlPoint1.Y = y;
        }

        internal virtual Vector2 ControlPoint1 => _controlPoint1;

        internal virtual void SetControlPoint2(float x, float y)
        {
            _controlPoint2.X = x;
            _controlPoint2.Y = y;
        }

        internal virtual Vector2 ControlPoint2 => _controlPoint2;

        internal virtual void SetVertex(float x, float y)
        {
            _vertex.X = x;
            _vertex.Y = y;
        }

        internal virtual Vector2 Vertex => _vertex;
    }
}