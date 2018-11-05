// Copyright(c) Microsoft Corporation.All rights reserved.
// Licensed under the MIT License.

using System;

namespace Microsoft.Toolkit.Uwp.UI.Lottie.WinCompData.Expressions
{
#if !WINDOWS_UWP
    public
#endif
    sealed class Max : BinaryExpression
    {
        public Max(Expression left, Expression right) : base(left, right)
        {
        }

        protected override Expression Simplify()
        {
            var a = Left.Simplified;
            var b = Right.Simplified;

            var numberA = a as Number;
            var numberB = b as Number;
            if (numberA != null && numberB != null)
            {
                // They're both constants. Evaluate them.
                return new Number(Math.Max(numberA.Value, numberB.Value));
            }

            if (a != Left || b != Right)
            {
                return new Max(a, b);
            }

            return this;
        }

        protected override string CreateExpressionString() => $"Max({Parenthesize(Left.Simplified)}, {Parenthesize(Right.Simplified)})";

        public override ExpressionType InferredType =>
            ExpressionType.AssertMatchingTypes(
                TypeConstraint.Scalar | TypeConstraint.Vector2 | TypeConstraint.Vector3 | TypeConstraint.Vector4, 
                Left.InferredType, Right.InferredType,
                TypeConstraint.Scalar | TypeConstraint.Vector2 | TypeConstraint.Vector3 | TypeConstraint.Vector4);
    }
}
