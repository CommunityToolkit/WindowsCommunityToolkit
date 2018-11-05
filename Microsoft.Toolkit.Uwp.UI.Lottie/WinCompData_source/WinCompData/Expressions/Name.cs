// Copyright(c) Microsoft Corporation.All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.Toolkit.Uwp.UI.Lottie.WinCompData.Expressions
{
    /// <summary>
    /// A name in an <see cref="Expression"/>.
    /// </summary>
#if !WINDOWS_UWP
    public
#endif
    sealed class Name : Expression
    {
        readonly string _value;
        internal static readonly Name[] s_emptyNames = new Name[0];

        public Name(string value)
        {
            _value = value;
        }

        protected override Expression Simplify()
        {
            return this;
        }

        public string Value => _value;

        protected override string CreateExpressionString() => _value;


        public override ExpressionType InferredType => new ExpressionType(TypeConstraint.AllValidTypes);

        internal override bool IsAtomic => true;
    }
}
