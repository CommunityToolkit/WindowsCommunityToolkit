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

using Windows.UI.Composition;

namespace Microsoft.Toolkit.Uwp.UI.Animations.Expressions
{
    /// <summary>
    /// Class VisualReferenceNode. This class cannot be inherited.
    /// </summary>
    /// <seealso cref="Microsoft.Toolkit.Uwp.UI.Animations.Expressions.ReferenceNode" />
    public sealed class VisualReferenceNode : ReferenceNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VisualReferenceNode"/> class.
        /// </summary>
        /// <param name="paramName">Name of the parameter.</param>
        /// <param name="v">The v.</param>
        internal VisualReferenceNode(string paramName, Visual v = null)
            : base(paramName, v)
        {
        }

        /// <summary>
        /// Creates the target reference.
        /// </summary>
        /// <returns>VisualReferenceNode.</returns>
        internal static VisualReferenceNode CreateTargetReference()
        {
            var node = new VisualReferenceNode(null);
            node.NodeType = ExpressionNodeType.TargetReference;

            return node;
        }

        /// <summary>
        /// Gets the opacity.
        /// </summary>
        /// <value>The opacity.</value>
        public ScalarNode Opacity
        {
            get { return ReferenceProperty<ScalarNode>("Opacity"); }
        }

        public ScalarNode RotationAngle
        {
            get { return ReferenceProperty<ScalarNode>("RotationAngle"); }
        }

        public ScalarNode RotationAngleInDegrees
        {
            get { return ReferenceProperty<ScalarNode>("RotationAngleInDegrees"); }
        }

        public Vector2Node AnchorPoint
        {
            get { return ReferenceProperty<Vector2Node>("AnchorPoint"); }
        }

        public Vector2Node RelativeSize
        {
            get { return ReferenceProperty<Vector2Node>("RelativeSize"); }
        }

        public Vector2Node Size
        {
            get { return ReferenceProperty<Vector2Node>("Size"); }
        }

        public Vector3Node CenterPoint
        {
            get { return ReferenceProperty<Vector3Node>("CenterPoint"); }
        }

        public Vector3Node Offset
        {
            get { return ReferenceProperty<Vector3Node>("Offset"); }
        }

        public Vector3Node RelativeOffset
        {
            get { return ReferenceProperty<Vector3Node>("RelativeOffset"); }
        }

        public Vector3Node RotationAxis
        {
            get { return ReferenceProperty<Vector3Node>("RotationAxis"); }
        }

        public Vector3Node Scale
        {
            get { return ReferenceProperty<Vector3Node>("Scale"); }
        }

        public QuaternionNode Orientation
        {
            get { return ReferenceProperty<QuaternionNode>("Orientation"); }
        }

        public Matrix4x4Node TransformMatrix
        {
            get { return ReferenceProperty<Matrix4x4Node>("TransformMatrix"); }
        }
    }
}