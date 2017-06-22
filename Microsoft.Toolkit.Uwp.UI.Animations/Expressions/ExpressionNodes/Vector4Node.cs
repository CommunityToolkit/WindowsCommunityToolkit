using System.Numerics;

namespace Microsoft.Toolkit.Uwp.UI.Animations.Expressions
{
    // Ignore warning: 'Vector4Node' defines operator == or operator != but does not override Object.Equals(object o) && Object.GetHashCode()
#pragma warning disable CS0660, CS0661
    public sealed class Vector4Node : ExpressionNode
    {
        internal Vector4Node()
        {
        }

        internal Vector4Node(Vector4 value)
        {
            _value = value;
            NodeType = ExpressionNodeType.ConstantValue;
        }

        internal Vector4Node(string paramName)
        {
            ParamName = paramName;
            NodeType = ExpressionNodeType.ConstantParameter;
        }

        internal Vector4Node(string paramName, Vector4 value)
        {
            ParamName = paramName;
            _value = value;
            NodeType = ExpressionNodeType.ConstantParameter;

            SetVector4Parameter(paramName, value);
        }

        public static implicit operator Vector4Node(Vector4 value)
        {
            return new Vector4Node(value);
        }

        public static Vector4Node operator +(Vector4Node left, Vector4Node right)
        {
            return ExpressionFunctions.Function<Vector4Node>(ExpressionNodeType.Add, left, right);
        }

        public static Vector4Node operator -(Vector4Node left, Vector4Node right)
        {
            return ExpressionFunctions.Function<Vector4Node>(ExpressionNodeType.Subtract, left, right);
        }

        public static Vector4Node operator -(Vector4Node value)
        {
            return ExpressionFunctions.Function<Vector4Node>(ExpressionNodeType.Negate, value);
        }

        public static Vector4Node operator *(Vector4Node left, ScalarNode right)
        {
            return ExpressionFunctions.Function<Vector4Node>(ExpressionNodeType.Multiply, left, right);
        }

        public static Vector4Node operator *(Vector4Node left, Vector4Node right)
        {
            return ExpressionFunctions.Function<Vector4Node>(ExpressionNodeType.Multiply, left, right);
        }

        public static Vector4Node operator /(Vector4Node left, Vector4Node right)
        {
            return ExpressionFunctions.Function<Vector4Node>(ExpressionNodeType.Divide, left, right);
        }

        public static Vector4Node operator %(Vector4Node left, Vector4Node right)
        {
            return ExpressionFunctions.Function<Vector4Node>(ExpressionNodeType.Modulus, left, right);
        }

        public static BooleanNode operator ==(Vector4Node left, Vector4Node right)
        {
            return ExpressionFunctions.Function<BooleanNode>(ExpressionNodeType.Equals, left, right);
        }

        public static BooleanNode operator !=(Vector4Node left, Vector4Node right)
        {
            return ExpressionFunctions.Function<BooleanNode>(ExpressionNodeType.NotEquals, left, right);
        }

        public enum Subchannel
        {
            X,
            Y,
            Z,
            W
        }

        // Commonly accessed subchannels
        public ScalarNode X
        {
            get { return GetSubchannels(Subchannel.X); }
        }

        public ScalarNode Y
        {
            get { return GetSubchannels(Subchannel.Y); }
        }

        public ScalarNode Z
        {
            get { return GetSubchannels(Subchannel.Z); }
        }

        public ScalarNode W
        {
            get { return GetSubchannels(Subchannel.W); }
        }

        public Vector2Node XY
        {
            get { return GetSubchannels(Subchannel.X, Subchannel.Y); }
        }

        public Vector3Node XYZ
        {
            get { return GetSubchannels(Subchannel.X, Subchannel.Y, Subchannel.Z); }
        }

        /// <summary> Create a new type by re-arranging the Vector subchannels. </summary>
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
            return $"Vector4({_value.X},{_value.Y},{_value.Z},{_value.W})";
        }

        private Vector4 _value;
    }
#pragma warning restore CS0660, CS0661
}