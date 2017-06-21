///---------------------------------------------------------------------------------------------------------------------
/// <copyright company="Microsoft">
///     Copyright (c) Microsoft Corporation.  All rights reserved.
/// </copyright>
///---------------------------------------------------------------------------------------------------------------------

namespace Microsoft.Toolkit.Uwp.UI.Animations.Expressions
{
// Ignore warning: 'ScalarNode' defines operator == or operator != but does not override Object.Equals(object o) && Object.GetHashCode()
#pragma warning disable CS0660, CS0661
    public sealed class ScalarNode : ExpressionNode
    {
        internal ScalarNode() 
        {
        }
        
        internal ScalarNode(float value)
        {
            _value = value;
            _nodeType = ExpressionNodeType.ConstantValue;
        }
        
        internal ScalarNode(string paramName)
        {
            _paramName = paramName;
            _nodeType = ExpressionNodeType.ConstantParameter;
        }
        
        internal ScalarNode(string paramName, float value)
        {
            _paramName = paramName;
            _value = value;
            _nodeType = ExpressionNodeType.ConstantParameter;

            SetScalarParameter(paramName, value);
        }
        
        
        //
        // Operator overloads
        //

        public static implicit operator ScalarNode(float value) { return new ScalarNode(value); }
        public static implicit operator ScalarNode(int value) { return new ScalarNode((float)value); }

        public static ScalarNode operator +(ScalarNode left, ScalarNode right) { return ExpressionFunctions.Function<ScalarNode>(ExpressionNodeType.Add, left, right);      }
        public static ScalarNode operator -(ScalarNode left, ScalarNode right) { return ExpressionFunctions.Function<ScalarNode>(ExpressionNodeType.Subtract, left, right); }
        public static ScalarNode operator -(ScalarNode value)                  { return ExpressionFunctions.Function<ScalarNode>(ExpressionNodeType.Negate, value);         }

        public static ScalarNode    operator *(ScalarNode left, ScalarNode right)    { return ExpressionFunctions.Function<ScalarNode>(ExpressionNodeType.Multiply, left, right);    }
        public static Vector2Node   operator *(ScalarNode left, Vector2Node right)   { return ExpressionFunctions.Function<Vector2Node>(ExpressionNodeType.Multiply, left, right);   }
        public static Vector3Node   operator *(ScalarNode left, Vector3Node right)   { return ExpressionFunctions.Function<Vector3Node>(ExpressionNodeType.Multiply, left, right);   }
        public static Vector4Node   operator *(ScalarNode left, Vector4Node right)   { return ExpressionFunctions.Function<Vector4Node>(ExpressionNodeType.Multiply, left, right);   }
        public static Matrix4x4Node operator *(ScalarNode left, Matrix4x4Node right) { return ExpressionFunctions.Function<Matrix4x4Node>(ExpressionNodeType.Multiply, left, right); }

        public static ScalarNode operator /(ScalarNode left, ScalarNode right) { return ExpressionFunctions.Function<ScalarNode>(ExpressionNodeType.Divide, left, right);  }
        public static ScalarNode operator %(ScalarNode left, ScalarNode right) { return ExpressionFunctions.Function<ScalarNode>(ExpressionNodeType.Modulus, left, right); }

        public static BooleanNode operator ==(ScalarNode left, ScalarNode right) { return ExpressionFunctions.Function<BooleanNode>(ExpressionNodeType.Equals, left, right);            }
        public static BooleanNode operator !=(ScalarNode left, ScalarNode right) { return ExpressionFunctions.Function<BooleanNode>(ExpressionNodeType.NotEquals, left, right);         }
        public static BooleanNode operator <=(ScalarNode left, ScalarNode right) { return ExpressionFunctions.Function<BooleanNode>(ExpressionNodeType.LessThanEquals, left, right);    }
        public static BooleanNode operator <(ScalarNode left, ScalarNode right)  { return ExpressionFunctions.Function<BooleanNode>(ExpressionNodeType.LessThan, left, right);          }
        public static BooleanNode operator >=(ScalarNode left, ScalarNode right) { return ExpressionFunctions.Function<BooleanNode>(ExpressionNodeType.GreaterThanEquals, left, right); }
        public static BooleanNode operator >(ScalarNode left, ScalarNode right)  { return ExpressionFunctions.Function<BooleanNode>(ExpressionNodeType.GreaterThan, left, right);       }

        internal protected override string GetValue()
        {
            return _value.ToString();
        }

        private float _value;
    }
#pragma warning restore CS0660, CS0661
}