// Copyright(c) Microsoft Corporation.All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.Toolkit.Uwp.UI.Lottie.WinCompData.Expressions
{
#if !WINDOWS_UWP
    public
#endif
    sealed class Ternary : Expression
    {
        public Ternary(Expression condition, Expression trueValue, Expression falseValue)
        {
            Condition = condition;
            TrueValue = trueValue;
            FalseValue = falseValue;

        }
        public Expression Condition;
        public Expression TrueValue;
        public Expression FalseValue;

        protected override Expression Simplify()
        {
            var c = Condition.Simplified;
            var t = TrueValue.Simplified;
            var f = FalseValue.Simplified;

            if (c is Boolean cBool)
            {
                return cBool.Value ? t : f;
            }

            if (t != TrueValue || f != FalseValue)
            {
                return new Ternary(c, t, f);
            }
            return this;
        }

        protected override string CreateExpressionString()
            => $"{Parenthesize(Condition)} ? {Parenthesize(TrueValue)} : {Parenthesize(FalseValue)}";

        public override ExpressionType InferredType
        {
            get {
                var trueType = TrueValue.InferredType;
                var falseType = FalseValue.InferredType;

                return ExpressionType.AssertMatchingTypes(
                    TypeConstraint.AllValidTypes,
                    trueType,
                    falseType,
                    ExpressionType.IntersectConstraints(trueType.Constraints, falseType.Constraints));
            }
        }

    }
}
