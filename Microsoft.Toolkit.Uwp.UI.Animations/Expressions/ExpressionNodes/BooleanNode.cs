// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

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

        /// <summary>
        /// Evaluates the current value of the expression
        /// </summary>
        /// <returns>The current value of the expression</returns>
        public bool Evaluate()
        {
            switch (NodeType)
            {
                case ExpressionNodeType.ConstantValue:
                    return _value;
                case ExpressionNodeType.Equals:
                    return Equals(Children[0], Children[1]);
                case ExpressionNodeType.NotEquals:
                    return !Equals(Children[0], Children[1]);
                case ExpressionNodeType.And:
                    return (Children[0] as BooleanNode).Evaluate() && (Children[1] as BooleanNode).Evaluate();
                case ExpressionNodeType.Or:
                    return (Children[0] as BooleanNode).Evaluate() || (Children[1] as BooleanNode).Evaluate();
                case ExpressionNodeType.LessThan:
                    return (Children[0] as ScalarNode).Evaluate() < (Children[1] as ScalarNode).Evaluate();
                case ExpressionNodeType.LessThanEquals:
                    return (Children[0] as ScalarNode).Evaluate() <= (Children[1] as ScalarNode).Evaluate();
                case ExpressionNodeType.GreaterThan:
                    return (Children[0] as ScalarNode).Evaluate() > (Children[1] as ScalarNode).Evaluate();
                case ExpressionNodeType.GreaterThanEquals:
                    return (Children[0] as ScalarNode).Evaluate() >= (Children[1] as ScalarNode).Evaluate();
                case ExpressionNodeType.Not:
                    return !(Children[0] as BooleanNode).Evaluate();
                case ExpressionNodeType.ReferenceProperty:
                    var reference = (Children[0] as ReferenceNode).Reference;
                    switch (PropertyName)
                    {
                        default:
                            reference.Properties.TryGetBoolean(PropertyName, out var referencedProperty);
                            return referencedProperty;
                    }

                case ExpressionNodeType.Conditional:
                    return
                        (Children[0] as BooleanNode).Evaluate() ?
                        (Children[1] as BooleanNode).Evaluate() :
                        (Children[2] as BooleanNode).Evaluate();
                default:
                    throw new NotImplementedException();
            }

            bool Equals(ExpressionNode e1, ExpressionNode e2) => (e1, e2) switch
                {
                    (BooleanNode n1, BooleanNode n2) => n1.Evaluate() == n2.Evaluate(),
                    (ScalarNode n1, ScalarNode n2) => n1.Evaluate() == n2.Evaluate(),
                    (Vector2Node n1, Vector2Node n2) => n1.Evaluate() == n2.Evaluate(),
                    (Vector3Node n1, Vector3Node n2) => n1.Evaluate() == n2.Evaluate(),
                    (Vector4Node n1, Vector4Node n2) => n1.Evaluate() == n2.Evaluate(),
                    (ColorNode n1, ColorNode n2) => n1.Evaluate() == n2.Evaluate(),
                    (QuaternionNode n1, QuaternionNode n2) => n1.Evaluate() == n2.Evaluate(),
                    (Matrix3x2Node n1, Matrix3x2Node n2) => n1.Evaluate() == n2.Evaluate(),
                    (Matrix4x4Node n1, Matrix4x4Node n2) => n1.Evaluate() == n2.Evaluate(),
                    _ => false
                };
        }
    }
#pragma warning restore CS0660, CS0661
}