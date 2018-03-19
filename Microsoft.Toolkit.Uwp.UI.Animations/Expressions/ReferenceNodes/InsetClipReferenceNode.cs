﻿// ******************************************************************
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

using Windows.UI.Composition;

namespace Microsoft.Toolkit.Uwp.UI.Animations.Expressions
{
    /// <summary>
    /// Class InsetClipReferenceNode. This class cannot be inherited.
    /// </summary>
    /// <seealso cref="Microsoft.Toolkit.Uwp.UI.Animations.Expressions.ReferenceNode" />
    public sealed class InsetClipReferenceNode : ReferenceNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InsetClipReferenceNode"/> class.
        /// </summary>
        /// <param name="paramName">Name of the parameter.</param>
        /// <param name="ic">The ic.</param>
        internal InsetClipReferenceNode(string paramName, InsetClip ic = null)
            : base(paramName, ic)
        {
        }

        /// <summary>
        /// Creates the target reference.
        /// </summary>
        /// <returns>InsetClipReferenceNode.</returns>
        internal static InsetClipReferenceNode CreateTargetReference()
        {
            var node = new InsetClipReferenceNode(null);
            node.NodeType = ExpressionNodeType.TargetReference;

            return node;
        }

        /// <summary>
        /// Gets the bottom inset.
        /// </summary>
        /// <value>The bottom inset.</value>
        public ScalarNode BottomInset
        {
            get { return ReferenceProperty<ScalarNode>("BottomInset"); }
        }

        /// <summary>
        /// Gets the left inset.
        /// </summary>
        /// <value>The left inset.</value>
        public ScalarNode LeftInset
        {
            get { return ReferenceProperty<ScalarNode>("LeftInset"); }
        }

        /// <summary>
        /// Gets the right inset.
        /// </summary>
        /// <value>The right inset.</value>
        public ScalarNode RightInset
        {
            get { return ReferenceProperty<ScalarNode>("RightInset"); }
        }

        /// <summary>
        /// Gets the top inset.
        /// </summary>
        /// <value>The top inset.</value>
        public ScalarNode TopInset
        {
            get { return ReferenceProperty<ScalarNode>("TopInset"); }
        }

        /// <summary>
        /// Gets the rotation angle.
        /// </summary>
        /// <value>The rotation angle.</value>
        public ScalarNode RotationAngle
        {
            get { return ReferenceProperty<ScalarNode>("RotationAngle"); }
        }

        /// <summary>
        /// Gets the rotation angle in degrees.
        /// </summary>
        /// <value>The rotation angle in degrees.</value>
        public ScalarNode RotationAngleInDegrees
        {
            get { return ReferenceProperty<ScalarNode>("RotationAngleInDegrees"); }
        }

        /// <summary>
        /// Gets the anchor point.
        /// </summary>
        /// <value>The anchor point.</value>
        public Vector2Node AnchorPoint
        {
            get { return ReferenceProperty<Vector2Node>("AnchorPoint"); }
        }

        /// <summary>
        /// Gets the center point.
        /// </summary>
        /// <value>The center point.</value>
        public Vector2Node CenterPoint
        {
            get { return ReferenceProperty<Vector2Node>("CenterPoint"); }
        }

        /// <summary>
        /// Gets the offset.
        /// </summary>
        /// <value>The offset.</value>
        public Vector2Node Offset
        {
            get { return ReferenceProperty<Vector2Node>("Offset"); }
        }

        /// <summary>
        /// Gets the scale.
        /// </summary>
        /// <value>The scale.</value>
        public Vector2Node Scale
        {
            get { return ReferenceProperty<Vector2Node>("Scale"); }
        }

        /// <summary>
        /// Gets the transform matrix.
        /// </summary>
        /// <value>The transform matrix.</value>
        public Matrix3x2Node TransformMatrix
        {
            get { return ReferenceProperty<Matrix3x2Node>("TransformMatrix"); }
        }
    }
}