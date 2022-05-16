// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Numerics;
using Windows.UI.Composition;

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
            return $"Quaternion({_value.X.ToCompositionString()},{_value.Y.ToCompositionString()},{_value.Z.ToCompositionString()},{_value.W.ToCompositionString()})";
        }

        private Quaternion _value;

        /// <summary>
        /// Evaluates the current value of the expression
        /// </summary>
        /// <returns>The current value of the expression</returns>
        public Quaternion Evaluate()
        {
            switch (NodeType)
            {
                case ExpressionNodeType.ConstantValue:
                    return _value;
                case ExpressionNodeType.ConstantParameter:
                    throw new NotImplementedException();
                case ExpressionNodeType.ReferenceProperty:
                    var reference = (Children[0] as ReferenceNode).Reference;
                    return PropertyName switch
                    {
                        nameof(Visual.Orientation) => (reference as Visual).Orientation,
                        _ => GetProperty()
                    };

                    Quaternion GetProperty()
                    {
                        reference.Properties.TryGetQuaternion(PropertyName, out var value);
                        return value;
                    }

                case ExpressionNodeType.Add:
                    return
                        (Children[0] as QuaternionNode).Evaluate() +
                        (Children[1] as QuaternionNode).Evaluate();
                case ExpressionNodeType.Subtract:
                    return
                        (Children[0] as QuaternionNode).Evaluate() -
                        (Children[1] as QuaternionNode).Evaluate();
                case ExpressionNodeType.Negate:
                    return
                        -(Children[0] as QuaternionNode).Evaluate();
                case ExpressionNodeType.Multiply:
                    return (Children[0], Children[1]) switch
                    {
                        (QuaternionNode v1, QuaternionNode v2) => v1.Evaluate() * v2.Evaluate(),
                        (QuaternionNode v1, ScalarNode s2) => v1.Evaluate() * s2.Evaluate(),
                        (ScalarNode s1, QuaternionNode v2) => v2.Evaluate() * s1.Evaluate(),
                        _ => throw new NotImplementedException()
                    };
                case ExpressionNodeType.Divide:
                    return
                        (Children[0] as QuaternionNode).Evaluate() /
                        (Children[1] as QuaternionNode).Evaluate();
                case ExpressionNodeType.QuaternionFromAxisAngle:
                    return Quaternion.CreateFromAxisAngle((Children[0] as Vector3Node).Evaluate(), (Children[1] as ScalarNode).Evaluate());
                case ExpressionNodeType.Conditional:
                    return
                        (Children[0] as BooleanNode).Evaluate() ?
                        (Children[1] as QuaternionNode).Evaluate() :
                        (Children[2] as QuaternionNode).Evaluate();
                case ExpressionNodeType.Swizzle:
                    return new Quaternion(this.EvaluateSubchannel(Subchannels[0]), this.EvaluateSubchannel(Subchannels[1]), this.EvaluateSubchannel(Subchannels[2]), this.EvaluateSubchannel(Subchannels[4]));
                default:
                    throw new NotImplementedException();
            }
        }
    }
#pragma warning restore CS0660, CS0661
}