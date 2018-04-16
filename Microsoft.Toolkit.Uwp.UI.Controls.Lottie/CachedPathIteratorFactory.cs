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

namespace Microsoft.Toolkit.Uwp.UI.Controls.Lottie
{
    /// <summary>
    /// Class that returns iterators for a given path. These iterators are lightweight and can be reused
    /// multiple times to iterate over the path.
    /// </summary>
    internal class CachedPathIteratorFactory
    {
        private readonly PathIterator.ContourType[] _types;
        private readonly float[][] _coordinates;
        private readonly float[] _segmentsLength;

        internal CachedPathIteratorFactory(PathIterator iterator)
        {
            var typesArray = new List<PathIterator.ContourType>();
            var pointsArray = new List<float[]>();
            var points = new float[6];
            while (!iterator.Done)
            {
                var type = iterator.CurrentSegment(points);
                var nPoints = GetNumberOfPoints(type) * 2; // 2 coordinates per point

                typesArray.Add(type);
                var itemPoints = new float[nPoints];
                Array.Copy(points, 0, itemPoints, 0, nPoints);
                pointsArray.Add(itemPoints);
                iterator.Next();
            }

            _types = new PathIterator.ContourType[typesArray.Count];
            _coordinates = new float[_types.Length][];
            for (var i = 0; i < typesArray.Count; i++)
            {
                _types[i] = typesArray[i];
                _coordinates[i] = pointsArray[i];
            }

            // Do measurement
            _segmentsLength = new float[_types.Length];

            // Curves that we can reuse to estimate segments length
            float lastX = 0;
            float lastY = 0;
            for (var i = 0; i < _types.Length; i++)
            {
                switch (_types[i])
                {
                    case PathIterator.ContourType.Bezier:
                        _segmentsLength[i] = (float)Path.BezierContour.BezLength(lastX, lastY, _coordinates[i][0], _coordinates[i][1], _coordinates[i][2], _coordinates[i][3], lastX = _coordinates[i][4], lastY = _coordinates[i][5]);
                        break;
                    case PathIterator.ContourType.Arc:
                        _segmentsLength[i] = (float)Path.BezierContour.BezLength(lastX, lastY, lastX + (2 * (_coordinates[i][0] - lastX) / 3), lastY + (2 * (_coordinates[i][1] - lastY) / 3), _coordinates[i][2] + (2 * (_coordinates[i][0] - _coordinates[i][2]) / 3), _coordinates[i][3] + (2 * (_coordinates[i][1] - _coordinates[i][3]) / 3), lastX = _coordinates[i][2], lastY = _coordinates[i][3]);
                        break;
                    case PathIterator.ContourType.Close:
                        _segmentsLength[i] = Vector2.Distance(new Vector2(lastX, lastY), new Vector2(lastX = _coordinates[0][0], lastY = _coordinates[0][1]));
                        _coordinates[i] = new float[2];

                        // We convert a CloseContour segment to a LineContour so we do not have to worry
                        // about this special case in the rest of the code.
                        _types[i] = PathIterator.ContourType.Line;
                        _coordinates[i][0] = _coordinates[0][0];
                        _coordinates[i][1] = _coordinates[0][1];
                        break;
                    case PathIterator.ContourType.MoveTo:
                        _segmentsLength[i] = 0;
                        lastX = _coordinates[i][0];
                        lastY = _coordinates[i][1];
                        break;
                    case PathIterator.ContourType.Line:
                        _segmentsLength[i] = Vector2.Distance(new Vector2(lastX, lastY), new Vector2(_coordinates[i][0], _coordinates[i][1]));
                        lastX = _coordinates[i][0];
                        lastY = _coordinates[i][1];
                        break;
                }
            }
        }

        private static void QuadCurveSegment(float[] coords, float t0, float t1)
        {
            // Calculate X and Y at 0.5 (We'll use this to reconstruct the control point later)
            var mt = t0 + ((t1 - t0) / 2);
            var mu = 1 - mt;
            var mx = (mu * mu * coords[0]) + (2 * mu * mt * coords[2]) + (mt * mt * coords[4]);
            var my = (mu * mu * coords[1]) + (2 * mu * mt * coords[3]) + (mt * mt * coords[5]);

            var u0 = 1 - t0;
            var u1 = 1 - t1;

            // coords at t0
            coords[0] = (coords[0] * u0 * u0) + (coords[2] * 2 * t0 * u0) + (coords[4] * t0 * t0);
            coords[1] = (coords[1] * u0 * u0) + (coords[3] * 2 * t0 * u0) + (coords[5] * t0 * t0);

            // coords at t1
            coords[4] = (coords[0] * u1 * u1) + (coords[2] * 2 * t1 * u1) + (coords[4] * t1 * t1);
            coords[5] = (coords[1] * u1 * u1) + (coords[3] * 2 * t1 * u1) + (coords[5] * t1 * t1);

            // estimated control point at t'=0.5
            coords[2] = (2 * mx) - (coords[0] / 2) - (coords[4] / 2);
            coords[3] = (2 * my) - (coords[1] / 2) - (coords[5] / 2);
        }

        private static void CubicCurveSegment(float[] coords, float t0, float t1)
        {
            // http://stackoverflow.com/questions/11703283/cubic-bezier-curve-segment
            var u0 = 1 - t0;
            var u1 = 1 - t1;

            // Calculate the points at t0 and t1 for the quadratic curves formed by (P0, P1, P2) and
            // (P1, P2, P3)
            var qxa = (coords[0] * u0 * u0) + (coords[2] * 2 * t0 * u0) + (coords[4] * t0 * t0);
            var qxb = (coords[0] * u1 * u1) + (coords[2] * 2 * t1 * u1) + (coords[4] * t1 * t1);
            var qxc = (coords[2] * u0 * u0) + (coords[4] * 2 * t0 * u0) + (coords[6] * t0 * t0);
            var qxd = (coords[2] * u1 * u1) + (coords[4] * 2 * t1 * u1) + (coords[6] * t1 * t1);

            var qya = (coords[1] * u0 * u0) + (coords[3] * 2 * t0 * u0) + (coords[5] * t0 * t0);
            var qyb = (coords[1] * u1 * u1) + (coords[3] * 2 * t1 * u1) + (coords[5] * t1 * t1);
            var qyc = (coords[3] * u0 * u0) + (coords[5] * 2 * t0 * u0) + (coords[7] * t0 * t0);
            var qyd = (coords[3] * u1 * u1) + (coords[5] * 2 * t1 * u1) + (coords[7] * t1 * t1);

            // Linear interpolation
            coords[0] = (qxa * u0) + (qxc * t0);
            coords[1] = (qya * u0) + (qyc * t0);

            coords[2] = (qxa * u1) + (qxc * t1);
            coords[3] = (qya * u1) + (qyc * t1);

            coords[4] = (qxb * u0) + (qxd * t0);
            coords[5] = (qyb * u0) + (qyd * t0);

            coords[6] = (qxb * u1) + (qxd * t1);
            coords[7] = (qyb * u1) + (qyd * t1);
        }

        /// <summary>
        /// Returns the end point of a given segment
        /// </summary>
        /// <param name="type"> the segment type </param>
        /// <param name="coords"> the segment coordinates array </param>
        /// <param name="point"> the return array where the point will be stored </param>
        private static void GetShapeEndPoint(PathIterator.ContourType type, float[] coords, float[] point)
        {
            // start index of the end point for the segment type
            var pointIndex = (GetNumberOfPoints(type) - 1) * 2;
            point[0] = coords[pointIndex];
            point[1] = coords[pointIndex + 1];
        }

        /// <summary>
        /// Returns the number of points stored in a coordinates array for the given segment type.
        /// </summary>
        /// <returns>Returns the number of points for a given ContourType</returns>
        private static int GetNumberOfPoints(PathIterator.ContourType segmentType)
        {
            switch (segmentType)
            {
                case PathIterator.ContourType.Arc:
                    return 2;
                case PathIterator.ContourType.Bezier:
                    return 3;
                case PathIterator.ContourType.Close:
                    return 0;
                default:
                    return 1;
            }
        }

        /// <summary>
        /// Returns the estimated position along a path of the given length.
        /// </summary>
        private void GetPointAtLength(PathIterator.ContourType type, float[] coords, float lastX, float lastY, float t, float[] point)
        {
            if (type == PathIterator.ContourType.Line)
            {
                point[0] = lastX + ((coords[0] - lastX) * t);
                point[1] = lastY + ((coords[1] - lastY) * t);

                // Return here, since we do not need a shape to estimate
                return;
            }

            var curve = new float[8];
            var lastPointIndex = (GetNumberOfPoints(type) - 1) * 2;

            Array.Copy(coords, 0, curve, 2, coords.Length);
            curve[0] = lastX;
            curve[1] = lastY;
            if (type == PathIterator.ContourType.Bezier)
            {
                CubicCurveSegment(curve, 0f, t);
            }
            else
            {
                QuadCurveSegment(curve, 0f, t);
            }

            point[0] = curve[2 + lastPointIndex];
            point[1] = curve[2 + lastPointIndex + 1];
        }

        public virtual CachedPathIterator Iterator()
        {
            return new CachedPathIterator(this);
        }

        /// <summary>
        /// Class that allows us to iterate over a path multiple times
        /// </summary>
        public class CachedPathIterator : PathIterator
        {
            private readonly CachedPathIteratorFactory _outerInstance;

            /// <summary>
            /// Stores the coordinates array of the current segment. The number of points stored depends
            /// on the segment type.
            /// </summary>
            /// <seealso cref="PathIterator"></seealso>
            private readonly float[] _currentCoords = new float[6];

            /// <summary>
            /// Point where the current segment started
            /// </summary>
            private readonly float[] _mLastPoint = new float[2];

            private int _nextIndex;

            /// <summary>
            /// Current segment type.
            /// </summary>
            /// <seealso cref="PathIterator"/>
            private ContourType _currentType;

            private float _currentSegmentLength;

            /// <summary>
            /// Current segment length offset. When asking for the length of the current segment, the
            /// length will be reduced by this amount. This is useful when we are only using portions of
            /// the segment.
            /// </summary>
            /// <seealso cref="JumpToSegment(float)"></seealso>
            private float _mOffsetLength;

            private bool _isIteratorDone;

            internal CachedPathIterator(CachedPathIteratorFactory outerInstance)
            {
                _outerInstance = outerInstance;
                Next();
            }

            public virtual float CurrentSegmentLength => _currentSegmentLength;

            public override bool Done => _isIteratorDone;

            public override bool Next()
            {
                if (_nextIndex >= _outerInstance._types.Length)
                {
                    _isIteratorDone = true;
                    return false;
                }

                if (_nextIndex >= 1)
                {
                    // We've already called next() once so there is a previous segment in this path.
                    // We want to get the coordinates where the path ends.
                    GetShapeEndPoint(_currentType, _currentCoords, _mLastPoint);
                }
                else
                {
                    // This is the first segment, no previous point so initialize to 0, 0
                    _mLastPoint[0] = _mLastPoint[1] = 0f;
                }

                _currentType = _outerInstance._types[_nextIndex];
                _currentSegmentLength = _outerInstance._segmentsLength[_nextIndex] - _mOffsetLength;

                if (_mOffsetLength > 0f && (_currentType == ContourType.Bezier || _currentType == ContourType.Arc))
                {
                    // We need to skip part of the start of the current segment (because
                    // mOffsetLength > 0)
                    var points = new float[8];

                    if (_nextIndex < 1)
                    {
                        points[0] = points[1] = 0f;
                    }
                    else
                    {
                        GetShapeEndPoint(_outerInstance._types[_nextIndex - 1], _outerInstance._coordinates[_nextIndex - 1], points);
                    }

                    Array.Copy(_outerInstance._coordinates[_nextIndex], 0, points, 2, _outerInstance._coordinates[_nextIndex].Length);
                    var t0 = (_outerInstance._segmentsLength[_nextIndex] - _currentSegmentLength) /
                             _outerInstance._segmentsLength[_nextIndex];
                    if (_currentType == ContourType.Bezier)
                    {
                        CubicCurveSegment(points, t0, 1f);
                    }
                    else
                    {
                        QuadCurveSegment(points, t0, 1f);
                    }

                    Array.Copy(points, 2, _currentCoords, 0, _outerInstance._coordinates[_nextIndex].Length);
                }
                else
                {
                    Array.Copy(_outerInstance._coordinates[_nextIndex], 0, _currentCoords, 0, _outerInstance._coordinates[_nextIndex].Length);
                }

                _mOffsetLength = 0f;
                _nextIndex++;
                return true;
            }

            public override ContourType CurrentSegment(float[] coords)
            {
                Array.Copy(_currentCoords, 0, coords, 0, GetNumberOfPoints(_currentType) * 2);
                return _currentType;
            }

            /// <summary>
            /// Returns the point where the current segment ends
            /// </summary>
            public virtual void GetCurrentSegmentEnd(float[] point)
            {
                point[0] = _mLastPoint[0];
                point[1] = _mLastPoint[1];
            }

            /// <summary>
            /// Restarts the iterator and jumps all the segments of this path up to the length value.
            /// </summary>
            public virtual void JumpToSegment(float length)
            {
                _isIteratorDone = false;
                if (length <= 0f)
                {
                    _nextIndex = 0;
                    return;
                }

                float accLength = 0;
                var lastPoint = new float[2];
                for (_nextIndex = 0; _nextIndex < _outerInstance._types.Length; _nextIndex++)
                {
                    var segmentLength = _outerInstance._segmentsLength[_nextIndex];
                    if (accLength + segmentLength >= length && _outerInstance._types[_nextIndex] != ContourType.MoveTo)
                    {
                        var estimatedPoint = new float[2];
                        _outerInstance.GetPointAtLength(_outerInstance._types[_nextIndex], _outerInstance._coordinates[_nextIndex], lastPoint[0], lastPoint[1], (length - accLength) / segmentLength, estimatedPoint);

                        // This segment makes us go further than length so we go back one step,
                        // set a moveto and offset the length of the next segment by the length
                        // of this segment that we've already used.
                        _currentType = ContourType.MoveTo;
                        _currentCoords[0] = estimatedPoint[0];
                        _currentCoords[1] = estimatedPoint[1];
                        _currentSegmentLength = 0;

                        // We need to offset next path length to account for the segment we've just
                        // skipped.
                        _mOffsetLength = length - accLength;
                        return;
                    }

                    accLength += segmentLength;
                    GetShapeEndPoint(_outerInstance._types[_nextIndex], _outerInstance._coordinates[_nextIndex], lastPoint);
                }
            }

            /// <summary>
            /// Returns the current segment up to certain length. If the current segment is shorter than
            /// length, then the whole segment is returned. The segment coordinates are copied into the
            /// coords array.
            /// </summary>
            /// <returns> the segment type </returns>
            public ContourType CurrentSegment(float[] coords, float length)
            {
                var type = CurrentSegment(coords);

                // If the length is greater than the current segment length, no need to find
                // the cut point. Same if this is a SEG_MOVETO.
                if (_currentSegmentLength <= length || type == ContourType.MoveTo)
                {
                    return type;
                }

                var t = length / CurrentSegmentLength;

                // We find at which offset the end point is located within the coords array and set
                // a new end point to cut the segment short
                switch (type)
                {
                    case ContourType.Bezier:
                    case ContourType.Arc:
                        var curve = new float[8];
                        curve[0] = _mLastPoint[0];
                        curve[1] = _mLastPoint[1];
                        Array.Copy(coords, 0, curve, 2, coords.Length);
                        if (type == ContourType.Bezier)
                        {
                            CubicCurveSegment(curve, 0f, t);
                        }
                        else
                        {
                            QuadCurveSegment(curve, 0f, t);
                        }

                        Array.Copy(curve, 2, coords, 0, coords.Length);
                        break;
                    default:
                        var point = new float[2];
                        _outerInstance.GetPointAtLength(type, coords, _mLastPoint[0], _mLastPoint[1], t, point);
                        coords[0] = point[0];
                        coords[1] = point[1];
                        break;
                }

                return type;
            }
        }
    }
}