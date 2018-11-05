// Copyright(c) Microsoft Corporation.All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.Toolkit.Uwp.UI.Lottie.WinCompData.Expressions
{
#if !WINDOWS_UWP
    public
#endif
    sealed class LessThan : BinaryExpression
    {
        public LessThan(Expression left, Expression right) : base(left, right)
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
                return new Boolean(numberA.Value < numberB.Value);
            }

            if (a != Left || b != Right)
            {
                return new LessThan(a, b);
            }

            return this;
        }

        protected override string CreateExpressionString() => $"{Parenthesize(Left.Simplified)} < {Parenthesize(Right.Simplified)}";

        public override ExpressionType InferredType => 
            ExpressionType.AssertMatchingTypes(TypeConstraint.Scalar, Left.InferredType, Right.InferredType, TypeConstraint.Boolean);
    }
}
