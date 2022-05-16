// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Numerics;
using Windows.UI.Composition;

namespace Microsoft.Toolkit.Uwp.UI.Animations.Expressions
{
    // Ignore warning: 'ScalarNode' defines operator == or operator != but does not override Object.Equals(object o) && Object.GetHashCode()
#pragma warning disable CS0660, CS0661
    /// <summary>
    /// Class ScalarNode. This class cannot be inherited.
    /// </summary>
    /// <seealso cref="Microsoft.Toolkit.Uwp.UI.Animations.Expressions.ExpressionNode" />
    public sealed class ScalarNode : ExpressionNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ScalarNode"/> class.
        /// </summary>
        internal ScalarNode()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScalarNode"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        internal ScalarNode(float value)
        {
            _value = value;
            NodeType = ExpressionNodeType.ConstantValue;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScalarNode"/> class.
        /// </summary>
        /// <param name="paramName">Name of the parameter.</param>
        internal ScalarNode(string paramName)
        {
            ParamName = paramName;
            NodeType = ExpressionNodeType.ConstantParameter;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScalarNode"/> class.
        /// </summary>
        /// <param name="paramName">Name of the parameter.</param>
        /// <param name="value">The value.</param>
        internal ScalarNode(string paramName, float value)
        {
            ParamName = paramName;
            _value = value;
            NodeType = ExpressionNodeType.ConstantParameter;

            SetScalarParameter(paramName, value);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="float"/> to <see cref="ScalarNode"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator ScalarNode(float value)
        {
            return new ScalarNode(value);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="int"/> to <see cref="ScalarNode"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator ScalarNode(int value)
        {
            return new ScalarNode((float)value);
        }

        /// <summary>
        /// Implements the + operator.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static ScalarNode operator +(ScalarNode left, ScalarNode right)
        {
            return ExpressionFunctions.Function<ScalarNode>(ExpressionNodeType.Add, left, right);
        }

        /// <summary>
        /// Implements the - operator.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static ScalarNode operator -(ScalarNode left, ScalarNode right)
        {
            return ExpressionFunctions.Function<ScalarNode>(ExpressionNodeType.Subtract, left, right);
        }

        /// <summary>
        /// Implements the - operator.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the operator.</returns>
        public static ScalarNode operator -(ScalarNode value)
        {
            return ExpressionFunctions.Function<ScalarNode>(ExpressionNodeType.Negate, value);
        }

        /// <summary>
        /// Implements the * operator.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static ScalarNode operator *(ScalarNode left, ScalarNode right)
        {
            return ExpressionFunctions.Function<ScalarNode>(ExpressionNodeType.Multiply, left, right);
        }

        /// <summary>
        /// Implements the * operator.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static Vector2Node operator *(ScalarNode left, Vector2Node right)
        {
            return ExpressionFunctions.Function<Vector2Node>(ExpressionNodeType.Multiply, left, right);
        }

        /// <summary>
        /// Implements the * operator.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static Vector3Node operator *(ScalarNode left, Vector3Node right)
        {
            return ExpressionFunctions.Function<Vector3Node>(ExpressionNodeType.Multiply, left, right);
        }

        /// <summary>
        /// Implements the * operator.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static Vector4Node operator *(ScalarNode left, Vector4Node right)
        {
            return ExpressionFunctions.Function<Vector4Node>(ExpressionNodeType.Multiply, left, right);
        }

        /// <summary>
        /// Implements the * operator.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static Matrix4x4Node operator *(ScalarNode left, Matrix4x4Node right)
        {
            return ExpressionFunctions.Function<Matrix4x4Node>(ExpressionNodeType.Multiply, left, right);
        }

        /// <summary>
        /// Implements the / operator.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static ScalarNode operator /(ScalarNode left, ScalarNode right)
        {
            return ExpressionFunctions.Function<ScalarNode>(ExpressionNodeType.Divide, left, right);
        }

        /// <summary>
        /// Implements the % operator.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static ScalarNode operator %(ScalarNode left, ScalarNode right)
        {
            return ExpressionFunctions.Function<ScalarNode>(ExpressionNodeType.Modulus, left, right);
        }

        /// <summary>
        /// Implements the == operator.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static BooleanNode operator ==(ScalarNode left, ScalarNode right)
        {
            return ExpressionFunctions.Function<BooleanNode>(ExpressionNodeType.Equals, left, right);
        }

        /// <summary>
        /// Implements the != operator.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static BooleanNode operator !=(ScalarNode left, ScalarNode right)
        {
            return ExpressionFunctions.Function<BooleanNode>(ExpressionNodeType.NotEquals, left, right);
        }

        /// <summary>
        /// Implements the &lt;= operator.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static BooleanNode operator <=(ScalarNode left, ScalarNode right)
        {
            return ExpressionFunctions.Function<BooleanNode>(ExpressionNodeType.LessThanEquals, left, right);
        }

        /// <summary>
        /// Implements the &lt; operator.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static BooleanNode operator <(ScalarNode left, ScalarNode right)
        {
            return ExpressionFunctions.Function<BooleanNode>(ExpressionNodeType.LessThan, left, right);
        }

        /// <summary>
        /// Implements the &gt;= operator.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static BooleanNode operator >=(ScalarNode left, ScalarNode right)
        {
            return ExpressionFunctions.Function<BooleanNode>(ExpressionNodeType.GreaterThanEquals, left, right);
        }

        /// <summary>
        /// Implements the &gt; operator.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static BooleanNode operator >(ScalarNode left, ScalarNode right)
        {
            return ExpressionFunctions.Function<BooleanNode>(ExpressionNodeType.GreaterThan, left, right);
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <returns>System.String.</returns>
        protected internal override string GetValue()
        {
            return _value.ToCompositionString();
        }

        private float _value;

        /// <summary>
        /// Evaluates the current value of the expression
        /// </summary>
        /// <returns>The current value of the expression</returns>
        public float Evaluate()
        {
            switch (NodeType)
            {
                case ExpressionNodeType.ConstantValue:
                    return _value;
                case ExpressionNodeType.ReferenceProperty:
                    var reference = (Children[0] as ReferenceNode).Reference;
                    return PropertyName switch
                    {
                        nameof(Visual.Opacity) => (reference as Visual).Opacity,
                        nameof(Visual.RotationAngle) => (reference as Visual).RotationAngle,
                        nameof(InsetClip.BottomInset) => (reference as InsetClip).BottomInset,
                        nameof(InsetClip.LeftInset) => (reference as InsetClip).LeftInset,
                        nameof(InsetClip.RightInset) => (reference as InsetClip).RightInset,
                        nameof(InsetClip.TopInset) => (reference as InsetClip).TopInset,
                        _ => GetProperty()
                    };

                    float GetProperty()
                    {
                        reference.Properties.TryGetScalar(PropertyName, out var value);
                        return value;
                    }

                case ExpressionNodeType.Negate:
                    return -(Children[0] as ScalarNode).Evaluate();
                case ExpressionNodeType.Add:
                    return (Children[0] as ScalarNode).Evaluate() + (Children[1] as ScalarNode).Evaluate();
                case ExpressionNodeType.Subtract:
                    return (Children[0] as ScalarNode).Evaluate() - (Children[1] as ScalarNode).Evaluate();
                case ExpressionNodeType.Multiply:
                    return (Children[0] as ScalarNode).Evaluate() * (Children[1] as ScalarNode).Evaluate();
                case ExpressionNodeType.Divide:
                    return (Children[0] as ScalarNode).Evaluate() / (Children[1] as ScalarNode).Evaluate();
                case ExpressionNodeType.Min:
                    return MathF.Min((Children[0] as ScalarNode).Evaluate(), (Children[1] as ScalarNode).Evaluate());
                case ExpressionNodeType.Max:
                    return MathF.Max((Children[0] as ScalarNode).Evaluate(), (Children[1] as ScalarNode).Evaluate());
                case ExpressionNodeType.Absolute:
                    return MathF.Abs((Children[0] as ScalarNode).Evaluate());
                case ExpressionNodeType.Sin:
                    return MathF.Sin((Children[0] as ScalarNode).Evaluate());
                case ExpressionNodeType.Cos:
                    return MathF.Cos((Children[0] as ScalarNode).Evaluate());
                case ExpressionNodeType.Asin:
                    return MathF.Asin((Children[0] as ScalarNode).Evaluate());
                case ExpressionNodeType.Acos:
                    return MathF.Acos((Children[0] as ScalarNode).Evaluate());
                case ExpressionNodeType.Atan:
                    return MathF.Atan((Children[0] as ScalarNode).Evaluate());
                case ExpressionNodeType.Log10:
                    return MathF.Log10((Children[0] as ScalarNode).Evaluate());
                case ExpressionNodeType.Conditional:
                    return (Children[0] as BooleanNode).Evaluate() ? (Children[1] as ScalarNode).Evaluate() : (Children[2] as ScalarNode).Evaluate();
                case ExpressionNodeType.Distance:
                    return Vector2.Distance((Children[0] as Vector2Node).Evaluate(), (Children[1] as Vector2Node).Evaluate());
                case ExpressionNodeType.Lerp:
                {
                    var start = (Children[0] as ScalarNode).Evaluate();
                    var end = (Children[1] as ScalarNode).Evaluate();
                    var progress = (Children[2] as ScalarNode).Evaluate();
                    return start + (progress * (end - start));
                }

                case ExpressionNodeType.Swizzle:
                    return Children[0] switch
                    {
                        ScalarNode n => n.Evaluate(),
                        Vector2Node n => Subchannels[0] switch
                        {
                            "X" => n.Evaluate().X,
                            _ => n.Evaluate().Y,
                        },
                        Vector3Node n => Subchannels[0] switch
                        {
                            "X" => n.Evaluate().X,
                            "Y" => n.Evaluate().Y,
                            _ => n.Evaluate().Z,
                        },
                        Vector4Node n => Subchannels[0] switch
                        {
                            "X" => n.Evaluate().X,
                            "Y" => n.Evaluate().Y,
                            "Z" => n.Evaluate().Z,
                            _ => n.Evaluate().W,
                        },
                        _ => throw new NotImplementedException()
                    };
                default:
                    throw new NotImplementedException();
            }
        }
    }
#pragma warning restore CS0660, CS0661
}