using System.Numerics;

namespace Microsoft.Toolkit.Uwp.UI.Animations.Expressions
{
    // Ignore warning: 'Matrix4x4Node' defines operator == or operator != but does not override Object.Equals(object o) && Object.GetHashCode()
#pragma warning disable CS0660, CS0661
    public sealed class Matrix4x4Node : ExpressionNode
    {
        internal Matrix4x4Node()
        {
        }

        internal Matrix4x4Node(Matrix4x4 value)
        {
            _value = value;
            NodeType = ExpressionNodeType.ConstantValue;
        }

        internal Matrix4x4Node(string paramName)
        {
            ParamName = paramName;
            NodeType = ExpressionNodeType.ConstantParameter;
        }

        internal Matrix4x4Node(string paramName, Matrix4x4 value)
        {
            ParamName = paramName;
            _value = value;
            NodeType = ExpressionNodeType.ConstantParameter;

            SetMatrix4x4Parameter(paramName, value);
        }

        //
        // Operator overloads
        //
        public static implicit operator Matrix4x4Node(Matrix4x4 value)
        {
            return new Matrix4x4Node(value);
        }

        public static Matrix4x4Node operator +(Matrix4x4Node left, Matrix4x4Node right)
        {
            return ExpressionFunctions.Function<Matrix4x4Node>(ExpressionNodeType.Add, left, right);
        }

        public static Matrix4x4Node operator -(Matrix4x4Node left, Matrix4x4Node right)
        {
            return ExpressionFunctions.Function<Matrix4x4Node>(ExpressionNodeType.Subtract, left, right);
        }

        public static Matrix4x4Node operator -(Matrix4x4Node value)
        {
            return ExpressionFunctions.Function<Matrix4x4Node>(ExpressionNodeType.Negate, value);
        }

        public static Matrix4x4Node operator *(Matrix4x4Node left, ScalarNode right)
        {
            return ExpressionFunctions.Function<Matrix4x4Node>(ExpressionNodeType.Multiply, left, right);
        }

        public static Matrix4x4Node operator *(Matrix4x4Node left, Matrix4x4Node right)
        {
            return ExpressionFunctions.Function<Matrix4x4Node>(ExpressionNodeType.Multiply, left, right);
        }

        public static BooleanNode operator ==(Matrix4x4Node left, Matrix4x4Node right)
        {
            return ExpressionFunctions.Function<BooleanNode>(ExpressionNodeType.Equals, left, right);
        }

        public static BooleanNode operator !=(Matrix4x4Node left, Matrix4x4Node right)
        {
            return ExpressionFunctions.Function<BooleanNode>(ExpressionNodeType.NotEquals, left, right);
        }

        //
        // Subchannels
        //
        public enum Subchannel
        {
            Channel11, Channel12, Channel13, Channel14,
            Channel21, Channel22, Channel23, Channel24,
            Channel31, Channel32, Channel33, Channel34,
            Channel41, Channel42, Channel43, Channel44,
        }

        // Commonly accessed subchannels
        public ScalarNode Channel11
        {
            get { return GetSubchannels(Subchannel.Channel11); }
        }

        public ScalarNode Channel12
        {
            get { return GetSubchannels(Subchannel.Channel12); }
        }

        public ScalarNode Channel13
        {
            get { return GetSubchannels(Subchannel.Channel13); }
        }

        public ScalarNode Channel14
        {
            get { return GetSubchannels(Subchannel.Channel14); }
        }

        public ScalarNode Channel21
        {
            get { return GetSubchannels(Subchannel.Channel21); }
        }

        public ScalarNode Channel22
        {
            get { return GetSubchannels(Subchannel.Channel22); }
        }

        public ScalarNode Channel23
        {
            get { return GetSubchannels(Subchannel.Channel23); }
        }

        public ScalarNode Channel24
        {
            get { return GetSubchannels(Subchannel.Channel24); }
        }

        public ScalarNode Channel31
        {
            get { return GetSubchannels(Subchannel.Channel31); }
        }

        public ScalarNode Channel32
        {
            get { return GetSubchannels(Subchannel.Channel32); }
        }

        public ScalarNode Channel33
        {
            get { return GetSubchannels(Subchannel.Channel33); }
        }

        public ScalarNode Channel34
        {
            get { return GetSubchannels(Subchannel.Channel34); }
        }

        public ScalarNode Channel41
        {
            get { return GetSubchannels(Subchannel.Channel41); }
        }

        public ScalarNode Channel42
        {
            get { return GetSubchannels(Subchannel.Channel42); }
        }

        public ScalarNode Channel43
        {
            get { return GetSubchannels(Subchannel.Channel43); }
        }

        public ScalarNode Channel44
        {
            get { return GetSubchannels(Subchannel.Channel44); }
        }

        public Vector3Node Channel11Channel22Channel33
        {
            get { return GetSubchannels(Subchannel.Channel11, Subchannel.Channel22, Subchannel.Channel33); }
        }

        public Vector3Node Channel41Channel42Channel43
        {
            get { return GetSubchannels(Subchannel.Channel41, Subchannel.Channel42, Subchannel.Channel43); }
        }

        /// <summary> Create a new type by re-arranging the Matrix subchannels. </summary>
        public ScalarNode GetSubchannels(Subchannel s)
        {
            return SubchannelsInternal<ScalarNode>(s.ToString());
        }

        public Vector2Node GetSubchannels(Subchannel s1, Subchannel s2)
        {
            return SubchannelsInternal<Vector2Node>(s1.ToString(), s2.ToString());
        }

        public Vector3Node GetSubchannels(Subchannel s1, Subchannel s2, Subchannel s3)
        {
            return SubchannelsInternal<Vector3Node>(s1.ToString(), s2.ToString(), s3.ToString());
        }

        public Vector4Node GetSubchannels(Subchannel s1, Subchannel s2, Subchannel s3, Subchannel s4)
        {
            return SubchannelsInternal<Vector4Node>(s1.ToString(), s2.ToString(), s3.ToString(), s4.ToString());
        }

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

        protected internal override string GetValue()
        {
            return $"Matrix4x4({_value.M11},{_value.M12},{_value.M13},{_value.M14}," +
                             $"{_value.M21},{_value.M22},{_value.M23},{_value.M24}," +
                             $"{_value.M31},{_value.M32},{_value.M33},{_value.M34}," +
                             $"{_value.M41},{_value.M42},{_value.M43},{_value.M44})";
        }

        private Matrix4x4 _value;
    }
#pragma warning restore CS0660, CS0661
}