// Copyright(c) Microsoft Corporation.All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.Toolkit.Uwp.UI.Lottie.WinCompData.Expressions
{
#if !WINDOWS_UWP
    public
#endif
    sealed class Multiply : BinaryExpression
    {
        public Multiply(Expression left, Expression right) : base(left, right)
        {
        }

        protected override Expression Simplify()
        {
            var a = Left.Simplified;
            var b = Right.Simplified;

            if (IsZero(a))
            {
                return a;
            }
            if (IsZero(b))
            {
                return b;
            }
            if (IsOne(a))
            {
                return b;
            }
            if (IsOne(b))
            {
                return a;
            }

            var numberA = a as Number;
            var numberB = b as Number;
            if (numberA != null && numberB != null)
            {
                // They're both constants. Evaluate them.
                return new Number(numberA.Value * numberB.Value);
            }

            if (a != Left || b != Right)
            {
                return new Multiply(a, b);
            }

            return this;
        }

        protected override string CreateExpressionString()
        {
            var a = Left.Simplified;
            var b = Right.Simplified;

            // Avoid the parentheses if the child is a Multiply - multiply
            // is commutative.
            var aString = a is Multiply ? a.ToString() : Parenthesize(a);
            var bString = b is Multiply ? b.ToString() : Parenthesize(b);

            return $"{aString} * {bString}";
        }

        public override ExpressionType InferredType
        {
            get
            {
                var leftType = Left.InferredType;
                var rightType = Right.InferredType;

                return ExpressionType.AssertMatchingTypes(
                        TypeConstraint.Scalar | TypeConstraint.Vector2 | TypeConstraint.Vector3 | TypeConstraint.Vector4,
                        leftType,
                        rightType,
                        ExpressionType.IntersectConstraints(leftType.Constraints, rightType.Constraints));
            }
        }
    }
}
