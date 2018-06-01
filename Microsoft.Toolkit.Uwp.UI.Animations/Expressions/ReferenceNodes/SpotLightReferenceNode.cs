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
    /// Class SpotLightReferenceNode. This class cannot be inherited.
    /// </summary>
    /// <seealso cref="Microsoft.Toolkit.Uwp.UI.Animations.Expressions.ReferenceNode" />
    public sealed class SpotLightReferenceNode : ReferenceNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SpotLightReferenceNode"/> class.
        /// </summary>
        /// <param name="paramName">Name of the parameter.</param>
        /// <param name="light">The light.</param>
        internal SpotLightReferenceNode(string paramName, SpotLight light = null)
            : base(paramName, light)
        {
        }

        /// <summary>
        /// Creates the target reference.
        /// </summary>
        /// <returns>SpotLightReferenceNode.</returns>
        internal static SpotLightReferenceNode CreateTargetReference()
        {
            var node = new SpotLightReferenceNode(null);
            node.NodeType = ExpressionNodeType.TargetReference;

            return node;
        }

        /// <summary>
        /// Gets the constant attenuation.
        /// </summary>
        /// <value>The constant attenuation.</value>
        public ScalarNode ConstantAttenuation
        {
            get { return ReferenceProperty<ScalarNode>("ConstantAttenuation"); }
        }

        /// <summary>
        /// Gets the linear attenuation.
        /// </summary>
        /// <value>The linear attenuation.</value>
        public ScalarNode LinearAttenuation
        {
            get { return ReferenceProperty<ScalarNode>("LinearAttenuation"); }
        }

        /// <summary>
        /// Gets the quadratic attentuation.
        /// </summary>
        /// <value>The quadratic attentuation.</value>
        public ScalarNode QuadraticAttentuation
        {
            get { return ReferenceProperty<ScalarNode>("QuadraticAttentuation"); }
        }

        /// <summary>
        /// Gets the inner cone angle.
        /// </summary>
        /// <value>The inner cone angle.</value>
        public ScalarNode InnerConeAngle
        {
            get { return ReferenceProperty<ScalarNode>("InnerConeAngle"); }
        }

        /// <summary>
        /// Gets the inner cone angle in degrees.
        /// </summary>
        /// <value>The inner cone angle in degrees.</value>
        public ScalarNode InnerConeAngleInDegrees
        {
            get { return ReferenceProperty<ScalarNode>("InnerConeAngleInDegrees"); }
        }

        /// <summary>
        /// Gets the outer cone angle.
        /// </summary>
        /// <value>The outer cone angle.</value>
        public ScalarNode OuterConeAngle
        {
            get { return ReferenceProperty<ScalarNode>("OuterConeAngle"); }
        }

        /// <summary>
        /// Gets the outer cone angle in degrees.
        /// </summary>
        /// <value>The outer cone angle in degrees.</value>
        public ScalarNode OuterConeAngleInDegrees
        {
            get { return ReferenceProperty<ScalarNode>("OuterConeAngleInDegrees"); }
        }

        /// <summary>
        /// Gets the color.
        /// </summary>
        /// <value>The color.</value>
        public ColorNode Color
        {
            get { return ReferenceProperty<ColorNode>("Color"); }
        }

        /// <summary>
        /// Gets the color of the inner cone.
        /// </summary>
        /// <value>The color of the inner cone.</value>
        public ColorNode InnerConeColor
        {
            get { return ReferenceProperty<ColorNode>("InnerConeColor"); }
        }

        /// <summary>
        /// Gets the color of the outer cone.
        /// </summary>
        /// <value>The color of the outer cone.</value>
        public ColorNode OuterConeColor
        {
            get { return ReferenceProperty<ColorNode>("OuterConeColor"); }
        }

        /// <summary>
        /// Gets the direction.
        /// </summary>
        /// <value>The direction.</value>
        public Vector3Node Direction
        {
            get { return ReferenceProperty<Vector3Node>("Direction"); }
        }

        /// <summary>
        /// Gets the offset.
        /// </summary>
        /// <value>The offset.</value>
        public Vector3Node Offset
        {
            get { return ReferenceProperty<Vector3Node>("Offset"); }
        }
    }
}