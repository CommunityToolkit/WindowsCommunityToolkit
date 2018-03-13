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
using System.Linq;
using System.Numerics;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using Windows.Foundation;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Lottie
{
    internal class Path
    {
        public interface IContour
        {
            void Transform(Matrix3X3 matrix);

            IContour Copy();

            float[] Points { get; }

            PathIterator.ContourType Type { get; }

            void AddPathSegment(CanvasPathBuilder canvasPathBuilder, ref bool closed);

            void Offset(float dx, float dy);
        }

        private class ArcContour : IContour
        {
            private readonly float _startAngle;
            private readonly float _sweepAngle;

            private Vector2 _startPoint;
            private Vector2 _endPoint;
            private Rect _rect;
            private float _a;
            private float _b;

            public ArcContour(Vector2 startPoint, Rect rect, float startAngle, float sweepAngle)
            {
                _startPoint = startPoint;
                _rect = rect;
                _a = (float)(rect.Width / 2);
                _b = (float)(rect.Height / 2);
                _startAngle = startAngle;
                _sweepAngle = sweepAngle;

                _endPoint = GetPointAtAngle(startAngle + sweepAngle);
            }

            public void Transform(Matrix3X3 matrix)
            {
                _startPoint = matrix.Transform(_startPoint);
                _endPoint = matrix.Transform(_endPoint);

                var p1 = new Vector2((float)_rect.Left, (float)_rect.Top);
                var p2 = new Vector2((float)_rect.Right, (float)_rect.Top);
                var p3 = new Vector2((float)_rect.Left, (float)_rect.Bottom);
                var p4 = new Vector2((float)_rect.Right, (float)_rect.Bottom);

                p1 = matrix.Transform(p1);
                p2 = matrix.Transform(p2);
                p3 = matrix.Transform(p3);
                p4 = matrix.Transform(p4);

                _a = (p2 - p1).Length() / 2;
                _b = (p4 - p3).Length() / 2;
            }

            public IContour Copy()
            {
                return new ArcContour(_startPoint, _rect, _startAngle, _sweepAngle);
            }

            public float[] Points => new[] { _startPoint.X, _startPoint.Y, _endPoint.X, _endPoint.Y };

            public PathIterator.ContourType Type => PathIterator.ContourType.Arc;

            public void AddPathSegment(CanvasPathBuilder canvasPathBuilder, ref bool closed)
            {
                canvasPathBuilder.AddArc(_endPoint, _a, _b, (float)MathExt.ToRadians(_sweepAngle), CanvasSweepDirection.Clockwise, CanvasArcSize.Small);

                closed = false;
            }

            public void Offset(float dx, float dy)
            {
                _startPoint.X += dx;
                _startPoint.Y += dy;
                _endPoint.X += dx;
                _endPoint.Y += dy;
            }

            private Vector2 GetPointAtAngle(float t)
            {
                var u = Math.Tan(MathExt.ToRadians(t) / 2);

                var u2 = u * u;

                var x = _a * (1 - u2) / (u2 + 1);
                var y = 2 * _b * u / (u2 + 1);

                return new Vector2((float)(_rect.Left + _a + x), (float)(_rect.Top + _b + y));
            }
        }

        internal class BezierContour : IContour
        {
            private Vector2 _control1;
            private Vector2 _control2;
            private Vector2 _vertex;

            public BezierContour(Vector2 control1, Vector2 control2, Vector2 vertex)
            {
                _control1 = control1;
                _control2 = control2;
                _vertex = vertex;
            }

            public void Transform(Matrix3X3 matrix)
            {
                _control1 = matrix.Transform(_control1);
                _control2 = matrix.Transform(_control2);
                _vertex = matrix.Transform(_vertex);
            }

            public IContour Copy()
            {
                return new BezierContour(_control1, _control2, _vertex);
            }

            internal static double BezLength(float c0X, float c0Y, float c1X, float c1Y, float c2X, float c2Y, float c3X, float c3Y)
            {
                const double steps = 1000d; // TODO: improve

                var length = 0d;
                float prevPtX = 0;
                float prevPtY = 0;

                for (var i = 0d; i < steps; i++)
                {
                    var pt = GetPointAtT(c0X, c0Y, c1X, c1Y, c2X, c2Y, c3X, c3Y, i / steps);

                    if (i > 0)
                    {
                        var x = pt.X - prevPtX;
                        var y = pt.Y - prevPtY;
                        length = length + Math.Sqrt((x * x) + (y * y));
                    }

                    prevPtX = pt.X;
                    prevPtY = pt.Y;
                }

                return length;
            }

            private static Vector2 GetPointAtT(float c0X, float c0Y, float c1X, float c1Y, float c2X, float c2Y, float c3X, float c3Y, double t)
            {
                var t1 = 1d - t;

                if (t1 < 5e-6)
                {
                    t = 1.0;
                    t1 = 0.0;
                }

                var t13 = t1 * t1 * t1;
                var t13A = 3 * t * (t1 * t1);
                var t13B = 3 * t * t * t1;
                var t13C = t * t * t;

                var ptX = (float)((c0X * t13) + (t13A * c1X) + (t13B * c2X) + (t13C * c3X));
                var ptY = (float)((c0Y * t13) + (t13A * c1Y) + (t13B * c2Y) + (t13C * c3Y));

                return new Vector2(ptX, ptY);
            }

            public float[] Points => new[] { _control1.X, _control1.Y, _control2.X, _control2.Y, _vertex.X, _vertex.Y };

            public PathIterator.ContourType Type => PathIterator.ContourType.Bezier;

            public void AddPathSegment(CanvasPathBuilder canvasPathBuilder, ref bool closed)
            {
                canvasPathBuilder.AddCubicBezier(_control1, _control2, _vertex);

                closed = false;
            }

            public void Offset(float dx, float dy)
            {
                _control1.X += dx;
                _control1.Y += dy;
                _control2.X += dx;
                _control2.Y += dy;
                _vertex.X += dx;
                _vertex.Y += dy;
            }
        }

        private class LineContour : IContour
        {
            private readonly float[] _points = new float[2];

            public LineContour(float x, float y)
            {
                _points[0] = x;
                _points[1] = y;
            }

            public void Transform(Matrix3X3 matrix)
            {
                var p = new Vector2(_points[0], _points[1]);

                p = matrix.Transform(p);

                _points[0] = p.X;
                _points[1] = p.Y;
            }

            public IContour Copy()
            {
                return new LineContour(_points[0], _points[1]);
            }

            public float[] Points => _points;

            public PathIterator.ContourType Type => PathIterator.ContourType.Line;

            public void AddPathSegment(CanvasPathBuilder canvasPathBuilder, ref bool closed)
            {
                canvasPathBuilder.AddLine(_points[0], _points[1]);

                closed = false;
            }

            public void Offset(float dx, float dy)
            {
                _points[0] += dx;
                _points[1] += dy;
            }
        }

        private class MoveToContour : IContour
        {
            private readonly float[] _points = new float[2];

            public MoveToContour(float x, float y)
            {
                _points[0] = x;
                _points[1] = y;
            }

            public float[] Points => _points;

            public PathIterator.ContourType Type => PathIterator.ContourType.MoveTo;

            public IContour Copy()
            {
                return new MoveToContour(_points[0], _points[1]);
            }

            public void AddPathSegment(CanvasPathBuilder canvasPathBuilder, ref bool closed)
            {
                if (!closed)
                {
                    canvasPathBuilder.EndFigure(CanvasFigureLoop.Open);
                }
                else
                {
                    closed = false;
                }

                canvasPathBuilder.BeginFigure(_points[0], _points[1]);
            }

            public void Offset(float dx, float dy)
            {
                _points[0] += dx;
                _points[1] += dy;
            }

            public void Transform(Matrix3X3 matrix)
            {
                var p = new Vector2(_points[0], _points[1]);

                p = matrix.Transform(p);

                _points[0] = p.X;
                _points[1] = p.Y;
            }
        }

        private class CloseContour : IContour
        {
            public float[] Points => new float[0];

            public PathIterator.ContourType Type => PathIterator.ContourType.Close;

            public IContour Copy()
            {
                return new CloseContour();
            }

            public void AddPathSegment(CanvasPathBuilder canvasPathBuilder, ref bool closed)
            {
                if (!closed)
                {
                    canvasPathBuilder.EndFigure(CanvasFigureLoop.Closed);
                    closed = true;
                }
            }

            public void Offset(float dx, float dy)
            {
            }

            public void Transform(Matrix3X3 matrix)
            {
            }
        }

        public PathFillType FillType { get; set; }

        public List<IContour> Contours { get; }

        internal Path()
        {
            Contours = new List<IContour>();
            FillType = PathFillType.Winding;
        }

        public void Set(Path path)
        {
            Contours.Clear();
            Contours.AddRange(path.Contours.Select(p => p.Copy()));
            FillType = path.FillType;
        }

        public void Transform(Matrix3X3 matrix)
        {
            for (var j = 0; j < Contours.Count; j++)
            {
                Contours[j].Transform(matrix);
            }
        }

        public CanvasGeometry GetGeometry(CanvasDevice device)
        {
            var fill = FillType == PathFillType.Winding
                ? CanvasFilledRegionDetermination.Winding
                : CanvasFilledRegionDetermination.Alternate;

            // FillRule = path.FillType == PathFillType.EvenOdd ? FillRule.EvenOdd : FillRule.Nonzero,
            using (var canvasPathBuilder = new CanvasPathBuilder(device))
            {
                canvasPathBuilder.SetFilledRegionDetermination(fill);

                var closed = true;

                for (var i = 0; i < Contours.Count; i++)
                {
                    Contours[i].AddPathSegment(canvasPathBuilder, ref closed);
                }

                if (!closed)
                {
                    canvasPathBuilder.EndFigure(CanvasFigureLoop.Open);
                }

                return CanvasGeometry.CreatePath(canvasPathBuilder);
            }
        }

        public void ComputeBounds(out Rect rect)
        {
            if (Contours.Count == 0)
            {
                RectExt.Set(ref rect, 0, 0, 0, 0);
                return;
            }

            using (var geometry = GetGeometry(CanvasDevice.GetSharedDevice()))
            {
                rect = geometry.ComputeBounds();
            }
        }

        public void AddPath(Path path, Matrix3X3 matrix)
        {
            var pathCopy = new Path();
            pathCopy.Set(path);
            pathCopy.Transform(matrix);
            Contours.AddRange(pathCopy.Contours);
        }

        public void AddPath(Path path)
        {
            Contours.AddRange(path.Contours.Select(p => p.Copy()).ToList());
        }

        public void Reset()
        {
            Contours.Clear();
        }

        public void MoveTo(float x, float y)
        {
            Contours.Add(new MoveToContour(x, y));
        }

        public void CubicTo(float x1, float y1, float x2, float y2, float x3, float y3)
        {
            var bezier = new BezierContour(
                new Vector2(x1, y1),
                new Vector2(x2, y2),
                new Vector2(x3, y3));
            Contours.Add(bezier);
        }

        public void LineTo(float x, float y)
        {
            var newLine = new LineContour(x, y);
            Contours.Add(newLine);
        }

        public void Offset(float dx, float dy)
        {
            for (var i = 0; i < Contours.Count; i++)
            {
                Contours[i].Offset(dx, dy);
            }
        }

        public void Close()
        {
            Contours.Add(new CloseContour());
        }

        /*
         Set this path to the result of applying the Op to the two specified paths. The resulting path will be constructed from non-overlapping contours. The curve order is reduced where possible so that cubics may be turned into quadratics, and quadratics maybe turned into lines.
          Path1: The first operand (for difference, the minuend)
          Path2: The second operand (for difference, the subtrahend)
        */
        public void Op(Path path1, Path path2, CanvasGeometryCombine op)
        {
            // TODO
        }

        public void ArcTo(float x, float y, Rect rect, float startAngle, float sweepAngle)
        {
            var newArc = new ArcContour(new Vector2(x, y), rect, startAngle, sweepAngle);
            Contours.Add(newArc);
        }

        public float[] Approximate(float precision)
        {
            var pathIteratorFactory = new CachedPathIteratorFactory(new FullPathIterator(this));
            var pathIterator = pathIteratorFactory.Iterator();
            float[] points = new float[8];
            var segmentPoints = new List<Vector2>();
            var lengths = new List<float>();
            float errorSquared = precision * precision;
            while (!pathIterator.Done)
            {
                var type = pathIterator.CurrentSegment(points);
                switch (type)
                {
                    case PathIterator.ContourType.MoveTo:
                        AddMove(segmentPoints, lengths, points);
                        break;
                    case PathIterator.ContourType.Close:
                        AddLine(segmentPoints, lengths, points);
                        break;
                    case PathIterator.ContourType.Line:
                        AddLine(segmentPoints, lengths, points.Skip(2).ToArray());
                        break;
                    case PathIterator.ContourType.Arc:
                        AddBezier(points, QuadraticBezierCalculation, segmentPoints, lengths, errorSquared, false);
                        break;
                    case PathIterator.ContourType.Bezier:
                        AddBezier(points, CubicBezierCalculation, segmentPoints, lengths, errorSquared, true);
                        break;
                }

                pathIterator.Next();
            }

            if (!segmentPoints.Any())
            {
                int numVerbs = Contours.Count;
                if (numVerbs == 1)
                {
                    AddMove(segmentPoints, lengths, Contours[0].Points);
                }
                else
                {
                    // Invalid or empty path. Fall back to point(0,0)
                    AddMove(segmentPoints, lengths, new[] { 0.0f, 0.0f });
                }
            }

            float totalLength = lengths.Last();
            if (totalLength == 0)
            {
                // Lone Move instructions should still be able to animate at the same value.
                segmentPoints.Add(segmentPoints.Last());
                lengths.Add(1);
                totalLength = 1;
            }

            var numPoints = segmentPoints.Count;
            var approximationArraySize = numPoints * 3;

            var approximation = new float[approximationArraySize];

            int approximationIndex = 0;
            for (var i = 0; i < numPoints; i++)
            {
                var point = segmentPoints[i];
                approximation[approximationIndex++] = lengths[i] / totalLength;
                approximation[approximationIndex++] = point.X;
                approximation[approximationIndex++] = point.Y;
            }

            return approximation;
        }

        private static float QuadraticCoordinateCalculation(float t, float p0, float p1, float p2)
        {
            float oneMinusT = 1 - t;
            return (oneMinusT * ((oneMinusT * p0) + (t * p1))) + (t * ((oneMinusT * p1) + (t * p2)));
        }

        private static Vector2 QuadraticBezierCalculation(float t, float[] points)
        {
            float x = QuadraticCoordinateCalculation(t, points[0], points[2], points[4]);
            float y = QuadraticCoordinateCalculation(t, points[1], points[3], points[5]);
            return new Vector2(x, y);
        }

        private static float CubicCoordinateCalculation(float t, float p0, float p1, float p2, float p3)
        {
            float oneMinusT = 1 - t;
            float oneMinusTSquared = oneMinusT * oneMinusT;
            float oneMinusTCubed = oneMinusTSquared * oneMinusT;
            float tSquared = t * t;
            float tCubed = tSquared * t;
            return (oneMinusTCubed * p0) + (3 * oneMinusTSquared * t * p1)
                                         + (3 * oneMinusT * tSquared * p2) + (tCubed * p3);
        }

        private static Vector2 CubicBezierCalculation(float t, float[] points)
        {
            float x = CubicCoordinateCalculation(t, points[0], points[2], points[4], points[6]);
            float y = CubicCoordinateCalculation(t, points[1], points[3], points[5], points[7]);
            return new Vector2(x, y);
        }

        private static void AddMove(List<Vector2> segmentPoints, List<float> lengths, float[] point)
        {
            float length = 0;
            if (lengths.Any())
            {
                length = lengths.Last();
            }

            segmentPoints.Add(new Vector2(point[0], point[1]));
            lengths.Add(length);
        }

        private static void AddLine(List<Vector2> segmentPoints, List<float> lengths, float[] toPoint)
        {
            if (!segmentPoints.Any())
            {
                segmentPoints.Add(Vector2.Zero);
                lengths.Add(0);
            }
            else if (segmentPoints.Last().X == toPoint[0] && segmentPoints.Last().Y == toPoint[1])
            {
                return; // Empty line
            }

            var vector2 = new Vector2(toPoint[0], toPoint[1]);
            float length = lengths.Last() + (vector2 - segmentPoints.Last()).Length();
            segmentPoints.Add(vector2);
            lengths.Add(length);
        }

        private delegate Vector2 BezierCalculation(float t, float[] points);

        private static void AddBezier(float[] points, BezierCalculation bezierFunction, List<Vector2> segmentPoints, List<float> lengths, float errorSquared, bool doubleCheckDivision)
        {
            points[7] = points[5];
            points[6] = points[4];
            points[5] = points[3];
            points[4] = points[2];
            points[3] = points[1];
            points[2] = points[0];
            points[1] = 0;
            points[0] = 0;

            var tToPoint = new List<KeyValuePair<float, Vector2>>
            {
                new KeyValuePair<float, Vector2>(0, bezierFunction(0, points)),
                new KeyValuePair<float, Vector2>(1, bezierFunction(1, points))
            };

            for (int i = 0; i < tToPoint.Count - 1; i++)
            {
                bool needsSubdivision;
                do
                {
                    needsSubdivision = SubdividePoints(points, bezierFunction, tToPoint[i].Key, tToPoint[i].Value, tToPoint[i + 1].Key, tToPoint[i + 1].Value, out var midT, out var midPoint, errorSquared);
                    if (!needsSubdivision && doubleCheckDivision)
                    {
                        needsSubdivision = SubdividePoints(points, bezierFunction, tToPoint[i].Key, tToPoint[i].Value, midT, midPoint, out _, out _, errorSquared);
                        if (needsSubdivision)
                        {
                            // Found an inflection point. No need to double-check.
                            doubleCheckDivision = false;
                        }
                    }

                    if (needsSubdivision)
                    {
                        tToPoint.Insert(i + 1, new KeyValuePair<float, Vector2>(midT, midPoint));
                    }
                }
                while (needsSubdivision);
            }

            // Now that each division can use linear interpolation with less than the allowed error
            foreach (var iter in tToPoint)
            {
                AddLine(segmentPoints, lengths, new[] { iter.Value.X, iter.Value.Y });
            }
        }

        private static bool SubdividePoints(float[] points, BezierCalculation bezierFunction, float t0, Vector2 p0, float t1, Vector2 p1, out float midT, out Vector2 midPoint, float errorSquared)
        {
            midT = (t1 + t0) / 2;
            float midX = (p1.X + p0.X) / 2;
            float midY = (p1.Y + p0.Y) / 2;

            midPoint = bezierFunction(midT, points);
            float xError = midPoint.X - midX;
            float yError = midPoint.Y - midY;
            float midErrorSquared = (xError * xError) + (yError * yError);
            return midErrorSquared > errorSquared;
        }
    }

    internal enum PathFillType
    {
        EvenOdd,
        InverseWinding,
        Winding
    }
}