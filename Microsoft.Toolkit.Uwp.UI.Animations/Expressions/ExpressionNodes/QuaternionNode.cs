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
    // Ignore warning: 'QuaternionNode' defines operator == or operator != but does not override Object.Equals(object o) && Object.GetHashCode()
#pragma warning disable CS0660, CS0661
    /// <summary>
    /// Class QuaternionNode. This class cannot be inherited.
    /// </summary>
    /// <seealso cref="Microsoft.Toolkit.Uwp.UI.Animations.Expressions.ExpressionNode" />
    public sealed class QuaternionNode : ExpressionNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="QuaternionNode"/> class.
        /// </summary>
        internal QuaternionNode()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="QuaternionNode"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        internal QuaternionNode(Quaternion value)
        {
            _value = value;
            NodeType = ExpressionNodeType.ConstantValue;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="QuaternionNode"/> class.
        /// </summary>
        /// <param name="paramName">Name of the parameter.</param>
        internal QuaternionNode(string paramName)
        {
            ParamName = paramName;
            NodeType = ExpressionNodeType.ConstantParameter;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="QuaternionNode"/> class.
        /// </summary>
        /// <param name="paramName">Name of the parameter.</param>
        /// <param name="value">The value.</param>
        internal QuaternionNode(string paramName, Quaternion value)
        {
            ParamName = paramName;
            _value = value;
            NodeType = ExpressionNodeType.ConstantParameter;

            SetQuaternionParameter(paramName, value);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="Quaternion"/> to <see cref="QuaternionNode"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator QuaternionNode(Quaternion value)
        {
            return new QuaternionNode(value);
        }

        /// <summary>
        /// Implements the * operator.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static QuaternionNode operator *(QuaternionNode left, ScalarNode right)
        {
            return ExpressionFunctions.Function<QuaternionNode>(ExpressionNodeType.Multiply, left, right);
        }

        /// <summary>
        /// Implements the * operator.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static QuaternionNode operator *(QuaternionNode left, QuaternionNode right)
        {
            return ExpressionFunctions.Function<QuaternionNode>(ExpressionNodeType.Multiply, left, right);
        }

        /// <summary>
        /// Implements the / operator.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static QuaternionNode operator /(QuaternionNode left, QuaternionNode right)
        {
            return ExpressionFunctions.Function<QuaternionNode>(ExpressionNodeType.Divide, left, right);
        }

        /// <summary>
        /// Implements the == operator.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static BooleanNode operator ==(QuaternionNode left, QuaternionNode right)
        {
            return ExpressionFunctions.Function<BooleanNode>(ExpressionNodeType.Equals, left, right);
        }

        /// <summary>
        /// Implements the != operator.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static BooleanNode operator !=(QuaternionNode left, QuaternionNode right)
        {
            return ExpressionFunctions.Function<BooleanNode>(ExpressionNodeType.NotEquals, left, right);
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <returns>System.String.</returns>
        protected internal override string GetValue()
        {
            return $"Quaternion({_value.X},{_value.Y},{_value.Z},{_value.W})";
        }

        private Quaternion _value;
    }
#pragma warning restore CS0660, CS0661
}