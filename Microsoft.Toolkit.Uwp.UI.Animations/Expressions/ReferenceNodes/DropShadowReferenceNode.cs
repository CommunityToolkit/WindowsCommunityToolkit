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
    /// Class DropShadowReferenceNode. This class cannot be inherited.
    /// </summary>
    /// <seealso cref="Microsoft.Toolkit.Uwp.UI.Animations.Expressions.ReferenceNode" />
    public sealed class DropShadowReferenceNode : ReferenceNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DropShadowReferenceNode"/> class.
        /// </summary>
        /// <param name="paramName">Name of the parameter.</param>
        /// <param name="source">The source.</param>
        internal DropShadowReferenceNode(string paramName, DropShadow source = null)
            : base(paramName, source)
        {
        }

        /// <summary>
        /// Creates the target reference.
        /// </summary>
        /// <returns>DropShadowReferenceNode.</returns>
        internal static DropShadowReferenceNode CreateTargetReference()
        {
            var node = new DropShadowReferenceNode(null);
            node.NodeType = ExpressionNodeType.TargetReference;

            return node;
        }

        /// <summary>
        /// Gets the blur radius.
        /// </summary>
        /// <value>The blur radius.</value>
        public ScalarNode BlurRadius
        {
            get { return ReferenceProperty<ScalarNode>("BlurRadius"); }
        }

        /// <summary>
        /// Gets the opacity.
        /// </summary>
        /// <value>The opacity.</value>
        public ScalarNode Opacity
        {
            get { return ReferenceProperty<ScalarNode>("Opacity"); }
        }

        /// <summary>
        /// Gets the offset.
        /// </summary>
        /// <value>The offset.</value>
        public Vector3Node Offset
        {
            get { return ReferenceProperty<Vector3Node>("Offset"); }
        }

        /// <summary>
        /// Gets the color.
        /// </summary>
        /// <value>The color.</value>
        public ColorNode Color
        {
            get { return ReferenceProperty<ColorNode>("Color"); }
        }
    }
}