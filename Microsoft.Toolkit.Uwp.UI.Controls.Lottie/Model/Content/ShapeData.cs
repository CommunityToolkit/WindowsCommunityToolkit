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
using System.Collections.Generic;
using System.Numerics;
using Microsoft.Toolkit.Uwp.UI.Controls.Lottie.Utils;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Lottie.Model.Content
{
    internal class ShapeData
    {
        private readonly List<CubicCurveData> _curves = new List<CubicCurveData>();
        private Vector2 _initialPoint;
        private bool _closed;

        internal ShapeData(Vector2 initialPoint, bool closed, List<CubicCurveData> curves)
        {
            _initialPoint = initialPoint;
            _closed = closed;
            _curves.AddRange(curves);
        }

        internal ShapeData()
        {
        }

        private void SetInitialPoint(float x, float y)
        {
            if (_initialPoint == null)
            {
                _initialPoint = default(Vector2);
            }

            _initialPoint.X = x;
            _initialPoint.Y = y;
        }

        internal virtual Vector2 InitialPoint => _initialPoint;

        internal virtual bool Closed => _closed;

        internal virtual List<CubicCurveData> Curves => _curves;

        internal virtual void InterpolateBetween(ShapeData shapeData1, ShapeData shapeData2, float percentage)
        {
            if (_initialPoint == null)
            {
                _initialPoint = default(Vector2);
            }

            _closed = shapeData1.Closed || shapeData2.Closed;

            if (_curves.Count > 0 && _curves.Count != shapeData1.Curves.Count && _curves.Count != shapeData2.Curves.Count)
            {
                throw new InvalidOperationException("Curves must have the same number of control points. This: " + Curves.Count + "\tShape 1: " + shapeData1.Curves.Count + "\tShape 2: " + shapeData2.Curves.Count);
            }

            if (_curves.Count == 0)
            {
                for (var i = shapeData1.Curves.Count - 1; i >= 0; i--)
                {
                    _curves.Add(new CubicCurveData());
                }
            }

            var initialPoint1 = shapeData1.InitialPoint;
            var initialPoint2 = shapeData2.InitialPoint;

            SetInitialPoint(MiscUtils.Lerp(initialPoint1.X, initialPoint2.X, percentage), MiscUtils.Lerp(initialPoint1.Y, initialPoint2.Y, percentage));

            for (var i = _curves.Count - 1; i >= 0; i--)
            {
                var curve1 = shapeData1.Curves[i];
                var curve2 = shapeData2.Curves[i];

                var cp11 = curve1.ControlPoint1;
                var cp21 = curve1.ControlPoint2;
                var vertex1 = curve1.Vertex;

                var cp12 = curve2.ControlPoint1;
                var cp22 = curve2.ControlPoint2;
                var vertex2 = curve2.Vertex;

                _curves[i].SetControlPoint1(MiscUtils.Lerp(cp11.X, cp12.X, percentage), MiscUtils.Lerp(cp11.Y, cp12.Y, percentage));
                _curves[i].SetControlPoint2(MiscUtils.Lerp(cp21.X, cp22.X, percentage), MiscUtils.Lerp(cp21.Y, cp22.Y, percentage));
                _curves[i].SetVertex(MiscUtils.Lerp(vertex1.X, vertex2.X, percentage), MiscUtils.Lerp(vertex1.Y, vertex2.Y, percentage));
            }
        }

        public override string ToString()
        {
            return "ShapeData{" + "numCurves=" + _curves.Count + "closed=" + _closed + '}';
        }
    }
}