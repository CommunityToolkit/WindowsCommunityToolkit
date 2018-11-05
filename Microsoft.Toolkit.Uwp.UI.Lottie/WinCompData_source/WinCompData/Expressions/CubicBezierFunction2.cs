// Copyright(c) Microsoft Corporation.All rights reserved.
// Licensed under the MIT License.

using System;
using SnVector2 = System.Numerics.Vector2;

namespace Microsoft.Toolkit.Uwp.UI.Lottie.WinCompData.Expressions
{
    /// <summary>
    /// A cubic bezier function with type Vector2.
    /// </summary>
#if !WINDOWS_UWP
    public
#endif
    sealed class CubicBezierFunction2 : Expression
    {
        readonly SnVector2 _p0;
        readonly SnVector2 _p1;
        readonly SnVector2 _p2;
        readonly SnVector2 _p3;
        readonly Expression _t;

        public static CubicBezierFunction2 Create(SnVector2 controlPoint0, SnVector2 controlPoint1, SnVector2 controlPoint2, SnVector2 controlPoint3, Expression t)
        {
            return new CubicBezierFunction2(controlPoint0, controlPoint1, controlPoint2, controlPoint3, t);
        }

        CubicBezierFunction2(SnVector2 controlPoint0, SnVector2 controlPoint1, SnVector2 controlPoint2, SnVector2 controlPoint3, Expression t)
        {
            _p0 = controlPoint0;
            _p1 = controlPoint1;
            _p2 = controlPoint2;
            _p3 = controlPoint3;
            _t = t;
        }

        /// <summary>
        /// A <see cref="CubicBezierFunction2"/> that describes a line from 0 to 0.
        /// </summary>
        public static CubicBezierFunction2 Zero { get; } = Create(new SnVector2(0,0), new SnVector2(0, 0), new SnVector2(0, 0), new SnVector2(0, 0), Scalar(0));

        /// <summary>
        /// True iff the cubic bezier is equivalent to a line drawn from point 0 to point 3.
        /// </summary>
        public bool IsEquivalentToLinear
        {
            get
            {
                if (!IsColinear)
                {
                    return false;
                }

                // The points are on the same line. The cubic bezier is a line if
                // p1 and p2 are between p0..p3.
                if (!IsBetween(_p0.X, _p1.X, _p2.X, _p3.X))
                {
                    return false;
                }

                if (!IsBetween(_p0.Y, _p1.Y, _p2.Y, _p3.Y))
                {
                    return false;
                }

                return true;
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
        /// True iff all of the control points are on the same line.
        /// </summary>
        bool IsColinear
        {
            get
            {
                var p01X = _p0.X - _p1.X;
                var p01Y = _p0.Y - _p1.Y;

                var p02X = _p0.X - _p2.X;
                var p02Y = _p0.Y - _p2.Y;

                var p03X = _p0.X - _p3.X;
                var p03Y = _p0.Y - _p3.Y;

                if (p01Y == 0 || p02Y == 0 || p03Y == 0)
                {
                    // Can't divide by Y because it's 0 in at least one case. (i.e. horizontal line)
                    if (p01X == 0 || p02X == 0 || p03X == 0)
                    {
                        // Can't divide by X because it's 0 in at least one case (i.e. vertical line)
                        // The points can only be colinear if they're all equal.
                        return p01X == p02X && p02X == p03X && p03X == p01X;
                    }
                    else
                    {
                        return (p01Y / p01X) == (p02Y / p02X) &&
                               (p01Y / p01X) == (p03Y / p03X);
                    }
                }
                else
                {
                    return (p01X / p01Y) == (p02X / p02Y) &&
                           (p01X / p01Y) == (p03X / p03Y);
                }
            }
        }

        // (1-t)^3P0 + 3(1-t)^2tP1 + 3(1-t)t^2P2 + t^3P3
        protected override Expression Simplify()
        {
            var OneMinusT = Subtract(Scalar(1), _t);

            // (1-t)^3P0
            var p0Part = Multiply(Cubed(OneMinusT), Constant(_p0));

            // (1-t)^2t3P1
            var p1Part = Multiply(Scalar(3), Squared(OneMinusT), _t, Constant(_p1));

            // (1-t)t^23P2
            var p2Part = Multiply(Scalar(3), OneMinusT, Squared(_t), Constant(_p2));

            // t^3P3
            var p3Part = Multiply(Cubed(_t), Constant(_p3));

            return Sum(p0Part, p1Part, p2Part, p3Part).Simplified;
        }

        protected override string CreateExpressionString() => Simplified.ToString();

        public override ExpressionType InferredType => new ExpressionType(TypeConstraint.Vector2);
    }
}
