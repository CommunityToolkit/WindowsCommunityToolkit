// Copyright(c) Microsoft Corporation.All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.Toolkit.Uwp.UI.Lottie.WinCompData.Expressions
{
    /// <summary>
    /// Raises a value to the power of 3. 
    /// </summary>
#if !WINDOWS_UWP
    public
#endif
    sealed class Cubed : Expression
    {
        public Cubed(Expression value)
        {
            Value = value;
        }

        public Expression Value { get; }

        protected override Expression Simplify()
        {
            var simplifiedValue = Value.Simplified;
            var numberValue = simplifiedValue as Number;
            return (numberValue != null)
                ? new Number(numberValue.Value * numberValue.Value * numberValue.Value)
                : (Expression)this;
        }

        internal override bool IsAtomic => true;

        protected override string CreateExpressionString()
        {
            var simplifiedValue = Value.Simplified;

            return $"Pow({simplifiedValue}, 3)";
        }

        public override ExpressionType InferredType => new ExpressionType(TypeConstraint.Scalar);
    }
}
