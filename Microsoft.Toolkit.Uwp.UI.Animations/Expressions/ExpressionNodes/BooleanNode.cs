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
    public sealed class BooleanNode : ExpressionNode
    {
        internal BooleanNode()
        {
        }

        internal BooleanNode(bool value)
        {
            _value = value;
            NodeType = ExpressionNodeType.ConstantValue;
        }

        internal BooleanNode(string paramName)
        {
            ParamName = paramName;
            NodeType = ExpressionNodeType.ConstantParameter;
        }

        internal BooleanNode(string paramName, bool value)
        {
            ParamName = paramName;
            _value = value;
            NodeType = ExpressionNodeType.ConstantParameter;

            SetBooleanParameter(paramName, value);
        }

        public static implicit operator BooleanNode(bool value)
        {
            return new BooleanNode(value);
        }

        public static BooleanNode operator ==(BooleanNode left, BooleanNode right)
        {
            return ExpressionFunctions.Function<BooleanNode>(ExpressionNodeType.Equals, left, right);
        }

        public static BooleanNode operator !=(BooleanNode left, BooleanNode right)
        {
            return ExpressionFunctions.Function<BooleanNode>(ExpressionNodeType.NotEquals, left, right);
        }

        public static BooleanNode operator &(BooleanNode left, BooleanNode right)
        {
            return ExpressionFunctions.Function<BooleanNode>(ExpressionNodeType.And, left, right);
        }

        public static BooleanNode operator |(BooleanNode left, BooleanNode right)
        {
            return ExpressionFunctions.Function<BooleanNode>(ExpressionNodeType.Or, left, right);
        }

        public static BooleanNode operator !(BooleanNode value)
        {
            return ExpressionFunctions.Function<BooleanNode>(ExpressionNodeType.Not, value);
        }

        protected internal override string GetValue()
        {
            return _value ? "true" : "false";
        }

        private bool _value;
    }
#pragma warning restore CS0660, CS0661
}