using System.Numerics;

namespace Microsoft.Toolkit.Uwp.UI.Animations.Expressions
{
// Ignore warning: 'QuaternionNode' defines operator == or operator != but does not override Object.Equals(object o) && Object.GetHashCode()
#pragma warning disable CS0660, CS0661
    public sealed class QuaternionNode : ExpressionNode
    {
        internal QuaternionNode() 
        {
        }
        
        internal QuaternionNode(Quaternion value)
        {
            _value = value;
            _nodeType = ExpressionNodeType.ConstantValue;
        }
        
        internal QuaternionNode(string paramName)
        {
            _paramName = paramName;
            _nodeType = ExpressionNodeType.ConstantParameter;
        }
        
        internal QuaternionNode(string paramName, Quaternion value)
        {
            _paramName = paramName;
            _value = value;
            _nodeType = ExpressionNodeType.ConstantParameter;

            SetQuaternionParameter(paramName, value);
        }
        
        
        //
        // Operator overloads
        //

        public static implicit operator QuaternionNode(Quaternion value) { return new QuaternionNode(value); }

        public static QuaternionNode operator *(QuaternionNode left, ScalarNode right)     { return ExpressionFunctions.Function<QuaternionNode>(ExpressionNodeType.Multiply, left, right); }
        public static QuaternionNode operator *(QuaternionNode left, QuaternionNode right) { return ExpressionFunctions.Function<QuaternionNode>(ExpressionNodeType.Multiply, left, right); }

        public static QuaternionNode operator /(QuaternionNode left, QuaternionNode right) { return ExpressionFunctions.Function<QuaternionNode>(ExpressionNodeType.Divide, left, right); }

        public static BooleanNode operator ==(QuaternionNode left, QuaternionNode right) { return ExpressionFunctions.Function<BooleanNode>(ExpressionNodeType.Equals, left, right);    }
        public static BooleanNode operator !=(QuaternionNode left, QuaternionNode right) { return ExpressionFunctions.Function<BooleanNode>(ExpressionNodeType.NotEquals, left, right); }

        internal protected override string GetValue()
        {
            return $"Quaternion({_value.X},{_value.Y},{_value.Z},{_value.W})";
        }

        private Quaternion _value;
    }
#pragma warning restore CS0660, CS0661
}