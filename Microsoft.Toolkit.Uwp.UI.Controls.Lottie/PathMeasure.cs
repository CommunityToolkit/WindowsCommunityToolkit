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
using System.Numerics;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Lottie
{
    internal class PathMeasure : IDisposable
    {
        private CachedPathIteratorFactory _originalPathIterator;
        private Path _path;
        private CanvasGeometry _geometry;

        public PathMeasure(Path path)
        {
            _originalPathIterator = new CachedPathIteratorFactory(new FullPathIterator(path));
            _path = path;
            _geometry = _path.GetGeometry(CanvasDevice.GetSharedDevice());
            Length = _geometry.ComputePathLength();
        }

        public void SetPath(Path path)
        {
            _originalPathIterator = new CachedPathIteratorFactory(new FullPathIterator(path));
            _path = path;
            _geometry?.Dispose();
            _geometry = _path.GetGeometry(CanvasDevice.GetSharedDevice());
            Length = _geometry.ComputePathLength();
        }

        public float Length { get; private set; }

        public Vector2 GetPosTan(float distance)
        {
            if (distance < 0)
            {
                distance = 0;
            }

            var length = Length;
            if (distance > length)
            {
                distance = length;
            }

            return _geometry.ComputePointOnPath(distance);
        }

        public bool GetSegment(float startD, float stopD, ref Path dst, bool startWithMoveTo)
        {
            var length = Length;

            if (startD < 0)
            {
                startD = 0;
            }

            if (stopD > length)
            {
                stopD = length;
            }

            if (startD >= stopD)
            {
                return false;
            }

            var iterator = _originalPathIterator.Iterator();

            var accLength = startD;
            var isZeroLength = true;

            var points = new float[6];

            iterator.JumpToSegment(accLength);

            while (!iterator.Done && stopD - accLength > 0.1f)
            {
                var type = iterator.CurrentSegment(points, stopD - accLength);

                if (accLength - iterator.CurrentSegmentLength <= stopD)
                {
                    if (startWithMoveTo)
                    {
                        startWithMoveTo = false;

                        if (type != PathIterator.ContourType.MoveTo == false)
                        {
                            var lastPoint = new float[2];
                            iterator.GetCurrentSegmentEnd(lastPoint);
                            dst.MoveTo(lastPoint[0], lastPoint[1]);
                        }
                    }

                    isZeroLength = isZeroLength && iterator.CurrentSegmentLength > 0;
                    switch (type)
                    {
                        case PathIterator.ContourType.MoveTo:
                            dst.MoveTo(points[0], points[1]);
                            break;
                        case PathIterator.ContourType.Line:
                            dst.LineTo(points[0], points[1]);
                            break;
                        case PathIterator.ContourType.Close:
                            dst.Close();
                            break;
                        case PathIterator.ContourType.Bezier:
                        case PathIterator.ContourType.Arc:
                            dst.CubicTo(points[0], points[1], points[2], points[3], points[4], points[5]);
                            break;
                    }
                }

                accLength += iterator.CurrentSegmentLength;
                iterator.Next();
            }

            return !isZeroLength;
        }

        internal void Dispose(bool disposing)
        {
            if (_geometry != null)
            {
                try
                {
                    if (disposing)
                    {
                        _geometry.Dispose();
                    }
                    else
                    {
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(_geometry);
                    }
                }
                catch (Exception)
                {
                    // Ignore, but should not happen
                }
                finally
                {
                    _geometry = null;
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~PathMeasure()
        {
            Dispose(false);
        }
    }
}