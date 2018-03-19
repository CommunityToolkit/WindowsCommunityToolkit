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
    // ExpressionValues is a static class instead of a namespace to improve intellisense discoverablity and consistency with the other helper classes.

    /// <summary>
    /// Class ExpressionValues.
    /// </summary>
    public static partial class ExpressionValues
    {
        /// <summary>
        /// Refer to the current value of the property this expression is connected to.
        /// </summary>
        public static class CurrentValue
        {
            /// <summary>
            /// Create a reference to the current value of the boolean property that this expression will be connected to.
            /// </summary>
            /// <returns>BooleanNode.</returns>
            public static BooleanNode CreateBooleanCurrentValue()
            {
                return ExpressionNode.CreateValueKeyword<BooleanNode>(ValueKeywordKind.CurrentValue);
            }

            /// <summary>
            /// Create a reference to the current value of the float property that this expression will be connected to.
            /// </summary>
            /// <returns>ScalarNode.</returns>
            public static ScalarNode CreateScalarCurrentValue()
            {
                return ExpressionNode.CreateValueKeyword<ScalarNode>(ValueKeywordKind.CurrentValue);
            }

            /// <summary>
            /// Create a reference to the current value of the Vector2 property that this expression will be connected to.
            /// </summary>
            /// <returns>Vector2Node.</returns>
            public static Vector2Node CreateVector2CurrentValue()
            {
                return ExpressionNode.CreateValueKeyword<Vector2Node>(ValueKeywordKind.CurrentValue);
            }

            /// <summary>
            /// Create a reference to the current value of the Vector3 property that this expression will be connected to.
            /// </summary>
            /// <returns>Vector3Node.</returns>
            public static Vector3Node CreateVector3CurrentValue()
            {
                return ExpressionNode.CreateValueKeyword<Vector3Node>(ValueKeywordKind.CurrentValue);
            }

            /// <summary>
            /// Create a reference to the current value of the Vector4 property that this expression will be connected to.
            /// </summary>
            /// <returns>Vector4Node.</returns>
            public static Vector4Node CreateVector4CurrentValue()
            {
                return ExpressionNode.CreateValueKeyword<Vector4Node>(ValueKeywordKind.CurrentValue);
            }

            /// <summary>
            /// Create a reference to the current value of the Color property that this expression will be connected to.
            /// </summary>
            /// <returns>ColorNode.</returns>
            public static ColorNode CreateColorCurrentValue()
            {
                return ExpressionNode.CreateValueKeyword<ColorNode>(ValueKeywordKind.CurrentValue);
            }

            /// <summary>
            /// Create a reference to the current value of the Quaternion property that this expression will be connected to.
            /// </summary>
            /// <returns>QuaternionNode.</returns>
            public static QuaternionNode CreateQuaternionCurrentValue()
            {
                return ExpressionNode.CreateValueKeyword<QuaternionNode>(ValueKeywordKind.CurrentValue);
            }

            /// <summary>
            /// Create a reference to the current value of the Matrix3x2 property that this expression will be connected to.
            /// </summary>
            /// <returns>Matrix3x2Node.</returns>
            public static Matrix3x2Node CreateMatrix3x2CurrentValue()
            {
                return ExpressionNode.CreateValueKeyword<Matrix3x2Node>(ValueKeywordKind.CurrentValue);
            }

            /// <summary>
            /// Create a reference to the current value of the Matrix4x4 property that this expression will be connected to.
            /// </summary>
            /// <returns>Matrix4x4Node.</returns>
            public static Matrix4x4Node CreateMatrix4x4CurrentValue()
            {
                return ExpressionNode.CreateValueKeyword<Matrix4x4Node>(ValueKeywordKind.CurrentValue);
            }
        }
    }
}