// Copyright(c) Microsoft Corporation.All rights reserved.
// Licensed under the MIT License.

using Microsoft.Toolkit.Uwp.UI.Lottie.WinCompData.Expressions;
using System;
using System.Linq;
using Sn = System.Numerics;

namespace Microsoft.Toolkit.Uwp.UI.Lottie.LottieToWinComp
{
    sealed class ExpressionFactory : Expression
    {
        // The name used to bind to the property set that contains the Progress property.
        const string c_rootName = "_";
        static readonly Expression s_myTStart = Scalar("my.TStart");
        static readonly Expression s_myTEnd = Scalar("my.TEnd");

        // An expression that refers to the name of the root property set and the Progress property on it.
        internal static readonly Expression RootProgress = Scalar($"{c_rootName}.{LottieToWinCompTranslator.ProgressPropertyName}");
        internal static readonly Expression MaxTStartTEnd = Max(s_myTStart, s_myTEnd);
        internal static readonly Expression MinTStartTEnd = Min(s_myTStart, s_myTEnd);
        internal static readonly Expression MyPosition2 = Vector2("my.Position");
        internal static readonly Expression HalfSize2 = Divide(Vector2("my.Size"), Vector2(2, 2));
        // Depends on MyPosition2 and HalfSize2 so must be declared after them.
        internal static readonly Expression PositionAndSizeToOffsetExpression = Subtract(MyPosition2, HalfSize2);
        internal static readonly Expression TransformMatrixM11Expression = Scalar("my.TransformMatrix._11");
        internal static readonly Expression MyAnchor2 = Vector2("my.Anchor");
        internal static readonly Expression PositionMinusAnchor2 = Subtract(MyPosition2, MyAnchor2);
        internal static readonly Expression MyAnchor3 = Vector3(Scalar("my.Anchor.X"), Scalar("my.Anchor.Y"));
        internal static readonly Expression PositionMinusAnchor3 = Vector3(
                                                                        Subtract(Scalar("my.Position.X"), Scalar("my.Anchor.X")),
                                                                        Subtract(Scalar("my.Position.Y"), Scalar("my.Anchor.Y")),
                                                                        Scalar(0));

        internal static Expression PositionToOffsetExpression(Sn.Vector2 position) => Subtract(Vector2(position), HalfSize2);
        internal static Expression HalfSizeToOffsetExpression(Sn.Vector2 halfSize) => Subtract(MyPosition2, Vector2(halfSize));
        internal static Expression ScaledAndOffsetRootProgress(double scale, double offset)
        {
            var result = RootProgress;

            if (scale != 1)
            {
                result = Multiply(result, Scalar(scale));
            }
            if (offset != 0)
            {
                result = Sum(result, Scalar(offset));
            }

            return result;
        }

        ExpressionFactory() { }

        protected override string CreateExpressionString()
        {
            // Not needed - the class cannot be instantiated.
            throw new NotImplementedException();
        }

        protected override Expression Simplify()
        {
            // Not needed - the class cannot be instantiated.
            throw new NotImplementedException();
        }

        /// <summary>
        /// A segment of a progress expression. Defines the expression that is to be
        /// evaluated between two progress values.
        /// </summary>
        public sealed class Segment
        {
            public Segment(double fromProgress, double toProgress, Expression value)
            {
                Value = value;
                FromProgress = fromProgress;
                ToProgress = toProgress;
            }

            /// <summary>
            /// Defines the value for a progress expression over this segment.
            /// </summary>
            public Expression Value { get; }
            public double FromProgress { get; }
            public double ToProgress { get; }
        }


        internal static Expression CreateProgressExpression(Expression progress, params Segment[] segments)
        {
            // Verify that the segments are contiguous and start <= 0 and end >= 1
            var orderedSegments = segments.OrderBy(e => e.FromProgress).ToArray();
            if (orderedSegments.Length == 0)
            {
                throw new ArgumentException();
            }

            double previousTo = orderedSegments[0].FromProgress;
            int? firstSegmentIndex = null;
            int? lastSegmentIndex = null;

            for (var i = 0; i < orderedSegments.Length && !lastSegmentIndex.HasValue; i++)
            {
                var cur = orderedSegments[i];
                if (cur.FromProgress != previousTo)
                {
                    throw new ArgumentException("Progress expression is not contiguous.");
                }
                previousTo = cur.ToProgress;

                // If the segment includes 0, it is the first segment.
                if (!firstSegmentIndex.HasValue)
                {
                    if (cur.FromProgress <= 0 && cur.ToProgress > 0)
                    {
                        firstSegmentIndex = i;
                    }
                }

                // If the segment includes 1, it is the last segment.
                if (!lastSegmentIndex.HasValue)
                {
                    if (cur.ToProgress >= 1)
                    {
                        lastSegmentIndex = i;
                    }
                }
            }

            if (!firstSegmentIndex.HasValue || !lastSegmentIndex.HasValue)
            {
                throw new ArgumentException("Progress expression is not fully defined.");
            }

            // Include only the segments that are >= 0 or <= 1.
            return CreateProgressExpression(
                new ArraySegment<Segment>(
                    array: orderedSegments,
                    offset: firstSegmentIndex.Value,
                    count: 1 + lastSegmentIndex.Value - firstSegmentIndex.Value), progress);
        }

        static Expression CreateProgressExpression(ArraySegment<Segment> segments, Expression progress)
        {
            switch (segments.Count)
            {
                case 0:
                    throw new ArgumentException();
                case 1:
                    return segments.Array[segments.Offset].Value;
                default:
                    // Divide the list of expressions into 2 segments.
                    var pivot = segments.Count / 2;
                    var segmentsArray = segments.Array;
                    var expression0 = CreateProgressExpression(new ArraySegment<Segment>(segmentsArray, segments.Offset, pivot), progress);
                    var expression1 = CreateProgressExpression(new ArraySegment<Segment>(segmentsArray, segments.Offset + pivot, segments.Count - pivot), progress);
                    var pivotProgress = segmentsArray[segments.Offset + pivot - 1].ToProgress;
                    return new Ternary(
                        condition: new LessThan(progress, new Number(pivotProgress)),
                        trueValue: expression0,
                        falseValue: expression1);
            }
        }



    }
}
