using Windows.UI;

namespace Microsoft.Toolkit.Uwp.UI.Animations.Expressions
{
    // Ignore warning: 'ColorNode' defines operator == or operator != but does not override Object.Equals(object o) && Object.GetHashCode()
#pragma warning disable CS0660, CS0661
    public sealed class ColorNode : ExpressionNode
    {
        internal ColorNode()
        {
        }

        internal ColorNode(Color value)
        {
            _value = value;
            NodeType = ExpressionNodeType.ConstantValue;
        }

        internal ColorNode(string paramName)
        {
            ParamName = paramName;
            NodeType = ExpressionNodeType.ConstantParameter;
        }

        internal ColorNode(string paramName, Color value)
        {
            ParamName = paramName;
            _value = value;
            NodeType = ExpressionNodeType.ConstantParameter;

            SetColorParameter(paramName, value);
        }

        //
        // Operator overloads
        //
        public static implicit operator ColorNode(Color value)
        {
            return new ColorNode(value);
        }

        public static BooleanNode operator ==(ColorNode left, ColorNode right)
        {
            return ExpressionFunctions.Function<BooleanNode>(ExpressionNodeType.Equals, left, right);
        }

        public static BooleanNode operator !=(ColorNode left, ColorNode right)
        {
            return ExpressionFunctions.Function<BooleanNode>(ExpressionNodeType.NotEquals, left, right);
        }

        protected internal override string GetValue()
        {
            return $"ColorRgb({_value.A},{_value.R},{_value.G},{_value.B})";
        }

        private Color _value;
    }
#pragma warning restore CS0660, CS0661
}