// Copyright(c) Microsoft Corporation.All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.Toolkit.Uwp.UI.Lottie.WinCompData.Expressions
{
    /// <summary>
    /// Raises a value to the power of 2. 
    /// </summary>
#if !WINDOWS_UWP
    public
#endif
    sealed class Squared : Expression
    {
        public Squared(Expression value)
        {
            Value = value;
        }

        public Expression Value { get; }

        protected override Expression Simplify()
        {
            var simplifiedValue = Value.Simplified;
            var numberValue = simplifiedValue as Number;
            return (numberValue != null)
                ? new Number(numberValue.Value * numberValue.Value)
                : (Expression)this;
        }

        protected override string CreateExpressionString() => $"Square({Value.Simplified})";

        internal override bool IsAtomic => true;

        public override ExpressionType InferredType
        {
            get
            {
                return new ExpressionType(TypeConstraint.AllValidTypes);
            }
        }


    }
}
