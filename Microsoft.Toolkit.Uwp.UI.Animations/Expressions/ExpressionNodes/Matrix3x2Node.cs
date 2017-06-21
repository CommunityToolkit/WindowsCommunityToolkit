namespace Microsoft.Toolkit.Uwp.UI.Animations.Expressions
{
    using System.Numerics;

// Ignore warning: 'Matrix3x2Node' defines operator == or operator != but does not override Object.Equals(object o) && Object.GetHashCode()
#pragma warning disable CS0660, CS0661
    public sealed class Matrix3x2Node : ExpressionNode
    {
        internal Matrix3x2Node() 
        {
        }
        
        internal Matrix3x2Node(Matrix3x2 value)
        {
            _value = value;
            _nodeType = ExpressionNodeType.ConstantValue;
        }
        
        internal Matrix3x2Node(string paramName)
        {
            _paramName = paramName;
            _nodeType = ExpressionNodeType.ConstantParameter;
        }
        
        internal Matrix3x2Node(string paramName, Matrix3x2 value)
        {
            _paramName = paramName;
            _value = value;
            _nodeType = ExpressionNodeType.ConstantParameter;

            SetMatrix3x2Parameter(paramName, value);
        }
        
        
        //
        // Operator overloads
        //

        public static implicit operator Matrix3x2Node(Matrix3x2 value) { return new Matrix3x2Node(value); }

        public static Matrix3x2Node operator +(Matrix3x2Node left, Matrix3x2Node right) { return ExpressionFunctions.Function<Matrix3x2Node>(ExpressionNodeType.Add, left, right);      }
        public static Matrix3x2Node operator -(Matrix3x2Node left, Matrix3x2Node right) { return ExpressionFunctions.Function<Matrix3x2Node>(ExpressionNodeType.Subtract, left, right); }
        public static Matrix3x2Node operator -(Matrix3x2Node value)                     { return ExpressionFunctions.Function<Matrix3x2Node>(ExpressionNodeType.Negate, value);         }

        public static Matrix3x2Node operator *(Matrix3x2Node left, ScalarNode right)    { return ExpressionFunctions.Function<Matrix3x2Node>(ExpressionNodeType.Multiply, left, right); }
        public static Matrix3x2Node operator *(Matrix3x2Node left, Matrix3x2Node right) { return ExpressionFunctions.Function<Matrix3x2Node>(ExpressionNodeType.Multiply, left, right); }

        public static BooleanNode operator ==(Matrix3x2Node left, Matrix3x2Node right) { return ExpressionFunctions.Function<BooleanNode>(ExpressionNodeType.Equals, left, right);    }
        public static BooleanNode operator !=(Matrix3x2Node left, Matrix3x2Node right) { return ExpressionFunctions.Function<BooleanNode>(ExpressionNodeType.NotEquals, left, right); }

        
        //
        // Subchannels
        //

        public enum Subchannel
        {
            _11, _12,
            _21, _22,
            _31, _32,
        }

        // Commonly accessed subchannels
        public ScalarNode _11 { get { return GetSubchannels(Subchannel._11); } }
        public ScalarNode _12 { get { return GetSubchannels(Subchannel._12); } }
        public ScalarNode _21 { get { return GetSubchannels(Subchannel._21); } }
        public ScalarNode _22 { get { return GetSubchannels(Subchannel._22); } }
        public ScalarNode _31 { get { return GetSubchannels(Subchannel._31); } }
        public ScalarNode _32 { get { return GetSubchannels(Subchannel._32); } }

        /// <summary> Create a new type by re-arranging the Matrix subchannels. </summary>
        public ScalarNode  GetSubchannels(Subchannel s) { return SubchannelsInternal<ScalarNode>(s.ToString()); }
        public Vector2Node GetSubchannels(Subchannel s1, Subchannel s2) { return SubchannelsInternal<Vector2Node>(s1.ToString(), s2.ToString()); }
        public Vector3Node GetSubchannels(Subchannel s1, Subchannel s2, Subchannel s3) { return SubchannelsInternal<Vector3Node>(s1.ToString(), s2.ToString(), s3.ToString()); }
        public Vector4Node GetSubchannels(Subchannel s1, Subchannel s2, Subchannel s3, Subchannel s4) { return SubchannelsInternal<Vector4Node>(s1.ToString(), s2.ToString(), s3.ToString(), s4.ToString()); }

        public Matrix3x2Node GetSubchannels(Subchannel s1, Subchannel s2, Subchannel s3, Subchannel s4, Subchannel s5, Subchannel s6)
        { 
            return SubchannelsInternal<Matrix3x2Node>(s1.ToString(), s2.ToString(), s3.ToString(), s4.ToString(), s5.ToString(), s6.ToString()); 
        }

        public Matrix4x4Node GetSubchannels(Subchannel s1, Subchannel s2, Subchannel s3, Subchannel s4,
                                         Subchannel s5, Subchannel s6, Subchannel s7, Subchannel s8,
                                         Subchannel s9, Subchannel s10, Subchannel s11, Subchannel s12,
                                         Subchannel s13, Subchannel s14, Subchannel s15, Subchannel s16)
        {
            return SubchannelsInternal<Matrix4x4Node>(s1.ToString(), s2.ToString(), s3.ToString(), s4.ToString(),
                                                      s5.ToString(), s6.ToString(), s7.ToString(), s8.ToString(),
                                                      s9.ToString(), s10.ToString(), s11.ToString(), s12.ToString(),
                                                      s13.ToString(), s14.ToString(), s15.ToString(), s16.ToString());
        }

        internal protected override string GetValue()
        {
            return $"Matrix3x2({_value.M11},{_value.M12},{_value.M21},{_value.M22},{_value.M31},{_value.M32})";
        }

        private Matrix3x2 _value;
    }
#pragma warning restore CS0660, CS0661
}