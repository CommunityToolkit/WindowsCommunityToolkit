// Copyright(c) Microsoft Corporation.All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.Toolkit.Uwp.UI.Lottie.WinCompData.Expressions
{
#if !WINDOWS_UWP
    public
#endif
    sealed class Vector3 : Expression
    {
        public Expression X { get; }
        public Expression Y { get; }
        public Expression Z { get; }

        internal Vector3(Expression x, Expression y, Expression z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        protected override Expression Simplify() => this;

        protected override string CreateExpressionString() => $"Vector3({Parenthesize(X)},{Parenthesize(Y)},{Parenthesize(Z)})";

        internal override bool IsAtomic => true;

        public override ExpressionType InferredType => new ExpressionType(TypeConstraint.Vector3);
    }
}
