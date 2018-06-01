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

namespace Microsoft.Toolkit.Uwp.UI.Animations.Expressions
{
    // Ignore warning: 'BooleanNode' defines operator == or operator != but does not override Object.Equals(object o) && Object.GetHashCode()
#pragma warning disable CS0660, CS0661
    /// <summary>
    /// Class BooleanNode. This class cannot be inherited.
    /// </summary>
    /// <seealso cref="Microsoft.Toolkit.Uwp.UI.Animations.Expressions.ExpressionNode" />
    public sealed class BooleanNode : ExpressionNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BooleanNode"/> class.
        /// </summary>
        internal BooleanNode()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BooleanNode"/> class.
        /// </summary>
        /// <param name="value">if set to <c>true</c> [value].</param>
        internal BooleanNode(bool value)
        {
            _value = value;
            NodeType = ExpressionNodeType.ConstantValue;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BooleanNode"/> class.
        /// </summary>
        /// <param name="paramName">Name of the parameter.</param>
        internal BooleanNode(string paramName)
        {
            ParamName = paramName;
            NodeType = ExpressionNodeType.ConstantParameter;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BooleanNode"/> class.
        /// </summary>
        /// <param name="paramName">Name of the parameter.</param>
        /// <param name="value">if set to <c>true</c> [value].</param>
        internal BooleanNode(string paramName, bool value)
        {
            ParamName = paramName;
            _value = value;
            NodeType = ExpressionNodeType.ConstantParameter;

            SetBooleanParameter(paramName, value);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="bool"/> to <see cref="BooleanNode"/>.
        /// </summary>
        /// <param name="value">if set to <c>true</c> [value].</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator BooleanNode(bool value)
        {
            return new BooleanNode(value);
        }

        /// <summary>
        /// Implements the == operator.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static BooleanNode operator ==(BooleanNode left, BooleanNode right)
        {
            return ExpressionFunctions.Function<BooleanNode>(ExpressionNodeType.Equals, left, right);
        }

        /// <summary>
        /// Implements the != operator.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static BooleanNode operator !=(BooleanNode left, BooleanNode right)
        {
            return ExpressionFunctions.Function<BooleanNode>(ExpressionNodeType.NotEquals, left, right);
        }

        /// <summary>
        /// Implements the &amp; operator.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static BooleanNode operator &(BooleanNode left, BooleanNode right)
        {
            return ExpressionFunctions.Function<BooleanNode>(ExpressionNodeType.And, left, right);
        }

        /// <summary>
        /// Implements the | operator.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static BooleanNode operator |(BooleanNode left, BooleanNode right)
        {
            return ExpressionFunctions.Function<BooleanNode>(ExpressionNodeType.Or, left, right);
        }

        /// <summary>
        /// Implements the ! operator.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the operator.</returns>
        public static BooleanNode operator !(BooleanNode value)
        {
            return ExpressionFunctions.Function<BooleanNode>(ExpressionNodeType.Not, value);
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <returns>System.String.</returns>
        protected internal override string GetValue()
        {
            return _value ? "true" : "false";
        }

        private bool _value;
    }
#pragma warning restore CS0660, CS0661
}