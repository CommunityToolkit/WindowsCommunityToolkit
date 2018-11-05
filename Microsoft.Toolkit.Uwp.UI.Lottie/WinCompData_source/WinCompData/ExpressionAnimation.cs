// Copyright(c) Microsoft Corporation.All rights reserved.
// Licensed under the MIT License.

using Microsoft.Toolkit.Uwp.UI.Lottie.WinCompData.Expressions;

namespace Microsoft.Toolkit.Uwp.UI.Lottie.WinCompData
{
#if !WINDOWS_UWP
    public
#endif
    sealed class ExpressionAnimation : CompositionAnimation
    {
        internal ExpressionAnimation(Expression expression) : this(null, expression)
        {
        }

        ExpressionAnimation(ExpressionAnimation other, Expression expression) : base(other)
        {
            Expression = expression;
        }

        public Expression Expression { get; }

        public override CompositionObjectType Type => CompositionObjectType.ExpressionAnimation;

        internal override CompositionAnimation Clone() => new ExpressionAnimation(this, Expression);

        public override string ToString() => Expression.ToString();
    }
}
