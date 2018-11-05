// Copyright(c) Microsoft Corporation.All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;

namespace Microsoft.Toolkit.Uwp.UI.Lottie.LottieData
{
    /// <summary>
    /// A segment defined as a cubic bezier curve from <see cref="ControlPoint0"/> to <see cref="ControlPoint3"/>;
    /// </summary>
#if !WINDOWS_UWP
    public
#endif
    sealed class BezierSegment : IEquatable<BezierSegment>
    {
        public BezierSegment(Vector2 cp0, Vector2 cp1, Vector2 cp2, Vector2 cp3)
        {
            ControlPoint0 = cp0;
            ControlPoint1 = cp1;
            ControlPoint2 = cp2;
            ControlPoint3 = cp3;
        }

        public Vector2 ControlPoint0 { get; }
        public Vector2 ControlPoint1 { get; }
        public Vector2 ControlPoint2 { get; }
        public Vector2 ControlPoint3 { get; }

        public bool Equals(BezierSegment other) => EqualityComparer.Equals(this, other);

        internal static IEqualityComparer<BezierSegment> EqualityComparer { get; } = new Comparer();

        public override string ToString()
        {
            if (ControlPoint3 - ControlPoint0 == Vector2.Zero)
            {
                return $"0-length line @ {ControlPoint0}";
            }
            else if (IsALine)
            {
                return $"Line from {ControlPoint0} to {ControlPoint3}";
            }
            else
            {
                return $"Curve from {ControlPoint0} to {ControlPoint3}";
            }
        }

        sealed class Comparer : IEqualityComparer<BezierSegment>
        {
            public bool Equals(BezierSegment x, BezierSegment y)
            {
                if (ReferenceEquals(x, y))
                {
                    return true;
                }

                if (x == null || y == null)
                {
                    return false;
                }

                return
                    x.ControlPoint0.Equals(y.ControlPoint0) &&
                    x.ControlPoint1.Equals(y.ControlPoint1) &&
                    x.ControlPoint2.Equals(y.ControlPoint2) &&
                    x.ControlPoint3.Equals(y.ControlPoint3);
            }

            public int GetHashCode(BezierSegment obj)
            {
                if (obj == null)
                {
                    return 0;
                }

                return
                    obj.ControlPoint0.GetHashCode() ^
                    obj.ControlPoint1.GetHashCode() ^
                    obj.ControlPoint2.GetHashCode() ^
                    obj.ControlPoint3.GetHashCode();
            }
        }

        // Returns true iff b and c and between a and d.
        static bool IsBetween(double a, double b, double c, double d)
        {
            var deltaAD = Math.Abs(a - d);

            if (Math.Abs(a - b) > deltaAD)
            {
                return false;
            }
            if (Math.Abs(d - b) > deltaAD)
            {
                return false;
            }
            if (Math.Abs(a - c) > deltaAD)
            {
                return false;
            }
            if (Math.Abs(d - c) > deltaAD)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Returns true iff the segment defines a line.
        /// </summary>
        public bool IsALine => IsALineWithinTolerance(0);

        /// <summary>
        /// Returns true if the segment describes a line. The tolerance value is necessary
        /// to deal with floating point imprecision.
        /// </summary>
        bool IsALineWithinTolerance(double tolerance)
        {
            if (!ArePointsColinear(tolerance, ControlPoint0, ControlPoint1, ControlPoint2, ControlPoint3))
            {
                return false;
            }

            // The points are on the same line. The cubic bezier is a line if
            // p1 and p2 are between p0..p3.
            if (!IsBetween(ControlPoint0.X, ControlPoint1.X, ControlPoint2.X, ControlPoint3.X))
            {
                return false;
            }
            if (!IsBetween(ControlPoint0.Y, ControlPoint1.Y, ControlPoint2.Y, ControlPoint3.Y))
            {
                return false;
            }

            return true;
        }

        // Returns the distance between 2 points.
        static double DistanceSquared(Vector2 a, Vector2 b)
        {
            var x = a.X - b.X;
            var y = a.Y - b.Y;

            return (x * x) + (y * y);
        }

        enum Segment
        {
            AB,
            AC,
            AD,
            BC,
            BD,
            CD,
        }

        public static bool ArePointsColinear(double tolerance, Vector2 a, Vector2 b, Vector2 c)
        {
            // Get the lengths for between each pair of points.
            var ab = DistanceSquared(a, b);
            var ac = DistanceSquared(a, c);
            var bc = DistanceSquared(b, c);

            // Find the longest segment.
            var longestSegmentId = Segment.AB;
            var longestSegment = ab;
            if (ac > longestSegment)
            {
                longestSegment = ac;
                longestSegmentId = Segment.AC;
            }
            if (bc > longestSegment)
            {
                longestSegment = bc;
                longestSegmentId = Segment.BC;
            }

            double inner0;
            double inner1;

            switch (longestSegmentId)
            {
                case Segment.AB:
                    inner0 = ac;
                    inner1 = bc;
                    break;
                case Segment.AC:
                    inner0 = ab;
                    inner1 = bc;
                    break;
                case Segment.BC:
                    inner0 = ab;
                    inner1 = ac;
                    break;
                default:
                    throw new InvalidOperationException();
            }

            var outer = Math.Sqrt(longestSegment);
            var sum = Math.Sqrt(inner0) + Math.Sqrt(inner1);

            // TODO - include tolerance.
            return sum == outer;
        }

        // Returns true if each of the given points is on the same line.
        public static bool ArePointsColinear(double tolerance, Vector2 a, Vector2 b, Vector2 c, Vector2 d)
        {
            // Get the lengths for between each pair of points.
            var ab = DistanceSquared(a, b);
            var ac = DistanceSquared(a, c);
            var ad = DistanceSquared(a, d);
            var bc = DistanceSquared(b, c);
            var bd = DistanceSquared(b, d);
            var cd = DistanceSquared(c, d);

            // Find the longest segment.
            var longestSegmentId = Segment.AB;
            var longestSegment = ab;
            if (ac > longestSegment)
            {
                longestSegment = ac;
                longestSegmentId = Segment.AC;
            }
            if (ad > longestSegment)
            {
                longestSegment = ad;
                longestSegmentId = Segment.AD;
            }
            if (bc > longestSegment)
            {
                longestSegment = bc;
                longestSegmentId = Segment.BC;
            }
            if (bd > longestSegment)
            {
                longestSegment = bd;
                longestSegmentId = Segment.BD;
            }
            if (cd > longestSegment)
            {
                longestSegment = cd;
                longestSegmentId = Segment.CD;
            }

            double inner00;
            double inner01;
            double inner10;
            double inner11;

            switch (longestSegmentId)
            {
                case Segment.AB:
                    inner00 = ac;
                    inner01 = bc;
                    inner10 = ad;
                    inner11 = bd;
                    break;
                case Segment.AC:
                    inner00 = ab;
                    inner01 = bc;
                    inner10 = ad;
                    inner11 = cd;
                    break;
                case Segment.AD:
                    inner00 = ab;
                    inner01 = bd;
                    inner10 = ac;
                    inner11 = cd;
                    break;
                case Segment.BC:
                    inner00 = ab;
                    inner01 = ac;
                    inner10 = bd;
                    inner11 = cd;
                    break;
                case Segment.BD:
                    inner00 = bc;
                    inner01 = cd;
                    inner10 = ab;
                    inner11 = ad;
                    break;
                case Segment.CD:
                    inner00 = ac;
                    inner01 = ad;
                    inner10 = bd;
                    inner11 = bc;
                    break;
                default:
                    throw new InvalidOperationException();
            }

            var outer = Math.Sqrt(longestSegment);
            var sum0 = Math.Sqrt(inner00) + Math.Sqrt(inner01);
            var sum1 = Math.Sqrt(inner10) + Math.Sqrt(inner11);

            // TODO - include tolerance.
            return sum0 == outer && sum1 == outer;
        }
    }
}
