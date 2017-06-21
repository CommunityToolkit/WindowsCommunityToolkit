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
            _nodeType = ExpressionNodeType.ConstantValue;
        }

        internal Matrix4x4Node(string paramName)
        {
            _paramName = paramName;
            _nodeType = ExpressionNodeType.ConstantParameter;
        }

        internal Matrix4x4Node(string paramName, Matrix4x4 value)
        {
            _paramName = paramName;
            _value = value;
            _nodeType = ExpressionNodeType.ConstantParameter;

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
            _11, _12, _13, _14,
            _21, _22, _23, _24,
            _31, _32, _33, _34,
            _41, _42, _43, _44,
        }

        // Commonly accessed subchannels
        public ScalarNode _11 { get { return GetSubchannels(Subchannel._11); } }

        public ScalarNode _12 { get { return GetSubchannels(Subchannel._12); } }

        public ScalarNode _13 { get { return GetSubchannels(Subchannel._13); } }

        public ScalarNode _14 { get { return GetSubchannels(Subchannel._14); } }

        public ScalarNode _21 { get { return GetSubchannels(Subchannel._21); } }

        public ScalarNode _22 { get { return GetSubchannels(Subchannel._22); } }

        public ScalarNode _23 { get { return GetSubchannels(Subchannel._23); } }

        public ScalarNode _24 { get { return GetSubchannels(Subchannel._24); } }

        public ScalarNode _31 { get { return GetSubchannels(Subchannel._31); } }

        public ScalarNode _32 { get { return GetSubchannels(Subchannel._32); } }

        public ScalarNode _33 { get { return GetSubchannels(Subchannel._33); } }

        public ScalarNode _34 { get { return GetSubchannels(Subchannel._34); } }

        public ScalarNode _41 { get { return GetSubchannels(Subchannel._41); } }

        public ScalarNode _42 { get { return GetSubchannels(Subchannel._42); } }

        public ScalarNode _43 { get { return GetSubchannels(Subchannel._43); } }

        public ScalarNode _44 { get { return GetSubchannels(Subchannel._44); } }

        public Vector3Node _11_22_33 { get { return GetSubchannels(Subchannel._11, Subchannel._22, Subchannel._33); } }

        public Vector3Node _41_42_43 { get { return GetSubchannels(Subchannel._41, Subchannel._42, Subchannel._43); } }

        /// <summary> Create a new type by re-arranging the Matrix subchannels. </summary>
        public ScalarNode GetSubchannels(Subchannel s) { return SubchannelsInternal<ScalarNode>(s.ToString()); }

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
            return $"Matrix4x4({_value.M11},{_value.M12},{_value.M13},{_value.M14}," +
                             $"{_value.M21},{_value.M22},{_value.M23},{_value.M24}," +
                             $"{_value.M31},{_value.M32},{_value.M33},{_value.M34}," +
                             $"{_value.M41},{_value.M42},{_value.M43},{_value.M44})";
        }

        private Matrix4x4 _value;
    }
#pragma warning restore CS0660, CS0661
}