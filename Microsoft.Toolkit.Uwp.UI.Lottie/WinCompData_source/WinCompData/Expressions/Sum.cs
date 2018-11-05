// Copyright(c) Microsoft Corporation.All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.Toolkit.Uwp.UI.Lottie.WinCompData.Expressions
{
#if !WINDOWS_UWP
    public
#endif
    sealed class Sum : BinaryExpression
    {
        public Sum(Expression left, Expression right) : base(left, right)
        {
        }

        protected override Expression Simplify()
        {
            var a = Left.Simplified;
            var b = Right.Simplified;
            if (IsZero(a))
            {
                return b;
            }
            if (IsZero(b))
            {
                return a;
            }

            if (a is Number numberA && b is Number numberB)
            {
                return Sum(numberA, numberB);
            }

            return this;
        }

        protected override string CreateExpressionString()
        {
            var a = Left.Simplified;
            var b = Right.Simplified;

            var aString = a is Sum ? a.ToString() : Parenthesize(a);
            var bString = b is Sum ? b.ToString() : Parenthesize(b);

            return $"{aString} + {bString}";
        }

        public override ExpressionType InferredType =>
            ExpressionType.ConstrainToTypes(TypeConstraint.Scalar, Left.InferredType, Right.InferredType);
    }
}
