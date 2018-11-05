// Copyright(c) Microsoft Corporation.All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.Toolkit.Uwp.UI.Lottie.WinCompData.Expressions
{
#if !WINDOWS_UWP
    public
#endif
    abstract class BinaryExpression : Expression
    {
        public Expression Left { get; }
        public Expression Right { get; }
        protected private BinaryExpression(Expression left, Expression right)
        {
            Left = left;
            Right = right;
        }
    }
}
