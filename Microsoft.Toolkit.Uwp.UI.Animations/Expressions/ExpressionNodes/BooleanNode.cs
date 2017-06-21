namespace Microsoft.Toolkit.Uwp.UI.Animations.Expressions
{
    // Ignore warning: 'BooleanNode' defines operator == or operator != but does not override Object.Equals(object o) && Object.GetHashCode()
#pragma warning disable CS0660, CS0661
    public sealed class BooleanNode : ExpressionNode
    {
        internal BooleanNode()
        {
        }

        internal BooleanNode(bool value)
        {
            _value = value;
            _nodeType = ExpressionNodeType.ConstantValue;
        }

        internal BooleanNode(string paramName)
        {
            _paramName = paramName;
            _nodeType = ExpressionNodeType.ConstantParameter;
        }

        internal BooleanNode(string paramName, bool value)
        {
            _paramName = paramName;
            _value = value;
            _nodeType = ExpressionNodeType.ConstantParameter;

            SetBooleanParameter(paramName, value);
        }

        //
        // Operator overloads
        //

        public static implicit operator BooleanNode(bool value) { return new BooleanNode(value); }

        public static BooleanNode operator ==(BooleanNode left, BooleanNode right)
        {
            return ExpressionFunctions.Function<BooleanNode>(ExpressionNodeType.Equals, left, right);
        }

        public static BooleanNode operator !=(BooleanNode left, BooleanNode right)
        {
            return ExpressionFunctions.Function<BooleanNode>(ExpressionNodeType.NotEquals, left, right);
        }

        public static BooleanNode operator &(BooleanNode left, BooleanNode right)
        {
            return ExpressionFunctions.Function<BooleanNode>(ExpressionNodeType.And, left, right);
        }

        public static BooleanNode operator |(BooleanNode left, BooleanNode right)
        {
            return ExpressionFunctions.Function<BooleanNode>(ExpressionNodeType.Or, left, right);
        }

        public static BooleanNode operator !(BooleanNode value)
        {
            return ExpressionFunctions.Function<BooleanNode>(ExpressionNodeType.Not, value);
        }

        protected internal override string GetValue()
        {
            return _value ? "true" : "false";
        }

        private bool _value;
    }
#pragma warning restore CS0660, CS0661
}