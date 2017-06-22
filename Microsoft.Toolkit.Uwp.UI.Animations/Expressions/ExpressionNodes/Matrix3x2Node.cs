// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

using System.Numerics;

namespace Microsoft.Toolkit.Uwp.UI.Animations.Expressions
{
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
            NodeType = ExpressionNodeType.ConstantValue;
        }

        internal Matrix3x2Node(string paramName)
        {
            ParamName = paramName;
            NodeType = ExpressionNodeType.ConstantParameter;
        }

        internal Matrix3x2Node(string paramName, Matrix3x2 value)
        {
            ParamName = paramName;
            _value = value;
            NodeType = ExpressionNodeType.ConstantParameter;

            SetMatrix3x2Parameter(paramName, value);
        }

        public static implicit operator Matrix3x2Node(Matrix3x2 value)
        {
            return new Matrix3x2Node(value);
        }

        public static Matrix3x2Node operator +(Matrix3x2Node left, Matrix3x2Node right)
        {
            return ExpressionFunctions.Function<Matrix3x2Node>(ExpressionNodeType.Add, left, right);
        }

        public static Matrix3x2Node operator -(Matrix3x2Node left, Matrix3x2Node right)
        {
            return ExpressionFunctions.Function<Matrix3x2Node>(ExpressionNodeType.Subtract, left, right);
        }

        public static Matrix3x2Node operator -(Matrix3x2Node value)
        {
            return ExpressionFunctions.Function<Matrix3x2Node>(ExpressionNodeType.Negate, value);
        }

        public static Matrix3x2Node operator *(Matrix3x2Node left, ScalarNode right)
        {
            return ExpressionFunctions.Function<Matrix3x2Node>(ExpressionNodeType.Multiply, left, right);
        }

        public static Matrix3x2Node operator *(Matrix3x2Node left, Matrix3x2Node right)
        {
            return ExpressionFunctions.Function<Matrix3x2Node>(ExpressionNodeType.Multiply, left, right);
        }

        public static BooleanNode operator ==(Matrix3x2Node left, Matrix3x2Node right)
        {
            return ExpressionFunctions.Function<BooleanNode>(ExpressionNodeType.Equals, left, right);
        }

        public static BooleanNode operator !=(Matrix3x2Node left, Matrix3x2Node right)
        {
            return ExpressionFunctions.Function<BooleanNode>(ExpressionNodeType.NotEquals, left, right);
        }

        public enum Subchannel
        {
            Channel11, Channel12,
            Channel21, Channel22,
            Channel31, Channel32,
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

        public ScalarNode Channel21
        {
            get { return GetSubchannels(Subchannel.Channel21); }
        }

        public ScalarNode Channel22
        {
            get { return GetSubchannels(Subchannel.Channel22); }
        }

        public ScalarNode Channel31
        {
            get { return GetSubchannels(Subchannel.Channel31); }
        }

        public ScalarNode Channel32
        {
            get { return GetSubchannels(Subchannel.Channel32); }
        }

        /// <summary>
        /// Create a new type by re-arranging the Matrix subchannels.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public ScalarNode GetSubchannels(Subchannel s)
        {
            return SubchannelsInternal<ScalarNode>(s.ToString());
        }

        /// <summary>
        /// Create a new type by re-arranging the Matrix subchannels.
        /// </summary>
        /// <param name="s1"></param>
        /// <param name="s2"></param>
        /// <returns></returns>
        public Vector2Node GetSubchannels(Subchannel s1, Subchannel s2)
        {
            return SubchannelsInternal<Vector2Node>(s1.ToString(), s2.ToString());
        }

        /// <summary>
        /// Create a new type by re-arranging the Matrix subchannels.
        /// </summary>
        /// <param name="s1"></param>
        /// <param name="s2"></param>
        /// <param name="s3"></param>
        /// <returns></returns>
        public Vector3Node GetSubchannels(Subchannel s1, Subchannel s2, Subchannel s3)
        {
            return SubchannelsInternal<Vector3Node>(s1.ToString(), s2.ToString(), s3.ToString());
        }

        /// <summary>
        /// Create a new type by re-arranging the Matrix subchannels.
        /// </summary>
        /// <param name="s1"></param>
        /// <param name="s2"></param>
        /// <param name="s3"></param>
        /// <param name="s4"></param>
        /// <returns></returns>
        public Vector4Node GetSubchannels(Subchannel s1, Subchannel s2, Subchannel s3, Subchannel s4)
        {
            return SubchannelsInternal<Vector4Node>(s1.ToString(), s2.ToString(), s3.ToString(), s4.ToString());
        }

        /// <summary>
        /// Create a new type by re-arranging the Matrix subchannels.
        /// </summary>
        /// <param name="s1"></param>
        /// <param name="s2"></param>
        /// <param name="s3"></param>
        /// <param name="s4"></param>
        /// <param name="s5"></param>
        /// <param name="s6"></param>
        /// <returns></returns>
        public Matrix3x2Node GetSubchannels(Subchannel s1, Subchannel s2, Subchannel s3, Subchannel s4, Subchannel s5, Subchannel s6)
        {
            return SubchannelsInternal<Matrix3x2Node>(s1.ToString(), s2.ToString(), s3.ToString(), s4.ToString(), s5.ToString(), s6.ToString());
        }

        /// <summary>
        /// Create a new type by re-arranging the Matrix subchannels.
        /// </summary>
        /// <param name="s1"></param>
        /// <param name="s2"></param>
        /// <param name="s3"></param>
        /// <param name="s4"></param>
        /// <param name="s5"></param>
        /// <param name="s6"></param>
        /// <param name="s7"></param>
        /// <param name="s8"></param>
        /// <param name="s9"></param>
        /// <param name="s10"></param>
        /// <param name="s11"></param>
        /// <param name="s12"></param>
        /// <param name="s13"></param>
        /// <param name="s14"></param>
        /// <param name="s15"></param>
        /// <param name="s16"></param>
        /// <returns></returns>
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
            return $"Matrix3x2({_value.M11},{_value.M12},{_value.M21},{_value.M22},{_value.M31},{_value.M32})";
        }

        private Matrix3x2 _value;
    }
#pragma warning restore CS0660, CS0661
}