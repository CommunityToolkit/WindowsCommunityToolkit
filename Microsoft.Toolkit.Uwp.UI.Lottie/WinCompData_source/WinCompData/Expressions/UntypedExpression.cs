// Copyright(c) Microsoft Corporation.All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.Toolkit.Uwp.UI.Lottie.WinCompData.Expressions
{
#if !WINDOWS_UWP
    public
#endif
    sealed class UntypedExpression : Expression
    {
        readonly string _value;
        public UntypedExpression(string value)
        {
            _value = value;
        }

        protected override Expression Simplify()
        {
            return this;
        }

        protected override string CreateExpressionString() => _value;

        public override ExpressionType InferredType => new ExpressionType(TypeConstraint.AllValidTypes);
    }
}
